package com.waloyo.sgi.sync;

import com.waloyo.sgi.entity.SyncWatermarkEntity;
import com.waloyo.sgi.repository.SyncWatermarkRepository;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.event.EventListener;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.time.OffsetDateTime;
import java.util.Arrays;
import java.util.List;
import java.util.concurrent.atomic.AtomicBoolean;
import java.util.concurrent.atomic.AtomicInteger;

@Service
@Slf4j
public class DatabaseSyncScheduler {

    private final SgiSyncProperties syncProperties;
    private final MssqlExtractionService extractionService;
    private final RawMirrorService mirrorService;
    private final UnifiedTransformService transformService;
    private final SyncWatermarkRepository watermarkRepository;

    private final AtomicBoolean isSyncRunning = new AtomicBoolean(false);

    public DatabaseSyncScheduler(
            SgiSyncProperties syncProperties,
            MssqlExtractionService extractionService,
            RawMirrorService mirrorService,
            UnifiedTransformService transformService,
            SyncWatermarkRepository watermarkRepository) {
        this.syncProperties = syncProperties;
        this.extractionService = extractionService;
        this.mirrorService = mirrorService;
        this.transformService = transformService;
        this.watermarkRepository = watermarkRepository;
    }

    @EventListener(ApplicationReadyEvent.class)
    public void onStartup() {
        log.info("🚀 [SGI-SYNC-SCHEDULER] Motor de Sincronización Reactiva Inicializado.");
        log.info("⏱️ [SGI-SYNC-SCHEDULER] Configuración -> Cron: {}, MaxAttempts: {}, DelayMs: {} ms",
                syncProperties.getSync().getCron(),
                syncProperties.getSync().getRetry().getMaxAttempts(),
                syncProperties.getSync().getRetry().getDelayMs());
    }

    @Scheduled(cron = "${sgi.sync.cron:0 0 */1 * * *}")
    public void runScheduledSync() {
        if (!syncProperties.getSync().isEnabled()) {
            log.info("⏸️ [SGI-SYNC-SCHEDULER] Sincronización desactivada por configuración (sgi.sync.enabled=false).");
            return;
        }

        if (!isSyncRunning.compareAndSet(false, true)) {
            log.warn("⚠️ [SGI-SYNC-SCHEDULER] Ya existe una sincronización en curso. Omitiendo ejecución concurrente.");
            return;
        }

        executeSyncWithRetry()
                .doFinally(signalType -> isSyncRunning.set(false))
                .subscribe(
                        success -> log.info("🏁 [SGI-SYNC-SCHEDULER] Ciclo de sincronización finalizado exitosamente."),
                        error -> log.error("🚨 [SGI-SYNC-SCHEDULER] Ciclo de sincronización finalizado con error definitivo: {}", error.getMessage())
                );
    }

    public Mono<Boolean> triggerManualSync() {
        if (!isSyncRunning.compareAndSet(false, true)) {
            return Mono.just(false);
        }

        return executeSyncWithRetry()
                .doFinally(signalType -> isSyncRunning.set(false))
                .thenReturn(true);
    }

    public boolean isRunning() {
        return isSyncRunning.get();
    }

    private Mono<Void> executeSyncWithRetry() {
        int maxAttempts = syncProperties.getSync().getRetry().getMaxAttempts();
        long delayMs = syncProperties.getSync().getRetry().getDelayMs();

        return executeFullSyncPipeline()
                .retryWhen(reactor.util.retry.Retry.fixedDelay(maxAttempts, java.time.Duration.ofMillis(delayMs))
                        .doBeforeRetry(retrySignal -> {
                            log.warn("🔄 [SGI-SYNC-RETRY] Intento fallido #{} de {}. Reintentando en {} ms. Error: {}",
                                    retrySignal.totalRetries() + 1, maxAttempts, delayMs, retrySignal.failure().getMessage());
                        })
                )
                .onErrorResume(e -> {
                    log.error("💥 [SGI-SYNC-FATAL] Se agotaron los {} reintentos. La sincronización se suspende hasta el próximo ciclo cron. Error: {}",
                            maxAttempts, e.getMessage());
                    return Mono.empty();
                });
    }

    private Mono<Void> executeFullSyncPipeline() {
        SgiSyncProperties.Mssql mssqlConfig = syncProperties.getDatasource().getMssql();

        List<SyncTarget> targets = Arrays.asList(
                new SyncTarget(mssqlConfig.getDbAgenda(), "Clientes"),
                new SyncTarget(mssqlConfig.getDbAgenda(), "Contratos"),
                new SyncTarget(mssqlConfig.getDbAgenda(), "Agenda"),
                new SyncTarget(mssqlConfig.getDbAgenda(), "Usuarios"),
                new SyncTarget(mssqlConfig.getDbConsultor(), "Terceros"),
                new SyncTarget(mssqlConfig.getDbConsultor(), "AspNetUsers"),
                new SyncTarget(mssqlConfig.getDbConsultor(), "Auditorias")
        );

        return Flux.fromIterable(targets)
                .concatMap(this::syncSingleTable)
                .then();
    }

    private Mono<Void> syncSingleTable(SyncTarget target) {
        return Mono.<Void>defer(() -> {
            SyncWatermarkEntity watermarkEntity = watermarkRepository
                    .findByOrigenDbAndTablaOrigen(target.db, target.table)
                    .orElseGet(() -> SyncWatermarkEntity.builder()
                            .origenDb(target.db)
                            .tablaOrigen(target.table)
                            .estado("PENDIENTE")
                            .build());

            OffsetDateTime lastSync = watermarkEntity.getUltimaSincronizacionExitosa();
            AtomicInteger processedCount = new AtomicInteger(0);
            AtomicBoolean schemaInitialized = new AtomicBoolean(false);

            log.info("⏳ [SGI-SYNC] Iniciando tabla {}.{} (Watermark: {})", target.db, target.table, lastSync);

            return extractionService.extractIncremental(target.db, target.table, lastSync)
                    .concatMap(rawRecord -> {
                        Mono<Void> ensureSchemaMono = Mono.empty();
                        if (!schemaInitialized.get()) {
                            ensureSchemaMono = mirrorService.ensureRawSchemaAndTable(target.db, target.table, rawRecord.getData())
                                    .doOnSuccess(v -> schemaInitialized.set(true));
                        }

                        return ensureSchemaMono
                                .then(mirrorService.insertRawRecord(rawRecord))
                                .then(transformService.transformAndUpsert(rawRecord))
                                .doOnSuccess(v -> processedCount.incrementAndGet());
                    })
                    .then(Mono.<Void>fromRunnable(() -> {
                        watermarkEntity.setUltimaSincronizacionExitosa(OffsetDateTime.now());
                        watermarkEntity.setEstado("EXITOSO");
                        watermarkEntity.setRegistrosProcesados(processedCount.get());
                        watermarkEntity.setMensajeError(null);
                        watermarkRepository.save(watermarkEntity);
                        log.info("🎉 [SGI-SYNC] Finalizada tabla {}.{} -> {} registros procesados.",
                                target.db, target.table, processedCount.get());
                    }))
                    .onErrorResume(e -> {
                        watermarkEntity.setEstado("FALLIDO");
                        watermarkEntity.setMensajeError(e.getMessage());
                        watermarkRepository.save(watermarkEntity);
                        log.error("❌ [SGI-SYNC] Error en tabla {}.{}: {}", target.db, target.table, e.getMessage());
                        return Mono.<Void>error(e);
                    });
        });
    }

    private static class SyncTarget {
        final String db;
        final String table;

        SyncTarget(String db, String table) {
            this.db = db;
            this.table = table;
        }
    }
}
