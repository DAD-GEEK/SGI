package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.UsuarioEntity;
import com.waloyo.sgi.repository.UsuarioRepository;
import com.waloyo.sgi.events.UsuarioStatusPublisher;
import com.waloyo.sgi.auth.AuthService;
import com.waloyo.sgi.common.UsuarioConstants;
import com.waloyo.sgi.dto.UsuarioDTO;
import com.waloyo.sgi.service.UsuarioPayloadService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.http.codec.ServerSentEvent;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;
import reactor.core.publisher.Flux;

import java.util.List;
import java.util.Map;
import java.util.UUID;

@RestController
@RequestMapping("/api/usuarios")
@RequiredArgsConstructor
public class UsuarioController {

    private final UsuarioRepository usuarioRepository;
    private final UsuarioStatusPublisher statusPublisher;
    private final AuthService authService;
    private final UsuarioPayloadService payloadService;

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
        if (email == null || email.isBlank()) {
            return ResponseEntity.badRequest().build();
        }

        return usuarioRepository.findByEmail(email)
                .map(this::handleConfirmarClave)
                .orElse(ResponseEntity.notFound().build());
    }

    private ResponseEntity<Map<String, Object>> handleConfirmarClave(UsuarioEntity usuario) {
        usuario.setMustChangePassword(false);
        usuarioRepository.save(usuario);
        Map<String, Object> payload = payloadService.buildUsuarioPayload(usuario);
        statusPublisher.publish(usuario.getEmail(), payload);

        return ResponseEntity.ok(Map.of(
                UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                UsuarioConstants.MESSAGE, "Contraseña definitiva confirmada."
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
        usuario.setActivo(!Boolean.TRUE.equals(usuario.getActivo()));
        usuarioRepository.save(usuario);
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
        usuarioRepository.delete(usuario);
        Map<String, Object> payload = payloadService.buildDeactivatedPayload(usuario);
        statusPublisher.publish(email, payload);

        return ResponseEntity.ok(Map.of(
                UsuarioConstants.STATUS, UsuarioConstants.SUCCESS,
                UsuarioConstants.MESSAGE, "Usuario eliminado definitivamente de la base de datos."
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
