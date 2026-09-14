package com.waloyo.sgi.controller;

import com.waloyo.sgi.config.CorsConfig;
import com.waloyo.sgi.dto.AuditoriaDashboardDTO;
import com.waloyo.sgi.dto.DashboardKpisDTO;
import com.waloyo.sgi.service.LegacyDashboardMetricsService;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.context.annotation.Import;
import org.springframework.test.web.servlet.MockMvc;

import java.util.List;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyInt;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest(DashboardMetricsController.class)
@Import(CorsConfig.class)
class DashboardMetricsControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private LegacyDashboardMetricsService dashboardMetricsService;

    @Test
    @DisplayName("Debe consultar KPIs operativos del dashboard exitosamente")
    void testObtenerKpis() throws Exception {
        DashboardKpisDTO kpisMock = DashboardKpisDTO.builder()
                .horasEjecutadasMes(35.5)
                .horasContratadasMes(40.0)
                .porcentajeEjecucionHoras(88.8)
                .compromisosPendientes(4)
                .compromisosVencidos(1)
                .auditoriasEnCurso(5)
                .auditoriasPendientesFirma(2)
                .planesAccionPendientes(3)
                .diagnosticosEnProceso(2)
                .build();

        when(dashboardMetricsService.obtenerKpis(any(), any())).thenReturn(kpisMock);

        mockMvc.perform(get("/api/dashboard/kpis")
                        .param("email", "admon@waloyogroup.com")
                        .header("Origin", "https://crm.gestionintegralsgi.com.co"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.horasEjecutadasMes").value(35.5))
                .andExpect(jsonPath("$.porcentajeEjecucionHoras").value(88.8))
                .andExpect(jsonPath("$.compromisosPendientes").value(4))
                .andExpect(jsonPath("$.auditoriasEnCurso").value(5));
    }

    @Test
    @DisplayName("Debe consultar lista de auditorias recientes para la tabla del dashboard")
    void testObtenerAuditorias() throws Exception {
        AuditoriaDashboardDTO audMock = AuditoriaDashboardDTO.builder()
                .id("201")
                .codigo("AUD-2026-01")
                .cliente("Transportes del Norte S.A.")
                .norma("SG-SST (Res. 0312)")
                .estado("En Ejecucion")
                .auditor("Laura Tenorio")
                .fechaInicio("10 de Septiembre")
                .fechaFin("15 de Septiembre")
                .firmada(false)
                .build();

        when(dashboardMetricsService.obtenerAuditoriasRecientes(any(), anyInt(), any())).thenReturn(List.of(audMock));

        mockMvc.perform(get("/api/dashboard/auditorias")
                        .param("limit", "6")
                        .param("soloMias", "false")
                        .header("Origin", "https://crm.gestionintegralsgi.com.co"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$[0].id").value("201"))
                .andExpect(jsonPath("$[0].cliente").value("Transportes del Norte S.A."))
                .andExpect(jsonPath("$[0].norma").value("SG-SST (Res. 0312)"))
                .andExpect(jsonPath("$[0].estado").value("En Ejecucion"));
    }
}
