var home = {

    //Vistas
    getAllSistemasDeGestion: function () {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'Home/GetAllSistemasDeGestion',
                data: {},
                dataType: "html",
                async: false,
                success: function (respuesta) {

                    $('#divSistemasDeGestion').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAll \n" + ex, "error");
        }
    },

};
