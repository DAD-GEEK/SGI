package com.waloyo.sgi.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class DashboardKpisDTO {
    // Clientes asignados a cargo del consultor (o global si es admin)
    private long clientesAsignados;

    // Horas de asesoria del mes actual (dbo.Agenda vs dbo.Contratos)
    private double horasEjecutadasMes;
    private double horasContratadasMes;
    private double porcentajeEjecucionHoras;

    // Compromisos y tareas derivadas de actas (dbo.ActividadesActa)
    private long compromisosPendientes;
    private long compromisosVencidos;

    // Auditorias de sistemas de gestion (dbo.Auditorias)
    private long auditoriasEnCurso;
    private long auditoriasPendientesFirma;

    // Acciones correctivas y preventivas (dbo.AuditoriasDetalleAC)
    private long planesAccionPendientes;

    // Diagnosticos iniciales en evaluacion (dbo.DocumentosDiagnostico)
    private long diagnosticosEnProceso;
}
