var listasDeVerificacionCRUD = {

    obtenerAuditoriaDetalleAsync: function (auditoriaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/ObtenerAuditoriaDetalleAsync',
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
            swal.fire("Oops!", "Error en el método obtenerAuditoriaDetalleAsync \n" + ex, "error");
        }
    },

    obtenerListaDeVerificacionEncabezadoAsync: function (auditoriaDetalleID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/ObtenerListaDeVerificacionEncabezadoAsync',
                data: { auditoriaDetalleID: auditoriaDetalleID },
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
            swal.fire("Oops!", "Error en el método obtenerListaDeVerificacionEncabezadoAsync \n" + ex, "error");
        }
    },

    obtenerListaDeVerificacionDetalleAsync: function (auditoriaDetalleID, isVistaInforme) {
        'use strict';

        if (isVistaInforme != true)
            isVistaInforme = false;

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/ObtenerListaDeVerificacionDetalleAsync',
                data: { auditoriaDetalleID: auditoriaDetalleID, isVistaInforme: isVistaInforme },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divListaDeVerificacionDetalle').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerListaDeVerificacionDetalleAsync \n" + ex, "error");
        }
    },

    vistaCargarListaDeVerificacionDeProcesoActualAsync: function (auditoriaDetalleID) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'ListasDeVerificacion/VistaCargarListaDeVerificacionDeProcesoActualAsync',
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
            swal.fire("Oops!", "Error en el método vistaCargarListaDeVerificacionDeProcesoActualAsync \n" + ex, "error");
        }
    },

    copiarPlantillaEnListaDeVerificacionDeProcesoActualAsync: function (plantillaDetalleID) {
        'use strict';

        try {

            var auditoriaDetalleID = $("#auditoriaDetalleID").val();

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/CopiarPlantillaEnListaDeVerificacionDeProcesoActualAsync',
                data: { auditoriaDetalleID: auditoriaDetalleID, plantillaDetalleID: plantillaDetalleID },
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                        var auditoriaDetalleID = $("#auditoriaDetalleID").val();
                        listasDeVerificacionCRUD.obtenerListaDeVerificacionDetalleAsync(auditoriaDetalleID);

                        swal.fire("¡Notificación!", "Lista de verificación actualizada exitosamente", "success");
                    } else
                        swal.fire("¡Alerta!", respuesta.error, "warning");

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método copiarPlantillaEnListaDeVerificacionDeProcesoActualAsync \n" + ex, "error");
        }
    },

    auditarListaDeVerificacion: function (listaDeVerificacionID, isVistaInforme) {
        'use strict';

        if (isVistaInforme != true)
            isVistaInforme = false;

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/AuditarListaDeVerificacion',
                data: { listaDeVerificacionID: listaDeVerificacionID, isVistaInforme: isVistaInforme },
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
            swal.fire("Oops!", "Error en el método auditarListaDeVerificacion \n" + ex, "error");
        }
    },

    crearItemListaDeVerificacion: function (procesoID, auditoriaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/CrearItemListaDeVerificacion',
                data: { procesoID: procesoID, auditoriaID: auditoriaID },
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
            swal.fire("Oops!", "Error en el método crearItemListaDeVerificacion \n" + ex, "error");
        }
    },

    cargarPlantillaListaDeVerificacion: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/CargarPlantillaListaDeVerificacion',
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
            swal.fire("Oops!", "Error en el método cargarPlantillaListaDeVerificacion \n" + ex, "error");
        }
    },

    obtenerItemListaDeVerificacionParaEditar: function (listaVerificacionID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/ObtenerItemListaDeVerificacionParaEditar',
                data: { listaVerificacionID: listaVerificacionID },
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
            swal.fire("Oops!", "Error en el método obtenerItemListaDeVerificacionParaEditar \n" + ex, "error");
        }
    },

    guardarListaVerificacionItemAsync: function () {
        'use strict';

        $(".btnGuardarRegistroModal").click(function (e) {
            try {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formGuardarGlobalItem");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var procesoID = $("#procesoID").val();

                    var modelo = {
                        "IntListaVerificacionID": $("#listaDeVerificacionID").val(),
                        "IntAuditoriaDetalleID": $("#auditoriaDetalleID").val(),
                        "StrTitulo": $("#listaDeVerificacionTitulo").val(),
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'ListasDeVerificacion/GuardarListasDeVerificacionItemAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, procesoID: procesoID },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {

                                    toastr.success('El registro se guardó correctamente.');
                                    listasDeVerificacionCRUD.obtenerListaDeVerificacionDetalleAsync($("#auditoriaDetalleID").val());
                                    return listasDeVerificacionCRUD.obtenerItemListaDeVerificacionParaEditar(respuesta.listaDeVerificacionID);
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                $('#modal').modal('hide');
                                $('#contenedor').empty();
                                listasDeVerificacionCRUD.obtenerListaDeVerificacionDetalleAsync($("#auditoriaDetalleID").val());

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

                swal.fire("Oops!", "Error en el método guardarListaVerificacionItem \n" + ex, "error");
            }
        });
    },

    cargarPlantillaListaDeVerificacionAsync: function () {
        'use strict';

        $("#formCargarListaDeVerificacion").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var sobreEscribirDatos = false;

                    swal.fire({
                        title: "¡Sobreescribir!",
                        text: "¿Desea eliminar la lista de verificación actual?",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonText: "SI",
                        cancelButtonText: "NO",
                    }).then(function (resultado) {

                        if (resultado.isConfirmed) {
                            sobreEscribirDatos = true;
                        }

                        var modelo = {
                            "IntPlantillaID": $("#cargarPlantillaPlantilla").val(),
                            "IntAuditoriaDetalleID": $("#auditoriaDetalleID").val(),
                            "BitSobreEscribirDatos": sobreEscribirDatos
                        };

                        var token = $('input[name="__RequestVerificationToken"]').val();

                        $.ajax({
                            type: "POST",
                            url: rootHost + 'ListasDeVerificacion/CargarPlantillaListaDeVerificacionAsync',
                            data: { __RequestVerificationToken: token, modelo: modelo },
                            dataType: 'JSON',
                            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                            async: true,
                            success: function (respuesta) {

                                if (respuesta.msn === "success") {

                                    swal.fire("¡Notificación!", "La plantilla se cargó correctamente.", "success");
                                    $('#modal').modal('hide');
                                    $('#contenedor').empty();
                                    listasDeVerificacionCRUD.obtenerListaDeVerificacionDetalleAsync($("#auditoriaDetalleID").val());

                                } else {
                                    form.parsley().destroy();
                                    swal.fire("¡Alerta!", respuesta.error, "warning");
                                }

                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                            }
                        });

                    });


                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método cargarPlantillaListaDeVerificacionAsync \n" + ex, "error");
            }
        });
    },

    guardarAuditoriaPorItemAsync: function (isVistaInforme) {
        'use strict';

        if (!isVistaInforme)
            isVistaInforme = false;

        $(".btnGuardarRegistroModal").click(function (e) {
            try {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formGuardarAuditarItem");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var fechaAuditoria = $("#auditoriaDetalleFechaAudita").val();
                    var horaAuditoria = $("#auditoriaDetalleFechaAuditaHora").val();
                    var fechaCompleta = moment(fechaAuditoria + ' ' + horaAuditoria, "YYYY-MM-DD hh:mm a").format("YYYY-MM-DD hh:mm a");

                    var modelo = {

                        "IntListaVerificacionID": $("#listaDeVerificacionID").val(),
                        "StrHallazgo": $("#auditarItemHallazgo").val(),
                        "BitNoConformidad": $("#listaNoConformidad").is(':checked'),
                        "BitConformidad": $("#listaConformidad").is(':checked'),
                        "BitObservacion": $("#listaObservacion").is(':checked'),
                        "DatFechaEnQueAudita": fechaCompleta,
                        "ListasDeVerificacion_Requisitos": null
                    };

                    var auditoriaNormas = $(".auditarRequisitos");
                    var listaRequisitosSeleccionados = [];
                    var i = 0;

                    $(".auditarRequisitos:checked").each(function (index, check) {

                        var numeralID = $(check).attr('id');
                        var item = {
                            IntListaDeVerificacionID: $("#listaDeVerificacionID").val(),
                            IntNumeralID: numeralID
                        }

                        listaRequisitosSeleccionados[i] = item;
                        i++;

                    });

                    modelo.ListasDeVerificacion_Requisitos = listaRequisitosSeleccionados;

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'ListasDeVerificacion/GuardarAuditoriaPorItemAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, isVistaInforme: isVistaInforme },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {

                                    toastr.success('El registro se guardó correctamente.');
                                    listasDeVerificacionCRUD.obtenerListaDeVerificacionDetalleAsync($("#auditoriaDetalleID").val(), isVistaInforme);
                                    return listasDeVerificacionCRUD.auditarListaDeVerificacion(respuesta.listaVerificacionID, isVistaInforme);
                                }

                                listasDeVerificacionCRUD.obtenerListaDeVerificacionDetalleAsync($("#auditoriaDetalleID").val(), isVistaInforme);

                                if (isVistaInforme)
                                    listasDeVerificacionCRUD.obtenerNoConformidadesAuditoriaAsync(respuesta.auditoriaID, isVistaInforme);

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
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

                swal.fire("Oops!", "Error en el método guardarAuditoriaPorItemAsync \n" + ex, "error");
            }
        });
    },

    deleteListaDeVerificacionAsync: function (listaDeVerificacionID) {
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
                        url: rootHost + 'ListasDeVerificacion/DeleteItemListaDeVerificacionAsync',
                        data: { listaDeVerificacionID: listaDeVerificacionID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                listasDeVerificacionCRUD.obtenerListaDeVerificacionDetalleAsync($("#auditoriaDetalleID").val());

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteListaDeVerificacionAsync \n" + ex, "error");
        }
    },

    obtenerNoConformidadesAuditoriaAsync: function (auditoriaID, isVistaInforme) {
        'use strict';

        if (isVistaInforme != true)
            isVistaInforme = false;

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/ObtenerNoConformidadesAuditoriaAsync',
                data: { auditoriaID: auditoriaID, isVistaInforme: isVistaInforme },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divNoConformidades').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerNoConformidadesAuditoriaAsync \n" + ex, "error");
        }
    },

    vistaObtenerNumeralesDeListaDeVerificacionAsync: function (listaDeVerificacionID, normaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/VistaObtenerNumeralesDeListaDeVerificacionAsync',
                data: { listaDeVerificacionID: listaDeVerificacionID, normaID: normaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetListasDeVerificacionNumerales').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método vistaObtenerNumeralesDeListaDeVerificacionAsync \n" + ex, "error");
        }
    },

    vistaDropDownNumeralesPorNormaAsync: function (normaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/VistaDropDownNumeralesPorListaDeVerificacionAsync',
                data: { normaID: normaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divNumerales').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método vistaDropDownNumeralesPorNormaAsync \n" + ex, "error");
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

                    var listaDeVerificacionID = $("#listaDeVerificacionID").val();

                    if (listaDeVerificacionID == 0)
                        return swal.fire("¡Alerta!", "Para agregar un numeral primero debe guardar los cambios.", "warning");

                    var modelo = {

                        "IntListaVerificacionID": $("#listaDeVerificacionID").val(),
                        "IntNumeralID": $("#listaDeVerificacionNumerales").val(),
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'ListasDeVerificacion/GuardarListaDeVerificacionNumeralAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var normaID = $("#listaDeVerificacionNormas").val();
                                if (normaID == "")
                                    normaID = 0;

                                listasDeVerificacionCRUD.vistaObtenerNumeralesDeListaDeVerificacionAsync($("#listaDeVerificacionID").val(), normaID);

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

    deleteListaVerificacionNumeralAsync: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ListasDeVerificacion/DeleteListaVerificacionNumeralAsync',
                data: { registroID: registroID },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                    } else
                        swal.fire("¡Alerta!", respuesta.error, "warning");

                    var normaID = 0;
                    listasDeVerificacionCRUD.vistaObtenerNumeralesDeListaDeVerificacionAsync($("#listaDeVerificacionID").val(), normaID);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteListaVerificacionNumeralAsync \n" + ex, "error");
        }
    },

}