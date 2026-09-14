# Changelog — AgendaSGI

Todos los cambios notables del proyecto AgendaSGI serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/).

## [2.1.0] - 2026-09-13

### Added
- **SSO Transparente para Iframes (Web/Controllers/AuthController.cs)**: Endpoint `GET /Auth/SSO(string email)` que autentica al usuario directamente y establece la sesión de ASP.NET cuando es embebido dentro del CRM de Gestión Integral SGI.

### Changed
- **Mensaje de Error Amigable en Login (Web/Controllers/AuthController.cs)**: Actualizada la respuesta ante contraseña incorrecta para informar al usuario de forma clara: *"Contraseña incorrecta. Si modificó su clave recientemente en el CRM, contacte al Administrador para sincronizar o ingrese su clave anterior."*
- **Compilación Resiliente (Web/Web.csproj)**: Añadida la condición `And '$(TypeScriptCompileBlocked)' != 'true'` a los targets de TypeScript para prevenir bloqueos de compilación en entornos headless.

## [2.0.0] - 2026-09-05

### Added
- **Manual Operativo del Consultor (docs/MANUAL_CONSULTOR_SGI_AGENDA.md)**: Guía detallada para asesores en campo que cubre el flujo de visitas, diligenciamiento de actas y gestión de compromisos.
- **Manual de Usuario Oficial (docs/MANUAL_USUARIO_SGI_AGENDA.md)**: Documentación para administradores y consultores sobre gestión de clientes, contratos, sedes y navegación del calendario.
- **Configuración de Librerías (Web/libman.json)**: Definición para gestión de librerías estáticas de cliente.

### Changed
- **.gitignore**: Añadida exclusión para archivos de respaldo comprimidos (*.rar).