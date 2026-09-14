package com.waloyo.sgi.sync;

import org.springframework.stereotype.Service;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.SecureRandom;
import java.util.Arrays;
import java.util.Base64;

/**
 * Servicio criptografico dual para interoperabilidad de contrasenas con los sistemas legados:
 * 1. AgendaSGI (gestioni_datosNet): TripleDES (3DES ECB PKCS5Padding) con llave derivada de MD5(password).
 * 2. ConsultorSGI (gestioni_consultorNet): ASP.NET Identity v2 PasswordHasher (PBKDF2-HMAC-SHA1, 1000 iteraciones).
 */
@Service
public class SgiLegacyCryptoService {

    private static final SecureRandom SECURE_RANDOM = new SecureRandom();

    /**
     * Cifra una contrasena usando el algoritmo exacto de AgendaSGI:
     * MD5 hash como llave -> TripleDES ECB PKCS5Padding -> Base64.
     */
    public String encryptAgendaPassword(String plainPassword) {
        if (plainPassword == null) return null;
        try {
            MessageDigest md5 = MessageDigest.getInstance("MD5");
            byte[] keyBytes = md5.digest(plainPassword.getBytes(StandardCharsets.UTF_8));

            // TripleDES requiere 24 bytes. Si MD5 genera 16 bytes (128 bits),
            // en .NET TripleDESCryptoServiceProvider con llave de 16 bytes repite los primeros 8 bytes al final (Keying Option 2).
            byte[] key24 = new byte[24];
            System.arraycopy(keyBytes, 0, key24, 0, 16);
            System.arraycopy(keyBytes, 0, key24, 16, 8);

            SecretKey secretKey = new SecretKeySpec(key24, "DESede");
            Cipher cipher = Cipher.getInstance("DESede/ECB/PKCS5Padding");
            cipher.init(Cipher.ENCRYPT_MODE, secretKey);

            byte[] encrypted = cipher.doFinal(plainPassword.getBytes(StandardCharsets.UTF_8));
            return Base64.getEncoder().encodeToString(encrypted);
        } catch (Exception e) {
            throw new IllegalStateException("Error al cifrar contrasena para AgendaSGI", e);
        }
    }

    /**
     * Genera un hash compatible al 100% con Microsoft.AspNet.Identity.PasswordHasher v2:
     * Byte 0: 0x00 (version v2)
     * Bytes 1-16: Salt de 128 bits (16 bytes)
     * Bytes 17-48: Subkey derivada de 256 bits (32 bytes) usando PBKDF2WithHmacSHA1 (1000 iteraciones)
     * Retorna Base64 de 68 caracteres.
     */
    public String hashConsultorPassword(String plainPassword) {
        if (plainPassword == null) return null;
        try {
            byte[] salt = new byte[16];
            SECURE_RANDOM.nextBytes(salt);

            PBEKeySpec spec = new PBEKeySpec(plainPassword.toCharArray(), salt, 1000, 256);
            SecretKeyFactory skf = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
            byte[] subkey = skf.generateSecret(spec).getEncoded();

            byte[] output = new byte[1 + 16 + 32];
            output[0] = 0x00; // formato v2
            System.arraycopy(salt, 0, output, 1, 16);
            System.arraycopy(subkey, 0, output, 17, 32);

            return Base64.getEncoder().encodeToString(output);
        } catch (Exception e) {
            throw new IllegalStateException("Error al generar hash para ConsultorSGI (AspNetUsers)", e);
        }
    }

    /**
     * Valida si una contrasena coincide con el hash generado por ConsultorSGI (v2).
     */
    public boolean verifyConsultorPassword(String hashedPassword, String plainPassword) {
        if (hashedPassword == null || plainPassword == null) return false;
        try {
            byte[] decoded = Base64.getDecoder().decode(hashedPassword);
            if (decoded.length != 49 || decoded[0] != 0x00) {
                return false;
            }

            byte[] salt = Arrays.copyOfRange(decoded, 1, 17);
            byte[] expectedSubkey = Arrays.copyOfRange(decoded, 17, 49);

            PBEKeySpec spec = new PBEKeySpec(plainPassword.toCharArray(), salt, 1000, 256);
            SecretKeyFactory skf = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
            byte[] actualSubkey = skf.generateSecret(spec).getEncoded();

            return MessageDigest.isEqual(expectedSubkey, actualSubkey);
        } catch (Exception e) {
            return false;
        }
    }
}
