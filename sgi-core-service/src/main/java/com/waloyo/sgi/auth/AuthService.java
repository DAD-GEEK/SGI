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
        this.supabaseUrl = env.getProperty("SUPABASE_URL");
        this.apiStreamSecret = env.getProperty("API_STREAM_SECRET");
        this.webClient = WebClient.builder().build();
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
        if (header != null && header.startsWith("Bearer ")) {
            token = header.substring(7).trim();
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
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(Map.class)
                .map(map -> {
                    Object email = map.get("email");
                    return email != null ? email.toString() : null;
                })
                .onErrorResume(e -> Mono.empty());
    }
}

