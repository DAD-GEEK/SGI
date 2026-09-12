package com.waloyo.sgi.sync;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.jdbc.core.JdbcTemplate;

import java.util.HashMap;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class RawMirrorServiceTest {

    @Mock
    private JdbcTemplate jdbcTemplate;

    @InjectMocks
    private RawMirrorService rawMirrorService;

    @Test
    @DisplayName("Debe certificar esquema y tabla espejo sin errores")
    void testEnsureRawSchemaAndTable() {
        Map<String, Object> sampleRow = new HashMap<>();
        sampleRow.put("NIT", "900123456");
        sampleRow.put("Razon Social", "Empresa Test");

        assertDoesNotThrow(() -> rawMirrorService.ensureRawSchemaAndTable("SGI_Agenda", "Clientes", sampleRow).block());
        verify(jdbcTemplate, atLeast(2)).execute(anyString());
    }

    @Test
    @DisplayName("Debe insertar registro crudo en tabla espejo sanitizada")
    void testInsertRawRecord() {
        Map<String, Object> data = new HashMap<>();
        data.put("id", 100);
        data.put("nombre", "Consultor Test");

        RawRecord record = RawRecord.builder()
                .sourceDb("SGI_Consultor")
                .sourceTable("Usuarios")
                .primaryKey("100")
                .data(data)
                .build();

        assertDoesNotThrow(() -> rawMirrorService.insertRawRecord(record).block());
        verify(jdbcTemplate, times(1)).update(anyString(), any(Object[].class));
    }
}
