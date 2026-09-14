# 📘 Manual de Usuario Oficial — Plataforma ConsultorSGI

**Empresa:** Gestión Integral SGI S.A.S  
**Aplicativo:** ConsultorSGI (Sistema de Gestión de Seguridad y Salud en el Trabajo SG-SST, Auditorías e ISO)  
**Ubicación de Código:** `D:\Waloyo\WaloyoGroup\apps\client\SGI\ConsultorSGI`  
**Versión:** 2.0  
**Fecha:** Agosto 2026

---

## 📌 Tabla de Contenido

1. [Introducción y Propósito del Software ConsultorSGI](#1-introducción-y-propósito-del-software-consultorsgi)
2. [Acceso al Sistema y Seguridad](#2-acceso-al-sistema-y-seguridad)
   - [2.1 Inicio de Sesión (Login)](#21-inicio-de-sesión-login)
   - [2.2 Recuperación de Contraseña](#22-recuperación-de-contraseña)
   - [2.3 Administración de Perfil de Usuario](#23-administración-de-perfil-de-usuario)
3. [Módulo de Gestión de Terceros / Empresas Clientes](#3-módulo-de-gestión-de-terceros--empresas-clientes)
   - [3.1 Registro de Empresas Clientes (Terceros)](#31-registro-de-empresas-clientes-terceros)
   - [3.2 Asignación de Sistemas de Gestión (ISO 9001, 14001, 45001, Res. 0312)](#32-asignación-de-sistemas-de-gestión-iso-9001-14001-45001-res-0312)
   - [3.3 Configuración de Sedes y Clientes Secundarios](#33-configuración-de-sedes-y-clientes-secundarios)
4. [Módulo de Diagnósticos de Cumplimiento Normativo (Res. 0312 / ISO)](#4-módulo-de-diagnósticos-de-cumplimiento-normativo-res-0312--iso)
   - [4.1 Evaluación Inicial de Estándares Mínimos](#41-evaluación-inicial-de-estándares-mínimos)
   - [4.2 Calificación de Criterios y Numerales](#42-calificación-de-criterios-y-numerales)
   - [4.3 Carga y Gestión de Documentos Evidencia](#43-carga-y-gestión-de-documentos-evidencia)
   - [4.4 Generación de Planes de Acción Automáticos](#44-generación-de-planes-de-acción-automáticos)
   - [4.5 Exportación de Informes de Diagnóstico en PDF](#45-exportación-de-informes-de-diagnóstico-en-pdf)
5. [Módulo de Auditorías Internas y de Sistema de Gestión](#5-módulo-de-auditorías-internas-y-de-sistema-de-gestión)
   - [5.1 Elaboración del Plan de Auditoría](#51-elaboración-del-plan-de-auditoría)
   - [5.2 Registro de Reuniones de Apertura y Cierre](#52-registro-de-reuniones-de-apertura-y-cierre)
   - [5.3 Evaluación de Hallazgos y No Conformidades](#53-evaluación-de-hallazgos-y-no-conformidades)
   - [5.4 Generación del Informe Final de Auditoría en PDF](#54-generación-del-informe-final-de-auditoría-en-pdf)
   - [5.5 Acciones Correctivas, Preventivas y de Mejora (RC & ACPM)](#55-acciones-correctivas-preventivas-y-de-mejora-rc--acpm)
6. [Módulo de Ausentismo e Indicadores del SG-SST](#6-módulo-de-ausentismo-e-indicadores-del-sg-sst)
   - [6.1 Registro de Incapacidades, Licencias y Accidentes de Trabajo](#61-registro-de-incapacidades-licencias-y-accidentes-de-trabajo)
   - [6.2 Gestión de Próximas Prórrogas](#62-gestión-de-próximas-prórrogas)
   - [6.3 Análisis de Costos Directos e Indirectos de Ausentismo](#63-análisis-de-costos-directos-e-indirectos-de-ausentismo)
   - [6.4 Cálculo Automático de Indicadores Mínimos del SG-SST](#64-cálculo-automático-de-indicadores-mínimos-del-sg-sst)
7. [Módulo de Empleados y Perfil Sociodemográfico](#7-módulo-de-empleados-y-perfil-sociodemográfico)
   - [7.1 Registro de Empleados y Datos Personales](#71-registro-de-empleados-y-datos-personales)
   - [7.2 Asignación de Cargos, Áreas y Centros de Trabajo](#72-asignación-de-cargos-áreas-y-centros-de-trabajo)
   - [7.3 Nivel de Riesgo ARL, Turnos y Afiliaciones (EPS, AFP, Fondo)](#73-nivel-de-riesgo-arl-turnos-y-afiliaciones-eps-afp-fondo)
   - [7.4 Diagnóstico Sociodemográfico de la Población Trabajadora](#74-diagnóstico-sociodemográfico-de-la-población-trabajadora)
8. [Módulo de Listas de Verificación e Inspecciones](#8-módulo-de-listas-de-verificación-e-inspecciones)
   - [8.1 Plantillas de Inspección (EPP, Extintores, Locativas, Vehículos)](#81-plantillas-de-inspección-epp-extintores-locativas-vehículos)
   - [8.2 Ejecución de Inspecciones en Campo y Calificación](#82-ejecución-de-inspecciones-en-campo-y-calificación)
   - [8.3 Cálculo del Porcentaje de Cumplimiento de Inspección](#83-cálculo-del-porcentaje-de-cumplimiento-de-inspección)
9. [Módulo de Estructura Organizacional y Normativa](#9-módulo-de-estructura-organizacional-y-normativa)
   - [9.1 Mapa de Procesos de la Empresa](#91-mapa-de-procesos-de-la-empresa)
   - [9.2 Catálogo de Normas Técnicas y Legales](#92-catálogo-de-normas-técnicas-y-legales)
10. [Módulo de Seguridad, Roles y Permisos](#10-módulo-de-seguridad-roles-y-permisos)
    - [10.1 Gestión de Usuarios y Roles](#101-gestión-de-usuarios-y-roles)
    - [10.2 Configuración Granular de Permisos por Módulo](#102-configuración-granular-de-permisos-por-módulo)
11. [Preguntas Frecuentes y Guía de Solución de Problemas](#11-preguntas-frecuentes-y-guía-de-solución-de-problemas)

---

## 1. Introducción y Propósito del Software ConsultorSGI

La plataforma **ConsultorSGI** es el software especializado diseñado por **Gestión Integral SGI S.A.S** para automatizar, evaluar y auditar los **Sistemas de Gestión de Seguridad y Salud en el Trabajo (SG-SST)**, así como las normas internacionales de gestión (**ISO 9001:2015, ISO 14001:2015, ISO 45001:2018, Resolución 0312 de 2019, Decreto 1072 de 2015**).

A través de esta plataforma, las empresas clientes y los consultores asignados pueden:
- Realizar la **Evaluación Inicial de Estándares Mínimos** y diagnósticos de cumplimiento normativo.
- Gestionar **Auditorías Internas** completas con emisión de informes en PDF.
- Registrar el **Perfil Sociodemográfico de los Trabajadores**.
- Controlar el **Ausentismo Laboral**, incapacidades y calcular los indicadores legales obligatorios (Frecuencia, Severidad, Accidentabilidad).
- Administrar el catálogo de **Listas de Chequeo e Inspecciones de Seguridad**.
- Construir y hacer seguimiento en tiempo real a los **Planes de Acción y ACPM**.

---

## 2. Acceso al Sistema y Seguridad

### 2.1 Inicio de Sesión (Login)
1. Acceda a la URL oficial del sistema `ConsultorSGI`.
2. En el formulario de autenticación ingrese su **Usuario / Correo Electrónico** y **Contraseña**.
3. Haga clic en **"Iniciar Sesión"**.

### 2.2 Recuperación de Contraseña
Si ha olvidado su contraseña:
1. Haga clic en el enlace **"¿Olvidó su contraseña?"**.
2. Ingrese el correo electrónico registrado.
3. El sistema le enviará una notificación con el enlace seguro para restablecer su clave.

### 2.3 Administración de Perfil de Usuario
Desde la barra superior, al presionar su nombre de usuario:
- Podrá consultar su rol asignado, empresa tercera asociada y actualizar sus datos básicos de contacto.

---

## 3. Módulo de Gestión de Terceros / Empresas Clientes

Acceso desde el menú principal: **`CONSULTOR SGI -> Terceros`** (`/Terceros`).

### 3.1 Registro de Empresas Clientes (Terceros)
Este módulo administra el portafolio de empresas a las que se les presta el servicio de consultoría.
- **Datos de la Empresa:** NIT, Dígito de Verificación (DV), Razón Social, Dirección Principal, Teléfono Fijo y Celular.
- **Representante Legal:** Nombre y documento del representante legal.
- **Información de Riesgo Laboral:** ARL contratada, Actividad Económica Principal, Código CIIU y **Nivel de Riesgo Principal** (Riesgo I a Riesgo V).

### 3.2 Asignación de Sistemas de Gestión (ISO / Res. 0312)
En el perfil del Tercero (`Terceros_SistemasDeGestion`):
- Asigne los sistemas de gestión que aplican a la empresa (ej. *Resolución 0312 Estándares Mínimos*, *Decreto 1072*, *ISO 9001 Calidad*, *ISO 45001 SST*, *ISO 14001 Ambiental*).

### 3.3 Configuración de Sedes y Clientes Secundarios
Permite ramificar empresas con múltiples sedes o filiales (`Terceros_Clientes`) para realizar diagnósticos e inspecciones por cada centro operativo de forma independiente.

---

## 4. Módulo de Diagnósticos de Cumplimiento Normativo (Res. 0312 / ISO)

Acceso desde el menú: **`ConsultorSGI -> Diagnósticos`** (`/Diagnosticos`).

### 4.1 Evaluación Inicial de Estándares Mínimos
El núcleo de la autoevaluación y diagnóstico del SG-SST:
1. Seleccione la empresa cliente (Tercero) y el estándar a evaluar (ej. *Resolución 0312 de 2019 - 60 Estándares*).
2. El sistema desplegará el árbol normativo por Fases o Ciclos PHVA (Planear, Hacer, Verificar, Actuar).

### 4.2 Calificación de Criterios y Numerales
Para cada ítem normativo evaluado, el consultor o auditor asigna una calificación:
- **Cumple (C):** Asigna el puntaje ponderado completo del estándar.
- **No Cumple (NC):** Asigna 0 puntos y habilita la creación automática de un plan de acción.
- **No Aplica (NA):** Si el estándar no aplica legalmente a la empresa (con justificación).

### 4.3 Carga y Gestión de Documentos Evidencia
Por cada criterio normativo evaluated:
- El usuario puede presionar **"Cargar Evidencia"** (`DocumentosDiagnosticoPasos_DocumentoEvidencia`).
- Permite subir archivos PDF, imágenes o documentos Word que respaldan el cumplimiento del estándar (ej. *Política de SST firmada*, *Matriz de Riesgos*, *Plan de Capacitaciones*).

### 4.4 Generación de Planes de Acción Automáticos
Cuando un ítem se califica como **"No Cumple"**:
- El sistema crea de forma automática una entrada en la tabla de **Planes de Acción** (`DocumentosDiagnosticoDetalle_PlanesDeAccion`).
- Permite asignar la **Acción Correctiva o Preventiva**, el **Responsable de Ejecución**, la **Fecha Límite** y los **Recursos Requeridos**.

### 4.5 Exportación de Informes de Diagnóstico en PDF
- Presione el botón **"Exportar Diagnóstico PDF"**.
- El sistema compilará un informe ejecutivo con el **porcentaje de cumplimiento global**, gráficos por ciclo PHVA y el listado de evidencias y hallazgos.

---

## 5. Módulo de Auditorías Internas y de Sistema de Gestión

Acceso desde el menú: **`ConsultorSGI -> Auditorías`** (`/Auditorias`).

### 5.1 Elaboración del Plan de Auditoría
Permite estructurar auditorías internas de calidad, ambiente o SST:
1. Haga clic en **"Crear Plan de Auditoría"**.
2. Defina: **Objetivo de la Auditoría**, **Alcance**, **Criterios Normativos**, **Auditor Líder** y **Equipo Auditor**.
3. **Cronograma de Auditoría (`AuditoriasDetalle`):** Programe los bloques de auditoría por proceso, indicando Fecha, Hora Inicial, Hora Final, Proceso a Auditar y Numerales Normativos a verificar.

### 5.2 Registro de Reuniones de Apertura y Cierre
En la sección `AuditoriasDetalleAC`:
- Registre el acta formal de la **Reunión de Apertura** (asistentes, acuerdos) y de la **Reunión de Cierre** (presentación de resultados ante la alta dirección).

### 5.3 Evaluación de Hallazgos y No Conformidades
Durante la ejecución de la auditoría:
- Registre cada hallazgo categorizándolo como:
  * **Conformidad** (Fortaleza).
  * **No Conformidad Mayor (NC M).**
  * **No Conformidad Menor (NC m).**
  * **Observación.**
  * **Oportunidad de Mejora (OM).**

### 5.4 Generación del Informe Final de Auditoría en PDF
- Presione **"Exportar Informe de Auditoría PDF"** (`_ExportarInformesAuditoriaPDF.cshtml`).
- Se generará un informe técnico completo con firmas digitales, cuadro cuantitativo de hallazgos por norma y las conclusiones del auditor líder.

### 5.5 Acciones Correctivas, Preventivas y de Mejora (RC & ACPM)
- Módulo para el tratamiento posterior de las No Conformidades.
- Permite realizar el análisis de causa raíz (Metodología 5 Porqués, Diagrama de Ishikawa) y hacer seguimiento al cierre de acciones.

---

## 6. Módulo de Ausentismo e Indicadores del SG-SST

Acceso desde el menú: **`ConsultorSGI -> Ausentismo`** (`/Ausentismo`).

### 6.1 Registro de Incapacidades, Licencias y Accidentes de Trabajo
Permite alimentar la base de datos de ausentismo de la empresa:
1. Presione **"Registrar Ausentismo"**.
2. Seleccione el **Empleado** (traerá automáticamente su cargo, área y salario).
3. Seleccione el **Tipo de Ausentismo**:
   - Incapacidad por Enfermedad Común (EG).
   - Licencia de Maternidad / Paternidad.
   - Accidentes de Trabajo (AT).
   - Enfermedad Laboral (EL).
   - Permiso Remunerado / No Remunerado.
4. Ingrese la **Fecha Inicial**, **Número de Días** y el **Código CIE-10** del diagnóstico médico.

### 6.2 Gestión de Próximas Prórrogas
- Si una incapacidad es prórroga de un evento anterior, el sistema vincula los registros para no duplicar eventos pero acumular los días de incapacidad continua.

### 6.3 Análisis de Costos Directos e Indirectos de Ausentismo
- **Costo Directo:** El sistema calcula automáticamente el costo económico del ausentismo basándose en el salario diario del trabajador y los días asumidos por la empresa o ARL/EPS.
- **Reportes por Área, Proceso y Año:** Gráficos que identifican las áreas con mayor índice de ausentismo o mayor costo económico.

### 6.4 Cálculo Automático de Indicadores Mínimos del SG-SST
En la sección **`AusentismoIndicadores`**:
El sistema calcula automáticamente los indicadores obligatorios exigidos por la legislación colombiana (Res. 0312/2019):
- **Índice de Frecuencia de Ausentismo (IFA):** `(Número de eventos de incapacidad en el mes / Número de trabajadores) * 100`.
- **Índice de Severidad de Ausentismo (ISA):** `(Número de días de incapacidad en el mes / Número de trabajadores) * 100`.
- **Proporción de Accidentes de Trabajo Mortales.**
- **Tasa de Incidencia y Prevalencia de Enfermedad Laboral.**

---

## 7. Módulo de Empleados y Perfil Sociodemográfico

Acceso desde el menú: **`ConsultorSGI -> Empleados`** (`/Empleados`).

### 7.1 Registro de Empleados y Datos Personales
Ficha médica y laboral del trabajador:
- Nombres, Apellidos, Tipo y Número de Documento, Fecha de Nacimiento, Género, Dirección, Teléfono, Estado Civil.

### 7.2 Asignación de Cargos, Áreas y Centros de Trabajo
- **Cargo:** Puesto que desempeña el trabajador (`/Cargos`).
- **Área:** Departamento organizacional (`/Areas`).
- **Centro de Trabajo:** Sede o centro operativo al que pertenece (`/CentrosDeTrabajo`).

### 7.3 Nivel de Riesgo ARL, Turnos y Afiliaciones
- **Nivel de Riesgo:** Riesgo I (Bajo) a Riesgo V (Máximo).
- **Afiliaciones:** Registro de la EPS, ARL, Fondo de Pensiones (AFP) y Caja de Compensación del trabajador.
- **Turnos:** Horario o turno de trabajo asignado (`/Turnos`).

### 7.4 Diagnóstico Sociodemográfico de la Población Trabajadora
- Módulo de análisis estadístico que genera los gráficos de distribución por:
  - Rangos de Edad y Sexo (Pirámide poblacional).
  - Nivel de Escolaridad.
  - Estado Civil y Número de Hijos.
  - Tipo de Vivienda y Tipo de Contrato.

---

## 8. Módulo de Listas de Verificación e Inspecciones

Acceso desde el menú: **`ConsultorSGI -> Listas de Verificación`** (`/ListasDeVerificacion`).

### 8.1 Plantillas de Inspección
Permite diseñar o utilizar listas de chequeo preconcebidas para inspecciones de seguridad:
- Inspección de Extintores y Gabinetes Contra Incendio.
- Inspección de Botiquines de Primeros Auxilios.
- Inspección de Equipos de Protección Personal (EPP).
- Inspecciones Locativas y de Orden y Aseo (5S).
- Inspección de Vehículos y Maquinaria.

### 8.2 Ejecución de Inspecciones en Campo y Calificación
Durante la ronda o inspección técnica:
- Seleccione la plantilla y el centro de trabajo a inspeccionar.
- Califique cada ítem de verificación: **Cumple (C)**, **No Cumple (NC)** o **No Aplica (NA)**.
- Agregue observaciones y fotografías de evidencia si se detecta una condición subestándar.

### 8.3 Cálculo del Porcentaje de Cumplimiento de Inspección
- El sistema calcula instantáneamente la nota porcentual de la inspección.
- Si el porcentaje es inferior al umbral configurado, el sistema genera automáticamente un plan de acción para subsanar los hallazgos encontradas.

---

## 9. Módulo de Estructura Organizacional y Normativa

### 9.1 Mapa de Procesos de la Empresa (`/Procesos`)
Permite registrar y mapear la cadena de valor del cliente dividida en:
- **Procesos Estratégicos:** Alta Dirección, Planeación, Calidad.
- **Procesos Misionales / Operativos:** Producción, Comercial, Prestación del Servicio.
- **Procesos de Apoyo / Soporte:** Gestión Humana, Compras, Mantenimiento, Sistemas.
- **Procesos de Evaluación:** Auditoría Interna, Control de Gestión.

### 9.2 Catálogo de Normas Técnicas y Legales (`/Normas` / `/Numerales`)
Base de conocimientos normativos integrada en el sistema conteniendo la estructura completa de capítulos y numerales de las normas ISO y Decretos reglamentarios para asociarlos a diagnósticos y auditorías.

---

## 10. Módulo de Seguridad, Roles y Permisos

Acceso restringido a Administradores: **`ConsultorSGI -> Seguridad`** (`/Seguridad` / `/AspNetRoles`).

### 10.1 Gestión de Usuarios y Roles
- Permite crear cuentas para Consultores, Auditores y Usuarios Cliente.
- Asignación de Roles (ej. *Administrador General*, *Consultor Líder*, *Auditor Externo*, *Cliente Consulta*).

### 10.2 Configuración Granular de Permisos por Módulo
En la matriz de permisos (`_GetPermisosModulos.cshtml`):
- El administrador puede definir para cada rol qué acciones tiene permitidas por cada módulo:
  - **Consultar (Lectura).**
  - **Crear.**
  - **Editar.**
  - **Eliminar.**
  - **Exportar Informes / PDF.**

---

## 11. Preguntas Frecuentes y Guía de Solución de Problemas

### Q1: ¿Cómo genero la Evaluación Inicial de la Res. 0312 de 2019 para un cliente nuevo?
> **Respuesta:**
> 1. Vaya a `/Terceros` y cree la empresa cliente.
> 2. En la pestaña Sistemas de Gestión, asigne la **Resolución 0312 de 2019**.
> 3. Ingrese a `/Diagnosticos`, seleccione el cliente y la norma.
> 4. Califique los 60 estándares (o 7/21 según el tamaño de la empresa), cargue las evidencias PDF y presione **"Exportar Diagnóstico PDF"**.

### Q2: ¿El sistema calcula los costos del ausentismo automáticamente?
> **Respuesta:** Sí. Al registrar una incapacidad en el módulo `/Ausentismo` asociando el empleado, el sistema toma su salario registrado en la ficha del trabajador y multiplica por el número de días ausentes para entregar el costo directo del evento.

### Q3: ¿Cómo exporto el informe oficial de una Auditoría Interna en PDF?
> **Respuesta:** Ingrese a `/Auditorias`, seleccione la auditoría correspondiente y presione el botón **"Exportar Informe PDF"**. El sistema generará el reporte consolidado con las firmas de los auditores y el desglose de No Conformidades.

---
*Manual oficial de usuario desarrollado para la plataforma ConsultorSGI de Gestión Integral SGI S.A.S.*
