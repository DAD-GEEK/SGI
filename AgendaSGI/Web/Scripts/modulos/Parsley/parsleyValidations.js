(function (window, document, $, undefined) {
    'use strict';

    $(initParsleyForPages);

    function initParsleyForPages() {

        // Parsley options setup for bootstrap validation classes
        var parsleyOptions = {
            errorClass: 'is-invalid',
            successClass: 'is-valid',
            classHandler: function (ParsleyField) {
                var el = ParsleyField.$element.parents('.form-group').find('input');
                if (!el.length) // support custom checkbox
                    el = ParsleyField.$element.parents('.c-checkbox').find('label');
                return el;
            },
            errorsContainer: function (ParsleyField) {
                return ParsleyField.$element.parents('.form-group');
            },
            errorsWrapper: '<div class="text-help">',
            errorTemplate: '<div></div>'
        };

        //// Register form validation with Parsley

        var login = $("form");
        if (login.length)
            login.parsley(parsleyOptions);

        var login = $(".validarParsley");
        if (login.length)
            login.parsley(parsleyOptions);

    }

})(window, document, window.jQuery);


//Cambiar de pestaña al validar
//window.Parsley.on('field:error', function () {
//    // This global callback will be called for any field that fails validation.
//    this.$element.closest('.tab-pane').addClass('validation_error');
//    $('.tab-content .tab-pane.validation_error:eq(0)').addClass('first_tab_with_errors');
//    current_tab_id = $('.tab-content .tab-pane.validation_error.first_tab_with_errors').attr('id');

//    $('.nav-pills a[href="' + '#' + current_tab_id + '"]').tab('show');
//    $('.nav-pills a[href="' + '#' + (this.$element.closest('.tab-pane').attr('id')) + '"]').addClass('validation_errors');

//});

//window.Parsley.on('field:success', function () {
//    // This global callback will be called for any field that fails validation.
//    if (this.$element.closest('.validation_error').children().find('.parsley-error').length) {
//    } else {
//        $('.nav-pills a[href="' + '#' + (this.$element.closest('.tab-pane').attr('id')) + '"]').removeClass('validation_errors');
//    }
//});
