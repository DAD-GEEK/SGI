package com.waloyo.sgi.events;

import org.springframework.http.codec.ServerSentEvent;
import org.springframework.stereotype.Service;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Sinks;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

@Service
public class UsuarioStatusPublisher {

    // email -> sink
    private final ConcurrentHashMap<String, Sinks.Many<Map<String, Object>>> sinks = new ConcurrentHashMap<>();

    public Flux<ServerSentEvent<Map<String, Object>>> subscribe(String email) {
        Sinks.Many<Map<String, Object>> sink = sinks.computeIfAbsent(email, k -> Sinks.many().multicast().onBackpressureBuffer());
        return sink.asFlux()
                .map(payload -> ServerSentEvent.<Map<String, Object>>builder()
                        .event("usuario-estado")
                        .data(payload)
                        .build());
    }

    public void publish(String email, Map<String, Object> payload) {
        Sinks.Many<Map<String, Object>> sink = sinks.get(email);
        if (sink != null) {
            sink.tryEmitNext(payload);
        }
    }
}

