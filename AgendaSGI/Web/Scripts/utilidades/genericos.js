$(document).ready(function () {

    //Método que permite validar que solo se ingresen números en el campo donde se ponga la clase .numerico
    $('body').on('keypress', '.numerico', function (key) {
        var charCode = key.charCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
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

});

