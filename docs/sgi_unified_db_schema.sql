-- ==============================================================================
-- 🐘 ESQUEMA UNIFICADO DE BASE DE DATOS POSTGRESQL 15 — GESTIÓN INTEGRAL SGI
-- ==============================================================================
-- Unificación de las bases de datos legacy:
-- 1. gestioni_datosNet (AgendaSGI: Clientes, Agenda, Actas, Compromisos)
-- 2. gestioni_consultorNet (ConsultorSGI: Auditorías, Res. 0312, PHVA, Ausentismo)
-- ==============================================================================

CREATE SCHEMA IF NOT EXISTS sgi;
SET search_path TO sgi, public;

-- ------------------------------------------------------------------------------
-- 1. CAPA GEOGRÁFICA Y TABLAS MAESTRAS
-- ------------------------------------------------------------------------------
CREATE TABLE departamentos (
    id SERIAL PRIMARY KEY,
    codigo_dane VARCHAR(10) NOT NULL UNIQUE,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE ciudades (
    id SERIAL PRIMARY KEY,
    departamento_id INT NOT NULL REFERENCES departamentos(id) ON DELETE CASCADE,
    codigo_dane VARCHAR(10) NOT NULL UNIQUE,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE normas_sistemas (
    id SERIAL PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL UNIQUE, -- EJ: SGSST_1072, RES_0312, ISO_9001, PESV_2251
    nombre VARCHAR(150) NOT NULL,
    descripcion TEXT,
    version VARCHAR(20),
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE procesos_organizacionales (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    descripcion TEXT,
    activo BOOLEAN DEFAULT TRUE
);

-- ------------------------------------------------------------------------------
-- 2. CAPA DE IDENTIDAD Y DIRECTORIO UNIFICADO (TERCEROS / CLIENTES / USUARIOS)
-- ------------------------------------------------------------------------------
-- Unifica AgendaSGI.Clientes + ConsultorSGI.Terceros
CREATE TABLE terceros_clientes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nit VARCHAR(20) NOT NULL UNIQUE,
    razon_social VARCHAR(200) NOT NULL,
    nombre_comercial VARCHAR(200),
    direccion VARCHAR(255),
    telefono VARCHAR(50),
    email_contacto VARCHAR(150),
    ciudad_id INT REFERENCES ciudades(id),
    representante_legal VARCHAR(150),
    numero_empleados INT DEFAULT 0,
    sector_economico VARCHAR(100),
    nivel_riesgo_arl VARCHAR(10),
    activo BOOLEAN DEFAULT TRUE,
    creado_en TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    actualizado_en TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- Unifica AgendaSGI.Usuarios + ConsultorSGI.AspNetUsers (Integrado con Keycloak SSO)
CREATE TABLE usuarios_consultores (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    keycloak_id VARCHAR(100) UNIQUE, -- ID del usuario en Keycloak
    documento VARCHAR(20) NOT NULL UNIQUE,
    nombre_completo VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    telefono VARCHAR(50),
    rol VARCHAR(50) NOT NULL, -- ADMIN, ASESOR_SENIOR, AUDITOR, CLIENTE_VIP
    licencia_sst VARCHAR(100), -- Número de licencia de Salud Ocupacional
    activo BOOLEAN DEFAULT TRUE,
    creado_en TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- ------------------------------------------------------------------------------
-- 3. CAPA DE CONTRATACIÓN Y ASIGNACIÓN B2B
-- ------------------------------------------------------------------------------
CREATE TABLE contratos_b2b (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cliente_id UUID NOT NULL REFERENCES terceros_clientes(id) ON DELETE CASCADE,
    numero_contrato VARCHAR(50) NOT NULL UNIQUE,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    horas_contratadas_mes INT DEFAULT 0,
    valor_total NUMERIC(15,2) DEFAULT 0.00,
    estado VARCHAR(30) DEFAULT 'ACTIVO', -- ACTIVO, SUSPENDIDO, FINALIZADO
    creado_en TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE contratos_sistemas (
    contrato_id UUID NOT NULL REFERENCES contratos_b2b(id) ON DELETE CASCADE,
    norma_id INT NOT NULL REFERENCES normas_sistemas(id) ON DELETE CASCADE,
    PRIMARY KEY (contrato_id, norma_id)
);

CREATE TABLE contratos_asesores (
    contrato_id UUID NOT NULL REFERENCES contratos_b2b(id) ON DELETE CASCADE,
    asesor_id UUID NOT NULL REFERENCES usuarios_consultores(id) ON DELETE CASCADE,
    es_lider BOOLEAN DEFAULT FALSE,
    PRIMARY KEY (contrato_id, asesor_id)
);

-- ------------------------------------------------------------------------------
-- 4. CAPA DE AGENDA OPERATIVA Y EVENTOS (AgendaSGI)
-- ------------------------------------------------------------------------------
CREATE TABLE tipos_eventos_agenda (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL, -- Auditoría, Capacitación, Inspección, COPASST, Acompañamiento
    color_hex VARCHAR(10) DEFAULT '#055bb2'
);

CREATE TABLE agenda_eventos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cliente_id UUID NOT NULL REFERENCES terceros_clientes(id) ON DELETE CASCADE,
    asesor_id UUID NOT NULL REFERENCES usuarios_consultores(id),
    tipo_evento_id INT REFERENCES tipos_eventos_agenda(id),
    titulo VARCHAR(200) NOT NULL,
    descripcion TEXT,
    fecha_inicio TIMESTAMP WITH TIME ZONE NOT NULL,
    fecha_fin TIMESTAMP WITH TIME ZONE NOT NULL,
    modalidad VARCHAR(20) DEFAULT 'PRESENCIAL', -- PRESENCIAL, VIRTUAL
    lugar VARCHAR(255),
    estado VARCHAR(30) DEFAULT 'PROGRAMADO', -- PROGRAMADO, EJECUTADO, CANCELADO, REPROGRAMADO
    creado_en TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- ------------------------------------------------------------------------------
-- 5. CAPA DE ACTAS DE VISITA Y COMPROMISOS (AgendaSGI)
-- ------------------------------------------------------------------------------
CREATE TABLE actas_visita (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    consecutivo VARCHAR(50) NOT NULL UNIQUE,
    cliente_id UUID NOT NULL REFERENCES terceros_clientes(id),
    asesor_id UUID NOT NULL REFERENCES usuarios_consultores(id),
    proceso_id INT REFERENCES procesos_organizacionales(id),
    fecha_visita TIMESTAMP WITH TIME ZONE NOT NULL,
    objetivo_visita TEXT NOT NULL,
    desarrollo_puntos TEXT NOT NULL,
    conclusiones TEXT,
    firma_asesor_url TEXT,
    firma_cliente_url TEXT,
    creado_en TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE actividades_compromisos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    acta_id UUID NOT NULL REFERENCES actas_visita(id) ON DELETE CASCADE,
    descripcion_tarea TEXT NOT NULL,
    responsable_nombre VARCHAR(150) NOT NULL,
    responsable_email VARCHAR(150),
    fecha_limite DATE NOT NULL,
    estado VARCHAR(30) DEFAULT 'PENDIENTE', -- PENDIENTE, EN_PROCESO, CUMPLIDO, VENCIDO
    evidencia_url TEXT,
    fecha_cierre TIMESTAMP WITH TIME ZONE
);

-- ------------------------------------------------------------------------------
-- 6. CAPA DE AUDITORÍAS Y DIAGNÓSTICOS PHVA / RES. 0312 (ConsultorSGI)
-- ------------------------------------------------------------------------------
CREATE TABLE auditorias_encabezado (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cliente_id UUID NOT NULL REFERENCES terceros_clientes(id),
    auditor_lider_id UUID NOT NULL REFERENCES usuarios_consultores(id),
    norma_id INT NOT NULL REFERENCES normas_sistemas(id),
    codigo_auditoria VARCHAR(50) NOT NULL UNIQUE,
    tipo_auditoria VARCHAR(30) DEFAULT 'INTERNA', -- INTERNA, EXTERNA, CERTIFICACION
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    porcentaje_cumplimiento NUMERIC(5,2) DEFAULT 0.00,
    estado VARCHAR(30) DEFAULT 'EN_PROCESO' -- EN_PROCESO, FINALIZADA, CERTIFICADA
);

CREATE TABLE auditorias_hallazgos_detalle (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    auditoria_id UUID NOT NULL REFERENCES auditorias_encabezado(id) ON DELETE CASCADE,
    numeral_norma VARCHAR(50) NOT NULL, -- EJ: 4.1, 6.1.2, Estándar 3.1.1
    criterio_evaluado TEXT NOT NULL,
    hallazgo_tipo VARCHAR(30) NOT NULL, -- CONFORMIDAD, NO_CONFORMIDAD_MAYOR, NO_CONFORMIDAD_MENOR, OPORTUNIDAD_MEJORA
    descripcion_hallazgo TEXT NOT NULL,
    plan_accion_requerido TEXT,
    responsable_solucion VARCHAR(150),
    fecha_limite_solucion DATE
);

-- ------------------------------------------------------------------------------
-- 7. CAPA DE INDICADORES DE AUSENTISMO LABORAL (ConsultorSGI)
-- ------------------------------------------------------------------------------
CREATE TABLE ausentismo_registros (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cliente_id UUID NOT NULL REFERENCES terceros_clientes(id) ON DELETE CASCADE,
    empleado_documento VARCHAR(20) NOT NULL,
    empleado_nombre VARCHAR(150) NOT NULL,
    tipo_incapacidad VARCHAR(50) NOT NULL, -- ENFERMEDAD_COMUN, ACCIDENTE_TRABAJO, ENFERMEDAD_LABORAL, MATERNIDAD
    diagnostico_cie10 VARCHAR(10),
    dias_incapacidad INT NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE NOT NULL,
    costo_estimado NUMERIC(12,2) DEFAULT 0.00
);

-- VISTA UNIFICADA: INDICADORES DE GESTIÓN Y CUMPLIMIENTO B2B
CREATE OR REPLACE VIEW vista_resumen_clientes_b2b AS
SELECT 
    c.id AS cliente_id,
    c.nit,
    c.razon_social,
    COUNT(DISTINCT a.id) AS total_actas_visita,
    COUNT(DISTINCT comp.id) FILTER (WHERE comp.estado = 'PENDIENTE') AS compromisos_pendientes,
    COUNT(DISTINCT comp.id) FILTER (WHERE comp.estado = 'CUMPLIDO') AS compromisos_cumplidos,
    COALESCE(AVG(aud.porcentaje_cumplimiento), 0) AS promedio_cumplimiento_phva
FROM terceros_clientes c
LEFT JOIN actas_visita a ON a.cliente_id = c.id
LEFT JOIN actividades_compromisos comp ON comp.acta_id = a.id
LEFT JOIN auditorias_encabezado aud ON aud.cliente_id = c.id
GROUP BY c.id, c.nit, c.razon_social;
