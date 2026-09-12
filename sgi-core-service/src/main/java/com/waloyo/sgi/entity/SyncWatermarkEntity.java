package com.waloyo.sgi.entity;

import jakarta.persistence.*;
import lombok.*;
import java.time.OffsetDateTime;
import java.util.UUID;

@Entity
@Table(name = "sync_watermarks", schema = "sgi")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class SyncWatermarkEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private UUID id;

    @Column(name = "origen_db", nullable = false, length = 100)
    private String origenDb;

    @Column(name = "tabla_origen", nullable = false, length = 100)
    private String tablaOrigen;

    @Column(name = "ultima_sincronizacion_exitosa")
    private OffsetDateTime ultimaSincronizacionExitosa;

    @Column(name = "estado", nullable = false, length = 30)
    @Builder.Default
    private String estado = "PENDIENTE";

    @Column(name = "registros_procesados")
    @Builder.Default
    private Integer registrosProcesados = 0;

    @Column(name = "reintentos_ejecutados")
    @Builder.Default
    private Integer reintentosEjecutados = 0;

    @Column(name = "mensaje_error", columnDefinition = "TEXT")
    private String mensajeError;

    @Column(name = "actualizado_en")
    private OffsetDateTime actualizadoEn;

    @PrePersist
    @PreUpdate
    public void onUpdate() {
        this.actualizadoEn = OffsetDateTime.now();
    }
}
