# ════════════════════ STAGE 1: BUILD ════════════════════
FROM maven:3.8.5-openjdk-17-slim AS builder
WORKDIR /app

# Copiar configuración de dependencias y descargar cache
COPY pom.xml .
RUN mvn dependency:go-offline -B

# Copiar el código fuente y compilar el JAR de producción
COPY src ./src
RUN mvn clean package -DskipTests -B

# ════════════════════ STAGE 2: RUNTIME ════════════════════
FROM eclipse-temurin:17-jre-alpine
WORKDIR /app

# Instalar utilidades de diagnóstico básicas
RUN apk add --no-cache curl

# Crear un usuario de sistema dedicado para evitar privilegios de root
RUN addgroup -S waloyogroup && adduser -S waloyo -G waloyogroup

# Copiar el ejecutable compilado desde la fase anterior
COPY --from=builder /app/target/sgi-core-service-1.0.0.jar app.jar

# Cambiar permisos al usuario no privilegiado
RUN chown -R waloyo:waloyogroup /app
USER waloyo

# Exponer el puerto del servidor interno
EXPOSE 8084

# Inyectar variables de entorno por defecto para producción
ENV SPRING_PROFILES_ACTIVE=prod

ENTRYPOINT ["java", "-jar", "app.jar"]
