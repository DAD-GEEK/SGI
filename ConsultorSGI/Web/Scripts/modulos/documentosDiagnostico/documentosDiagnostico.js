var documentosDiagnosticoCRUD = {

    index: function () {

        var bloquearDocumento = $("#documentoDiganosticoFinalizado").is(":checked");

        console.log(bloquearDocumento);

        if (bloquearDocumento)
            $(".bloquear").attr("disabled", true);
    },

    //Vistas
    getAllPorSistemaDeGestion: function () {
        'use strict';

        try {

            var sistemaDeGestion = $("#sistemaDeGestion").val();

            $.ajax({
                type: "GET",
                url: rootHost + 'DocumentosDiagnostico/GetAllPorSistemaDeGestion',
                data: { sistemaDeGestion: sistemaDeGestion },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divDocumentosDiagnostico').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllPorSistemaDeGestion \n" + ex, "error");
        }
    },

    abrirVistaCrear: function () {
        'use strict';

        try {

            $("#btnCrearDocumento").click(function () {

                $.ajax({
                    type: "GET",
                    url: rootHost + 'DocumentosDiagnostico/AbrirVistaCrear',
                    data: {},
                    dataType: "html",
                    async: true,
                    success: function (respuesta) {

                        $('#contenedor').html(respuesta);
                        $("#modal").modal('show');
                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                        funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                    }
                });

            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método abrirVistaCrear \n" + ex, "error");
        }
    },

    abrirVistaParaDiligenciarDocumento: function (documentoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'DocumentosDiagnostico/AbrirVistaParaDiligenciar',
                data: { documentoID: documentoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divDocumentosDiagnostico').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método abrirVistaParaDiligenciarDocumento \n" + ex, "error");
        }
    },

    abrirVistaInformacionEmpresa: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'DocumentosDiagnostico/AbrirVistaInformacionEmpresa',
                data: { terceroID: terceroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divInformacionEmpresa').html(respuesta);
                },

                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método abrirVistaInformacionEmpresa \n" + ex, "error");
        }
    },

    obtenerConsolidadoDeResultadosGlobales: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                global: false,
                url: rootHost + 'DocumentosDiagnostico/ObtenerConsolidadoDeResultadosGlobales',
                data: { documentoID: $("#documentoDiganosticoID").val() },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $("#consolidado").empty().html(respuesta);


                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerConsolidadoDeResultadosGlobales \n" + ex, "error");
        }
    },


    //Base de datos
    crearDocumento: function () {
        'use strict';

        $("#frmCrearDocumento").submit(function (e) {

            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "DatFechaElaboracion": $("#documentoFecha").val(),
                        "StrSistemaDeGestionID": $("#sistemaDeGestion").val()

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'DocumentosDiagnostico/CrearDocumento',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $("#modal").modal('hide');
                                swal.fire("¡Notificación!", "Documento generado correctamente. \nAhora puede proceder a diligenciarlo.", "success");
                                documentosDiagnosticoCRUD.getAllPorSistemaDeGestion();

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

                swal.fire("Oops!", "Error en el método crearDocumento \n" + ex, "error");
            }
        });
    },

    delete: function (registroID) {
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
                        url: rootHost + 'DocumentosDiagnostico/Delete',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                documentosDiagnosticoCRUD.getAllPorSistemaDeGestion($("#sistemaDeGestionID").val());
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método delete \n" + ex, "error");
        }
    },

    agregarElementosPendientes: function () {

        try {

            var modelo = {

                "StrDocumentoID": $("#documentoDiganosticoID").val(),
                "PasosGenericosPendientesDeAsignar": null,
                "CriteriosGenericosPendientesDeAsignar": null,
            };

            //Pasos
            var elementosPasosPendientes = $(".pasosPendientes");
            var listaPasosSeleccionados = [];
            var i = 0;

            $.each(elementosPasosPendientes, (index, value) => {

                var isChecked = $(value).is(":checked");


                if (isChecked) {

                    var item = {
                        StrPasoID: $(value).attr("id")
                    }

                    listaPasosSeleccionados[i] = item;
                    i++;
                }
            });

            var elementosCriteriosPendientes = $(".criteriosPendientes");
            var listaCriteriosSeleccionados = [];
            var i = 0;
            $.each(elementosCriteriosPendientes, (index, value) => {

                var isChecked = $(value).is(":checked");


                if (isChecked) {

                    var item = {
                        StrCriterioID: $(value).attr("id")
                    }

                    listaCriteriosSeleccionados[i] = item;
                    i++;
                }
            });

            modelo.PasosGenericosPendientesDeAsignar = listaPasosSeleccionados;
            modelo.CriteriosGenericosPendientesDeAsignar = listaCriteriosSeleccionados;

            $.ajax({
                type: "POST",
                url: rootHost + 'DocumentosDiagnostico/AgregarElementosPendientes',
                data: { modelo: modelo },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                        $("#modal2").modal('hide');
                        swal.fire("¡Notificación!", "Los elementos se agregaron exitosamente al documento.", "success");
                        documentosDiagnosticoCRUD.abrirVistaParaDiligenciarDocumento($("#documentoDiganosticoID").val());

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

            swal.fire("Oops!", "Error en el método crearDocumento \n" + ex, "error");
        }
    },

    finalizarDocumento: function () {
        'use strict';
        try {

            $("#documentoFinalizar").click(function () {

                swal.fire({
                    title: "¡Finalizar documento!",
                    text: "¿Está seguro que desea dar por finalizado el documento diagnóstico?\nUna vez hecho esto no podrá modificarlo. ",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: "SI",
                    cancelButtonText: "NO",
                }).then(function (resultado) {

                    if (resultado.isConfirmed) {

                        var documento = {
                            "StrDocumentoID": $("#documentoDiganosticoID").val(),
                        }

                        var token = $('input[name="__RequestVerificationToken"]').val();


                        $.ajax({
                            type: "POST",
                            url: rootHost + 'DocumentosDiagnostico/FinalizarDocumento',
                            data: { __RequestVerificationToken: token, documento: documento },
                            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                            dataType: "JSON",
                            async: true,
                            success: function (respuesta) {

                                if (respuesta.msn === "success") {

                                    documentosDiagnosticoCRUD.getAllPorSistemaDeGestion($("#sistemaDeGestionID").val());
                                    swal.fire("¡Notificación!", "El registro se actualizó correctamente.", "success");
                                } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                            }
                        });

                    }

                });

            });



        } catch (ex) {

            swal("Oops!", "Error en el método delete \n" + ex, "error");
        }
    },


    //Stepper pasos
    configuracionStepperPasos: function (event) {

        var faseActual = event.detail.from;
        var faseSiguiente = event.detail.to;

        if (faseSiguiente > faseActual) {

            var faltanFormulariosPorValidar = false;
            var numeroDePaso = 0;
            var listaPasosPendientes = [];
            var contadorPasosPendientes = 0;
            $(".criteriosPorPaso > form").each(function (i, elemento) {

                var faseActualSeleccionada = String(parseInt(faseActual) + 1);
                var faseFormulario = $(elemento).data('fasedescripcion');

                if (faseFormulario.indexOf(faseActualSeleccionada) !== -1) {
                    var formularioID = $(elemento).attr('id');
                    var form = $("#" + formularioID);
                    form.parsley().validate();

                    if (!form.parsley().isValid()) {
                        faltanFormulariosPorValidar = true;
                        numeroDePaso = $(elemento).data("pasonumero");
                        listaPasosPendientes[contadorPasosPendientes] = numeroDePaso;
                        contadorPasosPendientes++;
                    }
                }

            });

            if (faltanFormulariosPorValidar) {
                var listaPasosPendientesAgrupados = $.unique(listaPasosPendientes);

                if (listaPasosPendientesAgrupados.length == 1)
                    swal.fire("¡Criterios pendientes!", "Hay criterios pendientes por calificar en el paso N°" + listaPasosPendientesAgrupados.join("") + ".", "warning");
                else
                    swal.fire("¡Criterios pendientes!", "Hay criterios pendientes por calificar en los pasos N°" + listaPasosPendientesAgrupados.join(", N°") + ".", "warning");

                event.preventDefault();
            }
        }
    },


};
