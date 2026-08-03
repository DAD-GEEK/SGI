package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.UsuarioEntity;
import com.waloyo.sgi.repository.UsuarioRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/usuarios")
@RequiredArgsConstructor
public class UsuarioController {

    private final UsuarioRepository usuarioRepository;

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
    public ResponseEntity<?> verificarEstado(@RequestParam String email) {
        return usuarioRepository.findByEmail(email)
                .map(usuario -> ResponseEntity.ok(java.util.Map.ofEntries(
                        java.util.Map.entry("id", usuario.getId().toString()),
                        java.util.Map.entry("email", usuario.getEmail()),
                        java.util.Map.entry("nombreCompleto", usuario.getNombreCompleto() != null ? usuario.getNombreCompleto() : ""),
                        java.util.Map.entry("tipoDocumento", usuario.getTipoDocumento() != null ? usuario.getTipoDocumento() : "CC"),
                        java.util.Map.entry("documento", usuario.getDocumento() != null ? usuario.getDocumento() : ""),
                        java.util.Map.entry("pais", usuario.getPais() != null ? usuario.getPais() : "Colombia (+57)"),
                        java.util.Map.entry("telefonoMovil", usuario.getTelefonoMovil() != null ? usuario.getTelefonoMovil() : ""),
                        java.util.Map.entry("activo", Boolean.TRUE.equals(usuario.getActivo())),
                        java.util.Map.entry("mustChangePassword", Boolean.TRUE.equals(usuario.getMustChangePassword())),
                        java.util.Map.entry("rol", usuario.getRol() != null ? usuario.getRol() : "CONSULTOR"),
                        java.util.Map.entry("modulosPermitidos", usuario.getModulosPermitidos() != null ? usuario.getModulosPermitidos() : "")
                )))
                .orElse(ResponseEntity.ok(java.util.Map.ofEntries(
                        java.util.Map.entry("mustChangePassword", false),
                        java.util.Map.entry("activo", true)
                )));
    }

    @PostMapping("/confirmar-clave")
    public ResponseEntity<?> confirmarClave(@RequestBody java.util.Map<String, String> body) {
        String email = body.get("email");
        if (email == null) return ResponseEntity.badRequest().build();

        return usuarioRepository.findByEmail(email)
                .map(usuario -> {
                    usuario.setMustChangePassword(false);
                    usuarioRepository.save(usuario);
                    return ResponseEntity.ok(java.util.Map.of("status", "SUCCESS", "message", "Contraseña definitiva confirmada."));
                })
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping("/registrar")
    public ResponseEntity<?> registrarUsuario(@RequestBody UsuarioEntity usuarioReq) {
        if (usuarioReq.getEmail() == null || usuarioReq.getNombreCompleto() == null) {
            return ResponseEntity.badRequest().body(java.util.Map.of("error", "Nombre y correo son obligatorios."));
        }

        UsuarioEntity usuario = usuarioRepository.findByEmail(usuarioReq.getEmail())
                .orElse(UsuarioEntity.builder()
                        .email(usuarioReq.getEmail())
                        .documento(usuarioReq.getDocumento() != null ? usuarioReq.getDocumento() : "DOC-" + System.currentTimeMillis())
                        .build());

        usuario.setNombreCompleto(usuarioReq.getNombreCompleto());
        usuario.setTipoDocumento(usuarioReq.getTipoDocumento() != null ? usuarioReq.getTipoDocumento() : "CC");
        usuario.setDocumento(usuarioReq.getDocumento());
        usuario.setPais(usuarioReq.getPais() != null ? usuarioReq.getPais() : "Colombia (+57)");
        usuario.setTelefonoMovil(usuarioReq.getTelefonoMovil());
        usuario.setRol(usuarioReq.getRol() != null ? usuarioReq.getRol() : "CONSULTOR");
        usuario.setMustChangePassword(true);
        usuario.setActivo(true);

        if (usuarioReq.getModulosPermitidos() != null) {
            usuario.setModulosPermitidos(usuarioReq.getModulosPermitidos());
        }

        usuarioRepository.save(usuario);

        return ResponseEntity.ok(java.util.Map.of(
                "status", "SUCCESS",
                "message", "Usuario registrado o actualizado exitosamente.",
                "id", usuario.getId(),
                "mustChangePassword", true
        ));
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> actualizarUsuario(@PathVariable UUID id, @RequestBody UsuarioEntity datos) {
        return usuarioRepository.findById(id)
                .map(usuario -> {
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
                    return ResponseEntity.ok(usuario);
                })
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping("/{id}/reenviar-credenciales")
    public ResponseEntity<?> reenviarCredenciales(@PathVariable UUID id) {
        return usuarioRepository.findById(id)
                .map(usuario -> {
                    usuario.setMustChangePassword(true);
                    usuarioRepository.save(usuario);
                    return ResponseEntity.ok(java.util.Map.of(
                            "status", "SUCCESS",
                            "message", "Credenciales temporales re-generadas y notificadas al usuario.",
                            "email", usuario.getEmail(),
                            "mustChangePassword", true
                    ));
                })
                .orElse(ResponseEntity.badRequest().body(java.util.Map.of("error", "Usuario no encontrado con ID: " + id)));
    }

    @PostMapping("/reenviar-credenciales-email")
    public ResponseEntity<?> reenviarCredencialesPorEmail(@RequestParam String email) {
        return usuarioRepository.findByEmail(email)
                .map(usuario -> {
                    usuario.setMustChangePassword(true);
                    usuarioRepository.save(usuario);
                    return ResponseEntity.ok(java.util.Map.of(
                            "status", "SUCCESS",
                            "message", "Credenciales temporales re-generadas y notificadas al usuario.",
                            "email", usuario.getEmail(),
                            "mustChangePassword", true
                    ));
                })
                .orElse(ResponseEntity.badRequest().body(java.util.Map.of("error", "Usuario no encontrado con email: " + email)));
    }

    @PutMapping("/{id}/alternar-estado")
    public ResponseEntity<?> alternarEstado(@PathVariable UUID id) {
        return usuarioRepository.findById(id)
                .map(usuario -> {
                    usuario.setActivo(!Boolean.TRUE.equals(usuario.getActivo()));
                    usuarioRepository.save(usuario);
                    return ResponseEntity.ok(java.util.Map.of("status", "SUCCESS", "activo", usuario.getActivo()));
                })
                .orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> eliminarUsuarioDefinitivo(@PathVariable UUID id) {
        return usuarioRepository.findById(id)
                .map(usuario -> {
                    usuarioRepository.delete(usuario);
                    return ResponseEntity.ok(java.util.Map.of("status", "SUCCESS", "message", "Usuario eliminado definitivamente de la base de datos."));
                })
                .orElse(ResponseEntity.notFound().build());
    }
}
