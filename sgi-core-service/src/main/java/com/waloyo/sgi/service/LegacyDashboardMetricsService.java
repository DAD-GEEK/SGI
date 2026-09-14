package com.waloyo.sgi.service;

import com.waloyo.sgi.dto.AuditoriaDashboardDTO;
import com.waloyo.sgi.dto.DashboardKpisDTO;
import com.waloyo.sgi.sync.SgiSyncProperties;
import lombok.extern.slf4j.Slf4j;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.*;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

@Service
@Slf4j
public class LegacyDashboardMetricsService {

    private final SgiSyncProperties syncProperties;
    private final JdbcTemplate jdbcTemplate;

    private static final DateTimeFormatter DISPLAY_DATE = DateTimeFormatter.ofPattern("d 'de' MMMM", new Locale("es", "CO"));

    public LegacyDashboardMetricsService(SgiSyncProperties syncProperties, JdbcTemplate jdbcTemplate) {
        this.syncProperties = syncProperties;
        this.jdbcTemplate = jdbcTemplate;
    }

    /**
     * Obtiene los KPIs de alto valor desde Agenda y Consultor.
     */
    public DashboardKpisDTO obtenerKpis(String email, Boolean soloMias) {
        boolean filtrarPorUsuario = Boolean.TRUE.equals(soloMias) || (!isAdminUser(email) && email != null && !email.isBlank());

        DashboardKpisDTO kpis = new DashboardKpisDTO();
        consultarMetricasAgenda(kpis, email, filtrarPorUsuario);
        consultarMetricasConsultor(kpis, email, filtrarPorUsuario);

        // Calcular porcentaje de ejecucion de horas del mes
        if (kpis.getHorasContratadasMes() > 0) {
            double pct = (kpis.getHorasEjecutadasMes() / kpis.getHorasContratadasMes()) * 100.0;
            kpis.setPorcentajeEjecucionHoras(Math.round(pct * 10.0) / 10.0);
        } else if (kpis.getHorasEjecutadasMes() > 0) {
            kpis.setPorcentajeEjecucionHoras(100.0);
        } else {
            kpis.setPorcentajeEjecucionHoras(0.0);
        }

        return kpis;
    }

    /**
     * Consulta las ultimas auditorias para la tabla principal del Dashboard.
     * Para asesores filtra por sus auditorias asignadas; para admin muestra todas.
     */
    public List<AuditoriaDashboardDTO> obtenerAuditoriasRecientes(String email, int limit, Boolean soloMias) {
        int safeLimit = (limit <= 0 || limit > 50) ? 6 : limit;
        boolean filtrarPorUsuario = Boolean.TRUE.equals(soloMias) || (!isAdminUser(email) && email != null && !email.isBlank());
        List<AuditoriaDashboardDTO> lista = new ArrayList<>();

        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        if (mssqlConfig.getUser() == null || mssqlConfig.getUser().trim().isEmpty()) {
            return lista;
        }

        String dbName = mssqlConfig.getDbConsultor() != null ? mssqlConfig.getDbConsultor() : "gestioni_consultorNet";
        String jdbcUrl = mssqlConfig.buildJdbcUrl(dbName);

        StringBuilder sql = new StringBuilder();
        sql.append("SELECT TOP (?) ");
        sql.append("    a.IntAuditoriaID, ");
        sql.append("    ISNULL(a.StrCodigo, 'AUD') AS Codigo, ");
        sql.append("    ISNULL(tc.StrNombre, 'Empresa SGI') AS ClienteNombre, ");
        sql.append("    a.DatFechaInicial, ");
        sql.append("    a.DatFechaFinal, ");
        sql.append("    ISNULL(a.BitEstado, 1) AS BitEstado, ");
        sql.append("    ISNULL(a.BitFirma, 0) AS BitFirma, ");
        sql.append("    ISNULL(u.UserName, 'Auditor Lider') AS AuditorNombre, ");
        sql.append("    (SELECT TOP 1 ISNULL(n.StrDescripcion, n.StrCodigo) FROM dbo.Auditorias_Normas an ");
        sql.append("     JOIN dbo.Normas n ON an.IntNormaID = n.IntNormaID WHERE an.IntAuditoriaID = a.IntAuditoriaID) AS NormaDescripcion ");
        sql.append("FROM dbo.Auditorias a ");
        sql.append("LEFT JOIN dbo.Terceros_Clientes tc ON a.IntTerceroClienteID = tc.IntTerceroClienteID ");
        sql.append("LEFT JOIN dbo.AspNetUsers u ON a.StrUsuarioID = u.Id ");

        if (filtrarPorUsuario && email != null && !email.isBlank()) {
            sql.append("WHERE (LOWER(u.Email) = LOWER(?) OR LOWER(u.UserName) = LOWER(?) OR LOWER(a.StrUsuarioID) = LOWER(?)) ");
        }
        sql.append("ORDER BY a.DatFechaInicial DESC");

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword());
             PreparedStatement stmt = conn.prepareStatement(sql.toString())) {

            stmt.setInt(1, safeLimit);
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                String term = email.trim();
                stmt.setString(2, term);
                stmt.setString(3, term);
                stmt.setString(4, term);
            }

            try (ResultSet rs = stmt.executeQuery()) {
                while (rs.next()) {
                    Timestamp tsInicio = rs.getTimestamp("DatFechaInicial");
                    Timestamp tsFin = rs.getTimestamp("DatFechaFinal");
                    boolean estado = rs.getBoolean("BitEstado");
                    boolean firma = rs.getBoolean("BitFirma");

                    String estadoTexto;
                    if (!estado) {
                        estadoTexto = "Cerrada";
                    } else if (!firma) {
                        estadoTexto = "Pendiente Firma";
                    } else {
                        estadoTexto = "En Ejecucion";
                    }

                    String fechaInicioStr = tsInicio != null ? tsInicio.toLocalDateTime().format(DISPLAY_DATE) : "";
                    String fechaFinStr = tsFin != null ? tsFin.toLocalDateTime().format(DISPLAY_DATE) : "";

                    AuditoriaDashboardDTO item = AuditoriaDashboardDTO.builder()
                            .id(String.valueOf(rs.getInt("IntAuditoriaID")))
                            .codigo(rs.getString("Codigo"))
                            .cliente(rs.getString("ClienteNombre"))
                            .norma(rs.getString("NormaDescripcion") != null ? rs.getString("NormaDescripcion") : "SG-SST / ISO")
                            .estado(estadoTexto)
                            .auditor(rs.getString("AuditorNombre"))
                            .fechaInicio(fechaInicioStr)
                            .fechaFin(fechaFinStr)
                            .firmada(firma)
                            .build();

                    lista.add(item);
                }
            }
        } catch (Exception e) {
            log.warn("[SGI-DASHBOARD-METRICS] No se pudieron consultar auditorias en MSSQL: {}", e.getMessage());
        }

        // Si el asesor no tiene auditorias creadas aun, traer las ultimas generales como referencia
        if (lista.isEmpty() && filtrarPorUsuario) {
            return obtenerAuditoriasRecientes(email, limit, false);
        }

        return lista;
    }

    private void consultarMetricasAgenda(DashboardKpisDTO kpis, String email, boolean filtrarPorUsuario) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        if (mssqlConfig.getUser() == null || mssqlConfig.getUser().trim().isEmpty()) {
            return;
        }

        String dbName = mssqlConfig.getDbAgenda() != null ? mssqlConfig.getDbAgenda() : "gestioni_datosNet";
        String jdbcUrl = mssqlConfig.buildJdbcUrl(dbName);

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword())) {
            // 1. Clientes asignados a cargo del consultor (o global si es admin)
            StringBuilder sqlClientes = new StringBuilder();
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlClientes.append("SELECT COUNT(DISTINCT c.IntClienteID) AS TotalClientes ");
                sqlClientes.append("FROM dbo.Contratos c ");
                sqlClientes.append("JOIN dbo.Contratos_Usuarios cu ON c.IntContratoID = cu.IntContratoID ");
                sqlClientes.append("JOIN dbo.Usuarios u ON cu.IntUsuarioID = u.IntUsuarioID ");
                sqlClientes.append("WHERE (c.OpcEstado IS NULL OR c.OpcEstado = 1) ");
                sqlClientes.append("  AND LOWER(u.StrEmail) = LOWER(?) ");
            } else {
                sqlClientes.append("SELECT COUNT(DISTINCT IntClienteID) AS TotalClientes ");
                sqlClientes.append("FROM dbo.Contratos WHERE (OpcEstado IS NULL OR OpcEstado = 1)");
            }

            try (PreparedStatement stmt = conn.prepareStatement(sqlClientes.toString())) {
                if (filtrarPorUsuario && email != null && !email.isBlank()) {
                    stmt.setString(1, email.trim());
                }
                try (ResultSet rs = stmt.executeQuery()) {
                    if (rs.next()) {
                        kpis.setClientesAsignados(rs.getLong("TotalClientes"));
                    }
                }
            }

            // 2. Horas de asesoria ejecutadas en el mes actual
            StringBuilder sqlHorasEjecutadas = new StringBuilder();
            sqlHorasEjecutadas.append("SELECT SUM(TRY_CAST(REPLACE(a.StrHoras, ',', '.') AS FLOAT)) AS TotalHoras ");
            sqlHorasEjecutadas.append("FROM dbo.Agenda a ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlHorasEjecutadas.append("JOIN dbo.Usuarios u ON a.IntUsuarioID = u.IntUsuarioID ");
            }
            sqlHorasEjecutadas.append("WHERE (a.OpcCancelada IS NULL OR a.OpcCancelada = 0) ");
            sqlHorasEjecutadas.append("  AND MONTH(a.DatFechaInicial) = MONTH(GETDATE()) ");
            sqlHorasEjecutadas.append("  AND YEAR(a.DatFechaInicial) = YEAR(GETDATE()) ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlHorasEjecutadas.append("  AND LOWER(u.StrEmail) = LOWER(?) ");
            }

            try (PreparedStatement stmt = conn.prepareStatement(sqlHorasEjecutadas.toString())) {
                if (filtrarPorUsuario && email != null && !email.isBlank()) {
                    stmt.setString(1, email.trim());
                }
                try (ResultSet rs = stmt.executeQuery()) {
                    if (rs.next()) {
                        kpis.setHorasEjecutadasMes(Math.round(rs.getDouble("TotalHoras") * 10.0) / 10.0);
                    }
                }
            }

            // 3. Horas contratadas del mes
            StringBuilder sqlHorasContratadas = new StringBuilder();
            sqlHorasContratadas.append("SELECT SUM(c.IntHoras) AS TotalContratadas ");
            sqlHorasContratadas.append("FROM dbo.Contratos c ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlHorasContratadas.append("JOIN dbo.Contratos_Usuarios cu ON c.IntContratoID = cu.IntContratoID ");
                sqlHorasContratadas.append("JOIN dbo.Usuarios u ON cu.IntUsuarioID = u.IntUsuarioID ");
            }
            sqlHorasContratadas.append("WHERE (c.OpcEstado IS NULL OR c.OpcEstado = 1) ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlHorasContratadas.append("  AND LOWER(u.StrEmail) = LOWER(?) ");
            }

            try (PreparedStatement stmt = conn.prepareStatement(sqlHorasContratadas.toString())) {
                if (filtrarPorUsuario && email != null && !email.isBlank()) {
                    stmt.setString(1, email.trim());
                }
                try (ResultSet rs = stmt.executeQuery()) {
                    if (rs.next()) {
                        kpis.setHorasContratadasMes(rs.getDouble("TotalContratadas"));
                    }
                }
            }

            // 4. Compromisos derivados de Actas (dbo.ActividadesActa)
            StringBuilder sqlCompromisos = new StringBuilder();
            sqlCompromisos.append("SELECT ");
            sqlCompromisos.append("    COUNT(*) AS Pendientes, ");
            sqlCompromisos.append("    SUM(CASE WHEN aa.DatFecha < CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END) AS Vencidos ");
            sqlCompromisos.append("FROM dbo.ActividadesActa aa ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlCompromisos.append("JOIN dbo.Actas act ON aa.IntActaID = act.IntActaID ");
                sqlCompromisos.append("JOIN dbo.Agenda ag ON act.IntAgendaID = ag.IntAgendaID ");
                sqlCompromisos.append("JOIN dbo.Usuarios u ON ag.IntUsuarioID = u.IntUsuarioID ");
            }
            sqlCompromisos.append("WHERE (aa.OpcEjecuta IS NULL OR aa.OpcEjecuta = 0) ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlCompromisos.append("  AND LOWER(u.StrEmail) = LOWER(?) ");
            }

            try (PreparedStatement stmt = conn.prepareStatement(sqlCompromisos.toString())) {
                if (filtrarPorUsuario && email != null && !email.isBlank()) {
                    stmt.setString(1, email.trim());
                }
                try (ResultSet rs = stmt.executeQuery()) {
                    if (rs.next()) {
                        kpis.setCompromisosPendientes(rs.getLong("Pendientes"));
                        kpis.setCompromisosVencidos(rs.getLong("Vencidos"));
                    }
                }
            }
        } catch (Exception e) {
            log.warn("[SGI-DASHBOARD-METRICS] Error consultando metricas de Agenda: {}", e.getMessage());
        }
    }

    private void consultarMetricasConsultor(DashboardKpisDTO kpis, String email, boolean filtrarPorUsuario) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        if (mssqlConfig.getUser() == null || mssqlConfig.getUser().trim().isEmpty()) {
            return;
        }

        String dbName = mssqlConfig.getDbConsultor() != null ? mssqlConfig.getDbConsultor() : "gestioni_consultorNet";
        String jdbcUrl = mssqlConfig.buildJdbcUrl(dbName);

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword())) {
            // 1. Auditorias en curso y pendientes de firma
            StringBuilder sqlAuditorias = new StringBuilder();
            sqlAuditorias.append("SELECT ");
            sqlAuditorias.append("    COUNT(*) AS EnCurso, ");
            sqlAuditorias.append("    SUM(CASE WHEN a.BitFirma IS NULL OR a.BitFirma = 0 THEN 1 ELSE 0 END) AS PendienteFirma ");
            sqlAuditorias.append("FROM dbo.Auditorias a ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlAuditorias.append("JOIN dbo.AspNetUsers au ON a.StrUsuarioID = au.Id ");
            }
            sqlAuditorias.append("WHERE (a.BitEstado IS NULL OR a.BitEstado = 1) ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlAuditorias.append("  AND (LOWER(au.Email) = LOWER(?) OR LOWER(au.UserName) = LOWER(?) OR LOWER(a.StrUsuarioID) = LOWER(?)) ");
            }

            try (PreparedStatement stmt = conn.prepareStatement(sqlAuditorias.toString())) {
                if (filtrarPorUsuario && email != null && !email.isBlank()) {
                    String term = email.trim();
                    stmt.setString(1, term);
                    stmt.setString(2, term);
                    stmt.setString(3, term);
                }
                try (ResultSet rs = stmt.executeQuery()) {
                    if (rs.next()) {
                        kpis.setAuditoriasEnCurso(rs.getLong("EnCurso"));
                        kpis.setAuditoriasPendientesFirma(rs.getLong("PendienteFirma"));
                    }
                }
            }

            // 2. Planes de accion / No conformidades (dbo.AuditoriasDetalleAC)
            StringBuilder sqlPlanes = new StringBuilder();
            sqlPlanes.append("SELECT COUNT(*) AS TotalPlanes FROM dbo.AuditoriasDetalleAC ac ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlPlanes.append("JOIN dbo.Auditorias a ON ac.IntAuditoriaID = a.IntAuditoriaID ");
                sqlPlanes.append("JOIN dbo.AspNetUsers au ON a.StrUsuarioID = au.Id ");
                sqlPlanes.append("WHERE (LOWER(au.Email) = LOWER(?) OR LOWER(au.UserName) = LOWER(?) OR LOWER(a.StrUsuarioID) = LOWER(?)) ");
            }

            try (PreparedStatement stmt = conn.prepareStatement(sqlPlanes.toString())) {
                if (filtrarPorUsuario && email != null && !email.isBlank()) {
                    String term = email.trim();
                    stmt.setString(1, term);
                    stmt.setString(2, term);
                    stmt.setString(3, term);
                }
                try (ResultSet rs = stmt.executeQuery()) {
                    if (rs.next()) {
                        kpis.setPlanesAccionPendientes(rs.getLong("TotalPlanes"));
                    }
                }
            }

            // 3. Diagnosticos iniciales en proceso (dbo.DocumentosDiagnostico)
            StringBuilder sqlDiag = new StringBuilder();
            sqlDiag.append("SELECT COUNT(*) AS EnProceso FROM dbo.DocumentosDiagnostico d ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlDiag.append("JOIN dbo.AspNetUsers au ON d.StrUsuarioID = au.Id ");
            }
            sqlDiag.append("WHERE (d.BitFinalizado IS NULL OR d.BitFinalizado = 0) ");
            if (filtrarPorUsuario && email != null && !email.isBlank()) {
                sqlDiag.append("  AND (LOWER(au.Email) = LOWER(?) OR LOWER(au.UserName) = LOWER(?) OR LOWER(d.StrUsuarioID) = LOWER(?)) ");
            }

            try (PreparedStatement stmt = conn.prepareStatement(sqlDiag.toString())) {
                if (filtrarPorUsuario && email != null && !email.isBlank()) {
                    String term = email.trim();
                    stmt.setString(1, term);
                    stmt.setString(2, term);
                    stmt.setString(3, term);
                }
                try (ResultSet rs = stmt.executeQuery()) {
                    if (rs.next()) {
                        kpis.setDiagnosticosEnProceso(rs.getLong("EnProceso"));
                    }
                }
            }
        } catch (Exception e) {
            log.warn("[SGI-DASHBOARD-METRICS] Error consultando metricas de Consultor: {}", e.getMessage());
        }
    }

    private boolean isAdminUser(String email) {
        if (email == null) return false;
        String normalized = email.trim().toLowerCase();
        return normalized.equals("admon@waloyogroup.com")
                || normalized.contains("admin")
                || normalized.equals("gerencia@gestionintegralsgi.com.co");
    }
}
