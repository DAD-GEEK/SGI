var configuracionCRUD = {

    getConfiguracion: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Configuracion/GetConfiguracion',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetConfiguracion').html(respuesta);
                },
                error: function (textStatus) {
                    //console.log(textStatus);
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getConfiguracion\n" + ex, "error");
        }
    },

    crearConfiguracionAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrHoraInicial": String(moment($("#configuracionHoraInicial").val(), "h:mm A").format("HH:mm:ss")),
                        "StrHoraFinal": String(moment($("#configuracionHoraFinal").val(), "h:mm A").format("HH:mm:ss")),
                        "StrVistaAgenda": $("#configuracionVistaDefault").val(),
                        "StrSMPTEmail": $("#configuracionEmailRemitente").val(),
                        "StrSMTPPassword": $("#configuracionClave").val(),
                        "StrSMTPPort": $("#configuracionPuerto").val(),
                        "StrSMTPHost": $("#configuracionHost").val(),
                        "OpcSMTPSSL": $("#opcSSL").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Configuracion/CrearConfiguracionAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                configuracionCRUD.getConfiguracion();

                                swal.fire("¡Notificación!", "El registro se actualizó correctamente.", "success");

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

                swal.fire("Oops!", "Error en el método crearConfiguracionAsync \n" + ex, "error");
            }
        });
    },

    updateConfiguracionAsync: function () {
        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrHoraInicial": String(moment($("#configuracionHoraInicial").val(), "h:mm A").format("HH:mm:ss")),
                        "StrHoraFinal": String(moment($("#configuracionHoraFinal").val(), "h:mm A").format("HH:mm:ss")),
                        "StrVistaAgenda": $("#configuracionVistaDefault").val(),
                        "StrSMPTEmail": $("#configuracionEmailRemitente").val(),
                        "StrSMTPPassword": $("#configuracionClave").val(),
                        "StrSMTPPort": $("#configuracionPuerto").val(),
                        "StrSMTPHost": $("#configuracionHost").val(),
                        "OpcSMTPSSL": $("#opcSSL").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Configuracion/UpdateConfiguracionAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                configuracionCRUD.getConfiguracion();

                                swal.fire("¡Notificación!", "El registro se actualizó correctamente.", "success");

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

                swal.fire("Oops!", "Error en el método updateConfiguracionAsync \n" + ex, "error");
            }
        });
    },

};