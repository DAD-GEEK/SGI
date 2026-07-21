# 🚀 Plan Maestro — CRM & Software B2B de Gestión Integral SGI

---

## 1. Visión y Objetivos del Software CRM

El módulo **SGI CRM & Software B2B** es la plataforma privada de operación para asesores, consultores y clientes de **Gestión Integral SGI S.A.S.** Permite la gestión de auditorías, el seguimiento de compromisos del SG-SST/PESV, la administración de usuarios y el control de tableros de mando en tiempo real.

---

## 2. Arquitectura de Desacoplamiento Total (Landing vs. CRM)

Para garantizar **alta disponibilidad y aislamiento de fallos (Fault Isolation)**, la aplicación comercial Landing (`gestionintegralsgi.com.co`) y el Software CRM (`app.gestionintegralsgi.com.co`) se construyen como **dos proyectos independientes y desacoplados**:

```
apps/client/SGI/
├── apps/ (o carpetas desacopladas)
│   ├── landing/   # Aplicación pública comercial (Puerto 3004)
│   └── crm/       # Software B2B privado (Puerto 3005)
```

### Beneficios del Desacoplamiento:
1. **Resiliencia Operativa**: Si la Landing sufre mantenimiento o alta carga de tráfico comercial, el CRM de los asesores sigue operando al 100% sin interrupciones.
2. **Independencia de Build y Despliegue**: Cada aplicación genera su propio bundle empaquetado (`dist`), con sus propias rutas (`React Router DOM`) y dependencias optimizadas.
3. **Seguridad RBAC / SSO**: El CRM se integra directamente con **Keycloak (OAuth2 / OIDC)** y APIs restringidas, mientras la Landing permanece como sitio público estático servido por CDN.

---

## 3. Módulos y Pantallas del CRM (Basados en `stitch_sgi_digital_ecosystem_transformation`)

| Módulo / Ruta | Plantilla de Origen | Descripción & Funcionalidades |
|---|---|---|
| **`Login` (`/login`)** | `sgi_crm_inicio_de_sesi_n` | Autenticación segura para Asesores, Clientes y Administradores, recuperación de contraseña y branding SGI Unified Core. |
| **`Dashboard` (`/dashboard`)** | `sgi_crm_dashboard_panel_expandido` | Tablero principal expandible con KPIs de auditoría, calendario de citas, gráficos de cumplimiento PHVA y estado de compromisos. |
| **`Perfil & Preferencias` (`/perfil`)** | `sgi_crm_perfil_y_preferencias` | Configuración de datos del consultor/cliente, notificaciones, cambio de clave y preferencias de la cuenta. |

---

## 4. Stack Tecnológico del CRM

- **Core**: React 19 + TypeScript + Vite 6
- **Estilos**: Tailwind CSS v4 + SGI Unified Core Tokens (`#055bb2`, `#1e293b`, `Manrope`, `Inter`)
- **Iconografía**: Lucide React + Material Symbols Outlined
- **Ruteo**: React Router DOM v7
- **Puerto Local**: `http://localhost:3005/`

---

> **Waloyo Group Tech Governance** — *Tecnología resiliente. Operación continua.*
