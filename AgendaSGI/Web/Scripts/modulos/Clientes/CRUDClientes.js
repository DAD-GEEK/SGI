var clientesCRUD = {

    getAllClientes: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Clientes/GetAllClientes',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllClientes').html(respuesta);
                },
                error: function (textStatus) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método GetAllTerceros \n" + ex, "error");
        }
    },

    loadInfoVistas: function () {
        'use strict';
        try {

            //Inicializar Select2
            $(".select2").select2({
                placeholder: '-- Selecccione una opción --',
                theme: 'default'
            });

            $(".select2bs4").select2({
                placeholder: '-- Selecccione una opción --',
                theme: 'bootstrap4'
            });

            $("#procesosAuditoria").select2({
                placeholder: '-- Selecccione una opción --',
                theme: 'bootstrap4'
            });

            //Agregar el nomobre del proceso al input de Alias
            $("#procesosAuditoria").change(function () {

                var alias = $("#procesosAlias");
                alias.val($("#procesosAuditoria option:selected").text());

            });            

            //Reiniciar los contratos eliminados
            $("#contratosDeleted > li").each(function (i, li) {
                $(this).remove();
            });

            //Dar formato al input de identificación
            var nit = $("#clienteIdentificacion").val();
            nit = parseInt((nit.replace(/\./gi, '')));
            $("#clienteIdentificacion").val(clientesCRUD.formatearNumeros(nit, "punto"));

            //Formatear valor como tipo moneda
            $("#valorContrato").keyup(function () {

                var valor = $(this).val();
                valor = parseInt((valor.replace(/,/gi, '')).replace('$', ''));

                var valorFormateado = clientesCRUD.formatearNumeros(parseInt(valor), "moneda");
                $(this).val(valorFormateado);
            });

            //Formatear Nit con separación de puntos
            $("#clienteIdentificacion").keyup(function () {

                var valor = $(this).val();
                valor = parseInt((valor.replace(/\./gi, '')));

                $("#clienteDV").val(clientesCRUD.calcularDigitoDeVerificacion(valor));

                var valorFormateado = clientesCRUD.formatearNumeros(parseInt(valor), "punto");
                $(this).val(valorFormateado);
            });

            //Inicializar fechas calendario
            $('#timepickerFechaIngreso').datetimepicker({
                format: 'YYYY-MM-DD',
                defaultDate: new Date,
                locale: 'es',
                icons: {
                    time: "fa fa-clock",
                    date: "fa fa-calendar",
                    up: "fa fa-arrow-up",
                    down: "fa fa-arrow-down"
                }
            });

            $('#timepickerFechaInicialContrato').datetimepicker({
                format: 'YYYY-MM-DD',
                locale: 'es',
                icons: {
                    time: "fa fa-clock",
                    date: "fa fa-calendar",
                    up: "fa fa-arrow-up",
                    down: "fa fa-arrow-down"
                }
            });

            $('#timepickerFechaFinalContrato').datetimepicker({
                format: 'YYYY-MM-DD',
                locale: 'es',
                icons: {
                    time: "fa fa-clock",
                    date: "fa fa-calendar",
                    up: "fa fa-arrow-up",
                    down: "fa fa-arrow-down"
                },
            });


        } catch (ex) {

            swal.fire("Oops!", "Error en el método loadInfoVistas \n" + ex, "error");
        }
    },

    crearClienteAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                if ($("#clienteEmail").val() != "") {
                    var texto = $("#clienteEmail").val();
                    var regex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;

                    if (!regex.test(texto)) {
                        swal.fire("¡Validación!", "El correo no está formado correctamente, por favor revisar.", "warning")
                        return;
                    }

                }

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var identificacion = $("#clienteIdentificacion").val();
                    identificacion = parseInt((identificacion.replace(/\./gi, '')));

                    //modelo Cliente
                    var modelo = {

                        "StrIdentificacion": identificacion,
                        "StrNombre": ($("#clienteNombre").val()).toUpperCase(),
                        "IntDV": $("#clienteDV").val(),
                        "StrDireccion": $("#clienteDireccion").val(),
                        "StrEmail": ($("#clienteEmail").val()).toLowerCase(),
                        "IntCiudadID": $("#clienteCiudad").val(),
                        "StrTelefono": $("#clienteTelefono").val(),
                        "StrPaginaWeb": $("#clienteWeb").val(),
                        "DatFechaIngreso": $("#clienteFechaIngreso").val(),
                        "OpcEstado": true 
                    };

                    //modelo Contratos
                    var i = 0;
                    var u = 0;  
                    var s = 0;
                    var listaContratos = [];
                    var listaUsuarios = [];
                    var listaSistemas = [];

                    $('#tblContratos > tbody  > tr').each(function (j, tr) {

                        var numeroContrato = $("span.iNumeroContrato", tr).html();
                        var fechaInicial = $("span.iFechaInicialContrato", tr).html();
                        var fechaFinal = $("span.iFechaFinalContrato", tr).html();
                        var horas = $("span.iHorasContrato", tr).html();
                        var valor = $("span.iValorContrato", tr).html();
                        valor = parseInt((valor.replace(/,/gi, '')).replace('$', ''));
                        var email = $("span.iEmailContrato", tr).html();
                        var estado = $(".iEstado", tr).data("estado");
                      
                        var item = {
                            IntNumeroContrato: numeroContrato,
                            DatFechaInicial: fechaInicial,
                            DatFechaFinal: fechaFinal,
                            IntHoras: horas,
                            IntValor: valor,
                            StrEmail: email,
                            IntClienteID: 0,
                            OpcEstado: estado,
                        };

                        listaContratos[i] = item;
                        i++;

                        //modelo Contratos_Usuarios                    
                        $(".iUsuarioID", this).each(function (j, user) {

                            var usuarioID = $(user).html();
                            var numeroContrato = $(user).data("contrato");

                            var item = {
                                IntContratoID: numeroContrato,
                                IntUsuarioID: usuarioID,                             
                            };

                            listaUsuarios[u] = item;
                           
                            u++;
                        });  


                        //modelo Contratos_Sistemas                   
                        $(".iSistemaID", this).each(function (j, sistema) {

                            var sistemaID = $(sistema).html();
                            var numeroContrato = $(sistema).data("contrato");

                            var item = {
                                IntContratoID: numeroContrato,
                                IntSistemaID: sistemaID,
                            };

                            listaSistemas[s] = item;

                            s++;
                        });  

                    });
               
                    //modelo Contactos
                    var i = 0;                 
                    var listaContactos = [];

                    $('#tblContactos > tbody  > tr').each(function (j, tr) {

                        var contactoID = $("span.iContactoID", tr).html();
                        var nombreContacto = $("span.iNombreContacto", tr).html();
                        var cargoContacto = $("span.iCargoContacto", tr).html();
                        var telefonoContacto = $("span.iTelefonoContacto", tr).html();
                        var celularContacto = $("span.iCelularContacto", tr).html();
                        var emailContacto = $("span.iEmailContacto", tr).html();

                        var item = {
                            //IntContactoID: contactoID,
                            StrNombre: nombreContacto,
                            StrCargo: cargoContacto,
                            StrTelefonoFijo: telefonoContacto,
                            StrCelular: celularContacto,
                            StrEmail: emailContacto,
                            IntClienteID: 0,
                        };

                        listaContactos[i] = item;
                        i++;
                    });

                    //Modelo procesos
                    var i = 0;
                    var listaProcesos = [];
                    var identificacion = $("#clienteIdentificacion").val();
                    identificacion = parseInt((identificacion.replace(/\./gi, '')));

                    $("#divGenerarDetalleProceso > tr").each(function (i, tr) {

                        var procesoID = $("span.iProcesoID", tr).html();
                        var descripcionProceso = $("input.iProcesoNombre", tr).val();

                        var item = {

                            IntClienteID: identificacion,
                            IntProcesoID: procesoID,
                            StrAlias: descripcionProceso
                        }

                        listaProcesos[i] = item;
                        i++;

                    });

                    //Convertir modelos a JSON
                    var modeloUsuarios = JSON.stringify(listaUsuarios);
                    var modeloContratos = JSON.stringify(listaContratos);
                    var modeloSistemas = JSON.stringify(listaSistemas);
                    var modeloContactos = JSON.stringify(listaContactos);
                    var modeloProcesos = JSON.stringify(listaProcesos);

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Clientes/CrearClienteAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, modeloContratos: modeloContratos, modeloUsuarios, modeloUsuarios, modeloSistemas: modeloSistemas, modeloContactos: modeloContactos, modeloProcesos: modeloProcesos },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                clientesCRUD.getAllClientes();

                                swal.fire("¡Notificación!", "El cliente se agregó correctamente.", "success");


                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Valicación!", respuesta.error, "warning");

                            }
                        },
                        error: function (ex) {

                            swal.fire("Oops!", ex, "error");
                        }
                    });
                } else {
                    swal.fire("¡Validación!", "Debe diligenciar todos los campos obligatorios. Valide en todas las pestañas los campos resaltados de rojo y agregue información.", "info");
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearClienteAsync \n" + ex, "error");
            }
        });
    },

    getAllContratos: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Clientes/GetAllContratos',
                data: { clienteID: $("#clienteID").val() },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllContratos').html(respuesta);
                },
                error: function (textStatus) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método GetAllTerceros \n" + ex, "error");
        }
    },

    obtenerContratosbyCliente: function () {
        'use strict'
        try {
            if ($("#vistaEditarContratos").val() === "True") {

                $.ajax({
                    type: "POST",
                    url: rootHost + 'Clientes/GetAllContratosByEditar',
                    data: { clienteID: $("#clienteID").val() },
                    dataType: "html",
                    async: true,
                    success: function (newtr) {

                        if (newtr != "0") {

                            $('#divGenerarDetalleContrato').append(newtr);

                            $("#divGenerarDetalleContrato").each(function (i, tr) {
                                var valor = $(".iValorContrato", tr).html();
                                $(".iValorContrato", tr).html(clientesCRUD.formatearNumeros(valor, "moneda"));
                            });

                            $('.remove-item').off().click(function (e) {
                                $(this).parent('td').parent('tr').remove();

                                $("#modificContrato").val("True");

                                $("#divGenerarDetalleContrato > tr").each(function (i, tr) {

                                    $(".iNumeroContrato", tr).html(i + 1);
                                    $("#numeroContrato").val(i + 1);
                                });
                            });

                            $("#vistaEditarContratos").val("False");
                        }
                    },
                    error: function (textStatus) {
                        swal.fire("Oops!", textStatus, "error");
                    }
                });
            }

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerContratosbyCliente \n" + ex, "error");
        }

    },

    getAllContactos: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Clientes/GetAllContactos',
                data: { clienteID: $("#clienteID").val() },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllContactos').html(respuesta);                   
                },
                error: function (textStatus) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllContactos \n" + ex, "error");
        }
    },

    obtenerContactosByCliente: function () {
        'use strict'
        try {
            if ($("#vistaEditarContactos").val() === "True") {

                $.ajax({
                    type: "POST",
                    url: rootHost + 'Clientes/GetAllContactosByEditar',
                    data: { clienteID: $("#clienteID").val() },
                    dataType: "html",
                    async: true,
                    success: function (newtr) {

                        $('#divGenerarDetalleContacto').append(newtr);

                        $('.remove-item').off().click(function (e) {
                            $(this).parent('td').parent('tr').remove();
                        });

                        $("#vistaEditarContactos").val("False");
                    },
                    error: function (textStatus) {
                        swal.fire("Oops!", textStatus, "error");
                    }
                });
            }
        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerContactosByCliente \n" + ex, "error");
        }

    },

    getAllProcesos: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Clientes/GetAllProcesos',
                data: { clienteID: $("#clienteID").val() },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllProcesos').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllProcesos \n" + ex, "error");
        }
    },

    generarDetalleProcesos: function () {

        'use strict';

        try {

            var procesoText = $("#procesosAuditoria option:selected").text();
            var proceso = $("#procesosAuditoria");
            var procesoAlias = $("#procesosAlias").val();

            //Validaciones
            if (procesoAlias.length == 0) return swal.fire("¡Notificación!", "El campo Alias no puede estar vacío.", "warning");
            if (proceso.val() == null) return swal.fire("¡Notificación!", "Debe seleccionar un proceso.", "warning")
            if (proceso.val().length == "") return swal.fire("¡Notificación!", "Debe seleccionar un proceso.", "warning");

            var duplicado = 0;
            $("#divGenerarDetalleProceso > tr").each(function (i, tr) {

                var id = $(tr).find(".iProcesoID").html();
                if (id == proceso.val()) duplicado = 1;
                
            });

            if (duplicado == 1) return swal.fire("¡Notificación!", "El proceso ya fue seleccionado.", "warning");
            //Fin Validaciones

            //Crear detalleContrato
            var newtr = '<tr class="item iRegistroID" data-registroid="0">';
            newtr = newtr + '<td style="display:none!important" class"d-none"><span class="iProcesoID">' + proceso.val() + '</span></td>';
            newtr = newtr + '<td class="pt-0 pb-0"><span class="form-control font-weight-bold border-0 ml-0 pl-0 iProceso">' + procesoText + '</span></td>';
            newtr = newtr + '<td class="pt-0 pb-0"><input class="form-control border-0 ml-0 pl-0 letras iProcesoNombre" type="text" style="width:100%!important" value="' + procesoAlias + '" maxlength="100" /></td>';
            newtr = newtr + '<td class="pt-0 pb-0 text-right"><button type="button" class="btn bg-white remove-item"><i class="fa fa-trash-alt fa-1x text-danger"></i></button></td></tr>';

            $('#divGenerarDetalleProceso').append(newtr);

            //Limpiar los input del proceso
            $(".proceso").each(function (i, elemento) {
                $(elemento).val("");
            });
            $('#procesosAuditoria').val(0).trigger('change.select2');

            //Eliminar el detalle
            $('.remove-item').off().click(function (e) {
                $(this).parent('td').parent('tr').remove();
            });


        } catch (ex) {

            swal.fire("¡Error!", "Error en el método generarDetalleProcesos \n" + ex, "error");
        }
    },

    getClienteParaEditarAsync: function (id) {
        'use strict';

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Clientes/GetClienteParaEditarAsync',
                data: "{ clienteID: '" + $(id).val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                //async: true,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);

                    $('#modal').modal('show');

                },
                error: function (respuesta) {

                    swal.fire("Oops!", JSON.stringify(respuesta), "error");
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método getClienteParaEditarAsync \n" + ex, "error");
        }
    },

    generarDetalleContratos: function () {

        'use strict';

        try {

            $("#modificContrato").val("True");

            var bandera = 1

            //Validar input sin datos del contrato
            $(".contrato").each(function (i, elemento) {

                if (($(elemento).val()).length == 0) {
                    
                    bandera = 0
                    return;
                } else {

                    var texto = $("#clienteEmail").val();
                    var regex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;

                    if (!regex.test(texto)) {

                        bandera = 2;
                        return;
                    }                                                          
                }
            });

            if (bandera == 1) {
                //Obtener el número de contrato
                var numeroContrato = 0;
              
                $("#divGenerarDetalleContrato > tr").each(function (i, filas) {
                    numeroContrato++;
                });

                numeroContrato = numeroContrato + 1;

                $("#numeroContrato").val(numeroContrato);

                var contratoID = $("#contratoID").val();
                var fechaInicial = $("#fechaInicialContrato").val();
                var fechaFinal = $("#fechaFinalContrato").val();
                var horas = $("#horasContrato").val();
                var valor = $("#valorContrato").val();
                var email = $("#clienteEmail").val();//Email ppal del cliente

                if (Date.parse(fechaInicial) > Date.parse(fechaFinal)) {
                    swal.fire("¡Validación!", "La fecha final debe ser mayor a la fecha inicial", "info");
                    return;
                }
                
                //Agregar al detalle los datos del select Usuarios
                var selectObject = document.getElementById("asesorasContrato");
                var tdUsuariosID = "";
                var tdUsuariosDescripcion = "";
                for (var i = 0; i < selectObject.options.length; i++) {
                    if (selectObject.options[i].selected == true) {
                        tdUsuariosID = tdUsuariosID + "<span class ='iUsuarioID' data-contrato='" + numeroContrato + "'>" + selectObject.options[i].value + '</span>'
                        tdUsuariosDescripcion = tdUsuariosDescripcion + "<span class='badge badge-primary mr-1'>" + selectObject.options[i].text + '</span>';
                    }
                }

                ////Agregar al detalle los datos del select Sistemas
                var selectObject = document.getElementById("sistemasContrato");
                var tdSistemasID = "";
                var tdSistemasDescripcion = "";
                for (var i = 0; i < selectObject.options.length; i++) {
                    if (selectObject.options[i].selected == true) {
                        tdSistemasID = tdSistemasID + "<span class ='iSistemaID' data-contrato='" + numeroContrato + "'>" + selectObject.options[i].value + '</span>'
                        tdSistemasDescripcion = tdSistemasDescripcion + "<span class='badge badge-primary mr-1'>" + selectObject.options[i].text + '</span>';
                    }
                }

                //Inactivar todos los contratos
                clientesCRUD.inactivarContratosAnteriores();

                //Crear detalleContrato
                var newtr = '<tr class="item" data-numerocontrato = "' + numeroContrato + '" data-id="' + contratoID + '">';

                newtr = newtr + '<td style="text-align:center; display:none;"><span class="iContratoID">' + contratoID + '</span></td>';
                newtr = newtr + '<td class="font-weight-bold"><span class="iNumeroContrato" data-contratobd="' + numeroContrato + '" >' + numeroContrato + '</span></td>';
                newtr = newtr + '<td class=""><span class="iFechaInicialContrato">' + fechaInicial + '</span></td>';
                newtr = newtr + '<td class=""><span class="iFechaFinalContrato">' + fechaFinal + '</span></td>';
                newtr = newtr + '<td class=""><span class="iHorasContrato">' + horas + '</span></td>';
                newtr = newtr + '<td class=""><span class="iValorContrato">' + valor + '</span></td>';
                newtr = newtr + '<td class="" id="listaUsuariosID' + numeroContrato + '" style="text-align:center; display:none;">' + tdUsuariosID + '</td>';
                newtr = newtr + '<td class="" id="listaUsuarios' + numeroContrato + '">' + tdUsuariosDescripcion + '</td>';
                newtr = newtr + '<td class="" id="listaSistemasID' + numeroContrato + '" style="text-align:center; display:none;" >' + tdSistemasID + '</td>';
                newtr = newtr + '<td class="" id="listaSistemas' + numeroContrato + '">' + tdSistemasDescripcion + '</td>';
                newtr = newtr + '<td class=""><span class="iEmailContrato">' + email + '</span></td>';
                newtr = newtr + '<td class="text-center h5"><a href="#" class="iEstado" data-contratoid = "' + contratoID + '" data-estado="True" onclick ="clientesCRUD.cambiarEstadoContrato(this)"> <span class="badge badge-success">ACTIVO</span></a></td>';
                newtr = newtr + '<td class="text-right"><button type="button" class="btn bg-white pr-0 mr-1" data-numerocontrato="' + numeroContrato + '" onclick="clientesCRUD.getContratoParaEditarAsync(this);"><i class="fa fa-edit fa-1x text-info"></i></button><button type="button" class="btn bg-white remove-item" onclick="clientesCRUD.identificarContratosDeleted(this)"><i class="fa fa-trash-alt fa-1x text-danger"></i></button></td></tr>';

                $('#divGenerarDetalleContrato').append(newtr);

                //Limpiar los input del contrato
                $(".contrato").each(function (i, elemento) {
                    $(elemento).val("");                   
                });   

                //Resetear fechas
                $("#fechaInicialContrato").val("");
                $("#fechaFinalContrato").val("");
               
                $('#sistemasContrato').val(0).trigger('change.select2');
                $('#asesorasContrato').val(0).trigger('change.select2');

            } else {
                if (bandera == 2) {
                    swal.fire("¡Validación!", "El correo principal no está bien formado. Revise por favor.", "info");
                    return;
                } 
                swal.fire("¡Validación!", "El contrato debe tener todos los datos.", "info");
            }

            //Eliminar el detalle
            $('.remove-item').off().click(function (e) {
                $(this).parent('td').parent('tr').remove();

                $("#modificContrato").val("True");

                $("#divGenerarDetalleContrato > tr").each(function (i, tr) {
                    $(".iNumeroContrato", tr).html(i + 1);
                    $("#numeroContrato").val(i + 1);
                });
            });

 
        } catch (ex) {

            swal.fire("¡Error!", "Error en el método generarDetalleContratos \n" + ex, "error");
        }
    },

    generarDetalleContactos: function () {

        'use strict';

        try {

            $("#modificContacto").val("True");
            var bandera = 1
            //Validar input sin datos del contrato
            $(".contacto").each(function (i, elemento) {

                if (($(elemento).val()).length == 0) {

                    bandera = 0
                    return;

                } else {
                    var texto = $("#emailContacto").val();
                    var regex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;

                    if (!regex.test(texto)) {

                        bandera = 2;
                        return;
                    }
                }
            });

            if (bandera == 1) {
                var contactoID = $("#contactoID").val();
                var nombre = $("#nombreContacto").val();
                var cargo = $("#cargoContacto").val();
                var telefono = $("#telefonoContacto").val();
                var celular = $("#celularContacto").val();             
                var email = $("#emailContacto").val();

                var newtr = '<tr class="item" data-id="' + contactoID + '">';

                newtr = newtr + '<td style="text-align:center; display:none;"><span class="iContactoID">' + contactoID + '</span></td>';
                newtr = newtr + '<td class=""><span class="iNombreContacto">' + nombre + '</span></td>';
                newtr = newtr + '<td class=""><span class="iCargoContacto">' + cargo + '</span></td>';
                newtr = newtr + '<td class=""><span class="iTelefonoContacto">' + telefono + '</span></td>';
                newtr = newtr + '<td class=""><span class="iCelularContacto">' + celular + '</span></td>';
                newtr = newtr + '<td class=""><span class="iEmailContacto">' + email + '</span></td>';
                newtr = newtr + '<td class="text-right"><button class="btn bg-white p-0 remove-item" onclick="clientesCRUD.identificarContactosDeleted(this)"><em class="fa fa-trash-alt fa-1x text-danger"></em> </button></td></tr>';

                $('#divGenerarDetalleContacto').append(newtr);

                //Limpiar los input del contrato
                $(".contacto").each(function (i, elemento) {

                    $(elemento).val("");
                });

            } else {
                if (bandera == 2) {
                    swal.fire("¡Validación!", "El correo no está bien formado. Revise por favor.", "info");
                    return;
                }
                swal.fire("¡Validación!", "El contacto debe tener todos los datos obligatorios.", "info");
            }

            //Eliminar el detalle
            $('.remove-item').off().click(function (e) {
                $(this).parent('td').parent('tr').remove();
            });

        } catch (ex) {

            swal.fire("¡Error!", "Error en el método generarDetalleContactos \n" + ex, "error");
        }
    },

    updateClienteAsync: function () {

        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                if ($("#clienteEmail").val() != "") {
                    var texto = $("#clienteEmail").val();
                    var regex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;

                    if (!regex.test(texto)) {
                        swal.fire("¡Validación!", "El correo no está formado correctamente, por favor revisar.", "info")
                        return;
                    }
                }                

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var identificacion = $("#clienteIdentificacion").val();
                    identificacion = parseInt((identificacion.replace(/\./gi, '')));

                    var clienteID = $("#clienteID").val();

                    //modelo Cliente
                    var modelo = {

                        "IntClienteID": clienteID,
                        "StrIdentificacion": identificacion,
                        "StrNombre": ($("#clienteNombre").val()).toUpperCase(),
                        "IntDV": $("#clienteDV").val(),
                        "StrDireccion": $("#clienteDireccion").val(),
                        "StrEmail": ($("#clienteEmail").val()).toLowerCase(),
                        "IntCiudadID": $("#clienteCiudad").val(),
                        "StrTelefono": $("#clienteTelefono").val(),
                        "StrPaginaWeb": $("#clienteWeb").val(),
                        "DatFechaIngreso": $("#clienteFechaIngreso").val(),
                        "OpcEstado": $("#opcEstado").is(':checked')
                    };

                    //modelo Contratos
                    var i = 0;
                    var u = 0;
                    var s = 0;
                    var listaContratos = [];
                    var listaUsuarios = [];
                    var listaSistemas = [];

                    $('#tblContratos > tbody  > tr').each(function (j, tr) {

                        var contratoID = $("span.iContratoID", tr).html();

                        if (contratoID == 'undefined') {
                            contratoID = 0;
                        }
                        
                        var numeroContrato = $("span.iNumeroContrato", tr).html();
                        var numeroContratoDB = $("span.iNumeroContrato", tr).data("contratobd");
                        var fechaInicial = $("span.iFechaInicialContrato", tr).html();
                        var fechaFinal = $("span.iFechaFinalContrato", tr).html();
                        var horas = $("span.iHorasContrato", tr).html();
                        var valor = $("span.iValorContrato", tr).html();
                        valor = parseInt((valor.replace(/,/gi, '')).replace('$', ''));
                        var email = $("#clienteEmail").val();
                        var estado = $("a", tr).data("estado");

                        var item = {
                            IntContratoID: contratoID,
                            IntNumeroContrato: numeroContrato,
                            IntNumeroContratoDB: numeroContratoDB,
                            DatFechaInicial: fechaInicial,
                            DatFechaFinal: fechaFinal,
                            IntHoras: horas,
                            IntValor: valor,
                            StrEmail: email,
                            IntClienteID: clienteID,
                            OpcEstado: estado,
                        };

                        listaContratos[i] = item;
                        i++;

                        //modelo Contratos_Usuarios                    
                        $(".iUsuarioID", this).each(function (j, user) {

                            var usuarioID = $(user).html();
                            var numeroContrato = $(user).data("contrato");

                            var item = {
                                IntContratoID: numeroContrato,
                                IntUsuarioID: usuarioID,
                            };

                            listaUsuarios[u] = item;

                            u++;
                        });


                        //modelo Contratos_Sistemas                   
                        $(".iSistemaID", this).each(function (j, sistema) {

                            var sistemaID = $(sistema).html();
                            var numeroContrato = $(sistema).data("contrato");

                            var item = {
                                IntContratoID: numeroContrato,
                                IntSistemaID: sistemaID,
                            };

                            listaSistemas[s] = item;

                            s++;
                        });

                    });

                    //modelo Contactos
                    var i = 0;
                    var listaContactos = [];

                    $('#tblContactos > tbody  > tr').each(function (j, tr) {

                        var contactoID = $("span.iContactoID", tr).html();

                        if (contactoID == 'undefined') {
                            contactoID = 0;
                        }

                        var nombreContacto = $("span.iNombreContacto", tr).html();
                        var cargoContacto = $("span.iCargoContacto", tr).html();
                        var telefonoContacto = $("span.iTelefonoContacto", tr).html();
                        var celularContacto = $("span.iCelularContacto", tr).html();
                        var emailContacto = $("span.iEmailContacto", tr).html();

                        var item = {
                            IntContactoID: contactoID,
                            StrNombre: nombreContacto,
                            StrCargo: cargoContacto,
                            StrTelefonoFijo: telefonoContacto,
                            StrCelular: celularContacto,
                            StrEmail: emailContacto,
                            IntClienteID: clienteID,
                        };

                        listaContactos[i] = item;
                        i++;
                    });

                    //Modelo procesos
                    var i = 0;
                    var listaProcesos = [];
                    var identificacion = $("#clienteIdentificacion").val();
                    identificacion = parseInt((identificacion.replace(/\./gi, '')));

                    var validar = 0;
                    $("#divGenerarDetalleProceso > tr").each(function (i, tr) {

                        var registroID = $(tr).data("registroid");
                        var procesoID = $("span.iProcesoID", tr).html();
                        var descripcionProceso = $("input.iProcesoNombre", tr).val().trim();

                        if (descripcionProceso.length == 0 ) validar = 1;
                     
                        var item = {

                            IntRegistroID: registroID,
                            IntClienteID: identificacion,
                            IntProcesoID: procesoID,
                            StrAlias: descripcionProceso
                        }

                        listaProcesos[i] = item;
                        i++;

                    });

                    if (validar == 1) return swal.fire("¡Notificación!", "El campo Proceso Descripción del detalle no puede estar vació", "warning");

                    //Identificar contratos eliminados
                    var i = 0;
                    var listaContratosDeleted = [];
                    $("#contratosDeleted > li").each(function (i, li) {
                        var id = $(this).html();

                        var item = {
                            IntContratoID: id
                        };

                        listaContratosDeleted[i] = item;
                        i++;
                    });

                    //Identificar contactos eliminados
                    var i = 0;
                    var listaContactosDeleted = [];
                    $("#contactosDeleted > li").each(function (i, li) {
                        var id = $(this).html();

                        var item = {
                            IntContactoID: id
                        };

                        listaContactosDeleted[i] = item;
                        i++;
                    });

                    //Identificar contactos eliminados
                    var i = 0;
                    var listaProcesosDeleted = [];
                    $("#procesosDeleted > li").each(function (i, li) {
                        var id = $(this).html();

                        var item = {
                            IntProcesoID: id
                        };

                        listaProcesosDeleted[i] = item;
                        i++;
                    });


                    //Convertir modelos a JSON
                    var modeloUsuarios = JSON.stringify(listaUsuarios);
                    var modeloContratos = JSON.stringify(listaContratos);
                    var modeloSistemas = JSON.stringify(listaSistemas);
                    var modeloContactos = JSON.stringify(listaContactos);
                    var modeloProcesos = JSON.stringify(listaProcesos);

                    var modeloContratosDeleted = JSON.stringify(listaContratosDeleted);
                    var modeloContactosDeleted = JSON.stringify(listaContactosDeleted);
                    var modeloProcesosDeleted = JSON.stringify(listaProcesosDeleted);


                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Clientes/UpdateClienteAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, modeloContratos: modeloContratos, modeloUsuarios, modeloUsuarios, modeloSistemas: modeloSistemas, modeloContactos: modeloContactos, modeloProcesos: modeloProcesos, modificContrato: $("#modificContrato").val(), modificContacto: $("#modificContacto").val(), modeloContratosDeleted: modeloContratosDeleted, modeloContactosDeleted: modeloContactosDeleted, modeloProcesosDeleted: modeloProcesosDeleted },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                $('#modal').modal('hide');

                                clientesCRUD.getAllClientes();

                                swal.fire("¡Notificación!", "El cliente se modificó correctamente.", "success");

                            } else if(respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Valicación!", respuesta.error, "warning");

                            }
                        },
                        error: function (ex) {

                            swal.fire("Oops!", ex, "error");
                        }
                    });
                } else {
                    swal.fire("¡Validación!", "Debe diligenciar todos los campos obligatorios. Valide en todas las pestañas los campos resaltados de rojo y agregue información.", "info");
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método updateClienteAsync \n" + ex, "error");
            }
        });   
    },

    deleteClienteAsync: function (id) {
        'use strict';
        try {
            var token = $('input[name="__RequestVerificationToken"]').val();

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
                        url: rootHost + 'Agenda/DeleteEventoAsync',
                        data: { __RequestVerificationToken: token, EventoID: $(id).data("eventoid") },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {


                            if (respuesta.msn == "success") {
                                $.ajax({
                                                        type: "POST",
                                                        url: rootHost + 'Clientes/DeleteClienteAsync',
                                                        data: { __RequestVerificationToken: token, clienteID: $(id).val() },
                                                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                                                        dataType: "JSON",
                                                        async: true,
                                                        success: function (respuesta) {

                                                            if (respuesta.msn === "success") {

                                                                clientesCRUD.getAllClientes();

                                                                swal.fire("¡Notificación!", "El cliente se eliminó correctamente.", "success");
                                                            } else if (respuesta.error != null) {
                                                                if (respuesta.exc == true) {
                                                                    swal.fire("¡Error!", respuesta.error, "error");
                                                                    return;
                                                                }
                                                                swal.fire("¡Valicación!", respuesta.error, "info");

                                                            }
                                                        },
                                                        error: function (respuesta) {

                                                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                                                        }
                                                    });
                            } else {
                                swal.fire("¡Notificación!", respuesta.error, "warning");
                            }
                        },
                        error: function (respuesta) {

                            swal.fire("Oops!", JSON.stringify(respuesta), "error");
                        }
                    });


                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });


        } catch (ex) {

            swal.fire("Oops!", "Error en el método deleteClienteAsync \n" + ex, "error");
        }
    },

    checkExists: function (id) {
        'use strict';

        try {
            var valor = $(id).val();
            valor = parseInt((valor.replace(/\./gi, '')));

            $.ajax({
                type: "POST",
                url: rootHost + 'Clientes/CheckExists',
                data: "{ StrIdentificacion: '" + valor + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    var codigo = $("#codigo").val();

                    $("#codigoIcono").show();
                    $(id).addClass("border-right-0");
                    $(id).removeClass("is-invalid");
                    $("#errorClienteIdentificacion").text("");

                    if (respuesta === "True" && $(id).val() != codigo) {

                        $("#clienteIdentificacion").parsley().destroy();
                        $("#codigoIcono").hide();
                        $(id).removeClass("border-right-0");
                        $(id).addClass("is-invalid");
                        $("#errorClienteIdentificacion").text("Este cliente ya existe. Intente con otro Nit diferente.");
                       
                    }
                },
                error: function (respuesta) {

                    swal.fire("Oops!", JSON.stringify(respuesta), "error");
                }
            });
        } catch (ex) {
            swal.fire("¡Error!", "Error en el método CheckExists \n" + ex, "error");
        }
      
    },

    formatearNumeros: function (numero, tipo) {
        'use strict';

        try {
            
            if (numero.length == 0 || isNaN(numero) || numero == 'undefined' ) {
                numero = 0;
            }

            //Formato con simbolo de moneda
            if (tipo == "moneda") {
                var newNumero = new Intl.NumberFormat("en-US", {
                    style: "currency",
                    currency: "USD",
                    minimumFractionDigits: 0
                }).format(parseInt(numero));
                //Formato con separación de punto
            } else if (tipo == "punto") {
                var newNumero = new Intl.NumberFormat("es-CO").format(parseInt(numero));
            } else {
                //Formato sin moneda
                var newNumero = new Intl.NumberFormat("en-US").format(parseInt(numero));
            }

            return newNumero;

        } catch (ex) {
            swal.fire("Oops!", "Error en el método formatearNumeros \n" + ex, "error");
        }

    },
    
    calcularDigitoDeVerificacion: function (myNit) {
        'use strict';

        try {         

            myNit = String(myNit);

            var vpri,
                x,
                y,
                z;

            // Procedimiento
            vpri = new Array(16);
            z = myNit.length;

            vpri[1] = 3;
            vpri[2] = 7;
            vpri[3] = 13;
            vpri[4] = 17;
            vpri[5] = 19;
            vpri[6] = 23;
            vpri[7] = 29;
            vpri[8] = 37;
            vpri[9] = 41;
            vpri[10] = 43;
            vpri[11] = 47;
            vpri[12] = 53;
            vpri[13] = 59;
            vpri[14] = 67;
            vpri[15] = 71;

            x = 0;
            y = 0;
            for (var i = 0; i < z; i++) {
                y = (myNit.substr(i, 1));
                // console.log ( y + "x" + vpri[z-i] + ":" ) ;

                x += (y * vpri[z - i]);
                // console.log ( x ) ;    
            }

            y = x % 11;
            // console.log ( y ) ;

            return (y > 1) ? 11 - y : y;
              

        } catch (ex) {
            swal.fire("Oops!", "Error en el método calcularDigitoDeVerificacion \n" + ex, "error");
        }

    },

    cambiarEstadoContrato: function (elemento){
        'use strict';

        try {
            $("#modificContrato").val("True");
            var estado = $(elemento).data("estado");

            if (estado == "True") {
                $(elemento).find("span").html("INACTIVO");
                $(elemento).find("span").removeClass("badge-success");
                $(elemento).find("span").addClass("badge-danger");
                $(elemento).data("estado","False");
            } else {
                clientesCRUD.inactivarContratosAnteriores();
                $(elemento).find("span").html("ACTIVO");
                $(elemento).find("span").removeClass("badge-danger");
                $(elemento).find("span").addClass("badge-success");
                $(elemento).data("estado", "True");
            }

        } catch (ex) {
            swal.fire("Oops!", "Error en el método inactivarContrato \n" + ex, "error");
        }

    },

    identificarContratosDeleted: function (id) {
        'use strict';
        try {
            var contrato = $(id).data("contratoid");
            if (contrato) {
                var newli = "<li>" + contrato + "</li>"

                $("#contratosDeleted").append(newli);
            } 

        } catch (ex) {
            swal.fire("Oops!", "Error en el método identificarContratosDeleted \n" + ex, "error");
        }
    },

    identificarContactosDeleted: function (id) {
        'use strict';
        try {
            var contacto = $(id).data("contactoid");
            if (contacto) {
                var newli = "<li>" + contacto + "</li>"

                $("#contactosDeleted").append(newli);
            }

        } catch (ex) {
            swal.fire("Oops!", "Error en el método identificarContactosDeleted \n" + ex, "error");
        }
    },

    identificarProcesosDeleted: function (id) {
        'use strict';
        try {
            var proceso = $(id).data("procesoid");
            if (proceso) {
                var newli = "<li>" + proceso + "</li>"

                $("#procesosDeleted").append(newli);
            }

        } catch (ex) {
            swal.fire("Oops!", "Error en el método identificarProcesosDeleted \n" + ex, "error");
        }
    },

    formatearTextoMinusculas: function (text) {
        'strict'
        try {
            var valor = $(text).val();
            $(text).val(valor.toLowerCase());      

        } catch (ex) {
            swal.fire("Oops!", "Error en el método formatearTextoMinusculas \n" + ex, "error");
        }
    },

    formatearTextoMayusculas: function (text) {
        'strict'
        try {
            var valor = $(text).val();
            $(text).val(valor.toUpperCase());      
        } catch (ex) {
            swal.fire("Oops!", "Error en el método formatearTextoMayusculas \n" + ex, "error");
        }
    },

    inactivarContratosAnteriores: function () {
        'use strict'

        try {

            $("#divGenerarDetalleContrato > tr").each(function (i, filas) {
                $(filas).find(".iEstado").data("estado","False");
                $(filas).find(".iEstado > span").html("INACTIVO");
                $(filas).find(".iEstado > span").removeClass("badge-success");
                $(filas).find(".iEstado > span").addClass("badge-danger");
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método inactivarContratosAnteriores \n" + ex, "error");
        }


    },

    getContratoParaEditarAsync: function (elementos) {
        'use strict';

        var numeroContrato = $(elementos).data("numerocontrato");    
        var modelo = {};
        var arrayListaUsuarios = [];
        var arrayListaSistemas = [];

        $("#divGenerarDetalleContrato > tr").each(function (i, tr) {

            var listaNumeroContrato = $(tr).data("numerocontrato");
            if (listaNumeroContrato == numeroContrato) {
                var idUsuarios = "#listaUsuariosID" + numeroContrato;
                var idSistemas = "#listaSistemasID" + numeroContrato;

                var fechaInicialContrato = $("span.iFechaInicialContrato",tr).html();
                var fechaFinalContrato = $("span.iFechaFinalContrato",tr).html();
                var horasContrato = $("span.iHorasContrato",tr).html();
                var valorContrato = parseInt(($("span.iValorContrato",tr).html().replace(/\,/gi, '')).replace("$", ""));
                var listaUsuariosID = $(idUsuarios,tr).html();
                var listaSistemasID = $(idSistemas, tr).html();

                modelo = {
                    "IntNumeroContrato": numeroContrato,
                    "DatFechaInicial": fechaInicialContrato,
                    "DatFechaFinal": fechaFinalContrato,
                    "IntHoras": horasContrato,
                    "IntValor": valorContrato,
                }

                var i = 0;
                $(listaUsuariosID).each(function (i, span) {

                    var item = {
                        "IntUsuarioID": $(span).html()
                    }

                    arrayListaUsuarios[i] = item;
                });

                $(listaSistemasID).each(function (i, span) {

                    var item = {
                        "IntSistemaID": $(span).html()
                    }

                    arrayListaSistemas[i] = item;
                });                
            }
        });

        var modeloUsuarios = JSON.stringify(arrayListaUsuarios);
        var modeloSistemas = JSON.stringify(arrayListaSistemas);      

        try {
            $.ajax({
                type: "POST",
                url: rootHost + 'Clientes/GetContratoParaEditarAsync',
                data: { modeloContrato: modelo, modeloUsuarios: modeloUsuarios, modeloSistemas: modeloSistemas },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#contenedor2').empty().html(respuesta);

                    $('#modal2').modal('show');

                },
                error: function (respuesta) {

                    swal.fire("Oops!", JSON.stringify(respuesta), "error");
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método getContratoParaEditarAsync \n" + ex, "error");
        }
    },

    updateContratoInFront: function () {

        'use strict';

        $("#formEditarContrato").submit(function (e) {

            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var numeroContrato = $("#editarNumeroContrato").val();
                    var fechaInicialContrato = $("#editarFechaInicialContrato").val();
                    var fechaFinalContrato = $("#editarFechaFinalContrato").val();
                    var horasContrato = $("#editarHorasContrato").val();
                    var valorContrato = $("#editarValorContrato").val();

                    //Agregar al detalle los datos del select Usuarios
                    var selectObject = document.getElementById("editarAsesorasContrato");
                    var tdUsuariosID = "";
                    var tdUsuariosDescripcion = "";
                    for (var i = 0; i < selectObject.options.length; i++) {
                        if (selectObject.options[i].selected == true) {
                            tdUsuariosID = tdUsuariosID + "<span class ='iUsuarioID' data-contrato='" + numeroContrato + "'>" + selectObject.options[i].value + '</span>'
                            tdUsuariosDescripcion = tdUsuariosDescripcion + "<span class='badge badge-primary mr-1'>" + selectObject.options[i].text + '</span>';
                        }
                    }

                    ////Agregar al detalle los datos del select Sistemas
                    var selectObject = document.getElementById("editarSistemasContrato");
                    var tdSistemasID = "";
                    var tdSistemasDescripcion = "";
                    for (var i = 0; i < selectObject.options.length; i++) {
                        if (selectObject.options[i].selected == true) {
                            tdSistemasID = tdSistemasID + "<span class ='iSistemaID' data-contrato='" + numeroContrato + "'>" + selectObject.options[i].value + '</span>'
                            tdSistemasDescripcion = tdSistemasDescripcion + "<span class='badge badge-primary mr-1'>" + selectObject.options[i].text + '</span>';
                        }
                    }


                    $("#divGenerarDetalleContrato > tr").each(function (i, tr) {

                        var listaNumeroContrato = $(tr).data("numerocontrato");
                        if (listaNumeroContrato == numeroContrato) {

                            var idUsuariosID = "#listaUsuariosID" + numeroContrato;
                            var idSistemasID = "#listaSistemasID" + numeroContrato;
                            var idUsuarios = "#listaUsuarios" + numeroContrato;
                            var idSistemas = "#listaSistemas" + numeroContrato;


                            $("span.iFechaInicialContrato",tr).html(fechaInicialContrato);
                            $("span.iFechaFinalContrato",tr).html(fechaFinalContrato);
                            $("span.iHorasContrato",tr).html(horasContrato);
                            $("span.iValorContrato",tr).html(valorContrato);
                            $(idUsuariosID,tr).html(tdUsuariosID);
                            $(idUsuarios,tr).html(tdUsuariosDescripcion);
                            $(idSistemasID,tr).html(tdSistemasID);
                            $(idSistemas,tr).html(tdSistemasDescripcion);

                            $("#modificContrato").val("True");

                            swal.fire("¡Notificación!", "Contrato modificado correctamente.", "info");

                        }

                    });

                    $('#modal2').modal('hide');
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método updateContratoInFront \n" + ex, "error");
            }
        });
    },


};