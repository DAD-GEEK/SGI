package com.waloyo.sgi.controller;

import com.waloyo.sgi.dto.UsuarioDTO;
import com.waloyo.sgi.entity.UsuarioEntity;
import com.waloyo.sgi.repository.UsuarioRepository;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

import java.util.Map;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
public class UsuarioControllerTest {

    @Autowired
    private UsuarioController usuarioController;

    @Autowired
    private UsuarioRepository usuarioRepository;

    @Test
    @DisplayName("Debe verificar estado de un usuario existente o retornar activo por defecto")
    void testVerificarEstadoUsuario() {
        ResponseEntity<Map<String, Object>> response = usuarioController.verificarEstado("admon@waloyogroup.com");
        assertEquals(HttpStatus.OK, response.getStatusCode());
        assertNotNull(response.getBody());
        assertTrue(response.getBody().containsKey("activo"));
    }

    @Test
    @DisplayName("Debe registrar y actualizar asesor garantizando rol y persistencia")
    void testRegistrarYActualizarUsuario() {
        String testEmail = "test_unitario_" + System.currentTimeMillis() + "@waloyogroup.com";
        UsuarioDTO dto = new UsuarioDTO();
        dto.setEmail(testEmail);
        dto.setNombreCompleto("Consultor Test Automatizado");
        dto.setRol("CONSULTOR");
        dto.setModulosPermitidos("dashboard,clientes");
        dto.setDocumento("DOC-TEST-" + System.currentTimeMillis());

        ResponseEntity<Map<String, Object>> res = usuarioController.registrarUsuario(dto);
        assertEquals(HttpStatus.OK, res.getStatusCode());
        assertNotNull(res.getBody());
        assertEquals("SUCCESS", res.getBody().get("status"));

        Optional<UsuarioEntity> opt = usuarioRepository.findByEmail(testEmail);
        assertTrue(opt.isPresent(), "El usuario registrado debe existir en PostgreSQL");
        UsuarioEntity saved = opt.get();
        assertEquals("CONSULTOR", saved.getRol());

        // Limpieza de prueba
        usuarioRepository.delete(saved);
    }
}