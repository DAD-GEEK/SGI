var escolaridadesCRUD = {

    //Vistas
    getAllEscolaridadesAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Escolaridades/GetAllEscolaridadesAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetEscolaridades').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllEscolaridadesAsync \n" + ex, "error");
        }
    },

    crearEscolaridad: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Escolaridades/CrearEscolaridad',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetEscolaridades').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearEscolaridad \n" + ex, "error");
        }
    },

    getEscolaridadByEditarAsync: function (escolaridadID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Escolaridades/GetEscolaridadByEditarAsync',
                data: { escolaridadID: escolaridadID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetEscolaridades').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getEscolaridadByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos  
    guardarEscolaridadAsync: function () {
        'use strict';

        $("#formGuardarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntEscolaridadID": $("#escolaridadID").val(),
                        "StrCodigo": $("#escolaridadCodigo").val(),
                        "StrDescripcion": $("#escolaridadDescripcion").val(),
                        "BitActivo": $("#escolaridadActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Escolaridades/GuardarEscolaridadAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                escolaridadesCRUD.getAllEscolaridadesAsync();
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

                swal.fire("Oops!", "Error en el método crearEscolaridadAsync \n" + ex, "error");
            }
        });
    },

    deleteEscolaridadAsync: function (escolaridadID) {
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
                        url: rootHost + 'Escolaridades/DeleteEscolaridadAsync',
                        data: { escolaridadID: escolaridadID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            escolaridadesCRUD.getAllEscolaridadesAsync();

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteEscolaridadAsync \n" + ex, "error");
        }
    }
};
