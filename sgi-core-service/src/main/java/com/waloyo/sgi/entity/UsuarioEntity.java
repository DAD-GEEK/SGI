package com.waloyo.sgi.entity;

import jakarta.persistence.*;
import lombok.*;
import java.time.OffsetDateTime;
import java.util.UUID;

@Entity
@Table(name = "usuarios_consultores", schema = "sgi")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class UsuarioEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private UUID id;

    @Column(name = "tipo_documento", length = 20)
    private String tipoDocumento;

    @Column(nullable = false, unique = true, length = 20)
    private String documento;

    @Column(length = 50)
    private String pais;

    @Column(name = "nombre_completo", nullable = false, length = 255)
    private String nombreCompleto;

    @Column(nullable = false, unique = true, length = 150)
    private String email;

    @Column(length = 255)
    private String passwordHash;

    @Column(name = "telefono_movil", length = 50)
    private String telefonoMovil;

    @Column(nullable = false, length = 50)
    private String rol;

    @Builder.Default
    @Column(nullable = false)
    private Boolean activo = true;

    @Builder.Default
    @Column(name = "must_change_password")
    private Boolean mustChangePassword = false;

    @Column(name = "modulos_permitidos", length = 500)
    private String modulosPermitidos;

    @Column(name = "creado_en", insertable = false, updatable = false)
    private OffsetDateTime creadoEn;
}
