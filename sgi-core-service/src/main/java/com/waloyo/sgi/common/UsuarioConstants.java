package com.waloyo.sgi.common;

public final class UsuarioConstants {

    private UsuarioConstants() {
        throw new UnsupportedOperationException("Esta clase no debe ser instanciada");
    }

    // Field names
    public static final String ID = "id";
    public static final String EMAIL = "email";
    public static final String ACTIVO = "activo";
    public static final String MUST_CHANGE_PASSWORD = "mustChangePassword";
    public static final String ROL = "rol";
    public static final String MODULOS_PERMITIDOS = "modulosPermitidos";
    public static final String NOMBRE_COMPLETO = "nombreCompleto";
    public static final String TIPO_DOCUMENTO = "tipoDocumento";
    public static final String DOCUMENTO = "documento";
    public static final String PAIS = "pais";
    public static final String TELEFONO_MOVIL = "telefonoMovil";

    // Response values
    public static final String STATUS = "status";
    public static final String SUCCESS = "SUCCESS";
    public static final String MESSAGE = "message";
    public static final String ERROR = "error";

    // Defaults
    public static final String DEFAULT_ROL = "CONSULTOR";
    public static final String DEFAULT_PAIS = "Colombia (+57)";
    public static final String DEFAULT_TIPO_DOCUMENTO = "CC";
    public static final String DEFAULT_EMPTY = "";
}

