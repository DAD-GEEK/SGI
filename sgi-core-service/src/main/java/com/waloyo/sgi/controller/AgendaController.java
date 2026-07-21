package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.AgendaEventoEntity;
import com.waloyo.sgi.repository.AgendaEventoRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.OffsetDateTime;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/agenda")
@RequiredArgsConstructor
public class AgendaController {

    private final AgendaEventoRepository agendaEventoRepository;

    @GetMapping
    public ResponseEntity<List<AgendaEventoEntity>> listarTodos() {
        return ResponseEntity.ok(agendaEventoRepository.findAll());
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

    @PostMapping
    public ResponseEntity<AgendaEventoEntity> crearEvento(@RequestBody AgendaEventoEntity evento) {
        return ResponseEntity.ok(agendaEventoRepository.save(evento));
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
                    return ResponseEntity.ok(agendaEventoRepository.save(evento));
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
