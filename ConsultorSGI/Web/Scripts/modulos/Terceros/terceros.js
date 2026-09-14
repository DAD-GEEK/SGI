var tercerosCRUD = {

    index: function () {

        $("#terceroIdentificacion").keyup(function () {
            var valor = $(this).val();
            var digitoDeVerificacion = funcionesGlobales.calcularDigitoDeVerificacion(valor);

            $("#terceroDV").val(digitoDeVerificacion);
        });

    },

    //Vistas
    getAllTerceros: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/GetAllTerceros',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divVistaParcialTerceros').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método GetAllTerceros \n" + ex, "error");
        }
    },

    crearTercero: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/CrearTercero',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divVistaParcialTerceros').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearTercero \n" + ex, "error");
        }
    },

    getTerceroByEditarAsync: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/GetTerceroByEditar',
                data: { terceroID: terceroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divVistaParcialTerceros').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getTerceroByEditar \n" + ex, "error");
        }
    },

    vistaObtenerPermisosPorTerceroAsync: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/ObtenerPermisosPorTerceroAsync',
                data: { terceroID: terceroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divPermisosTercero').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método vistaObtenerPermisosPorTerceroAsync \n" + ex, "error");
        }
    },

    vistaObtenerNormasPorTerceroAsync: function (terceroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/ObtenerNormasPorTerceroAsync',
                data: { terceroID: terceroID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divNormasTercero').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método vistaObtenerNormasPorTerceroAsync \n" + ex, "error");
        }
    },

    vistaAgregarRoles: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/VistaAgregarRolesAsync',
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
            swal.fire("Oops!", "Error en el método vistaAgregarRoles \n" + ex, "error");
        }
    },

    vistaAgregarNormas: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/VistaAgregarNormasAsync',
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
            swal.fire("Oops!", "Error en el método vistaAgregarNormas \n" + ex, "error");
        }
    },

    vistaInformacionGeneralAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/VistaInformacionGeneralAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divVistaParcialMiEmpresa').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método vistaInformacionGeneralAsync \n" + ex, "error");
        }
    },

    //Base de datos    
    guardarTerceroAsync: function (terceroID) {
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

                        "IntTerceroID": terceroID,
                        "StrIdentificacion": $("#terceroIdentificacion").val(),
                        "IntDV": $("#terceroDV").val(),
                        "StrNombre": ($("#terceroRazonSocial").val()).toUpperCase(),
                        "StrSigla": $("#terceroSigla").val(),
                        "StrEmail": ($("#terceroMail").val()).toLowerCase(),
                        "StrTelefono": $("#terceroTelefono").val(),
                        "DatFechaIngreso": new Date(),
                        "StrDireccion": $("#terceroDireccion").val(),
                        "IntCiudadID": $("#terceroCiudad").val(),
                        "StrSede": $("#terceroSede").val(),
                        "StrHabilitacion": $("#terceroResolucion").val(),
                        "StrActividadEconomica": $("#terceroActividadEconomica").val(),
                        "StrRepresentanteLegal": $("#terceroRepresentanteLegal").val(),
                        "OpcEstado": $("#terceroActivo").is(':checked'),
                        'IntNumeroEmpleados': $("#terceroNumeroDeEmpleados").val(),
                        'IntContratistasConductores': $("#terceroEmpleadosContratistas").val(),
                        'IntNumeroDeVehiculos': $("#terceroNumeroDeVehiculos").val()

                    };


                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Terceros/GuardarTerceroAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var imagen = $("#terceroLogo");

                                if (imagen.val() != "")
                                    tercerosCRUD.guardarImagenAsync(imagen, respuesta.terceroID);

                                seguridad.asignarRolesPorDefectoPorTerceros(terceroID);

                                if (aplicar) {
                                    toastr.success('El registro se guardó correctamente.');
                                    return tercerosCRUD.getTerceroByEditarAsync(respuesta.terceroID);
                                }

                                swal.fire("¡Notificación!", "El tercero se guardó correctamente.", "success");
                                tercerosCRUD.getAllTerceros();

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

                swal.fire("Oops!", "Error en el método guardarTerceroAsync \n" + ex, "error");
            }
        });
    },

    guardarInformacionTerceroDocumentoDiagnostico: function () {
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

                        "IntTerceroID": $("#documentoTerceroID").val(),
                        'IntNumeroEmpleados': $("#terceroNumeroDeEmpleados").val(),
                        'IntContratistasConductores': $("#terceroEmpleadosContratistas").val(),
                        'IntNumeroDeVehiculos': $("#terceroNumeroDeVehiculos").val()
                    };


                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Terceros/GuardarInformacionTerceroDocumentoDiagnostico',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                swal.fire("¡Notificación!", "La información se actualizó correctamente.", "success");
                                tercerosCRUD.getAllTerceros();

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

                swal.fire("Oops!", "Error en el método guardarInformacionTerceroDocumentoDiagnostico \n" + ex, "error");
            }
        });
    },

    deleteTerceroAsync: function (terceroID) {
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
                        url: rootHost + 'Terceros/DeleteTerceroAsync',
                        data: { terceroID: terceroID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                tercerosCRUD.getAllTerceros();
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else
                                swal.fire("¡Novedad!", respuesta.error, "danger");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteTerceroAsync \n" + ex, "error");
        }
    },

    eliminarRolPorTerceroAsync: function (registroID) {
        'use strict';

        try {

            var token = $('input[name="__RequestVerificationToken"]').val();

            $.ajax({
                type: "POST",
                url: rootHost + 'Seguridad/EliminarRolPorTerceroAsync',
                data: { __RequestVerificationToken: token, registroID: registroID },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success")
                        tercerosCRUD.vistaObtenerPermisosPorTerceroAsync($("#terceroID").val());
                    else
                        swal.fire("¡Alerta!", respuesta.error, "warning");

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método eliminarRolPorTerceroAsync \n" + ex, "error");
        }
    },

    eliminarNormaPorTerceroAsync: function (registroID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/EliminarNormaPorTerceroAsync',
                data: { registroID: registroID },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success")
                        tercerosCRUD.vistaObtenerNormasPorTerceroAsync($("#terceroID").val());
                    else
                        swal.fire("¡Alerta!", respuesta.error, "warning");

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal("Oops!", "Error en el método eliminarNormaPorTerceroAsync \n" + ex, "error");
        }
    },

    agregarRolesAsync: function () {
        'use strict';

        $(".btnGuardarRegistroModal").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobalModal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    //modelo Cliente
                    var modelo = {

                        "IntTerceroID": $("#terceroID").val(),
                        "IntRolID": $("#terceroRoles").val()

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Seguridad/AgregarRolPorTerceroAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                tercerosCRUD.vistaObtenerPermisosPorTerceroAsync($("#terceroID").val());

                                if (aplicar)
                                    return toastr.success('El registro se guardó correctamente.');

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success")
                                $('#modal').modal('hide');

                            } else
                                swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método agregarRolesAsync \n" + ex, "error");
            }
        });
    },

    agregarNormasAsync: function () {
        'use strict';

        $(".btnGuardarRegistroModal").click(function (e) {
            try {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobalModal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    //modelo Cliente
                    var modelo = {

                        "IntTerceroID": $("#terceroID").val(),
                        "IntNormaID": $("#terceroNorma").val()

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Terceros/AgregarNormaPorTerceroAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                tercerosCRUD.vistaObtenerNormasPorTerceroAsync($("#terceroID").val());

                                if (aplicar)
                                    return toastr.success('El registro se guardó correctamente.');

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success")
                                $('#modal').modal('hide');

                            } else
                                swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método agregarNormasAsync \n" + ex, "error");
            }
        });
    },

    guardarImagenAsync: function (elementoFile, terceroID) {
        'use strict'

        try {

            var formData = new FormData();
            formData.append('image', elementoFile[0].files[0]);
            formData.append('terceroID', terceroID);

            $.ajax({
                type: "POST",
                url: rootHost + 'Terceros/GuardarImagenAsync',
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
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método guardarImagenAsync \n" + ex, "error");
        }

    },
};

var elementosComunesTercerosUsuarios = {

    index: function () {

        $('[data-toggle="tooltip"]').tooltip();
        //Terceros
        var btnCrearTercero = $("#btnCrearTercero");
        var btnGetAllTerceros = $(".btnGetAllTerceros");
        var inputNombre = $("#terceroRazonSocial");
        var inputNit = $("#terceroIdentificacion");

        btnGetAllTerceros.click(function () {
            tercerosCRUD.getAllTerceros();
        });

        btnCrearTercero.click(function () {
            tercerosCRUD.crearTercero();
        });

        inputNombre.keyup(function () {
            $("#terceroLabelNombre").html($(this).val());
        });

        inputNit.keyup(function () {
            $("#terceroLabelNit").html($(this).val());
        });

        //Usuarios
        var btnAbrirModalUsuario = $("#btnAbrirModalUsuario");
        var btnCrearUsuario = $("#btnCrearUsuario");
        var btnEditarUsuario = $("#btnEditarUsuario");

        btnAbrirModalUsuario.click(function () {

            var tipoVista = $("#tipoVista");

            if (tipoVista.val() === 'editarTercero') return usuariosCRUD.crearUsuario();
            else return swal.fire("¡Alerta!", "Para crear un usuario primero debe guardar los datos del tercero.", "warning");

        });

        btnCrearUsuario.click(function () {
            usuariosCRUD.crearUsuarioAsync();
        });

        btnEditarUsuario.click(function () {
            usuariosCRUD.updateUsuarioAsync();
        });
    },

}