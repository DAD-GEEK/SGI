package com.waloyo.sgi.service;

import com.waloyo.sgi.sync.SgiSyncProperties;
import lombok.extern.slf4j.Slf4j;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Duration;

@Service
@Slf4j
public class LegacyKeepAliveService {

    private final SgiSyncProperties syncProperties;
    private final HttpClient httpClient;

    public LegacyKeepAliveService(SgiSyncProperties syncProperties) {
        this.syncProperties = syncProperties;
        this.httpClient = HttpClient.newBuilder()
                .connectTimeout(Duration.ofSeconds(10))
                .followRedirects(HttpClient.Redirect.NORMAL)
                .build();
    }

    /**
     * Ejecuta un ping HTTP ligero cada 3 minutos (180,000 ms) hacia las aplicaciones legadas ASP.NET MVC
     * alojadas en Plesk/IIS (Agenda y Consultor).
     *
     * Proposito: Evitar que el grupo de aplicaciones IIS alcance el tiempo limite de inactividad
     * (Idle Time-out de 5 minutos) y termine el proceso w3wp.exe, garantizando que el arranque en frio
     * (Cold Start) no afecte a los asesores y la carga en los iframes sea instantanea.
     */
    @Scheduled(fixedRate = 180000, initialDelay = 15000)
    public void pingLegacyApplications() {
        if (!syncProperties.getLegacy().isKeepAliveEnabled()) {
            return;
        }

        String agendaUrl = syncProperties.getLegacy().getAgendaUrl();
        String consultorUrl = syncProperties.getLegacy().getConsultorUrl();

        pingEndpoint("AGENDA", agendaUrl);
        pingEndpoint("CONSULTOR", consultorUrl);
    }

    private void pingEndpoint(String appName, String url) {
        if (url == null || url.trim().isEmpty()) {
            return;
        }

        try {
            long startTime = System.currentTimeMillis();
            HttpRequest request = HttpRequest.newBuilder()
                    .uri(URI.create(url.trim()))
                    .timeout(Duration.ofSeconds(15))
                    .header("User-Agent", "Waloyo-SGI-KeepAlive/1.0")
                    .GET()
                    .build();

            httpClient.sendAsync(request, HttpResponse.BodyHandlers.discarding())
                    .thenAccept(response -> {
                        long latency = System.currentTimeMillis() - startTime;
                        log.info("[SGI-KEEP-ALIVE] Ping exitoso a {} (HTTP {}, Latencia: {} ms)",
                                appName, response.statusCode(), latency);
                    })
                    .exceptionally(ex -> {
                        log.warn("[SGI-KEEP-ALIVE] Advertencia en ping a {}: {}",
                                appName, ex.getMessage());
                        return null;
                    });
        } catch (Exception e) {
            log.warn("[SGI-KEEP-ALIVE] Error al despachar ping a {}: {}", appName, e.getMessage());
        }
    }
}
