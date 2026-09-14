var DocumentosDiagnosticoDetalle_PlanesDeAccionCRUD = {

    //Vistas
    //Base de datos
    guardarRegistro: function () {
        'use strict';

        try {

            $('.criteriosPorPaso').on('click', 'button', function () {

                var formularioID = $(this).data("formularioid");

                var form = $("#" + formularioID);

                form.parsley().validate();

                if (form.parsley().isValid()) {

                    console.log($(this));

                    let elementoID = $(this).attr("id");

                    var modelo = {

                        "StrCalificacionID": $("#pasoDetalleID_" + elementoID).val(),
                        "BitAplica": $("#pasoDetalleAplica_" + elementoID).is(":checked"),
                        "BitSeEvidencia": $("#pasoDetalleCumple_" + elementoID).is(":checked"),
                        "StrComoSeEvidencia": $("#pasoDetalleComoEvidencia_" + elementoID).val(),
                        "StrPlanDeAccion": $("#pasoDetallePlanDeAccion_" + elementoID).val(),
                        "StrResponsable": $("#pasoDetalleResponsable_" + elementoID).val(),
                        "DatFecha": $("#pasoDetalleFecha_" + elementoID).val(),

                    };

                    var token = $('input[name="__RequestVerificationToken"]').val();

                    $.ajax({
                        type: "POST",
                        url: rootHost + 'DocumentosDiagnosticoDetalle_PlanesDeAccion/Guardar',
                        data: { __RequestVerificationToken: token, modelo: modelo },
                        dataType: 'JSON',
                        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
                        async: true,
                        success: function (respuesta) {

                            var id = form.data("id");

                            var lblEstadoCriterio = $("#lblEstadoCriterio_" + id);

                            if (respuesta.msn === "success") {                               

                                lblEstadoCriterio.removeClass("badge-danger");
                                lblEstadoCriterio.addClass("badge-success");
                                lblEstadoCriterio.html("OK");
                                
                                toastr.success("Criterio calificado exitosamente.");

                            } else {

                                lblEstadoCriterio.addClass("badge-danger");
                                lblEstadoCriterio.removeClass("badge-success");
                                lblEstadoCriterio.html("PENDIENTE");

                                swal.fire("¡Alerta!", respuesta.error, "warning");
                            }

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            funcionesGlobales.generarAjaxMensajeException(jqXHR, textStatus)
                        }
                    });
                }
            });

        } catch (ex) {

            swal.fire("Oops!", "Error en el método crearDocumento \n" + ex, "error");
        }
    },

    seleccionarCheckboxAplicar: function (elemento) {

        DocumentosDiagnosticoDetalle_PlanesDeAccionCRUD.calcularResultado(elemento);

        var elementoID = $(elemento).data("id");
        var divCumple = $("#divCumple_" + elementoID);
        var divDesarrollo = $("#divDesarrollo_" + elementoID);
        var divCumpleSICumple = $(".divDesarrolloSICumple_" + elementoID);
        var divCumpleNOCumple = $(".divDesarrolloNOCumple_" + elementoID);
        var checkboxCumple = $("#pasoDetalleCumple_" + elementoID);


        var isChecked = $(elemento).is(":checked");
        var isCheckedCumple = $("#pasoDetalleCumple_" + elementoID).is(":checked");

        if (isChecked) {
            $("#lblPasoDetalleAplica_" + elementoID).html("SI aplica");
            divCumple.removeClass('d-none');
            divDesarrollo.removeClass('d-none');

            if (isCheckedCumple) {
                divCumpleSICumple.attr("data-parsley-required", true);
                divCumpleNOCumple.attr("data-parsley-required", false);
            }
            else {
                divCumpleSICumple.attr("data-parsley-required", false);
                divCumpleNOCumple.attr("data-parsley-required", true);

                $("#lblPasoDetalleCumple_" + elementoID).html("NO cumple");
                divCumple.addClass('text-danger');
                divCumpleSICumple.addClass("d-none");
                divCumpleSICumple.attr("data-parsley-required", false);

                divCumpleNOCumple.removeClass("d-none");
                divCumpleNOCumple.attr("data-parsley-required", true);
            }
        }
        else {
            $("#lblPasoDetalleAplica_" + elementoID).html("NO aplica");
            divCumple.addClass('d-none');
            divDesarrollo.addClass('d-none');
            checkboxCumple.prop("checked", false);
            divCumpleSICumple.attr("data-parsley-required", false);
            divCumpleNOCumple.attr("data-parsley-required", false);
        }

    },

    seleccionarCheckboxCumple: function (elemento) {

        DocumentosDiagnosticoDetalle_PlanesDeAccionCRUD.calcularResultado(elemento);

        var elementoID = $(elemento).data("id");
        var isChecked = $(elemento).is(":checked");
        var divCumple = $("#divCumple_" + elementoID);
        var divCumpleSICumple = $(".divDesarrolloSICumple_" + elementoID);
        var divCumpleNOCumple = $(".divDesarrolloNOCumple_" + elementoID);

        if (isChecked) {

            $("#lblPasoDetalleCumple_" + elementoID).html("SI cumple");
            divCumple.removeClass('text-danger');
            divCumpleSICumple.removeClass("d-none");
            divCumpleSICumple.attr("data-parsley-required", true);

            divCumpleNOCumple.addClass("d-none");
            divCumpleNOCumple.attr("data-parsley-required", false);
        }
        else {

            $("#lblPasoDetalleCumple_" + elementoID).html("NO cumple");
            divCumple.addClass('text-danger');
            divCumpleSICumple.addClass("d-none");
            divCumpleSICumple.attr("data-parsley-required", false);

            divCumpleNOCumple.removeClass("d-none");
            divCumpleNOCumple.attr("data-parsley-required", true);
        }

    },

    calcularResultado: function (elemento) {

        var pasoID = $(elemento).data("pasoid");
        var elementoCantidadSiAplica = $("#pasoCantidadSiAplica_" + pasoID);
        var elementoCantidadSICumple = $("#pasoCantidadSiCumple_" + pasoID);
        var elementoCantidadNOCumple = $("#pasoCantidadNOCumple_" + pasoID);
        var elementoResultado = $("#pasoResultado_" + pasoID);

        var contadorSIAplica = 0;

        $(".checkboxAplica_" + pasoID).each(function (i, elemento) {

            var isCheckedSIAplica = $(elemento).is(":checked");

            if (isCheckedSIAplica)
                contadorSIAplica = contadorSIAplica + 1;
        });

        elementoCantidadSiAplica.html(contadorSIAplica);

        var contadorSICumple = 0;

        if (contadorSIAplica != 0) {
            $(".checkboxCumple_" + pasoID).each(function (i, elemento) {

                var isCheckedSICumple = $(elemento).is(":checked");

                if (isCheckedSICumple)
                    contadorSICumple = contadorSICumple + 1;
            });
        }     

        if (contadorSICumple > contadorSIAplica)
            contadorSICumple = contadorSIAplica;

        elementoCantidadSICumple.html(contadorSICumple);

        var cantidadNOCumple = parseInt(contadorSIAplica) - parseInt(contadorSICumple);
        if (cantidadNOCumple < 0)
            cantidadNOCumple = 0;

        elementoCantidadNOCumple.html(cantidadNOCumple);

        var resultado = 0.0;

        if (contadorSIAplica > 0)
            resultado = parseInt((parseFloat(contadorSICumple) / parseFloat(contadorSIAplica) * 100));

        elementoResultado.html(resultado + "%");

    },

    //Documento diagnóstico pasos

};
