var rolesCRUD = {

    //Vistas
    getAllRolesAsync: function () {
        'use strict';
        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'AspNetRoles/GetAllRolesAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllRoles').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllRolesAsync \n" + ex, "error");
        }
    },

    crearRol: function () {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'AspNetRoles/CrearRol',
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

            swal("Oops!", "Error en el método crearRol \n" + ex, "error");
        }
    },

    getRolByEditarAsync: function (registroID) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'AspNetRoles/GetRolByEditarAsync',
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

            swal("Oops!", "Error en el método getRolByEditarAsync \n" + ex, "error");
        }
    },

    getPermisosModulos: function (registroID) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'AspNetRoles/GetPermisosModulos',
                data: { registroID: registroID },
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

            swal("Oops!", "Error en el método crearRol \n" + ex, "error");
        }
    },


    //Acceso a base de datos
    crearRolAsync: function () {
        'use strict'
        try {

            $(".btnGuardarRegistroModal").click(function (e) {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formCrearGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {
                        "Name": $("#rolName").val(),
                        "Description": $("#rolDescripcion").val(),
                        "BitDefault": $("#rolMostrarEnTerceros").is(':checked'),
                        "BitActivo": true
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'AspNetRoles/GuardarRolesAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                rolesCRUD.getAllRolesAsync();

                                if (aplicar) {
                                    rolesCRUD.getRolByEditarAsync(respuesta.rolID);
                                    return toastr.success('El registro se guardó correctamente.');
                                }

                                swal.fire("¡Notificación!", "El rol se guardó correctamente.", "success");
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

            swal.fire("¡Error!", "Error en el método crearRolAsync \n" + ex, "error");
        }

    },

    updateRolAsync: function () {
        'use strict'
        try {

            $(".btnGuardarRegistroModal").click(function (e) {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formEditarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {
                        "Id": $("#rolID").val(),
                        "Name": $("#rolName").val(),
                        "Description": $("#rolDescripcion").val(),
                        "BitDefault": $("#rolMostrarEnTerceros").is(':checked'),
                        "BitActivo": $("#rolActivo").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'AspNetRoles/GuardarRolesAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                rolesCRUD.getAllRolesAsync();

                                if (aplicar)
                                    return toastr.success('El registro se guardó correctamente.');

                                swal.fire("¡Notificación!", "El rol se guardó correctamente.", "success");
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

            swal.fire("¡Error!", "Error en el método updateRolAsync \n" + ex, "error");
        }

    },

    deleteRolAsync: function (registroID) {
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
                        url: rootHost + 'AspNetRoles/DeleteRolAsync',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                                rolesCRUD.getAllRolesAsync();
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteRolAsync \n" + ex, "error");
        }
    },


};