package com.waloyo.sgi.sync;

import lombok.extern.slf4j.Slf4j;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import reactor.core.publisher.Mono;
import reactor.core.scheduler.Schedulers;

import java.sql.Timestamp;
import java.time.OffsetDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@Service
@Slf4j
public class RawMirrorService {

    private final JdbcTemplate jdbcTemplate;

    public RawMirrorService(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    public Mono<Void> ensureRawSchemaAndTable(String sourceDb, String sourceTable, Map<String, Object> sampleRow) {
        return Mono.fromRunnable(() -> {
            String sanitizedTable = sanitizeTableName(sourceDb, sourceTable);
            jdbcTemplate.execute("CREATE SCHEMA IF NOT EXISTS sgi_raw;");

            StringBuilder createSql = new StringBuilder();
            createSql.append(String.format("CREATE TABLE IF NOT EXISTS sgi_raw.%s (", sanitizedTable));
            createSql.append("_raw_id UUID PRIMARY KEY DEFAULT gen_random_uuid(), ");
            createSql.append("_raw_source_db VARCHAR(100), ");
            createSql.append("_raw_source_pk VARCHAR(100), ");
            createSql.append("_raw_synced_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP");

            for (String colName : sampleRow.keySet()) {
                String safeCol = colName.replaceAll("[^a-zA-Z0-9_]", "_").toLowerCase();
                createSql.append(String.format(", %s TEXT", safeCol));
            }
            createSql.append(");");

            jdbcTemplate.execute(createSql.toString());
            log.debug("📁 [RAW-MIRROR] Certificada tabla espejo: sgi_raw.{}", sanitizedTable);
        }).subscribeOn(Schedulers.boundedElastic()).then();
    }

    public Mono<Void> insertRawRecord(RawRecord record) {
        return Mono.fromRunnable(() -> {
            String sanitizedTable = sanitizeTableName(record.getSourceDb(), record.getSourceTable());
            Map<String, Object> row = record.getData();

            List<String> cols = new ArrayList<>();
            List<String> placeholders = new ArrayList<>();
            List<Object> values = new ArrayList<>();

            cols.add("_raw_source_db");
            placeholders.add("?");
            values.add(record.getSourceDb());

            cols.add("_raw_source_pk");
            placeholders.add("?");
            values.add(record.getPrimaryKey());

            for (Map.Entry<String, Object> entry : row.entrySet()) {
                String safeCol = entry.getKey().replaceAll("[^a-zA-Z0-9_]", "_").toLowerCase();
                cols.add(safeCol);
                placeholders.add("?");
                values.add(entry.getValue() != null ? String.valueOf(entry.getValue()) : null);
            }

            String insertSql = String.format(
                "INSERT INTO sgi_raw.%s (%s) VALUES (%s)",
                sanitizedTable,
                String.join(", ", cols),
                String.join(", ", placeholders)
            );

            jdbcTemplate.update(insertSql, values.toArray());
        }).subscribeOn(Schedulers.boundedElastic()).then();
    }

    private String sanitizeTableName(String sourceDb, String sourceTable) {
        String prefix = sourceDb.toLowerCase().contains("consultor") ? "consultor" : "agenda";
        String cleanTable = sourceTable.replaceAll("[^a-zA-Z0-9_]", "_").toLowerCase();
        return String.format("%s_%s", prefix, cleanTable);
    }
}
