package com.waloyo.sgi.repository;

import com.waloyo.sgi.entity.SyncWatermarkEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;
import java.util.UUID;

@Repository
public interface SyncWatermarkRepository extends JpaRepository<SyncWatermarkEntity, UUID> {
    Optional<SyncWatermarkEntity> findByOrigenDbAndTablaOrigen(String origenDb, String tablaOrigen);
}
