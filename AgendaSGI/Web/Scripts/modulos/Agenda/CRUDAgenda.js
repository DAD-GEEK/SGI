var agendaCRUD = {

    getAllEventos: function () {
        'use strict'
        try {

            var calendarEl = document.getElementById('calendar');

            var calendar = new FullCalendar.Calendar(calendarEl, {

                plugins: ['interaction', 'dayGrid', 'timeGrid', 'list'],

                header: //Contenido del encabezado
                {
                    left: 'prevYear,prev,today,next,nextYear',
                    center: 'title',
                    right: 'listWeek,dayGridMonth,timeGridWeek,timeGridDay'
                },

                buttonText: { //Texto en los botones
                    today: 'Hoy',
                    month: 'Mes',
                    week: 'Semana',
                    day: 'Dia',
                    list: 'Agenda SGI'
                },

                defaultDate: $("#fechaDefault").val(), //Fecha por defecto
                defaultView: $("#vistaDefault").val(), //Vista por defecto de la semana al cargar la ventana
                height: 'auto', //Altura del calendario se calcula automaticamente de acuerdo a sus elementos internos
                minTime: $("#horaInicial").val(), //Mínima hora en el calendario
                maxTime: $("#horaFinal").val(), //Máxima hora en el calendario

                titleFormat: { //Formato del titulo del calendario
                    year: 'numeric',
                    month: 'long',
                    day: 'numeric',
                },

                slotLabelFormat: { //se visualizara de esta manera 01:00 AM en la columna de horas
                    hour: '2-digit',
                    minute: '2-digit',
                    hour12: true,
                    omitZeroMinute: true,
                    meridiem: 'narrow'
                },

                eventTimeFormat: { //se visualizara de esta manera 01:00 AM en la columna de semanas
                    week: 'short',
                    hour: 'numeric',
                    minute: '2-digit',
                    omitZeroMinute: false,
                    meridiem: 'false',
                    hour12: true
                },

                businessHours: { //Horario y dias del calendario --> los resalta de color diferente los que no están en la selección
                    daysOfWeek: [1, 2, 3, 4, 5], // Monday - Thursday --> days of week. an array of zero-based day of week integers (0=Sunday)
                    startTime: $("#horaInicial").val(), //Hora inicial
                    endTime: $("#horaFinal").val(), //Hora final
                },

                eventColor: $("#usuarioColorAgenda").val(), //Color de backgroud mientras se está seleccionando el evento
                locale: 'es-us', //Activar lenguaje español
                showNonCurrentDates: false, //En la vista de mes muestra solo los dias del mes que estoy consultando
                editable: true, //Permite modificar los eventos
                navLinks: true, //Cuando se da click en el dia, la vista cambia para ese dia
                droppable: true, //Si los eventos se pueden arrastrar hasta otra fecha
                selectable: true, //Permite seleccionar fechas del calendario
                selectMirror: true,
                eventLimit: true,
                nowIndicator: true, //En la vista semana y vista dia muestra una linea roja en el dia y hora actual
                dragRevertDuration: 1000, //Tiempo que tarda un evento fallido en retornar a su posición
                expandRows: true, //Expandir las casillas si el tamaño del evento es grande
                lazyFetching: false, //Si está en falso hace llamados a la BD siempre, si es true guarda la info en caché

                eventDrop: function (obj) { //Se dispara cuando se mueve un evento para una fecha diferente
                    agendaCRUD.updateEventoDropResizeAsync(obj, 'Drop');
                },

                eventResize: function (obj) { //Se dispara cuando se cambia el tamaño de un evento exiistente
                    agendaCRUD.updateEventoDropResizeAsync(obj, 'Resize');
                },

                select: function (obj) { //Se dispara cuando se selecciona una fecha sin evento
                    //console.log(moment(obj.startStr).format("YYYY-MM-DD") + "--" + moment(new Date()).format("YYYY-MM-DD"))

                    //if (moment(obj.startStr).format("YYYY-MM-DD") < moment(new Date()).format("YYYY-MM-DD")) {
                    //    swal.fire("¡Alerta!", "No se permite agendar visitas para dias anteriores.", "warning");
                    //} else {

                    var url = rootHost + 'Agenda/AgregarCita';
                    $.get(url, function (data) {

                        if (data.error != null) {
                            if (data.exc == true) {
                                swal.fire("¡Error!", data.error, "error");
                                return;
                            }
                            swal.fire("¡Validación!", data.error, "warning");

                        } else {
                            $('#contenedor').empty().html(data);
                            $('#modal').modal('show');

                            //Si doy click en todo el dia
                            $("#citaAllDay").prop("checked", obj.allDay);

                            //Si la opción todo el dia esta checked entonces inhabilito los selectores de fecha
                            if ($('#citaAllDay').prop('checked') === true) {
                                $(".datetimepicker-input").prop("disabled", true);
                            }

                            //Formatear fechas en el formulario Agregar Cita
                            if (obj.view.type == "dayGridMonth" || obj.allDay === true) {

                                if (obj.allDay === true) {
                                    $('#timepickerFinal').datetimepicker({
                                        defaultDate: (moment(obj.startStr + ' ' + $("#horaFinal").val(), 'YYYY-MM-DD h:m A')),
                                        format: 'YYYY-MM-DD h:mm A',
                                        //defaultDate: "11/1/2013 08:00:00",
                                        locale: 'es',
                                        icons: {
                                            time: "fa fa-clock",
                                            date: "fas fa-calendar-alt",
                                        }

                                    });
                                } else {
                                    $('#timepickerFinal').datetimepicker({
                                        defaultDate: (moment(obj.startStr + ' ' + $("#horaInicial").val(), 'YYYY-MM-DD h:m A')),
                                        format: 'YYYY-MM-DD h:mm A',
                                        //defaultDate: "11/1/2013 08:00:00",
                                        locale: 'es',
                                        icons: {
                                            time: "fa fa-clock",
                                            date: "fas fa-calendar-alt",
                                        }

                                    });
                                }

                                $('#timepickerInicio').datetimepicker({
                                    defaultDate: (moment(obj.startStr + ' ' + $("#horaInicial").val(), 'YYYY-MM-DD h:m A')),
                                    format: 'YYYY-MM-DD h:mm A',
                                    //defaultDate: "11/1/2013 08:00:00",
                                    locale: 'es',
                                    icons: {
                                        time: "fa fa-clock",
                                        date: "fas fa-calendar-alt",
                                    }
                                });

                            } else {
                                $('#timepickerFinal').datetimepicker({
                                    defaultDate: (moment(obj.end, 'YYYY-MM-DD h:m A')),/*(moment(obj.start, 'YYYY-MM-DD h:m A').add(parseInt($("#duracionDefault").val()),'hours')),*/
                                    format: 'YYYY-MM-DD h:mm A',
                                    //defaultDate: "11/1/2013 08:00:00",
                                    locale: 'es',
                                    icons: {
                                        time: "fa fa-clock",
                                        date: "fas fa-calendar-alt",
                                    }
                                });

                                $('#timepickerInicio').datetimepicker({
                                    defaultDate: (moment(obj.start, 'YYYY-MM-DD h:m A')),
                                    format: 'YYYY-MM-DD h:mm A',
                                    //defaultDate: "11/1/2013 08:00:00",
                                    locale: 'es',
                                    icons: {
                                        time: "fa fa-clock",
                                        date: "fas fa-calendar-alt",
                                    }
                                });
                            }
                        }
                    });

                    //}
                },

                eventClick: function (eventObj) { //Se dispara cuando se da click al evento

                    var eventoID = eventObj.event.id;

                    agendaCRUD.getEvento(eventoID);

                    //var url = rootHost + 'Agenda/GetEvento';
                    //$.get(url, { EventoID: eventoID }).done(function (data) {

                    //    if (data.error != null) {
                    //        if (data.exc == true) {
                    //            swal.fire("¡Error!", data.error, "error");
                    //            return;
                    //        }
                    //        swal.fire("¡Validación!", data.error, "warning");

                    //    } else {
                    //        $('#contenedor').empty().html(data);
                    //        $('#modal').modal('show');
                    //    }

                    //});
                },

                eventAllow: function (dropInfo, draggedEvent) {//Controlar que eventos se pueden realizar en cierto tiempo de tiempo
                    //No permite mover eventos si la hora inicial o final estan por fuera del rango configurado
                    if (draggedEvent.extendedProps.cancelada == true) {
                        return false;
                    }

                    if (moment(dropInfo.start).add(1, 'minutes').format("HH:mm") <= $("#horaInicial").val() || moment(dropInfo.end).add(-1, 'minutes').format("HH:mm") >= $("#horaFinal").val() || moment(dropInfo.startStr).format("YYYY-MM-DD") < moment(new Date()).format("YYYY-MM-DD")) {
                        return false;
                    }
                    else {
                        return true;
                    }
                },

                eventOverlap: function (stillEvent, movingEvent) { //Si se sobreponen dos eventos y tienen el mismo asesorID no lo permite
                    if ((stillEvent.extendedProps.asesorID != movingEvent.extendedProps.asesorID)) {
                        return true;
                    } else if (stillEvent.extendedProps.cancelada == true) {
                        return true;
                    }
                },

                //Obtener eventos en a mostrar en el calendario
                events: function (fetchInfo, successCallback, failureCallback) {


                    //Obtener los asesores filtrados
                    var selectObject = document.getElementById("asesorBuscar");

                    var a = 0;
                    var listaAsesores = [];
                    for (var i = 0; i < selectObject.options.length; i++) {
                        if (selectObject.options[i].selected == true) {

                            var item = {
                                IntUsuarioID: selectObject.options[i].value
                            };

                            listaAsesores[a] = item;
                            a++;
                        }
                    }

                    var jsonAsesores = JSON.stringify(listaAsesores);

                    //Obtener los clientes filtrados
                    var selectObject = document.getElementById("clienteBuscar");

                    var a = 0;
                    var listaClientes = [];
                    for (var i = 0; i < selectObject.options.length; i++) {
                        if (selectObject.options[i].selected == true) {

                            var item = {
                                IntClienteID: selectObject.options[i].value
                            };

                            listaClientes[a] = item;

                            a++;
                        }
                    }

                    var jsonClientes = JSON.stringify(listaClientes);


                    //Obtener los tipos de visita filtrados
                    var selectObject = document.getElementById("tipoEventoBuscar");

                    var a = 0;
                    var listaTipoEvento = [];
                    for (var i = 0; i < selectObject.options.length; i++) {
                        if (selectObject.options[i].selected == true) {

                            var item = {
                                IntTipoEventoID: selectObject.options[i].value
                            };

                            listaTipoEvento[a] = item;
                            a++;
                        }
                    }

                    var jsonTipoEvento = JSON.stringify(listaTipoEvento);

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Agenda/GetAllProgramacion',
                        data: { fechaInicio: fetchInfo.startStr, fechaFinal: fetchInfo.endStr, asesores: jsonAsesores, clientes: jsonClientes, tipoEvento: jsonTipoEvento },
                        dataType: "JSON",
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (respuesta) {

                            if (respuesta.success) {

                                $("#fechaInicialProgramacion").val(respuesta.fechaInicial);
                                $("#fechaFinalProgramacion").val(respuesta.fechaFinal);

                                var events = [];

                                $.each(respuesta.data, function (i, item) { //Agregar los eventos al calendario
                                    events.push({
                                        'id': item.id,
                                        'title': item.titulo,
                                        //description: item.Descripcion,
                                        'start': moment(item.fechaInicial, 'YYYY-MM-DD HH:mm').format('YYYY-MM-DD HH:mm'),
                                        'end': moment(item.fechaFinal, 'YYYY-MM-DD HH:mm').format('YYYY-MM-DD HH:mm'),
                                        'allDay': item.allDay,
                                        'backgroundColor': item.backgroundColor,
                                        'borderColor': 'white',
                                        'textColor': "white",
                                        'editable': item.editable,
                                        'extendedProps': { //Objeto con datos propios
                                            clienteNombre: item.clienteNombre,
                                            tipoEvento: item.tipoEvento,
                                            descripcion: item.descripcion,
                                            asesorID: item.asesorID,
                                            asesor: item.asesor,
                                            enviado: item.enviado,
                                            cancelada: item.cancelada,
                                            soporte: item.soporte
                                        },
                                    });

                                    //Corrección Bug duplicado evento al cambiar de vista despues de agregarlo
                                    if (calendar.getEventById(item.id) != null) {
                                        calendar.getEventById(item.id).remove();
                                    }
                                });

                                successCallback(events);
                            } else if (respuesta.error != null) {
                                if (respuesta.exc == true) {
                                    swal.fire("¡Error!", respuesta.error, "error");
                                    return;
                                }
                                swal.fire("¡Validación!", respuesta.error, "warning");

                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                        }
                    });

                },

                eventRender: function (info) { //Se establece despues de cargar el calendario, recurre nuevamente los eventos y agrega las funciones aquí descritas

                    //Agregar salto de linea despues de la hora en la vista mes
                    $(info.el).find(".fc-time").append("<br/>");

                    //Mostrar toltip si estoy en la vista del mes o listaSemana y el evento existe
                    if (info.event.id != '') {
                        if (info.view.type == "dayGridMonth" || info.view.type == "listWeek") {

                            //Si la vista es listaSemana
                            if (info.view.type == "listWeek") {

                                if (info.event.extendedProps.soporte == true) {
                                    $(info.el).find('a').append('<span class="float-right badge badge-secondary text-white">No aplica</span>');
                                } else if (info.event.extendedProps.enviado !== false) {
                                    $(info.el).find('a').append('<span class="float-right badge badge-success text-white">Enviado</span>');
                                } else {
                                    if (info.event.extendedProps.cancelada !== false) {
                                        $(info.el).find('a').append('<span class="float-right badge badge-secondary text-white">Cancelado</span>');
                                    } else {
                                        $(info.el).find('a').append('<span class="float-right badge badge-danger text-white">Sin enviar</span>');
                                    }
                                }
                            }

                            var descripcion = "<small>Sin descripción</small>"

                            if (info.event.extendedProps.descripcion != '') {
                                if (info.event.extendedProps.descripcion != null) {
                                    descripcion = '<p>' + info.event.extendedProps.descripcion + '</p>';
                                }
                            }

                            var fechas = "";

                            if (info.event.allDay == false) {
                                fechas = '<em>Inicio: ' + moment(info.event.start).format('YYYY-MM-DD h:mm A') + '</em>' +
                                    '</br>' +
                                    '<em>Final: ' + moment(info.event.end).format('YYYY-MM-DD h:mm A') + '</em>';
                            } else {
                                fechas = '<em>Todo el dia</em>';
                            }

                            $(info.el).tooltip({
                                html: true,
                                title: '<span class="text-white">' + info.event.extendedProps.clienteNombre + '</span>' +
                                    '</br >' +
                                    '<small class="text-white">' + info.event.extendedProps.tipoEvento + '</small >' +
                                    '</br>' +
                                    '<small class="text-white">' + info.event.extendedProps.asesor + '</small >' +
                                    '</br>' + fechas +
                                    '</br >' + descripcion
                            });
                        }
                    }

                    //Si la vista es listaSemana
                    if (info.view.type == "timeGridWeek" || info.view.type == "timeGridDay") {
                        if (info.event.extendedProps.tipoEvento != undefined) {
                            $(info.el).append('<em>' + info.event.extendedProps.tipoEvento + '</em>');
                        }
                    }
                },
            });

            calendar.render();

            function AgregarEventos() {

                //Formatear horas
                var fechaIni = moment($("#inputStart").val(),).format('YYYY-MM-DD HH:mm:ss');
                var fechaFin = moment($("#inputEnd").val(),).format('YYYY-MM-DD HH:mm:ss')

                //Formatear a bool opcAllDay
                var opcAllDay = TodoElDia($("#inputAllDay").val());
                function TodoElDia(opc) {
                    if (opc == "true") {
                        return true;
                    }
                    return false;
                }

                //Generar modelo
                var modelo = {
                    'id': $("#inputId").val(),
                    'title': $("#inputTitle").val(),
                    'start': fechaIni,
                    'end': fechaFin,
                    'allDay': opcAllDay,
                    'backgroundColor': $("#inputBackgroundColor").val(),
                    'borderColor': 'white',
                    'textColor': "white",
                    'editable': true,
                    'extendedProps': {
                        clienteNombre: $("#inputClienteNombre").val(),
                        tipoEvento: $("#inputTipoEvento").val(),
                        descripcion: $("#inputDescripcion").val(),
                        asesorID: $("#inputAsesorID").val(),
                        asesor: $("#inputAsesorNombre").val(),
                        enviado: false,
                        cancelada: $("#inputCancelada").val(),
                        soporte: $("#inputSoporte").val()
                    },
                }
                calendar.addEvent(modelo);
            }

            //Saber siempre en que vista del calendario me encuentro.
            $(".fc-listWeek-button,.fc-dayGridMonth-button,.fc-timeGridWeek-button,.fc-timeGridDay-button").click(function () {

                var clase = $(this).attr('class');
                var arrayClase = clase.split('-');
                $("#vistaDefault").val(arrayClase[1]);

            });

            function RefrescarEvento() {
                EliminarEvento();
                AgregarEventos();
            }

            function EliminarEvento() {

                calendar.getEventById($("#eventoID").val()).remove();
            }

            function LimpiarInputs() {

                $("#inputAgregar").prop("checked", false);
                $("#inputEditar").prop("checked", false);
                $("#inputEliminar").prop("checked", false);

                $(".modeloEventos").each(function (i, element) {
                    $(this).val("");
                });
            }

            $('#modal').on('hide.bs.modal', function () {

                if ($('#inputAgregar').prop('checked')) {

                    AgregarEventos();
                    //LimpiarInputs();

                } else if ($('#inputEditar').prop('checked')) {

                    RefrescarEvento();
                    //LimpiarInputs();

                } else if ($('#inputEliminar').prop('checked')) {

                    EliminarEvento();
                    //LimpiarInputs();
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAllEventos \n" + ex, "error");
        }
    },

    getCalendario: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Agenda/GetCalendario',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetCalendario').html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getCalendario \n" + ex, "error");
        }
    },

    getTiemposAsesoriasAsync: function (usuarioID, clienteID, fechaInicial, fechaFinal) {
        'use strict';

        try {

            var modelo = {
                IntUsuarioID: usuarioID,
                IntClienteID: clienteID,
                DatFechaInicial: fechaInicial,
                DatFechaFinal: fechaFinal
            };

            $.ajax({
                type: "POST",
                url: rootHost + 'Agenda/GetTiemposAsesoriasAsync',
                data: { modelo: modelo },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetTiemposAsesoria').empty().html(respuesta);
                   
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getTiemposAsesoriasAsync \n" + ex, "error");
        }
    },

    crearEventoAsync: function () {
        'use strict';

        $("#formCrearGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var fechaInicio = moment($("#citaFechaInicial").val(), 'YYYY-MM-DD h:m:s A');
                    var fechaFinal = moment($("#citaFechaFinal").val(), 'YYYY-MM-DD h:m:s A')

                    if (fechaInicio >= fechaFinal) {
                        swal.fire("Validación", "La fecha final debe ser mayor o diferente a la fecha inicial", "info");
                        return;
                    }

                    var horaInicialFech = String(fechaInicio.format("HH:mm:ss"));
                    var finalFech = String(fechaFinal.format("YYYY-MM-DD"));
                    var inicialFech = ($("#citaFechaInicial").val()).substring(0, 10);
                    var horaFinalFech = String(fechaFinal.format("HH:mm:ss"));

                    var opcAllDay = false;
                    if (horaInicialFech == $("#horaInicial").val() && finalFech == inicialFech && horaFinalFech == $("#horaFinal").val()) {
                        opcAllDay = true;
                    }

                    var modelo = {

                        "StrTitulo": $("#citaTitulo").val(),
                        "StrDescripcion": $("#citaDescripcion").val(),
                        "DatFechaInicial": moment($("#citaFechaInicial").val(), 'YYYY-MM-DD h:m:s A').format('YYYY-MM-DD hh:mm A'),
                        "DatFechaFinal": moment($("#citaFechaFinal").val(), 'YYYY-MM-DD h:m:s A').format('YYYY-MM-DD hh:mm A'),
                        "OpcAllDay": opcAllDay,
                        "OpcEnviado": false,
                        "OpcCancelada": false,
                        "IntClienteID": $("#citaCliente").val(),
                        "IntTipoEventoID": $("#citaTipoEvento").val(),
                        "OpcLunch": $('#opcLunch').is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Agenda/CrearEventoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo, fechaInicialCalendario: $("#fechaInicialProgramacion").val(), fechaFinalCalendario: $("#fechaFinalProgramacion").val() },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var eventos = JSON.parse(respuesta.modeloEventos);

                                agendaCRUD.actualizarInputsEvento(eventos);

                                if (respuesta.superaHoras != null)
                                    swal.fire("¡Alerta!", respuesta.superaHoras, "warning");

                                if (respuesta.validacionEventoAsesores != "") {
                                    var mensaje = respuesta.validacionEventoAsesores.replace(",", ", ");
                                    swal.fire("¡Validación!", `${mensaje}.`, "warning");
                                }

                                $("#inputAgregar").prop("checked", true);
                                $("#inputEditar").prop("checked", false);
                                $("#inputEliminar").prop("checked", false);
                                $('#modal').modal('hide');

                            } else {

                                swal.fire("¡Validación!", respuesta.error, "warning");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearEventoAsync\n" + ex, "error");
            }
        });
    },

    getEvento: function (eventoID) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url : rootHost + 'Agenda/GetEvento',
                data: { EventoID: eventoID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#contenedor').empty().html(respuesta);
                    $('#modal').modal('show');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getEvento \n" + ex, "error");
        }
    },

    updateEventoAsync: function () {
        'use strict';

        $("#formEditarGlobal").submit(function (e) {
            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var fechaInicio = moment($("#citaFechaInicial").val(), 'YYYY-MM-DD h:m:s A');
                    var fechaFinal = moment($("#citaFechaFinal").val(), 'YYYY-MM-DD h:m:s A')

                    if (fechaInicio >= fechaFinal) {
                        swal.fire("Validación", "La fecha final debe ser mayor o diferente a la fecha inicial", "warning");
                        return;
                    }

                    //Validar si se seleccionó un horario de todo el dia
                    var horaInicialFech = String(fechaInicio.format("HH:mm:ss"));
                    var finalFech = String(fechaFinal.format("YYYY-MM-DD"));
                    var inicialFech = ($("#citaFechaInicial").val()).substring(0, 10);
                    var horaFinalFech = String(fechaFinal.format("HH:mm:ss"));

                    var opcAllDay = false;
                    if (horaInicialFech == $("#horaInicial").val() && finalFech == inicialFech && horaFinalFech == $("#horaFinal").val()) {
                        opcAllDay = true;
                    }

                    var modelo = {

                        "IntAgendaID": $("#agendaID").val(),
                        "StrTitulo": $("#citaTitulo").val(),
                        "StrDescripcion": $("#citaDescripcion").val(),
                        "DatFechaInicial": moment($("#citaFechaInicial").val(), 'YYYY-MM-DD h:m:s A').format('YYYY-MM-DD hh:mm A'),
                        "DatFechaFinal": moment($("#citaFechaFinal").val(), 'YYYY-MM-DD h:m:s A').format('YYYY-MM-DD hh:mm A'),
                        "OpcAllDay": opcAllDay,
                        "OpcEnviado": false,
                        "OpcCancelada": false,
                        "IntClienteID": $("#citaCliente").val(),
                        "IntTipoEventoID": $("#citaTipoEvento").val(),
                        "IntUsuarioID": $("#usuarioID").val(),
                        "OpcLunch": $('#opcLunch').is(':checked')
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Agenda/UpdateEventoAsync',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var eventos = JSON.parse(respuesta.modeloEventos);

                                agendaCRUD.actualizarInputsEvento(eventos);

                                $("#inputAgregar").prop("checked", false);
                                $("#inputEditar").prop("checked", true);
                                $("#inputEliminar").prop("checked", false);

                                $('#modal').modal('hide');

                                if (respuesta.superaHoras != null) {
                                    swal.fire("¡Alerta!", respuesta.superaHoras, "warning");
                                    return;
                                }

                                swal.fire("¡Notificación!", "La visita se modificó correctamente.", "success");



                            } else {

                                swal.fire("¡Validación!", respuesta.error, "warning");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                        }
                    });
                }

            } catch (ex) {

                swal.fire("Oops!", "Error en el método crearClienteAsync \n" + ex, "error");
            }
        });
    },

    marcarLlegadaAsync: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Agenda/MarcarHoraDeLlegadaAsync',
                data: { agendaID: $("#agendaID").val() },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                        $("#divBtnMarcarLlegada").addClass("d-none");

                        //$('#contenedor').empty();
                        ////$('#modal').modal('hide');

                        ////agendaCRUD.getEvento($("#agendaID").val());

                    } else {

                        swal.fire("¡Validación!", respuesta.error, "warning");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });


        } catch (ex) {

            swal.fire("Oops!", "Error en el método crearClienteAsync \n" + ex, "error");
        }
    },

    updateEventoDropResizeAsync: function (obj, tipo) {
        'use strict';

        try {

            var fechaInicio = moment(obj.event.start, 'YYYY-MM-DD h:m:s A');
            var fechaFinal = moment(obj.event.end, 'YYYY-MM-DD h:m:s A');

            //Validar si se seleccionó un horario de todo el dia
            var inicialFech = String(fechaInicio.format("YYYY-MM-DD"));
            var horaInicialFech = String(fechaInicio.format("HH:mm:ss"));
            var finalFech = String(fechaFinal.format("YYYY-MM-DD"));
            var horaFinalFech = String(fechaFinal.format("HH:mm:ss")) != null ? String(fechaFinal.format("HH:mm:ss")) : null;

            var opcAllDay = false;

            if (horaFinalFech != null) {
                if (horaInicialFech == $("#horaInicial").val() && finalFech == inicialFech && horaFinalFech == $("#horaFinal").val()) {
                    opcAllDay = true;
                }
            }

            var modelo = {

                "IntAgendaID": obj.event.id,
                "DatFechaInicial": moment(obj.event.start).format('YYYY-MM-DD HH:mm:ss'),
                "DatFechaFinal": moment(obj.event.end).format('YYYY-MM-DD HH:mm:ss'),
                "OpcAllDay": opcAllDay /*obj.event.allDay*/
            };

            $.ajax({
                type: "POST",
                url: rootHost + 'Agenda/UpdateEventoDropResizeAsync',
                data: { modelo: modelo, tipo: tipo },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {
                        if (respuesta.superaHoras != null) {
                            swal.fire("¡Alerta!", respuesta.superaHoras, "warning");

                        }

                        return true;
                    } else {
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método crearClienteAsync \n" + ex, "error");
        }

    },

    cancelarVisitaAsync: function (id) {
        'use strict'
        try {
            $("#eventoID").val($(id).data("eventoid"));

            var token = $('input[name="__RequestVerificationToken"]').val();

            agendaCRUD.limpiarInputsCalendario();

            var descripcion = $("#citaDescripcion").val();
            if (descripcion == "") {
                swal.fire("¡Validación!", "Por favor ingrese la razón de la cancelación de la visita en el campo descripción.", "warning");
                return;
            }

            swal.fire({
                title: "¡Cancelación!",
                text: "¿Está seguro de cancelar la visita programada?",
                type: "warning",

                showCancelButton: true,
                confirmButtonText: "SI",
                cancelButtonText: "NO",
                closeOnConfirm: true,
                reverseButtons: true
            }).then(function (resultado) {

                if (resultado.isConfirmed) {

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'Agenda/CancelarEventoAsync',
                        data: { __RequestVerificationToken: token, EventoID: $(id).data("eventoid") },
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        dataType: "JSON",
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                var eventos = JSON.parse(respuesta.modeloEventos);

                                agendaCRUD.actualizarInputsEvento(eventos);

                                $("#inputAgregar").prop("checked", false);
                                $("#inputEditar").prop("checked", true);
                                $("#inputEliminar").prop("checked", false);

                                swal.fire("¡Notificación!", "La visita fue cancelada correctamente.", "success");
                                $('#modal').modal('hide');

                            } else {
                                swal.fire("¡Notificación!", respuesta.error, "warning");
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                        }
                    });
                } else swal.fire("¡Alerta!", "La visita no fue cancelada.", "warning");

            });


        } catch (ex) {

            swal.fire("Oops!", "Error en el método cancelarVisitaAsync \n" + ex, "error");
        }

    },

    deleteEventoAsync: function (id) {
        'use strict';
        try {

            $("#eventoID").val($(id).data("eventoid"));

            var token = $('input[name="__RequestVerificationToken"]').val();

            agendaCRUD.limpiarInputsCalendario();

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

                                swal.fire("¡Notificación!", "La visita programada se eliminó correctamente.", "success");

                                $("#inputAgregar").prop("checked", false);
                                $("#inputEditar").prop("checked", false);
                                $("#inputEliminar").prop("checked", true);

                                $('#modal').modal('hide');


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

            swal.fire("Oops!", "Error en el método deleteEventoAsync \n" + ex, "error");
        }
    },

    validarEventoAsync: function (dropInfo, draggedEvent) {
        'use strict';

        try {

            var usuarioID = $("#usuarioID").val();
            var fechaInicial = moment(dropInfo.start, 'YYYY-MM-DD hh:mm A').format('YYYY-MM-DD hh:mm A');
            var fechaFinal = moment(dropInfo.end, 'YYYY-MM-DD hh:mm A').format('YYYY-MM-DD hh:mm A');
            var eventoID = draggedEvent.id;

            $.ajax({
                type: "POST",
                url: rootHost + 'Agenda/ValidarEventoAsync',
                data: { EventoID: eventoID, UsuarioID: usuarioID, FechaInicial: fechaInicial, FechaFinal: fechaFinal },
                dataType: 'JSON',
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                async: true,
                success: function (respuesta) {

                    if (respuesta.msn === "success") {

                        swal.fire('Notificación', 'evento modificado', 'info');

                    } else {

                        swal.fire("¡Error!", respuesta.error, "error");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus);
                }
            });



        } catch (ex) {

            swal.fire("Oops!", "Error en el método validarEventoAsync\n" + ex, "error");
        }


    },

    refrescarCalendario: function () {
        'use strict'
        try {
            $('#asesorBuscar').val(0).trigger('change.select2');
            $('#clienteBuscar').val(0).trigger('change.select2');
            $('#tipoEventoBuscar').val(0).trigger('change.select2');

            agendaCRUD.getCalendario();


        } catch (ex) {

            swal.fire("Oops!", "Error en el método refrescarCalendario\n" + ex, "error");
        }

    },

    limpiarInputsCalendario: function () {

        $("#inputAgregar").prop("checked", false);
        $("#inputEditar").prop("checked", false);
        $("#inputEliminar").prop("checked", false);

        $(".modeloEventos").each(function (i, element) {
            $(this).val("");
        });

    },

    actualizarInputsEvento: function (eventos) {

        $("#eventoID").val(eventos.id);
        $("#inputId").val(eventos.id);
        $("#inputTitle").val(eventos.title);
        $("#inputStart").val(eventos.start);
        $("#inputEnd").val(eventos.end);
        $("#inputBackgroundColor").val(eventos.backgroundColor);
        $("#inputClienteNombre").val(eventos.clienteNombre);
        $("#inputTipoEvento").val(eventos.tipoEvento);
        $("#inputDescripcion").val(eventos.descripcion);
        $("#inputAsesorID").val(eventos.asesorID);
        $("#inputAsesorNombre").val(eventos.asesorNombre);
        $("#inputAllDay").val(eventos.allDay);
        $("#inputEnviado").val(eventos.enviado);
        $("#inputDescripcion").val(eventos.descripcion);
        $("#inputCancelada").val(eventos.cancelada);
        $("#inputSoporte").val(eventos.soporte);

    }

};