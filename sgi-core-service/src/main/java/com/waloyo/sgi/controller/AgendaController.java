package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.AgendaEventoEntity;
import com.waloyo.sgi.repository.AgendaEventoRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.http.codec.ServerSentEvent;
import org.springframework.web.bind.annotation.*;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Sinks;

import java.time.Duration;
import java.time.OffsetDateTime;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/agenda")
@RequiredArgsConstructor
public class AgendaController {

    private final AgendaEventoRepository agendaEventoRepository;
    private final Sinks.Many<AgendaEventoEntity> agendaSink = Sinks.many().multicast().onBackpressureBuffer();

    @GetMapping
    public ResponseEntity<List<AgendaEventoEntity>> listarTodos() {
        return ResponseEntity.ok(agendaEventoRepository.findAll());
    }

    @GetMapping(value = "/stream", produces = MediaType.TEXT_EVENT_STREAM_VALUE)
    public Flux<ServerSentEvent<List<AgendaEventoEntity>>> agendaStream() {
        return Flux.interval(Duration.ofSeconds(2))
                .map(seq -> ServerSentEvent.<List<AgendaEventoEntity>>builder()
                        .id(String.valueOf(seq))
                        .event("agenda-update")
                        .data(agendaEventoRepository.findAll())
                        .build());
    }

    @GetMapping("/{id}")
    public ResponseEntity<AgendaEventoEntity> obtenerPorId(@PathVariable UUID id) {
        return agendaEventoRepository.findById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }

    @GetMapping("/rango")
    public ResponseEntity<List<AgendaEventoEntity>> obtenerPorRango(
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) OffsetDateTime inicio,
            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) OffsetDateTime fin) {
        return ResponseEntity.ok(agendaEventoRepository.findByRangoFechas(inicio, fin));
    }

    @GetMapping("/asesor/{asesorId}")
    public ResponseEntity<List<AgendaEventoEntity>> obtenerPorAsesor(@PathVariable UUID asesorId) {
        return ResponseEntity.ok(agendaEventoRepository.findByAsesorId(asesorId));
    }

    @PostMapping
    public ResponseEntity<AgendaEventoEntity> crearEvento(@RequestBody AgendaEventoEntity evento) {
        AgendaEventoEntity saved = agendaEventoRepository.save(evento);
        agendaSink.tryEmitNext(saved);
        return ResponseEntity.ok(saved);
    }

    @PutMapping("/{id}")
    public ResponseEntity<AgendaEventoEntity> actualizarEvento(@PathVariable UUID id, @RequestBody AgendaEventoEntity eventoDetails) {
        return agendaEventoRepository.findById(id)
                .map(evento -> {
                    evento.setTitulo(eventoDetails.getTitulo());
                    evento.setDescripcion(eventoDetails.getDescripcion());
                    evento.setFechaInicio(eventoDetails.getFechaInicio());
                    evento.setFechaFin(eventoDetails.getFechaFin());
                    evento.setEstado(eventoDetails.getEstado());
                    AgendaEventoEntity updated = agendaEventoRepository.save(evento);
                    agendaSink.tryEmitNext(updated);
                    return ResponseEntity.ok(updated);
                })
                .orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> eliminarEvento(@PathVariable UUID id) {
        if (agendaEventoRepository.existsById(id)) {
            agendaEventoRepository.deleteById(id);
            return ResponseEntity.noContent().build();
        }
        return ResponseEntity.notFound().build();
    }
}
