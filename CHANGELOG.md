# Changelog — Gestión Integral SGI Submodule

Todos los cambios del submódulo SGI (`apps/client/SGI`) se registran en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/).

## [Sin Versionar] - 2026-08-03

### Added
- **Auditoría de cambios no comiteados en SGI**: Se revisó el estado real del submódulo y se verificó que existen cambios sin confirmar en la landing pública, el CRM y el microservicio `sgi-core-service`.
- **Documentación de gobernanza del submódulo**: Quedó registrada la revisión del estado actual del submódulo y la validación de compilación correspondiente antes de preparar un commit formal.
- **Seguimiento de cambios de seguridad y autenticación**: Se documentan las actualizaciones de CORS, reenvío de credenciales, activación de sesiones, y manejo de cambios de contraseña en SGI CRM.

### Changed
- **Estado de trabajo del submódulo**: Actualizado el historial del proyecto con la revisión de archivos modificados pendientes: `CHANGELOG.md`, `crm/package.json`, `crm/package-lock.json`, `crm/src/App.tsx`, `crm/src/pages/Login.tsx`, `crm/src/pages/Profile.tsx`, `crm/src/pages/UsuariosView.tsx`, `crm/src/pages/ChangePassword.tsx`, `src/App.tsx`, `src/components/Navbar.tsx`, `src/pages/Home.tsx` y backend `sgi-core-service`.
- **Validación de compilación**: Ejecutado `npm run build` en el frontend SGI para comprobar que la compilación se mantiene operativa antes de cualquier commit.

### Fixed
- **Cobertura documental del estado actual**: Se actualizó el changelog para reflejar la revisión y el bloque de validación previo al commit, sin alterar la lógica funcional del submódulo.

---

### Added
- **Validación Estricta de Campos y Selectores Internacionales (`src/pages/UsuariosView.tsx` & `sgi-core-service`)**: Incorporadas validaciones programáticas de tipo de dato, formato y longitud (Nombre: solo letras de 3+ caracteres; Correo: estructura RFC válida; Documento: alfanumérico entre 5 y 20 caracteres; Teléfono: solo números de 7+ dígitos). Añadidos desplegables para **Tipo de Documento** (CC, CE, PP, NIT, PPT) y **País / Indicativo Internacional** (🇨🇴 +57, 🇲🇽 +52, 🇵🇪 +51, 🇪🇨 +593, 🇨🇱 +56, 🇵🇦 +507, 🇺🇸 +1, 🇪🇸 +34).
- **Manejo Robusto de Excepción `Auth session missing` (`src/pages/ChangePassword.tsx`)**: Agregado control de excepciones para permitir el establecimiento transparente de la clave definitiva en el backend Spring Boot PostgreSQL (`POST /api/usuarios/confirmar-clave`) y desactivar `mustChangePassword = false` incluso cuando se inicie sesión en entornos locales o de desarrollo sin un token activo de Supabase Auth.
- **Medidor de Fortaleza de Contraseña y Bloqueo de Copiar/Pegar (`src/pages/ChangePassword.tsx`)**: Implementado evaluador de seguridad OWASP en tiempo real (Débil, Aceptable, Fuerte, Excelente) con lista de requisitos de complejidad. Agregado icono de ojito (👁️/👁️‍🗨️) para alternar visibilidad e inhabilitado el pegado/copiado (`onPaste`, `onCopy`) en el campo de confirmación para forzar la escritura manual obligatoria.
- **Validaciones Programáticas Corporativas (`src/pages/Login.tsx` & `src/pages/UsuariosView.tsx`)**: Eliminadas las descripciones emergentes genéricas nativas del navegador (`noValidate`). Implementadas validaciones de campo obligatorio con banners del sistema Tailwind y toasters interactivos.
- **Persistencia de Correo con "Recordar esta Sesión" (`src/pages/Login.tsx`)**: Habilitada la casilla de verificación para almacenar de forma segura el correo en `sgi_remembered_email` y auto-rellenarlo en el formulario de inicio de sesión cuando el usuario vuelve a abrir la aplicación.
- **Modal Corporativo del Sistema de Expiración de Sesión (`src/components/CrmSidebar.tsx`)**: Eliminados por completo los `alert()` nativos del navegador. Diseñado e integrado un Modal Corporativo SGI de alta seguridad con animación Tailwind, icono de candado y botón de reingreso al sistema para notificar expiraciones y desactivaciones.
- **Garantía y Reinicio Inmediato de Cronómetro al Guardar Política (`src/pages/UsuariosView.tsx` & `src/components/CrmSidebar.tsx`)**: Ajustado el manipulador de guardar política de seguridad para que, al seleccionar `⏱️ 10 Segundos (Modo Pruebas)`, la marca de tiempo `loginTimestamp` se reinicie al instante exacto (`Date.now()`). Además, `CrmSidebar.tsx` auto-crea la clave `sgi_user` si no existiera previamente, garantizando la activación inmediata del contador de 10 segundos en vivo.
- **Evaluación Continuamente Activa de Sesión cada 1 Segundo (`src/components/CrmSidebar.tsx`)**: Corregido el bucle de validación en segundo plano para ejecutarse cada 1000ms. La opción `⏱️ 10 Segundos (Modo Pruebas)` y cualquier límite de tiempo expira de forma precisa e instantánea en vivo sin necesidad de cambiar de pantalla.
- **Fallback Automático de Reenvío por Email (`src/pages/UsuariosView.tsx` & `sgi-core-service`)**: Añadido endpoint alternativo `POST /api/usuarios/reenviar-credenciales-email` para garantizar la generación y notificación de claves temporales incluso cuando el usuario no posea UUID local pre-asignado.
- **Eliminación Total de Datos Quemados (`UsuarioController.java`)**: Limpiado el 100% de la lógica de auto-seeding hardcodeada en el controlador Java. Todas las consultas de usuarios operan de forma pura sobre la base de datos PostgreSQL (`schema=sgi`).
- **Restricción Exclusiva de CORS a Dominios SGI (`CorsConfig.java`)**: Removidos los dominios de `waloyogroup.com` del controlador de CORS de SGI Core Service, restringiendo orígenes permitidos únicamente a los dominios autorizados del cliente (`https://gestionintegralsgi.com.co`, `https://app.gestionintegralsgi.com.co`, `https://sgi-*.web.app`).
- **Blindaje Estricto de CORS y Cifrado SSL/TLS de Extremo a Extremo (`CorsConfig.java` & `application.yml`)**: Eliminado el comodín `*` en backend. Restringidos los orígenes permitidos únicamente a los dominios corporativos autorizados de SGI/Waloyo (`https://sgi-crm.web.app`, `https://sgi-waloyo.web.app`, `https://admin.waloyogroup.com`) y entornos de desarrollo local.
- **Eliminación Física e Irreversible de Usuarios (`src/pages/UsuariosView.tsx` & `sgi-core-service`)**: Incorporado botón de **Eliminar (🗑️)** en la tabla de asesores con modal corporativo de confirmación de eliminación permanente e integración con `DELETE /api/usuarios/{id}` en Spring Boot.
- **Modo Edición Total de Perfil para Administrador TI (`src/pages/Profile.tsx`)**: Habilitada la modificación de correo electrónico y rol de gobernanza para la cuenta `ADMIN_TI`, mientras que para administradores de SGI y consultores estándar se mantiene bloqueado como medida de seguridad.
- **Inserción y Auto-seeding de Cuenta Matriz (`admon@waloyogroup.com`) con Rol `ADMIN_TI` (`sgi-core-service` & Supabase Producción)**: Insertada la cuenta superadministradora `admon@waloyogroup.com` en la base de datos de producción Supabase (`aws-0-ca-central-1.pooler.supabase.com`) y PostgreSQL Local con permisos totales a todos los módulos y rol `ADMIN_TI`.
- **Cuenta Matriz de Administrador TI (`admin@gestionintegralsgi.com.co`) & Rol `ADMIN_TI` (`src/pages/UsuariosView.tsx` & `sgi-core-service`)**: Auto-creación de la cuenta superadministradora con rol `ADMIN_TI` (Holding / Super Admin) con privilegios totales de gobernanza y configuración global.
- **Panel de Configuración del Sistema de Seguridad y Límite Configurable de Sesión (`src/pages/UsuariosView.tsx` & `src/components/CrmSidebar.tsx`)**: Diseñado panel exclusivo para `ADMIN_TI` en `UsuariosView.tsx` que permite modificar a demanda la duración máxima de las sesiones (1h, 2h, 4h, 8h, 12h, 24h) aplicable dinámicamente a todos los colaboradores.
- **Expulsión Inmediata y Bloqueo de Asesores Inactivos (`src/pages/Login.tsx` & `src/components/CrmSidebar.tsx`)**: Si un administrador desactiva a un asesor, el sistema cancela su sesión inmediatamente con el mensaje `"Su cuenta de asesor ha sido desactivada. Comuníquese con el administrador para restablecer su acceso"`, e impide nuevos inicios de sesión.
- **Sincronización Dinámica de Permisos RBAC en Navegación (`src/components/CrmSidebar.tsx`)**: Al guardar los cambios de un asesor en `UsuariosView.tsx`, el menú del sidebar consulta el estado en tiempo real. Si a un asesor se le deshabilitan ciertos módulos, estos desaparecen inmediatamente de su menú lateral. Si la cuenta es marcada como `INACTIVO`, se bloquea el acceso y se cierra la sesión de inmediato.
- **Reenvío de Credenciales Temporales a Usuarios Registrados (`src/pages/UsuariosView.tsx`)**: Incorporado botón **Reenviar Clave (🔑)** en la tabla y modal de edición. Si un correo ya existe, regenera la contraseña temporal, marca `mustChangePassword = true` y notifica al usuario.
- **Modales Corporativos del Sistema (`src/pages/UsuariosView.tsx`)**: Reemplazadas al 100% las ventanas de confirmación y alertas nativas del navegador (`window.confirm`, `alert`) por modales animados del sistema con estética SGI, botones de confirmación contextuales (activar/desactivar) y notificaciones flotantes de estado.
- **Autenticación Nativa Supabase Auth (`Email & Password`)**: Configurado el cliente singleton `@supabase/supabase-js` (`src/config/supabaseClient.ts`) para autenticar usuarios corporativos en SGI CRM.
- **Pantalla Obligatoria de Cambio de Contraseña (`src/pages/ChangePassword.tsx`)**: Implementado flujo de cambio de clave para usuarios que inician por primera vez con contraseña temporal.
- **Módulo Administrador de Gestión de Colaboradores y Edición de Accesos (`src/pages/UsuariosView.tsx`)**: Diseñado modal interactivo para creación y edición de asesores (nombre, Cédula/CC, correo, teléfono, rol y estado Activo/Inactivo) con asignación granular de permisos a módulos.
- **Sincronización Reactiva en Segundo Plano (`src/pages/UsuariosView.tsx`)**: Eliminado el botón manual "Sincronizar API", sustituyéndolo por refresco reactivo en segundo plano cada 10 segundos.
- **Endpoints de Edición y Desactivación (`sgi-core-service`)**: Incorporados los endpoints REST `PUT /api/usuarios/{id}` y `DELETE /api/usuarios/{id}` en `UsuarioController.java`.
- **Desincorporación de Binarios Compilados de Git (`.gitignore` & `sgi-core-service/target`)**: Agregado `target/` y `*.class` a `.gitignore` y eliminados los artefactos de compilación del control de versiones.
- **Normalización Dinámica de API URL (`crm/src/config/apiConfig.ts`)**: Creado cliente centralizado que autodetecta `VITE_SGI_API_URL` (para `https://sgi-core-service.onrender.com`), aplicando normalización defensiva para auto-inyectar `/api` si está ausente.
- **Redirección de Enlace CRM en Landing (`src/components/Navbar.tsx`)**: Actualizado el botón de navegación móvil para redirigir directamente a `https://sgi-crm.web.app/login` en producción.

## [Sin Versionar] - 2026-07-21

### Added
- **Plan Maestro 1 a 1 `AgendaSGI` & `ConsultorSGI` (`docs/agenda_sgi_functionality_mapping_plan.md`)**: Especificación exhaustiva de los 8 módulos de la aplicación legada C# Razor.
- **Bitácora de Errores y Reglas de Gobernanza (`docs/errores_y_reglas_migracion_sgi.md`)**: Registro de lecciones aprendidas, resolución de duplicados en ETL y prevención de choques de rutas en REST controllers.
- **Sub-Changelogs de Aplicaciones**:
  - `apps/client/SGI/crm/CHANGELOG.md` para el cliente de React 19.
  - `apps/client/SGI/sgi-core-service/CHANGELOG.md` para el backend de Spring Boot 3.

---

## [1.0.0] - 2026-07-20

### Added
- **Inicialización de Plataforma SGI**: Creación del subproyecto en `apps/client/SGI`.
- **Lanzamiento de Sitio Comercial**: Landing comercial con especialidades en SG-SST, ISO 9001/14001/45001 y PESV.
