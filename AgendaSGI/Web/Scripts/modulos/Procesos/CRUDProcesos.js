var procesosCRUD = {

    getAllProcesos: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/GetAllProcesos',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllProcesos').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getAllProcesos \n" + ex, "error");
        }
    },

    crearProcesoAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

               
                    //modelo Cliente
                    var modelo = {

                        "StrDescripcion": $("#procesoDescripcion").val(),                        
                        "OpcEstado": true 
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Procesos/CrearProcesoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                procesosCRUD.getAllProcesos();

                                swal.fire("¡Notificación!", "El proceso se agregó correctamente.", "success");


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

                swal.fire("Oops!", "Error en el método crearProcesoAsync \n" + ex, "error");
            }
        });
    },

    getProcesoParaEditarAsync: function (id) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Procesos/GetProcesoParaEditarAsync',
                data: "{ procesoID: '" + $(id).val() + "'}",
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

            swal.fire("Oops!", "Error en el método getProcesoParaEditarAsync \n" + ex, "error");
        }
    },

    updateProcesoAsync: function () {
        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {
                        "IntProcesoID": $("#procesoID").val(),
                        "StrDescripcion": $("#procesoDescripcion").val(),
                        "OpcEstado": $("#opcEstado").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Procesos/UpdateProcesoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                procesosCRUD.getAllProcesos();

                                swal.fire("¡Notificación!", "El proceso se modificó correctamente.", "success");


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

                swal.fire("Oops!", "Error en el método updateProcesoAsync \n" + ex, "error");
            }
        });
    },

    deleteProcesoAsync: function (id) {
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
                        url: rootHost + 'Procesos/DeleteProcesoAsync',
                        data: { __RequestVerificationToken: token, procesoID: $(id).val() },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                procesosCRUD.getAllProcesos();

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