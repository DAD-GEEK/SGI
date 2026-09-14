package com.waloyo.sgi.config;

import org.apache.catalina.connector.ClientAbortException;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.context.request.async.AsyncRequestTimeoutException;
import org.springframework.web.server.ResponseStatusException;

import java.io.IOException;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

class GlobalExceptionHandlerTest {

    private final GlobalExceptionHandler handler = new GlobalExceptionHandler();

    @Test
    @DisplayName("Debe manejar ClientAbortException silenciosamente sin error")
    void testHandleClientAbortException() {
        ClientAbortException ex = new ClientAbortException("Broken pipe");
        assertDoesNotThrow(() -> handler.handleClientAbortException(ex));
    }

    @Test
    @DisplayName("Debe manejar AsyncRequestTimeoutException silenciosamente")
    void testHandleAsyncRequestTimeoutException() {
        AsyncRequestTimeoutException ex = new AsyncRequestTimeoutException();
        assertDoesNotThrow(() -> handler.handleAsyncRequestTimeoutException(ex));
    }

    @Test
    @DisplayName("Debe manejar IOException de Broken Pipe con 204 No Content")
    void testHandleIOExceptionBrokenPipe() {
        IOException ex = new IOException("Broken pipe");
        ResponseEntity<Void> response = handler.handleIOException(ex);
        assertEquals(HttpStatus.NO_CONTENT, response.getStatusCode());
    }

    @Test
    @DisplayName("Debe manejar IOException de Connection reset con 204 No Content")
    void testHandleIOExceptionConnectionReset() {
        IOException ex = new IOException("Connection reset by peer");
        ResponseEntity<Void> response = handler.handleIOException(ex);
        assertEquals(HttpStatus.NO_CONTENT, response.getStatusCode());
    }

    @Test
    @DisplayName("Debe manejar IOException no relacionada con socket con 500")
    void testHandleGenericIOException() {
        IOException ex = new IOException("File not found or permission error");
        ResponseEntity<Void> response = handler.handleIOException(ex);
        assertEquals(HttpStatus.INTERNAL_SERVER_ERROR, response.getStatusCode());
    }

    @Test
    @DisplayName("Debe manejar ResponseStatusException preservando codigo de estado")
    void testHandleResponseStatusException() {
        ResponseStatusException ex = new ResponseStatusException(HttpStatus.UNAUTHORIZED, "No autorizado");
        ResponseEntity<Map<String, Object>> response = handler.handleResponseStatusException(ex);
        assertEquals(HttpStatus.UNAUTHORIZED, response.getStatusCode());
        assertNotNull(response.getBody());
        assertEquals("No autorizado", response.getBody().get("error"));
    }

    @Test
    @DisplayName("Debe manejar Exception generica con 500 controlado")
    void testHandleGenericException() {
        Exception ex = new RuntimeException("Error inesperado");
        ResponseEntity<Map<String, Object>> response = handler.handleGenericException(ex);
        assertEquals(HttpStatus.INTERNAL_SERVER_ERROR, response.getStatusCode());
        assertNotNull(response.getBody());
        assertEquals(500, response.getBody().get("status"));
    }
}
