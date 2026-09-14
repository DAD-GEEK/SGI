var turnosCRUD = {

    //Vistas
    getAllTurnosAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Turnos/GetAllTurnosAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetTurnos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllTurnosAsync \n" + ex, "error");
        }
    },

    crearTurno: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Turnos/CrearTurno',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetTurnos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearTurno \n" + ex, "error");
        }
    },

    getTurnoByEditarAsync: function (turnoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Turnos/GetTurnoByEditarAsync',
                data: { turnoID: turnoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetTurnos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getTurnoByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos    
    guardarTurnoAsync: function () {
        'use strict';

        $("#formGuardarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntTurnoID": $("#turnoID").val(),
                        "StrCodigo": $("#turnoCodigo").val(),
                        "StrDescripcion": $("#turnoDescripcion").val(),
                        "BitActivo": $("#turnoActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Turnos/GuardarTurnoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El turno se guardó correctamente.", "success");
                                turnosCRUD.getAllTurnosAsync();
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

                swal.fire("Oops!", "Error en el método crearTurnoAsync \n" + ex, "error");
            }
        });
    },

    deleteTurnoAsync: function (turnoID) {
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
                        url: rootHost + 'Turnos/DeleteTurnoAsync',
                        data: { turnoID: turnoID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                turnosCRUD.getAllTurnosAsync();
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

            swal("Oops!", "Error en el método deleteTurnoAsync \n" + ex, "error");
        }
    }
};
