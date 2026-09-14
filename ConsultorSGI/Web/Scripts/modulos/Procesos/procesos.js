var procesosCRUD = {

    //Vistas
    getAllProcesosAsync: function (terceroID, esVistaGlobal) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/GetAllProcesosAsync',
                data: { terceroID: terceroID, esVistaGlobal: esVistaGlobal },
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
            swal.fire("Oops!", "Error en el método getAllProcesosAsync \n" + ex, "error");
        }
    },

    vistaObtenerNumeralesPorProcesoAsync: function (procesoID, normaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/VistaObtenerNumeralesPorProcesoAsync',
                data: { procesoID: procesoID, normaID: normaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetProcesosNumerales').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método vistaObtenerNumeralesPorProcesoAsync \n" + ex, "error");
        }
    },

    vistaDropDownNumeralesPorNormaAsync: function (normaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/VistaDropDownNumeralesPorNormaAsync',
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

    vistaAgregarNumeralesModalAsync: function (procesoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/VistaAgregarNumeralesModalAsync',
                data: { procesoID: procesoID },
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
            swal.fire("Oops!", "Error en el método vistaAgregarNumeralesModalAsync \n" + ex, "error");
        }
    },

    vistaSeleccionarProcesosAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/VistaSeleccionarProcesosAsync',
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
            swal.fire("Oops!", "Error en el método vistaSeleccionarProcesosAsync \n" + ex, "error");
        }
    },

    vistaCargarDatosGenericosAsync: function (procesoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/VistaCargarDatosGenericosAsync',
                data: { procesoID: procesoID },
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
            swal.fire("Oops!", "Error en el método vistaCargarDatosGenericosAsync \n" + ex, "error");
        }
    },

    crearProceso: function (esVistaGlobal) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/CrearProceso',
                data: { esVistaGlobal: esVistaGlobal },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    if (esVistaGlobal) {
                        $('#divGetProcesos').empty().html(respuesta);
                        return;
                    }

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearProceso \n" + ex, "error");
        }
    },

    getProcesoByEditarAsync: function (procesoID, esVistaGlobal, esVistaListaVerificacion) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

        if (!esVistaListaVerificacion)
            esVistaListaVerificacion = false;

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/GetProcesoByEditarAsync',
                data: { procesoID: procesoID, esVistaGlobal: esVistaGlobal },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    if (esVistaGlobal) {
                        $('#divGetProcesos').empty().html(respuesta);

                        var pestanaPrincial = 'labelProceso';
                        var pestanaSecundaria = 'labelListasVerificacion';

                        var contenedorSecundario = 'listasVerificacion';
                        var contenedorPrincipal = 'proceso';

                        if (esVistaListaVerificacion)
                            funcionesGlobales.seleccionarPestanaSecundariaEnTab(pestanaPrincial, contenedorPrincipal, pestanaSecundaria, contenedorSecundario);

                        return;
                    }

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');

                    var pestanaPrincial = 'labelProceso';
                    var pestanaSecundaria = 'labelListasVerificacion';

                    var contenedorSecundario = 'listasVerificacion';
                    var contenedorPrincipal = 'proceso';

                    if (esVistaListaVerificacion)
                        funcionesGlobales.seleccionarPestanaSecundariaEnTab(pestanaPrincial, contenedorPrincipal, pestanaSecundaria, contenedorSecundario);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getProcesoByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos
    guardarProcesoAsync: function (esVistaGlobal) {
        'use strict';

        if (!esVistaGlobal)
            esVistaGlobal = false;

        $(".btnGuardarRegistro,.btnGuardarProceso").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");
                var agregarListaVerificacion = $(this).data("agregarlista");

                e.preventDefault();

                var form = $("#formGuardarGlobalPR");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var terceroClienteID = $("#terceroClienteID").val();
                    var procesoOrigen = $("#procesoOrigen").val();

                    if (esVistaGlobal) {
                        terceroClienteID = 0;
                        procesoOrigen = 0;
                    }

                    var modelo = {

                        "IntProcesoID": $("#procesoID").val(),
                        "StrCodigo": $("#procesoCodigo").val(),
                        "StrDescripcion": $("#procesoDescripcion").val(),
                        "IntMacroProceso": $("#terceroMacroProceso").val(),
                        "IntTerceroClienteID": terceroClienteID,
                        "IntProcesoOrigen": procesoOrigen,
                        "BitActivo": $("#procesoActivo").is(":checked"),
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Procesos/GuardarProcesoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var esAuditoria = $("#auditoriaVerificacion").is(":checked");

                                if (esAuditoria) {
                                    auditoriaDetalleCRUD.getAllDetallePlanAuditoriasAsync($("#auditoriaID").val());
                                    auditoriasCRUD.getAllProcesosAuditoriaAsync();
                                }
                                else
                                    procesosCRUD.getAllProcesosAsync(respuesta.terceroID, esVistaGlobal);

                                var procesoNombre = $("#procesoDescripcion").val();

                                if (esVistaGlobal) {

                                    if (aplicar) {
                                        toastr.success('El registro se guardó correctamente.');

                                        if (agregarListaVerificacion)
                                            return plantillaListasDeVerificacionDetalleCRUD.crearPlantillaListaDeVerificacionDetalle(respuesta.procesoID, procesoNombre, esVistaGlobal);

                                        return procesosCRUD.getProcesoByEditarAsync(respuesta.procesoID, esVistaGlobal);
                                    }

                                    return swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                }


                                if (aplicar) {
                                    toastr.success('El registro se guardó correctamente.');
                                    return procesosCRUD.getProcesoByEditarAsync(respuesta.procesoID);
                                }

                                $('#modal').modal('hide');
                                $('#contenedor').empty();
                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");


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

                swal.fire("Oops!", "Error en el método guardarProcesoAsync \n" + ex, "error");
            }
        });
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

                        "IntProcesoID": $("#procesoID").val(),
                        "IntNumeralID": $("#procesoNumerales").val(),
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Procesos/GuardarProcesoNumeralAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var normaID = $("#procesoNormas").val();
                                if (normaID == "")
                                    normaID = 0;

                                procesosCRUD.vistaObtenerNumeralesPorProcesoAsync($("#procesoID").val(), normaID);

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

    seleccionarProcesoGlobalAsync: function (procesoID) {
        'use strict';

        try {

            var modelo = {

                "IntProcesoID": procesoID,
                "IntTerceroClienteID": $("#terceroClienteID").val(),
            };

            var token = $('input[name="__RequestVerificationToken"]').val();

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/SeleccionarProcesoGlobalAsync',
                data: { __RequestVerificationToken: token, modelo: modelo },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                        var esAuditoria = $("#auditoriaVerificacion").is(":checked");

                        if (esAuditoria) {
                            auditoriaDetalleCRUD.getAllDetallePlanAuditoriasAsync($("#auditoriaID").val());
                            auditoriasCRUD.getAllProcesosAuditoriaAsync();
                        }
                        else
                            procesosCRUD.getAllProcesosAsync($("#terceroClienteID").val());
                       
                    }
                    else
                        swal.fire("¡Alerta!", respuesta.error, "warning");

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método seleccionarProcesoGlobalAsync \n" + ex, "error");
        }
    },

    cargarDatosDeProcesoGlobalAsync: function (procesoGlobalID, procesoGenericoDescripcion, procesoActualID) {
        'use strict';

        try {
            var procesoActualDescripcion = $("#procesoDescripcion").val();

            swal.fire({
                title: "¡Cargar listas de verificación!",
                text: `¿Está seguro que desea cargar las listas de verificación del proceso genérico ${procesoGenericoDescripcion} en el proceso ${procesoActualDescripcion}?`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: "SI",
                cancelButtonText: "NO",
            }).then(function (resultado) {

                if (resultado.isConfirmed) {

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Procesos/CargarDatosDeProcesoGlobalAsync',
                        data: { __RequestVerificationToken: token, procesoGlobalID: procesoGlobalID, procesoActualID: procesoActualID },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var esAuditoria = $("#auditoriaVerificacion").is(":checked");

                                if (esAuditoria) {
                                    auditoriaDetalleCRUD.getAllDetallePlanAuditoriasAsync($("#auditoriaID").val());
                                    auditoriasCRUD.getAllProcesosAuditoriaAsync();
                                }  

                                $('#modal').modal('hide');
                                $('#contenedor').empty();

                                swal.fire("¡Notificación!", "Las listas de verificación fueron cargadas exitosamente", "success");
                            }
                            else
                                swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "Las listas de verificación NO fueron cargadas.", "warning");

            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método cargarNumeralesDeProcesoGlobalAsync \n" + ex, "error");
        }
    },

    deleteProcesoAsync: function (procesoID, esVistaGlobal) {
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
                        url: rootHost + 'Procesos/DeleteProcesoAsync',
                        data: { procesoID: procesoID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");

                            } else
                                swal.fire("¡Alerta!", respuesta.error, "warning");

                            var terceroID = $("#terceroClienteID").val();

                            if (esVistaGlobal)
                                terceroID = 0;

                            console.log(terceroID);
                            procesosCRUD.getAllProcesosAsync(terceroID, esVistaGlobal);

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteProcesoAsync \n" + ex, "error");
        }
    },

    deleteProcesoNumeralAsync: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/DeleteNumeralProcesoAsync',
                data: { registroID: registroID },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                    } else
                        swal.fire("¡Alerta!", respuesta.error, "warning");

                    var normaID = 0;
                    procesosCRUD.vistaObtenerNumeralesPorProcesoAsync($("#procesoID").val(), normaID);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteProcesoNumeralAsync \n" + ex, "error");
        }
    },
};
