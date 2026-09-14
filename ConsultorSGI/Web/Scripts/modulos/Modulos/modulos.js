var modulosCRUD = {

    //Vistas
    getAllModulosAsync: function () {
        'use strict';
        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Modulos/GetAllModulosAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllModulos').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllModulos \n" + ex, "error");
        }
    },

    crearModulo: function () {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Modulos/CrearModulo',
                data: {},
                dataType: "html",
                async: false,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método crearModulo \n" + ex, "error");
        }
    },

    getModuloByEditarAsync: function (registroID) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'modulos/GetModuloByEditarAsync',
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

            swal("Oops!", "Error en el método getModuloByEditarAsync \n" + ex, "error");
        }
    },

    //Acceso a base de datos
    crearModuloAsync: function () {
        'use strict'
        try {

            $(".btnGuardarRegistroModal").click(function (e) {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formCrearGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var numeroOrden = $("#moduloOrden").val();
                    if (numeroOrden.length == 0) numeroOrden = 0;

                    var modelo = {
                        "StrModulo": $("#moduloNombre").val(),
                        "StrDescripcion": $("#moduloDescripcion").val(),
                        "StrIcono": $("#moduloIcono").val(),
                        "TIntOrden": numeroOrden,
                        "BitActivo": true
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Modulos/CrearModuloAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                modulosCRUD.getAllModulosAsync();

                                if (aplicar) {
                                    modulosCRUD.getModuloByEditarAsync(respuesta.moduloID);
                                    return toastr.success('El usuario se guardó correctamente.');
                                }

                                swal.fire("¡Notificación!", "El modulo se guardó correctamente.", "success");
                                $('#modal').modal('hide');
                                $('#contenedor').empty();

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                }
            });



        } catch (ex) {

            swal.fire("¡Error!", "Error en el método crearModuloAsync \n" + ex, "error");
        }

    },

    updateModuloAsync: function () {
        'use strict'
        try {

            $(".btnGuardarRegistroModal").click(function (e) {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formEditarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var numeroOrden = $("#moduloOrden").val();
                    if (numeroOrden.length == 0) numeroOrden = 0;

                    var modelo = {
                        "IntModuloID": $("#moduloRegistroID").val(),
                        "StrModulo": $("#moduloNombre").val(),
                        "StrDescripcion": $("#moduloDescripcion").val(),
                        "StrIcono": $("#moduloIcono").val(),
                        "TIntOrden": numeroOrden,
                        "BitActivo": $("#moduloActivo").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Modulos/UpdateModuloAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                modulosCRUD.getAllModulosAsync();

                                if (aplicar)
                                    return toastr.success('El usuario se guardó correctamente.');

                                swal.fire("¡Notificación!", "El modulo se guardó correctamente.", "success");
                                $('#modal').modal('hide');
                                $('#contenedor').empty();

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                }
            });

        } catch (ex) {

            swal.fire("¡Error!", "Error en el método updateModuloAsync \n" + ex, "error");
        }

    },

    deleteModuloAsync: function (registroID) {
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
                        url: rootHost + 'Modulos/DeleteModuloAsync',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                                modulosCRUD.getAllModulosAsync();

                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteModuloAsync \n" + ex, "error");
        }
    },


};