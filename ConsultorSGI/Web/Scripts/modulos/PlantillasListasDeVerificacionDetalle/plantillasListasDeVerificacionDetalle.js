var plantillaListasDeVerificacionDetalleCRUD = {

    getAllPlantillasListasDeVerificacionDetalleAsync: function (procesoID, esVistaGlobal) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

        var esVistaListaDeVerificacion = $("#esVistaListaDeVerificacion").is(":checked");

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'PlantillasListasDeVerificacionDetalle/GetAllPlantillasListasDeVerificacionDetalleAsync',
                data: { procesoID: procesoID, esVistaGlobal: esVistaGlobal, esVistaListaDeVerificacion: esVistaListaDeVerificacion },
                dataType: "html",
                async: true,
                success: function (respuesta) {
                    $('#divPlantillaDetalle').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllPlantillasListasDeVerificacionDetalleAsync \n" + ex, "error");
        }
    },

    crearPlantillaListaDeVerificacionDetalle: function (procesoID, procesoNombre, esVistaGlobal) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'PlantillasListasDeVerificacionDetalle/CrearPlantillaListaDeVerificacionDetalle',
                data: { procesoID: procesoID, procesoNombre: procesoNombre, esVistaGlobal: esVistaGlobal },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    if (esVistaGlobal)
                        return $('#divGetProcesos').empty().html(respuesta);

                    $('#contenedor2').empty().html(respuesta);
                    $('#modal2').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearPlantillaListaDeVerificacionDetalle \n" + ex, "error");
        }
    },

    obtenerPlantillaListaDeVerificacionDetalleParaEditar: function (plantillaDetalleID, esVistaGlobal) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'PlantillasListasDeVerificacionDetalle/ObtenerPlantillaListaDeVerificacionParaEditar',
                data: { plantillaDetalleID: plantillaDetalleID, esVistaGlobal: esVistaGlobal },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    if (esVistaGlobal) {
                        $('#divGetProcesos').empty().html(respuesta);
                        return;
                    }

                    $('#contenedor2').empty().html(respuesta);
                    $('#modal2').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerPlantillaListaDeVerificacionParaEditar \n" + ex, "error");
        }
    },

    guardarPalntillaListaVerificacionDetalleAsync: function (esVistaGlobal, esVistaListaVerificacion) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

        if (!esVistaListaVerificacion)
            esVistaListaVerificacion = false;

        $(".btnGuardarRegistro,.btnGuardarItemLista").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarDetalleGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntPlantillaDetalleID": $("#plantillaDetalleID").val(),
                        "IntProcesoID": $("#procesoID").val(),
                        "StrTitulo": $("#plantillaDetalleDescripcion").val(),
                        "BitEstado": $("#plantillaDetalleEstado").is(":checked"),
                        "PlantillasListasDeVerificacion_Numerales": null
                    };

                    var auditoriaID = $("#auditoriaID").val();

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'PlantillasListasDeVerificacionDetalle/GuardarPlantillaListasDeVerificacionDetalleAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, auditoriaID: auditoriaID },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (!esVistaGlobal)
                                    plantillaListasDeVerificacionDetalleCRUD.getAllPlantillasListasDeVerificacionDetalleAsync(respuesta.procesoID, esVistaGlobal);

                                if (aplicar) {
                                    toastr.success('El registro se guardó correctamente.');
                                    return plantillaListasDeVerificacionDetalleCRUD.obtenerPlantillaListaDeVerificacionDetalleParaEditar(respuesta.plantillaDetalleID, esVistaGlobal);
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                procesosCRUD.getProcesoByEditarAsync(respuesta.procesoID, esVistaGlobal, esVistaListaVerificacion);

                                if (!esVistaGlobal) {
                                    $('#contenedor2').empty();
                                    $('#modal2').modal('hide');
                                }

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

                swal.fire("Oops!", "Error en el método guardarPalntillaListaVerificacionAsync \n" + ex, "error");
            }
        });
    },

    deletePlantillaListaDeVerificacionDetalleAsync: function (plantillaDetalleID, esVistaGlobal) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

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
                        url: rootHost + 'PlantillasListasDeVerificacionDetalle/DeletePlantillaListaDeVerificacionDetalleAsync',
                        data: { plantillaDetalleID: plantillaDetalleID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                                plantillaListasDeVerificacionDetalleCRUD.getAllPlantillasListasDeVerificacionDetalleAsync(respuesta.procesoID);

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deletePlantillaListaDeVerificacionAsync \n" + ex, "error");
        }
    },

    agregarNumeralAsync: function () {
        'use strict';

        $("#btnAgregarNumeral").click(function (e) {
            try {

                e.preventDefault();

                var form = $("#formAgregarNumeral");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntPlantillaDetalleID": $("#plantillaDetalleID").val(),
                        "IntNumeralID": $("#plantillaDetalleNumerales").val(),
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'PlantillasListasDeVerificacionDetalle/GuardarPlantillaDetalleNumeralAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var normaID = $("#plantillaDetalleNormas").val();
                                if (normaID == "")
                                    normaID = 0;

                                plantillaListasDeVerificacionDetalleCRUD.vistaObtenerNumeralesPorDetalleAsync($("#plantillaDetalleID").val(), normaID);

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

                swal.fire("Oops!", "Error en el método agregarNumeralAsync \n" + ex, "error");
            }
        });
    },

    deleteNumeralPlantillaDetalleAsync: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'PlantillasListasDeVerificacionDetalle/DeleteNumeralPlantillaDetalleAsync',
                data: { registroID: registroID },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                    } else
                        swal.fire("¡Alerta!", respuesta.error, "warning");

                    var normaID = 0;
                    plantillaListasDeVerificacionDetalleCRUD.vistaObtenerNumeralesPorDetalleAsync($("#plantillaDetalleID").val(), normaID);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteNumeralPlantillaDetalleAsync \n" + ex, "error");
        }
    },

    cambiarOrdenamientoPlantillasAsync: function (listaPlantillasOrdenada) {
        'use strict'

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'PlantillasListasDeVerificacionDetalle/CambiarOrdenamientoPlantillasAsync',
                data: { listaPlantillasOrdenada: listaPlantillasOrdenada },
                dataType: 'JSON',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {



                    } else {
                        form.parsley().destroy();
                        swal.fire("¡Alerta!", respuesta.error, "warning");
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });


        } catch (ex) {

            swal.fire("Oops!", "Error en el método cambiarOrdenamientoPlantillasAsync \n" + ex, "error");
        }


    },

}