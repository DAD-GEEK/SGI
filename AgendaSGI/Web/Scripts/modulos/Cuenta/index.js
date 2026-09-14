var AccountUsuario = {

    getPerfilEditarAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Cuenta/GetMiPerfil',
                data: {},
                dataType: "html",
                success: function (respuesta) {

                    $('#divGetMiPerfil').html(respuesta);
                },
                error: function (textStatus) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getPerfilEditarAsync \n" + ex, "error");
        }
    },
};