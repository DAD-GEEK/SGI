package com.waloyo.sgi.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

@Configuration
public class CorsConfig {

    @Bean
    public WebMvcConfigurer corsConfigurer() {
        return new WebMvcConfigurer() {
            @Override
            public void addCorsMappings(CorsRegistry registry) {
                registry.addMapping("/**")
                        .allowedOriginPatterns(
                                "http://localhost:*",
                                "http://127.0.0.1:*",
                                "https://sgi-crm.web.app",
                                "https://sgi-crm.firebaseapp.com",
                                "https://sgi-landing.web.app",
                                "https://sgi-landing.firebaseapp.com",
                                "https://sgi-waloyo.web.app",
                                "https://sgi-waloyo.firebaseapp.com",
                                "https://crm.gestionintegralsgi.com.co",
                                "https://app.gestionintegralsgi.com.co",
                                "https://gestionintegralsgi.com.co",
                                "https://*.gestionintegralsgi.com.co",
                                "https://waloyogroup.com",
                                "https://*.waloyogroup.com"
                        )
                        .allowedMethods("GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH")
                        .allowedHeaders("*")
                        .allowCredentials(true);
            }
        };
    }
}
