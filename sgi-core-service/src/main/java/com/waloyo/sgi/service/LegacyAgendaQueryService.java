package com.waloyo.sgi.service;

import com.waloyo.sgi.dto.AgendaDashboardDTO;
import com.waloyo.sgi.sync.SgiSyncProperties;
import lombok.extern.slf4j.Slf4j;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.*;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

@Service
@Slf4j
public class LegacyAgendaQueryService {

    private final SgiSyncProperties syncProperties;
    private final JdbcTemplate jdbcTemplate;

    private static final DateTimeFormatter DISPLAY_DATE_FORMAT = DateTimeFormatter.ofPattern("d 'de' MMMM", new Locale("es", "CO"));
    private static final DateTimeFormatter DISPLAY_TIME_FORMAT = DateTimeFormatter.ofPattern("hh:mm a", Locale.ENGLISH);

    public LegacyAgendaQueryService(SgiSyncProperties syncProperties, JdbcTemplate jdbcTemplate) {
        this.syncProperties = syncProperties;
        this.jdbcTemplate = jdbcTemplate;
    }

    /**
     * Consulta las próximas asesorías (desde hoy en adelante en orden ASC).
     * Si soloMias es true o el usuario es un consultor regular, filtra por su email.
     * Si el usuario es administrador y soloMias es false, muestra las de todo el holding.
     */
    public List<AgendaDashboardDTO> obtenerProximasAsesorias(String email, int limit, Boolean soloMias) {
        int safeLimit = (limit <= 0 || limit > 50) ? 5 : limit;
        boolean filtrarPorUsuario = Boolean.TRUE.equals(soloMias) || (!isAdminUser(email) && email != null && !email.isBlank());

        List<AgendaDashboardDTO> resultados = consultarMssqlDirecto(email, safeLimit, filtrarPorUsuario, true);
        if (resultados.isEmpty()) {
            // Si no hay futuras programadas, traer las mas recientes pasadas
            resultados = consultarMssqlDirecto(email, safeLimit, filtrarPorUsuario, false);
        }

        if (!resultados.isEmpty()) {
            return resultados;
        }

        log.debug("[SGI-AGENDA-QUERY] Recurriendo a consulta defensiva en espejo local para {}", email);
        List<AgendaDashboardDTO> localRes = consultarEspejoLocal(email, safeLimit, filtrarPorUsuario, true);
        if (localRes.isEmpty()) {
            localRes = consultarEspejoLocal(email, safeLimit, filtrarPorUsuario, false);
        }
        return localRes;
    }

    private List<AgendaDashboardDTO> consultarMssqlDirecto(String email, int limit, boolean filtrarPorUsuario, boolean futuras) {
        List<AgendaDashboardDTO> lista = new ArrayList<>();
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();

        if (mssqlConfig.getUser() == null || mssqlConfig.getUser().trim().isEmpty()) {
            return lista;
        }

        String dbName = mssqlConfig.getDbAgenda() != null ? mssqlConfig.getDbAgenda() : "gestioni_datosNet";
        String jdbcUrl = mssqlConfig.buildJdbcUrl(dbName);

        StringBuilder sql = new StringBuilder();
        sql.append("SELECT TOP (?) ");
        sql.append("    a.IntAgendaID, a.StrTitulo, a.StrDescripcion, a.DatFechaInicial, a.DatFechaFinal, a.StrHoras, ");
        sql.append("    ISNULL(c.StrNombre, 'Sin Cliente Asignado') AS ClienteNombre, ");
        sql.append("    ISNULL(u.StrNombre, 'Asesor SGI') AS AsesorNombre, ");
        sql.append("    u.StrEmail AS AsesorEmail, ");
        sql.append("    ISNULL(te.StrDescripcion, 'Asesoria Tecnica') AS TipoEvento ");
        sql.append("FROM dbo.Agenda a ");
        sql.append("LEFT JOIN dbo.Clientes c ON a.IntClienteID = c.IntClienteID ");
        sql.append("LEFT JOIN dbo.Usuarios u ON a.IntUsuarioID = u.IntUsuarioID ");
        sql.append("LEFT JOIN dbo.TipoEventos te ON a.IntTipoEventoID = te.IntTipoEventoID ");
        sql.append("WHERE (a.OpcCancelada IS NULL OR a.OpcCancelada = 0) ");
        sql.append("  AND a.DatFechaInicial IS NOT NULL ");

        if (futuras) {
            sql.append("  AND a.DatFechaInicial >= CAST(GETDATE() AS DATE) ");
        } else {
            sql.append("  AND a.DatFechaInicial < CAST(GETDATE() AS DATE) ");
        }

        if (filtrarPorUsuario && email != null && !email.trim().isEmpty()) {
            sql.append("  AND LOWER(u.StrEmail) = LOWER(?) ");
        }

        if (futuras) {
            sql.append("ORDER BY a.DatFechaInicial ASC");
        } else {
            sql.append("ORDER BY a.DatFechaInicial DESC");
        }

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword());
             PreparedStatement stmt = conn.prepareStatement(sql.toString())) {

            stmt.setInt(1, limit);
            if (filtrarPorUsuario && email != null && !email.trim().isEmpty()) {
                stmt.setString(2, email.trim());
            }

            try (ResultSet rs = stmt.executeQuery()) {
                while (rs.next()) {
                    lista.add(mapearFilaMssql(rs));
                }
            }
            log.debug("[SGI-AGENDA-QUERY] Obtenidos {} registros desde SQL Server (futuras={}) para {}", lista.size(), futuras, email);
        } catch (Exception e) {
            log.warn("[SGI-AGENDA-QUERY] Error consultando SQL Server: {}", e.getMessage());
        }

        return lista;
    }

    private List<AgendaDashboardDTO> consultarEspejoLocal(String email, int limit, boolean filtrarPorUsuario, boolean futuras) {
        List<AgendaDashboardDTO> lista = new ArrayList<>();

        StringBuilder sql = new StringBuilder();
        sql.append("SELECT DISTINCT ON (a.datfechainicial, a.intagendaid) ");
        sql.append("    a.intagendaid, a.strtitulo, a.strdescripcion, a.datfechainicial, a.datfechafinal, a.strhoras, ");
        sql.append("    COALESCE(c.strnombre, 'Sin Cliente Asignado') AS cliente_nombre, ");
        sql.append("    COALESCE(u.strnombre, 'Asesor SGI') AS asesor_nombre, ");
        sql.append("    u.stremail AS asesor_email ");
        sql.append("FROM sgi_raw.agenda_agenda a ");
        sql.append("LEFT JOIN sgi_raw.agenda_clientes c ON a.intclienteid = c.intclienteid ");
        sql.append("LEFT JOIN sgi_raw.agenda_usuarios u ON a.intusuarioid = u.intusuarioid ");
        sql.append("WHERE (a.opccancelada IS NULL OR a.opccancelada != '1') ");
        sql.append("  AND a.datfechainicial IS NOT NULL ");

        if (futuras) {
            sql.append("  AND a.datfechainicial >= TO_CHAR(CURRENT_DATE, 'YYYY-MM-DD') ");
        } else {
            sql.append("  AND a.datfechainicial < TO_CHAR(CURRENT_DATE, 'YYYY-MM-DD') ");
        }

        List<Object> params = new ArrayList<>();
        if (filtrarPorUsuario && email != null && !email.trim().isEmpty()) {
            sql.append("  AND LOWER(u.stremail) = LOWER(?) ");
            params.add(email.trim());
        }

        if (futuras) {
            sql.append("ORDER BY a.datfechainicial ASC, a.intagendaid ASC ");
        } else {
            sql.append("ORDER BY a.datfechainicial DESC, a.intagendaid DESC ");
        }
        sql.append("LIMIT ?");
        params.add(limit);

        try {
            jdbcTemplate.query(sql.toString(), rs -> {
                lista.add(mapearFilaLocal(rs));
            }, params.toArray());
            log.debug("[SGI-AGENDA-QUERY] Obtenidos {} registros desde espejo local (futuras={}) para {}", lista.size(), futuras, email);
        } catch (Exception e) {
            log.warn("[SGI-AGENDA-QUERY] Error consultando espejo local: {}", e.getMessage());
        }

        return lista;
    }

    private AgendaDashboardDTO mapearFilaMssql(ResultSet rs) throws SQLException {
        Timestamp inicio = rs.getTimestamp("DatFechaInicial");
        String horaStr = "08:00 AM";
        String fechaStr = "Hoy";

        if (inicio != null) {
            LocalDateTime ldt = inicio.toLocalDateTime();
            LocalDate ld = ldt.toLocalDate();
            LocalDate hoy = LocalDate.now();

            if (ld.isEqual(hoy)) {
                fechaStr = "Hoy";
            } else if (ld.isEqual(hoy.plusDays(1))) {
                fechaStr = "Mañana";
            } else {
                fechaStr = ldt.format(DISPLAY_DATE_FORMAT);
            }
            horaStr = ldt.format(DISPLAY_TIME_FORMAT);
        }

        String duracionRaw = rs.getString("StrHoras");
        String duracion = (duracionRaw != null && !duracionRaw.isBlank()) ? (duracionRaw + " Horas") : "2 Horas";

        return AgendaDashboardDTO.builder()
                .id(String.valueOf(rs.getInt("IntAgendaID")))
                .titulo(rs.getString("StrTitulo"))
                .cliente(rs.getString("ClienteNombre"))
                .descripcion(rs.getString("StrDescripcion"))
                .fecha(fechaStr)
                .hora(horaStr)
                .duracion(duracion)
                .asesorNombre(rs.getString("AsesorNombre"))
                .asesorEmail(rs.getString("AsesorEmail"))
                .tipoEvento(rs.getString("TipoEvento"))
                .estado("PROGRAMADO")
                .build();
    }

    private AgendaDashboardDTO mapearFilaLocal(ResultSet rs) throws SQLException {
        String fechaIniRaw = rs.getString("datfechainicial");
        String horaStr = "08:00 AM";
        String fechaStr = "Hoy";

        if (fechaIniRaw != null && !fechaIniRaw.isBlank()) {
            try {
                String clean = fechaIniRaw.replace(".0", "").replace(" ", "T");
                if (clean.length() == 19) {
                    LocalDateTime ldt = LocalDateTime.parse(clean);
                    LocalDate ld = ldt.toLocalDate();
                    LocalDate hoy = LocalDate.now();

                    if (ld.isEqual(hoy)) {
                        fechaStr = "Hoy";
                    } else if (ld.isEqual(hoy.plusDays(1))) {
                        fechaStr = "Mañana";
                    } else {
                        fechaStr = ldt.format(DISPLAY_DATE_FORMAT);
                    }
                    horaStr = ldt.format(DISPLAY_TIME_FORMAT);
                }
            } catch (Exception ignored) {}
        }

        String duracionRaw = rs.getString("strhoras");
        String duracion = (duracionRaw != null && !duracionRaw.isBlank()) ? (duracionRaw + " Horas") : "2 Horas";

        return AgendaDashboardDTO.builder()
                .id(rs.getString("intagendaid"))
                .titulo(rs.getString("strtitulo"))
                .cliente(rs.getString("cliente_nombre"))
                .descripcion(rs.getString("strdescripcion"))
                .fecha(fechaStr)
                .hora(horaStr)
                .duracion(duracion)
                .asesorNombre(rs.getString("asesor_nombre"))
                .asesorEmail(rs.getString("asesor_email"))
                .tipoEvento("Asesoria Especializada")
                .estado("PROGRAMADO")
                .build();
    }

    private boolean isAdminUser(String email) {
        if (email == null || email.isBlank()) return true;
        String e = email.trim().toLowerCase();
        return e.equals("admon@waloyogroup.com") || e.contains("admin");
    }
}
