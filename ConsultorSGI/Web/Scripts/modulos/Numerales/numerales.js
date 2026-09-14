var numeralesCRUD = {

    //Vistas
    getAllNumeralesAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Numerales/GetAllNumeralesAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetNormas').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllNumeralesAsync \n" + ex, "error");
        }
    },

    crearNumeral: function (normaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Numerales/CrearNumeral',
                data: { normaID: normaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetNormas').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearNumeral \n" + ex, "error");
        }
    },

    getNumeralByEditarAsync: function (numeralID, normaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Numerales/GetNumeralByEditarAsync',
                data: { numeralID: numeralID, normaID: normaID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetNormas').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getNumeralByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos  
    guardarNumeralAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    EliminarultimoCaracterSiEsPunto();

                    function EliminarultimoCaracterSiEsPunto() {
                        var codigoNumeral = $("#numeralCodigo").val().trim();
                        var ultimoCaracter = codigoNumeral.charAt(codigoNumeral.length - 1);

                        if (ultimoCaracter === ".")
                            EliminarPuntoFinal(codigoNumeral);
                    }

                    function EliminarPuntoFinal(codigoNumeral) {
                        codigoNumeral = codigoNumeral.substring(0, codigoNumeral.length - 1);
                        $("#numeralCodigo").val(codigoNumeral);
                        EliminarultimoCaracterSiEsPunto();
                    }

                    var modelo = {
                        "IntNumeralID": $("#numeralID").val(),
                        "StrCodigo": $("#numeralCodigo").val(),
                        "StrDescripcion": $("#numeralDescripcion").val(),
                        "StrInterpretacion": $("#numeralInterpretacion").val(),
                        "IntNormaID": $("#normaID").val(),
                        "BitActivo": $("#numeralActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Numerales/GuardarNumeralAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, normaID: $("#normaID").val() },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {
                                    numeralesCRUD.getNumeralByEditarAsync(respuesta.numeralID, respuesta.normaID);
                                    return toastr.success('El registro se guardó correctamente');
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                normasCRUD.getAllNormasAsync();
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

                swal.fire("Oops!", "Error en el método crearNumeralAsync \n" + ex, "error");
            }
        });
    },

    deleteNumeralAsync: function (numeralID) {
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
                        url: rootHost + 'Numerales/DeleteNumeralAsync',
                        data: { numeralID: numeralID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            normasCRUD.getAllNormasAsync();

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteNumeralAsync \n" + ex, "error");
        }
    }
};
