# 📋 Documentación de Errores, Lecciones Aprendidas y Reglas de Calidad (SGI CRM)

---

## 1. Bitácora de Errores Identificados y Solucionados

### ❌ Error 1: Contaminación de Datos con Registros Incompletos o Ficticios
- **Síntoma / Causa Raíz**: Durante el primer proceso ETL, se incluyeron NITs ficticios o de prueba (como `0`, `1`, `4`, `10`, `12345678`) y clientes inactivos sin validar su estado `OpcEstado`.
- **Impacto**: La lista de clientes mostraba registros inválidos o nombres incompletos.
- **Acción Correctora**: Se ajustó `mssql_to_postgresql_etl.py` con una lista blanca estricta (`len(clean_nit) >= 8` y filtrado de ceros/placeholders) garantizando únicamente los **67 clientes reales B2B** (47 Activos y 20 Inactivos).

---

### ❌ Error 2: Conflicto de Rutas en Spring MVC (`/stream` vs `/{id}`)
- **Síntoma / Causa Raíz**: El endpoint SSE `/api/clientes/stream` arrojaba `MethodArgumentTypeMismatchException` porque Spring Boot intentaba mapear el string `"stream"` como un parámetro UUID al endpoint `GET /api/clientes/{id}`.
- **Impacto**: Error HTTP 500 en las peticiones del navegador a la API reactiva.
- **Acción Correctora**: Se reordenaron las anotaciones `@GetMapping` en `ClienteController.java` colocando la ruta específica `/stream` **antes** que la ruta paramétrica `/{id}`.

---

### ❌ Error 3: Omisión Inicial de Campos Legados en la Interfaz React 19
- **Síntoma / Causa Raíz**: En el primer rediseño del modal de clientes, se omitieron campos clave de `_CrearCliente.cshtml` como el cálculo automático del **Dígito de Verificación (DV DIAN)**, la lista desplegable de **ciudades DANE Colombia**, el sitio web, las fechas de ingreso, valores de contratos en COP, selección de asesores SGI y celular de contacto.
- **Impacto**: Falta de paridad 1:1 con el sistema original de Gestión Integral SGI.
- **Acción Correctora**: Se realizó una auditoría completa del HTML de `_CrearCliente.cshtml` y se reconstruyó `ClientesView.tsx` incorporando el 100% de los campos en tres pestañas interactivas (`Información general`, `Contratos`, `Contactos`).

---

### ❌ Error 4: Importaciones no Utilizadas en el Frontend
- **Síntoma / Causa Raíz**: Al refactorizar los íconos Lucide, quedaron imports sin uso (`Filter`, `DollarSign`, `Menu`, `ChevronRight`) que fallaban la verificación de TypeScript en `npm run build`.
- **Impacto**: Fallo en la compilación estricta de producción.
- **Acción Correctora**: Limpieza de todos los imports no utilizados y ejecución exitosa de `npm run build` en 16 segundos.

---

## 2. Propuesta de Nuevas Reglas de Gobernanza en `.agents/AGENTS.md`

Solicitamos la aprobación verbal del usuario para incorporar las siguientes directrices al archivo de reglas del holding:

```markdown
## 🛡️ Regla: Protocolo de Datos y Paridad 1:1 en Migraciones Legacy

1. **Sanitización de Datos en ETL**:
   - Prohibido importar registros ficticios o placeholders (`NIT: 0`, `NIT: 12345678`).
   - Aplicar listas blancas de unicidad y formato oficial (mínimo 8 dígitos para NITs de empresas colombianas).
   - Preservar exactamente el mapeo de estados legados (`OpcEstado`: 47 Activos, 20 Inactivos).

2. **Precedencia de Rutas en Controladores REST**:
   - En Spring Boot o Express, las rutas específicas (ej. `/stream`, `/rango`, `/cliente/{id}`) deben declararse SIEMPRE antes que las rutas paramétricas genéricas (ej. `/{id}`).

3. **Paridad Total de Campos de Formularios**:
   - Antes de diseñar un formulario o modal, el agente debe auditar la plantilla Razor/HTML legada e implementar el 100% de los campos y validaciones sin omitir nada.

4. **Compilación Limpia Obligatoria (`npm run build`)**:
   - Todo componente debe pasar `npm run build` sin errores de TypeScript ni advertencias de importaciones no utilizadas antes de darlo por completado.
```

---

## 3. Cobertura de Pruebas Automatizadas (Backend & Frontend)

### Backend (Spring Boot 3 - JUnit 5)
- Archivo: `sgi-core-service/src/test/java/com/waloyo/sgi/controller/ClienteControllerTest.java`
- Pruebas incluidas:
  - `testObtenerTodosLosClientes`: Verifica la respuesta de los 67 clientes reales.
  - `testCrearCliente`: Prueba la inserción de un nuevo cliente y asignación de UUID.
  - `testDesactivarCliente`: Verifica el cambio de estado a INACTIVO sin borrado físico.

### Frontend (React 19 - Vitest & React Testing Library)
- Archivo: `apps/client/SGI/crm/src/pages/__tests__/ClientesView.test.tsx`
- Pruebas incluidas:
  - Verificación del renderizado del encabezado y conexión SSE.
  - Botón de acción para creación de clientes.
  - Verificación de la paginación predeterminada a 5 registros.

---

> **Waloyo Group Quality & Governance** — *Tecnología resiliente. Operación continua.*
