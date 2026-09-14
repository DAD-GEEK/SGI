var ausentismoVariablesCRUD = {

    //Vistas
    getAllAusentismoVariablesAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'AusentismoVariables/GetAllAusentismoVariablesAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismoVariables').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllAusentismoVariablesAsync \n" + ex, "error");
        }
    },

    crearAusentismoVariable: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'AusentismoVariables/CrearAusentismoVariable',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismoVariables').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearAusentismoVariable \n" + ex, "error");
        }
    },

    getAusentismoVariableByEditarAsync: function (ausentismoVariableID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'AusentismoVariables/GetAusentismoVariableByEditarAsync',
                data: { ausentismoVariableID: ausentismoVariableID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAusentismoVariables').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAusentismoVariableByEditar \n" + ex, "error");
        }
    },

    //Base de datos

    GuardarAusentismoVariableAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    //modelo Cliente
                    var modelo = {
                        "IntAusentismoVariablesID": $("#ausentismoVariableID").val(),
                        "IntAno": $("#ausentismoVariableAnio").val(),
                        "TIntPeriodo": $("#ausentismoVariablePeriodo").val(),
                        "IntTotalEmpleados": $("#ausentismoVariableTotalEmpleados").val(),
                        "IntHorasHombre": $("#ausentismoVariableTotalHorasHombre").val(),
                        "IntHorasHombreProgramadas": $("#ausentismoVariableTotalHorasHombreProgramadas").val(),
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'AusentismoVariables/GuardarAusentismoVariableAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar)
                                    return ausentismoVariablesCRUD.getAusentismoVariableByEditarAsync(respuesta.ausentismoVariableID);

                                swal.fire("¡Notificación!", "La variable se guardó correctamente.", "success");
                                ausentismoVariablesCRUD.getAllAusentismoVariablesAsync();

                            } else {
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearAusentismoVariableAsync \n" + ex, "error");
            }
        });
    },

    deleteAusentismoVariableAsync: function (ausentismoVariableID) {
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
                        url: rootHost + 'AusentismoVariables/DeleteAusentismoVariableAsync',
                        data: { ausentismoVariableID: ausentismoVariableID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {
                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                                ausentismoVariablesCRUD.getAllAusentismoVariablesAsync();
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });


                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");


            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteAusentismoVariableAsync \n" + ex, "error");
        }
    }
};

