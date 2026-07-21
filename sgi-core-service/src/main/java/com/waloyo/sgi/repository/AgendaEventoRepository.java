package com.waloyo.sgi.repository;

import com.waloyo.sgi.entity.AgendaEventoEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.time.OffsetDateTime;
import java.util.List;
import java.util.UUID;

@Repository
public interface AgendaEventoRepository extends JpaRepository<AgendaEventoEntity, UUID> {

    List<AgendaEventoEntity> findByClienteId(UUID clienteId);

    List<AgendaEventoEntity> findByAsesorId(UUID asesorId);

    @Query("SELECT a FROM AgendaEventoEntity a WHERE a.fechaInicio >= :inicio AND a.fechaFin <= :fin ORDER BY a.fechaInicio DESC")
    List<AgendaEventoEntity> findByRangoFechas(@Param("inicio") OffsetDateTime inicio, @Param("fin") OffsetDateTime fin);
}
