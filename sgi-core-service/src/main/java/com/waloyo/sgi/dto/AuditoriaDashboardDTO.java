package com.waloyo.sgi.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class AuditoriaDashboardDTO {
    private String id;
    private String codigo;
    private String cliente;
    private String norma;
    private String estado;
    private String auditor;
    private String fechaInicio;
    private String fechaFin;
    private boolean firmada;
}
