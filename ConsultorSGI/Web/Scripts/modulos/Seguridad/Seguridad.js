var seguridad = {

    //Login
    loginAsync: function () {
        'use strict';

        $("#formLogin").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var loginViewModel = {
                        "Email": $("#usuarioLogin").val(),
                        "Password": $("#usuarioClave").val(),
                        "RememberMe": $("#usuarioRecordarme").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/LoginAsync',
                        data: { __RequestVerificationToken: token, model: loginViewModel },
                        async: true,
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (response) {

                            if (response.msn === 'success')
                                return window.location = rootHost + "Home/index";

                            var respuestaError = $("#respuestaError");
                            respuestaError.html(response.error);

                            form.parsley().destroy();
                        },
                        error: function (response) {

                            return window.location = rootHost + "Seguridad/Login";
                        }
                    });
                }

            } catch (ex) {

                return window.location = rootHost + "Seguridad/Login";
            }
        });
    },

    recuperarContraseñaAsync: function () {
        'use strict';

        $("#formRecuperarContraseña").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var forgotPasswordViewModel = {
                        "Email": $("#inputEmailRecuperarContraseña").val(),
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/RecuperarContraseñaAsync',
                        data: { __RequestVerificationToken: token, model: forgotPasswordViewModel },
                        async: true,
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (response) {

                            if (response.msn === 'success') {
                                Swal.fire({
                                    title: "¡Recuperar contraseña!",
                                    text: 'Las instrucciones para recuperar su contraseña han sido enviadas a su correo electrónico.',
                                    icon: 'success',
                                    showDenyButton: false,
                                    showCancelButton: false,
                                    confirmButtonText: 'OK',
                                }).then(function (result) {
                                    if (result.isConfirmed) window.location.href = rootHost + "Seguridad/Login";
                                });
                            } else return Swal.fire("¡Alerta!", response.error, "warning");

                        },
                        error: function (response) {

                            swal.fire("Oops!", JSON.stringify(response), "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método recuperarContraseñaAsync \n" + ex, "error");
            }
        });
    },

    cambiarContraseñaAsync: function () {
        'use strict';

        $("#formCambiarContraseña").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var ResetPasswordViewModel = {
                        "Email": $("#registroEmail").val(),
                        "Password": $("#registroClave").val(),
                        "ConfirmPassword": $("#registroConfirmarClave").val(),
                        "Code": $("#registroCode").val()
                    };

                    if ($("#registroClave").val().trim().toUpperCase() != $("#registroConfirmarClave").val().trim().toUpperCase()) return swal.fire("¡Alerta!", "La contraseña no coincide con la contraseña de confirmación", "warning");

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/CambiarContraseñaAsync',
                        data: { __RequestVerificationToken: token, model: ResetPasswordViewModel },
                        async: true,
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (response) {

                            if (response.msn === 'success') {
                                Swal.fire({
                                    title: "¡Recuperar contraseña!",
                                    text: 'Su contraseña fue restablecida exitosamente.',
                                    icon: 'success',
                                    showDenyButton: false,
                                    showCancelButton: false,
                                    confirmButtonText: 'OK',
                                }).then(function (result) {
                                    if (result.isConfirmed) window.location.href = rootHost + "Seguridad/Login";
                                });
                            } else return Swal.fire("¡Alerta!", response.error, "warning");


                        },
                        error: function (response) {

                            swal.fire("Oops!", JSON.stringify(response), "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método recuperarContraseñaAsync \n" + ex, "error");
            }
        });
    },

    RegistroAsync: function () {
        'use strict';

        $("#formRegistro").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var RegisterViewModel = {
                        "Name": $("#registroNombre").val(),
                        "Email": $("#registroEmail").val(),
                        "PhoneNumber": $("#registroTelefono").val(),
                        "FechaNacimiento": $("#registroFechaNacimiento").val(),
                        "Password": $("#registroClave").val(),
                        "ConfirmPassword": $("#registroConfirmarClave").val(),
                    };

                    if ($("#registroClave").val().trim().toUpperCase() != $("#registroConfirmarClave").val().trim().toUpperCase()) return swal.fire("¡Alerta!", "La contraseña no coincide con la contraseña de confirmación", "warning");

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/RegistroAsync',
                        data: { __RequestVerificationToken: token, model: RegisterViewModel },
                        async: true,
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (response) {

                            if (response.msn === 'success') {
                                Swal.fire({
                                    title: "¡Registro!",
                                    text: 'Ingrese a su correo electrónico y confirme su cuenta para continuar con el registro.',
                                    icon: 'success',
                                    showDenyButton: false,
                                    showCancelButton: false,
                                    confirmButtonText: 'OK',
                                }).then(function (result) {
                                    if (result.isConfirmed) return window.location.href = rootHost + "Seguridad/Login";
                                });
                            } else return Swal.fire("¡Alerta!", response.error, "warning");

                        },
                        error: function (response) {

                            swal.fire("Oops!", JSON.stringify(response), "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método RegistroAsync \n" + ex, "error");
            }
        });
    },

    confirmarLoginExternoAsync: function () {
        'use strict';

        $("#formConfirmacionLoginExterno").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var ExternalLoginConfirmationViewModel = {
                        "Email": $("#inputRegistroExterno").val()
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/ConfirmacionLoginExterno',
                        data: { __RequestVerificationToken: token, model: ExternalLoginConfirmationViewModel, returnUrl: $("#returnUrl").val() },
                        async: true,
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (response) {

                            if (response.msn === 'success') {
                                Swal.fire({
                                    title: "¡Registro!",
                                    text: 'Su correo electrónico ha sido asociada exitosamente con nuestra aplicación.',
                                    icon: 'success',
                                    showDenyButton: false,
                                    showCancelButton: false,
                                    confirmButtonText: 'OK',
                                }).then(function (result) {
                                    if (result.isConfirmed) return window.location.href = rootHost + "Seguridad/Login";
                                });
                            } else return Swal.fire("¡Alerta!", response.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método confirmarLoginExternoAsync \n" + ex, "error");
            }
        });
    },

    //Administrador
    getModuloSeguridad: function () {
        'use strict';
        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/GetModuloSeguridad',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetSeguridad').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getModuloSeguridad \n" + ex, "error");
        }
    },

    getAllUsuariosTerceros: function () {
        'use strict';
        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/GetAllUsuariosTerceros',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllUsuariosTerceros').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllUsuariosTerceros \n" + ex, "error");
        }
    },

    getAllRoles: function () {
        'use strict';
        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/GetAllRoles',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllRoles').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllRoles \n" + ex, "error");
        }
    },

    getAllModulosDetalle: function () {
        'use strict';
        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/GetAllModulosDetalle',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllModulosDetalle').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllModulosDetalle \n" + ex, "error");
        }
    },

    //Permisos
    guardarPermisosByRolAsync: function (tree) {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var listaPermisos = tree.getCheckedNodes();

                var token = $('input[name="__RequestVerificationToken"]').val();

                $.ajax({
                    type: "POST",
                    url: rootHost + 'Seguridad/GuardarPermisosByRolAsync',
                    data: { __RequestVerificationToken: token, listaPermisos: listaPermisos, rolID: $("#rolID").val() },
                    async: true,
                    dataType: 'JSON',
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    success: function (response) {

                        if (response.msn === "success") {

                            swal.fire("¡Notificación!", "Los permisos se guardaron correctamente.", "success");
                            $('#modal').modal('hide');
                            $('#contenedor').empty();

                        } else swal.fire("¡Alerta!", response.error, "warning");
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                    }
                });


            } catch (ex) {

                swal.fire("Oops!", "Error en el método asignarPermisosByRolAsync \n" + ex, "error");
            }
        });
    },

    //Menu dinámico
    getMenuDinamicoAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/GetMenuDinamicoAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (response) {

                    $('#divMenu').empty().html(response);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el metodo getMenuDinamicoAsync \n" + ex, "error");
        }


    },

    //Usuarios
    getAllUsuarios: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/GetAllUsuariosAsync',
                data: { terceroID: terceroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllUsuarios').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllUsuarios \n" + ex, "error");
        }
    },

    crearUsuario: function () {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/CrearUsuario',
                data: {},
                dataType: "html",
                async: false,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método crearUsuario \n" + ex, "error");
        }
    },

    getUsuarioByEditarAsync: function (registroID) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/GetUsuarioByEditarAsync',
                data: { registroID: registroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método getUsuarioByEditarAsync \n" + ex, "error");
        }
    },

    crearUsuarioAsync: function () {
        'use strict'
        try {

            $(".btnGuardarRegistroModal").click(function (e) {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formCrearGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntTerceroID": $("#usuarioTercero").val(),
                        "StrUsuarioEmail": $("#usuarioEmail").val().toLowerCase(),
                        "StrUsuarioNombre": $("#usuarioNombre").val().toUpperCase(),
                        "OpcEstado": true

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/CrearUsuarioAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {

                                    seguridad.getUsuarioByEditarAsync(respuesta.registroID);
                                    seguridad.getAllUsuariosTerceros();
                                    return toastr.success('El usuario se guardó correctamente.');
                                }

                                swal.fire("¡Notificación!", "El usuario se guardó correctamente.", "success");
                                $('#modal').modal('hide');
                                $('#contenedor').empty();

                                seguridad.getAllUsuariosTerceros();

                            } else {
                                form.parsley().destroy();
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                }
            });



        } catch (ex) {

            swal.fire("¡Error!", "Error en el método crearUsuarioAsync \n" + ex, "error");
        }

    },

    updateUsuarioAsync: function () {
        'use strict'
        try {

            $(".btnGuardarRegistroModal").click(function (e) {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formEditarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntRegistroID": $("#usuarioRegistroID").val(),
                        "IntTerceroID": $("#usuarioTercero").val(),
                        "StrUsuarioEmail": $("#usuarioEmail").val().toLowerCase(),
                        "StrUsuarioNombre": $("#usuarioNombre").val().toUpperCase(),
                        "OpcEstado": $("#opcEstado").is(':checked')

                    };

                    var roles = $("#usuarioRoles").val();
                    var rolesJson = JSON.stringify(roles);

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/UpdateUsuarioAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, roles: rolesJson },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {

                                    seguridad.getUsuarioByEditarAsync(respuesta.registroID);
                                    seguridad.getAllUsuariosTerceros();
                                    return toastr.success('El usuario se guardó correctamente.');
                                }

                                swal.fire("¡Notificación!", "El usuario se guardó correctamente.", "success");
                                $('#modal').modal('hide');
                                $('#contenedor').empty();

                                seguridad.getAllUsuariosTerceros();

                            } else {
                                form.parsley().destroy();
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                }
            });

        } catch (ex) {

            swal.fire("¡Error!", "Error en el método updateUsuarioAsync \n" + ex, "error");
        }

    },

    deleteUsuarioAsync: function (registroID) {
        'use strict';
        try {

            swal.fire({
                title: "¡Eliminación!",
                text: "¿Está seguro que desea eliminar el registro?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: "SI",
                cancelButtonText: "NO",
            }).then(function (resultado) {

                if (resultado.isConfirmed) {

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/DeleteUsuarioAsync',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                seguridad.getAllUsuariosTerceros();
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteUsuarioAsync \n" + ex, "error");
        }
    },

    //Terceros
    asignarRolesPorDefectoPorTerceros: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/AsignarRolesPorDefectoPorTerceros',
                data: { terceroID: terceroID },
                dataType: 'JSON',
                async: true,
                success: function (respuesta) {

                    //if (respuesta.msn === "success") {

                    //    swal.fire("¡Notificación!", "El tercero se guardó correctamente.", "success");



                    //} else {

                    //    swal.fire("¡Alerta!", respuesta.error, "warning");

                    //}
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método asignarRolesPorDefectoPorTerceros \n" + ex, "error");
        }
    },

}