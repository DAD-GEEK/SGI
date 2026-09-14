var cargosCRUD = {

    //Vistas
    getAllCargosAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Cargos/GetAllCargosAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetCargos').empty().html(respuesta);
                  
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllCargosAsync \n" + ex, "error");
        }
    },

    crearCargo: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Cargos/CrearCargo',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetCargos').empty().html(respuesta);
                    
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearCargo \n" + ex, "error");
        }
    },

    getCargoByEditarAsync: function (cargoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Cargos/GetCargoByEditarAsync',
                data: { cargoID: cargoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetCargos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getCargoByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos

    guardarCargoAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntCargoID": $("#cargoID").val(),
                        "StrCodigo": $("#cargoCodigo").val(),
                        "StrDescripcion": $("#cargoDescripcion").val(),                       
                        "BitActivo": $("#cargoActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Cargos/GuardarCargoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {
                                    toastr.success('El registro se guardó correctamente.');
                                    return cargosCRUD.getCargoByEditarAsync(respuesta.cargoID);
                                }

                                swal.fire("¡Notificación!", "El cargo se guardó correctamente.", "success");
                                cargosCRUD.getAllCargosAsync();
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

                swal.fire("Oops!", "Error en el método crearCargoAsync \n" + ex, "error");
            }
        });
    },

    deleteCargoAsync: function (cargoID) {
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
                        url: rootHost + 'Cargos/DeleteCargoAsync',
                        data: { cargoID: cargoID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                cargosCRUD.getAllCargosAsync();
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

            swal("Oops!", "Error en el método deleteCargoAsync \n" + ex, "error");
        }
    }
};
