package com.waloyo.sgi.repository;

import com.waloyo.sgi.entity.ContratoEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.UUID;

@Repository
public interface ContratoRepository extends JpaRepository<ContratoEntity, UUID> {
    List<ContratoEntity> findByClienteId(UUID clienteId);
}
