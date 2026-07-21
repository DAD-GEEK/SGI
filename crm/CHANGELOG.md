# Changelog — SGI CRM (React 19 SPA)

Todos los cambios relevantes del cliente CRM B2B de Gestión Integral SGI se registran en este archivo.

## [Sin Versionar] - 2026-07-21

### Added
- **Modulo de Clientes (`src/pages/ClientesView.tsx`)**:
  - Paginación dinámica configurable con valor predeterminado desde **5 registros por vista** (5, 10, 20, 50, 100).
  - Integración reactiva en tiempo real vía Server-Sent Events (`/api/clientes/stream`).
  - Modal con paridad 1:1 de `_CrearCliente.cshtml` / `_EditarCliente.cshtml`:
    - Pestaña **Información General**: NIT con validador de longitud, **Dígito de Verificación (DV DIAN)** autocalculado con Módulo 11 (readonly), Razón Social, Nombre Comercial, Teléfono, Email, Dirección, **Selector de 1,120 Municipios DANE de Colombia**, Página Web y Fecha de Ingreso.
    - Pestaña **Contratos**: Vigencia inicial y final, Horas mensuales, Valor en Pesos COP (`$ 1.600.000 COP`), Selección múltiple de Asesores SGI (*María Elisa*, *Laura Natali*, *Lesly Velez*, *Milena Valencia*) y Sistemas normativos (*SG-SST*, *SGC*, *SGI*, *PESV*).
    - Pestaña **Contactos**: Nombre, Cargo, Teléfono fijo, Celular y Email.
- **Barra Lateral Interactiva (`src/components/CrmSidebar.tsx`)**:
  - Expansión/colapso con hover del mouse (`onMouseEnter`/`onMouseLeave`) y botón de pin fijo.
- **Flujo de Pantalla Fluido (`src/pages/Dashboard.tsx` & `ClientesView.tsx`)**:
  - Removido `max-w-7xl` para que el contenedor `<main>` ocupe el 100% de la pantalla cuando la barra lateral está colapsada.
- **Suite de Pruebas Unitarias (`src/pages/__tests__/ClientesView.test.tsx`)**:
  - Pruebas de renderizado de componentes y paginación con Vitest.
