var areasCRUD = {

    //Vistas
    getAllAreasAsync: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Areas/GetAllAreasAsync',
                data: { terceroID: terceroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAreas').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllAreasAsync \n" + ex, "error");
        }
    },

    crearArea: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Areas/CrearArea',
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

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearArea \n" + ex, "error");
        }
    },

    getAreaByEditarAsync: function (areaID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Areas/GetAreaByEditarAsync',
                data: { areaID: areaID },
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
            swal.fire("Oops!", "Error en el método getAreaByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos

    guardarAreaAsync: function () {
        'use strict';

        $(".btnGuardarRegistroModal").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobalAR");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntAreaID": $("#areaID").val(),
                        "StrCodigo": $("#areaCodigo").val(),
                        "StrDescripcion": $("#areaDescripcion").val(),
                        "IntTerceroID": $("#terceroID").val(),
                        "BitActivo": $("#areaActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Areas/GuardarAreaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var terceroID = $("#terceroID").val();

                                areasCRUD.getAllAreasAsync(terceroID);

                                if (aplicar) {
                                    areasCRUD.getAreaByEditarAsync(respuesta.areaID);
                                    return toastr.success('El registro se guardó correctamente.');
                                }

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                $('#modal').modal('hide');
                                $('#contenedor').empty();

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

                swal.fire("Oops!", "Error en el método guardarAreaAsync \n" + ex, "error");
            }
        });
    },

    deleteAreaAsync: function (areaID) {
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
                        url: rootHost + 'Areas/DeleteAreaAsync',
                        data: { areaID: areaID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            var terceroID = $("#terceroID").val();
                            areasCRUD.getAllAreasAsync(terceroID);

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteAreaAsync \n" + ex, "error");
        }
    }
};
