var actaCRUD = {

    getAllActasByCliente: function (id) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Actas/GetAllActasByCliente',
                data: { clienteID: id },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllActasByCliente').html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllActasByCliente \n" + ex, "error");
        }
    },

    getActaByActaID: function (id) {
        'use strict';

        try {
            var url = rootHost + 'Actas/GetVistaPreviaActa';
            $.get(url, { AgendaID: id }).done(function (data) {
                if (data.error != null) {
                    if (data.exc == true) {
                        swal.fire("¡Error!", data.error, "error");
                        return;
                    }
                    swal.fire("¡Validación!", data.error, "info");
                } else {
                    $('#contenedor').empty().html(data);
                    $('#modal').modal('show');
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método getActaByActaID \n" + ex, "error");
        }
    },

    agregarAsistentes: function () {
        'use strict'

        try {

            var nombre = $("#asistenteNombre").val();
            var proceso = $("#asistenteProceso").val();

            if (nombre != "" && proceso != "") {

                var badgeAsiste = '<a href="#" class=".iOpcAsiste" data-asistencia="False" onclick="actaCRUD.cambiarAsistencia(this);"><span class="badge badge-danger">NO ASISTE</span></a>';

                if ($('#opcAsiste').prop('checked')) {
                    badgeAsiste = '<a href="#" class="iOpcAsiste" data-asistencia="True" onclick="actaCRUD.cambiarAsistencia(this);"><span class="badge badge-success">ASISTE</span></a>';
                }

                var newtr = '<tr class="iAsistenteID" data-id="0">';

                newtr = newtr + '<td class=""><span class="iNombreAsistente">' + nombre + '</span></td>';
                newtr = newtr + '<td class=""><span class="iProcesoAsistente">' + proceso + '</span></td>';
                newtr = newtr + '<td class="text-center">' + badgeAsiste + '</td>';
                newtr = newtr + '<td class="text-center"><button type="button" class="btn bg-white p-0 remove-item"><em class="fa fa-trash-alt fa-1x text-danger"></em> </button></td></tr>';

                $('#tblDetalleAsistentes').append(newtr);

                $('.remove-item').off().click(function (e) {
                    $(this).parent('td').parent('tr').remove();
                });

                $('#modal3').modal('hide');

            } else {
                swal.fire("¡Notificación!", "Debe ingresar los datos requeridos.", "warning");
                return;
            }

        } catch (ex) {

            swal.fire("Oops!", "Error en el método agregarAsistentes \n" + ex, "error");
        }
    },

    cambiarAsistencia: function (elemento) {
        'use strict';

        try {

            var estado = $(elemento).data("asistencia");

            if (estado == "True") {
                $(elemento).find("span").html("NO ASISTE");
                $(elemento).find("span").removeClass("badge-success");
                $(elemento).find("span").addClass("badge-danger");
                $(elemento).data("asistencia", "False");
            } else {
                $(elemento).find("span").html("ASISTE");
                $(elemento).find("span").removeClass("badge-danger");
                $(elemento).find("span").addClass("badge-success");
                $(elemento).data("asistencia", "True");
            }

        } catch (ex) {
            swal.fire("Oops!", "Error en el método cambiarAsistencia \n" + ex, "error");
        }

    },

    getAllAsistentes: function (id) {
        'use strict';

        try {


            $.ajax({
                type: "POST",
                url: rootHost + 'Actas/GetAllAsistentes',
                data: { actaID: id },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllAsistentes').html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllAsistentes \n" + ex, "error");
        }
    },

    getAllTemas: function (id) {
        'use strict';

        try {


            $.ajax({
                type: "POST",
                url: rootHost + 'Actas/GetAllTemas',
                data: { actaID: id },
                dataType: "html",
                async: true,
                success: function (respuesta) {
                    $('#divGetAllTemas').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllTemas \n" + ex, "error");
        }
    },

    getAllActividades: function (id) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Actas/GetAllActividades',
                data: { actaID: id },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllActividades').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllTemas \n" + ex, "error");
        }
    },

    getAllCompromisosActaAnterior: function (id) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Actas/GetAllCompromisosActaAnterior',
                data: { actaID: id },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllCompromisos').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método GetAllCompromisosActaAnterior \n" + ex, "error");
        }
    },

    generarDetalleTemas: function () {
        'use strict'

        try {

            var contador = $("#contadorTemas").val();
            contador = parseInt(contador) + 1;

            $("#contadorTemas").val(contador);

            var actaID = $("#actaID").val();
            var tema = $("#actaTema").val();
            var responsable = $("#actaResponsable").val();

            if (tema != "" && responsable != "") {

                var newli = '<li class="card m-0" data-id="0">' +
                    '<div class="card-header border-bottom-0 pl-0 pt-1 pb-0 mt-0 mb-0 pb-0 " id = "heading' + contador + '" >' +
                    '<div class="row" >' +
                    '<div class="col-7">' +
                    '<a href="#" class="btn text-muted h4 mb-0 pb-0" data-toggle="collapse" data-target="#collapse' + contador + '" aria-expanded="true" aria-controls="collapse' + contador + '">' +
                    '<h6 class="text-left text-muted"><span class="font-weight-bold">Tema: </span><span class="iTema">' + tema + '</span></h6>' +
                    '</a>' +
                    '</div>' +
                    '<div class="col-4 pt-2 ">' +
                    '<h6 class="text-left text-muted"><span class="font-weight-bold">Responsable: </span><span class="iResponsable">' + responsable + '</span></h6>' +
                    '</div>' +
                    '<div class="col-1">' +
                    '<button type="button" class="close remove-item" data-id="0" onclick="actaCRUD.deleteTemasByIDAsync(0)">&times;</button> ' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div id="collapse' + contador + '" class="collapse show" aria-labelledby="heading' + contador + '" data-parent="#accordionTemas">' +
                    '<hr class="pt-0 mt-0" />' +
                    '<div class="card-body pt-1 ">' +
                    '<div class="form-group">' +
                    '<textarea class="form-control iDesarrollo" style="height:100px" placeholder="Ingrese aquí el desarrollo de este tema..."></textarea>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</li >';

                $('#accordionTemas').append(newli);

                //Limpiar inputs
                $(".temas").each(function (i, elemento) {
                    $(elemento).val("");

                });

                $('.remove-item').off().click(function (e) {
                    var id = $(this).data("id");
                    $("#"+ id).remove();
                });

            } else {
                swal.fire("¡Notificación!", "Debe ingresar los datos requeridos.", "warning");
                return;
            }

        } catch (ex) {

            swal.fire("Oops!", "Error en el método generarDetalleTemas \n" + ex, "error");
        }
    },

    generarDetalleActividades: function () {
        'use strict'

        try {

            var actaID = $("#actaID").val();
            var actividad = $("#actividadActa").val();
            var responsable = $("#responsableActividad").val();
            var fechaActividad = $("#fechaActividad").val();

            if (actividad != "" && responsable != "" && fechaActividad != "" ) {

                var newtr = '<tr class="item" data-id="0">';

                newtr = newtr + '<td style="text-align:center; display:none;"><span class="iActividadID"></span></td>';
                newtr = newtr + '<td class=""><span class="iActividadDescripcion text-muted">' + actividad + '</span></td>';
                newtr = newtr + '<td class=""><span class="iActividadResponsable text-muted">' + responsable + '</span></td>';
                newtr = newtr + '<td class="text-center"><span class="iActividadFecha text-muted">' + fechaActividad + '</span></td>';
                newtr = newtr + '<td class="text-center h5"><a href="#" class="iActividadEstado" data-estado="False" onclick ="actaCRUD.cambiarEstadoActividad(this)"><span class="badge badge-danger">NO</span></a></td>';
                newtr = newtr + '<td class="text-right"><button type="button" class="btn bg-white pr-0 mr-1" data-actividad = "' + actividad + '" data-responsable = "' + responsable + '" data-fecha = "' + fechaActividad + '" onclick="actaCRUD.getActividadParaEditarAsync(this);"><i class="fa fa-edit fa-1x text-info"></i></button><button type="button" class="btn bg-white p-0 remove-item"><em class="fa fa-trash-alt fa-1x text-danger"></em> </button></td></tr>';

                $('#tblDetalleActividades').append(newtr);

                //Limpiar inputs
                $(".actividades").each(function (i, elemento) {
                    $(elemento).val("");
                });

                $('.remove-item').off().click(function (e) {
                    $(this).parent('td').parent('tr').remove();
                });


            } else {
                swal.fire("¡Notificación!", "Debe ingresar los datos requeridos.", "warning");
                return;
            }

        } catch (ex) {

            swal.fire("Oops!", "Error en el método generarDetalleActividades \n" + ex, "error");
        }
    },

    crearActaAsync: function () {
        'use strict';

        $("#formCrearActaGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    //modelo Acta
                    var modelo = {
                        "IntAgendaID": $("#agendaID").val(),
                        "IntConsecutivo": $("#consecutivoActa").val(),                 
                    };

                    //modeloAsistentes
                    var i = 0;
                    var listaAsistentes = [];
                    $("#tblDetalleAsistentes > tr").each(function (i, tr) {

                        var asistenteNombre = $("span.iNombreAsistente", tr).html();
                        var asistenteProceso = $("span.iProcesoAsistente", tr).html();
                        var asistenteAsiste = $("a.iOpcAsiste", tr).data("asistencia");

                        var item = {
                            StrNombre: asistenteNombre,
                            StrProceso: asistenteProceso,
                            OpcAsiste: asistenteAsiste,
                        };

                        listaAsistentes[i] = item;
                        i++;
                    });

                    //modeloTemas
                    i = 0;
                    var listaTemas = [];
                    $("#accordionTemas > li").each(function (i, li) {

                        var temaDescripcion = $(li).find(".iTema").html();
                        var temaResponsable = $(li).find(".iResponsable").html();
                        var temaDesarrollo = $(li).find(".iDesarrollo").val();

                        var item = {
                            StrDescripcion: temaDescripcion,
                            StrResponsable: temaResponsable,
                            StrDesarrollo: temaDesarrollo,
                        };

                        listaTemas[i] = item;
                        i++;
                    });

                    //modeloActividades
                    i = 0;
                    var listaActividades = [];
                    $("#tblDetalleActividades > tr").each(function (i, tr) {

                        var actividadDescripcion = $("span.iActividadDescripcion", tr).html();
                        var actividadResponsable = $("span.iActividadResponsable", tr).html();                       
                        var actividadFecha = $("span.iActividadFecha", tr).html();
                        var actividadEstado = $("a.iActividadEstado", tr).data("estado");

                        var item = {
                            StrDescripcion: actividadDescripcion,
                            StrResponsable: actividadResponsable,
                            DatFecha: actividadFecha,
                            OpcEjecuta: actividadEstado,
                        };

                        listaActividades[i] = item;
                        i++;
                    });

                    var jsonAsistentes = JSON.stringify(listaAsistentes);
                    var jsonTemas = JSON.stringify(listaTemas);
                    var jsonActividades = JSON.stringify(listaActividades);

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Actas/CrearActaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, modeloAsistentes: jsonAsistentes, modeloTemas: jsonTemas, modeloActividades: jsonActividades },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {                         

                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                //$('#modal2').modal('hide');

                            } else {

                                swal.fire("¡Error!", respuesta.error, "error");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                        }
                    });
                } else {
                    swal.fire("¡Validación!", "Debe diligenciar todos los campos obligatorios. Valide en todas las pestañas los campos resaltados de rojo y agregue información.", "warning");
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearActaAsync \n" + ex, "error");
            }
        });
    },

    updateActaAsync: function () {

        'use strict';

        $("#formEditarActaGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    //modelo Acta
                    var modelo = {
                        "IntActaID": $("#actaID").val(),
                        "IntAgendaID": $("#agendaID").val(),
                        "IntConsecutivo": $("#consecutivoActa").val(),
                    };

                    //modeloAsistentes
                    var i = 0;
                    var listaAsistentes = [];
                    $("#tblDetalleAsistentes > tr").each(function (i, tr) {

                        var asistenteID = $(this).data("id");
                        var asistenteNombre = $("span.iNombreAsistente", tr).html();
                        var asistenteProceso = $("span.iProcesoAsistente", tr).html();
                        var asistenteAsiste = $("a.iOpcAsiste", tr).data("asistencia");

                        var item = {

                            IntAsistenteID: asistenteID,
                            StrNombre: asistenteNombre,
                            StrProceso: asistenteProceso,
                            OpcAsiste: asistenteAsiste,
                        };

                        listaAsistentes[i] = item;
                        i++;
                    });

                    //modeloTemas
                    i = 0;
                    var listaTemas = [];
                    $("#accordionTemas > li").each(function (i, li) {

                        var temaID = $(this).data("id");
                        var temaDescripcion = $(li).find(".iTema").html();
                        var temaResponsable = $(li).find(".iResponsable").html();
                        var temaDesarrollo = $(li).find(".iDesarrollo").val();

                        if (temaDesarrollo.length > 4000) throw new ("El campo desarrollo del tema supera el tamaño de 4000 mil carácteres.");

                        var item = {

                            IntTemaID: temaID,
                            StrDescripcion: temaDescripcion,
                            StrResponsable: temaResponsable,
                            StrDesarrollo: temaDesarrollo,
                        };

                        listaTemas[i] = item;
                        i++;
                    });

                    //modeloActividades
                    i = 0;
                    var listaActividades = [];
                    $("#tblDetalleActividades > tr").each(function (i, tr) {

                        var actividadesID = $(this).data("id");
                        var actividadDescripcion = $("span.iActividadDescripcion", tr).html();
                        var actividadResponsable = $("span.iActividadResponsable", tr).html();
                        var actividadFecha = $("span.iActividadFecha", tr).html();

                        if (actividadDescripcion.length > 2000) throw new ("El campo desarrollo del tema supera el tamaño de 4000 mil carácteres.");

                        var actividadEstado = $("a.iActividadEstado", tr).data("estado");

                        var item = {
                            IntActividadesID: actividadesID,
                            StrDescripcion: actividadDescripcion,
                            StrResponsable: actividadResponsable,
                            DatFecha: actividadFecha,
                            OpcEjecuta: actividadEstado,
                        };

                        listaActividades[i] = item;
                        i++;
                    });
                 
                    //Identificar asistentes eliminados
                    var i = 0;
                    var listaAsistentesDeleted = [];
                    $("#asistentesDeleted > li").each(function (i, li) {
                        var id = $(this).html();

                        var item = {
                            IntAsistenteID: id
                        };

                        listaAsistentesDeleted[i] = item;
                        i++;
                    });

                    //Identificar temas eliminados
                    var i = 0;
                    var listaTemasDeleted = [];
                    $("#temasDeleted > li").each(function (i, li) {
                        var id = $(this).html();

                        var item = {
                            IntTemaID: id
                        };

                        listaTemasDeleted[i] = item;
                        i++;
                    });

                    //Identificar actividades eliminados
                    var i = 0;
                    var listaActividadesDeleted = [];
                    $("#actividadesDeleted > li").each(function (i, li) {
                        var id = $(this).html();

                        var item = {
                            IntActividadID: id
                        };

                        listaActividadesDeleted[i] = item;
                        i++;
                    });

                    //Convertir modelos a JSON
                    var jsonAsistentes = JSON.stringify(listaAsistentes);
                    var jsonTemas = JSON.stringify(listaTemas);
                    var jsonActividades = JSON.stringify(listaActividades);
                    var jsonAsistentesDeleted = JSON.stringify(listaAsistentesDeleted);
                    var jsonTemasDeleted = JSON.stringify(listaTemasDeleted);
                    var jsonActividadesDeleted = JSON.stringify(listaActividadesDeleted);

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Actas/UpdateActaAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, modeloAsistentes: jsonAsistentes, modeloTemas: jsonTemas, modeloActividades: jsonActividades },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                actaCRUD.getAllTemas(respuesta.actaID);

                                toastr.success('El registro se guardó correctamente');
                            }

                            else toastr.error(respuesta.error);
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                        }
                    });
                } else {
                    swal.fire("¡Validación!", "Debe diligenciar todos los campos obligatorios. Valide en todas las pestañas los campos resaltados de rojo y agregue información.", "warning");
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método updateActaAsync \n" + ex, "error");
            }
        });   
    },

    cambiarEstadoActividad: function (elemento) {
        'use strict';

        try {
            var estado = $(elemento).data("estado");

            if (estado == "True") {
                $(elemento).find("span").html("NO");
                $(elemento).find("span").removeClass("badge-success");
                $(elemento).find("span").addClass("badge-danger");
                $(elemento).data("estado", "False");
                if ($("#opcEnviado").prop("checked")) {
                    var id = $(elemento).data("id");
                    var estado = $(elemento).data("estado");
                        //actaCRUD.updateEjecutaActividadesAsync(id, false);
                }

            } else {
                $(elemento).find("span").html("SI");
                $(elemento).find("span").removeClass("badge-danger");
                $(elemento).find("span").addClass("badge-success");
                $(elemento).data("estado", "True");
                if ($("#opcEnviado").prop("checked")) {
                    var id = $(elemento).data("id");
                    var estado = $(elemento).data("estado");
                    //actaCRUD.updateEjecutaActividadesAsync(id, true);
                }
            }


        } catch (ex) {
            swal.fire("Oops!", "Error en el método cambiarEstadoActividad \n" + ex, "error");
        }

    },

    updateEjecutaActividadesAsync: function (id, estado) {

        'use strict';
        try {

            $("#formEditarActaGlobal").submit();

            var modelo = {
                IntActividadID: id,
                OpcEjecuta: estado
            };

            $.ajax({
                type: "POST",
                url: rootHost + 'Actividades/UpdateEjecutaActividadAsync',
                data: { modelo: modelo },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    //if (respuesta.msn == "success") actaCRUD.getAllActividades(respuesta.actaID);                    
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });


        } catch (ex) {
            swal.fire("Oops!", "Error en el método updateEjecutaActividadesAsync \n" + ex, "error");
        }

    },

    updateEjecutaCompromisosAsync: function (id, estado) {

        'use strict';
        try {

            var modelo = {
                IntActividadID: id,
                OpcEjecuta: estado
            };

            var actaIDActual = $("#actaID").val();

            $.ajax({
                type: "POST",
                url: rootHost + 'Actividades/UpdateEjecutaActividadAsync',
                data: { modelo: modelo, actaID: actaIDActual },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn == "success")
                        actaCRUD.getAllCompromisosActaAnterior(respuesta.actaID);
                    
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });


        } catch (ex) {
            swal.fire("Oops!", "Error en el método updateEjecutaCompromisosAsync \n" + ex, "error");
        }

    },

    identificarAsistentesDeleted: function (id) {
        'use strict';
        try {
            var asistente = $(id).data("id");
            if (asistente) {
                var newli = "<li>" + asistente + "</li>"

                $("#asistentesDeleted").append(newli);
            } 

        } catch (ex) {
            swal.fire("Oops!", "Error en el método identificarAsistentesDeleted \n" + ex, "error");
        }
    },

    identificarTemasDeleted: function (id) {
        'use strict';
        try {
            var temas = $(id).data("id");
            if (temas) {
                var newli = "<li>" + temas + "</li>"

                $("#temasDeleted").append(newli);
            }

        } catch (ex) {
            swal.fire("Oops!", "Error en el método identificarTemasDeleted \n" + ex, "error");
        }
    },

    identificarActividadesDeleted: function (id) {
        'use strict';
        try {
            var actividades = $(id).data("id");
            if (actividades) {
                var newli = "<li>" + actividades + "</li>"

                $("#actividadesDeleted").append(newli);
            }

        } catch (ex) {
            swal.fire("Oops!", "Error en el método identificarActividadesDeleted \n" + ex, "error");
        }
    },

    limpiarItemsDeleted: function () {
        'use strict'
        try {
            $("#asistentesDeleted,#temasDeleted,#actividadesDeleted > li").each(function (i, li) {
                $(li).remove();
            });
        } catch (ex) {
            swal.fire("Oops!", "Error en el método limpiarItemsDeleted \n" + ex, "error");
        }

    },

    descargarActaPDF: function (id) {
        'use strict';

        try {

            var listaCompromisosSeleccionados = [];
            var contadorCompromisos = 0;

            $("#tblCompromisosSeleccionados > tbody > tr").each(function (i, elemento) {

                var elementoCheckbox = $(elemento).find(".iSeleccionarCompromiso");
                var seleccionarCompromiso = elementoCheckbox.is(":checked");

                var compromisoID = $(elementoCheckbox).data("id");

                if (seleccionarCompromiso === true) {
                    listaCompromisosSeleccionados[contadorCompromisos] = compromisoID;
                    contadorCompromisos++;
                }

            });

            var listaActividadesSeleccionadas = [];
            var contadorActividades = 0;

            $("#tblActividadesSeleccionadas > tbody > tr").each(function (i, elemento) {


                var elementoCheckbox = $(elemento).find(".iSeleccionarActividad");
                var seleccionarActividad = elementoCheckbox.is(":checked");

                var actividadID = $(elementoCheckbox).data("id");

                if (seleccionarActividad === true) {
                    listaActividadesSeleccionadas[contadorActividades] = actividadID;
                    contadorActividades++;
                }

            });

            window.location.href = rootHost + 'Actas/DescargarActaPDF?actaID=' + id + '&listaCompromisosSeleccionados=' + listaCompromisosSeleccionados + '&listaActividadesSeleccionadas=' + listaActividadesSeleccionadas;

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllTemas \n" + ex, "error");
        }
    },

    EnviarActaByEmailAsync: function (agendaID) {
        'use strict';

        try {         

            var listaCompromisosSeleccionados = [];
            var contadorCompromisos = 0;

            $("#tblCompromisosSeleccionados > tbody > tr").each(function (i, elemento) {

                var elementoCheckbox = $(elemento).find(".iSeleccionarCompromiso");
                var seleccionarCompromiso = elementoCheckbox.is(":checked");

                var compromisoID = $(elementoCheckbox).data("id");

                if (seleccionarCompromiso === true) {
                    listaCompromisosSeleccionados[contadorCompromisos] = compromisoID;
                    contadorCompromisos++;
                }

            });

            var listaActividadesSeleccionadas = [];
            var contadorActividades = 0;

            $("#tblActividadesSeleccionadas > tbody > tr").each(function (i, elemento) {


                var elementoCheckbox = $(elemento).find(".iSeleccionarActividad");
                var seleccionarActividad = elementoCheckbox.is(":checked");

                var actividadID = $(elementoCheckbox).data("id");

                if (seleccionarActividad === true) {
                    listaActividadesSeleccionadas[contadorActividades] = actividadID;
                    contadorActividades++;
                }

            });

            var cantidadIntentos = 0;

            $.ajax({
                type: "POST",
                url: rootHost + 'Actas/EnviarActaByEmailAsync',
                data: { agendaID: agendaID, listaCompromisosSeleccionados: listaCompromisosSeleccionados, listaActividadesSeleccionadas: listaActividadesSeleccionadas, cantidadIntentos: cantidadIntentos },
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                        swal.fire("¡Notificación!", "El correo ha sido enviado correctamente.", "success");

                        $('#modal').modal('hide');
                        $('#modal2').modal('hide');

                    } else if (respuesta.exc) {
                        swal.fire("¡Error!", respuesta.error, "error");
                    } else {

                        swal.fire("¡Validacion!", respuesta.error, "warning");
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método getClienteParaEditarAsync \n" + ex, "error");
        }
           
    },

    getActividadParaEditarAsync: function (elementos) {
        'use strict';

        //Almacenar el indice de la fila que se está actualizando
        $('#tblActividades').find('tr').click(function () {
            var indice = $(this).index() + 1;
            $("#indexTblActividades").val($(this).index() + 1);

            $("#tblDetalleActividades > tr").each(function (i, tr) {

                if (parseInt(indice) == i + 1) {

                    var actividad = $("span.iActividadDescripcion", tr).html();
                    var responsable = $("span.iActividadResponsable", tr).html();
                    var fecha = moment($("span.iActividadFecha", tr).html(), "YYYY-MM-DD").format("YYYY-MM-DD");

                    var modelo = {
                        "StrDescripcion": actividad,
                        "StrResponsable": responsable,
                        "DatFecha": fecha,
                    }

                    try {
                        $.ajax({
                            type: "POST",
                            url: rootHost + 'Actas/GetActividadesForEditarAsync',
                            data: { modelo: modelo },
                            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                            dataType: "html",
                            async: true,
                            success: function (respuesta) {

                                $('#contenedor3').empty().html(respuesta);

                                $('#modal3').modal('show');

                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                            }
                        });
                    } catch (ex) {

                        swal.fire("Oops!", "Error en el método getActividadParaEditarAsync \n" + ex, "error");
                    }
                }

            });

        });
    },

    updateActividadInFront: function () {

        'use strict';

        $("#formEditarActividad").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {


                    var descripcion = $("#editarActividadDescripcion").val();
                    var responsable = $("#editarActividadResponsable").val();
                    var DatFecha = $("#editarActividadFecha").val();

                    $("#tblDetalleActividades > tr").each(function (i, tr) {

                        var indice = $("#indexTblActividades").val();
                        if (indice == i + 1) {                         

                            $("span.iActividadDescripcion", tr).html(descripcion);
                            $("span.iActividadResponsable", tr).html(responsable);
                            $("span.iActividadFecha", tr).html(DatFecha);                           

                            swal.fire("¡Notificación!", "La actividad se modificó correctamente.", "info");
                        }

                    });

                    $('#modal3').modal('hide');
                } 

            } catch (ex) {

                swal.fire("Oops!", "Error en el método updateActaAsync \n" + ex, "error");
            }
        });
    },

    deleteTemasByIDAsync: function (temaID) {
        'use strict';
        try {

            swal.fire({
                title: "¡Eliminación!",
                text: "Antes de eliminar el tema por favor guarde los cambios. ¿Está seguro que desea eliminar el registro?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: "SI",
                cancelButtonText: "NO",
            }).then(function (resultado) {

                if (resultado.isConfirmed) {

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Actas/DeleteTemasByIDAsync',
                        data: { temaID: temaID },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {
                                actaCRUD.getAllTemas(respuesta.actaID);
                            } else {
                                swal.fire("¡Notificación!", respuesta.error, "warning");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                        }
                    });


                } else swal.fire("¡Alerta!", "El registro no fue eliminado.", "warning");

            });


        } catch (ex) {

            swal.fire("Oops!", "Error en el método deleteTemasByIDAsync \n" + ex, "error");
        }
    },

};