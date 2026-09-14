package com.waloyo.sgi.controller;

import com.waloyo.sgi.config.CorsConfig;
import com.waloyo.sgi.repository.SyncWatermarkRepository;
import com.waloyo.sgi.sync.DatabaseSyncScheduler;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.context.annotation.Import;
import org.springframework.test.web.servlet.MockMvc;
import reactor.core.publisher.Mono;

import java.util.Collections;

import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest(SyncController.class)
@Import(CorsConfig.class)
class SyncControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private DatabaseSyncScheduler syncScheduler;

    @MockBean
    private SyncWatermarkRepository watermarkRepository;

    @Test
    @DisplayName("Debe disparar sincronizacion manual sin errores de CORS ni excepciones")
    void testTriggerManualSyncSuccess() throws Exception {
        when(syncScheduler.triggerManualSync()).thenReturn(Mono.just(true));

        var mvcResult = mockMvc.perform(post("/api/sync/trigger")
                        .header("Origin", "https://sgi-crm.web.app"))
                .andReturn();

        mockMvc.perform(org.springframework.test.web.servlet.request.MockMvcRequestBuilders.asyncDispatch(mvcResult))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.status").value("INICIADA"))
                .andExpect(jsonPath("$.mensaje").exists());
    }

    @Test
    @DisplayName("Debe responder EN_CURSO si ya hay sincronizacion activa")
    void testTriggerManualSyncAlreadyRunning() throws Exception {
        when(syncScheduler.triggerManualSync()).thenReturn(Mono.just(false));

        var mvcResult = mockMvc.perform(post("/api/sync/trigger")
                        .header("Origin", "https://app.gestionintegralsgi.com.co"))
                .andReturn();

        mockMvc.perform(org.springframework.test.web.servlet.request.MockMvcRequestBuilders.asyncDispatch(mvcResult))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.status").value("EN_CURSO"));
    }

    @Test
    @DisplayName("Debe consultar estado de sincronizacion")
    void testGetSyncStatus() throws Exception {
        when(watermarkRepository.findAll()).thenReturn(Collections.emptyList());
        when(syncScheduler.isRunning()).thenReturn(false);

        mockMvc.perform(get("/api/sync/status"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.isSyncRunning").value(false))
                .andExpect(jsonPath("$.totalTablasAuditadas").value(0));
    }
}
