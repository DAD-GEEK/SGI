var elementosComunesCRUD = {

    //Vistas
    getAllElementosComunesAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ElementosComunes/GetAllElementosComunesAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetElementosComunes').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllElementosComunesAsync \n" + ex, "error");
        }
    },

    crearElementoComun: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ElementosComunes/CrearElementoComun',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetElementosComunes').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearElementoComun \n" + ex, "error");
        }
    },

    getElementoComunByEditarAsync: function (elementoComunID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'ElementosComunes/GetElementoComunByEditarAsync',
                data: { elementoComunID: elementoComunID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetElementosComunes').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getElementoComunByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos  
    guardarElementoComunAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                e.preventDefault();

                var aplicar = $(this).data("aplicar");

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntElementoComunID": $("#elementoComunID").val(),
                        "StrDescripcion": $("#elementoComunDescripcion").val(),
                        "BitActivo": $("#elementoComunActivo").is(":checked"),
                        "ElementosComunes_Normas": null
                    };

                    var normas = $("#elementoComunNormas").val();

                    var listaNormas = [];
                    var i = 0;
                    $.each(normas, (index, value) => {

                        var item = {
                            IntNormaID: value,
                            IntElementoComunID: $("#elementoComunID").val()
                        }

                        listaNormas[i] = item;
                        i++;
                    });

                    modelo.ElementosComunes_Normas = listaNormas;

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'ElementosComunes/GuardarElementoComunAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {

                                    toastr.success("El registro se guardó correctamente.");
                                    return elementosComunesCRUD.getElementoComunByEditarAsync(respuesta.elementoComunID);
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                elementosComunesCRUD.getAllElementosComunesAsync();
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

                swal.fire("Oops!", "Error en el método crearElementoComunAsync \n" + ex, "error");
            }
        });
    },

    deleteElementoComunAsync: function (elementoComunID) {
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
                        url: rootHost + 'ElementosComunes/DeleteElementoComunAsync',
                        data: { elementoComunID: elementoComunID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            elementosComunesCRUD.getAllElementosComunesAsync();

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteElementoComunAsync \n" + ex, "error");
        }
    }
};
