package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.ContratoEntity;
import com.waloyo.sgi.repository.ContratoRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/contratos")
@RequiredArgsConstructor
public class ContratoController {

    private final ContratoRepository contratoRepository;

    @GetMapping
    public ResponseEntity<List<ContratoEntity>> listarTodos() {
        return ResponseEntity.ok(contratoRepository.findAll());
    }

    @GetMapping("/cliente/{clienteId}")
    public ResponseEntity<List<ContratoEntity>> listarPorCliente(@PathVariable UUID clienteId) {
        return ResponseEntity.ok(contratoRepository.findByClienteId(clienteId));
    }

    @PostMapping
    public ResponseEntity<ContratoEntity> crearContrato(@RequestBody ContratoEntity contrato) {
        if (contrato.getActivo() == null) {
            contrato.setActivo(true);
        }
        return ResponseEntity.ok(contratoRepository.save(contrato));
    }
}
