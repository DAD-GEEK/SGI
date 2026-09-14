var ausentismoIndicadores = {

    index: function () {
        'use strict'



    },

    //Indicadores principales
    getAll: function () {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'AusentismoIndicadores/GetAll',
                data: {},
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetAllIndicadores').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método getAll \n" + ex, "error");
        }
    },

    editarIndicador: function (indicadorID) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'AusentismoIndicadores/EditarIndicador',
                data: { indicadorID: indicadorID },
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
            swal.fire("Oops!", "Error en el método editarIndicador \n" + ex, "error");
        }
    },

    guardarIndicador: function () {
        'use strict';

        $("#formGuardarIndicador").submit(function (e) {

            try {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var modelo = {

                        "StrId": $("#indicadorID").val(),
                        "StrNombreIndicador": $("#indicadorNombre").val(),
                        "StrFormula": $("#indicadorFormula").val(),
                        "StrFuente": $("#indicadorFuente").val(),
                        "StrFuente": $("#indicadorFuente").val(),
                        "StrObjetivo": $("#indicadorObjetivo").val()
                    }

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'AusentismoIndicadores/GuardarIndicador',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: "JSON",
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                ausentismoIndicadores.getAll();
                                swal.fire("¡Notificación!", "El registro se guardó correctamente.", "success");
                                $('#modal').modal('hide');
                                $('#contenedor').empty();

                            } else
                                swal.fire("¡Error!", respuesta.error, "error");

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                }


            } catch (ex) {
                swal.fire("Oops!", "Error en el método guardarIndicador \n" + ex, "error");
            }

        });
    },
    //Fin indicadores principales

    abrirVistaGraficosPorIndicador: function (ausentismoCodigoIndicador, canvasID) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'AusentismoIndicadores/CargarGraficosPorIndicador',
                data: { ausentismoCodigoIndicador: ausentismoCodigoIndicador, canvasID: canvasID },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#div_' + ausentismoCodigoIndicador).empty().html(respuesta);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método abrirVistaGraficosPorIndicador \n" + ex, "error");
        }
    },

    guardarInformacionDeIndicadorPorEmpresa: function (ausentismoCodigoIndicador, canvasID) {
        'use strict'
        try {

            $("#form_" + ausentismoCodigoIndicador).submit(function (e) {

                e.preventDefault();

                var form = $(this);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    var frecuanciaIndicador = $("#indicadorFrecuencia_" + ausentismoCodigoIndicador).val();
                    var meta_1 = $("#meta_1_" + ausentismoCodigoIndicador).val();

                    if (frecuanciaIndicador == "anual")
                        meta_1 = $("#meta_anual_" + ausentismoCodigoIndicador).val();

                    var modelo = {
                        "StrId": $("#indicadorID_" + ausentismoCodigoIndicador).val(),
                        "StrAusentismoIndicadoresID": $("#indicadorPrincipalID_" + ausentismoCodigoIndicador).val(),
                        "StrProceso": $("#indicadorProceso_" + ausentismoCodigoIndicador).val(),
                        "StrCargos": $("#indicadorCargo_" + ausentismoCodigoIndicador).val(),
                        "StrResponsable": $("#indicadorResponsable_" + ausentismoCodigoIndicador).val(),
                        "StrTipoGrafico": $("#indicadorTipoGrafico_" + ausentismoCodigoIndicador).val(),
                        "StrFrecuencia": $("#indicadorFrecuencia_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_1": meta_1,
                        "IntMeta_2": $("#meta_2_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_3": $("#meta_3_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_4": $("#meta_4_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_5": $("#meta_5_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_6": $("#meta_6_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_7": $("#meta_7_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_8": $("#meta_8_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_9": $("#meta_9_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_10": $("#meta_10_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_11": $("#meta_11_" + ausentismoCodigoIndicador).val(),
                        "IntMeta_12": $("#meta_12_" + ausentismoCodigoIndicador).val()
                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'AusentismoIndicadores/GuardarInformacionDeIndicadorPorEmpresa',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        async: true,
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        success: function (respuesta) {

                            if (respuesta.msn === "success") {

                                ausentismoIndicadores.abrirVistaGraficosPorIndicador(ausentismoCodigoIndicador, canvasID);
                                $("#indicadorID_" + ausentismoCodigoIndicador).val(respuesta.id);
                                return toastr.success('La información del indicador se guardó correctamente.');

                            } else swal.fire("¡Alerta!", respuesta.error, "warning");
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });

                }
            });



        } catch (ex) {

            swal.fire("¡Error!", "Error en el método guardarInformacionDeIndicadorPorEmpresa \n" + ex, "error");
        }

    },

    obtenerDatosDeIndicadores_MedicionGeneralAusentismo: function (anio, tipoDeAusentismo) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'AusentismoIndicadores/ObtenerDatosDeIndicadores_MedicionGeneralAusentismo',
                data: { anio: anio, tipoDeAusentismo: tipoDeAusentismo },
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    var dataMedicionGeneralAusentismo = respuesta.indicadores.MedicionGeneralAusentismo;
                    var dataMedicionGeneralAusentismo_Chart = respuesta.indicadores.MedicionesGeneralAusentismo_DatosChart;
                    var canvasID = respuesta.indicadores.CanvasID;

                    $("#medicionGeneralAusentismo_totalIncapacidades").html(dataMedicionGeneralAusentismo[0].NumeroTotalIncapacidades);
                    $("#medicionGeneralAusentismo_totalDias").html(dataMedicionGeneralAusentismo[0].NumeroTotalDias);
                    $("#medicionGeneralAusentismo_dias_EG_AC").html(dataMedicionGeneralAusentismo[0].NumeroDiasPorEnfermedadGeneral_AccidenteComun);
                    $("#medicionGeneralAusentismo_incapacidades_EG_AC").html(dataMedicionGeneralAusentismo[0].NumeroIncapacidadesPorEnfermedadGeneral_AccidenteComun);

                    $("#medicionGeneralAusentismo_anio").html(respuesta.anio);

                    var ticksStyle = {
                        fontColor: '#495057',
                        fontStyle: 'bold'
                    }

                    var mode = 'index'
                    var intersect = true

                    var $informacionChart = $("#" + canvasID);
                    // eslint-disable-next-line no-unused-vars
                    var informacionChart = new Chart($informacionChart, {
                        type: 'bar',
                        data: dataMedicionGeneralAusentismo_Chart,
                        options: {
                            maintainAspectRatio: false,
                            tooltips: {
                                mode: mode,
                                intersect: intersect
                            },
                            hover: {
                                mode: mode,
                                intersect: intersect,
                                animationDuration: 0,
                            },
                            legend: {
                                display: false,
                            },
                            scales: {
                                yAxes: [{
                                    display: true,
                                    gridLines: { //Si se oculta muestra lineas cuadriculadas en el gráfico, sino, solo lineas verticales
                                        display: true,
                                        lineWidth: '4px',
                                        color: 'rgba(0, 0, 0, .2)',
                                        zeroLineColor: 'transparent'
                                    },
                                    ticks: $.extend({
                                        beginAtZero: true,

                                        // Include a dollar sign in the ticks
                                        callback: function (value) {
                                            //if (value >= 1000) {
                                            //    value /= 1000
                                            //    value += 'k'
                                            //}

                                            return value
                                        }
                                    }, ticksStyle)
                                }],
                                xAxes: [{
                                    display: true,
                                    gridLines: {
                                        display: true
                                    },
                                    ticks: ticksStyle
                                }]
                            },
                            animation: {
                                duration: 3000,
                                easing: "easeOutQuart",
                                onComplete: function () {
                                    var ctx = this.chart.ctx;
                                    ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontFamily, 'normal', Chart.defaults.global.defaultFontFamily);
                                    ctx.textAlign = 'center';
                                    ctx.textBaseline = 'bottom';

                                    this.data.datasets.forEach(function (dataset) {
                                        for (var i = 0; i < dataset.data.length; i++) {
                                            var model = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._model,
                                                scale_max = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._yScale.maxHeight;
                                            ctx.fillStyle = '#444';
                                            var y_pos = model.y - 5;
                                            // Make sure data value does not get overflown and hidden
                                            // when the bar's value is too close to max value of scale
                                            // Note: The y value is reverse, it counts from top down
                                            if ((scale_max - model.y) / scale_max >= 0.93)
                                                y_pos = model.y + 20;
                                            ctx.fillText(dataset.data[i], model.x, y_pos);
                                        }
                                    });
                                }
                            }
                        }
                    })

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerDatosDeIndicadores_MedicionGeneralAusentismo \n" + ex, "error");
        }
    },

 
    obtenerVistaIndicadores: function (anio, tipoDeAusentismo) {
        'use strict';

        try {

            $.ajax({
                type: "GET",
                url: rootHost + 'AusentismoIndicadores/ObtenerVistaIndicadores',
                data: { anio: anio, tipoDeAusentismo: tipoDeAusentismo },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetIndicadores').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método obtenerVistaIndicadores \n" + ex, "error");
        }
    },

    ObtenerDatosDeIndicadoresParaGraficar: function (ausentismoCodigoIndicador, canvasID) {
        'use strict';

        try {

            var anio = $("#ausentismoIndicaroresAnio").val();

            $.ajax({
                type: "POST",
                url: rootHost + 'AusentismoIndicadores/ObtenerDatosDeIndicadoresParaGraficar',
                data: { ausentismoCodigoIndicador: ausentismoCodigoIndicador, anio: anio },
                dataType: "JSON",
                async: true,
                success: function (respuesta) {

                    if (respuesta.error)
                        return $("#div_" + ausentismoCodigoIndicador).html(`
                        <div class="alert alert-danger block-center" role="alert">
                            <i class="fas fa-chart-area mr-1"></i> Ingrese la información del indicador y presione el botón <b>Guardar</b> para generar el gráfico.
                         </div>`);

                    var informacionIndicador = respuesta.informacionIndicador;
                    var informacionIndicadorGrafico = respuesta.informacionIndicador.Grafico;

                    var ticksStyle = {
                        fontColor: '#495057',
                        fontStyle: 'bold'
                    }

                    console.log("aquí");

                    var mode = 'index';
                    var intersect = true;

                    var $informacionChart = $("#" + canvasID);
                    var informacionChart = new Chart($informacionChart, {
                        type: 'bar',
                        data: informacionIndicadorGrafico,
                        options: {
                            //maintainAspectRatio: false,
                            tooltips: {
                                mode: mode,
                                intersect: intersect
                            },
                            hover: {
                                mode: mode,
                                intersect: intersect,
                                animationDuration: 0,
                            },
                            legend: {
                                display: false,
                            },
                            scales: {
                                yAxes: [{
                                    display: true,
                                    text: 'Dias de incapacidad',
                                    gridLines: {
                                        display: true,
                                        lineWidth: '4px',
                                        color: 'rgba(0, 0, 0, .2)',
                                        zeroLineColor: 'transparent'
                                    },
                                    ticks: $.extend({
                                        beginAtZero: true,
                                        userCallback: function (label, index, labels) {
                                            // when the floored value is the same as the value we have a whole number
                                            if (Math.floor(label) === label) {
                                                return label;
                                            }

                                        },
                                        // Include a dollar sign in the ticks
                                        callback: function (value) {
                                            //if (value >= 1000) {
                                            //    value /= 1000
                                            //    value += 'k'
                                            //}

                                            return value
                                        }
                                    }, ticksStyle),
                                }],
                                xAxes: [{
                                    display: true,
                                    gridLines: {
                                        display: true
                                    },
                                    ticks: ticksStyle
                                }],
                                precision: 0
                            },
                            animation: {
                                duration: 3000,
                                easing: "easeOutQuart",
                                onComplete: function () {
                                    var ctx = this.chart.ctx;
                                    ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontFamily, 'normal', Chart.defaults.global.defaultFontFamily);
                                    ctx.textAlign = 'center';
                                    ctx.textBaseline = 'bottom';

                                    this.data.datasets.forEach(function (dataset) {
                                        for (var i = 0; i < dataset.data.length; i++) {
                                            var model = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._model,
                                                scale_max = dataset._meta[Object.keys(dataset._meta)[0]].data[i]._yScale.maxHeight;
                                            ctx.fillStyle = '#444';
                                            var y_pos = model.y - 5;
                                            // Make sure data value does not get overflown and hidden
                                            // when the bar's value is too close to max value of scale
                                            // Note: The y value is reverse, it counts from top down
                                            if ((scale_max - model.y) / scale_max >= 0.93)
                                                y_pos = model.y + 20;
                                            ctx.fillText(dataset.data[i], model.x, y_pos);
                                        }
                                    });
                                }
                            }
                        }
                    })



                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método ObtenerDatosDeIndicadoresParaGraficar \n" + ex, "error");
        }
    },


};
