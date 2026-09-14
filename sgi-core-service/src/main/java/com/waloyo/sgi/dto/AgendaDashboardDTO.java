package com.waloyo.sgi.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class AgendaDashboardDTO {
    private String id;
    private String titulo;
    private String cliente;
    private String descripcion;
    private String fecha;
    private String hora;
    private String duracion;
    private String asesorNombre;
    private String asesorEmail;
    private String tipoEvento;
    private String estado;
}
