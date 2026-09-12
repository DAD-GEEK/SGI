package com.waloyo.sgi.auth;

import org.springframework.core.env.Environment;
import org.springframework.http.HttpHeaders;
import org.springframework.stereotype.Service;
import org.springframework.web.reactive.function.client.WebClient;
import reactor.core.publisher.Mono;

import java.util.Map;

@Service
public class AuthService {

    private final WebClient webClient;
    private final String supabaseUrl;
    private final String apiStreamSecret;

    public AuthService(Environment env) {
        String url = env.getProperty("SUPABASE_URL");
        if (url == null || url.isBlank()) {
            url = env.getProperty("supabase.url", "https://bmgfqxribkrhbzqvhsjp.supabase.co");
        }
        this.supabaseUrl = url;
        this.apiStreamSecret = env.getProperty("API_STREAM_SECRET");
        String sKey = env.getProperty("SUPABASE_SERVICE_ROLE_KEY");
        if (sKey == null || sKey.isBlank()) {
            sKey = env.getProperty("supabase.service-role-key");
        }
        this.serviceRoleKey = sKey;
        this.webClient = WebClient.builder().build();
    }

    private static final String BEARER_PREFIX = "Bearer ";
    private final String serviceRoleKey;

    /**
     * Aprovisiona o actualiza el usuario directamente en Supabase Auth usando la Service Role Key.
     * Invoca POST /auth/v1/admin/users con confirmación automática de correo.
     */
    @SuppressWarnings("unchecked")
    public Mono<Map<String, Object>> provisionUserInSupabase(String email, String password) {
        if (supabaseUrl == null || serviceRoleKey == null || serviceRoleKey.isBlank()) {
            return Mono.empty();
        }

        String url = supabaseUrl.endsWith("/") ? supabaseUrl : supabaseUrl + "/";
        String adminUsersUrl = url + "auth/v1/admin/users";

        Map<String, Object> body = Map.of(
                "email", email,
                "password", password != null && !password.isBlank() ? password : "Sgi" + System.currentTimeMillis() + "!*",
                "email_confirm", true
        );

        return webClient.post()
                .uri(adminUsersUrl)
                .header(HttpHeaders.AUTHORIZATION, BEARER_PREFIX + serviceRoleKey)
                .header("apikey", serviceRoleKey)
                .header(HttpHeaders.USER_AGENT, "Waloyo-Backend/1.0")
                .bodyValue(body)
                .retrieve()
                .bodyToMono(Map.class)
                .map(m -> (Map<String, Object>) m)
                .onErrorResume(e -> Mono.empty());
    }

    /**
     * Valida el token Authorization. Retorna el email si es válido.
     * Dos modos:
     * - Si API_STREAM_SECRET está configurado y coincide con el token, devuelve "__internal__".
     * - Si SUPABASE_URL está configurado, consulta SUPABASE_URL/auth/v1/user con el Bearer token y lee el email.
     * - Si no puede validar, retorna Mono.empty().
     */
    public Mono<String> validateToken(String authorizationHeader) {
        return validateToken(authorizationHeader, null);
    }

    public Mono<String> validateToken(String authorizationHeader, String tokenParam) {
        String header = authorizationHeader;
        String token = null;
        if (header != null && header.startsWith(BEARER_PREFIX)) {
            token = header.substring(BEARER_PREFIX.length()).trim();
        }
        if ((token == null || token.isEmpty()) && tokenParam != null && !tokenParam.isBlank()) {
            token = tokenParam.trim();
        }

        if (token == null || token.isEmpty()) return Mono.empty();

        if (apiStreamSecret != null && apiStreamSecret.equals(token)) {
            return Mono.just("__internal__");
        }

        if (supabaseUrl == null) return Mono.empty();

        String url = supabaseUrl;
        if (!url.endsWith("/")) url = url + "/";
        String userUrl = url + "auth/v1/user";

        return webClient.get()
                .uri(userUrl)
                .header(HttpHeaders.AUTHORIZATION, BEARER_PREFIX + token)
                .retrieve()
                .bodyToMono(Map.class)
                .map(map -> {
                    Object email = map.get("email");
                    return email != null ? email.toString() : null;
                })
                .onErrorResume(e -> Mono.empty());
    }
}

