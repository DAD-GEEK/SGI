package com.waloyo.sgi.config;

import lombok.extern.slf4j.Slf4j;
import org.apache.catalina.connector.ClientAbortException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;
import org.springframework.web.context.request.async.AsyncRequestTimeoutException;
import org.springframework.web.server.ResponseStatusException;

import java.io.IOException;
import java.util.Map;

/**
 * Manejador global de excepciones para sgi-core-service.
 * Intercepta desconexiones abruptas de clientes en flujos Server-Sent Events (SSE)
 * (ClientAbortException, Broken pipe, AsyncRequestTimeoutException) para evitar rastro
 * de errores 500 y desbordamiento de logs en producción.
 */
@Slf4j
@RestControllerAdvice
public class GlobalExceptionHandler {

    @ExceptionHandler(ClientAbortException.class)
    public void handleClientAbortException(ClientAbortException ex) {
        log.debug("[SGI-SSE] Cliente cerro conexion HTTP/SSE abruptamente (ClientAbortException): {}", ex.getMessage());
    }

    @ExceptionHandler(AsyncRequestTimeoutException.class)
    public void handleAsyncRequestTimeoutException(AsyncRequestTimeoutException ex) {
        log.debug("[SGI-SSE] Timeout en solicitud asincrona/SSE: {}", ex.getMessage());
    }

    @ExceptionHandler(IOException.class)
    public ResponseEntity<Void> handleIOException(IOException ex) {
        String msg = ex.getMessage() != null ? ex.getMessage().toLowerCase() : "";
        if (msg.contains("broken pipe") || msg.contains("connection reset")) {
            log.debug("[SGI-SSE] Socket cerrado por el cliente durante streaming: {}", ex.getMessage());
            return ResponseEntity.noContent().build();
        }
        log.error("[SGI-IO] Error de entrada/salida no controlado: {}", ex.getMessage(), ex);
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).build();
    }

    @ExceptionHandler(ResponseStatusException.class)
    public ResponseEntity<Map<String, Object>> handleResponseStatusException(ResponseStatusException ex) {
        log.warn("[SGI-HTTP] Excepcion de estado HTTP: {} - {}", ex.getStatusCode(), ex.getReason());
        return ResponseEntity.status(ex.getStatusCode()).body(Map.of(
                "error", ex.getReason() != null ? ex.getReason() : ex.getMessage(),
                "status", ex.getStatusCode().value()
        ));
    }

    @ExceptionHandler(Exception.class)
    public ResponseEntity<Map<String, Object>> handleGenericException(Exception ex) {
        log.error("[SGI-INTERNAL] Error interno no controlado: {}", ex.getMessage(), ex);
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(Map.of(
                "error", "Error interno del servidor",
                "status", HttpStatus.INTERNAL_SERVER_ERROR.value()
        ));
    }
}
