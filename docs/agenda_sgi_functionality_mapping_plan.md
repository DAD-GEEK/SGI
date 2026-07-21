# 🗺️ Plan Maestro de Migración 1 a 1 Ultradetallado: AgendaSGI & ConsultorSGI (ASP.NET MVC) a Spring Boot 3 + React 19 CRM

---

## 1. Mapeo de Arquitectura y Base de Datos Unificada

- **Base de Datos Unificada**: PostgreSQL 15 (`waloyodb`, esquema `sgi` en `127.0.0.1:5432`).
- **Backend Microservicio**: Spring Boot 3.2+ WebFlux & JPA (`sgi-core-service` en Puerto `8084`).
- **Rutas REST puras (sin `/v1`)**: `/api/clientes`, `/api/contratos`, `/api/contactos`, `/api/agenda`, `/api/actas`, `/api/auditorias`, `/api/usuarios`, `/api/configuracion`.
- **Transmisión Reactiva en Tiempo Real**: Spring Boot `Flux<ServerSentEvent<...>>` a través de browser native `EventSource`.
- **Frontend CRM B2B**: React 19 SPA + Vite + Material 3 Design System (`apps/client/SGI/crm` en Puerto `3005`).

---

## 2. Desglose 1 a 1 Exhaustivo de Módulos y Funcionalidades Legadas

---

### Módulo 1: Clientes (`ClientesController.cs` / `Views/Clientes/`)
- **Vistas C# Razor Legadas**: 
  - `Index.cshtml`
  - `_GetAllClientes.cshtml`
  - `_CrearCliente.cshtml`
  - `_EditarCliente.cshtml`
  - `_EditarContrato.cshtml`
  - `_GetAllContratos.cshtml`
  - `_GetAllContactos.cshtml`
  - `_GetAllProcesos.cshtml`
- **Campos del Modal de Creación / Edición (`_CrearCliente.cshtml` / `_EditarCliente.cshtml`)**:
  - **Pestaña 1: Información General (`#generalTab`)**:
    1. `clienteIdentificacion`: NIT / Cédula (obligatorio, 7 a 13 dígitos).
    2. `clienteDV`: Dígito de Verificación DIAN autocalculado con fórmula Módulo 11 (readonly).
    3. `clienteNombre`: Nombre / Razón Social (obligatorio).
    4. `nombreComercial`: Nombre comercial de la empresa.
    5. `clienteTelefono`: Teléfono de contacto (obligatorio).
    6. `clienteEmail`: Email corporativo principal (obligatorio, formateo automático en minúsculas).
    7. `clienteDireccion`: Dirección de la sede principal.
    8. `clienteCiudad`: Código DANE y Ciudad oficial (obligatorio, dropdown de 1,120 municipios de Colombia).
    9. `clienteWeb`: Dirección del sitio web.
    10. `clienteFechaIngreso`: Fecha de ingreso / vinculación del cliente.
    11. `OpcEstado`: Estado de habilitación (47 Activos, 20 Inactivos).
  - **Pestaña 2: Contratos (`#contratosTab`)**:
    1. `fechaInicialContrato`: Fecha de inicio de vigencia (obligatorio).
    2. `fechaFinalContrato`: Fecha de finalización de vigencia (obligatorio).
    3. `horasContrato`: Horas mensuales contratadas (obligatorio).
    4. `valorContrato`: Valor mensual del contrato en Pesos COP (obligatorio).
    5. `asesorasContrato`: Selección de Asesores asignados (`María Elisa Arias`, `Laura Natali Tenorio Arias`, `Lesly Velez Alvarez`, `Milena Valencia`).
    6. `sistemasContrato`: Selección de Sistemas de Gestión (`SG-SST`, `SGC ISO 9001`, `SGI Integrado`, `PESV`).
    7. `tblContratos`: Tabla dinámica en memoria que muestra el listado previo de contratos agregados antes de guardar.
  - **Pestaña 3: Contactos (`#contactosTab`)**:
    1. `nombreContacto`: Nombre completo de la persona de contacto (obligatorio).
    2. `cargoContacto`: Cargo directivo o técnico (obligatorio).
    3. `telefonoContacto`: Teléfono fijo de oficina.
    4. `celularContacto`: Celular de contacto directo (obligatorio).
    5. `emailContacto`: Correo electrónico del contacto (obligatorio).
    6. `tblContactos`: Tabla dinámica en memoria que muestra el listado previo de contactos agregados antes de guardar.
- **Funcionalidades de la Tabla Principal (`Index.cshtml` / `_GetAllClientes.cshtml`)**:
  - Paginación dinámica configurable comenzando desde **5 registros por página** (opciones: 5, 10, 20, 50, 100).
  - Selector de filtro por estado (`Todos`, `Activos (47)`, `Inactivos (20)`).
  - Buscador global instantáneo por NIT, Razón Social o Email.
  - Transmisión en tiempo real vía Server-Sent Events (`/api/clientes/stream`).
  - Acciones por fila: Editar cliente/contratos/contactos (`_EditarCliente.cshtml`) y Desactivar (`DELETE /api/clientes/{id}`).
- **Backend Spring Boot**: `ClienteController.java`, `ContratoController.java`, `ContactoController.java`.
- **Frontend React 19**: [`ClientesView.tsx`](file:///D:/Waloyo/WaloyoGroup/apps/client/SGI/crm/src/pages/ClientesView.tsx).

---

### Módulo 2: Agenda Interactiva & Tiempos de Asesoría (`AgendaController.cs` / `Views/Agenda/`)
- **Vistas C# Razor Legadas**:
  - `Index.cshtml`
  - `_AgregarCita.cshtml`
  - `_GetCalendario.cshtml`
  - `_GetEvento.cshtml`
  - `TiemposAsesorias.cshtml`
  - `_GetTiemposAsesoria.cshtml`
- **Campos del Modal de Agendamiento (`_AgregarCita.cshtml`)**:
  1. `IntClienteID`: Cliente receptor de la asesoría (obligatorio, búsqueda con autocompletado).
  2. `IntUsuarioID`: Asesor / Consultor SGI responsable (obligatorio).
  3. `StrTitulo`: Título / Asunto del compromiso (obligatorio).
  4. `StrDescripcion`: Descripción detallada o temario a desarrollar.
  5. `DatFechaInicial`: Fecha y hora de inicio (datepicker con selector de hora).
  6. `DatFechaFinal`: Fecha y hora de finalización.
  7. `IntTipoEventoID`: Categoría del evento (Capacitación, Auditoría, Asesoría SG-SST, Inspección, PESV).
  8. `OpcEstado`: Estado de la cita (`PROGRAMADO`, `EJECUTADO`, `CANCELADO`).
- **Funcionalidades del Calendario y Control de Tiempos**:
  - **8,395 eventos de agenda históricos** migrados en `sgi.agenda_eventos`.
  - Vistas interactivas: Mensual, Semanal, Diaria y Agenda en Lista.
  - Arrastrar y soltar (Drag and drop) para reprogramar citas de asesores.
  - **Informe de Tiempos de Asesoría (`TiemposAsesorias.cshtml`)**: Comparativo en tiempo real entre las horas ejecutadas por el asesor vs. las horas contratadas mensualmente por el cliente.
- **Backend Spring Boot**: `AgendaController.java` (`/api/agenda`, `/api/agenda/rango`, `/api/agenda/tiempos`).
- **Frontend React 19**: `AgendaView.tsx`, `AgendaCalendar.tsx`.

---

### Módulo 3: Actas de Visita Técnica (`ActasController.cs` / `Views/Actas/`)
- **Vistas C# Razor Legadas**:
  - `Index.cshtml`
  - `_CrearActa.cshtml`
  - `_EditarActa.cshtml`
  - `_EditarActividades.cshtml`
  - `_ActaPDF.cshtml`
  - `_AgregarAsistentes.cshtml`
  - `_GetAllActasByCliente.cshtml`
  - `_GetAllCompromisos.cshtml`
  - `_GetAllTemas.cshtml`
  - `_GetVistaPreviaActa.cshtml`
- **Campos y Estructura del Acta de Visita (`_CrearActa.cshtml`)**:
  1. `IntAgendaID`: Cita de origen de la agenda (vínculo obligatorio).
  2. `StrConsecutivo`: Número de acta consecutivo automático (ej: `ACTA-2026-089`).
  3. `DatFechaVisita`: Fecha y hora de realización.
  4. `StrObjetivo`: Objetivo de la sesión de trabajo.
  5. `StrTemasTratados`: Desarrollo de los temas abordados.
  6. `StrConclusiones`: Conclusiones y recomendaciones del consultor SGI.
  7. **Asistentes de la Reunión (`_AgregarAsistentes.cshtml`)**: Tabla de participantes con Nombre, Cargo, Email y Firma.
  8. **Matriz de Compromisos (`_EditarActividades.cshtml`)**: Registro de tareas asignadas durante la visita (Descripción, Responsable, Fecha Límite).
  9. **Generación e Impresión PDF (`_ActaPDF.cshtml`)**: Renderizado en PDF con plantilla institucional de SGI para descarga o firma digital.
- **Backend Spring Boot**: `ActaController.java` (`/api/actas`, `/api/actas/{id}/pdf`).
- **Frontend React 19**: `ActasVisitaView.tsx`, `ActaModal.tsx`, `ActaPdfViewer.tsx`.

---

### Módulo 4: Compromisos y Tareas Derivadas (`ActividadesController.cs`)
- **Vistas C# Razor Legadas**:
  - `Index.cshtml`
  - `_GetAllCompromisos.cshtml`
  - `_MarcarCumplido.cshtml`
- **Campos y Matriz PHVA de Seguimiento**:
  1. `StrDescripcion`: Detalle de la tarea o compromiso normativo.
  2. `IntClienteID`: Cliente asociado.
  3. `StrResponsable`: Persona responsable (Asesor SGI o Funcionario del Cliente).
  4. `DatFechaLimite`: Fecha límite de entrega.
  5. `IntPorcentajeAvance`: Porcentaje de avance de la actividad (0% a 100%).
  6. `StrEstado`: Estado del compromiso (`PENDIENTE`, `EN_PROCESO`, `CUMPLIDO`, `VENCIDO`).
- **Backend Spring Boot**: `ActividadController.java` (`/api/actividades`).
- **Frontend React 19**: `CompromisosView.tsx`.

---

### Módulo 5: Auditorías y Diagnósticos de Calidad (`AuditoriaController.cs` / `Views/Auditoria/`)
- **Vistas C# Razor Legadas**:
  - `Index.cshtml`
  - `_ProgramarAuditoria.cshtml`
  - `_GetAllAuditorias.cshtml`
- **Campos y Evaluación Normativa (`_ProgramarAuditoria.cshtml`)**:
  1. `IntTerceroClienteID`: Cliente auditado.
  2. `StrUsuarioID`: Auditor Líder asignado.
  3. `IntNormaID`: Norma a evaluar (`ISO 9001:2015`, `ISO 14001:2015`, `ISO 45001:2018`, `Res. 0312/2019`, `PESV`).
  4. `StrCodigo`: Código de auditoría (ej: `AUD-2026-005`).
  5. `DatFechaInicial` y `DatFechaFinal`: Fechas de ejecución de auditoría.
  6. `PorcentajeCumplimiento`: Porcentaje global obtenido (ej: `94.2%`).
- **Backend Spring Boot**: `AuditoriaController.java` (`/api/auditorias`).
- **Frontend React 19**: `AuditoriasView.tsx`.

---

### Módulo 6: Usuarios, Asesores y Roles (`UsuariosController.cs` / `Views/Usuarios/`)
- **Vistas C# Razor Legadas**:
  - `Index.cshtml`
  - `_CrearUsuario.cshtml`
  - `_EditarUsuario.cshtml`
  - `_GetAllUsuarios.cshtml`
- **Campos de Gestión del Personal (`_CrearUsuario.cshtml`)**:
  1. `StrDocumento`: Cédula / Documento de identidad (obligatorio).
  2. `StrNombre`: Nombre completo del consultor.
  3. `StrEmail`: Email corporativo para acceso y notificaciones.
  4. `StrRol`: Rol en la plataforma (`ADMIN`, `ASESOR_SENIOR`, `AUDITOR`).
  5. `OpcEstado`: Habilitación del usuario (13 Asesores y Consultores activos en base de datos).
- **Backend Spring Boot**: `UsuarioController.java` (`/api/usuarios`).
- **Frontend React 19**: [`UsuariosView.tsx`](file:///D:/Waloyo/WaloyoGroup/apps/client/SGI/crm/src/pages/UsuariosView.tsx).

---

### Módulo 7: Catálogos del Sistema & Configuración (`ConfiguracionController.cs`, `NumeralesController.cs`, `PreguntasController.cs`, `TipoEventosController.cs`)
- **Gestión de Listas Maestras**:
  - **Tipos de Eventos / Asesorías**: Capacitación, Auditoría, Inspección, Asesoría Técnica.
  - **Numerales Normativos**: Capítulos y numerales de las normas ISO y Res. 0312.
  - **Preguntas de Verificación**: Banco de preguntas de evaluación PHVA.
  - **Configuración General**: Parámetros globales de notificaciones por email e integraciones.
- **Backend Spring Boot**: `ConfiguracionController.java` (`/api/configuracion`).
- **Frontend React 19**: `ConfiguracionView.tsx`.

---

### Módulo 8: Reportes e Informes Ejecutivos (`InformesController.cs` / `Views/Informes/`)
- **Vistas C# Razor Legadas**:
  - `Index.cshtml`
  - `_GetBarrasHorasByAnoByAsesor.cshtml`
  - `_GetHorasContratoVSHorasMensuales.cshtml`
- **Indicadores y Gráficos Visuales**:
  - Gráfico de barras de horas ejecutadas por asesor por año.
  - Tablas comparativas de Horas de Contrato vs. Horas Mensuales efectivamente prestadas.
  - Exportación de informes en formatos Excel y PDF.
- **Backend Spring Boot**: `InformeController.java` (`/api/informes/horas-asesor`, `/api/informes/contrato-vs-ejecutado`).
- **Frontend React 19**: `InformesView.tsx`.

---

> **Waloyo Group Migration Governance** — *Garantía de Fidelidad 1:1. Paridad Total de Campos, Vistas y Operatividad.*
