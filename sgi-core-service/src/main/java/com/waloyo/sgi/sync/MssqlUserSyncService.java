package com.waloyo.sgi.sync;

import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import reactor.core.publisher.Mono;
import reactor.core.scheduler.Schedulers;

import java.sql.*;
import java.util.UUID;

/**
 * Servicio reactivo y asincrono para sincronizar credenciales, estado y accesos de usuarios
 * desde el CRM hacia las bases de datos legadas de SQL Server:
 * - gestioni_datosNet (AgendaSGI - Tabla Usuarios)
 * - gestioni_consultorNet (ConsultorSGI - Tabla AspNetUsers)
 */
@Service
@Slf4j
public class MssqlUserSyncService {

    private final SgiSyncProperties syncProperties;
    private final SgiLegacyCryptoService cryptoService;

    public MssqlUserSyncService(SgiSyncProperties syncProperties, SgiLegacyCryptoService cryptoService) {
        this.syncProperties = syncProperties;
        this.cryptoService = cryptoService;
    }

    /**
     * Sincroniza la contrasena en texto plano hacia ambas bases de datos legacy:
     * - En Agenda: se cifra con 3DES-MD5
     * - En Consultor: se hashea con ASP.NET Identity v2 (PBKDF2-HMAC-SHA1)
     */
    public Mono<Void> syncPassword(String email, String plainPassword) {
        if (email == null || email.isBlank() || plainPassword == null || plainPassword.isBlank()) {
            return Mono.empty();
        }

        return Mono.fromRunnable(() -> {
            log.info("[SGI-MSSQL-SYNC] Iniciando sincronizacion de contrasena para {}", email);
            syncAgendaPassword(email, plainPassword);
            syncConsultorPassword(email, plainPassword);
        }).subscribeOn(Schedulers.boundedElastic()).then();
    }

    /**
     * Sincroniza la contrasena unicamente si esta desactualizada en alguna de las bases legadas.
     * Retorna un Mono<Boolean> que emite true si se realizo alguna actualizacion, o false si ya estaban al dia.
     */
    public Mono<Boolean> syncPasswordIfOutdated(String email, String plainPassword) {
        if (email == null || email.isBlank() || plainPassword == null || plainPassword.isBlank()) {
            return Mono.just(false);
        }

        return Mono.fromCallable(() -> {
            boolean agendaOutdated = isAgendaPasswordOutdated(email, plainPassword);
            boolean consultorOutdated = isConsultorPasswordOutdated(email, plainPassword);

            if (agendaOutdated) {
                log.info("[SGI-MSSQL-SYNC] Clave desactualizada en Agenda para {}. Actualizando...", email);
                syncAgendaPassword(email, plainPassword);
            }
            if (consultorOutdated) {
                log.info("[SGI-MSSQL-SYNC] Clave desactualizada en Consultor para {}. Actualizando...", email);
                syncConsultorPassword(email, plainPassword);
            }

            boolean wasUpdated = agendaOutdated || consultorOutdated;
            if (wasUpdated) {
                log.info("[SGI-MSSQL-SYNC] Sincronizacion completada por desactualizacion para {}", email);
            } else {
                log.info("[SGI-MSSQL-SYNC] Contrasenas ya se encuentran al dia en aplicativos para {}", email);
            }
            return wasUpdated;
        }).subscribeOn(Schedulers.boundedElastic());
    }

    /**
     * Sincroniza el estado (activo / inactivo) en ambas bases de datos legacy.
     */
    public Mono<Void> syncUserStatus(String email, boolean activo) {
        if (email == null || email.isBlank()) {
            return Mono.empty();
        }

        return Mono.fromRunnable(() -> {
            log.info("[SGI-MSSQL-SYNC] Sincronizando estado (activo={}) para {}", activo, email);
            updateAgendaStatus(email, activo);
            updateConsultorStatus(email, activo);
        }).subscribeOn(Schedulers.boundedElastic()).then();
    }

    /**
     * Sincroniza el acceso segun los modulos permitidos del usuario:
     * - Si tiene 'agenda' o '*' -> crear o activar en gestioni_datosNet
     * - Si NO tiene 'agenda'   -> inactivar en gestioni_datosNet
     * - Si tiene 'consultor' o '*' -> crear o activar en gestioni_consultorNet
     * - Si NO tiene 'consultor'   -> inactivar en gestioni_consultorNet
     */
    public Mono<Void> syncModulePermissions(String email, String nombre, String documento, String modulosPermitidos, String plainPassword) {
        if (email == null || email.isBlank()) {
            return Mono.empty();
        }

        return Mono.fromRunnable(() -> {
            boolean hasAgenda = hasAccess(modulosPermitidos, "agenda");
            boolean hasConsultor = hasAccess(modulosPermitidos, "consultor");

            log.info("[SGI-MSSQL-SYNC] Evaluando permisos para {}: Agenda={}, Consultor={}", email, hasAgenda, hasConsultor);

            syncAgendaAccess(email, nombre, documento, hasAgenda, plainPassword);
            syncConsultorAccess(email, nombre, documento, hasConsultor, plainPassword);
        }).subscribeOn(Schedulers.boundedElastic()).then();
    }

    private boolean hasAccess(String modulosPermitidos, String modulo) {
        if (modulosPermitidos == null) return true; // Por defecto acceso general si no se especifica
        String lower = modulosPermitidos.toLowerCase();
        return lower.contains("*") || lower.contains(modulo.toLowerCase());
    }

    // ==========================================
    // MÉTODOS DE AGENDA (gestioni_datosNet.dbo.Usuarios)
    // ==========================================

    private void syncAgendaPassword(String email, String plainPassword) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        String jdbcUrl = mssqlConfig.buildJdbcUrl(mssqlConfig.getDbAgenda());

        if (isMssqlDisabled(mssqlConfig)) return;

        String encryptedPass = cryptoService.encryptAgendaPassword(plainPassword);

        String sql = "UPDATE Usuarios SET StrClave = ? WHERE LOWER(StrEmail) = LOWER(?) OR LOWER(StrCodigo) = LOWER(?)";

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword());
             PreparedStatement stmt = conn.prepareStatement(sql)) {

            stmt.setString(1, encryptedPass);
            stmt.setString(2, email.trim());
            stmt.setString(3, email.trim());

            int updated = stmt.executeUpdate();
            log.info("[SGI-MSSQL-SYNC] Agenda contrasena actualizada para {}: {} filas afectadas", email, updated);
        } catch (Exception e) {
            log.error("[SGI-MSSQL-SYNC] Error actualizando contrasena en Agenda para {}: {}", email, e.getMessage());
        }
    }

    private boolean isAgendaPasswordOutdated(String email, String plainPassword) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        if (isMssqlDisabled(mssqlConfig)) return false;

        String jdbcUrl = mssqlConfig.buildJdbcUrl(mssqlConfig.getDbAgenda());
        String expectedEnc = cryptoService.encryptAgendaPassword(plainPassword);
        String sql = "SELECT StrClave FROM Usuarios WHERE LOWER(StrEmail) = LOWER(?) OR LOWER(StrCodigo) = LOWER(?)";

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword());
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, email.trim());
            stmt.setString(2, email.trim());
            try (ResultSet rs = stmt.executeQuery()) {
                if (rs.next()) {
                    String currentClave = rs.getString("StrClave");
                    return currentClave == null || !currentClave.equals(expectedEnc);
                }
            }
        } catch (Exception e) {
            log.warn("[SGI-MSSQL-SYNC] Error verificando si clave de Agenda esta desactualizada para {}: {}", email, e.getMessage());
        }
        return false;
    }

    private void updateAgendaStatus(String email, boolean activo) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        String jdbcUrl = mssqlConfig.buildJdbcUrl(mssqlConfig.getDbAgenda());

        if (isMssqlDisabled(mssqlConfig)) return;

        String sql = "UPDATE Usuarios SET OpcEstado = ? WHERE LOWER(StrEmail) = LOWER(?) OR LOWER(StrCodigo) = LOWER(?)";

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword());
             PreparedStatement stmt = conn.prepareStatement(sql)) {

            stmt.setBoolean(1, activo);
            stmt.setString(2, email.trim());
            stmt.setString(3, email.trim());

            int updated = stmt.executeUpdate();
            log.info("[SGI-MSSQL-SYNC] Agenda estado actualizado para {} (activo={}): {} filas", email, activo, updated);
        } catch (Exception e) {
            log.error("[SGI-MSSQL-SYNC] Error actualizando estado en Agenda para {}: {}", email, e.getMessage());
        }
    }

    private void syncAgendaAccess(String email, String nombre, String documento, boolean hasAccess, String plainPassword) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        String jdbcUrl = mssqlConfig.buildJdbcUrl(mssqlConfig.getDbAgenda());

        if (isMssqlDisabled(mssqlConfig)) return;

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword())) {
            // Verificar si el usuario ya existe en Usuarios
            String checkSql = "SELECT IntUsuarioID FROM Usuarios WHERE LOWER(StrEmail) = LOWER(?) OR LOWER(StrCodigo) = LOWER(?)";
            try (PreparedStatement checkStmt = conn.prepareStatement(checkSql)) {
                checkStmt.setString(1, email.trim());
                checkStmt.setString(2, email.trim());

                try (ResultSet rs = checkStmt.executeQuery()) {
                    if (rs.next()) {
                        // Ya existe: actualizar estado
                        updateAgendaStatus(email, hasAccess);
                        if (hasAccess && plainPassword != null && !plainPassword.isBlank()) {
                            syncAgendaPassword(email, plainPassword);
                        }
                    } else if (hasAccess) {
                        // No existe y tiene acceso: Crear registro basico en Agenda con Rol activo y color institucional
                        String insertSql = "INSERT INTO Usuarios (StrCodigo, StrNombre, StrEmail, StrClave, IntRolID, StrColor, OpcEstado) " +
                                "VALUES (?, ?, ?, ?, (SELECT TOP 1 IntRolID FROM Roles WHERE StrDescripcion LIKE '%Admin%' OR StrCodigo LIKE '%Admin%'), '#1e3a8a', 1)";
                        try (PreparedStatement insertStmt = conn.prepareStatement(insertSql)) {
                            String code = (documento != null && !documento.isBlank()) ? documento : email.split("@")[0];
                            String encPass = (plainPassword != null && !plainPassword.isBlank())
                                    ? cryptoService.encryptAgendaPassword(plainPassword)
                                    : cryptoService.encryptAgendaPassword("Sgi" + System.currentTimeMillis() + "!*");

                            insertStmt.setString(1, code);
                            insertStmt.setString(2, nombre != null ? nombre : email.split("@")[0]);
                            insertStmt.setString(3, email.trim());
                            insertStmt.setString(4, encPass);

                            insertStmt.executeUpdate();
                            log.info("[SGI-MSSQL-SYNC] Usuario creado exitosamente en Agenda: {}", email);
                        }
                    }
                }
            }
        } catch (Exception e) {
            log.error("[SGI-MSSQL-SYNC] Error sincronizando acceso a Agenda para {}: {}", email, e.getMessage());
        }
    }

    // ==========================================
    // MÉTODOS DE CONSULTOR (gestioni_consultorNet.dbo.AspNetUsers)
    // ==========================================

    private void syncConsultorPassword(String email, String plainPassword) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        String jdbcUrl = mssqlConfig.buildJdbcUrl(mssqlConfig.getDbConsultor());

        if (isMssqlDisabled(mssqlConfig)) return;

        String passwordHash = cryptoService.hashConsultorPassword(plainPassword);
        String securityStamp = UUID.randomUUID().toString();

        String sql = "UPDATE AspNetUsers SET PasswordHash = ?, SecurityStamp = ? WHERE LOWER(Email) = LOWER(?) OR LOWER(UserName) = LOWER(?)";

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword());
             PreparedStatement stmt = conn.prepareStatement(sql)) {

            stmt.setString(1, passwordHash);
            stmt.setString(2, securityStamp);
            stmt.setString(3, email.trim());
            stmt.setString(4, email.trim());

            int updated = stmt.executeUpdate();
            log.info("[SGI-MSSQL-SYNC] Consultor contrasena actualizada para {}: {} filas afectadas", email, updated);
        } catch (Exception e) {
            log.error("[SGI-MSSQL-SYNC] Error actualizando contrasena en Consultor para {}: {}", email, e.getMessage());
        }
    }

    private boolean isConsultorPasswordOutdated(String email, String plainPassword) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        if (isMssqlDisabled(mssqlConfig)) return false;

        String jdbcUrl = mssqlConfig.buildJdbcUrl(mssqlConfig.getDbConsultor());
        String sql = "SELECT PasswordHash FROM AspNetUsers WHERE LOWER(Email) = LOWER(?) OR LOWER(UserName) = LOWER(?)";

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword());
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, email.trim());
            stmt.setString(2, email.trim());
            try (ResultSet rs = stmt.executeQuery()) {
                if (rs.next()) {
                    String currentHash = rs.getString("PasswordHash");
                    if (currentHash == null || currentHash.isBlank()) return true;
                    return !cryptoService.verifyConsultorPassword(currentHash, plainPassword);
                }
            }
        } catch (Exception e) {
            log.warn("[SGI-MSSQL-SYNC] Error verificando si clave de Consultor esta desactualizada para {}: {}", email, e.getMessage());
        }
        return false;
    }

    private void updateConsultorStatus(String email, boolean activo) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        String jdbcUrl = mssqlConfig.buildJdbcUrl(mssqlConfig.getDbConsultor());

        if (isMssqlDisabled(mssqlConfig)) return;

        // En ASP.NET Identity: Bloqueado = LockoutEnabled = 1 AND LockoutEndDateUtc > NOW
        // Activo = LockoutEndDateUtc IS NULL
        String sql = activo
                ? "UPDATE AspNetUsers SET LockoutEndDateUtc = NULL, AccessFailedCount = 0 WHERE LOWER(Email) = LOWER(?) OR LOWER(UserName) = LOWER(?)"
                : "UPDATE AspNetUsers SET LockoutEnabled = 1, LockoutEndDateUtc = '2099-12-31 23:59:59' WHERE LOWER(Email) = LOWER(?) OR LOWER(UserName) = LOWER(?)";

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword());
             PreparedStatement stmt = conn.prepareStatement(sql)) {

            stmt.setString(1, email.trim());
            stmt.setString(2, email.trim());

            int updated = stmt.executeUpdate();
            log.info("[SGI-MSSQL-SYNC] Consultor estado actualizado para {} (activo={}): {} filas", email, activo, updated);
        } catch (Exception e) {
            log.error("[SGI-MSSQL-SYNC] Error actualizando estado en Consultor para {}: {}", email, e.getMessage());
        }
    }

    private void syncConsultorAccess(String email, String nombre, String documento, boolean hasAccess, String plainPassword) {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();
        String jdbcUrl = mssqlConfig.buildJdbcUrl(mssqlConfig.getDbConsultor());

        if (isMssqlDisabled(mssqlConfig)) return;

        try (Connection conn = DriverManager.getConnection(jdbcUrl, mssqlConfig.getUser(), mssqlConfig.getPassword())) {
            String checkSql = "SELECT Id FROM AspNetUsers WHERE LOWER(Email) = LOWER(?) OR LOWER(UserName) = LOWER(?)";
            try (PreparedStatement checkStmt = conn.prepareStatement(checkSql)) {
                checkStmt.setString(1, email.trim());
                checkStmt.setString(2, email.trim());

                try (ResultSet rs = checkStmt.executeQuery()) {
                    if (rs.next()) {
                        // Ya existe: actualizar estado
                        updateConsultorStatus(email, hasAccess);
                        if (hasAccess && plainPassword != null && !plainPassword.isBlank()) {
                            syncConsultorPassword(email, plainPassword);
                        }
                    } else if (hasAccess) {
                        // No existe y tiene acceso: Crear registro básico en AspNetUsers
                        String insertSql = "INSERT INTO AspNetUsers (Id, UserName, Email, EmailConfirmed, PasswordHash, SecurityStamp, LockoutEnabled, AccessFailedCount, NombreUsuario, FechaIngreso) " +
                                "VALUES (?, ?, ?, 1, ?, ?, 0, 0, ?, CURRENT_TIMESTAMP)";

                        try (PreparedStatement insertStmt = conn.prepareStatement(insertSql)) {
                            String newId = UUID.randomUUID().toString();
                            String passHash = (plainPassword != null && !plainPassword.isBlank())
                                    ? cryptoService.hashConsultorPassword(plainPassword)
                                    : cryptoService.hashConsultorPassword("Sgi" + System.currentTimeMillis() + "!*");

                            insertStmt.setString(1, newId);
                            insertStmt.setString(2, email.trim());
                            insertStmt.setString(3, email.trim());
                            insertStmt.setString(4, passHash);
                            insertStmt.setString(5, UUID.randomUUID().toString());
                            insertStmt.setString(6, nombre != null ? nombre : email.split("@")[0]);

                            insertStmt.executeUpdate();
                            log.info("[SGI-MSSQL-SYNC] Usuario creado exitosamente en Consultor (AspNetUsers): {}", email);

                            // Auto-activación requerida por el dominio de Consultor en TercerosUsuarios y AspNetUserRoles
                            String terceroSql = "DECLARE @TerceroID INT; " +
                                    "SELECT TOP 1 @TerceroID = IntTerceroID FROM Terceros WHERE StrIdentificacion LIKE '%901459080%' OR StrNombre LIKE '%GESTION INTEGRAL%'; " +
                                    "IF @TerceroID IS NULL SELECT TOP 1 @TerceroID = IntTerceroID FROM Terceros WHERE OpcEstado = 1; " +
                                    "IF NOT EXISTS (SELECT 1 FROM TercerosUsuarios WHERE LOWER(StrUsuarioEmail) = LOWER(?)) " +
                                    "BEGIN " +
                                    "  INSERT INTO TercerosUsuarios (IntTerceroID, StrUsuarioNombre, StrUsuarioEmail, OpcEmailEnviado, OpcEstado) " +
                                    "  VALUES (@TerceroID, ?, ?, 1, 1); " +
                                    "END; " +
                                    "DECLARE @RolId NVARCHAR(128); " +
                                    "SELECT TOP 1 @RolId = Id FROM AspNetRoles WHERE Name IN ('Administrador', 'Asesor'); " +
                                    "IF @RolId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = ? AND RoleId = @RolId) " +
                                    "BEGIN " +
                                    "  INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (?, @RolId); " +
                                    "END;";

                            try (PreparedStatement tStmt = conn.prepareStatement(terceroSql)) {
                                tStmt.setString(1, email.trim());
                                tStmt.setString(2, nombre != null ? nombre : email.split("@")[0]);
                                tStmt.setString(3, email.trim());
                                tStmt.setString(4, newId);
                                tStmt.setString(5, newId);
                                tStmt.executeUpdate();
                                log.info("[SGI-MSSQL-SYNC] TercerosUsuarios y Roles asociados con éxito para {}", email);
                            }
                        }
                    }
                }
            }
        } catch (Exception e) {
            log.error("[SGI-MSSQL-SYNC] Error sincronizando acceso a Consultor para {}: {}", email, e.getMessage());
        }
    }

    private boolean isMssqlDisabled(SgiSyncProperties.Mssql mssqlConfig) {
        if (mssqlConfig.getUser() == null || mssqlConfig.getUser().trim().isEmpty()) {
            log.warn("[SGI-MSSQL-SYNC] Credenciales de MS SQL Server no configuradas (MSSQL_USER vacio). Omitiendo.");
            return true;
        }
        return false;
    }
}
