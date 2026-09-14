var sistemasDeGestionCRUD = {

    //Vistas
    getAll: function () {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'SistemasDeGestion/GetAll',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divSistemasDeGestion').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAll \n" + ex, "error");
        }
    },

    getAllPorTercero: function () {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'SistemasDeGestion/GetAllPorTercero',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divSistemasDeGestion').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllPorTercero \n" + ex, "error");
        }
    },

    crear: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'SistemasDeGestion/Crear',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divSistemasDeGestion').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crear \n" + ex, "error");
        }
    },

    getByEditar: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'SistemasDeGestion/GetByEditar',
                data: { registroID: registroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divSistemasDeGestion').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getByEditar \n" + ex, "error");
        }
    },

    getInformacionPorSistema: function (sistemaDeGestion) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'SistemasDeGestion/GetInformacionPorSistema',
                data: { sistemaDeGestion: sistemaDeGestion },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divSistemasDeGestion').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getInformacionPorSistema \n" + ex, "error");
        }
    },



    //Base de datos
    guardar: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrCodigoID": $("#sistemaDeGestionID").val(),
                        "StrDescripcion": $("#sistemaDeGestionCodigo").val(),
                        "StrIcono": $("#sistemaDeGestionIcono").val()

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'SistemasDeGestion/Guardar',
                        data: { __RequestVerificationToken: token, modelo: modelo, tipoDeAccion: $("#sistemaDeGestionTipoDeAccion").val() },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                if (aplicar) {
                                    toastr.success('El registro se guardó correctamente.');
                                    return sistemasDeGestionCRUD.getByEditar(respuesta.registroID);
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                sistemasDeGestionCRUD.getAll();
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

                swal.fire("Oops!", "Error en el método guardar \n" + ex, "error");
            }
        });
    },

    delete: function (registroID) {
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
                        url: rootHost + 'SistemasDeGestion/Delete',
                        data: { registroID: registroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                sistemasDeGestionCRUD.getAll();
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

            swal("Oops!", "Error en el método delete \n" + ex, "error");
        }
    },

};
