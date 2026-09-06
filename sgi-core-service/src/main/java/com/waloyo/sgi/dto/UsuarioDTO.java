package com.waloyo.sgi.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.UUID;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class UsuarioDTO {
    private UUID id;
    private String email;
    private String nombreCompleto;
    private String tipoDocumento;
    private String documento;
    private String pais;
    private String telefonoMovil;
    private Boolean activo;
    private Boolean mustChangePassword;
    private String rol;
    private String modulosPermitidos;
}

