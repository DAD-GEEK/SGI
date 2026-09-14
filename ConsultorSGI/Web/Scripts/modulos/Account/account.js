var accountUser = {

    iniciarSesionAsync: function () {
        'use strict';

        $("#formValidarLogin").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {


                    //modelo Cliente
                    var modelo = {

                        //"TerceroID": $("#loginNitTercero").val(),
                        "Email": $("#loginIdentificacionUsuario").val(),
                        "Password": $("#LoginClave").val(),

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Security/VerificarInicioSesion',
                        data: { __RequestVerificationToken: token, model: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');
                                $('#contenedor').empty();

                                preguntasCRUD.getAllPreguntas();

                                swal("¡Notificación!", "El pregunta se agregó correctamente.", "success");


                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal("¡Valicación!", respuesta.error, "warning");

                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }

            } catch (ex) {

                swal("Oops!", "Error en el método crearPreguntaAsync \n" + ex, "error");
            }
        });
    },

}