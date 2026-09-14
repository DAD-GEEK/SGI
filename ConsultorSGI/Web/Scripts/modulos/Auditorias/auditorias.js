var auditoriasCRUD = {

    index: function () {

        $('#auditoriaFechaVersion').daterangepicker({
            "autoUpdateInput": false,
            "singleDatePicker": true,
            "showDropdowns": true,
            "minYear": 2000,
            "maxYear": parseInt(moment().format('YYYY'), 10),
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
            },

            "opens": "center"
        });

        $('#auditoriaFechaVersion').on('apply.daterangepicker', function (ev, picker) {

            $(this).val(`${picker.startDate.format('YYYY-MM-DD')}`);
        });

        $('#auditoriaFechaVersion').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });

        $('#auditoriaFecha').daterangepicker({
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

            });

        $('#auditoriaFecha').on('apply.daterangepicker', function (ev, picker) {

            $("#auditoriaDuracion").removeClass('d-none');
            $("#auditoriaDuracion").html("Duración estimada de la auditoria: " + parseInt(picker.endDate.diff(picker.startDate, 'days') + 1) + " dias.");

            $(this).val(`${picker.startDate.format('YYYY-MM-DD')} - ${picker.endDate.format('YYYY-MM-DD')}`);
        });

        $('#auditoriaFecha').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });

    },

    //Vistas
    getAllAuditoriasAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditorias/GetAllAuditoriasAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAuditorias').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllAuditoriasAsync \n" + ex, "error");
        }
    },

    crearPlanAuditoria: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditorias/CrearPlanAuditoria',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAuditorias').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearPlanAuditoria \n" + ex, "error");
        }
    },

    obtenerComplementosPorTercero: function (clienteTerceroID, auditoriaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditorias/ObtenerComplementosPorTerceroAsync',
                data: { clienteTerceroID: clienteTerceroID, auditoriaID: auditoriaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divAuditoriaComplementosTercero').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerComplementosPorTercero \n" + ex, "error");
        }
    },

    getAuditoriaByEditarAsync: function (auditoriaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditorias/GetAuditoriaByEditarAsync',
                data: { auditoriaID: auditoriaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    var esListaDeVerificacion = $("#esListasDeVerificacion").is(':checked');

                    $('#divGetAuditorias').empty().html(respuesta);

                    if (esListaDeVerificacion) {

                        $("#tabPlanDeAuditoria").removeClass('active');
                        $("#planDeAuditoria").removeClass('active');

                        $("#tabProgramacion").addClass('active');
                        $("#programacion").addClass('active');
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAuditoriaByEditarAsync \n" + ex, "error");
        }
    },

    crearListaDeVerificacion: function (auditoriaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditorias/CrearListaDeVerificacion',
                data: { auditoriaID: auditoriaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAuditorias').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearListaDeVerificacion \n" + ex, "error");
        }
    },

    obtenerInformesDeAuditoria: function (auditoriaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditorias/ObtenerInformesDeAuditoria',
                data: { auditoriaID: auditoriaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAuditorias').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerInformesDeAuditoria \n" + ex, "error");
        }
    },

    validarFechasDeAuditoriaDentroDeRango: function (fechaInicial, fechaFinal) {
        'use strict'

        var fechaAuditoria = $("#auditoriaFecha").val();
        fechaAuditoria = fechaAuditoria.split(" ");
        var fechaInicialAuditoria = moment(fechaAuditoria[0]).format("YYYY-MM-DD HH:mm a");
        var fechaFinalAuditoria = moment(fechaAuditoria[2]).add(23, 'h').add(59, 'm').format("YYYY-MM-DD HH:mm a");

        if (fechaInicial < fechaInicialAuditoria)
            return false;

        if (fechaInicial > fechaFinalAuditoria)
            return false;

        if (fechaFinal > fechaFinalAuditoria)
            return false;

        if (fechaFinal < fechaInicialAuditoria)
            return false;

        return true;
    },

    getAllProcesosAuditoriaAsync: function () {
        'use strict';

        try {

            var terceroClienteID = $("#auditoriaTercero").val();
            var auditoriaID = $("#auditoriaID").val();

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditorias/GetAllProcesosAuditoriaAsync',
                data: { terceroClienteID: terceroClienteID, auditoriaID: auditoriaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetProcesos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllProcesosAuditoriaAsync \n" + ex, "error");
        }
    },

    seleccionarProcesoParaAuditar: function (procesoID) {
        'use strict';

        try {

            var auditoriaID = $("#auditoriaID").val();

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditorias/SeleccionarProcesoParaAuditar',
                data: { auditoriaID: auditoriaID, procesoID: procesoID },
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {
                        toastr.success("Proceso seleccionado correctamente.");
                        auditoriasCRUD.getAllProcesosAuditoriaAsync();
                    } else
                        toastr.error(respuesta.error);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método seleccionarProcesoParaAuditar \n" + ex, "error");
        }
    },


    //Base de datos  
    guardarAuditoriaAsync: function () {
        'use strict';

        $("#formGuardarAuditoria").submit(function (e) {

            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var $btn = $(document.activeElement);
                    var isDetalle = $btn.is('[name]');

                    var fechaAuditoria = $("#auditoriaFecha").val();
                    fechaAuditoria = fechaAuditoria.split(" ");
                    var fechaInicial = fechaAuditoria[0];
                    var fechaFinal = fechaAuditoria[2];

                    var modelo = {

                        "IntAuditoriaID": $("#auditoriaID").val(),
                        "IntTerceroClienteID": $("#auditoriaTercero").val(),
                        "DatFechaInicial": fechaInicial,
                        "DatFechaFinal": fechaFinal,
                        "StrCodigo": $("#auditoriaCodigo").val(),
                        "StrVersion": $("#auditoriaVersion").val(),
                        "DatFechaVersion": $("#auditoriaFechaVersion").val(),
                        "StrObjetivo": $("#auditoriaObjetivo").val(),
                        "StrAlcance": $("#auditoriaAlcance").val(),
                        "StrUsuarioFirma": $("#auditoriaAuditorPrincipal").val(),
                        "StrIntegranteCopasst": $("#auditoriaIntegranteCopasst").val(),
                        "StrRepresentanteLegal": $("#auditoriaRepresentanteLegal").val(),
                        "Auditorias_Normas": null,
                        "Auditorias_Procesos": null,
                        "BitEstado": $("#auditoriaEstado").is(":checked"),
                        "DatFechaElaboracionInforme": $("#auditoriaFechaInforme").val(),
                    };

                    //Normas
                    var auditoriaNormas = $("#auditoriaNormas").val();
                    var listaNormas = [];
                    var i = 0;

                    $.each(auditoriaNormas, (index, value) => {

                        var item = {
                            IntNormaID: value,
                            IntAuditoriaID: $("#auditoriaID").val()
                        }

                        listaNormas[i] = item;
                        i++;
                    });
                    modelo.Auditorias_Normas = listaNormas;

                    //Procesos
                    var auditoriaProcesos = $("#auditoriaProcesos").val();
                    var listaProcesos = [];
                    var i = 0;

                    $.each(auditoriaProcesos, (index, value) => {

                        var item = {
                            IntProcesoID: value,
                            IntAuditoriaID: $("#auditoriaID").val()
                        }

                        listaProcesos[i] = item;
                        i++;
                    });

                    modelo.Auditorias_Procesos = listaProcesos;

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Auditorias/GuardarAuditoriaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                aplicar = true;

                                if (isDetalle != true) {

                                    if (aplicar) {
                                        toastr.success('El registro se guardó correctamente.');
                                        return auditoriasCRUD.getAuditoriaByEditarAsync(respuesta.auditoriaID);
                                    }

                                    swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                    auditoriasCRUD.getAllAuditoriasAsync();
                                } else {
                                    $("#auditoriaID").val(respuesta.auditoriaID);
                                    form.parsley().destroy();
                                    auditoriaDetalleCRUD.crearPlanAuditoriaDetalle(respuesta.auditoriaID);
                                }

                            } else {
                                form.parsley().destroy();
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }

                        },
                        error: function (respuesta) {
                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearAuditoriaAsync \n" + ex, "error");
            }
        });
    },

    guardarInformeAsync: function () {
        'use strict';

        $("#btnGuardarInforme").click(function (e) {

            try {

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntAuditoriaID": $("#auditoriaID").val(),
                        "StrFortalezas": $("#auditoriaFortalezas").val(),
                        "StrConclusiones": $("#auditoriaConclusiones").val(),
                        "BitEstado": $("#auditoriaEstado").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Auditorias/GuardarInformeDeAuditoriaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success")
                                swal.fire("¡Notificación!", "El informe se guardó correctamente.", "success");
                            else
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                        },
                        error: function (respuesta) {
                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método guardarInformeAsync \n" + ex, "error");
            }
        });
    },

    deleteAuditoriaAsync: function (auditoriaID) {
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
                        url: rootHost + 'Auditorias/DeleteAuditoriaAsync',
                        data: { auditoriaID: auditoriaID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            auditoriasCRUD.getAllAuditoriasAsync();

                        },
                        error: function (respuesta) {

                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteAuditoriaAsync \n" + ex, "error");
        }
    },

    notificacionesEnTiempoReal_InformesAuditoria: function () {

        var notificacion = $.connection.auditoriaHub;

        var auditoriaActualID = $("#auditoriaID").val();
        var nombreUsuario = $("#usuarioActualNombre").val();
        var textFortalezas = $("#mensajeNotificacionFortalezas");
        var textConclusiones = $("#mensajeNotificacionConclusiones");
        var fortalezas = $("#auditoriaFortalezas");
        var conclusiones = $("#auditoriaConclusiones");
        var estadoAuditoria = $("#auditoriaEstado");

        notificacion.client.sendNotificationFortalezas = function (auditoriaID, name, text, finalice) {

            if (finalice) {
                textFortalezas.html("");
            } else {
                if (nombreUsuario != name && finalice == false && auditoriaID == auditoriaActualID) {
                    textFortalezas.html(`${name} ESTÁ REALIZANDO CAMBIOS EN ESTE ELEMENTO...`);
                    fortalezas.val(text);
                } else {
                    textFortalezas.html("");
                }
            }

        };

        notificacion.client.sendNotificationConclusiones = function (auditoriaID, name, text, finalice) {

            if (finalice) {
                textConclusiones.html("");
            } else {
                if (nombreUsuario != name && finalice == false && auditoriaID == auditoriaActualID) {
                    textConclusiones.html(`${name} ESTÁ REALIZANDO CAMBIOS EN ESTE ELEMENTO...`);
                    conclusiones.val(text);
                } else {
                    textConclusiones.html("");
                }
            }

        };

        notificacion.client.changeEstateAuditoria = function (auditoriaID, estado) {

            if (auditoriaID == auditoriaActualID)
                estadoAuditoria.prop('checked', estado);
        };

        $.connection.hub.start().done(function () {

            var name = nombreUsuario;

            fortalezas.keyup(function () {

                var text = fortalezas.val();
                notificacion.server.notificarCambiosEnFortalezas(auditoriaActualID, name, text, false);

            });

            conclusiones.keyup(function () {

                var text = conclusiones.val();
                notificacion.server.notificarCambiosEnConclusiones(auditoriaActualID, name, text, false);

            });

            $("#btnGuardarInforme").click(function () {

                notificacion.server.notificarCambiosEnFortalezas(auditoriaActualID, "", "", true);
                notificacion.server.notificarCambiosEnConclusiones(auditoriaActualID, "", "", true);

            });

            estadoAuditoria.change(function () {

                var estadoActual = $("#auditoriaEstado").is(':checked');
                notificacion.server.cambiarEstadoAuditoria(auditoriaActualID, estadoActual);

            });
        });

    },

};
