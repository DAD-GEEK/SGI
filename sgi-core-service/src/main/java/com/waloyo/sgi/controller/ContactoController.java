package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.ContactoEntity;
import com.waloyo.sgi.repository.ContactoRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/contactos")
@RequiredArgsConstructor
public class ContactoController {

    private final ContactoRepository contactoRepository;

    @GetMapping
    public ResponseEntity<List<ContactoEntity>> listarTodos() {
        return ResponseEntity.ok(contactoRepository.findAll());
    }

    @GetMapping("/cliente/{clienteId}")
    public ResponseEntity<List<ContactoEntity>> listarPorCliente(@PathVariable UUID clienteId) {
        return ResponseEntity.ok(contactoRepository.findByClienteId(clienteId));
    }

    @PostMapping
    public ResponseEntity<ContactoEntity> crearContacto(@RequestBody ContactoEntity contacto) {
        return ResponseEntity.ok(contactoRepository.save(contacto));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> eliminarContacto(@PathVariable UUID id) {
        contactoRepository.deleteById(id);
        return ResponseEntity.noContent().build();
    }
}
