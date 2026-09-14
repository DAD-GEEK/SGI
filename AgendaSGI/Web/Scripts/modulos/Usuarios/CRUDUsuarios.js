var usuariosCRUD = {

    getAllUsuarios: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Usuarios/GetAllUsuarios',
                data: {},
                dataType: "html",
                success: function (respuesta) {

                    $('#divGetAllUsuarios').html(respuesta);
                },
                error: function (textStatus) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método GetAllTerceros \n" + ex, "error");
        }
    },

    crearUsuarioAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrCodigo": $("#usuarioCodigo").val(),
                        "StrNombre": $("#usuarioNombre").val(),
                        "StrClave": $("#usuarioClave").val(),
                        "StrEmail": ($("#usuarioEmail").val()).toLowerCase(),
                        "StrColor": "#C1CDDD",
                        "IntRolID": $("#usuarioRol").val(),
                        "OpcEstado": true 
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Usuarios/CrearUsuarioAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                usuariosCRUD.getAllUsuarios();

                                swal.fire("¡Notificación!", "El usuario se agregó correctamente.", "success");


                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Validación!", respuesta.error, "info");


                            }
                        },
                        error: function (ex) {

                            swal.fire("Oops!", ex, "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearUsuarioAsync \n" + ex, "error");
            }
        });
    },

    getUsuarioParaEditarAsync: function (id) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Usuarios/GetUsuarioParaEditarAsync',
                data: "{ usuarioID: '" + $(id).val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);

                    $('#modal').modal('show');

                },
                error: function (respuesta) {

                    swal.fire("Oops!", JSON.stringify(respuesta), "error");
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método getTerceroParaEditarAsync \n" + ex, "error");
        }
    },

    updateUsuarioAsync: function () {

        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {
     
                    var usuarioClave = $("#usuarioClave").val();
                    var opcEstado = $("#opcEstado").is(':checked');
                    var vista = "El usuario"
                    var tipo = $("#tipoVista").val();

                    if (tipo == 'p') {
                        opcEstado = true;
                        vista = "El perfil";  

                        //Validar cambio de contraseña
                        if ($("#opcCambiarClave").is(":checked")) {

                            if ($("#usuarioClaveAnt").val().length == 0 || $("#usuarioClave").val().length == 0) {
                                swal.fire("¡Notificación!", "La contraseña debe tener mínimo un caracter.", "info");
                                return;
                            }

                            if ($("#usuarioClave").val() != $("#usuarioClaveConfirmar").val()) {
                                swal.fire("¡Notificación!", "El campo confirmar contraseña no coincide con la nueva contraseña.", "info");
                                return;
                            }

                        }      
                    }                      

                    var modelo = {
                        "IntUsuarioID": $("#usuarioID").val(),
                        "StrCodigo": $("#usuarioCodigo").val(),
                        "StrNombre": $("#usuarioNombre").val(),
                        "StrClave": usuarioClave,
                        "StrEmail": ($("#usuarioEmail").val()).toLowerCase(),
                        "IntCiudadID": $("#usuarioCiudad").val(),
                        "StrDireccion": $("#usuarioDireccion").val(),
                        "StrTelefonoFijo": $("#usuarioTelefonoFijo").val(),
                        "StrCelular": $("#usuarioCelular").val(),
                        "StrColor": $("#usuarioColor").val(),
                        "IntRolID": $("#usuarioRol").val(),
                        "OpcEstado": opcEstado
                    };

                    var claveAnt = $("#usuarioClaveAnt").val();

                    var token = $('input[name="__RequestVerificationToken"]').val();
                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Usuarios/UpdateUsuarioAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, tipo: tipo, opcClave: $("#opcCambiarClave").is(":checked"), claveAnt: claveAnt  },
                        async: true,
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if ($("#tipoVista").val() == 'p') {
                                    AccountUsuario.getPerfilEditarAsync();
                                } else {

                                    $('#modal').modal('hide');

                                    usuariosCRUD.getAllUsuarios();
                                }
                                swal.fire("¡Notificación!", vista + " se actualizó correctamente.", "success");

                            }else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Notificación!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Notificación!", respuesta.error, "info");
                            }
                        },
                        error: function (ex) {

                            swal.fire("Oops!", ex, "error");
                        }
                    });

                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método updateUsuarioAsync \n" + ex, "error");
            }
        });
    },

    deleteUsuarioAsync: function (id) {
        'use strict';
        try {
            var token = $('input[name="__RequestVerificationToken"]').val();

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
                        url: rootHost + 'Usuarios/DeleteUsuarioAsync',
                        data: { __RequestVerificationToken: token, usuarioID: $(id).val() },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                usuariosCRUD.getAllUsuarios();

                                swal.fire("¡Notificación!", "El usuario se eliminó correctamente.", "success");
                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;

                                }
                                swal.fire("¡Valicación!", respuesta.error, "info");

                            }
                        },
                        error: function (respuesta) {

                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });


                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

       

        } catch (ex) {

            swal.fire("Oops!", "Error en el método deleteUsuarioAsync \n" + ex, "error");
        }
    },

    CheckExists: function (id) {

        $.ajax({
            type: "POST",
            url: rootHost + 'Usuarios/CheckExists',
            data: "{ StrCodigo: '" + $(id).val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            async: true,
            success: function (respuesta) {

                var codigo = $("#codigo").val();

                $("#codigoIcono").show();
                $(id).addClass("border-right-0");
                $(id).removeClass("is-invalid");
                $("span small").text("");

                if (respuesta === "True" && $(id).val() != codigo) {

                    $("#codigoIcono").hide();
                    $(id).removeClass("border-right-0");
                    $(id).addClass("is-invalid");
                    $("span small").text("Este usuario ya existe. Intente con otro diferente.");
                }
            },
            error: function (respuesta) {

                swal.fire("Oops!", JSON.stringify(respuesta), "error");
            }
        });
    },

};