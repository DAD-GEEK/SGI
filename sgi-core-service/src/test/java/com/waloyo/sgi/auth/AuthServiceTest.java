package com.waloyo.sgi.auth;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.mock.env.MockEnvironment;

import static org.junit.jupiter.api.Assertions.*;

class AuthServiceTest {

    @Test
    @DisplayName("validateToken debe rechazar tokens nulos o vacios")
    void testValidateTokenEmpty() {
        MockEnvironment env = new MockEnvironment();
        env.setProperty("API_STREAM_SECRET", "secret123");
        AuthService authService = new AuthService(env);

        String result1 = authService.validateToken(null, null).block();
        assertNull(result1);

        String result2 = authService.validateToken("", "   ").block();
        assertNull(result2);
    }

    @Test
    @DisplayName("validateToken debe validar correctamente secret interno")
    void testValidateTokenInternalSecret() {
        MockEnvironment env = new MockEnvironment();
        env.setProperty("API_STREAM_SECRET", "mi_secreto_stream");
        AuthService authService = new AuthService(env);

        String result1 = authService.validateToken("Bearer mi_secreto_stream").block();
        assertEquals("__internal__", result1);

        String result2 = authService.validateToken(null, "mi_secreto_stream").block();
        assertEquals("__internal__", result2);
    }
}
