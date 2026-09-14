package com.waloyo.sgi.sync;

import com.waloyo.sgi.entity.ClienteEntity;
import com.waloyo.sgi.entity.UsuarioEntity;
import com.waloyo.sgi.repository.ClienteRepository;
import com.waloyo.sgi.repository.UsuarioRepository;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.util.HashMap;
import java.util.Map;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class UnifiedTransformServiceTest {

    @Mock
    private ClienteRepository clienteRepository;

    @Mock
    private UsuarioRepository usuarioRepository;

    @InjectMocks
    private UnifiedTransformService unifiedTransformService;

    @Test
    @DisplayName("Debe transformar y guardar cliente valido")
    void testProcessClienteValido() {
        Map<String, Object> data = new HashMap<>();
        data.put("NIT", "900123456-7");
        data.put("RazonSocial", "Empresa Test SAS");
        data.put("Direccion", "Calle 123");
        data.put("Telefono", "3001234567");
        data.put("Correo", "contacto@empresatest.com");
        data.put("OpcEstado", 1);

        RawRecord record = RawRecord.builder()
                .sourceDb("SGI_Agenda")
                .sourceTable("Clientes")
                .primaryKey("900123456-7")
                .data(data)
                .build();

        when(clienteRepository.findByNit(anyString())).thenReturn(Optional.empty());

        assertDoesNotThrow(() -> unifiedTransformService.transformAndUpsert(record).block());
        verify(clienteRepository, times(1)).save(any(ClienteEntity.class));
    }

    @Test
    @DisplayName("Debe ignorar cliente con NIT invalido o descartable")
    void testProcessClienteInvalido() {
        Map<String, Object> data = new HashMap<>();
        data.put("NIT", "11111111");
        data.put("RazonSocial", "Empresa Ficticia");

        RawRecord record = RawRecord.builder()
                .sourceDb("SGI_Agenda")
                .sourceTable("Clientes")
                .primaryKey("11111111")
                .data(data)
                .build();

        assertDoesNotThrow(() -> unifiedTransformService.transformAndUpsert(record).block());
        verify(clienteRepository, never()).save(any(ClienteEntity.class));
    }

    @Test
    @DisplayName("Debe ignorar creacion automatica no supervisada de usuarios desde ETL")
    void testProcessUsuarioIgnoradoEnEtl() {
        Map<String, Object> data = new HashMap<>();
        data.put("Email", "asesor@waloyo.com");
        data.put("Nombre", "Asesor Waloyo");
        data.put("Documento", "1020304050");

        RawRecord record = RawRecord.builder()
                .sourceDb("SGI_Consultor")
                .sourceTable("Usuarios")
                .primaryKey("asesor@waloyo.com")
                .data(data)
                .build();

        assertDoesNotThrow(() -> unifiedTransformService.transformAndUpsert(record).block());
        verify(usuarioRepository, never()).save(any(UsuarioEntity.class));
    }
}
