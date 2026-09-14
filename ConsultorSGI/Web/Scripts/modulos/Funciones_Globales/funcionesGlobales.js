//Método para darle formato a los números tipo moneda EU
//$100,000
$('body').on('keyup', '.monedaEU', function (event) {

    var numero = $(this).val();
    var numero = parseInt(numero.replace(/,/gi, '').replace('$', ''));

    if (numero.length == 0 || isNaN(numero) || numero == 'undefined') numero = 0;

    var newNumero = new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        minimumFractionDigits: 0
    }).format(parseInt(numero));

    $(this).val(newNumero);

});

//Método que permite validar que solo se ingresen números en el campo donde se ponga la clase .numerico
$('body').on('keypress', '.numerico', function (key) {
    var charCode = key.charCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
});

//Método que permite validar que solo se ingresen números en el campo donde se ponga la clase .numerico
$('body').on('keypress', '.numeral', function (key) {
    var charCode = key.charCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
        return false;
});

//Método que permite llevar el conteo de los caracteres que se ingresen en un campo de celular .celularCount
$('body').on('keyup', '.celularCount', function () {
    $('#resultTextCelular').remove();
    $(this).parent().append("<span id='resultTextCelular'> Usted ha ingresado " + $(this).val().length + " de 10 números </span>");

});

//Método que permite validar que solo se ingresen letras en el campo donde se ponga la clase .letras
$('body').on('keypress', '.letras', function (key) {
    if ((key.charCode < 97 || key.charCode > 122)//letras mayusculas
        && (key.charCode < 65 || key.charCode > 90) //letras minusculas
        && (key.charCode != 45) //retroceso
        && (key.charCode != 241) //ñ
        && (key.charCode != 209) //Ñ
        && (key.charCode != 32) //espacio
        && (key.charCode != 225) //á
        && (key.charCode != 233) //é
        && (key.charCode != 237) //í
        && (key.charCode != 243) //ó
        && (key.charCode != 250) //ú
        && (key.charCode != 193) //Á
        && (key.charCode != 201) //É
        && (key.charCode != 205) //Í
        && (key.charCode != 211) //Ó
        && (key.charCode != 218) //Ú
        && (key.charCode != 0) //Ú
    )
        return false;
});


//Método que permite llevar el conteo de los caracteres que se ingresen en un campo de teléfono .telefonoCount
$('body').on('keyup', '.telefonoCount', function () {
    $('#resultTextTelefono').remove();
    $(this).parent().append("<span id='resultTextTelefono'> Usted ha ingresado " + $(this).val().length + " de 7 carácteres </span>");

});

var funcionesGlobales = {

    openVistaSeleccionarNumerales: function (datosGenericos) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: `${rootHost}FuncionesGlobales/ObtenerNumeralesParaSeleccionarAsync`,
                data: { datosGenericos: datosGenericos },
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divSeleccionarNumerales').empty().html(respuesta);

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                }
            });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método openVistaSeleccionarNumerales \n" + ex, "error");
        }
    },

    seleccionarNumeralsAsync: function (controlador, modelo) {
        'use strict';

        try {
            var token = $('input[name="__RequestVerificationToken"]').val();
            var url = rootHost + controlador + '/SeleccionarNumeralAsync';
            var data = { modelo: modelo };
            var options = {
                method: 'POST',
                body: JSON.stringify(data),
                headers: {
                    'Content-type': 'application/json',
                    'dataType': 'JSON',
                    'async': true,
                }
            };

            fetch(url, options)
                .then(res => res.json())
                .then(res => {

                    if (res.msn == "success") {

                        if (res.procesoID > 0) {
                            var esVistaGlobal = $("#esVistaGlobal").is(":checked");
                            plantillaListasDeVerificacionDetalleCRUD.getAllPlantillasListasDeVerificacionDetalleAsync(res.procesoID, esVistaGlobal);
                        }

                        toastr.success("Numeral actualizado");
                    } else {
                        toastr.error(res.error);
                    }
                })
                .catch(function () {

                });

        } catch (ex) {
            swal.fire("Oops!", "Error en el método buscarDiagnosticoAsync \n" + ex, "error");
        }

    },

    seleccionarPestanaSecundariaEnTab: function (pestanaPrincipalID, contenedorPrincipalID, PestanaSecundariaID, contenedorSecundarioID) {
        'use strict'

        $(`#${PestanaSecundariaID}`).addClass("active");
        $(`#${pestanaPrincipalID}`).removeClass("active");

        $(`#${contenedorSecundarioID}`).addClass("active");
        $(`#${contenedorPrincipalID}`).removeClass("active");

    },

    graficarDatosChart: function (canvasID, data) {

        var ticksStyle = {
            fontColor: '#495057',
            fontStyle: 'bold'
        }

        var mode = 'index'
        var intersect = true

        var $informacionChart = $(`#${canvasID}`);
        // eslint-disable-next-line no-unused-vars
        var informacionChart = new Chart($informacionChart, {
            type: 'bar',
            data: data,
            options: {
                maintainAspectRatio: false,
                //responsive: false,
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
                    display: true,
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

    vistaExportarPDFOptions: function (vistaExportacion) {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'FuncionesGlobales/VistaExportarPDFOptions',
                data: { vistaExportacion: vistaExportacion },
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
            swal.fire("Oops!", "Error en el método vistaExportarPDFOptions \n" + ex, "error");
        }
    },

    generarAjaxMensajeException: function (jqXHR, textStatus) {

        if (jqXHR.status === 0)
            swal.fire('¡Opps!', 'Sin conexión: Verifique su conexión a internet.', 'error');
        else if (jqXHR.status == 404)
            swal.fire('¡Opps!', 'Página no encontrada [404]', 'error');
        else if (jqXHR.status == 500)
            swal.fire('¡Opps!', 'Error interno del servidor [500]', 'error');
        else if (textStatus === 'parsererror')
            swal.fire('¡Opps!', 'Solicitud JSON parse ha fallado.', 'error');
        else if (textStatus === 'timeout')
            swal.fire('¡Opps!', 'El tiempo de espera ha terminado', 'error');
        else if (textStatus === 'abort')
            swal.fire('¡Opps!', 'La solicitud Ajax ha sido abortada.', 'error');
        else
            swal.fire('¡Opps!', 'Error no encontrado: ' + jqXHR.responseText, 'error');
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

};




