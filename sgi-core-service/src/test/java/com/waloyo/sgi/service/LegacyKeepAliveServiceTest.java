package com.waloyo.sgi.service;

import com.waloyo.sgi.sync.SgiSyncProperties;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;

class LegacyKeepAliveServiceTest {

    @Test
    @DisplayName("Debe ejecutar pingLegacyApplications sin excepciones cuando keep-alive esta deshabilitado")
    void testPingLegacyApplicationsDisabled() {
        SgiSyncProperties properties = new SgiSyncProperties();
        properties.getLegacy().setKeepAliveEnabled(false);

        LegacyKeepAliveService service = new LegacyKeepAliveService(properties);

        assertDoesNotThrow(service::pingLegacyApplications);
    }

    @Test
    @DisplayName("Debe manejar URLs nulas o vacias sin lanzar excepciones")
    void testPingLegacyApplicationsEmptyUrls() {
        SgiSyncProperties properties = new SgiSyncProperties();
        properties.getLegacy().setKeepAliveEnabled(true);
        properties.getLegacy().setAgendaUrl(null);
        properties.getLegacy().setConsultorUrl("");

        LegacyKeepAliveService service = new LegacyKeepAliveService(properties);

        assertDoesNotThrow(service::pingLegacyApplications);
    }

    @Test
    @DisplayName("Debe despachar ping asincrono a URLs configuradas sin bloquear ni fallar")
    void testPingLegacyApplicationsConfigured() {
        SgiSyncProperties properties = new SgiSyncProperties();
        properties.getLegacy().setKeepAliveEnabled(true);
        properties.getLegacy().setAgendaUrl("http://localhost:9999/dummy-agenda");
        properties.getLegacy().setConsultorUrl("http://localhost:9999/dummy-consultor");

        LegacyKeepAliveService service = new LegacyKeepAliveService(properties);

        assertDoesNotThrow(service::pingLegacyApplications);
    }
}
