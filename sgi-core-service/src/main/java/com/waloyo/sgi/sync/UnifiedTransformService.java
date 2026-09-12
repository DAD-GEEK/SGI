package com.waloyo.sgi.sync;

import com.waloyo.sgi.entity.ClienteEntity;
import com.waloyo.sgi.entity.UsuarioEntity;
import com.waloyo.sgi.repository.ClienteRepository;
import com.waloyo.sgi.repository.UsuarioRepository;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import reactor.core.publisher.Mono;
import reactor.core.scheduler.Schedulers;

import java.util.Map;
import java.util.Optional;

@Service
@Slf4j
public class UnifiedTransformService {

    private final ClienteRepository clienteRepository;
    private final UsuarioRepository usuarioRepository;

    public UnifiedTransformService(ClienteRepository clienteRepository, UsuarioRepository usuarioRepository) {
        this.clienteRepository = clienteRepository;
        this.usuarioRepository = usuarioRepository;
    }

    public Mono<Void> transformAndUpsert(RawRecord record) {
        return Mono.fromRunnable(() -> {
            String table = record.getSourceTable().toLowerCase();

            if (table.contains("cliente") || table.contains("tercero")) {
                processCliente(record);
            } else if (table.contains("usuario") || table.contains("aspnetuser")) {
                processUsuario(record);
            }
        }).subscribeOn(Schedulers.boundedElastic()).then();
    }

    private void processCliente(RawRecord record) {
        Map<String, Object> data = record.getData();
        String nitRaw = getFirstNonNullString(data, "NIT", "Nit", "Documento", "Identificacion");
        if (nitRaw == null || !isValidNit(nitRaw)) {
            log.trace("[ETL-TRANSFORM] Cliente descartado por NIT ficticio o invalido: {}", nitRaw);
            return;
        }

        String nit = cleanNit(nitRaw);
        String razonSocial = getFirstNonNullString(data, "RazonSocial", "Nombre", "Empresa");
        if (razonSocial == null || razonSocial.trim().isEmpty()) {
            return;
        }

        String direccion = getFirstNonNullString(data, "Direccion", "Dir");
        String telefono = getFirstNonNullString(data, "Telefono", "Tel", "Celular");
        String email = getFirstNonNullString(data, "Correo", "Email", "EmailContacto");
        String contacto = getFirstNonNullString(data, "Contacto", "PersonaContacto", "RepresentanteLegal");

        Object opcEstado = data.get("OpcEstado");
        boolean activo = true;
        if (opcEstado != null) {
            String estadoStr = String.valueOf(opcEstado).trim();
            if ("0".equals(estadoStr) || "false".equalsIgnoreCase(estadoStr) || "Inactivo".equalsIgnoreCase(estadoStr)) {
                activo = false;
            }
        }

        Optional<ClienteEntity> optCliente = clienteRepository.findByNit(nit);
        ClienteEntity cliente;
        if (optCliente.isPresent()) {
            cliente = optCliente.get();
            cliente.setRazonSocial(razonSocial.trim());
            cliente.setDireccion(direccion);
            cliente.setTelefono(telefono);
            cliente.setEmailContacto(email);
            cliente.setPersonaContacto(contacto);
            cliente.setActivo(activo);
            log.debug("[ETL-TRANSFORM] Actualizando cliente existente: {} (NIT: {})", razonSocial, nit);
        } else {
            cliente = ClienteEntity.builder()
                .nit(nit)
                .razonSocial(razonSocial.trim())
                .direccion(direccion)
                .telefono(telefono)
                .emailContacto(email)
                .personaContacto(contacto)
                .activo(activo)
                .build();
            log.debug("[ETL-TRANSFORM] Creando nuevo cliente: {} (NIT: {})", razonSocial, nit);
        }

        clienteRepository.save(cliente);
    }

    private void processUsuario(RawRecord record) {
        Map<String, Object> data = record.getData();
        String email = getFirstNonNullString(data, "Email", "Correo", "UserName");
        if (email == null || !email.contains("@")) {
            return;
        }

        String docRaw = getFirstNonNullString(data, "Documento", "Cedula", "Nit", "Id");
        String doc = docRaw != null ? cleanNit(docRaw) : email;
        if (doc != null && doc.length() > 90) {
            doc = doc.substring(0, 90);
        }
        String nombre = getFirstNonNullString(data, "Nombre", "NombreCompleto", "Nombres");
        if (nombre == null || nombre.trim().isEmpty()) {
            nombre = email.split("@")[0];
        }

        Optional<UsuarioEntity> optUsuario = usuarioRepository.findByEmail(email.toLowerCase().trim());
        UsuarioEntity usuario;
        if (optUsuario.isPresent()) {
            usuario = optUsuario.get();
            usuario.setNombreCompleto(nombre.trim());
            log.debug("[ETL-TRANSFORM] Actualizando consultor existente: {}", email);
        } else {
            usuario = UsuarioEntity.builder()
                .documento(doc)
                .nombreCompleto(nombre.trim())
                .email(email.toLowerCase().trim())
                .rol("ASESOR_SENIOR")
                .activo(true)
                .build();
            log.debug("[ETL-TRANSFORM] Creando nuevo consultor: {}", email);
        }

        usuarioRepository.save(usuario);
    }

    private boolean isValidNit(String nitRaw) {
        String digits = nitRaw.replaceAll("\\D", "");
        if (digits.length() < 8) return false;
        return !digits.matches("^(0+|1+|9+|12345678|123456789)$");
    }

    private String cleanNit(String nitRaw) {
        return nitRaw.replaceAll("[^0-9a-zA-Z-]", "").trim();
    }

    private String getFirstNonNullString(Map<String, Object> map, String... keys) {
        for (String key : keys) {
            for (Map.Entry<String, Object> entry : map.entrySet()) {
                if (entry.getKey().equalsIgnoreCase(key) && entry.getValue() != null) {
                    String str = String.valueOf(entry.getValue()).trim();
                    if (!str.isEmpty() && !"null".equalsIgnoreCase(str)) {
                        return str;
                    }
                }
            }
        }
        return null;
    }
}
