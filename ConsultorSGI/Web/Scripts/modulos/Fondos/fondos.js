var fondosCRUD = {

    //Vistas
    getAllFondosAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Fondos/GetAllFondosAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetFondos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllFondosAsync \n" + ex, "error");
        }
    },

    crearFondo: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Fondos/CrearFondo',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetFondos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearFondo \n" + ex, "error");
        }
    },

    getFondoByEditarAsync: function (fondoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Fondos/GetFondoByEditarAsync',
                data: { fondoID: fondoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetFondos').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getFondoByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos   
    guardarFondoAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntFondoID": $("#fondoID").val(),
                        "StrCodigo": $("#fondoCodigo").val(),
                        "StrDescripcion": $("#fondoDescripcion").val(),
                        "StrNit": $("#fondoNit").val(),
                        "IntTipoFondoID": $("#fondoTiposFondo").val(),
                        "BitActivo": $("#fondoActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Fondos/GuardarFondoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {
                                    fondosCRUD.getFondoByEditarAsync(respuesta.fondoID);
                                    return toastr.success('El registro se guardó correctamente.');
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                fondosCRUD.getAllFondosAsync();
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

                swal.fire("Oops!", "Error en el método guardarFondoAsync \n" + ex, "error");
            }
        });
    },

    deleteFondoAsync: function (fondoID) {
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
                        url: rootHost + 'Fondos/DeleteFondoAsync',
                        data: { fondoID: fondoID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");
                            fondosCRUD.getAllFondosAsync();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteFondoAsync \n" + ex, "error");
        }
    }
};
