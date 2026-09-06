package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.ContratoEntity;
import com.waloyo.sgi.repository.ContratoRepository;
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
@RequestMapping("/api/contratos")
@RequiredArgsConstructor
public class ContratoController {

    private final ContratoRepository contratoRepository;
    private final Sinks.Many<ContratoEntity> contratoSink = Sinks.many().multicast().onBackpressureBuffer();

    @GetMapping
    public ResponseEntity<List<ContratoEntity>> listarTodos() {
        return ResponseEntity.ok(contratoRepository.findAll());
    }

    @GetMapping(value = "/stream", produces = MediaType.TEXT_EVENT_STREAM_VALUE)
    public Flux<ServerSentEvent<List<ContratoEntity>>> contratoStream() {
        return Flux.interval(Duration.ofSeconds(2))
                .map(seq -> ServerSentEvent.<List<ContratoEntity>>builder()
                        .id(String.valueOf(seq))
                        .event("contratos-update")
                        .data(contratoRepository.findAll())
                        .build());
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
        ContratoEntity saved = contratoRepository.save(contrato);
        contratoSink.tryEmitNext(saved);
        return ResponseEntity.ok(saved);
    }
}
