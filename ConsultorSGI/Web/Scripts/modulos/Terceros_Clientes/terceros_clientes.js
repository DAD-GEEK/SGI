var terceros_clientesCRUD = {

    index: function () {

        var terceroClienteIdentificacion = $("#terceroClienteIdentificacion");

        terceroClienteIdentificacion.keyup(function () {

            var valor = $(this).val();
            var digitoDeVerificacion = funcionesGlobales.calcularDigitoDeVerificacion(valor);

            $("#terceroClienteDV").val(digitoDeVerificacion);
        });

    },

    //Vistas
    getAll: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros_Clientes/GetAll',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetTerceros_Clientes').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAll \n" + ex, "error");
        }
    },

    crear: function () {
        'use strict';

        try {

            $("#btnCrearCliente").click(function () {
                $.ajax({
                    type: "POST",
                    url: rootHost + 'Terceros_Clientes/Crear',
                    data: {},
                    dataType: "html",
                    async: true,
                    success: function (respuesta) {

                        $('#divGetTerceros_Clientes').empty().html(respuesta);

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        swal.fire("Oops!", textStatus, "error");
                    }
                });
            });


        } catch (ex) {
            swal.fire("Oops!", "Error en el método crear \n" + ex, "error");
        }
    },

    getByEditar: function (id) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros_Clientes/GetByEditar',
                data: { id: id },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetTerceros_Clientes').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getByEditar \n" + ex, "error");
        }
    },

    //Base de datos    
    guardarAsync: function () {
        'use strict';

        $(".btnGuardarRegistro").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    //modelo Cliente
                    var modelo = {

                        "IntTerceroClienteID": $("#terceroClienteID").val(),
                        "StrIdentificacion": $("#terceroClienteIdentificacion").val(),
                        "IntDV": $("#terceroClienteDV").val(),
                        "StrNombre": ($("#terceroClienteRazonSocial").val()).toUpperCase(),
                        "StrEmail": ($("#terceroClienteMail").val()).toLowerCase(),
                        "StrTelefono": $("#terceroClienteTelefono").val(),
                        "StrDireccion": $("#terceroClienteDireccion").val(),
                        "IntCiudadID": $("#terceroClienteCiudad").val(),
                        "StrRepresentanteLegal": $("#terceroClienteRepresentanteLegal").val(),
                        "StrPaginaWeb": $("#terceroClientePaginaWeb").val(),
                        "BitEstado": $("#terceroClienteActivo").is(':checked'),

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Terceros_Clientes/GuardarAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var imagen = $("#terceroLogo");

                                if (imagen.val() != "")
                                    terceros_clientesCRUD.guardarImagenAsync(imagen, respuesta.terceroClienteID);

                                if (aplicar) {
                                    toastr.success('El registro se guardó correctamente.');
                                    return terceros_clientesCRUD.getTerceroByEditarAsync(respuesta.terceroID);
                                }

                                swal.fire("¡Notificación!", "El tercero se guardó correctamente.", "success");
                                terceros_clientesCRUD.getAll();

                            } else {
                                form.parsley().destroy();
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }
                        },
                        error: function (respuesta) {
                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método guardarAsync \n" + ex, "error");
            }
        });
    },

    deleteAsync: function (id) {
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
                        url: rootHost + 'Terceros_Clientes/DeleteAsync',
                        data: { id: id },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                terceros_clientesCRUD.getAll();
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else
                                swal.fire("¡Oops!", respuesta.error, "error");

                        },
                        error: function (respuesta) {

                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });
                } else swal.fire("¡Oops!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("¡Oops!", "Error en el método deleteAsync \n" + ex, "error");
        }
    },

    guardarImagenAsync: function (elementoFile, terceroClienteID) {
        'use strict'

        try {

            var formData = new FormData();
            formData.append('image', elementoFile[0].files[0]);
            formData.append('terceroClienteID', terceroClienteID);

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros_Clientes/GuardarImagenAsync',
                data: formData,
                async: true,
                dataType: 'JSON',
                contentType: false,
                processData: false,
                success: function (response) {

                    if (response.msn === "success") {

                    } else {
                        toastr.error(response.error);
                    }
                },
                error: function (ex) {

                    swal.fire("Oops!", ex, "error");
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método guardarImagenAsync \n" + ex, "error");
        }

    },
};
