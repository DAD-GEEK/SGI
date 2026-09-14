var auditoriaDetalleACCRUD = {

    index: function () {

    },

    crearDetalleAperturaCierre: function () {
        'use strict';

        try {

            $("#btnAgregarAperturaCierre").click(function () {

                $.ajax({
                    type: "POST",
                    url: rootHost + 'AuditoriasDetalleAC/CrearDetalleAperturaCierre',
                    data: {},
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
            swal.fire("Oops!", "Error en el método crearDetalleAperturaCierre \n" + ex, "error");
        }
    },

    getDetalleACByEditarAsync: function (auditoriaDetalleACID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'AuditoriasDetalleAC/GetDetalleACByEditarAsync',
                data: { auditoriaDetalleACID: auditoriaDetalleACID },
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
            swal.fire("Oops!", "Error en el método getPlanAuditoriaDetalleACByEditarAsync \n" + ex, "error");
        }
    },

    guardarDetalleAperturaCierreAsync: function () {
        'use strict';

        $(".btnGuardarRegistroModal").click(function (e) {
            try {
                
                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formGuardarDetalleACGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var fechaDetalle = $("#auditoriaDetalleACFecha").val();
                    var horaInicial = $("#auditoriaDetalleACHoraInicial").val();
                    var horaFinal = $("#auditoriaDetalleACHoraFinal").val();
                    var fechaInicial = moment(fechaDetalle + ' ' + horaInicial, "YYYY-MM-DD HH:mm a").format("YYYY-MM-DD HH:mm a");
                    var fechaFinal = moment(fechaDetalle + ' ' + horaFinal, "YYYY-MM-DD HH:mm a").format("YYYY-MM-DD HH:mm a")

                    var fechaAuditoria = $("#auditoriaFecha").val();
                    fechaAuditoria = fechaAuditoria.split(" ");
                    var fechaInicialAuditoria = fechaAuditoria[0];
                    var fechaFinalAuditoria = fechaAuditoria[2];

                    if (!auditoriasCRUD.validarFechasDeAuditoriaDentroDeRango(fechaInicial, fechaFinal))
                        return swal.fire("¡Alerta!", "La fecha debe estar entre " + fechaInicialAuditoria + " y " + fechaFinalAuditoria, "warning");

                    var modelo = {
                        'IntAuditoriaDetalleACID': $("#auditoriaDetalleACID").val(),
                        'IntAuditoriaID': $("#auditoriaID").val(),
                        'DatFechaInicial': fechaInicial,
                        'DatFechaFinal': fechaFinal,
                        'StrDetalle': $("#auditoriaDetalleACDetalle").val()
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'AuditoriasDetalleAC/GuardarDetalleAperturaCierreAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {

                                    toastr.success('El registro se guardó correctamente.');                                    
                                    auditoriaDetalleCRUD.getAllDetallePlanAuditoriasAsync($("#auditoriaID").val());
                                    return auditoriaDetalleACCRUD.getDetalleACByEditarAsync(respuesta.auditoriaDetalleACID)
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

                swal.fire("Oops!", "Error en el método guardarDetalleAperturaCierreAsync \n" + ex, "error");
            }
        });
    },

    deleteDetalleAperturaCierreAsync: function (auditoriaDetalleACID) {
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
                        url: rootHost + 'AuditoriasDetalleAC/DeleteDetalleAperturaCierreAsync',
                        data: { auditoriaDetalleACID: auditoriaDetalleACID },
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

            swal("Oops!", "Error en el método deleteDetalleAperturaCierreAsync \n" + ex, "error");
        }
    },

}