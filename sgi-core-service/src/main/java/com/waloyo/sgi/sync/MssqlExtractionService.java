package com.waloyo.sgi.sync;

import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import reactor.core.publisher.Flux;
import reactor.core.scheduler.Schedulers;

import java.sql.*;
import java.time.OffsetDateTime;
import java.time.ZoneOffset;
import java.util.*;

@Service
@Slf4j
public class MssqlExtractionService {

    private final SgiSyncProperties syncProperties;

    public MssqlExtractionService(SgiSyncProperties syncProperties) {
        this.syncProperties = syncProperties;
    }

    public Flux<RawRecord> extractIncremental(String dbName, String tableName, OffsetDateTime watermark) {
        return Flux.<RawRecord>create(sink -> {
            SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
            String jdbcUrl = mssqlConfig.buildJdbcUrl(dbName);

            log.info("[ETL-EXTRACT] Iniciando extraccion de {}.{} con watermark: {}", dbName, tableName, watermark);

            if (mssqlConfig.getUser() == null || mssqlConfig.getUser().trim().isEmpty()) {
                log.warn("[ETL-EXTRACT] Credenciales de MS SQL Server no configuradas (MSSQL_USER vacia). Omitiendo extraccion de {}.{}", dbName, tableName);
                sink.complete();
                return;
            }

            try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword())) {
                boolean hasModCol = checkColumnExists(conn, tableName, "FechaModificacion");
                boolean hasCreCol = checkColumnExists(conn, tableName, "FechaCreacion");
                boolean hasRegCol = checkColumnExists(conn, tableName, "FechaRegistro");

                String query;
                if (watermark != null && (hasModCol || hasCreCol || hasRegCol)) {
                    List<String> conditions = new ArrayList<>();
                    if (hasModCol) conditions.add("FechaModificacion > ?");
                    if (hasCreCol) conditions.add("FechaCreacion > ?");
                    if (hasRegCol) conditions.add("FechaRegistro > ?");
                    query = String.format("SELECT * FROM %s WHERE %s ORDER BY 1 ASC", tableName, String.join(" OR ", conditions));
                } else {
                    query = String.format("SELECT * FROM %s ORDER BY 1 ASC", tableName);
                }

                log.debug("[ETL-EXTRACT] Query SQL ejecutada: {}", query);

                try (PreparedStatement stmt = conn.prepareStatement(query)) {
                    if (watermark != null && (hasModCol || hasCreCol || hasRegCol)) {
                        Timestamp ts = Timestamp.from(watermark.toInstant());
                        int paramIdx = 1;
                        if (hasModCol) stmt.setTimestamp(paramIdx++, ts);
                        if (hasCreCol) stmt.setTimestamp(paramIdx++, ts);
                        if (hasRegCol) stmt.setTimestamp(paramIdx++, ts);
                    }

                    try (ResultSet rs = stmt.executeQuery()) {
                        ResultSetMetaData meta = rs.getMetaData();
                        int colCount = meta.getColumnCount();
                        long count = 0;

                        while (rs.next()) {
                            Map<String, Object> row = new HashMap<>();
                            String pkValue = null;
                            OffsetDateTime recordModTime = null;

                            for (int i = 1; i <= colCount; i++) {
                                String colName = meta.getColumnName(i);
                                Object val = rs.getObject(i);
                                row.put(colName, val);

                                if (i == 1 && val != null) {
                                    pkValue = String.valueOf(val);
                                }

                                if (val instanceof Timestamp) {
                                    OffsetDateTime odt = ((Timestamp) val).toInstant().atOffset(ZoneOffset.UTC);
                                    if (recordModTime == null || odt.isAfter(recordModTime)) {
                                        recordModTime = odt;
                                    }
                                }
                            }

                            RawRecord raw = RawRecord.builder()
                                .sourceDb(dbName)
                                .sourceTable(tableName)
                                .primaryKey(pkValue)
                                .data(row)
                                .modifiedAt(recordModTime)
                                .build();

                            sink.next(raw);
                            count++;
                        }

                        log.info("[ETL-EXTRACT] Extraidos exitosamente {} registros de {}.{}", count, dbName, tableName);
                        sink.complete();
                    }
                }
            } catch (Exception e) {
                log.error("[ETL-EXTRACT] Error extrayendo datos de {}.{}: {}", dbName, tableName, e.getMessage());
                sink.error(e);
            }
        }).subscribeOn(Schedulers.boundedElastic());
    }

    private boolean checkColumnExists(Connection conn, String tableName, String colName) {
        try (PreparedStatement stmt = conn.prepareStatement(
            "SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ? AND COLUMN_NAME = ?"
        )) {
            stmt.setString(1, tableName);
            stmt.setString(2, colName);
            try (ResultSet rs = stmt.executeQuery()) {
                return rs.next();
            }
        } catch (Exception e) {
            return false;
        }
    }
}
