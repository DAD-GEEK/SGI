var centrosDeTrabajoCRUD = {

    //Vistas
    getAllCentrosDeTrabajoAsync: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'CentrosDeTrabajo/GetAllCentrosDeTrabajoAsync',
                data: { terceroID : terceroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetCentrosDeTrabajo').empty().html(respuesta);
                  
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllCentrosDeTrabajoAsync \n" + ex, "error");
        }
    },

    crearCentroDeTrabajo: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'CentrosDeTrabajo/CrearCentroDeTrabajo',
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
            swal.fire("Oops!", "Error en el método crearCentroDeTrabajo \n" + ex, "error");
        }
    },

    getCentroDeTrabajoByEditarAsync: function (centroDeTrabajoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'CentrosDeTrabajo/GetCentroDeTrabajoByEditarAsync',
                data: { centroDeTrabajoID: centroDeTrabajoID },
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
            swal.fire("Oops!", "Error en el método getCentroDeTrabajoByEditarAsync \n" + ex, "error");
        }
    },

    //Base de datos
    
    guardarCentroDeTrabajoAsync: function () {
        'use strict';

        $(".btnGuardarRegistroModal").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobalCT");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "IntCentroDeTrabajoID": $("#centroDeTrabajoID").val(),
                        "StrCodigo": $("#centroDeTrabajoCodigo").val(),
                        "StrDescripcion": $("#centroDeTrabajoDescripcion").val(),
                        "IntTerceroID": $("#terceroID").val(),
                        "BitActivo": $("#centroDeTrabajoActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'CentrosDeTrabajo/GuardarCentroDeTrabajoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var terceroID = $("#terceroID").val();
                                centrosDeTrabajoCRUD.getAllCentrosDeTrabajoAsync(terceroID);

                                if (aplicar)
                                    return toastr.success('El registro se guardó correctamente.');

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

                swal.fire("Oops!", "Error en el método guardarCentroDeTrabajoAsync \n" + ex, "error");
            }
        });
    },

    deleteCentroDeTrabajoAsync: function (centroDeTrabajoID) {
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
                        url: rootHost + 'CentrosDeTrabajo/DeleteCentroDeTrabajoAsync',
                        data: { centroDeTrabajoID: centroDeTrabajoID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                            var terceroID = $("#terceroID").val();
                            centrosDeTrabajoCRUD.getAllCentrosDeTrabajoAsync(terceroID);

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteCentroDeTrabajoAsync \n" + ex, "error");
        }
    }
};
