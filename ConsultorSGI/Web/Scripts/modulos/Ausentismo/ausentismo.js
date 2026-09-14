var ausentismoCRUD = {

    getAllAusentismoAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetAllAusentismoAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismo').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllAusentismoAsync \n" + ex, "error");
        }
    },

    getAusentismoAgrupadoMensualmentePorAñoAsync: function (valor) {
        'use strict';

        var anio = $("#ausentismoAnio").html();

        if (typeof anio === 'undefined') {
            var fechaActual = new Date();
            anio = fechaActual.getFullYear();
        }

        anio = parseInt(anio) + parseInt(valor);

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetAusentismoAgrupadoMensualmentePorAñoAsync',
                data: { anio: anio },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismo').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAusentismoAgrupadoMensualmentePorAñoAsync \n" + ex, "error");
        }
    },

    getAusentismoAgrupadoPorAreaAsync: function (valor) {
        'use strict';

        var anio = $("#ausentismoAnio").html();

        if (typeof anio === 'undefined') {
            var fechaActual = new Date();
            anio = fechaActual.getFullYear();
        }

        anio = parseInt(anio) + parseInt(valor);

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetAusentismoAgrupadoPorAreaAsync',
                data: { anio: anio },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismo').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAusentismoAgrupadoMensualmentePorAñoAsync \n" + ex, "error");
        }
    },

    getAusentismoAgrupadoPorProcesoAsync: function (valor) {
        'use strict';

        var anio = $("#ausentismoAnio").html();

        if (typeof anio === 'undefined') {
            var fechaActual = new Date();
            anio = fechaActual.getFullYear();
        }

        anio = parseInt(anio) + parseInt(valor);

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetAusentismoAgrupadoPorProcesoAsync',
                data: { anio: anio },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismo').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAusentismoAgrupadoMensualmentePorAñoAsync \n" + ex, "error");
        }
    },

    registrarAusentismo: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/RegistrarAusentismo',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismo').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método registrarAusentismo \n" + ex, "error");
        }
    },

    getDatosEmpleadoAsync: function () {
        'use strict';
        var empleadoID = $("#ausentismoEmpleado").val();

        if (empleadoID.length != 0) {
            try {

                $.ajax({
                    type: "POST",
                    url: rootHost + 'Ausentismo/GetDatosEmpleadosAsync',
                    data: { empleadoID: empleadoID },
                    dataType: "html",
                    async: true,
                    success: function (respuesta) {

                        $('#divGetDatosEmpleado').empty().html(respuesta);

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                    }
                });

            } catch (ex) {
                swal.fire("Oops!", "Error en el método getDatosEmpleadoAsync \n" + ex, "error");
            }
        }

    },

    getCostosAusentismoAsync: function () {
        'use strict';

        try {

            var url = rootHost + 'Ausentismo/GetCostosAusentismoAsync';
            var data = { ausentismoID: $("#ausentismoID").val() };
            var options = {
                method: 'POST',
                body: JSON.stringify(data),
                headers: {
                    'Content-type': 'application/json',
                    'dataType': 'html',
                    'async': true,
                }
            };

            fetch(url, options)
                .then(function (respuesta) {
                    if (respuesta.ok) {
                        return respuesta.text();
                    }
                })
                .then(function (data) {

                    $('#divGetCostosAusentismo').empty().html(data);
                    $('#divGetCostosAusentismo').show('fade');

                })
                .catch(function (error) {
                    swal.fire("¡Error!", "Error al ejecturar", "danger");
                });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getCostosAusentismoAsync \n" + ex, "error");
        }

    },

    buscarDiagnosticoAsync: function (elemento) {
        'use strict';

        try {

            var url = rootHost + 'Ausentismo/BuscarDiagnosticoAsync';
            var data = { codigoDiagnostico: $(elemento).val() };
            var options = {
                method: 'POST',
                body: JSON.stringify(data),
                headers: {
                    'Content-type': 'application/json',
                }
            };

            fetch(url, options)
                .then(function (respuesta) {
                    if (respuesta.ok) {
                        return respuesta.json();
                    }
                })
                .then(function (data) {

                    if (data.msn === "success") {

                        //$("#ausentismoDescripcionDiagnostico").parsley().validate();
                        $("#ausentismoCodigoDiagnostico").val(data.diagnosticoCodigo);
                        $("#ausentismoCodigoDiagnostico").data("diagnosticoid", data.diagnosticoID);
                        $("#ausentismoDescripcionDiagnostico").val(data.diagnosticoDescripcion);
                        $("#mensajeErrorDiagnostico").html("");

                    } else {
                        $("ausentismoCodigoDiagnostico").data("diagnosticoid", "0");
                        $("#mensajeErrorDiagnostico").html(data.error);
                        $("#ausentismoDescripcionDiagnostico").val("");
                    }

                })
                .catch(function (error) {
                    swal.fire("¡Error!", "Error al ejecturar", "danger");
                });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método buscarDiagnosticoAsync \n" + ex, "error");
        }

    },

    calcularTiempoAusentismo: function () {

        var empleado = $("#ausentismoEmpleado").val();
        if (empleado.length == 0)
            return swal.fire("¡Alerta!", "Seleccione un empleado para agregar el tiempo de incapacidad.", "warning");

        if ($("#lblAusentismoDiasIncapacidad").html().length == 0)
            return swal.fire("¡Alerta!", "Seleccione una fecha para calcular el tiempo de incapacidad.", "warning");

        var diasIncapacidad = $("#lblAusentismoDiasIncapacidad").html();
        var prorroga = $("#lblAusentismoDiasProrroga").html();

        if (prorroga === undefined)
            prorroga = 0;

        $("#lblAusentismoDiasIncapacidad").html(diasIncapacidad);
        $("#lblAusentismoDiasProrroga").html(prorroga);
        var totalDias = parseInt(diasIncapacidad) + parseInt(prorroga);
        $("#lblAusentismoTotalDiasIncapacidad").html(totalDias);

        //ausentismoCRUD.getDatosMonetareosAsync();

    },

    registrarAusentismoAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var diasIncapapcidad = $("#lblAusentismoTotalDiasIncapacidad").html();
                    if (diasIncapapcidad == 0 || diasIncapapcidad.length == 0)
                        return swal.fire("¡Alerta!", "Debe calcular los dias de incapacidad antes de guardar los cambios.", "warning");

                    var fechaAusentismo = $("#ausentismoFecha").val();
                    fechaAusentismo = fechaAusentismo.split(" ");
                    var fechaInicial = fechaAusentismo[0];
                    var fechaFinal = fechaAusentismo[2];

                    var modelo = {

                        'IntAusentismoID': $('#ausentismoID').val(),
                        'IntAno': $('#ausentismoAnoEvento').val(),
                        'TIntPeriodo': $('#ausentismoMesEvento').val(),
                        'DatFechaInicial': fechaInicial,
                        'DatFechaFinal': fechaFinal,
                        'IntDiasCargados': "0",
                        'StrObservaciones': $("#ausentismoObservaciones").val(),
                        'IntEmpleadoID': $('#ausentismoEmpleado').val(),
                        'IntTipoEventoAusentismoID': $('#ausentismoTipoEvento').val(),
                        'IntDiagnosticoID': $('#ausentismoCodigoDiagnostico').data("diagnosticoid"),

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Ausentismo/RegistrarAusentismoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {
                                    ausentismoCRUD.getAusentismoByEditarAsync(respuesta.ausentismoID);
                                    return toastr.success('El registro se guardó correctamente.');
                                }

                                swal.fire("¡Notificación!", "El ausentismo se guardó correctamente.", "success");
                                ausentismoCRUD.getAllAusentismoAsync();

                            } else {
                                if (aplicar)
                                    return toastr.error(respuesta.error);

                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }


                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método registrarAusentismoAsync \n" + ex, "error");
            }
        });
    },

    agregarProrrogaAsync: function (fechaInicial, fechaFinal) {
        'use strict';

        try {

            var modelo = {

                'IntAusentismoID': $('#ausentismoID').val(),
                'DatFechaInicial': fechaInicial,
                'DatFechaFinal': fechaFinal,

            };

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/AgregarProrrogaAsync',
                data: { modelo: modelo },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                        $("#ausentismoProrroga").val('');
                        ausentismoCRUD.getProrrogasAsync(respuesta.ausentismoID);
                        ausentismoCRUD.getCostosAusentismoAsync();
                        return toastr.success('La prórroga se agregó correctamente.');

                    } else
                        return toastr.error(respuesta.error);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método agregarProrrogaAsync \n" + ex, "error");
        }

    },

    getAusentismoByEditarAsync: function (ausentismoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetAusentismoByEditarAsync',
                data: { ausentismoID: ausentismoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismo').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAusentismoByEditarAsync \n" + ex, "error");
        }
    },

    getAusentismoAgrupadoMensualmentePorEmpleadoAsync: function (periodo) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetAusentismoAgrupadoMensualmentePorEmpleadoAsync',
                data: { año: $("#ausentismoAnio").html(), periodo: periodo },
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
            swal.fire("Oops!", "Error en el método getAusentismoAgrupadoMensualmentePorEmpleadoAsync \n" + ex, "error");
        }
    },

    getAusentismoAgrupadoPorArea_EmpleadoAsync: function (areaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetAusentismoAgrupadoPorArea_EmpleadoAsync',
                data: { año: $("#ausentismoAnio").html(), areaID: areaID },
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
            swal.fire("Oops!", "Error en el método getAusentismoAgrupadoPorArea_EmpleadoAsync \n" + ex, "error");
        }
    },

    getAusentismoAgrupadoPorProceso_EmpleadoAsync: function (procesoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetAusentismoAgrupadoPorProceso_EmpleadoAsync',
                data: { año: $("#ausentismoAnio").html(), procesoID: procesoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAusentismoAgrupadoPorProceso_EmpleadoAsync \n" + ex, "error");
        }
    },

    abrirVistaCargarArchivos: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/AbrirVistaCargarArchivos',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {
                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método abrirVistaCargarArchivos \n" + ex, "error");
        }
    },

    deleteAusentismoAsync: function (ausentismoID) {
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
                        url: rootHost + 'Ausentismo/DeleteAusentismoAsync',
                        data: { ausentismoID: ausentismoID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                ausentismoCRUD.getAllAusentismoAsync();
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else
                                swal.fire("¡Alerta!", "El registro se eliminó correctamente.", "warning");
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }                    });
                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteAusentismoAsync \n" + ex, "error");
        }
    },

    deleteProrrogaAsync: function (prorrogaID) {
        'use strict';
        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/DeleteProrrogaAsync',
                data: { prorrogaID: prorrogaID },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {
                        ausentismoCRUD.getProrrogasAsync(respuesta.ausentismoID);
                        ausentismoCRUD.getAusentismoByEditarAsync($("#ausentismoID").val());
                    } else
                        swal.fire("¡Alerta!", respuesta.error, "warning");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteAusentismoAsync \n" + ex, "error");
        }
    },

    getProrrogasAsync: function (ausentismoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/GetProrrogasAsync',
                data: { ausentismoID: ausentismoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetProrrogas').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getProrrogasAsync \n" + ex, "error");
        }
    },

    abrirVistaCargarArchivos: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Ausentismo/AbrirVistaCargarArchivos',
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

        } catch (ex) {
            swal.fire("Oops!", "Error en el método abrirVistaCargarArchivos \n" + ex, "error");
        }
    },
};

var elementosComunesAusentismoUsuarios = {

    index: function () {

        var btnModalArchivos = $("#btnModalArchivos");
        btnModalArchivos.click(function () {
            ausentismoCRUD.abrirVistaCargarArchivos();
        });

        var btnRegistrarAusentismo = $("#btnCrearAusentismo");
        btnRegistrarAusentismo.click(function () {
            ausentismoCRUD.registrarAusentismo();
        });

        var btnAusentismoPorAño = $("#btnAusentismoPorAño");
        btnAusentismoPorAño.click(function () {
            ausentismoCRUD.getAusentismoAgrupadoMensualmentePorAñoAsync(0);
        });

        var btnAusentismoPorArea = $("#btnAusentismoPorArea");
        btnAusentismoPorArea.click(function () {
            ausentismoCRUD.getAusentismoAgrupadoPorAreaAsync(0);
        });

        var btnAusentismoPorProceso = $("#btnAusentismoPorProceso");
        btnAusentismoPorProceso.click(function () {
            ausentismoCRUD.getAusentismoAgrupadoPorProcesoAsync(0);
        });


        var fechaActual = new Date();
        var mesActual = fechaActual.getMonth() + 1;
        $("#ausentismoMesEvento option[value='" + mesActual + "']").attr("selected", true);

        var fechaIncapacidad = $('#ausentismoFecha').daterangepicker({
            "autoUpdateInput": false,
            "locale": {
                "format": "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Guardar",
                "cancelLabel": "Cancelar",
                "fromLabel": "Desde",
                "toLabel": "Hasta",
                "customRangeLabel": "Personalizar",
                "daysOfWeek": [
                    "Do",
                    "Lu",
                    "Ma",
                    "Mi",
                    "Ju",
                    "Vi",
                    "Sa"
                ],
                "monthNames": [
                    "Enero",
                    "Febrero",
                    "Marzo",
                    "Abril",
                    "Mayo",
                    "Junio",
                    "Julio",
                    "Agosto",
                    "Setiembre",
                    "Octubre",
                    "Noviembre",
                    "Diciembre"
                ],
                "firstDay": 1
            },

            "opens": "center"
        },
            function (start, end, label) {
                $("#lblAusentismoDiasIncapacidad").html(end.diff(start, 'days') + 1);

            });
        $('#ausentismoFecha').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(`${picker.startDate.format('YYYY-MM-DD')} - ${picker.endDate.format('YYYY-MM-DD')}`);
            $("#lblAusentismoDiasIncapacidad").html(picker.endDate.diff(picker.startDate, 'days') + 1);
            ausentismoCRUD.calcularTiempoAusentismo();

            //    $("#diasIncapacidad").val(end.diff(start, 'days') + 1);
        });

        var ausentismoProrroga = $('#ausentismoProrroga').daterangepicker({
            "autoUpdateInput": false,
            "locale": {
                "format": "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Guardar",
                "cancelLabel": "Cancelar",
                "fromLabel": "Desde",
                "toLabel": "Hasta",
                "customRangeLabel": "Personalizar",
                "daysOfWeek": [
                    "Do",
                    "Lu",
                    "Ma",
                    "Mi",
                    "Ju",
                    "Vi",
                    "Sa"
                ],
                "monthNames": [
                    "Enero",
                    "Febrero",
                    "Marzo",
                    "Abril",
                    "Mayo",
                    "Junio",
                    "Julio",
                    "Agosto",
                    "Setiembre",
                    "Octubre",
                    "Noviembre",
                    "Diciembre"
                ],
                "firstDay": 1
            },

            "opens": "center"
        },
            function (start, end, label) {
                $("#diasIncapacidad").val(end.diff(start, 'days') + 1);

            });

        $('#ausentismoProrroga').on('apply.daterangepicker', function (ev, picker) {
            ausentismoCRUD.agregarProrrogaAsync(picker.startDate.format('YYYY-MM-DD'), picker.endDate.format('YYYY-MM-DD'));
            $(this).val(`${picker.startDate.format('YYYY-MM-DD')} - ${picker.endDate.format('YYYY-MM-DD')}`);
            ausentismoCRUD.getAusentismoByEditarAsync($("#ausentismoID").val());
        });

        $('#ausentismoProrroga').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });


        $("#ausentismoEmpleado").change(function () {
            $("#divGetDatosMonetareos").hide();
            ausentismoCRUD.getDatosEmpleadoAsync();
        });

        $("#ausentismoCodigoDiagnostico").keyup(function () {
            $("#ausentismoDescripcionDiagnostico").val("");
        });

        $("#ausentismoBtnBuscarCodigo").click(function (e) {
            e.preventDefault();
            var inputDiagnostico = $("#ausentismoCodigoDiagnostico");
            ausentismoCRUD.buscarDiagnosticoAsync(inputDiagnostico);
        });

    },

}