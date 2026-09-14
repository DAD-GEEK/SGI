package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.UsuarioEntity;
import com.waloyo.sgi.repository.UsuarioRepository;
import com.waloyo.sgi.events.UsuarioStatusPublisher;
import com.waloyo.sgi.auth.AuthService;
import com.waloyo.sgi.common.UsuarioConstants;
import com.waloyo.sgi.dto.UsuarioDTO;
import com.waloyo.sgi.service.UsuarioPayloadService;
import com.waloyo.sgi.sync.MssqlUserSyncService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.http.codec.ServerSentEvent;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;

@RestController
@RequestMapping("/api/usuarios")
@RequiredArgsConstructor
public class UsuarioController {

    private final UsuarioRepository usuarioRepository;
    private final com.waloyo.sgi.repository.AgendaEventoRepository agendaEventoRepository;
    private final UsuarioStatusPublisher statusPublisher;
    private final AuthService authService;
    private final UsuarioPayloadService payloadService;
    private final MssqlUserSyncService mssqlUserSyncService;

    @GetMapping
    public ResponseEntity<List<UsuarioEntity>> listarTodos() {
        return ResponseEntity.ok(usuarioRepository.findAll());
    }

    @GetMapping("/{id}")
    public ResponseEntity<UsuarioEntity> obtenerPorId(@PathVariable UUID id) {
        return usuarioRepository.findById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }

    @GetMapping("/verificar-estado")
    public ResponseEntity<Map<String, Object>> verificarEstado(@RequestParam String email) {
        return usuarioRepository.findByEmail(email)
                .map(usuario -> ResponseEntity.ok(payloadService.buildFullUsuarioResponse(usuario)))
                .orElse(ResponseEntity.ok(buildDefaultResponse()));
    }

    @GetMapping(value = "/stream-estado", produces = MediaType.TEXT_EVENT_STREAM_VALUE)
    public Flux<ServerSentEvent<Map<String, Object>>> streamEstado(
            @RequestParam(required = false) String email,
            @RequestParam(value = "token", required = false) String tokenParam,
            @RequestHeader(value = "Authorization", required = false) String authorization) {

        Flux<ServerSentEvent<Map<String, Object>>> byToken = authService
                .validateToken(authorization, tokenParam)
                .flatMapMany(statusPublisher::subscribe);

        if (email != null) {
            return byToken.switchIfEmpty(statusPublisher.subscribe(email));
        }
        return byToken.switchIfEmpty(Flux.error(
                new ResponseStatusException(HttpStatus.UNAUTHORIZED, "Unauthorized or missing email")));
    }

    @PostMapping("/confirmar-clave")
    public ResponseEntity<Map<String, Object>> confirmarClave(@RequestBody Map<String, String> body) {
        String email = body.get(UsuarioConstants.EMAIL);
        String password = body.get("password");
        if (email == null || email.isBlank()) {
            return ResponseEntity.badRequest().build();
        }

        return usuarioRepository.findByEmail(email)
                .map(usuario -> handleConfirmarClave(usuario, password))
                .orElse(ResponseEntity.notFound().build());
    }

    private ResponseEntity<Map<String, Object>> handleConfirmarClave(UsuarioEntity usuario, String plainPassword) {
        usuario.setMustChangePassword(false);
        usuarioRepository.save(usuario);

        // Sincronizar en tiempo real con las bases de datos de Agenda (3DES) y Consultor (ASP.NET Identity)
        if (plainPassword != null && !plainPassword.isBlank()) {
            mssqlUserSyncService.syncPassword(usuario.getEmail(), plainPassword).subscribe();
            // Aprovisionar o actualizar en Supabase Auth
            authService.provisionUserInSupabase(usuario.getEmail(), plainPassword).subscribe();
        }

        Map<String, Object> payload = payloadService.buildUsuarioPayload(usuario);
        statusPublisher.publish(usuario.getEmail(), payload);

        return ResponseEntity.ok(Map.of(
                UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                UsuarioConstants.MESSAGE, "Contraseña definitiva confirmada y sincronizada."
        ));
    }

    @PostMapping("/auto-sincronizar-password")
    public Mono<ResponseEntity<Map<String, Object>>> autoSincronizarPassword(@RequestBody Map<String, String> body) {
        String email = body.get(UsuarioConstants.EMAIL);
        String password = body.get("password");
        if (email == null || email.isBlank() || password == null || password.isBlank()) {
            return Mono.just(ResponseEntity.badRequest().body(Map.of(
                    UsuarioConstants.STATUS, UsuarioConstants.ERROR,
                    UsuarioConstants.MESSAGE, "Email y contraseña son requeridos"
            )));
        }

        // Asegurar que en PostgreSQL el flag mustChangePassword quede en false
        usuarioRepository.findByEmail(email).ifPresent(u -> {
            if (Boolean.TRUE.equals(u.getMustChangePassword())) {
                u.setMustChangePassword(false);
                usuarioRepository.save(u);
            }
        });

        // Sincronizar unicamente si la clave en Agenda o Consultor esta desactualizada
        return mssqlUserSyncService.syncPasswordIfOutdated(email, password)
                .map(wasUpdated -> {
                    Map<String, Object> response = new HashMap<>();
                    response.put(UsuarioConstants.STATUS, UsuarioConstants.SUCCESS);
                    response.put("updated", wasUpdated);
                    response.put(UsuarioConstants.MESSAGE, wasUpdated
                            ? "Contraseñas desactualizadas en aplicativos detectadas y sincronizadas exitosamente."
                            : "Contraseñas en aplicativos ya se encuentran al día.");
                    return ResponseEntity.ok(response);
                })
                .defaultIfEmpty(ResponseEntity.ok(Map.of(
                        UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                        "updated", false,
                        UsuarioConstants.MESSAGE, "No se requirió sincronización."
                )));
    }

    @PostMapping("/admin/sincronizar-passwords")
    public ResponseEntity<Map<String, Object>> sincronizarPasswordsAdmin(@RequestBody Map<String, Object> body) {
        String claveMaestra = (String) body.get("clave");
        String emailEspecifico = (String) body.get("email");

        if (claveMaestra == null || claveMaestra.isBlank()) {
            return ResponseEntity.badRequest().body(Map.of(
                    UsuarioConstants.STATUS, UsuarioConstants.ERROR,
                    UsuarioConstants.MESSAGE, "El campo 'clave' es obligatorio."
            ));
        }

        List<UsuarioEntity> usuariosParaActualizar;
        if (emailEspecifico != null && !emailEspecifico.isBlank()) {
            usuariosParaActualizar = usuarioRepository.findByEmail(emailEspecifico.trim())
                    .map(List::of)
                    .orElse(List.of());
        } else {
            usuariosParaActualizar = usuarioRepository.findByActivoTrue();
        }

        if (usuariosParaActualizar.isEmpty()) {
            return ResponseEntity.status(404).body(Map.of(
                    UsuarioConstants.STATUS, UsuarioConstants.ERROR,
                    UsuarioConstants.MESSAGE, "No se encontraron usuarios para actualizar."
            ));
        }

        List<String> actualizados = new java.util.ArrayList<>();
        for (UsuarioEntity u : usuariosParaActualizar) {
            u.setMustChangePassword(false);
            usuarioRepository.save(u);

            // Sincronizar en tiempo real con Supabase Auth y MSSQL (Agenda + Consultor)
            mssqlUserSyncService.syncPassword(u.getEmail(), claveMaestra).subscribe();
            authService.provisionUserInSupabase(u.getEmail(), claveMaestra).subscribe();
            actualizados.add(u.getEmail());
        }

        return ResponseEntity.ok(Map.of(
                UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                UsuarioConstants.MESSAGE, "Contraseñas sincronizadas en Supabase Auth, Agenda y Consultor.",
                "totalProcesados", actualizados.size(),
                "usuarios", actualizados
        ));
    }

    @PostMapping("/registrar")
    public ResponseEntity<Map<String, Object>> registrarUsuario(@RequestBody UsuarioDTO usuarioReq) {
        if (usuarioReq.getEmail() == null || usuarioReq.getNombreCompleto() == null) {
            return ResponseEntity.badRequest().body(Map.of(
                    UsuarioConstants.ERROR, "Nombre y correo son obligatorios."));
        }

        UsuarioEntity usuario = usuarioRepository.findByEmail(usuarioReq.getEmail())
                .orElseGet(() -> buildNewUsuario(usuarioReq));

        updateUsuarioFromDTO(usuario, usuarioReq);
        usuarioRepository.save(usuario);

        // Aprovisionar simultáneamente en Supabase Auth con Service Role Key
        authService.provisionUserInSupabase(usuario.getEmail(), null).subscribe();

        // Sincronizar permisos y accesos hacia bases de datos de Agenda y Consultor
        mssqlUserSyncService.syncModulePermissions(
                usuario.getEmail(),
                usuario.getNombreCompleto(),
                usuario.getDocumento(),
                usuario.getModulosPermitidos(),
                null
        ).subscribe();

        Map<String, Object> payload = payloadService.buildUsuarioPayload(usuario);
        statusPublisher.publish(usuario.getEmail(), payload);

        return ResponseEntity.ok(Map.of(
                UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                UsuarioConstants.MESSAGE, "Usuario registrado o actualizado exitosamente.",
                UsuarioConstants.ID, usuario.getId(),
                UsuarioConstants.MUST_CHANGE_PASSWORD, true
        ));
    }

    private UsuarioEntity buildNewUsuario(UsuarioDTO usuarioReq) {
        return UsuarioEntity.builder()
                .email(usuarioReq.getEmail())
                .documento(usuarioReq.getDocumento() != null ? usuarioReq.getDocumento() :
                        "DOC-" + System.currentTimeMillis())
                .build();
    }

    private void updateUsuarioFromDTO(UsuarioEntity usuario, UsuarioDTO datos) {
        if (datos.getNombreCompleto() != null) usuario.setNombreCompleto(datos.getNombreCompleto());
        if (datos.getTipoDocumento() != null) usuario.setTipoDocumento(datos.getTipoDocumento());
        if (datos.getDocumento() != null) usuario.setDocumento(datos.getDocumento());
        if (datos.getPais() != null) usuario.setPais(datos.getPais());
        if (datos.getTelefonoMovil() != null) usuario.setTelefonoMovil(datos.getTelefonoMovil());
        if (datos.getRol() != null) usuario.setRol(datos.getRol());
        if (datos.getModulosPermitidos() != null) usuario.setModulosPermitidos(datos.getModulosPermitidos());

        usuario.setMustChangePassword(true);
        usuario.setActivo(true);
    }

    @PutMapping("/{id}")
    public ResponseEntity<UsuarioEntity> actualizarUsuario(
            @PathVariable UUID id,
            @RequestBody UsuarioDTO datos) {
        return usuarioRepository.findById(id)
                .map(usuario -> handleActualizarUsuario(usuario, datos))
                .orElse(ResponseEntity.notFound().build());
    }

    private ResponseEntity<UsuarioEntity> handleActualizarUsuario(UsuarioEntity usuario, UsuarioDTO datos) {
        if (datos.getNombreCompleto() != null) usuario.setNombreCompleto(datos.getNombreCompleto());
        if (datos.getTipoDocumento() != null) usuario.setTipoDocumento(datos.getTipoDocumento());
        if (datos.getDocumento() != null) usuario.setDocumento(datos.getDocumento());
        if (datos.getPais() != null) usuario.setPais(datos.getPais());
        if (datos.getTelefonoMovil() != null) usuario.setTelefonoMovil(datos.getTelefonoMovil());
        if (datos.getEmail() != null) usuario.setEmail(datos.getEmail());
        if (datos.getRol() != null) usuario.setRol(datos.getRol());
        if (datos.getActivo() != null) usuario.setActivo(datos.getActivo());
        if (datos.getModulosPermitidos() != null) usuario.setModulosPermitidos(datos.getModulosPermitidos());

        usuarioRepository.save(usuario);

        // Sincronizar cambios de estado y permisos de módulos hacia MSSQL (Agenda y Consultor)
        mssqlUserSyncService.syncUserStatus(usuario.getEmail(), Boolean.TRUE.equals(usuario.getActivo())).subscribe();
        mssqlUserSyncService.syncModulePermissions(
                usuario.getEmail(),
                usuario.getNombreCompleto(),
                usuario.getDocumento(),
                usuario.getModulosPermitidos(),
                null
        ).subscribe();

        Map<String, Object> payload = payloadService.buildUsuarioPayload(usuario);
        statusPublisher.publish(usuario.getEmail(), payload);

        return ResponseEntity.ok(usuario);
    }

    @PostMapping("/{id}/reenviar-credenciales")
    public ResponseEntity<Map<String, Object>> reenviarCredenciales(@PathVariable UUID id) {
        return usuarioRepository.findById(id)
                .map(this::handleReenviarCredenciales)
                .orElseGet(() -> buildErrorResponse("Usuario no encontrado con ID: " + id));
    }

    private ResponseEntity<Map<String, Object>> handleReenviarCredenciales(UsuarioEntity usuario) {
        usuario.setMustChangePassword(true);
        usuarioRepository.save(usuario);
        Map<String, Object> payload = payloadService.buildUsuarioPayload(usuario);
        statusPublisher.publish(usuario.getEmail(), payload);

        return ResponseEntity.ok(Map.of(
                UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                UsuarioConstants.MESSAGE, "Credenciales temporales re-generadas y notificadas al usuario.",
                UsuarioConstants.EMAIL, usuario.getEmail(),
                UsuarioConstants.MUST_CHANGE_PASSWORD, true
        ));
    }

    @PostMapping("/reenviar-credenciales-email")
    public ResponseEntity<Map<String, Object>> reenviarCredencialesPorEmail(@RequestParam String email) {
        return usuarioRepository.findByEmail(email)
                .map(this::handleReenviarCredenciales)
                .orElseGet(() -> buildErrorResponse("Usuario no encontrado con email: " + email));
    }

    @PutMapping("/{id}/alternar-estado")
    public ResponseEntity<Map<String, Object>> alternarEstado(@PathVariable UUID id) {
        return usuarioRepository.findById(id)
                .map(this::handleAlternarEstado)
                .orElse(ResponseEntity.notFound().build());
    }

    private ResponseEntity<Map<String, Object>> handleAlternarEstado(UsuarioEntity usuario) {
        boolean nuevoEstado = !Boolean.TRUE.equals(usuario.getActivo());
        usuario.setActivo(nuevoEstado);
        usuarioRepository.save(usuario);

        // Sincronizar activación/desactivación en tiempo real hacia MSSQL
        mssqlUserSyncService.syncUserStatus(usuario.getEmail(), nuevoEstado).subscribe();

        Map<String, Object> payload = payloadService.buildUsuarioPayload(usuario);
        statusPublisher.publish(usuario.getEmail(), payload);

        return ResponseEntity.ok(Map.of(
                UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                UsuarioConstants.ACTIVO, usuario.getActivo()
        ));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Map<String, Object>> eliminarUsuarioDefinitivo(@PathVariable UUID id) {
        return usuarioRepository.findById(id)
                .map(this::handleEliminarUsuario)
                .orElse(ResponseEntity.notFound().build());
    }

    private ResponseEntity<Map<String, Object>> handleEliminarUsuario(UsuarioEntity usuario) {
        String email = usuario.getEmail();
        UUID usuarioId = usuario.getId();

        // 1. PRESERVAR EVENTOS HISTÓRICOS: En lugar de borrarlos (deleteAll), desvincular el asesor y conservar auditoría
        List<com.waloyo.sgi.entity.AgendaEventoEntity> eventos = agendaEventoRepository.findByAsesorId(usuarioId);
        if (!eventos.isEmpty()) {
            for (com.waloyo.sgi.entity.AgendaEventoEntity ev : eventos) {
                ev.setAsesor(null);
                ev.setAsesorHistoricoNombre(usuario.getNombreCompleto());
                ev.setAsesorHistoricoEmail(usuario.getEmail());
            }
            agendaEventoRepository.saveAll(eventos);
        }

        // 2. Desactivar en bases de datos legadas MSSQL (sin borrar historia de Agenda y Consultor)
        mssqlUserSyncService.syncUserStatus(email, false).subscribe();

        // 3. Eliminar de PostgreSQL CRM
        usuarioRepository.delete(usuario);
        Map<String, Object> payload = payloadService.buildDeactivatedPayload(usuario);
        statusPublisher.publish(email, payload);

        return ResponseEntity.ok(Map.of(
                UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                UsuarioConstants.MESSAGE, "Usuario eliminado del CRM preservando integridad y auditoría de eventos históricos."
        ));
    }

    private ResponseEntity<Map<String, Object>> buildErrorResponse(String error) {
        return ResponseEntity.badRequest().body(Map.of(UsuarioConstants.ERROR, error));
    }

    private Map<String, Object> buildDefaultResponse() {
        return Map.of(
                UsuarioConstants.MUST_CHANGE_PASSWORD, false,
                UsuarioConstants.ACTIVO, true
        );
    }
}
