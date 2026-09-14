package com.waloyo.sgi.controller;

import com.waloyo.sgi.entity.SyncWatermarkEntity;
import com.waloyo.sgi.repository.SyncWatermarkRepository;
import com.waloyo.sgi.sync.DatabaseSyncScheduler;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import reactor.core.publisher.Mono;

import java.time.OffsetDateTime;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("/api/sync")
public class SyncController {

    private final DatabaseSyncScheduler syncScheduler;
    private final SyncWatermarkRepository watermarkRepository;

    public SyncController(DatabaseSyncScheduler syncScheduler, SyncWatermarkRepository watermarkRepository) {
        this.syncScheduler = syncScheduler;
        this.watermarkRepository = watermarkRepository;
    }

    @GetMapping("/status")
    public ResponseEntity<Map<String, Object>> getSyncStatus() {
        Map<String, Object> response = new HashMap<>();
        List<SyncWatermarkEntity> watermarks = watermarkRepository.findAll();

        response.put("timestamp", OffsetDateTime.now());
        response.put("isSyncRunning", syncScheduler.isRunning());
        response.put("totalTablasAuditadas", watermarks.size());
        response.put("watermarks", watermarks);

        return ResponseEntity.ok(response);
    }

    @PostMapping("/trigger")
    public Mono<ResponseEntity<Map<String, Object>>> triggerManualSync() {
        return syncScheduler.triggerManualSync()
                .map(started -> {
                    Map<String, Object> response = new HashMap<>();
                    response.put("timestamp", OffsetDateTime.now());
                    if (started) {
                        response.put("status", "INICIADA");
                        response.put("mensaje", "La sincronización reactiva ha comenzado en segundo plano con política de reintentos.");
                    } else {
                        response.put("status", "EN_CURSO");
                        response.put("mensaje", "Ya existe una sincronización en curso. Espera a que finalice.");
                    }
                    return ResponseEntity.ok(response);
                });
    }
}
