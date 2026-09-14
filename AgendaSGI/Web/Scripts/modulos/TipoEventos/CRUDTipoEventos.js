var tipoEventosCRUD = {

    getAllTipoEventos: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'TipoEventos/GetAllTipoEventos',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllTipoEventos').html(respuesta);
                },
                error: function (textStatus) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getAllTipoEventos \n" + ex, "error");
        }
    },

    crearTipoEventoAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    //modelo
                    var modelo = {

                        "StrCodigo": $("#tipoEventoCodigo").val(),
                        "StrDescripcion": $("#tipoEventoDescripcion").val(),
                        "OpcSoporte": $("#opcSoporte").is(':checked'),
                        "OpcAlerta": $("#opcAlerta").is(':checked'),
                        "OpcEstado": true 
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'TipoEventos/CrearTipoEventoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                tipoEventosCRUD.getAllTipoEventos();

                                swal.fire("¡Notificación!", "El registro se guardó correctamente", "success");


                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Valicación!", respuesta.error, "info");

                            }
                        },
                        error: function (ex) {

                            swal.fire("Oops!", ex, "error");
                        }
                    });
                } 

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearTipoEventoAsync \n" + ex, "error");
            }
        });
    },

    getTipoEventoParaEditarAsync: function (id) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'TipoEventos/GetTipoEventoParaEditarAsync',
                data: "{ tipoEventoID: '" + $(id).val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');

                },
                error: function (respuesta) {

                    swal.fire("Oops!", JSON.stringify(respuesta), "error");
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método getTipoEventoParaEditarAsync \n" + ex, "error");
        }
    },

    updateTipoEventoAsync: function () {
        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    //modelo
                    var modelo = {

                        "IntTipoEventoID": $("#tipoEventoID").val(),
                        "StrCodigo": $("#tipoEventoCodigo").val(),
                        "StrDescripcion": $("#tipoEventoDescripcion").val(),
                        "OpcSoporte": $("#opcSoporte").is(':checked'),
                        "OpcAlerta": $("#opcAlerta").is(':checked'),
                        "OpcEstado": $("#opcEstado").is(':checked'),
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'TipoEventos/UpdateTipoEventoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                tipoEventosCRUD.getAllTipoEventos();

                                swal.fire("¡Notificación!", "El registro se guardó correctamente", "success");


                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Valicación!", respuesta.error, "info");

                            }
                        },
                        error: function (ex) {

                            swal.fire("Oops!", ex, "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método updateTipoEventoAsync \n" + ex, "error");
            }
        });
    },

    deleteTipoEventoAsync: function (id) {
        'use strict';
        try {
            var token = $('input[name="__RequestVerificationToken"]').val();

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
                        url: rootHost + 'Agenda/DeleteEventoAsync',
                        data: { __RequestVerificationToken: token, EventoID: $(id).data("eventoid") },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {


                            if (respuesta.msn == "success") {

                                $.ajax({
                                    type: "POST",
                                    url: rootHost + 'TipoEventos/DeleteTipoEventoAsync',
                                    data: { __RequestVerificationToken: token, tipoEventoID: $(id).val() },
                                    contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                                    dataType: "JSON",
                                    async: true,
                                    success: function (respuesta) {

                                        if (respuesta.msn === "success") {

                                            tipoEventosCRUD.getAllTipoEventos();

                                            swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                                        } else if (respuesta.error != null) {
                                            if (respuesta.exc == true) {
                                                swal.fire("¡Error!", respuesta.error, "error");
                                                return;
                                            }
                                            swal.fire("¡Valicación!", respuesta.error, "info");

                                        }
                                    },
                                    error: function (respuesta) {

                                        swal.fire("Oops!", JSON.stringify(respuesta), "error");
                                    }
                                });

                            } else {
                                swal.fire("¡Notificación!", respuesta.error, "warning");
                            }
                        },
                        error: function (respuesta) {

                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });


                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });


        } catch (ex) {

            swal.fire("Oops!", "Error en el método deleteTipoEventoAsync \n" + ex, "error");
        }
    },

    checkExists: function (id) {
        'use strict';

        try {
            var valor = $(id).val();

            $.ajax({
                type: "POST",
                url: rootHost + 'TipoEventos/CheckExists',
                data: "{ StrCodigo: '" + valor + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    var codigo = $("#codigo").val();

                    $("#codigoIcono").show();
                    $(id).addClass("border-right-0");
                    $(id).removeClass("is-invalid");
                    $("#errorTipoEventoCodigo").text("");

                    if (respuesta === "True" && $(id).val() != codigo) {

                        if ($("#clienteIdentificacion").parsley() != null) {
                            $("#clienteIdentificacion").parsley().destroy();
                        }
                       
                        $("#codigoIcono").hide();
                        $(id).removeClass("border-right-0");
                        $(id).addClass("is-invalid");
                        $("#errorTipoEventoCodigo").text("Este código ya existe. Intente con otro código diferente.");
                       
                    }
                },
                error: function (respuesta) {

                    swal.fire("Oops!", JSON.stringify(respuesta), "error");
                }
            });
        } catch (ex) {
            swal.fire("¡Error!", "Error en el método CheckExists \n" + ex, "error");
        }
      
    },

};