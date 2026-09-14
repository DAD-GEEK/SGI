var empleadosCRUD = {

    commonEmpleados: function () {
        'use strict'

        var btnCrearEmpleado = $("#btnCrearEmpleado");
        var nombreEmpleado = $(".nombreEmpleado");
        var btnModalArchivos = $("#btnModalArchivos");

        btnCrearEmpleado.click(function () {
            empleadosCRUD.crearEmpleado();
        });

        btnModalArchivos.click(function () {
            empleadosCRUD.abrirVistaCargarArchivos();
        });

        nombreEmpleado.keyup(function () {

            var primerApellido = $("#empleadoPrimerApellido").val().toUpperCase();
            var segundoApellido = $("#empleadoSegundoApellido").val().toUpperCase();
            var primerNombre = $("#empleadoPrimerNombre").val().toUpperCase();
            var segundoNombre = $("#empleadoSegundoNombre").val().toUpperCase();

            $("#empleadoLabelNombre").html(primerApellido + " " + segundoApellido + " " + primerNombre + " " + segundoNombre);

        });

        $('#empleadoFechaNacimiento').daterangepicker({
            "autoUpdateInput": false,
            "singleDatePicker": true,
            "showDropdowns": true,
            "minYear": 1901,
            "maxYear": parseInt(moment().format('YYYY'), 10),
            "locale": {
                "format": "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Guardar",
                "cancelLabel": "Cancelar",
                "fromLabel": "Desde",
                "toLabel": "Hasta",
                "customRangeLabel": "Personalizar",
                "daysOfWeek": [
                    "Do",
                    "Lu",
                    "Ma",
                    "Mi",
                    "Ju",
                    "Vi",
                    "Sa"
                ],
                "monthNames": [
                    "Enero",
                    "Febrero",
                    "Marzo",
                    "Abril",
                    "Mayo",
                    "Junio",
                    "Julio",
                    "Agosto",
                    "Setiembre",
                    "Octubre",
                    "Noviembre",
                    "Diciembre"
                ],
            },

            "opens": "center"
        });

        $('#empleadoFechaIngreso').daterangepicker({
            "autoUpdateInput": false,
            "singleDatePicker": true,
            "showDropdowns": true,
            "minYear": 1901,
            "maxYear": parseInt(moment().format('YYYY'), 10),
            "locale": {
                "format": "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Guardar",
                "cancelLabel": "Cancelar",
                "fromLabel": "Desde",
                "toLabel": "Hasta",
                "customRangeLabel": "Personalizar",
                "daysOfWeek": [
                    "Do",
                    "Lu",
                    "Ma",
                    "Mi",
                    "Ju",
                    "Vi",
                    "Sa"
                ],
                "monthNames": [
                    "Enero",
                    "Febrero",
                    "Marzo",
                    "Abril",
                    "Mayo",
                    "Junio",
                    "Julio",
                    "Agosto",
                    "Setiembre",
                    "Octubre",
                    "Noviembre",
                    "Diciembre"
                ],
            },

            "opens": "center"
        });

        $('#empleadoFechaIngreso,#empleadoFechaNacimiento').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(`${picker.startDate.format('YYYY-MM-DD')}`);
        });

        $('#empleadoFechaIngreso,#empleadoFechaNacimiento').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });

    },

    //Vistas
    getAllEmpleadosAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Empleados/GetAllEmpleadosAsync',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllEmpleados').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllEmpleadosAsync \n" + ex, "error");
        }
    },

    crearEmpleado: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Empleados/CrearEmpleado',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {
                    $('#divGetAllEmpleados').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método crearEmpleado \n" + ex, "error");
        }
    },

    getEmpleadoByEditarAsync: function (EmpleadoID) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Empleados/GetEmpleadoByEditarAsync',
                data: { EmpleadoID: EmpleadoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {
                    $('#divGetAllEmpleados').empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getEmpleadoByEditarAsync \n" + ex, "error");
        }
    },

    abrirVistaCargarArchivos: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Empleados/AbrirVistaCargarArchivos',
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
            swal.fire("Oops!", "Error en el método abrirVistaCargarArchivos \n" + ex, "error");
        }
    },

    //Base de datos
    guardarEmpleadoAsync: function () {
        'use strict';

        try {

            $(".btnGuardarRegistro").click(function (e) {

                var aplicar = $(this).data("aplicar");

                e.preventDefault();

                var form = $("#formGuardarGlobal");

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    if ($("#empleadoGeneroMasculino").prop('checked') == false && $("#empleadoGeneroFemenino").prop('checked') == false)
                        return swal.fire("¡Alerta!", "Debe indicar el género del empleado. Los datos no serán guardados.", "warning");

                    var genero = 'F';
                    if ($("#empleadoGeneroMasculino").prop('checked') == true) genero = 'M';

                    var modelo = {

                        'IntEmpleadoID': $('#empleadoID').val(),
                        'StrIdentificacion': $('#empleadoIdentificacion').val(),
                        'IntTipoIdentificacion': $('#empleadoTipoIdentificacion').val(),
                        'StrPrimerApellido': $('#empleadoPrimerApellido').val(),
                        'StrSegundoApellido': $('#empleadoSegundoApellido').val(),
                        'StrPrimerNombre': $('#empleadoPrimerNombre').val(),
                        'StrSegundoNombre': $('#empleadoSegundoNombre').val(),
                        'StrRutaImagen': $("#empleadoNombreImagen").val(),
                        'StrGenero': genero,
                        'StrNacionalidad': $('#empleadoNacionalidad').val(),
                        'IntCentroDeTrabajo': $('#empleadoCentroDeTrabajo').val(),
                        'DatFechaNacimiento': $('#empleadoFechaNacimiento').val(),
                        'DatFechaIngreso': $('#empleadoFechaIngreso').val(),
                        'IntCiudadNacimiento': $('#empleadoCiudadNacimiento').val(),
                        'IntCiudadLabora': $('#empleadoCiudadLaboral').val(),
                        'IntCiudadResidencia': $('#empleadoCiudadResidencia').val(),
                        'StrDireccionResidencia': $('#empleadoDireccion').val(),
                        'StrTelefono': $('#empleadoTelefonoFijo').val(),
                        'StrCelular': $('#empleadoTelefonoMovil').val(),
                        'StrEmail': $('#empleadoMail').val(),
                        'IntAreaID': $('#empleadoArea').val(),
                        'IntProcesoID': $('#empleadoProceso').val(),
                        'IntCargo': $('#empleadoCargo').val(),
                        'IntTurno': $('#empleadoTurno').val(),
                        'IntARL': $('#empleadoARL').val(),
                        'IntEPS': $('#empleadoEPS').val(),
                        'IntAFP': $('#empleadoAFP').val(),
                        'IntTipoContrato': $('#empleadoTipoContrato').val(),
                        'IntSalario': ($("#empleadoSalario").val().replace(/,/gi, '')).replace("$", ""),
                        'IntEscolaridad': $('#empleadoEscolaridad').val(),
                        'IntEstadoCivil': $('#empleadoEstadoCivil').val(),
                        'TIntNumeroHijos': $('#empleadoNumeroHijos').val(),
                        'StrPersonasACargo': $('#empleadoPersonasACargo').val(),
                        'TIntEstrato': $('#empleadoEstrato').val(),
                        'StrGrupoSanguineo': $('#empleadoGrupoSanguineo').val(),
                        'StrCondiciones': $('#empleadoCondicionesDeSalud').val(),
                        'StrAlergias': $('#empleadoAlergias').val(),
                        'StrMedicinas': $('#empleadoMedicinas').val(),
                        'StrContacto': $('#empleadoContactos').val(),
                        'StrParentesco': $('#empleadoParentesco').val(),
                        'StrTelefonoContacto': $('#empleadoTelefonoContacto').val(),
                        'BitActivo': $("#empleadoActivo").is(":checked")
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    console.log(modelo);

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Empleados/GuardarEmpleadoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                var imagenEmpleado = $("#empleadoImagen");

                                if ($("#vistaEditar").val() == 'True')
                                    var imagenEmpleado = $("#empleadoImagenEditar");

                                empleadosCRUD.guardarImagenAsync(imagenEmpleado, respuesta.empleadoID);

                                if (aplicar) {
                                    toastr.success('El registro se guardó correctamente.');
                                    return empleadosCRUD.getEmpleadoByEditarAsync(respuesta.empleadoID);
                                }

                                empleadosCRUD.getAllEmpleadosAsync();
                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }

            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método guardarEmpleadoAsync \n" + ex, "error");
        }
    },

    guardarImagenAsync: function (elementoFile, empleadoID) {
        'use strict'

        try {

            var formData = new FormData();
            formData.append('image', elementoFile[0].files[0]);
            formData.append('empleadoID', empleadoID);
            formData.append('empleadoIdentificacion', $("#empleadoIdentificacion").val());

            $.ajax({
                type: "POST",
                url: rootHost + 'Empleados/GuardarImagenAsync',
                data: formData,
                async: false,
                dataType: 'JSON',
                contentType: false,
                processData: false,
                success: function (response) {

                    if (response.msn === "success") {

                        //$("#empleadoImagenEditar").fileinput('clear');

                    } else {
                        swal.fire("Oops!", response.error, "error");
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

    deleteEmpleadoAsync: function (EmpleadoID) {
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
                        url: rootHost + 'Empleados/DeleteEmpleadoAsync',
                        data: { EmpleadoID: EmpleadoID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                empleadosCRUD.getAllEmpleadosAsync();
                                swal.fire("¡Notificación!", "El registro se eliminó correctamente.", "success");
                            } else {
                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                } else swal.fire("¡Notificación!", "El registro no fue eliminado.", "warning");

            });

        } catch (ex) {

            swal("Oops!", "Error en el método deleteEmpleadoAsync \n" + ex, "error");
        }
    },
};

