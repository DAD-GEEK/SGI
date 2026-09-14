package com.waloyo.sgi.controller;

import com.waloyo.sgi.config.CorsConfig;
import com.waloyo.sgi.dto.AgendaDashboardDTO;
import com.waloyo.sgi.repository.AgendaEventoRepository;
import com.waloyo.sgi.service.LegacyAgendaQueryService;
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

@WebMvcTest(AgendaController.class)
@Import(CorsConfig.class)
class AgendaControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private AgendaEventoRepository agendaEventoRepository;

    @MockBean
    private LegacyAgendaQueryService legacyAgendaQueryService;

    @Test
    @DisplayName("Debe consultar proximas asesorias para el dashboard exitosamente")
    void testObtenerAgendaDashboard() throws Exception {
        AgendaDashboardDTO eventoMock = AgendaDashboardDTO.builder()
                .id("1001")
                .titulo("Auditoria SG-SST")
                .cliente("Transportes del Norte S.A.")
                .fecha("15 de Septiembre")
                .hora("09:00 AM")
                .duracion("3 Horas")
                .asesorNombre("Milena Valencia")
                .asesorEmail("samy1727@hotmail.com")
                .tipoEvento("Asesoria Especializada")
                .estado("PROGRAMADO")
                .build();

        when(legacyAgendaQueryService.obtenerProximasAsesorias(any(), anyInt(), any()))
                .thenReturn(List.of(eventoMock));

        mockMvc.perform(get("/api/agenda/dashboard")
                        .param("email", "samy1727@hotmail.com")
                        .param("limit", "5")
                        .header("Origin", "https://crm.gestionintegralsgi.com.co"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$[0].id").value("1001"))
                .andExpect(jsonPath("$[0].titulo").value("Auditoria SG-SST"))
                .andExpect(jsonPath("$[0].cliente").value("Transportes del Norte S.A."))
                .andExpect(jsonPath("$[0].asesorNombre").value("Milena Valencia"));
    }
}
