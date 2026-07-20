---
name: SGI Unified Core
colors:
  surface: '#f7f9fb'
  surface-dim: '#d8dadc'
  surface-bright: '#f7f9fb'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f2f4f6'
  surface-container: '#eceef0'
  surface-container-high: '#e6e8ea'
  surface-container-highest: '#e0e3e5'
  on-surface: '#191c1e'
  on-surface-variant: '#424752'
  inverse-surface: '#2d3133'
  inverse-on-surface: '#eff1f3'
  outline: '#727783'
  outline-variant: '#c2c6d4'
  surface-tint: '#0d5db5'
  primary: '#055bb2'
  on-primary: '#ffffff'
  primary-container: '#3374cd'
  on-primary-container: '#fefcff'
  inverse-primary: '#a9c7ff'
  secondary: '#545f73'
  on-secondary: '#ffffff'
  secondary-container: '#d5e0f8'
  on-secondary-container: '#586377'
  tertiary: '#4d5d73'
  on-tertiary: '#ffffff'
  tertiary-container: '#66768d'
  on-tertiary-container: '#fdfcff'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#d6e3ff'
  primary-fixed-dim: '#a9c7ff'
  on-primary-fixed: '#001b3e'
  on-primary-fixed-variant: '#00468c'
  secondary-fixed: '#d8e3fb'
  secondary-fixed-dim: '#bcc7de'
  on-secondary-fixed: '#111c2d'
  on-secondary-fixed-variant: '#3c475a'
  tertiary-fixed: '#d3e4fe'
  tertiary-fixed-dim: '#b7c8e1'
  on-tertiary-fixed: '#0b1c30'
  on-tertiary-fixed-variant: '#38485d'
  background: '#f7f9fb'
  on-background: '#191c1e'
  surface-variant: '#e0e3e5'
typography:
  display-lg:
    fontFamily: Manrope
    fontSize: 48px
    fontWeight: '700'
    lineHeight: 56px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Manrope
    fontSize: 32px
    fontWeight: '600'
    lineHeight: 40px
  headline-md:
    fontFamily: Manrope
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  headline-sm:
    fontFamily: Manrope
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
  body-lg:
    fontFamily: Inter
    fontSize: 18px
    fontWeight: '400'
    lineHeight: 28px
  body-md:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-sm:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  label-md:
    fontFamily: Inter
    fontSize: 12px
    fontWeight: '600'
    lineHeight: 16px
    letterSpacing: 0.05em
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  unit: 8px
  container-max: 1280px
  gutter: 24px
  margin-mobile: 16px
  margin-desktop: 40px
---

## Brand & Style
The design system is engineered to project a persona of **Reliable Innovation**. It bridges the gap between traditional corporate consultancy and modern SaaS efficiency. The target audience—business leaders and operations managers—expects a tool that feels authoritative yet effortless to navigate.

The aesthetic follows a **Corporate Modern** approach. It utilizes expansive whitespace to reduce cognitive load in data-heavy CRM environments, paired with subtle technical accents (like precision borders and refined shadows) that signal a tech-forward evolution. The visual mood is calm, structured, and profoundly professional.

## Colors
The palette is rooted in a refined **Corporate Blue**, pulled directly from the core SGI brand identity. This blue serves as the primary action color, used for CTA buttons, active states, and key navigational elements.

- **Primary (#3b7ad3):** Professional blue for high-priority interactions and brand presence.
- **Secondary (#1e293b):** A deep slate-navy for headers, text primary, and structural foundations.
- **Surface & Backgrounds:** The design relies on a hierarchy of whites and cool grays (`#ffffff` for cards, `#f8fafc` for page backgrounds, and `#f1f5f9` for subtle sectioning) to maintain a spacious, breathable feel.
- **Success/Warning/Error:** Standardized semantic colors should be desaturated to fit the corporate tone, avoiding overly neon or aggressive hues.

## Typography
This design system uses a dual-font strategy to balance character with utility. 

**Manrope** is used for headlines to provide a modern, geometric, and tech-forward feel. Its wide apertures and balanced proportions ensure titles feel approachable yet authoritative.

**Inter** is the workhorse for all body copy, data tables, and input fields. Chosen for its exceptional legibility in CRM environments, it maintains clarity even at small sizes and high information densities. All body text should adhere to a 1.5x line-height ratio to ensure maximum readability during prolonged usage sessions.

## Layout & Spacing
The layout follows a **Fluid-Fixed Hybrid** model. While the outer container caps at 1280px to prevent excessive line lengths on ultra-wide monitors, internal elements utilize a fluid 12-column grid.

- **Spacing Rhythm:** Based on an 8px base unit. 
- **Density:** The design system prioritizes "Medium Density." Information is grouped logically with 24px-32px gaps between major sections, while internal card padding is kept at a consistent 24px.
- **Mobile Adaptivity:** At the 768px breakpoint, the 12-column grid collapses to 4 columns. Side margins reduce to 16px. Display-sized typography scales down by 20% to fit smaller viewports.

## Elevation & Depth
Hierarchy is established through **Tonal Layering** and **Ambient Shadows**. Instead of heavy borders, the system uses "elevation levels" to distinguish between the background and interactive elements.

- **Level 0 (Background):** Solid `#f8fafc`.
- **Level 1 (Cards/Containers):** Pure white `#ffffff` with a very soft, diffused shadow (Blur: 12px, Y: 4px, Color: `rgba(30, 41, 59, 0.05)`).
- **Level 2 (Dropdowns/Modals):** Higher contrast shadow (Blur: 24px, Y: 8px, Color: `rgba(30, 41, 59, 0.1)`) to pull the element forward.
- **Interactive:** Hover states on cards should subtly increase the shadow spread or add a 1px border in the Primary color at 20% opacity.

## Shapes
The shape language is **Soft-Geometric**. A standard radius of 8px (Level 2) is applied to all primary UI components like buttons, input fields, and cards. This curvature strikes a balance between the precision of a sharp-edged "corporate" look and the friendly, accessible feel of modern SaaS software. Large containers or featured sections may use a 16px (rounded-lg) radius for a more distinct structural presence.

## Components
- **Buttons:** Primary buttons use the Primary Blue with white text. Secondary buttons use a transparent background with a 1px slate-gray border. All buttons have 8px rounded corners and an 11px vertical / 24px horizontal padding ratio.
- **Input Fields:** Use a light gray background (`#f1f5f9`) or a 1px border (`#e2e8f0`). On focus, the border transitions to Primary Blue with a 2px outer glow. Labels always sit above the field in `label-md` style.
- **Cards:** The primary container for CRM data. Use white backgrounds, 8px radius, and Level 1 elevation. Headers within cards should have a subtle bottom border (`#f1f5f9`).
- **Chips/Status Badges:** Pill-shaped with a 20% opacity background of the semantic color (e.g., green for "Active", amber for "Pending") and dark text in the same hue.
- **Navigation:** A clean sidebar or top-nav using the Secondary Navy background for high contrast, or a white background with Primary Blue active indicators for a lighter, "SaaS-native" look.