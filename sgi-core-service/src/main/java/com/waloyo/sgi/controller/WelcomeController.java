package com.waloyo.sgi.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.ClassPathResource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.util.StreamUtils;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.time.Instant;
import java.util.HashMap;
import java.util.Map;

@RestController
public class WelcomeController {

    @Autowired(required = false)
    private DataSource dataSource;

    @GetMapping(value = "/", produces = MediaType.TEXT_HTML_VALUE)
    public String welcome() {
        boolean dbOk = checkDatabaseHealth();
        String initialBadgeClass = dbOk ? "badge-active" : "badge-error";
        String initialBadgeText = dbOk ? "ACTIVE / SECURE" : "DEGRADED / ERROR";

        return "<!DOCTYPE html>\n" +
                "<html lang=\"es\">\n" +
                "<head>\n" +
                "    <meta charset=\"UTF-8\">\n" +
                "    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n" +
                "    <title>SGI Core Service — Ecosistema Digital</title>\n" +
                "    <link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">\n" +
                "    <link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin>\n" +
                "    <link href=\"https://fonts.googleapis.com/css2?family=Outfit:wght@400;600;700;900&family=Plus+Jakarta+Sans:wght@300;400;600;700&family=JetBrains+Mono:wght@400;700&display=swap\" rel=\"stylesheet\">\n" +
                "    <style>\n" +
                "        * {\n" +
                "            box-sizing: border-box;\n" +
                "            margin: 0;\n" +
                "            padding: 0;\n" +
                "        }\n" +
                "        body {\n" +
                "            font-family: 'Plus Jakarta Sans', sans-serif;\n" +
                "            background: radial-gradient(circle at 50% 0%, #E2E8F0 0%, #F8FAFC 100%);\n" +
                "            color: #0F172A;\n" +
                "            min-height: 100vh;\n" +
                "            display: flex;\n" +
                "            align-items: center;\n" +
                "            justify-content: center;\n" +
                "            padding: 24px 16px;\n" +
                "        }\n" +
                "        .card {\n" +
                "            background: rgba(255, 255, 255, 0.85);\n" +
                "            backdrop-filter: blur(20px);\n" +
                "            -webkit-backdrop-filter: blur(20px);\n" +
                "            border: 1px solid rgba(15, 23, 42, 0.08);\n" +
                "            border-radius: 24px;\n" +
                "            padding: 44px 36px 36px;\n" +
                "            max-width: 480px;\n" +
                "            width: 100%;\n" +
                "            box-shadow: 0 20px 50px -15px rgba(15, 23, 42, 0.08);\n" +
                "            text-align: center;\n" +
                "            display: flex;\n" +
                "            flex-direction: column;\n" +
                "            align-items: center;\n" +
                "        }\n" +
                "        .logo-container {\n" +
                "            margin-bottom: 20px;\n" +
                "            display: flex;\n" +
                "            align-items: center;\n" +
                "            justify-content: center;\n" +
                "            min-height: 70px;\n" +
                "        }\n" +
                "        .sgi-logo {\n" +
                "            max-width: 220px;\n" +
                "            max-height: 80px;\n" +
                "            width: auto;\n" +
                "            height: auto;\n" +
                "            object-fit: contain;\n" +
                "        }\n" +
                "        .badge {\n" +
                "            display: inline-flex;\n" +
                "            align-items: center;\n" +
                "            gap: 8px;\n" +
                "            font-family: 'JetBrains Mono', monospace;\n" +
                "            font-size: 11px;\n" +
                "            font-weight: 700;\n" +
                "            padding: 6px 14px;\n" +
                "            border-radius: 100px;\n" +
                "            margin-bottom: 20px;\n" +
                "            text-transform: uppercase;\n" +
                "            letter-spacing: 0.5px;\n" +
                "            transition: all 0.3s ease;\n" +
                "        }\n" +
                "        .badge-active {\n" +
                "            background: rgba(16, 185, 129, 0.12);\n" +
                "            border: 1px solid rgba(16, 185, 129, 0.3);\n" +
                "            color: #059669;\n" +
                "        }\n" +
                "        .badge-active .dot {\n" +
                "            background: #10B981;\n" +
                "            box-shadow: 0 0 10px #10B981;\n" +
                "        }\n" +
                "        .badge-error {\n" +
                "            background: rgba(239, 68, 68, 0.12);\n" +
                "            border: 1px solid rgba(239, 68, 68, 0.3);\n" +
                "            color: #DC2626;\n" +
                "        }\n" +
                "        .badge-error .dot {\n" +
                "            background: #EF4444;\n" +
                "            box-shadow: 0 0 10px #EF4444;\n" +
                "        }\n" +
                "        .badge-updating {\n" +
                "            background: rgba(245, 158, 11, 0.12);\n" +
                "            border: 1px solid rgba(245, 158, 11, 0.3);\n" +
                "            color: #D97706;\n" +
                "        }\n" +
                "        .badge-updating .dot {\n" +
                "            background: #F59E0B;\n" +
                "            box-shadow: 0 0 10px #F59E0B;\n" +
                "        }\n" +
                "        .dot {\n" +
                "            width: 7px;\n" +
                "            height: 7px;\n" +
                "            border-radius: 50%;\n" +
                "            display: inline-block;\n" +
                "            animation: pulse-dot 2s infinite ease-in-out;\n" +
                "        }\n" +
                "        @keyframes pulse-dot {\n" +
                "            0%, 100% { opacity: 1; transform: scale(1); }\n" +
                "            50% { opacity: 0.45; transform: scale(1.2); }\n" +
                "        }\n" +
                "        h1 {\n" +
                "            font-family: 'Outfit', sans-serif;\n" +
                "            font-size: 20px;\n" +
                "            font-weight: 700;\n" +
                "            color: #0F172A;\n" +
                "            letter-spacing: -0.3px;\n" +
                "            margin-bottom: 6px;\n" +
                "        }\n" +
                "        .service-title {\n" +
                "            font-size: 13px;\n" +
                "            color: #64748B;\n" +
                "            font-weight: 500;\n" +
                "            margin-bottom: 28px;\n" +
                "        }\n" +
                "        .footer-divider {\n" +
                "            width: 100%;\n" +
                "            height: 1px;\n" +
                "            background: linear-gradient(90deg, transparent, rgba(15, 23, 42, 0.08), transparent);\n" +
                "            margin-bottom: 22px;\n" +
                "        }\n" +
                "        .footer {\n" +
                "            width: 100%;\n" +
                "            display: flex;\n" +
                "            flex-direction: column;\n" +
                "            align-items: center;\n" +
                "            gap: 8px;\n" +
                "        }\n" +
                "        .copyright {\n" +
                "            font-size: 11px;\n" +
                "            color: #94A3B8;\n" +
                "            font-weight: 500;\n" +
                "        }\n" +
                "        .developed-by {\n" +
                "            font-size: 12px;\n" +
                "            color: #64748B;\n" +
                "            display: flex;\n" +
                "            align-items: center;\n" +
                "            gap: 5px;\n" +
                "        }\n" +
                "        .waloyo-brand {\n" +
                "            color: #0F172A;\n" +
                "            text-decoration: none;\n" +
                "            font-family: 'Outfit', sans-serif;\n" +
                "            font-weight: 900;\n" +
                "            letter-spacing: -0.2px;\n" +
                "            transition: color 0.2s;\n" +
                "        }\n" +
                "        .waloyo-brand span {\n" +
                "            color: #2563EB;\n" +
                "            font-weight: 600;\n" +
                "        }\n" +
                "        .waloyo-brand:hover {\n" +
                "            color: #2563EB;\n" +
                "        }\n" +
                "        .slogan {\n" +
                "            font-family: 'Outfit', sans-serif;\n" +
                "            font-size: 10px;\n" +
                "            font-weight: 700;\n" +
                "            color: #94A3B8;\n" +
                "            letter-spacing: 2px;\n" +
                "            text-transform: uppercase;\n" +
                "            margin-top: 4px;\n" +
                "        }\n" +
                "    </style>\n" +
                "</head>\n" +
                "<body>\n" +
                "    <div class=\"card\">\n" +
                "        <div class=\"logo-container\">\n" +
                "            <img src=\"/logo.png\" alt=\"Gestión Integral SGI Logo\" class=\"sgi-logo\" />\n" +
                "        </div>\n" +
                "        <div id=\"status-badge\" class=\"badge " + initialBadgeClass + "\">\n" +
                "            <span class=\"dot\"></span>\n" +
                "            <span id=\"badge-text\">" + initialBadgeText + "</span>\n" +
                "        </div>\n" +
                "        <h1>SGI Core Service</h1>\n" +
                "        <div class=\"service-title\">Motor Transaccional</div>\n" +
                "        <div class=\"footer-divider\"></div>\n" +
                "        <footer class=\"footer\">\n" +
                "            <p class=\"copyright\">© 2026 Gestión Integral SGI S.A.S. Todos los derechos reservados.</p>\n" +
                "            <div class=\"developed-by\">\n" +
                "                <span>Desarrollado y Gestionado por</span>\n" +
                "                <a href=\"https://waloyogroup.com/\" target=\"_blank\" rel=\"noopener noreferrer\" class=\"waloyo-brand\">\n" +
                "                    WALOYO <span>GROUP</span>\n" +
                "                </a>\n" +
                "            </div>\n" +
                "            <div class=\"slogan\">Ingeniería · Continuidad · Resiliencia</div>\n" +
                "        </footer>\n" +
                "    </div>\n" +
                "    <script>\n" +
                "        function checkHealth() {\n" +
                "            fetch('/api/health', { cache: 'no-store' })\n" +
                "                .then(function(res) {\n" +
                "                    if (!res.ok) throw new Error('HTTP ' + res.status);\n" +
                "                    return res.json();\n" +
                "                })\n" +
                "                .then(function(data) {\n" +
                "                    var badge = document.getElementById('status-badge');\n" +
                "                    var text = document.getElementById('badge-text');\n" +
                "                    if (data && data.database === 'RUNNING') {\n" +
                "                        badge.className = 'badge badge-active';\n" +
                "                        text.innerText = 'ACTIVE / SECURE';\n" +
                "                    } else {\n" +
                "                        badge.className = 'badge badge-error';\n" +
                "                        text.innerText = 'DEGRADED / ERROR';\n" +
                "                    }\n" +
                "                })\n" +
                "                .catch(function() {\n" +
                "                    var badge = document.getElementById('status-badge');\n" +
                "                    var text = document.getElementById('badge-text');\n" +
                "                    if (badge && text) {\n" +
                "                        badge.className = 'badge badge-updating';\n" +
                "                        text.innerText = 'ACTUALIZANDO / REINICIANDO...';\n" +
                "                    }\n" +
                "                });\n" +
                "        }\n" +
                "        setInterval(checkHealth, 5000);\n" +
                "    </script>\n" +
                "</body>\n" +
                "</html>";
    }

    @GetMapping(value = "/logo.png", produces = MediaType.IMAGE_PNG_VALUE)
    public ResponseEntity<byte[]> logo() {
        try {
            ClassPathResource resource = new ClassPathResource("static/logo.png");
            byte[] bytes = StreamUtils.copyToByteArray(resource.getInputStream());
            return ResponseEntity.ok()
                    .header(HttpHeaders.CACHE_CONTROL, "public, max-age=86400")
                    .body(bytes);
        } catch (Exception e) {
            return ResponseEntity.notFound().build();
        }
    }

    @GetMapping("/api/health")
    public ResponseEntity<Map<String, Object>> health() {
        Map<String, Object> health = new HashMap<>();
        health.put("status", "UP");
        health.put("timestamp", Instant.now().toString());
        health.put("service", "sgi-core-service");

        boolean dbOk = false;
        long totalClientes = 0;
        if (dataSource != null) {
            try (Connection conn = dataSource.getConnection();
                 PreparedStatement stmt = conn.prepareStatement("SELECT count(*) FROM sgi.terceros_clientes")) {
                try (ResultSet rs = stmt.executeQuery()) {
                    if (rs.next()) {
                        dbOk = true;
                        totalClientes = rs.getLong(1);
                    }
                }
            } catch (Exception ignored) {}
        }
        health.put("database", dbOk ? "RUNNING" : "DEGRADED");
        health.put("totalClientesSGI", totalClientes);

        return ResponseEntity.ok(health);
    }

    private boolean checkDatabaseHealth() {
        if (dataSource == null) return false;
        try (Connection conn = dataSource.getConnection();
             PreparedStatement stmt = conn.prepareStatement("SELECT 1")) {
            try (ResultSet rs = stmt.executeQuery()) {
                return rs.next();
            }
        } catch (Exception e) {
            return false;
        }
    }
}
