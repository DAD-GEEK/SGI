var preguntasCRUD = {

    getAllPreguntas: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Preguntas/GetAllPreguntas',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllPreguntas').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getAllPreguntas \n" + ex, "error");
        }
    },

    crearPreguntaAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

               
                    //modelo Cliente
                    var modelo = {

                        "StrDescripcion": $("#preguntaDescripcion").val(),    
                        "StrEvidencia": $("#preguntaEvidencia").val(),            
                        "IntProcesoID": $("#preguntaProceso").val(),                        
                        "IntNumeralID": $("#preguntaNumeral").val(),       
                        "OpcEstado": true 
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Preguntas/CrearPreguntaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                preguntasCRUD.getAllPreguntas();

                                swal.fire("¡Notificación!", "El pregunta se agregó correctamente.", "success");


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

                swal.fire("Oops!", "Error en el método crearPreguntaAsync \n" + ex, "error");
            }
        });
    },

    getPreguntaParaEditarAsync: function (id) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Preguntas/GetPreguntaParaEditarAsync',
                data: "{ preguntaID: '" + $(id).val() + "'}",
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

            swal.fire("Oops!", "Error en el método getPreguntaParaEditarAsync \n" + ex, "error");
        }
    },

    updatePreguntaAsync: function () {
        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrDescripcion": $("#preguntaDescripcion").val(),
                        "StrEvidencia": $("#preguntaEvidencia").val(),
                        "IntProcesoID": $("#preguntaProceso").val(),
                        "IntNumeralID": $("#preguntaNumeral").val(),
                        "OpcEstado": $("#opcEstado").is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Preguntas/UpdatePreguntaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                preguntasCRUD.getAllPreguntas();

                                swal.fire("¡Notificación!", "El pregunta se modificó correctamente.", "success");


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

                swal.fire("Oops!", "Error en el método updatePreguntaAsync \n" + ex, "error");
            }
        });
    },

    deletePreguntaAsync: function (id) {
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
                        url: rootHost + 'Preguntas/DeletePreguntaAsync',
                        data: { __RequestVerificationToken: token, preguntaID: $(id).val() },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                preguntasCRUD.getAllPreguntas();

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