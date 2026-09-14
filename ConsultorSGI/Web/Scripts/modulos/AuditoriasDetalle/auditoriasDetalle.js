var auditoriaDetalleCRUD = {

    getAllDetallePlanAuditoriasAsync: function () {
        'use strict';

        try {

            var auditoriaID = $("#auditoriaID").val();

            $.ajax({
                type: "POST",
                url: rootHost + 'AuditoriasDetalle/GetAllDetallePlanAuditoriasAsync',
                data: { auditoriaID: auditoriaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#getAuditoriaDetalle').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllDetallePlanAuditoriasAsync \n" + ex, "error");
        }
    },

    crearPlanAuditoriaDetalle: function () {
        'use strict';

        try {

            $("#btnCrearDePlanDeAuditoriaDetalle").click(function () {

                var auditoriaID = $("#auditoriaID").val();

                $.ajax({
                    type: "POST",
                    url: rootHost + 'AuditoriasDetalle/CrearPlanAuditoriaDetalle',
                    data: { auditoriaID: auditoriaID },
                    dataType: "html",
                    async: true,
                    success: function (respuesta) {

                        $('#contenedor').empty().html(respuesta);
                        $('#modal').modal('show');

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                    }
                });

            });

           

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearPlanAuditoriaDetalle \n" + ex, "error");
        }
    },

    getPlanAuditoriaDetalleByEditarAsync: function (auditoriaDetalleID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'AuditoriasDetalle/GetAuditoriaDetalleByEditarAsync',
                data: { auditoriaDetalleID: auditoriaDetalleID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getPlanAuditoriaDetalleByEditarAsync \n" + ex, "error");
        }
    },

    guardarAuditoriaDetalleAsync: function () {
        'use strict';

        $(".btnGuardarRegistroModal").click(function (e) {
            try {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formGuardarDetalleGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var fechaDetalle = $("#auditoriaDetalleFecha").val();
                    var horaInicial = $("#auditoriaDetalleHoraInicial").val();
                    var horaFinal = $("#auditoriaDetalleHoraFinal").val();
                    var fechaInicial = moment(fechaDetalle + ' ' + horaInicial, "YYYY-MM-DD hh:mm a").format("YYYY-MM-DD hh:mm a");
                    var fechaFinal = moment(fechaDetalle + ' ' + horaFinal, "YYYY-MM-DD hh:mm a").format("YYYY-MM-DD hh:mm a");

                    var fechaAuditoria = $("#auditoriaFecha").val();
                    fechaAuditoria = fechaAuditoria.split(" ");
                    var fechaInicialAuditoria = fechaAuditoria[0];
                    var fechaFinalAuditoria = fechaAuditoria[2];

                    if (!auditoriasCRUD.validarFechasDeAuditoriaDentroDeRango(fechaInicial, fechaFinal))
                        return swal.fire("¡Alerta!", "La fecha debe estar entre " + fechaInicialAuditoria + " y " + fechaFinalAuditoria, "warning");

                    var modelo = {
                        'IntAuditoriaDetalleID': $("#auditoriaDetalleID").val(),
                        'IntAuditoriaID': $("#auditoriaID").val(),
                        'IntProcesoID': $("#auditoriaDetalleProceso").val(),
                        'StrModalidad': $("#auditoriaDetalleModalidad").val(),
                        'StrAuditado': $("#auditoriaDetalleAuditado").val(),
                        'StrAuditor': $("#auditoriaDetalleAuditor").val(),
                        'DatFechaInicial': fechaInicial,
                        'DatFechaFinal': fechaFinal,
                        'StrDetalle': '',
                        'DatFechaProgramacion': $("#auditoriaDetalleFechaProgramacion").val()
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'AuditoriasDetalle/GuardarAuditoriaDetalleAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {

                                    toastr.success('El registro se guardó correctamente.');
                                    auditoriaDetalleCRUD.getAllDetallePlanAuditoriasAsync($("#auditoriaID").val());
                                    return auditoriaDetalleCRUD.getPlanAuditoriaDetalleByEditarAsync(respuesta.auditoriaDetalleID);
                                }

                                auditoriaDetalleCRUD.getAllDetallePlanAuditoriasAsync($("#auditoriaID").val());
                                $('#modal').modal('hide');
                                $('#contenedor').empty();

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

                swal.fire("Oops!", "Error en el método guardarAuditoriaDetalleAsync \n" + ex, "error");
            }
        });
    },

    deleteAuditoriaDetalleAsync: function (auditoriaDetalleID) {
        'use strict';
        try {

            swal.fire({
                title: "¡Eliminación!",
                text: "¿Está seguro que desea eliminar la programación actual? \nLos datos se perderán para siempre.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: "SI",
                cancelButtonText: "NO",
            }).then(function (resultado) {

                if (resultado.isConfirmed) {

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'AuditoriasDetalle/DeleteAuditoriaDetalleAsync',
                        data: { auditoriaDetalleID: auditoriaDetalleID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            auditoriaDetalleCRUD.getAllDetallePlanAuditoriasAsync($("#auditoriaID").val());

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteAuditoriaDetalleAsync \n" + ex, "error");
        }
    },

}