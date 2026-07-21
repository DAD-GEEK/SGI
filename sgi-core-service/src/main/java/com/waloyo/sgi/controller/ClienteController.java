package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.ClienteEntity;
import com.waloyo.sgi.repository.ClienteRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.http.codec.ServerSentEvent;
import org.springframework.web.bind.annotation.*;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Sinks;

import java.time.Duration;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/clientes")
@RequiredArgsConstructor
public class ClienteController {

    private final ClienteRepository clienteRepository;
    
    private final Sinks.Many<ClienteEntity> clienteSink = Sinks.many().multicast().onBackpressureBuffer();

    @GetMapping
    public ResponseEntity<List<ClienteEntity>> listarTodos() {
        return ResponseEntity.ok(clienteRepository.findAll());
    }

    // Explicit stream mapping placed BEFORE /{id} to avoid UUID path mismatch
    @GetMapping(value = "/stream", produces = MediaType.TEXT_EVENT_STREAM_VALUE)
    public Flux<ServerSentEvent<List<ClienteEntity>>> clienteStream() {
        return Flux.interval(Duration.ofSeconds(2))
                .map(sequence -> ServerSentEvent.<List<ClienteEntity>>builder()
                        .id(String.valueOf(sequence))
                        .event("clientes-update")
                        .data(clienteRepository.findAll())
                        .build());
    }

    @GetMapping("/{id}")
    public ResponseEntity<ClienteEntity> obtenerPorId(@PathVariable UUID id) {
        return clienteRepository.findById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping
    public ResponseEntity<ClienteEntity> crearCliente(@RequestBody ClienteEntity cliente) {
        if (cliente.getActivo() == null) {
            cliente.setActivo(true);
        }
        ClienteEntity saved = clienteRepository.save(cliente);
        clienteSink.tryEmitNext(saved);
        return ResponseEntity.ok(saved);
    }

    @PutMapping("/{id}")
    public ResponseEntity<ClienteEntity> actualizarCliente(@PathVariable UUID id, @RequestBody ClienteEntity detalles) {
        return clienteRepository.findById(id)
                .map(c -> {
                    c.setNit(detalles.getNit());
                    c.setRazonSocial(detalles.getRazonSocial());
                    c.setNombreComercial(detalles.getNombreComercial());
                    c.setDireccion(detalles.getDireccion());
                    c.setTelefono(detalles.getTelefono());
                    c.setEmailContacto(detalles.getEmailContacto());
                    c.setPersonaContacto(detalles.getPersonaContacto());
                    if (detalles.getActivo() != null) {
                        c.setActivo(detalles.getActivo());
                    }
                    ClienteEntity updated = clienteRepository.save(c);
                    clienteSink.tryEmitNext(updated);
                    return ResponseEntity.ok(updated);
                })
                .orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> eliminarCliente(@PathVariable UUID id) {
        return clienteRepository.findById(id)
                .map(c -> {
                    c.setActivo(false);
                    ClienteEntity deactivated = clienteRepository.save(c);
                    clienteSink.tryEmitNext(deactivated);
                    return ResponseEntity.noContent().<Void>build();
                })
                .orElse(ResponseEntity.notFound().build());
    }
}
