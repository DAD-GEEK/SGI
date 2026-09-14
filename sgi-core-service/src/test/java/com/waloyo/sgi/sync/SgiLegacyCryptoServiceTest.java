package com.waloyo.sgi.sync;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

class SgiLegacyCryptoServiceTest {

    private SgiLegacyCryptoService cryptoService;

    @BeforeEach
    void setUp() {
        cryptoService = new SgiLegacyCryptoService();
    }

    @Test
    @DisplayName("Debe cifrar contrasena en formato 3DES-MD5 para AgendaSGI")
    void testEncryptAgendaPassword() {
        String encrypted = cryptoService.encryptAgendaPassword("123456");
        assertNotNull(encrypted);
        assertFalse(encrypted.isBlank());
        assertTrue(encrypted.length() > 10);
    }

    @Test
    @DisplayName("Debe generar y verificar hash compatible con ASP.NET Identity v2 para ConsultorSGI")
    void testConsultorPasswordHashAndVerification() {
        String password = "MiClaveSegura2026!*";
        String hash = cryptoService.hashConsultorPassword(password);

        assertNotNull(hash);
        assertEquals(68, hash.length(), "El hash Base64 de ASP.NET Identity v2 debe tener exactamente 68 caracteres");

        assertTrue(cryptoService.verifyConsultorPassword(hash, password), "Debe verificar la contrasena correcta");
        assertFalse(cryptoService.verifyConsultorPassword(hash, "ClaveIncorrecta"), "Debe rechazar contrasenas incorrectas");
    }

    @Test
    @DisplayName("Generar contrasenas cifradas para Waloyo2026!*")
    void testGenerateUserHashes() {
        String raw = "Waloyo2026!*";
        String agendaEnc = cryptoService.encryptAgendaPassword(raw);
        String consultorHash = cryptoService.hashConsultorPassword(raw);

        System.out.println("=== AGENDA_ENCRYPTED ===" + agendaEnc + "=== END ===");
        System.out.println("=== CONSULTOR_HASH ===" + consultorHash + "=== END ===");

        assertNotNull(agendaEnc);
        assertNotNull(consultorHash);
        assertTrue(cryptoService.verifyConsultorPassword(consultorHash, raw));
    }
}
