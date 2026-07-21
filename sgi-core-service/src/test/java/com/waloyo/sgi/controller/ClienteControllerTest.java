package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.ClienteEntity;
import com.waloyo.sgi.repository.ClienteRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
public class ClienteControllerTest {

    @Autowired
    private ClienteController clienteController;

    @Autowired
    private ClienteRepository clienteRepository;

    private ClienteEntity testCliente;

    @BeforeEach
    void setUp() {
        testCliente = new ClienteEntity();
        testCliente.setNit("999888777");
        testCliente.setRazonSocial("EMPRESA PRUEBA UNITARIA S.A.S.");
        testCliente.setTelefono("3109998877");
        testCliente.setEmailContacto("prueba@sgi.com");
        testCliente.setActivo(true);
    }

    @Test
    @DisplayName("Debe listar los 67 clientes reales desde el repositorio")
    void testObtenerTodosLosClientes() {
        ResponseEntity<List<ClienteEntity>> res = clienteController.listarTodos();
        assertEquals(HttpStatus.OK, res.getStatusCode());
        List<ClienteEntity> clientes = res.getBody();
        assertNotNull(clientes, "La lista de clientes no debe ser nula");
        assertTrue(clientes.size() >= 67, "Debe contener al menos los 67 clientes reales migrados");
    }

    @Test
    @DisplayName("Debe guardar un nuevo cliente y recuperar su ID")
    void testCrearCliente() {
        ResponseEntity<ClienteEntity> res = clienteController.crearCliente(testCliente);
        assertEquals(HttpStatus.OK, res.getStatusCode());
        ClienteEntity creado = res.getBody();
        assertNotNull(creado);
        assertNotNull(creado.getId(), "El ID del cliente creado no debe ser nulo");
        assertEquals("999888777", creado.getNit());
        
        // Limpieza de prueba
        clienteRepository.deleteById(creado.getId());
    }

    @Test
    @DisplayName("Debe desactivar un cliente cambiando su estado a inactivo")
    void testDesactivarCliente() {
        testCliente.setNit("999888778");
        ResponseEntity<ClienteEntity> res = clienteController.crearCliente(testCliente);
        ClienteEntity creado = res.getBody();
        assertNotNull(creado);

        ResponseEntity<Void> response = clienteController.eliminarCliente(creado.getId());
        assertEquals(HttpStatus.NO_CONTENT, response.getStatusCode());

        ClienteEntity actualizado = clienteRepository.findById(creado.getId()).orElse(null);
        assertNotNull(actualizado);
        assertFalse(actualizado.getActivo(), "El cliente debe estar en estado INACTIVO");

        // Limpieza
        clienteRepository.deleteById(creado.getId());
    }
}
