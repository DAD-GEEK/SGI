package com.waloyo.sgi.sync;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class SgiSyncPropertiesTest {

    @Test
    @DisplayName("Debe construir la URL JDBC correctamente y mapear defaults")
    void testBuildJdbcUrlAndDefaults() {
        SgiSyncProperties properties = new SgiSyncProperties();
        assertNotNull(properties.getSync());
        assertNotNull(properties.getDatasource());

        SgiSyncProperties.Mssql mssql = properties.getDatasource().getMssql();
        mssql.setHost("192.168.1.100");
        mssql.setPort(1433);
        mssql.setUser("sa");
        mssql.setPassword("secret");
        mssql.setDbAgenda("SGI_Agenda");
        mssql.setDbConsultor("SGI_Consultor");

        String jdbcUrl = mssql.buildJdbcUrl("SGI_Agenda");
        assertTrue(jdbcUrl.contains("jdbc:sqlserver://192.168.1.100:1433"));
        assertTrue(jdbcUrl.contains("databaseName=SGI_Agenda"));
        assertTrue(jdbcUrl.contains("encrypt=true"));
        assertTrue(jdbcUrl.contains("trustServerCertificate=true"));

        assertEquals(3, properties.getSync().getRetry().getMaxAttempts());
        assertEquals(300000L, properties.getSync().getRetry().getDelayMs());
        assertTrue(properties.getSync().isEnabled());

        assertNotNull(properties.getLegacy());
        assertTrue(properties.getLegacy().isKeepAliveEnabled());
        properties.getLegacy().setAgendaUrl("https://agenda.local");
        properties.getLegacy().setConsultorUrl("https://consultor.local");
        assertEquals("https://agenda.local", properties.getLegacy().getAgendaUrl());
        assertEquals("https://consultor.local", properties.getLegacy().getConsultorUrl());
    }
}
