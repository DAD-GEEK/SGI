package com.waloyo.sgi.service;

import com.waloyo.sgi.common.UsuarioConstants;
import com.waloyo.sgi.entity.UsuarioEntity;
import org.springframework.stereotype.Service;

import java.util.Map;

@Service
public class UsuarioPayloadService {

    public Map<String, Object> buildUsuarioPayload(UsuarioEntity usuario) {
        if (usuario == null) {
            return Map.of();
        }

        String rol = usuario.getRol() != null ? usuario.getRol() : UsuarioConstants.DEFAULT_ROL;
        String modulos = usuario.getModulosPermitidos() != null ? usuario.getModulosPermitidos() : UsuarioConstants.DEFAULT_EMPTY;
        boolean activo = Boolean.TRUE.equals(usuario.getActivo());
        boolean mustChange = Boolean.TRUE.equals(usuario.getMustChangePassword());

        return Map.ofEntries(
                Map.entry(UsuarioConstants.ID, usuario.getId().toString()),
                Map.entry(UsuarioConstants.EMAIL, usuario.getEmail()),
                Map.entry(UsuarioConstants.ACTIVO, activo),
                Map.entry(UsuarioConstants.MUST_CHANGE_PASSWORD, mustChange),
                Map.entry(UsuarioConstants.ROL, rol),
                Map.entry(UsuarioConstants.MODULOS_PERMITIDOS, modulos)
        );
    }

    public Map<String, Object> buildDeactivatedPayload(UsuarioEntity usuario) {
        if (usuario == null) {
            return Map.of();
        }
        return Map.ofEntries(
                Map.entry(UsuarioConstants.ID, usuario.getId().toString()),
                Map.entry(UsuarioConstants.EMAIL, usuario.getEmail()),
                Map.entry(UsuarioConstants.ACTIVO, false)
        );
    }

    public Map<String, Object> buildFullUsuarioResponse(UsuarioEntity usuario) {
        if (usuario == null) {
            return Map.of();
        }

        String rol = usuario.getRol() != null ? usuario.getRol() : UsuarioConstants.DEFAULT_ROL;
        String modulos = usuario.getModulosPermitidos() != null ? usuario.getModulosPermitidos() : UsuarioConstants.DEFAULT_EMPTY;
        String tipoDoc = usuario.getTipoDocumento() != null ? usuario.getTipoDocumento() : UsuarioConstants.DEFAULT_TIPO_DOCUMENTO;
        String documento = usuario.getDocumento() != null ? usuario.getDocumento() : UsuarioConstants.DEFAULT_EMPTY;
        String pais = usuario.getPais() != null ? usuario.getPais() : UsuarioConstants.DEFAULT_PAIS;
        String telefono = usuario.getTelefonoMovil() != null ? usuario.getTelefonoMovil() : UsuarioConstants.DEFAULT_EMPTY;
        String nombre = usuario.getNombreCompleto() != null ? usuario.getNombreCompleto() : UsuarioConstants.DEFAULT_EMPTY;
        boolean activo = Boolean.TRUE.equals(usuario.getActivo());
        boolean mustChange = Boolean.TRUE.equals(usuario.getMustChangePassword());

        return Map.ofEntries(
                Map.entry(UsuarioConstants.ID, usuario.getId().toString()),
                Map.entry(UsuarioConstants.EMAIL, usuario.getEmail()),
                Map.entry(UsuarioConstants.NOMBRE_COMPLETO, nombre),
                Map.entry(UsuarioConstants.TIPO_DOCUMENTO, tipoDoc),
                Map.entry(UsuarioConstants.DOCUMENTO, documento),
                Map.entry(UsuarioConstants.PAIS, pais),
                Map.entry(UsuarioConstants.TELEFONO_MOVIL, telefono),
                Map.entry(UsuarioConstants.ACTIVO, activo),
                Map.entry(UsuarioConstants.MUST_CHANGE_PASSWORD, mustChange),
                Map.entry(UsuarioConstants.ROL, rol),
                Map.entry(UsuarioConstants.MODULOS_PERMITIDOS, modulos)
        );
    }
}

