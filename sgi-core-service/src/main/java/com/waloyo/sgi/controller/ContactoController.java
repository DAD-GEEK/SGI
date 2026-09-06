package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.ContactoEntity;
import com.waloyo.sgi.repository.ContactoRepository;
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
@RequestMapping("/api/contactos")
@RequiredArgsConstructor
public class ContactoController {

    private final ContactoRepository contactoRepository;
    private final Sinks.Many<ContactoEntity> contactoSink = Sinks.many().multicast().onBackpressureBuffer();

    @GetMapping
    public ResponseEntity<List<ContactoEntity>> listarTodos() {
        return ResponseEntity.ok(contactoRepository.findAll());
    }

    @GetMapping(value = "/stream", produces = MediaType.TEXT_EVENT_STREAM_VALUE)
    public Flux<ServerSentEvent<List<ContactoEntity>>> contactoStream() {
        return Flux.interval(Duration.ofSeconds(2))
                .map(seq -> ServerSentEvent.<List<ContactoEntity>>builder()
                        .id(String.valueOf(seq))
                        .event("contactos-update")
                        .data(contactoRepository.findAll())
                        .build());
    }

    @GetMapping("/cliente/{clienteId}")
    public ResponseEntity<List<ContactoEntity>> listarPorCliente(@PathVariable UUID clienteId) {
        return ResponseEntity.ok(contactoRepository.findByClienteId(clienteId));
    }

    @PostMapping
    public ResponseEntity<ContactoEntity> crearContacto(@RequestBody ContactoEntity contacto) {
        ContactoEntity saved = contactoRepository.save(contacto);
        contactoSink.tryEmitNext(saved);
        return ResponseEntity.ok(saved);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> eliminarContacto(@PathVariable UUID id) {
        contactoRepository.deleteById(id);
        return ResponseEntity.noContent().build();
    }
}
