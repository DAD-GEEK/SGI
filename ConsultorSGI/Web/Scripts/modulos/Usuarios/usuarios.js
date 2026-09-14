var usuariosCRUD = {

    //Vistas
    getAllUsuarios: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Usuarios/GetAllUsuariosAsync',
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



};