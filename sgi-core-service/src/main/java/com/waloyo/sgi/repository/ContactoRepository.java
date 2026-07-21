package com.waloyo.sgi.repository;

import com.waloyo.sgi.entity.ContactoEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.UUID;

@Repository
public interface ContactoRepository extends JpaRepository<ContactoEntity, UUID> {
    List<ContactoEntity> findByClienteId(UUID clienteId);
}
