# 🏢 Gestión Integral SGI — Portal Web & Plataforma B2B

![Gestión Integral SGI](https://images.unsplash.com/photo-1522071820081-009f0129c71c?auto=format&fit=crop&w=1200&q=80)

Bienvenido al repositorio oficial del portal web institucional y comercial de **Gestión Integral SGI S.A.S.** (`SU ASESOR… SU ALIADO`). Este proyecto pertenece al ecosistema de clientes B2B de Waloyo Group y está maquetado e implementado bajo los más altos estándares de desarrollo web, accesibilidad y diseño responsivo mobile-first.

---

## 🛠️ Stack Tecnológico

| Componente | Tecnología | Versión |
|---|---|---|
| **Core Framework** | React | `^19.0.0` |
| **Bundler / Build Tool** | Vite | `^6.1.0` |
| **Lenguaje** | TypeScript | `^5.7.3` |
| **Estilos & CSS** | Tailwind CSS v4 | `^4.0.0` (`@tailwindcss/vite`) |
| **Enrutamiento** | React Router DOM | `^7.1.5` |
| **Iconografía** | Lucide React + Material Symbols | `^0.475.0` |

---

## 🎨 Sistema de Diseño: SGI Unified Core

El proyecto adopta formalmente la especificación **SGI Unified Core** definida en `platillas diseño/DESIGN.md`:

- **Primary Blue (`#055bb2` / `#3b7ad3`)**: Color corporativo principal para CTAs, estados activos e identidad visual.
- **Secondary Navy (`#1e293b` / `#545f73`)**: Azul pizarra oscuro para titulares, estructuras y footer.
- **Tipografía Dual**:
  - **Titulares & Headings**: `Manrope` (700 Bold / 600 SemiBold) — Moderno, geométrico y corporativo.
  - **Cuerpo, Tablas & Formularios**: `Inter` (400 Regular / 600 SemiBold) — Máxima lecturabilidad B2B.
- **Bordero & Formas**: Radio soft-geometric de `8px` (`rounded-lg`) y sombras difusas (`elevation-1` / `elevation-2`).
- **Soporte Safe Area Mobile**: Implementación nativa de `pb-safe` y `viewport-fit=cover` para compatibilidad total con barras de gestos/navegación de iOS Safari y Android Chrome.

---

## 📁 Estructura del Proyecto

```
apps/client/SGI/
├── AgendaSGI/            # Código legacy referencia (.NET C# / SQL Server)
├── ConsultorSGI/         # Código legacy referencia (.NET C# / SQL Server)
├── docs/                 # Documentación técnica y arquitectura
│   ├── plan_sgi.md       # Plan Maestro de Onboarding e Ingeniería
│   └── arquitectura_sgi.md # Documento de Arquitectura y Migración
├── platillas diseño/     # Diseños fuente en HTML/CSS y capturas de pantalla
├── src/
│   ├── components/       # Componentes reutilizables (Navbar, Footer, etc.)
│   ├── pages/            # Vistas principales (Home, Services, PESV)
│   ├── App.tsx           # Ruteo dinámico
│   ├── main.tsx          # Punto de entrada de React
│   └── index.css         # Importación de Tailwind CSS v4 y tokens de diseño
├── CHANGELOG.md          # Bitácora de cambios del proyecto SGI
├── index.html            # Plantilla HTML base con fuentes de Google
├── package.json          # Dependencias y scripts
├── tsconfig.json         # Configuración estricta de TypeScript
├── vite.config.ts        # Configuración del empaquetador Vite
└── README.md             # Documentación oficial del repositorio
```

---

## 🚀 Scripts Disponibles

En el directorio del proyecto, puedes ejecutar:

### `npm run dev`
Inicia el servidor de desarrollo local de Vite en `http://localhost:3004/`. Habilita Hot Module Replacement (HMR) instantáneo.

### `npm run build`
Compila la aplicación para producción ejecutando `tsc -b` (verificación de tipos de TypeScript) y `vite build` (generación de bundles optimizados en `dist/`).

### `npm run preview`
Previsualiza de forma local el build de producción generado en la carpeta `dist/`.

---

## 📱 Normas de Responsividad y Accesibilidad

1. **Mobile-First (320px+)**: Todo componente se adapta desde pantallas móviles ultra-pequeñas hasta monitores ultra-wide (1280px max-container).
2. **Touch Targets Protegidos**: Área mínima de contacto táctil de `44px x 44px` en botones e inputs móviles.
3. **Barra de Navegación Inferior Móvil (Bottom Nav)**: Activación automática de barra flotante en viewports `< 768px` alineada con `pb-safe` para evitar colisiones con las barras del sistema operativo del smartphone.

---

## ⚖️ Licencia y Propiedad

Este repositorio contiene código propietario desarrollado por **Waloyo Group** para **Gestión Integral SGI S.A.S.** Queda prohibida la reproducción o distribución no autorizada sin la aprobación explícita de ambas partes.

> **Desarrollado por Waloyo Group** — *Tecnología resiliente. Operación continua.*
