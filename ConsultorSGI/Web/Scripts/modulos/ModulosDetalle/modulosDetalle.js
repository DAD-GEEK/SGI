var modulosDetalleCRUD = {

    //Vistas
    getAllModulosDetalleAsync: function (moduloID) {
        'use strict';
        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ModulosDetalle/GetAllModulosDetalleAsync',
                data: { moduloID: moduloID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllModulosDetalle').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllModulosDetalle \n" + ex, "error");
        }
    },

    crearModuloDetalle: function () {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'ModulosDetalle/CrearModuloDetalle',
                data: {},
                dataType: "html",
                async: false,
                success: function (respuesta) {

                    $('#contenedor2').empty().html(respuesta);
                    $('#modal2').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }

            });

        } catch (ex) {

            swal("Oops!", "Error en el método crearModuloDetalle \n" + ex, "error");
        }
    },

    getModuloDetalleByEditarAsync: function (registroID) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'ModulosDetalle/GetModuloDetalleByEditarAsync',
                data: { registroID: registroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#contenedor2').empty().html(respuesta);
                    $('#modal2').modal('show');

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }

            });

        } catch (ex) {

            swal("Oops!", "Error en el método getModuloDetalleByEditarAsync \n" + ex, "error");
        }
    },

    //Acceso a base de datos
    crearModuloDetalleAsync: function () {
        'use strict'
        try {

            $("#formCrearDetalleGlobal").submit(function (e) {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var numeroOrden = $("#moduloDetalleOrden").val();
                    if (numeroOrden.length == 0) numeroOrden = 0;

                    var modelo = {
                        "IntModuloID": $("#moduloRegistroID").val(),
                        "StrModuloDetalle": $("#moduloDetalleNombre").val(),
                        "StrModuloDetalle": $("#moduloDetalleNombre").val(),
                        "StrDescripcion": $("#moduloDetalleDescripcion").val(),
                        "StrControlador": $("#moduloDetalleControlador").val(),
                        "StrAccion": $("#moduloDetalleAccion").val(),
                        "StrIcono": $("#moduloDetalleIcono").val(),
                        "TIntOrden": numeroOrden,
                        "DatFechaCreacion": $("#moduloFecha").val(),
                        "BitActivo": true
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'ModulosDetalle/CrearModuloDetalleAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                var moduloID = $("#moduloRegistroID").val();
                                $('#modal2').modal('hide');
                                modulosDetalleCRUD.getAllModulosDetalleAsync(moduloID);

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                }
            });



        } catch (ex) {

            swal.fire("¡Error!", "Error en el método crearModuloDetalleAsync \n" + ex, "error");
        }

    },

    updateModuloDetalleAsync: function () {
        'use strict'
        try {

            $("#formEditarDetalleGlobal").submit(function (e) {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var numeroOrden = $("#moduloDetalleOrden").val();
                    if (numeroOrden.length == 0) numeroOrden = 0;

                    var modelo = {

                        "IntModuloDetalleID": $("#moduloDetalleID").val(),
                        "IntModuloID": $("#moduloPrincipalID").val(),
                        "StrModuloDetalle": $("#moduloDetalleNombre").val(),
                        "StrModuloDetalle": $("#moduloDetalleNombre").val(),
                        "StrDescripcion": $("#moduloDetalleDescripcion").val(),
                        "StrControlador": $("#moduloDetalleControlador").val(),
                        "StrAccion": $("#moduloDetalleAccion").val(),
                        "StrIcono": $("#moduloDetalleIcono").val(),
                        "TIntOrden": numeroOrden,
                        "DatFechaCreacion": $("#moduloFecha").val(),
                        "BitActivo": $("#moduloDetalleActivo").is(':checked')

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'ModulosDetalle/UpdateModuloDetalleAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                var moduloID = $("#moduloRegistroID").val();
                                $('#modal2').modal('hide');
                                modulosDetalleCRUD.getAllModulosDetalleAsync(moduloID);

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

    deleteModuloDetalleAsync: function (registroID) {
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
                        url: rootHost + 'ModulosDetalle/DeleteModuloDetalleAsync',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                var moduloID = $("#moduloRegistroID").val();
                                modulosDetalleCRUD.getAllModulosDetalleAsync(moduloID);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteModuloDetalleAsync \n" + ex, "error");
        }
    },


};