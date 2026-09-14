var auditoriaCRUD = {

    getAllAuditorias: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Auditoria/GetAllAuditorias',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllAuditorias').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getAllAuditoria \n" + ex, "error");
        }
    },

    programarAuditoriaView: function () {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Auditoria/ProgramarAuditoriaView',
                data: {},
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

            swal.fire("Oops!", "Error en el método programarAuditoriaView \n" + ex, "error");
        }
    },

    crearAuditoriaAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

               
                    //modelo Cliente
                    var modelo = {

                        "StrDescripcion": $("#auditoriaDescripcion").val(),                        
                        "OpcEstado": true 
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Auditoria/CrearAuditoriaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                auditoriaCRUD.getAllAuditoria();

                                swal.fire("¡Notificación!", "El auditoria se agregó correctamente.", "success");


                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Valicación!", respuesta.error, "warning");

                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {

                            swal.fire("Oops!", textStatus, "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearAuditoriaAsync \n" + ex, "error");
            }
        });
    },

    getAuditoriaParaEditarAsync: function (id) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Auditoria/GetAuditoriaParaEditarAsync',
                data: "{ auditoriaID: '" + $(id).val() + "'}",
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

            swal.fire("Oops!", "Error en el método getAuditoriaParaEditarAsync \n" + ex, "error");
        }
    },

    updateAuditoriaAsync: function () {
        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {
                        "IntAuditoriaID": $("#auditoriaID").val(),
                        "StrDescripcion": $("#auditoriaDescripcion").val(),
                        "OpcEstado": $("#opcEstado").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Auditoria/UpdateAuditoriaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                auditoriaCRUD.getAllAuditoria();

                                swal.fire("¡Notificación!", "El auditoria se modificó correctamente.", "success");


                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Valicación!", respuesta.error, "warning");

                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {

                            swal.fire("Oops!", textStatus, "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método updateAuditoriaAsync \n" + ex, "error");
            }
        });
    },

    deleteAuditoriaAsync: function (id) {
        'use strict';
        try {
            var token = $('input[name="__RequestVerificationToken"]').val();

            swal.fire({
                title: "¡Eliminación!",
                text: "¿Desea eliminar el registro?",
                type: "warning",

                showCancelButton: true,
                confirmButtonText: "SI",
                cancelButtonText: "NO",
                closeOnConfirm: false,
                reverseButtons: true
            },
                
                function () {
                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Auditoria/DeleteAuditoriaAsync',
                        data: { __RequestVerificationToken: token, auditoriaID: $(id).val() },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                auditoriaCRUD.getAllAuditoria();

                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Valicación!", respuesta.error, "warning");

                            }
                        },
                        error: function (respuesta) {

                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });
                });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método deleteClienteAsync \n" + ex, "error");
        }
    },

};