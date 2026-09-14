var manage = {

    updateDatosDePerfilDeUsuarioAsync: function () {
        'use strict';

        try {

            $("#formGuardarMiPerfil").submit(function (e) {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {
                        'NombreUsuario': $('#usuarioNombre').val(),
                        'UserName': $('#usuarioEmail').val(),
                        'PhoneNumber': $('#usuarioTelefono').val()
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Manage/UpdateDatosDePerfilDeUsuarioAsync',
                        data: { __RequestVerificationToken: token, model: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var isUserImagenAvatar = true;
                                var imagen = $("#usuarioImagen");

                                if (imagen.val() != "")
                                    manage.guardarImagenAsync(imagen, respuesta.userName, isUserImagenAvatar);

                                var imagen = $("#usuarioFirma");
                                isUserImagenAvatar = false;

                                if (imagen.val() != "")
                                    manage.guardarImagenAsync(imagen, respuesta.userName, isUserImagenAvatar);

                            }
                            else
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                        },
                        error: function (respuesta) {
                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });
                }
            });


        } catch (ex) {
            swal.fire("Oops!", "Error en el método updateDatosDePerfilDeUsuarioAsync \n" + ex, "error");
        }
    },

    cambiarPassword: function () {
        'use strict';

        try {

            $("#formCambiarPassword").submit(function (e) {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    if ($('#usuarioConfirmarContraseña').val() != $('#usuarioContraseñaNueva').val())
                        return swal.fire("¡Alerta!", "El campo nueva contraseña no coincide con el campo confirmar contraseña.", "warning");

                    if ($('#usuarioContraseñaAnterior').val() == $('#usuarioContraseñaNueva').val())
                        return swal.fire("¡Alerta!", "La nueva contraseña debe ser diferente a la anterior contraseña.", "warning");

                    var modelo = {

                        'OldPassword': $('#usuarioContraseñaAnterior').val(),
                        'NewPassword': $('#usuarioContraseñaNueva').val(),
                        'ConfirmPassword': $('#usuarioConfirmarContraseña').val()
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Manage/ChangePassword',
                        data: { __RequestVerificationToken: token, model: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                Swal.fire({
                                    title: '¡Cambiar contraseña!',
                                    text: 'La contraseña se cambio correctamente.',
                                    icon: 'success',
                                    showDenyButton: false,
                                    showCancelButton: false,
                                    confirmButtonText: 'OK',
                                }).then((result) => {

                                    if (result.isConfirmed) {
                                        window.location.href = respuesta.url + "/Manage";
                                    }
                                });
                            }
                            else
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                        },
                        error: function (respuesta) {
                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });
                }
            });


        } catch (ex) {
            swal.fire("Oops!", "Error en el método guardarEmpleadoAsync \n" + ex, "error");
        }
    },

    crearNuevaPassword: function () {
        'use strict';

        try {

            $("#formCrearNuevoPassword").submit(function (e) {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    if ($('#usuarioConfirmarContraseña').val() != $('#usuarioContraseñaNueva').val())
                        return swal.fire("¡Alerta!", "El campo nueva contraseña no coincide con el campo confirmar contraseña.", "warning");

                    var modelo = {

                        'NewPassword': $('#usuarioContraseñaNueva').val(),
                        'ConfirmPassword': $('#usuarioConfirmarContraseña').val()
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Manage/SetPassword',
                        data: { __RequestVerificationToken: token, model: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                Swal.fire({
                                    title: '¡Crear contraseña!',
                                    text: 'La contraseña se generó correctamente.',
                                    icon: 'success',
                                    showDenyButton: false,
                                    showCancelButton: false,
                                    confirmButtonText: 'OK',
                                }).then((result) => {

                                    if (result.isConfirmed) {
                                        window.location.href = respuesta.url + "/Manage";
                                    }
                                });
                            }
                            else
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                        },
                        error: function (respuesta) {
                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });
                }
            });


        } catch (ex) {
            swal.fire("Oops!", "Error en el método guardarEmpleadoAsync \n" + ex, "error");
        }
    },

    guardarImagenAsync: function (elementoFile, userName, isUserImage) {
        'use strict'

        try {

            var formData = new FormData();
            formData.append('image', elementoFile[0].files[0]);
            formData.append('userName', userName);
            formData.append('isUserImageAvatar', isUserImage);

            $.ajax({
                type: "POST",
                url: rootHost + 'Manage/GuardarImagenAsync',
                data: formData,
                async: false,
                dataType: 'JSON',
                contentType: false,
                processData: false,
                success: function (response) {

                    if (response.msn === "success") {

                        Swal.fire({
                            title: '¡Notificación!',
                            text: 'El registro se guardó correctamente.',
                            icon: 'success',
                            showDenyButton: false,
                            showCancelButton: false,
                            confirmButtonText: 'OK',
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = response.url + "/Manage";
                            }
                        });

                    } else {
                        swal.fire("Oops!", response.error, "error");
                    }
                },
                error: function (ex) {

                    swal.fire("Oops!", ex, "error");
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método guardarImagenAsync \n" + ex, "error");
        }

    },



}