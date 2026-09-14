var normasCRUD = {

    //Vistas
    getAllNormasAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Normas/GetAllNormasAsync',
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
            swal.fire("Oops!", "Error en el método getAllNormasAsync \n" + ex, "error");
        }
    },

    crearNorma: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Normas/CrearNorma',
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
            swal.fire("Oops!", "Error en el método crearNorma \n" + ex, "error");
        }
    },

    getNormaByEditarAsync: function (normaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Normas/GetNormaByEditarAsync',
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
            swal.fire("Oops!", "Error en el método getNormaByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos  
    guardarNormaAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntNormaID": $("#normaID").val(),
                        "StrCodigo": $("#normaCodigo").val(),
                        "StrDescripcion": $("#normaDescripcion").val(),
                        "IntTipoCriterio": $("#normaCriterio").val(),
                        "BitActivo": $("#normaActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Normas/GuardarNormaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {                               

                                if (aplicar)
                                    return normasCRUD.getNormaByEditarAsync(respuesta.normaID);

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

                swal.fire("Oops!", "Error en el método crearNormaAsync \n" + ex, "error");
            }
        });
    },

    deleteNormaAsync: function (normaID) {
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
                        url: rootHost + 'Normas/DeleteNormaAsync',
                        data: { normaID: normaID },
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

            swal("Oops!", "Error en el método deleteNormaAsync \n" + ex, "error");
        }
    }
};
