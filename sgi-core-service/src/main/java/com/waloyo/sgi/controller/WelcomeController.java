package com.waloyo.sgi.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
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
        return "<!DOCTYPE html>\n" +
                "<html lang=\"es\">\n" +
                "<head>\n" +
                "    <meta charset=\"UTF-8\">\n" +
                "    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n" +
                "    <title>SGI Core Service — API Telemetría</title>\n" +
                "    <link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">\n" +
                "    <link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin>\n" +
                "    <link href=\"https://fonts.googleapis.com/css2?family=Outfit:wght@400;750;900&family=Plus+Jakarta+Sans:wght@300;600;700&family=JetBrains+Mono:wght@400;700&display=swap\" rel=\"stylesheet\">\n" +
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
                "            padding: 20px;\n" +
                "        }\n" +
                "        .container {\n" +
                "            background: rgba(255, 255, 255, 0.75);\n" +
                "            backdrop-filter: blur(16px);\n" +
                "            border: 1px solid rgba(15, 23, 42, 0.08);\n" +
                "            border-radius: 20px;\n" +
                "            padding: 40px;\n" +
                "            max-width: 520px;\n" +
                "            width: 100%;\n" +
                "            box-shadow: 0 10px 40px -10px rgba(15, 23, 42, 0.08);\n" +
                "            text-align: center;\n" +
                "        }\n" +
                "        .logo-text {\n" +
                "            font-family: 'Outfit', sans-serif;\n" +
                "            font-size: 28px;\n" +
                "            font-weight: 900;\n" +
                "            color: #0f172a;\n" +
                "            letter-spacing: -0.5px;\n" +
                "            margin-bottom: 5px;\n" +
                "        }\n" +
                "        .logo-text span {\n" +
                "            color: #055bb2;\n" +
                "            font-weight: 700;\n" +
                "        }\n" +
                "        .badge {\n" +
                "            display: inline-flex;\n" +
                "            align-items: center;\n" +
                "            gap: 6px;\n" +
                "            background: rgba(16, 185, 129, 0.1);\n" +
                "            border: 1px solid rgba(16, 185, 129, 0.2);\n" +
                "            color: #059669;\n" +
                "            font-family: 'JetBrains Mono', monospace;\n" +
                "            font-size: 11px;\n" +
                "            font-weight: 700;\n" +
                "            padding: 4px 12px;\n" +
                "            border-radius: 100px;\n" +
                "            margin-bottom: 25px;\n" +
                "            text-transform: uppercase;\n" +
                "        }\n" +
                "        .badge .dot {\n" +
                "            width: 6px;\n" +
                "            height: 6px;\n" +
                "            background: #10B981;\n" +
                "            border-radius: 50%;\n" +
                "            box-shadow: 0 0 8px #10B981;\n" +
                "        }\n" +
                "        h2 {\n" +
                "            font-family: 'Outfit', sans-serif;\n" +
                "            font-size: 18px;\n" +
                "            font-weight: 750;\n" +
                "            color: #0F172A;\n" +
                "            margin-bottom: 12px;\n" +
                "        }\n" +
                "        p {\n" +
                "            font-size: 13px;\n" +
                "            color: #475569;\n" +
                "            line-height: 1.6;\n" +
                "            margin-bottom: 25px;\n" +
                "        }\n" +
                "        .telemetry {\n" +
                "            background: #090E17;\n" +
                "            border-radius: 12px;\n" +
                "            padding: 16px 20px;\n" +
                "            text-align: left;\n" +
                "            font-family: 'JetBrains Mono', monospace;\n" +
                "            font-size: 11px;\n" +
                "            color: #94A3B8;\n" +
                "            border: 1px solid rgba(255, 255, 255, 0.05);\n" +
                "            margin-bottom: 25px;\n" +
                "        }\n" +
                "        .telemetry-row {\n" +
                "            display: flex;\n" +
                "            justify-content: space-between;\n" +
                "            margin-bottom: 8px;\n" +
                "        }\n" +
                "        .telemetry-row:last-child {\n" +
                "            margin-bottom: 0;\n" +
                "        }\n" +
                "        .telemetry-label {\n" +
                "            color: #38BDF8;\n" +
                "        }\n" +
                "        .telemetry-value {\n" +
                "            color: #E2E8F0;\n" +
                "            font-weight: bold;\n" +
                "        }\n" +
                "        .slogan {\n" +
                "            font-family: 'Outfit', sans-serif;\n" +
                "            font-size: 10px;\n" +
                "            font-weight: bold;\n" +
                "            color: #94A3B8;\n" +
                "            letter-spacing: 2px;\n" +
                "            text-transform: uppercase;\n" +
                "        }\n" +
                "    </style>\n" +
                "</head>\n" +
                "<body>\n" +
                "    <div class=\"container\">\n" +
                "        <div class=\"logo-text\">SGI<span>Core</span></div>\n" +
                "        <div class=\"badge\"><span class=\"dot\"></span>Active / Secure</div>\n" +
                "        <h2>Gestión Integral SGI — API REST</h2>\n" +
                "        <p>Microservicio transaccional del cliente operando en Arquitectura On-Premise V3.0 sobre clúster PostgreSQL (esquema sgi).</p>\n" +
                "        <div class=\"telemetry\">\n" +
                "            <div class=\"telemetry-row\">\n" +
                "                <span class=\"telemetry-label\">STATUS:</span>\n" +
                "                <span class=\"telemetry-value\" style=\"color: #10B981;\">READY</span>\n" +
                "            </div>\n" +
                "            <div class=\"telemetry-row\">\n" +
                "                <span class=\"telemetry-label\">DATABASE_SCHEMA:</span>\n" +
                "                <span class=\"telemetry-value\">sgi (PostgreSQL Local)</span>\n" +
                "            </div>\n" +
                "            <div class=\"telemetry-row\">\n" +
                "                <span class=\"telemetry-label\">PORT:</span>\n" +
                "                <span class=\"telemetry-value\">8084 (Internal)</span>\n" +
                "            </div>\n" +
                "            <div class=\"telemetry-row\">\n" +
                "                <span class=\"telemetry-label\">VERSION:</span>\n" +
                "                <span class=\"telemetry-value\">v1.2.0</span>\n" +
                "            </div>\n" +
                "        </div>\n" +
                "        <div class=\"slogan\">SU ASESOR… SU ALIADO | WALOYO GROUP</div>\n" +
                "    </div>\n" +
                "</body>\n" +
                "</html>";
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
}
