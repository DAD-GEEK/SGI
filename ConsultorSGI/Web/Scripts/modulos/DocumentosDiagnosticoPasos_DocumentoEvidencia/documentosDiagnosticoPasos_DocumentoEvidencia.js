var documentosDiagnosticoPasos_DocumentoEvidenciaCRUD = {

    //Vistas
    getAllEvidenciasConCriteriosPaso: function () {

        'use strict';

        try {

            var pasoID = $("#pasosID").val();

            $.ajax({
                type: "GET",
                url: rootHost + 'DocumentosDiagnosticoPasos_DocumentoEvidencia/GetAllEvidenciasConCriteriosPaso',
                data: { pasoID: pasoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divCriteriosConEvidencia').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllEvidenciasConCriteriosPaso \n" + ex, "error");
        }

    },

    getAllEvidenciasPorPaso: function () {

        'use strict';

        try {

            var pasoID = $("#pasosID").val();
            var criterioID = $("#criterioID").val();

            $.ajax({
                type: "GET",
                url: rootHost + 'DocumentosDiagnosticoPasos_DocumentoEvidencia/GetAllEvidenciasPorCriterio',
                data: { pasoID: pasoID, criterioID: criterioID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divEvidenciasPorPaso').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllEvidenciasPorPaso \n" + ex, "error");
        }

    },

    crear: function () {
        'use strict';

        try {

            $("#btnAgregarDocumentoEvidencia").click(function () {

                $.ajax({
                    type: "POST",
                    url: rootHost + 'DocumentosDiagnosticoPasos_DocumentoEvidencia/Crear',
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
            swal.fire("Oops!", "Error en el método crear \n" + ex, "error");
        }
    },

    getByEditar: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'DocumentosDiagnosticoPasos_DocumentoEvidencia/GetByEditar',
                data: { registroID: registroID },
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
            swal.fire("Oops!", "Error en el método getByEditar \n" + ex, "error");
        }
    },

    //Base de datos
    guardar: function () {
        'use strict';

        $(".btnGuardarEvidencia").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarEvidencia");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrEvidenciaID": $("#evidenciaID").val(),
                        "StrPasoID": $("#pasosID").val(),
                        "StrDescripcion": $("#evidenciaDescripcion").val(),
                        "DocumentosDiagnosticoPasos_Criterios": null
                    };

                    var listaCriteriosSeleccionados = [];
                    var i = 0
                    $("#tblCriteriosPorEvidencia > tbody > tr").each(function (index, elemento) {

                        var elementoDeSeleccion = $(elemento).find('.iSeleccionarCriterio');
                        var estaSeleccionado = elementoDeSeleccion.is(":checked");

                        if (estaSeleccionado) {
                            var criterioID = elementoDeSeleccion.attr('id');

                            var modeloCriterio = {
                                StrCriterioID: criterioID
                            }

                            listaCriteriosSeleccionados[i] = modeloCriterio;
                            i++;
                        }
                    });

                    modelo.DocumentosDiagnosticoPasos_Criterios = listaCriteriosSeleccionados;

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'DocumentosDiagnosticoPasos_DocumentoEvidencia/Guardar',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                toastr.success('El soporte se guardó correctamente.');
                                documentosDiagnosticoPasos_DocumentoEvidenciaCRUD.getAllEvidenciasConCriteriosPaso($("#pasosID").val());

                                if (!aplicar) {
                                    $('#contenedor').empty();
                                    $('#modal').modal('hide');
                                } else
                                    documentosDiagnosticoPasos_DocumentoEvidenciaCRUD.getByEditar(respuesta.registroID);

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
                        url: rootHost + 'DocumentosDiagnosticoPasos_DocumentoEvidencia/Delete',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                documentosDiagnosticoPasos_DocumentoEvidenciaCRUD.getAllEvidenciasConCriteriosPaso($("#pasosID").val());
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

};
