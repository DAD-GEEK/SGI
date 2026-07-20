# Changelog — Gestión Integral SGI (SGI Web)

Todos los cambios relevantes de la aplicación web y plataforma comercial del cliente **Gestión Integral SGI S.A.S.** serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/).

---

## [Sin Versionar] - 2026-07-20

### Added
- **Inicialización de Repositorio y Proyecto Web (`apps/client/SGI`)**: Creado el proyecto en React 19 + Vite 6 + TypeScript + Tailwind CSS v4 + React Router DOM.
- **Sistema de Diseño `SGI Unified Core`**: Implementado en `src/index.css` con la paleta de colores oficial (`#055bb2` Azul SGI, `#1e293b` Azul Pizarra), tipografías `Manrope` e `Inter` y componentes con radio `8px` (`rounded-lg`).
- **Navegación Móvil y Safe Area Insets**:
  - Implementado el componente de **Bottom Navigation Bar** flotante para dispositivos móviles (`< 768px`) en `src/components/Navbar.tsx`.
  - Configurado el soporte para barras del sistema operativo móvil (`pb-safe`, `pt-safe`, y `viewport-fit=cover` en `index.html`).
- **Fidelidad Pixel-Perfect de Plantilla Móvil (`platillas diseño/version mobile/`)**:
  - Reestructurada la vista de Inicio en [Home.tsx](file:///D:/Waloyo/WaloyoGroup/apps/client/SGI/src/pages/Home.tsx) para renderizar de forma nativa la plantilla móvil `gesti_n_integral_sgi_home_m_vil/code.html` en viewports `< 768px`.
  - Incorporado el titular móvil `SU ASESOR... SU ALIADO`, botón de `Acceso Software/CRM` táctil, tarjetas de Misión/Visión con barra de acento vertical e indicadores de confianza (`10+ Años`, `100% Compromiso`).
- **Implementación de Plantillas de Diseño (1 a 1)**:
  - **`Home.tsx`**: Landing comercial responsiva con Hero corporativo, sección de Misión/Visión, Bento Grid de especialidades, barra de estadísticas y CTA directo a WhatsApp.
  - **`Services.tsx`**: Vista de servicios en detalle (SG-SST Decreto 1072 & Res. 0312, SGC ISO 9001, SGI Integrado y Auditorías Internas).
  - **`PESV.tsx`**: Vista especializada del Plan Estratégico de Seguridad Vial bajo la Ley 2251/2022 y Res. 40595.
- **Documentación Completa**: Creado el `README.md` del repositorio, el Plan Maestro `docs/plan_sgi.md` y la guía de arquitectura `docs/arquitectura_sgi.md`.
