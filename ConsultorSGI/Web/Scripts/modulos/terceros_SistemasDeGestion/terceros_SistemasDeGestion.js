var terceros_SistemasDeGestionCRUD = {

    //Vistas
    index: function () {

        var dropDownListSistemasDeGestion = $("#terceroSistemaDeGestionID");
        var dropDownListNiveles = $("#terceroNivelID");

        dropDownListSistemasDeGestion.change(function () {
            var sistemaDeGestion = $(this).val();

            if (sistemaDeGestion == 'PEVS') {
                $("#divTerceroNiveles").removeClass("d-none");
                dropDownListNiveles.attr("required", true);
            }
            else {
                $("#divTerceroNiveles").addClass("d-none");
                dropDownListNiveles.attr("required", false);
            }
        });
    },

    getAllAsync: function () {
        'use strict';

        try {

            var terceroID = $("#terceroID").val();

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros_SistemasDeGestion/GetAllByTerceroIDAsync',
                data: { terceroID: terceroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllSistemasDeGestion').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllAsync \n" + ex, "error");
        }
    },

    crear: function () {
        'use strict';

        try {

            var btnAgregarSistemaDeGestion = $("#btnAgregarSistemaDeGestion");
            btnAgregarSistemaDeGestion.click(function () {

                $.ajax({
                    type: "POST",
                    url: rootHost + 'Terceros_SistemasDeGestion/Crear',
                    data: {},
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


            });


        } catch (ex) {
            swal.fire("Oops!", "Error en el método crear \n" + ex, "error");
        }
    },

    getByEditarAsync: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros_SistemasDeGestion/GetByEditarAsync',
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
            swal.fire("Oops!", "Error en el método getByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos    
    guardarAsync: function () {
        'use strict';

        $(".btnGuardarRegistroModal").click(function (e) {
            try {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formGuardarGlobalModal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntRegistroID": $("#registroID").val(),
                        "IntTerceroID": $("#terceroID").val(),
                        "StrSistemaDeGestionID": $("#terceroSistemaDeGestionID").val(),
                        "Terceros_SistemasDeGestion_Normas": null
                    };

                    var listaNormas = [];
                    var iterador = 0

                    $("#tblSistemasDeGestionNormas > tbody > tr").each(function (i, elemento) {

                        var elementoCheckbox = $(elemento).find(".aplicarNorma");
                        var aplicarNorma = elementoCheckbox.is(":checked");

                        if (aplicarNorma) {

                            var normaID = elementoCheckbox.attr("id");

                            var item = {
                                IntNormaID: normaID
                            };

                            listaNormas[iterador] = item;
                            iterador = iterador + 1;
                        }

                    });

                    modelo.Terceros_SistemasDeGestion_Normas = listaNormas;

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Terceros_SistemasDeGestion/GuardarAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, nivelID: $("#terceroNivelID").val() },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                terceros_SistemasDeGestionCRUD.getAllAsync();

                                if (aplicar) {

                                    $("#registroID").val(String(respuesta.regitroID));
                                    return toastr.success('El registro se guardó correctamente.');
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");

                                $('#contenedor').empty();
                                $('#modal').modal('hide');

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

                swal.fire("Oops!", "Error en el método guardarAsync \n" + ex, "error");
            }
        });
    },

    deleteAsync: function (registroID) {
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
                        url: rootHost + 'Terceros_SistemasDeGestion/DeleteAsync',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                terceros_SistemasDeGestionCRUD.getAllAsync();
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

            swal("Oops!", "Error en el método deleteAsync \n" + ex, "error");
        }
    },

    aplicarNormaAsync: function (elemento) {
        'use strict'

        try {

            var registroID = $(elemento).data("registroid")
            var normaID = $(elemento).attr("id");
            var terceroID = $("#terceroID").val();

            var modelo = {
                IntRegistroID: registroID, // Id de la tabla Terceros_SistemasDeGestion_Normas
                IntTerceros_SistemasDeGestionID: $("#registroID").val(),// Id de la tabla Terceros_SistemasDeGestion
                IntNormaID: normaID,
                Terceros_SistemasDeGestion: {
                    IntTerceroID: terceroID
                }
            }

            var token = $('input[name="__RequestVerificationToken"]').val();


            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros_SistemasDeGestion/AplicarNormaAsync',
                data: { __RequestVerificationToken: token, modelo: modelo },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                        toastr.success('La norma se asignó correctamente.');
                        terceros_SistemasDeGestionCRUD.getAllAsync();

                    } else {
                        swal.fire("¡Alerta!", respuesta.error, "error");

                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });



        } catch (ex) {

            swal.fire("Oops!", "Error en el método guardarAsync \n" + ex, "error");
        }
    }
};
