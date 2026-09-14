# Changelog — ConsultorSGI

Todos los cambios notables del proyecto ConsultorSGI serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/).

## [2.1.0] - 2026-09-13

### Added
- **SSO Transparente para Iframes (Web/Controllers/SeguridadController.cs)**: Endpoint `GET /Seguridad/SSO(string email)` para autenticación directa en el CRM de Gestión Integral SGI.

### Changed
- **Blindaje de Cookies SameSite para Iframes (Web/App_Start/Startup.Auth.cs & Web/Global.asax.cs)**:
  - Configurado `CookieSecure = CookieSecureOption.Always` en OWIN.
  - Intercepción en `Application_EndRequest` de `Global.asax.cs` para inyectar `; SameSite=None; Secure` en todas las cookies de respuesta y encabezados `Set-Cookie`, garantizando la persistencia de sesión dentro del CRM embebido.
- **Mensaje de Error Amigable en Login (Web/Controllers/SeguridadController.cs)**: Información clara al usuario cuando la contraseña no coincide con la versión actualizada del CRM.

## [2.0.0] - 2026-09-05

### Added
- **Manual de Usuario Oficial (docs/MANUAL_USUARIO_CONSULTOR_SGI.md)**: Manual exhaustivo de operación del sistema SG-SST, auditorías e ISO cubriendo diagnósticos Res. 0312, gestión de terceros y evidencias.