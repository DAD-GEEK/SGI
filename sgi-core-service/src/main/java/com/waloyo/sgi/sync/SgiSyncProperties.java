package com.waloyo.sgi.sync;

import lombok.Getter;
import lombok.Setter;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.Configuration;

@Configuration
@ConfigurationProperties(prefix = "sgi")
@Getter
@Setter
public class SgiSyncProperties {

    private Sync sync = new Sync();
    private Datasource datasource = new Datasource();
    private Legacy legacy = new Legacy();

    @Getter
    @Setter
    public static class Legacy {
        private boolean keepAliveEnabled = true;
        private String agendaUrl;
        private String consultorUrl;
    }

    @Getter
    @Setter
    public static class Sync {
        private boolean enabled = true;
        private String cron = "0 0 */1 * * *";
        private Retry retry = new Retry();
    }

    @Getter
    @Setter
    public static class Retry {
        private int maxAttempts = 3;
        private long delayMs = 300000L; // 5 minutos
    }

    @Getter
    @Setter
    public static class Datasource {
        private Mssql mssql = new Mssql();
    }

    @Getter
    @Setter
    public static class Mssql {
        private String host;
        private int port = 1433;
        private String user;
        private String password;
        private String dbAgenda;
        private String dbConsultor;
        private boolean trustServerCertificate = true;
        private int loginTimeoutSec = 15;

        public String buildJdbcUrl(String databaseName) {
            return String.format(
                "jdbc:sqlserver://%s:%d;databaseName=%s;encrypt=true;trustServerCertificate=%b;loginTimeout=%d;",
                host, port, databaseName, trustServerCertificate, loginTimeoutSec
            );
        }
    }
}
