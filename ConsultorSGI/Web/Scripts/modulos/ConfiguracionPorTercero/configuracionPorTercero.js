var configuracionPorTerceroCRUD = {

    vistaInformacionGeneralAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ConfiguracionPorTercero/VistaInformacionGeneralAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divVistaParcialMiEmpresa').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método vistaInformacionGeneralAsync \n" + ex, "error");
        }
    },

};