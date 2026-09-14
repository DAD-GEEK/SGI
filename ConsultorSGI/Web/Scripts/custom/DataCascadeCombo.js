$(document).on('change', '[data-cascade-combo]', function (event) {

    var id = $(this).attr('data-cascade-combo');

    var url = $(this).attr('data-cascade-combo-source');

    var paramName = $(this).attr('data-cascade-combo-param-name');

    var paramMultiple = $(this).attr('data-cascade-combo-multiple');

    var data = {};
    data[paramName] = id;

    $.ajax({
        url: url,
        data: { id: $(this).val() },
        success: function (data) {

            $(id).html('');

            if (paramMultiple == 'Multiple') {
                $.each(data,
                    function (index, type) {
                        var content = '<option selected="true" value="' + type.id + '">' + type.Nombre + '</option>';
                        $(id).append(content);
                    });
            } else {
                $.each(data,
                    function (index, type) {
                        var content = '<option value="' + type.id + '">' + type.Nombre + '</option>';
                        $(id).append(content);
                    });
            }



            $('.selectpicker').selectpicker('refresh');

            $(id).trigger("chosen:updated");
        }
    });
});