var numeralesCRUD = {

    getAllNumerales: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Numerales/GetAllNumerales',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllNumerales').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getAllNumerales \n" + ex, "error");
        }
    },

    crearNumeralAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

               
                    //modelo Cliente
                    var modelo = {

                        "StrDescripcion": $("#numeralDescripcion").val(),                        
                        "OpcEstado": true 
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Numerales/CrearNumeralAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                numeralesCRUD.getAllNumerales();

                                swal.fire("¡Notificación!", "El numeral se agregó correctamente.", "success");


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

                swal.fire("Oops!", "Error en el método crearNumeralAsync \n" + ex, "error");
            }
        });
    },

    getNumeralParaEditarAsync: function (id) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Numerales/GetNumeralParaEditarAsync',
                data: "{ numeralID: '" + $(id).val() + "'}",
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

            swal.fire("Oops!", "Error en el método getNumeralParaEditarAsync \n" + ex, "error");
        }
    },

    updateNumeralAsync: function () {
        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {
                        "IntNumeralID": $("#numeralID").val(),
                        "StrDescripcion": $("#numeralDescripcion").val(),
                        "OpcEstado": $("#opcEstado").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Numerales/UpdateNumeralAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                numeralesCRUD.getAllNumerales();

                                swal.fire("¡Notificación!", "El numeral se modificó correctamente.", "success");


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

                swal.fire("Oops!", "Error en el método updateNumeralAsync \n" + ex, "error");
            }
        });
    },

    deleteNumeralAsync: function (id) {
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
                        url: rootHost + 'Numerales/DeleteNumeralAsync',
                        data: { __RequestVerificationToken: token, numeralID: $(id).val() },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                numeralesCRUD.getAllNumerales();

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