var documentosDiagnosticoPasosCRUD = {

    //Vistas
    getAllPorSistemaDeGestion: function (sistemaDeGestion) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'DocumentosDiagnosticoPasos/GetAllPorSistemaDeGestion',
                data: { sistemaDeGestion: sistemaDeGestion },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divDocumentoDiagnosticoPasos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllPorSistemaDeGestion \n" + ex, "error");
        }
    },

    crear: function () {
        'use strict';

        try {

            var sistemaDeGestionID = $("#sistemaDeGestionID").val();

            $.ajax({
                type: "POST",
                url: rootHost + 'DocumentosDiagnosticoPasos/Crear',
                data: { sistemaDeGestionID: sistemaDeGestionID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divDocumentoDiagnosticoPasos').empty().html(respuesta);
                },

                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crear \n" + ex, "error");
        }
    },

    getByEditar: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'DocumentosDiagnosticoPasos/GetByEditar',
                data: { registroID: registroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divDocumentoDiagnosticoPasos').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getByEditar \n" + ex, "error");
        }
    },

    //Base de datos
    crearRegistro: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");
                var abrirCriterios = $(this).data("abrircriterios");
                console.log(aplicar);

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrFaseID": $("#pasosFaseID").val(),
                        "StrSistemaDeGestionID": $("#sistemaDeGestionID").val(),
                        "IntNumero": $("#pasosNumero").val(),
                        "StrRequisito": $("#pasosRequisito").val(),
                        "BitActivo": true,
                        "DocumentosDiagnosticoPasos_Niveles": null

                    };

                    var listaNivelesSeleccionadas = $("#pasosNivelesID").val();
                    var listaNiveles = [];
                    var i = 0;

                    $.each(listaNivelesSeleccionadas, (index, value) => {

                        var item = {
                            StrNivelID: value
                        }

                        listaNiveles[i] = item;
                        i++;

                    });

                    modelo.DocumentosDiagnosticoPasos_Niveles = listaNiveles;

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'DocumentosDiagnosticoPasos/Guardar',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (abrirCriterios)
                                    documentosDiagnosticoPasos_CriteriosCRUD.crear();
                                else
                                    swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");

                                documentosDiagnosticoPasosCRUD.getByEditar(respuesta.registroID);

                            } else {
                                form.parsley().destroy();
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearRegistro \n" + ex, "error");
            }
        });
    },

    actualizarRegistro: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrPasoID": $("#pasosID").val(),
                        "StrFaseID": $("#pasosFaseID").val(),
                        "StrSistemaDeGestionID": $("#sistemaDeGestionID").val(),
                        "IntNumero": $("#pasosNumero").val(),
                        "StrRequisito": $("#pasosRequisito").val(),
                        "BitActivo": $("#pasosActivo").is(":checked"),

                    };

                    var listaNivelesSeleccionadas = $("#pasosNivelesID").val();
                    var listaNiveles = [];
                    var i = 0;

                    $.each(listaNivelesSeleccionadas, (index, value) => {

                        var item = {
                            StrNivelID: value
                        }

                        listaNiveles[i] = item;
                        i++;

                    });

                    modelo.DocumentosDiagnosticoPasos_Niveles = listaNiveles;

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'DocumentosDiagnosticoPasos/Guardar',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var abrirCriterios = $(this).data('abrircriterios');

                                if (aplicar) {
                                    toastr.success('El registro se guardó correctamente.');
                                    //    return documentosDiagnosticoPasosCRUD.getByEditar(respuesta.registroID);
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                documentosDiagnosticoPasosCRUD.getAllPorSistemaDeGestion($("#sistemaDeGestionID").val());
                            } else {
                                form.parsley().destroy();
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método guardar \n" + ex, "error");
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
                        url: rootHost + 'DocumentosDiagnosticoPasos/Delete',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                documentosDiagnosticoPasosCRUD.getAllPorSistemaDeGestion($("#sistemaDeGestionID").val());
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método delete \n" + ex, "error");
        }
    },

    //Documento diagnóstico pasos
    getAllDocumentoDiagnosticoPasosPorSistemaDeGestion: function (sistemaDeGestionID) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'DocumentosDiagnosticoPasos/GetAllDocumentoDiagnosticoPasosPorSistemaDeGestion',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divDocumentoDiagnosticoPasos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllDocumentoDiagnosticoPasosPorSistemaDeGestion \n" + ex, "error");
        }
    },

};
