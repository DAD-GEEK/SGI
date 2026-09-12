package com.waloyo.sgi;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@SpringBootApplication
@EnableScheduling
public class SgiCoreApplication {

    public static void main(String[] args) {
        SpringApplication.run(SgiCoreApplication.class, args);
    }
}
