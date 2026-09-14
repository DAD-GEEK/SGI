package com.waloyo.sgi.controller;

import com.waloyo.sgi.dto.AuditoriaDashboardDTO;
import com.waloyo.sgi.dto.DashboardKpisDTO;
import com.waloyo.sgi.service.LegacyDashboardMetricsService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("/api/dashboard")
@RequiredArgsConstructor
public class DashboardMetricsController {

    private final LegacyDashboardMetricsService dashboardMetricsService;

    @GetMapping("/kpis")
    public ResponseEntity<DashboardKpisDTO> obtenerKpis(
            @RequestParam(required = false) String email,
            @RequestParam(required = false) Boolean soloMias) {
        return ResponseEntity.ok(dashboardMetricsService.obtenerKpis(email, soloMias));
    }

    @GetMapping("/auditorias")
    public ResponseEntity<List<AuditoriaDashboardDTO>> obtenerAuditorias(
            @RequestParam(required = false) String email,
            @RequestParam(defaultValue = "6") int limit,
            @RequestParam(required = false) Boolean soloMias) {
        return ResponseEntity.ok(dashboardMetricsService.obtenerAuditoriasRecientes(email, limit, soloMias));
    }
}
