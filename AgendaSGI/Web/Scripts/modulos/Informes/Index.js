var indexInforme = {

    getBarrasHorasByAnoByAsesor: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Informes/GetBarrasHorasByAnoByAsesor',
                data: { ano: $("#barraByAno").val() },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "html",
                async: true,
                success: function (respuesta) {

                    $('#divGetBarrasHorasByAnoByAsesor').html(respuesta);
                },
                error: function (textStatus) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getBarrasHorasByAnoByAsesor \n" + ex, "error");
        }
    },

    datosBarrasHorasByAnoByAsesor: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Informes/AsesorByHorasAsync',
                data: { ano: $("#barraByAno").val() },
                dataType: "JSON",
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                success: function (data) {

                    if (data.msn == "success") {

                        var dataChart = JSON.parse(data.data);
                        var ticksStyle = {
                            fontColor: '#495057',
                            fontStyle: 'bold'
                        }

                        var mode = 'index'
                        var intersect = true

                        var $salesChart = $('#sales-chart')

                        var salesChart = new Chart($salesChart, {
                            type: 'bar',
                            data: {
                                labels: ['ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DIC'],
                                datasets: dataChart
                            },
                            options: {
                                maintainAspectRatio: false,
                                tooltips: {
                                    mode: mode,
                                    intersect: intersect
                                },
                                hover: {
                                    mode: mode,
                                    intersect: intersect
                                },
                                legend: {
                                    display: true,
                                    position: "top",

                                },
                                scales: {
                                    yAxes: [{
                                        // display: false,
                                        gridLines: {
                                            display: true,
                                            lineWidth: '10px',
                                            color: 'rgba(0, 0, 0, .2)',
                                            zeroLineColor: 'transparent'
                                        },
                                        ticks: $.extend({
                                            beginAtZero: true,
                                        }, ticksStyle)
                                    }],
                                    xAxes: [{
                                        display: true,
                                        gridLines: {
                                            display: true // Mostrar lineas de separación de los datos
                                        },
                                        ticks: ticksStyle
                                    }]
                                }
                            }
                        })

                    }
                    else if (data.error != null) {
                        if (data.exc == true) {
                            swal.fire("¡Error!", data.error, "error");
                            return;
                        }
                        swal.fire("¡Validación!", data.error, "info");

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    if (jqXHR.status === 0) {

                        alert('Not connect: Verify Network.');

                    } else if (jqXHR.status == 404) {

                        alert('Requested page not found [404]');

                    } else if (jqXHR.status == 500) {

                        alert('Internal Server Error [500].');

                    } else if (textStatus === 'parsererror') {

                        alert('Requested JSON parse failed.');

                    } else if (textStatus === 'timeout') {

                        alert('Time out error.');

                    } else if (textStatus === 'abort') {

                        alert('Ajax request aborted.');

                    } else {

                        alert('Uncaught Error: ' + jqXHR.responseText);

                    }

                }
            });

        } catch (e) {

            swal.fire("Oops!", "Error en el método datosBarrasHorasByAnoByAsesor \n" + ex, "error");

        }
    },

    getHorasContratoVSHorasMensuales: function () {
        'use strict';

        try {

            $.ajax({
                type: "POST",
                url: rootHost + 'Informes/GetHorasContratoVSHorasMensuales',
                data: { ano: $("#listaByAno").val(), mes: $("#listaByMes").val(), cliente: $("#busquedaByCliente").val() },
                contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                dataType: "html",
                success: function (respuesta) {

                    $('#divGetListaContratosMes').html(respuesta);
                },
                error: function (textStatus) {
                    swal.fire("Oops!", textStatus, "error");
                }
            });

        } catch (e) {
            swal.fire("Oops!", "Error en el método getHorasContratoVSHorasMensuales \n" + ex, "error");
        }
    },

    buscarHorasContratoVSHorasMensuales: function () {
        'use strict';

        try {

            var cliente = $("#busquedaByCliente").val();

            $("#detalleHorasContrato > tr").each(function (i, tr) {

                var identificacion = $(tr).find(".clienteIdentificacion").html();
                identificacion = (identificacion.replace(/\./gi, ''));

                var nombre = $(tr).find(".clienteNombre").html();

                if (identificacion.toLowerCase().includes(cliente.toLowerCase()) || nombre.toLowerCase().includes(cliente.toLowerCase())) {
                    $(tr).show();
                } else {
                    $(tr).hide();
                }
            });
        } catch (ex) {
            swal.fire("Oops!", "Error en el método buscarHorasContratoVSHorasMensuales \n" + ex, "error");
        }
    },


};