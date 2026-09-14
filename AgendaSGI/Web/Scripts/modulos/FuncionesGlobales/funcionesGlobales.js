var funcionesGlobales = {

    generarAjaxMensajeException: function (jqXHR, textStatus) {

        if (jqXHR.status === 0)
            swal.fire('¡Opps!', 'Sin conexión: Verifique su conexión a internet.', 'error');
        else if (jqXHR.status == 404)
            swal.fire('¡Opps!', 'Página no encontrada [404]', 'error');
        else if (jqXHR.status == 500)
            swal.fire('¡Opps!', 'Error interno del servidor [500] - ' + jqXHR.responseText, 'error');
        else if (textStatus === 'parsererror')
            swal.fire('¡Opps!', 'Solicitud JSON parse ha fallado.', 'error');
        else if (textStatus === 'timeout')
            swal.fire('¡Opps!', 'El tiempo de espera ha terminado', 'error');
        else if (textStatus === 'abort')
            swal.fire('¡Opps!', 'La solicitud Ajax ha sido abortada.', 'error');
        else
            swal.fire('¡Opps!', 'Error no encontrado: ' + jqXHR.responseText, 'error');
    },

    convertirEnDecimal: function (numero, cantidadDecimales) {

        numero = String(numero);
        return parseFloat(numero.replace(/,/gi, '').replace('$', '')).toFixed(cantidadDecimales);

    },

}
