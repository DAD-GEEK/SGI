# Changelog — Gestión Integral SGI Submodule

Todos los cambios del submódulo SGI (`apps/client/SGI`) se registran en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/).

## [1.4.11] - 2026-09-14

### 🎨 Refinamiento de UI de Login, Ayuda Contextual y Sello Corporativo Waloyo Group
- **Simplificación y Pulido Visual del Login (`Login.tsx`)**:
  - Subtítulo ajustado a **"Portal de Software"** para mayor sobriedad corporativa.
  - Botón de submit estandarizado a **"Iniciar Sesión"**.
  - Badge inferior simplificado a **"Conexión Cifrada"**.
  - Validación programática previa de formato de correo electrónico corporativo mediante expresión regular estandarizada.
- **Ayuda Contextual Interactiva en Recordar Sesión (`Login.tsx`)**:
  - Incorporado botón/icono de ayuda (`?` / `HelpCircle`) junto a la casilla "Recordar esta sesión" con tooltip flotante explicativo: aclara que guarda el correo en el equipo para agilizar accesos futuros y que por seguridad jamás almacena contraseñas.
- **Sello Corporativo Waloyo Group (`Login.tsx` & `CrmSidebar.tsx`)**:
  - Añadido pie de página corporativo con hipervínculo seguro a `https://waloyogroup.com/` ("Desarrollado por Waloyo Group — Tecnología resiliente. Operación continua.") en la pantalla de inicio de sesión.
  - Integrado enlace institucional homólogo en el pie del menú lateral colapsable del CRM.

## [1.4.10] - 2026-09-14

### 🔔 Reactividad en Notificaciones de Credenciales & Depuración Estructural de Asesores Fantasma
- **Reactividad Instantánea en Centro de Notificaciones (`Dashboard.tsx` & `Login.tsx`)**:
  - En `Login.tsx`, la llamada de auto-sincronización de credenciales se optimizó con timeout defensivo (2.5s) y genera notificación corporativa informativa tanto cuando se detectan y corrigen claves desactualizadas (`updated: true`) como cuando se confirma que las credenciales ya se encuentran al día (`success: true`).
  - Despacho del evento personalizado `sgi_notifications_changed` que es capturado en tiempo real por el hook reactivo de `Dashboard.tsx` actualizando el badge de la campana y la lista desplegable sin requerir recargar la página.
- **Depuración Estructural de Transformación ETL (`UnifiedTransformService.java` & `UnifiedTransformServiceTest.java`)**:
  - Eliminado por completo el método inactivo `processUsuario` y la inyección residual de `UsuarioRepository`, garantizando que bajo ninguna circunstancia el scheduler de sincronización reactiva vuelva a generar registros no supervisados en `sgi.usuarios_consultores`.
  - Actualizado el test unitario `UnifiedTransformServiceTest.java` para verificar de forma estricta la ausencia de escrituras de usuarios desde el pipeline ETL.
- **Script de Saneamiento para Asesores Fantasma en PostgreSQL (`sgi.usuarios_consultores`)**:
  - Documentado script SQL de purga para eliminar de forma segura los 9 registros de asesores `ASESOR_SENIOR` creados previamente por el ETL histórico en la base de datos de producción y QA.
- **Keep-Alive Ping Asíncrono contra Cold Start de IIS (`LegacyKeepAliveService.java` & `application.yml`)**:
  - Implementado servicio programado (`@Scheduled(fixedRate = 180000)`) que despacha pings HTTP asíncronos y no bloqueantes cada 3 minutos a las aplicaciones legadas ASP.NET MVC (`Agenda` y `Consultor`).
  - Previene que el Application Pool de IIS alcance el límite de inactividad de 5 minutos (*Idle Time-out*) y mate el proceso de trabajo `w3wp.exe`, eliminando por completo la latencia de arranque en frío (*Cold Start* de 15 a 30 segundos) para que los iframes carguen de forma instantánea.
- **Enrutamiento Canónico a Dominio Corporativo Principal (`App.tsx`)**:
  - Implementada redirección automática en caliente desde dominios de desarrollo de Firebase (`*.web.app` o `*.firebaseapp.com`) hacia el dominio personalizado oficial `https://crm.gestionintegralsgi.com.co`, preservando ruta, parámetros y hash.
- **Claridad Visual en Modal de Eliminación de Asesores (`UsuariosView.tsx`)**:
  - Actualizada la ventana modal de confirmación para reflejar la preservación legal de auditoría del SGSST: informa con claridad que los eventos de agenda asociados al asesor NO se eliminan, sino que se preservan intactos con su nombre y correo histórico inmutable.

## [1.4.9] - 2026-09-14

### 📦 Consolidación Arquitectónica: Integración de Aplicaciones Legadas (.NET MVC) en SGI
- **Unificación de Código Fuente en Repositorio SGI (`AgendaSGI/` y `ConsultorSGI/`)**:
  - Integradas las aplicaciones legadas ASP.NET MVC (.NET Framework 4.8) como módulos de código fuente directo dentro del repositorio SGI, eliminando dependencias de repositorios externos y referencias fragmentadas.
  - Purga estricta de binarios (`bin/`, `obj/`), paquetes NuGet (`packages/`), módulos de cliente (`node_modules/`, `Vendor/`), archivos comprimidos (`*.rar`, `*.zip`) y metadatos de entorno (`.vs/`, `*.user`, `*.suo`), garantizando un repositorio liviano y exclusivamente con código fuente versionable.
  - Actualizado `.gitignore` con exclusiones defensivas para la suite Visual Studio / MSBuild.

## [1.4.8] - 2026-09-13

### 🔐 SSO Automático Transparente en Iframes, Auto-Sincronización de Contraseñas y Centro de Notificaciones
- **Auto-Sincronización de Contraseñas en Segundo Plano (`UsuarioController.java`, `Login.tsx`, `ChangePassword.tsx`)**:
  - Implementado endpoint `POST /api/usuarios/auto-sincronizar-password` en `sgi-core-service` que sincroniza atómica y reactivamente la clave en MSSQL Agenda (3DES) y Consultor (ASP.NET Identity PBKDF2).
  - En `Login.tsx` y `ChangePassword.tsx`, al autenticar con éxito en el CRM, se despacha la sincronización en segundo plano garantizando que las credenciales de los aplicativos legados coincidan 100% con la del CRM sin bloquear la UI.
- **Centro de Notificaciones Interactivo en Header (`Dashboard.tsx`)**:
  - El botón de campana del Header (`/html/body/div/div/div/header/div[2]/button[1]/svg`) ahora cuenta con un Popover desplegable animado con badge contador de notificaciones no leídas.
  - Generación condicional de notificación corporativa únicamente cuando el sistema detecta que la clave estaba desactualizada. Incorporado botón interactivo con icono `X` en cada tarjeta para descartar/eliminar notificaciones individuales, botón para marcar todas como leídas y persistencia local.
- **SSO Transparente y Blindaje de Iframes (`AgendaView.tsx`, `ConsultorView.tsx`, `AuthController.cs`, `SeguridadController.cs`)**:
  - Implementado SSO directo pasando el email del usuario en sesión (`/Auth/SSO` y `/Seguridad/SSO`) permitiendo navegar sin re-autenticación.
  - En caso de acceso manual con clave desactualizada, las pantallas de login legadas informan amigablemente: *"Contraseña incorrecta. Si modificó su clave recientemente en el CRM, contacte al Administrador para sincronizar o ingrese su clave anterior."* permitiendo al usuario continuar con su clave legacy mientras se actualiza.

## [1.4.7] - 2026-09-13

### 🐛 Corrección Crítica de Generación de Asesores no Deseados (`ASESOR_SENIOR`) & Estandarización de Filtros Bento
- **Eliminación de Creación Inadvertida de Usuarios en ETL Reactivo (`UnifiedTransformService.java`)**:
  - Desactivado el procesamiento automático no supervisado de registros provenientes de tablas legadas (`gestioni_datosNet.Usuarios` y `gestioni_consultorNet.AspNetUsers`) que creaba 14 asesores ficticios con rol `ASESOR_SENIOR` en cada ciclo de sincronización.
  - La creación y aprovisionamiento de usuarios queda restringida de forma exclusiva a la gobernanza explícita del módulo administrativo (`POST /api/usuarios/registrar`) y scripts de base de datos controlados.
  - Actualizadas las pruebas unitarias en `UnifiedTransformServiceTest.java` verificando que ningún `UsuarioEntity` se persista automáticamente al sincronizar tablas de usuarios legados.
- **Estandarización de Barra de Filtros Bento en Vistas de Datos (`UsuariosView.tsx` - Asesores y Seguridad)**:
  - Implementada la barra de filtros avanzados estilo Bento idéntica a `ClientesView`:
    - Campo de búsqueda reactiva por Cédula (CC), nombre de asesor o correo corporativo (`md:col-span-2`).
    - Selector dinámico de Estado (`Todos los Estados`, `Solo Activos`, `Solo Inactivos`) con conteo de registros en tiempo real.
    - Selector de Registros por Vista configurable (5, 10, 20, 50, 100 registros por página).
  - Paginación dinámica enlazada al tamaño de página seleccionado con indicador de rango y total de asesores.
- **Mitigación y Blindaje de Conexiones Reactivas SSE contra `Broken pipe` (`UsuarioStatusPublisher.java` & `application.yml`)**:
  - Incorporada gestión de ciclo de vida con `.doOnCancel()` y `.doOnError()` en `UsuarioStatusPublisher` para purgar de forma inmediata los sinks de memoria cuando un navegador cierra la conexión o recarga la pestaña.
  - Configurado filtrado de nivel de logging en `application.yml` para suprimir trazas ruidosas de desconexión normal de clientes en Tomcat (`org.apache.catalina.connector.CoyoteAdapter`, `Http11NioProtocol`), manteniendo los logs limpios y enfocados en errores reales de producción.
- **Sincronización Bidireccional de Credenciales y Accesos hacia Bases de Datos Legadas (`MssqlUserSyncService.java` & `SgiLegacyCryptoService.java`)**:
  - Implementado `SgiLegacyCryptoService` replicando con precisión matemática los algoritmos de autenticación legados:
    - **AgendaSGI**: Cifrado simétrico TripleDES (3DES ECB PKCS5) con llave derivada de MD5 de la contraseña.
    - **ConsultorSGI**: Generación y verificación de hash PBKDF2-HMAC-SHA1 (1000 iteraciones con Salt aleatorio de 128 bits) compatible al 100% con Microsoft.AspNet.Identity.PasswordHasher v2 (Base64 de 68 caracteres).
  - Implementado `MssqlUserSyncService` con ejecución asíncrona reactiva (`Schedulers.boundedElastic`) para sincronizar contraseñas (`syncPassword`), activación/desactivación (`syncUserStatus`) y aprovisionamiento basado en permisos de módulos (`syncModulePermissions`) hacia `gestioni_datosNet.dbo.Usuarios` y `gestioni_consultorNet.dbo.AspNetUsers`.
  - Actualizado `ChangePassword.tsx` para enviar la contraseña en el cuerpo seguro de `POST /usuarios/confirmar-clave`, actualizándola simultáneamente en Supabase Auth y en ambas bases de datos MSSQL.
- **Preservación Histórica de Eventos y Actas de Asesores Eliminados (`UsuarioController.java` & `AgendaEventoEntity.java`)**:
  - Eliminada la instrucción destructiva `agendaEventoRepository.deleteAll(eventos)` al dar de baja un usuario.
  - Los eventos históricos ahora desvinculan la referencia foránea (`asesor_id = null`) y preservan los campos inmutables de auditoría `asesor_historico_nombre` y `asesor_historico_email`, garantizando integridad referencial y trazabilidad para reportes de auditoría y normas SST.
- **Asistente de Autenticación Rápida en Vistas Embebidas (`AgendaView.tsx` & `ConsultorView.tsx`)**:
  - Incorporada cápsula interactiva en la cabecera de las vistas de Agenda y Consultor que refleja la identidad del asesor logueado con botón de copiado en un clic (`¡Copiado!`), facilitando el acceso inmediato sin fricción ni desfasaje de contraseñas.
- **Gestión Voluntaria de Contraseña desde Perfil y Preferencias (`Profile.tsx`)**:
  - Implementada tarjeta interactiva de "Seguridad & Contraseña" en la vista de perfil de usuario (`/perfil`), permitiendo a asesores y administradores actualizar voluntariamente su credencial de acceso.
  - Validación defensiva de contraseñas alineada con estándares OWASP (mínimo 8 caracteres, mayúsculas, minúsculas, números y caracteres especiales).
  - Orquestación automática de la sincronización: actualiza de inmediato el hash en Supabase Auth y despacha a `POST /api/usuarios/confirmar-clave` para actualizar de forma atómica y reactiva las bases de datos MSSQL de Agenda (`gestioni_datosNet.dbo.Usuarios`) y Consultor (`gestioni_consultorNet.dbo.AspNetUsers`).

---

## [1.4.6] - 2026-09-13

### 🔒 Blindaje de Rutas Protegidas & Prevención de Fugas de Sesión con Clave Temporal (`ProtectedRoute.tsx`, `Login.tsx`, `ChangePassword.tsx`)
- **Blindaje de Rutas y Aislamiento de Claves Temporales (`ProtectedRoute.tsx`)**:
  - Implementado control estricto de sesión: si un usuario cuenta con clave temporal pendiente (`mustChangePassword: true`) y navega a cualquier ruta interna (`/dashboard`, `/clientes`, `/agenda`, `/consultor`, etc.), es interceptado y redirigido forzosamente a `/cambiar-password`.
  - Si un usuario con clave definitiva confirmada intenta acceder a `/cambiar-password`, es redirigido automáticamente a `/dashboard`.
- **Mitigación de "Pantalla en Blanco / Coco Limpio" ante Navegación "Atrás" del Navegador (`ChangePassword.tsx`)**:
  - Incorporada trampa de historial con escucha reactiva del evento `popstate`: si un usuario que ingresó con clave temporal presiona el botón "Atrás" del navegador antes de completar su clave definitiva, el sistema ejecuta de inmediato `performCompleteLogout()` y lo redirige a `/login` con `replace: true`, impidiendo que aterrice en una sesión incompleta o con componentes vacíos.
  - Añadido botón explícito de "Cancelar y volver al inicio de sesión" para dar salida segura al usuario.
- **Persistencia y Validación de Estado en Login (`Login.tsx`)**:
  - Al iniciar sesión con clave temporal, se registra la sesión temporal en `sgi_user` con el flag `mustChangePassword: true`.
- **Optimización de Viewport, Modales y Eliminación de Scroll Doble Innecesario (`UsuariosView.tsx`, `ClientesView.tsx`, `Dashboard.tsx`)**:
  - Eliminado el desbordamiento vertical de la ventana completa configurando `h-screen overflow-hidden` en el contenedor maestro y confinando el scroll vertical exclusivamente al área de trabajo `<main>`.
  - Reestructurados todos los modales y formularios flotantes (Crear/Editar Asesor, Agregar/Editar Cliente): ampliados horizontalmente (`max-w-2xl` y `max-w-4xl`), con cabeceras y barras de pestañas fijas (`shrink-0`), contenedores internos scrollables con límites de altura responsivos (`max-h-[calc(90vh-140px)]`) y barras de botones de acción fijadas en la parte inferior sobre fondos sólidos, eliminando la saturación vertical y los scrolls cortados.

---

## [1.4.5] - 2026-09-13

### 🎨 Optimización de Navegación Lateral en CRM (`crm/src/components/CrmSidebar.tsx`, `AgendaView.tsx`, `ConsultorView.tsx`)
- **Renombrado de Módulo de Agenda**:
  - Cambiada la etiqueta de navegación a `Módulo Agenda` para consistencia con la nomenclatura del sistema.
- **Limpieza de Accesos Redundantes**:
  - Eliminado el enlace externo a plataforma (`Plataforma SGI Ext.`) y purgado el import en desuso `ShieldCheck`.
- **Simplificación e Integración Directa de Vistas Embebidas (`AgendaView.tsx` y `ConsultorView.tsx`)**:
  - Removidos el botón alternador de vistas y el banner de advertencia (`Nota de Navegación`), dejando el `<iframe>` a pantalla completa cargando de forma fluida y directa.
- **Blindaje contra Cancelaciones Continuas en SSE (`stream-estado`)**:
  - Implementado backoff progresivo (15s a 60s) en el manejador `onerror` de `EventSource` para evitar saturación de peticiones canceladas en el inspector de red cuando el microservicio local no se encuentra encendido.

---

## [1.4.4] - 2026-09-12

### 🚀 Desacoplamiento Arquitectónico de Integración Continua (CI Propio en SGI)
- **Pipeline de CI Dedicado y Autónomo (`.github/workflows/ci.yml`)**:
  - Implementado pipeline de Integración Continua nativo dentro del repositorio de SGI ejecutado ante cada `pull_request` y `push` hacia `master` y `main`.
  - Detección precisa de componentes modificados vía `dorny/paths-filter`: `landing` (`src/**`), `crm` (`crm/**`) y `core` (`sgi-core-service/**`).
  - Validación automatizada con pruebas unitarias (`mvn clean test`), linters y compilación de frontends Vite (`npm run build`).
- **Corrección de Conflicto CORS en Controlador de Sincronización (`SyncController.java`)**:
  - Removida la anotación incompatible `@CrossOrigin(origins = "*")` que colisionaba con `allowCredentials(true)` configurado en `CorsConfig.java`, restaurando el funcionamiento del endpoint `/api/sync/trigger`.
- **Limpieza de Runtime en CI**:
  - Removido el forzado innecesario de Node 24 para silenciar warnings de deprecación en GitHub Actions.

---

## [1.4.3] - 2026-09-12

### 🔍 Observabilidad, Monitoreo de Errores & CD Automatizado (Sentry & Repository Dispatch)
- **Monitoreo en Tiempo Real en SGI CRM (`crm/src/main.tsx` & `crm/package.json`)**:
  - Incorporada la suite de observabilidad `@sentry/react` en el CRM de asesores para captura de excepciones no controladas y auditoría de llamadas fallidas.
  - Inicialización limpia y condicional con `import.meta.env.VITE_SENTRY_DSN` y `Sentry.browserTracingIntegration()`, garantizando cero llaves o DSNs hardcodeados en el repositorio.
- **Automatización de Despliegue Continuo Remoto (`.github/workflows/trigger-waloyo-cd.yml`)**:
  - Implementado workflow emisor de webhook `repository_dispatch` (evento `sgi-deploy`) hacia el monorepo WaloyoGroup para disparar el despliegue selectivo de SGI Core, CRM o Landing ante cada push/merge en la rama `master`.

---

## [1.4.2] - 2026-09-12

### 🛡️ Blindaje Reactivo SSE, Prevención de "Out of Memory" & Cierre de Sesión Atómico (SGI CRM & Core)
- **Eliminación Definitiva de Fugas de Memoria ("Out of Memory") (`CrmSidebar.tsx`)**:
  - Implementada política de cierre inmediato (`eventSource.close()`) al primer error de conexión con el backend para evitar tormentas de reconexión descontroladas del motor nativo del navegador.
  - Centralizada la purga de temporizadores (`clearTimers`) en manejadores de error y desmontaje de React, garantizando que nunca se acumulen timers zombis concurrentes en memoria.
  - Pausa automática de flujos reactivos cuando la pestaña pasa a segundo plano (`document.visibilityState !== 'visible'`).
- **Cierre de Sesión Atómico y No Bloqueante (`authUtils.ts` & `CrmSidebar.tsx`)**:
  - Desacoplamiento total entre la limpieza síncrona de credenciales (`localStorage.removeItem('sgi_user')`) y la revocación de tokens en Supabase Auth, ejecutada en microtask diferido (`setTimeout(..., 0)`).
  - Desconexión forzosa del socket `EventSource` vía referencia mutable (`useRef`) previo a la redirección para no dejar conexiones colgadas en estado `Pending`.
  - Navegación pura en memoria SPA (`navigate('/login', { replace: true })`), eliminando recargas completas de ventana (`window.location.href`) que causaban congelamiento de interfaz.
- **Aprovisionamiento Dual Automatizado en Supabase Auth (`UsuarioController.java` & `AuthService.java`)**:
  - Inyección de `service-role-key` y llamada automatizada a Supabase Auth (`/auth/v1/admin/users`) al registrar asesores en el CRM para que queden creados simultáneamente en Supabase y en la base de datos PostgreSQL de SGI.
- **Limpieza de Configuración y Cron Job Local de 15 Minutos (`SgiSyncProperties.java` & `application-local.yml`)**:
  - Removidos todos los valores quemados en código Java en `SgiSyncProperties.java`, delegando 100% la parametrización a los archivos de configuración y variables de entorno.
  - Expresión cron en `application-local.yml` corregida formalmente a 6 campos (`0 */15 * * * *`) para ejecutar la replicación incremental de BD cada 15 minutos en local.
- **Mitigación Integral de Vulnerabilidades ReDoS (Expresiones Regulares con Backtracking Superlineal) (`apiConfig.ts`, `ClientesView.tsx`, `UsuariosView.tsx`)**:
  - Reemplazado recorte de slashes por bucle determinista `while (cleaned.endsWith('/'))`.
  - Reemplazado formateo de NIT (`formatNit`) con lookahead anidado por algoritmo aritmético en bloques de 3 dígitos sin expresiones regulares.
  - Reemplazado `emailRegex` negado por validación desacoplada con `split('@')`, eliminando al 100% los 3 Security Hotspots de SonarQube (regla S5852).
- **Instrumentación de JaCoCo y Suite Exhaustiva de Pruebas Unitarias (`pom.xml`, `UsuarioControllerTest.java`, `ClienteControllerTest.java`, `AuthServiceTest.java`, `SgiSyncPropertiesTest.java`, `UnifiedTransformServiceTest.java`, `RawMirrorServiceTest.java`)**:
  - Integrado `jacoco-maven-plugin` para emitir reportes XML (`target/site/jacoco/jacoco.xml`) e importación directa en SonarQube On-Premise.
  - 14 pruebas unitarias automatizadas con 0 fallos cubriendo controladores, servicios de autenticación y pipeline ETL de replicación de bases de datos.

---

## [1.4.1] - 2026-09-12

### ⚡ Corrección de Cierre de Sesión & Aceleración de Carga (SGI CRM)
- **Descongelamiento Inmediato en Cierre de Sesión (`authUtils.ts`)**: Añadido límite estricto de tiempo asíncrono (`Promise.race` con timeout de 800ms) al invalidar la sesión en Supabase (`supabase.auth.signOut`). Previene cuelgues o bloqueos indefinidos del navegador cuando el usuario se encuentra en modo fallback/demo o si el endpoint remoto de Supabase no responde.
- **Redirección Robusta al Login (`CrmSidebar.tsx`)**: Refactorizado `handleLogout()` para invocar la purga de credenciales y ejecutar inmediatamente `window.location.href = '/login'`, eliminando el estado congelado del DOM y reseteando limpiamente las subscripciones y contextos de React.
- **Aceleración de Arranque y Login (`Login.tsx` & `CrmSidebar.tsx`)**:
  - Incorporado `AbortSignal.timeout(1500)` en la verificación de estado HTTP del usuario (`/api/usuarios/verificar-estado`).
  - Eliminación total de puertas traseras o accesos simulados en desarrollo (`Login.tsx`). El acceso al CRM exige estricta e indispensablemente autenticación válida y confirmada en Supabase Auth (`supabase.auth.signInWithPassword`). Cero bypasses no autorizados.
  - Añadido timeout de 1 segundo a las llamadas asíncronas de sesión (`supabase.auth.getUser` y `supabase.auth.getSession`) en la barra lateral para renderizar instantáneamente el Dashboard sin pausas perceptibles.
  - **Corrección de Longitud de Documento en ETL Reactivo (`UsuarioEntity.java` & `UnifiedTransformService.java`)**: Solucionado el error SQL `value too long for type character varying(20)` al replicar usuarios desde `gestioni_consultorNet.AspNetUsers`. Se expandió `documento` a `character varying(100)` y se aplicó truncamiento defensivo a 90 caracteres para UUIDs o identificadores extensos legados.
  - **Acceso Universal Garantizado para Super Admin (`ADMIN_TI` & `ADMIN`)**: Actualizado `Login.tsx` y `CrmSidebar.tsx` para que cualquier usuario con rol `ADMIN_TI` o `ADMIN` reciba acceso automático, irrestricto y perpetuo a todos los módulos existentes (`dashboard`, `clientes`, `agenda`, `consultor`, `usuarios`) y a cualquier módulo futuro que se incorpore en el sistema.

---

## [1.4.0] - 2026-09-12

### 🔄 Replicación de Base de Datos, ETL Reactivo & Watermark Incremental (sgi-core-service)
- **Motor de Replicación Reactiva y Extracción Incremental (`MssqlExtractionService.java` & WebFlux)**: Implementado servicio reactivo (`Flux<RawRecord>`) que conecta dinámicamente vía JDBC a Microsoft SQL Server (`gestioni_datosNet` y `gestioni_consultorNet` en `server163.tecnoweb.net:1433`) extrayendo de forma incremental solo registros modificados con base en marcas temporales (`FechaModificacion`, `FechaCreacion`, `FechaRegistro`).
- **Esquema Espejo Idéntico Raw (`sgi_raw` / `RawMirrorService.java`)**: Creación dinámica y desacoplada del esquema `sgi_raw` en PostgreSQL, persistiendo réplicas 1:1 de las tablas legadas de Agenda y Consultor como respaldo vivo sin impactar el rendimiento del CRM.
- **Transformación Unificada y Sanitización B2B (`UnifiedTransformService.java`)**: Filtrado estricto por lista blanca de NITs reales colombianos (>= 8 dígitos), mapeo de estados legados (`OpcEstado`) y sincronización automática hacia `sgi.terceros_clientes` y `sgi.usuarios_consultores`.
- **Orquestador Programado Resiliente con Reintentos (`DatabaseSyncScheduler.java`)**: Cronjob configurable vía secretos (`SGI_SYNC_CRON: 0 0 */1 * * *` / `0 0 */3 * * *`), política reactiva de 3 reintentos espaciados cada 5 minutos (`delay-ms: 300000`) y congelamiento de marca temporal (`last_successful_sync`) en caso de fallo para evitar pérdida de datos en la siguiente ventana programada.
- **Control de Marcas de Agua (`SyncWatermarkEntity.java` & `SyncWatermarkRepository.java`)**: Auditoría y control transaccional por tabla en `sgi.sync_watermarks` registrando estado, registros procesados y mensajes de error.
- **Endpoints de Monitoreo y Disparo Manual (`SyncController.java`)**: Habilitados `GET /api/sync/status` (telemetría en vivo) y `POST /api/sync/trigger` (ejecución manual reactiva).

---

## [1.3.0] - 2026-09-08

### 🔒 Seguridad, Autenticación & Gobernanza de Sesiones (SGI CRM)
- **Corrección de Supabase Anon Key (`supabaseClient.ts`)**: Solucionado el error HTTP 401 (`Invalid API key`) al autenticar con Supabase Auth (`/auth/v1/token?grant_type=password`) reemplazando el valor truncado de respaldo por la clave pública anónima formal del proyecto `bmgfqxribkrhbzqvhsjp`. Añadida plantilla `.env.example` e ignorado `.env` en Git.
- **Blindaje contra Retroceso del Navegador (`ProtectedRoute.tsx` & `App.tsx`)**: Implementado componente guardián `ProtectedRoute` envolviendo todas las rutas privadas (`/dashboard`, `/consultor`, `/agenda`, `/clientes`, `/usuarios`, `/perfil`, `/cambiar-password`). Previene el reingreso al presionar "Atrás" en el navegador tras cerrar sesión o expirar el token, limpiando `bfcache` (`pageshow`) y escuchando eventos `SIGNED_OUT` en tiempo real.
- **Eliminación de Auto-Inicialización Insegura (`CrmSidebar.tsx`)**: Removida la lógica que re-creaba usuarios inexistentes con perfil simulado de administrador al navegar a rutas privadas.
- **Cierre Integral de Sesión Multicapa (`authUtils.ts`)**: Implementada función `performCompleteLogout()` que purga `sgi_user` (localStorage/sessionStorage), elimina tokens persistidos de Supabase (`sb-*-auth-token`) e invalida la sesión remota en Supabase Auth (`scope: global/local`).
- **Temporizador de Auto-Redirección a 15 Segundos en Expiración de Sesión (`CrmSidebar.tsx`)**: Integrado contador regresivo de 15 segundos visible en el modal corporativo de seguridad ("Sesión Expirada por Seguridad" y "Acceso Desactivado"). Si el usuario no interactúa pulsando "Reingresar al Sistema SGI", el sistema lo redirige de forma automática e inmediata al login mediante `navigate('/login', { replace: true })`.
- **Monitoreo Continuo de Expiración cada 1 Segundo (`CrmSidebar.tsx`)**: Incorporado chequeo en segundo plano cada 1000ms que evalúa `sgi_session_limit_hours` con soporte reactivo exacto para `⏱️ 10 Segundos (Modo Pruebas)` y expiraciones dinámicas en vivo.
- **Soporte de URL Supabase en Microservicio (`application.yml` & `AuthService.java`)**: Configurada propiedad `supabase.url` con fallback dinámico para la validación de tokens en el endpoint reactivo SSE `/stream-estado`.

---

## [1.2.0] - 2026-09-05

### 🧹 Refactorización & Arquitectura de Repositorios
- **Desacoplamiento de Proyectos Legacy**: Removidas las referencias y punteros anidados de `AgendaSGI` y `ConsultorSGI` del repositorio de SGI, independizándolos como submódulos autónomos directos del holding. Resuelto el fallo de recursión Git en CI/CD (`actions/checkout@v4`).
- **WelcomeController & Telemetría API (`/` y `/api/health`)**: Implementada la pantalla visual de telemetría institucional y el endpoint `/api/health` para consultar el estado del esquema PostgreSQL y el total de clientes (`totalClientesSGI: 67`).
- **Blindaje de Integridad Referencial en Eliminación (`UsuarioController`)**: Corregido error SQL 23503 en `DELETE /api/usuarios/{id}` desvinculando y eliminando previamente los eventos asociados en `sgi.agenda_eventos` para prevenir violaciones de foreign key.
- **Previsualización de Eventos de Agenda en CRM (`UsuariosView.tsx` & `AgendaController`)**: Añadido endpoint `GET /api/agenda/asesor/{asesorId}` y cuadro de advertencia reactivo en el modal de eliminación del CRM, informando al administrador sobre la eliminación en cascada y listando los eventos específicos vinculados al consultor.

---

## [1.1.0] - 2026-09-05

### 🏗️ DevOps & Infraestructura On-Premise
- **sgi-core-service (Contenerización On-Premise)**: Configurada la ejecución soberana en Docker sobre el clúster local PostgreSQL (`waloyo-postgres-prod`), consumiendo de forma aislada el esquema `sgi` y exponiendo el puerto interno 8084.
- **Preparación de Despliegue Zero-Trust**: Ruteo preparado hacia `sgi-api.waloyogroup.com` mediante Cloudflare Tunnels sin requerir apertura de puertos ni IPs públicas.
- **Desmonte de Render en CI/CD**: Eliminada la dependencia del deploy hook de Render; el pipeline de GitHub Actions despliega el servicio directamente en el servidor físico local vía Self-Hosted Runner.

---

## [1.0.0] - 2026-08-03

### Added
- **Gobernanza y documentación final del estado SGI**: Se cerró la revisión del submódulo con registro explícito de la auditoría de cambios no comiteados, validación de compilación y trazabilidad de los cambios de autenticación, usuarios, permisos y sesión.
- **Autenticación corporativa SGI CRM**: Implementado el flujo de login con Supabase Auth usando email y contraseña, validaciones de formulario, recordatorio del correo y bloqueo de acceso para usuarios inactivos.
- **Cambio obligatorio de contraseña**: Añadida la pantalla de primer acceso y cambio de clave temporal `ChangePassword.tsx`, con requisitos de complejidad y validación OWASP en tiempo real.
- **Gestión de usuarios y colaboradores**: Se consolidó en `UsuariosView.tsx` la creación, edición, activación, desactivación, eliminación, reenvío de contraseñas temporales y control del estado de cada asesor.
- **RBAC y permisos por módulo**: Se dejó documentado y funcional el control de roles `ADMIN_TI`, `ADMIN` y `CONSULTOR`, con asignación granular de módulos permitidos y sincronización en el sidebar.
- **Gobernanza de sesiones**: Se implementó la expiración de sesión por tiempo configurable, cierre inmediato por inactividad, modal corporativo y validación continua cada segundo.
- **Normalización de configuración API**: Añadido `apiConfig.ts` y `supabaseClient.ts` para centralizar la conexión y evitar URLs con errores de formato en desarrollo y producción.

### Changed
- **Perfil de usuario y seguridad**: Se mejoró la edición de perfil con restricciones por rol, permitiendo al `ADMIN_TI` ajustes completos y manteniendo la política de solo lectura para consultores y administradores limitados.
- **Backend `sgi-core-service`**: Se ajustó `UsuarioController.java` para soportar edición, eliminación, reactivación, reenvío de credenciales, validación de clave y control de sesión por cuenta.
- **CORS y dominios**: Se reforzó la seguridad restringiendo orígenes permitidos a los dominios corporativos y entornos locales, evitando comodines abiertos.
- **Landing e integración SGI**: Se actualizó la navegación y la landing para apuntar a la experiencia CRM y mantener una integración coherente entre landing pública y panel administrativo.
- **Documentación del submódulo**: Se dejó el changelog con trazabilidad completa del último bloque de trabajo y del estado final antes del commit y push.

### Fixed
- **Normalización defensiva de la API SGI (`crm/src/config/apiConfig.ts`)**: Corregida la resolución de `VITE_SGI_API_URL` para usar `localhost` en desarrollo y devolver `/api` por defecto cuando la variable no está configurada, evitando errores de routing y 404.
- **Compatibilidad del endpoint de verificación de usuarios (`sgi-core-service/src/main/java/com/waloyo/sgi/controller/UsuarioController.java`)**: Reemplazado el uso de `Map.of` por `Map.ofEntries` para soportar respuestas con valores nulos sin romper la serialización JSON del backend.
- **Validación programática de formularios**: Se corrigieron validaciones de email, teléfono, documento y nombres, evitando datos inconsistentes en la base de datos y mejorando la UX del usuario.
- **Expulsión inmediata de cuentas inactivas**: Arreglado el flujo para cerrar sesión al detectar cuentas desactivadas y mostrar mensajes corporativos claro al usuario.
- **Reenvío de credenciales**: Corregido el flujo para generar claves temporales, marcar `mustChangePassword` y notificar la nueva credencial de forma fiable.
- **Estado de compilación**: Se ejecutó validación final de compilación del frontend SGI antes de cerrar el conjunto de cambios.

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
