var documentosDiagnosticoPasos_CriteriosCRUD = {

    //Vistas
    getAllCriteriosPorEvidencia: function () {
        'use strict';

        try {

            var pasoID = $("#pasosID").val();
            var evidenciaID = $("#evidenciaID").val();

            $.ajax({
                type: "GET",
                url: rootHost + 'DocumentosDiagnosticoPasos_Criterios/GetAllCriteriosPorEvidencia',
                data: { pasoID: pasoID, evidenciaID: evidenciaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divCriteriosPorEvidencia').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllCriteriosPorEvidencia \n" + ex, "error");
        }
    },

    crear: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'DocumentosDiagnosticoPasos_Criterios/Crear',
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
            swal.fire("Oops!", "Error en el método crear \n" + ex, "error");
        }
    },

    getByEditar: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'DocumentosDiagnosticoPasos_Criterios/GetByEditar',
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

        $(".btnGuardarCriterio").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarCriterio");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrCriterioID": $("#criterioID").val(),
                        "StrDescripcion": $("#criterioDescripcion").val(),
                        "StrPasoID": $("#pasosID").val(),
                        "StrEvidenciaID": $('input:radio[name=radioEvidencia]:checked').attr('id')

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'DocumentosDiagnosticoPasos_Criterios/Guardar',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                toastr.success('El criterio se guardó correctamente.');
                                documentosDiagnosticoPasos_DocumentoEvidenciaCRUD.getAllEvidenciasConCriteriosPaso($("#pasosID").val());

                                if (!aplicar) {
                                    $('#contenedor').empty();
                                    $('#modal').modal('hide');
                                } else
                                    documentosDiagnosticoPasos_CriteriosCRUD.getByEditar(respuesta.registroID);

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
                        url: rootHost + 'DocumentosDiagnosticoPasos_Criterios/Delete',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success")
                                documentosDiagnosticoPasos_DocumentoEvidenciaCRUD.getAllEvidenciasConCriteriosPaso($("#pasosID").val());
                            else swal.fire("¡Alerta!", respuesta.error, "warning");

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

    //Documento diagnóstico pasos
    getAllDocumentoDiagnosticoPasosPorSistemaDeGestion: function (sistemaDeGestionID) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'DocumentosDiagnosticoPasos_Criterios/GetAllDocumentoDiagnosticoPasosPorSistemaDeGestion',
                data: {},
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
            swal.fire("Oops!", "Error en el método getAllDocumentoDiagnosticoPasosPorSistemaDeGestion \n" + ex, "error");
        }
    },

    cambiarOrdenamientoSortable: function () {
        'use strict'

        $(".criteriosSortable").sortable({

            opacity: 0.5,
            delay: 300,
            update: function (event, ui) {

                var listaCriterios = [];

                $(".listaCriterios").each(function (index, elemento) {

                    var item = {
                        StrCriterioID: $(elemento).data('criterioid'),
                        IntOrden: index + 1
                    };

                    listaCriterios[index] = item;

                });

                $.ajax({
                    type: "POST",
                    url: rootHost + 'DocumentosDiagnosticoPasos_Criterios/CambiarOrdenamientoSortable',
                    data: { listaCriterios: listaCriterios },
                    dataType: 'JSON',
                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                    async: true,
                    success: function (respuesta) {

                        if (respuesta.msn === "success")
                            toastr.success('Reordenamiento exitoso.');
                        else {
                            form.parsley().destroy();
                            swal.fire("¡Alerta!", respuesta.error, "warning");
                        }

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                    }
                });
            },
        });
    },

};
