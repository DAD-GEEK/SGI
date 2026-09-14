
$(function () {
    //Parley JS
    $('form').parsley();

    //Select2
    $('.select2bs4').select2({
        theme: 'default',
        placeholder: "-- Seleccione una opción --",
        tags: true

    });

    $('[data-mask]').inputmask();

    $('[data-toggle="tooltip"]').tooltip();

    $('.fechaRango').daterangepicker({
        "autoUpdateInput": false,
        "applyButtonClasses": "btn-primary",
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
            "firstDay": 1
        },

        "opens": "center"
    },
        function (start, end, label) {

        });
    $('.fechaRango').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(`${picker.startDate.format('YYYY-MM-DD')} - ${picker.endDate.format('YYYY-MM-DD')}`);
    });
    $('.fechaRango').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });

    //Fecha individual
    $('.fechaIndividual').daterangepicker({
        "singleDatePicker": true,
        "autoUpdateInput": false,
        "applyButtonClasses": "btn-primary",
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
            "firstDay": 1
        },

        "opens": "center"
    },
        function (start, end, label) {

        });
    $('.fechaIndividual').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(`${picker.startDate.format('YYYY-MM-DD')}`);
    });
    $('.fechaIndividual').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });

    //Terceros
    $(".obtenerTercerosClientes").select2({
        ajax: {
            global: false,
            url: rootHost + 'Terceros_Clientes/ObtenerTercerosClientes',
            dataType: 'json',
            data: function (params) {
                return {
                    filtroTercero: params.term, // search term
                    page: params.page
                };
            },
            delay: 500,
            processResults: function (data, params) {

                params.page = params.page || 1;

                var contadorDeRegistros = 0;
                var listaElementos = [];

                $.each(data.listaRegistros, function (index, registro) {

                    var item = {
                        id: registro.IntTerceroClienteID,
                        text: registro.StrIdentificacion + " - " + registro.StrNombre,
                    }

                    listaElementos[contadorDeRegistros] = item;
                    contadorDeRegistros++;
                });

                return {
                    results: listaElementos,
                    pagination: {
                        more: (params.page * 30) < contadorDeRegistros
                    }
                };
            },
            cache: true
        },
        language: 'es',
        placeholder: '-- Seleccionar tercero --',
        minimumInputLength: 3,

    });

    $(".obtenerProcesos_TercerosClientes").select2({
        ajax: {
            global: false,
            url: rootHost + 'Procesos/ObtenerProcesos_TercerosClientes',
            dataType: 'json',
            data: function (params) {
                return {
                    filtroTercero: params.term, // search term
                    terceroClienteID: $("#terceroClienteID").val(), // search term
                    page: params.page
                };
            },
            delay: 500,
            processResults: function (data, params) {

                params.page = params.page || 1;

                var contadorDeRegistros = 0;
                var listaElementos = [];

                $.each(data.listaRegistros, function (index, registro) {

                    var item = {
                        id: registro.IntProcesoID,
                        text: registro.StrCodigo + " - " + registro.StrDescripcion,
                    }

                    listaElementos[contadorDeRegistros] = item;
                    contadorDeRegistros++;
                });

                return {
                    results: listaElementos,
                    pagination: {
                        more: (params.page * 30) < contadorDeRegistros
                    }
                };
            },
            cache: true
        },
        language: 'es',
        placeholder: '-- Seleccionar tercero --',
        minimumInputLength: 3,

    });

    $('#datetimepickerInicial').datetimepicker({
        "allowInputToggle": true,
        "showClose": true,
        "showClear": true,
        "showTodayButton": true,
        "format": "hh:mm A",
        "icons": {
            today: "fas fa-calendar-check",
        }
    });

    $('#datetimepickerFinal').datetimepicker({
        "allowInputToggle": true,
        "showClose": true,
        "showClear": true,
        "showTodayButton": true,
        "format": "hh:mm A",
        "icons": {
            today: "fas fa-calendar-check",
        }
    });


});




