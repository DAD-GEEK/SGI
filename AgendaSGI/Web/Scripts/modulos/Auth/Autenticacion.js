var usuarioAutenticacion = {

    validarLoginAsync: function () {
        'use strict';

        $("#formValidarLogin").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                $("#formValidarLogin").parsley().validate();

                if (form.parsley().isValid()) {

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Auth/Login',
                        data: { __RequestVerificationToken: token, usuarioLogin: $("#usuarioLogin").val(), contraseñaLogin: $("#contraseñaLogin").val() },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (respuesta) {

                            if (respuesta.login == true) {
                                $("#errorLogin").text("");
                                window.location.replace(rootHost + "Home");

                            } else {
                                $("#errorLogin").text(respuesta.error);
                                $("#usuarioLogin").val("");
                                $("#contraseñaLogin").val("");
                                $('#formValidarLogin').parsley().destroy();
                            }
                        },
                        error: function (ex) {

                            window.location.replace(rootHost + "Auth/Login");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método validarLoginAsync \n" + ex, "error");
            }
        });
    },

};