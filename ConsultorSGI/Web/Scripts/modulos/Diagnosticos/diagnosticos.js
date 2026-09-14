var diagnosticosCRUD = {

    //Vistas
    getAllDiagnosticosAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Diagnosticos/GetAllDiagnosticosAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetDiagnosticos').empty().html(respuesta);
                  
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllDiagnosticosAsync \n" + ex, "error");
        }
    },

    crearDiagnostico: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Diagnosticos/CrearDiagnostico',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetDiagnosticos').empty().html(respuesta);
                    
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearDiagnostico \n" + ex, "error");
        }
    },

    getDiagnosticoByEditarAsync: function (diagnosticoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Diagnosticos/GetDiagnosticoByEditarAsync',
                data: { diagnosticoID: diagnosticoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetDiagnosticos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getDiagnosticoByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos    
    guardarDiagnosticoAsync: function () {
        'use strict';

        $("#formGuardarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntDiagnosticoID": $("#diagnosticoID").val(),
                        "StrCodigo": $("#diagnosticoCodigo").val(),
                        "StrDescripcion": $("#diagnosticoDescripcion").val(),                       
                        "BitActivo": $("#diagnosticoActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Diagnosticos/GuardarDiagnosticoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                diagnosticosCRUD.getAllDiagnosticosAsync();
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

                swal.fire("Oops!", "Error en el método guardarDiagnosticoAsync \n" + ex, "error");
            }
        });
    },

    deleteDiagnosticoAsync: function (diagnosticoID) {
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
                        url: rootHost + 'Diagnosticos/DeleteDiagnosticoAsync',
                        data: { diagnosticoID: diagnosticoID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            diagnosticosCRUD.getAllDiagnosticosAsync();

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteDiagnosticoAsync \n" + ex, "error");
        }
    }
};
