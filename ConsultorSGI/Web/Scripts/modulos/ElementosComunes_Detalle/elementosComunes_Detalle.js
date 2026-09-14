var elementosComunesDetalleCRUD = {

    //Vistas
    ObtenerElementoComunPorIDAsync: function (elementoComunID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ElementosComunes_Detalle/ObtenerElementoComunPorIDAsync',
                data: { elementoComunID: elementoComunID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetElementosComunesDetalle').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerElementoComunPorIDAsync \n" + ex, "error");
        }
    },

    crearDetalleElementosComunesAsync: function (elementoComunID) {
        'use strict';

        var elementoComunID = $("#elementoComunID").val();

        if (elementoComunID == 0)
            return swal.fire("¡Alerta!", "Para agregar numerales debe guardar primero el elemento común que está creando.", "warning");

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ElementosComunes_Detalle/CrearDetalleElementosComunesAsync',
                data: { elementoComunID: elementoComunID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetElementosComunes').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearDetalleElementosComunesAsync \n" + ex, "error");
        }
    },

    getDetalleElementosComunesByEditAsync: function (elementoComunDetalleID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ElementosComunes_Detalle/GetDetalleElementosComunesByEditAsync',
                data: { elementoComunDetalleID: elementoComunDetalleID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetElementosComunes').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getDetalleElementosComunesByEditAsync \n" + ex, "error");
        }
    },

    //Base de datos  
    guardarElementoComunDetalleAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formGuardarDetalleGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntDetalleID": $("#elementoComunDetalleID").val(),
                        "IntElementoComunID": $("#elementoComunID").val(),
                        "StrInterpretacion": $("#elementoComunDetalleInterpretacion").val(),
                        "BitActivo": $("#elementoComunDetalleActivo").is(":checked"),
                        "ElementosComunes_Detalle_Numerales": null
                    };


                    var elementoComunDetalleNumerales = $(".elementoComunDetalleNumerales");

                    var listaNumerales = [];
                    var i = 0;

                    $.each(elementoComunDetalleNumerales, (index, elemento) => {

                        if ($(elemento).val() != "") {

                            var item = {
                                IntDetalleID: $("#elementoComunDetalleID").val(),
                                IntNormaID: $(elemento).data("norma"),
                                IntNumeralID: $(elemento).val(),
                            }

                            listaNumerales[i] = item;
                            i++;
                        }

                    });

                    modelo.ElementosComunes_Detalle_Numerales = listaNumerales;

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'ElementosComunes_Detalle/GuardarElementoComunDetalleAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {
                                    elementosComunesDetalleCRUD.getDetalleElementosComunesByEditAsync(respuesta.elementoComunDetalleID);
                                    return toastr.success("El registro se guardó correctamente.");
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                elementosComunesCRUD.getElementoComunByEditarAsync($("#elementoComunID").val(), true);
                            } else {
                                form.parsley().destroy();
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método guardarElementoComunDetalleAsync \n" + ex, "error");
            }
        });
    },

    deleteElementoComunDetalleAsync: function (registroID) {
        'use strict';
        try {

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
                        url: rootHost + 'ElementosComunes_Detalle/DeleteElementoComunDetalleAsync',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            elementosComunesCRUD.getElementoComunByEditarAsync(respuesta.elementoComunID);

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteElementoComunDetalleAsync \n" + ex, "error");
        }
    }
};
