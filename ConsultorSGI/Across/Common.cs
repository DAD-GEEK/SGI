using Models.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace Across
{
    public static class Common
    {      
        public static DateTime GetFechaCompilacion(this Assembly assembly, TimeZoneInfo target = null)
        {
            try
            {
                var filePath = assembly.Location;
                const int c_PeHeaderOffset = 60;
                const int c_LinkerTimestampOffset = 8;

                var buffer = new byte[2048];

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    stream.Read(buffer, 0, 2048);

                var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
                var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

                var tz = target ?? TimeZoneInfo.Local;
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

                return localTime;
            }
            catch (Exception)
            {

                return DateTime.Now.AddYears(-2021);
            }

        }

        public static DateTime ObtenerFechaExacta(DateTime date)
        {
            try
            {
                return DateTime.ParseExact(date.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DateTime ObtenerFechaActualExacta()
        {
            try
            {
                var tZCode = "SA Pacific Standard Time";

                var zona = TimeZoneInfo.FindSystemTimeZoneById(tZCode);
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zona);

                return DateTime.ParseExact(localTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static IEnumerable<SelectListItem> GetEnumToSelectList<T>()
        {
            try
            {
                return (Enum.GetValues(typeof(T)).Cast<T>().Select(
               e => new SelectListItem() { Value = e.GetHashCode().ToString(), Text = e.ToString() })).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }     
        
        public static List<MesesDelAnioDTO> GetMesesDelAnio()
        {
            try
            {
                return (Enum.GetValues(typeof(EnumMesesDelAño)).Cast<EnumMesesDelAño>().Select(
              e => new MesesDelAnioDTO() { Numero = (short)e.GetHashCode(), Nombre = e.ToString() })).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static string ConvertirNumerosEnLetras(this int value)
        {
            try
            {
                string Num2Text = "";
                if (value < 0) return "menos " + Math.Abs(value).ConvertirNumerosEnLetras();

                if (value == 0) Num2Text = "ninguna";
                else if (value == 1) Num2Text = "una";
                else if (value == 2) Num2Text = "dos";
                else if (value == 3) Num2Text = "tres";
                else if (value == 4) Num2Text = "cuatro";
                else if (value == 5) Num2Text = "cinco";
                else if (value == 6) Num2Text = "seis";
                else if (value == 7) Num2Text = "siete";
                else if (value == 8) Num2Text = "ocho";
                else if (value == 9) Num2Text = "nueve";
                else if (value == 10) Num2Text = "diez";
                else if (value == 11) Num2Text = "once";
                else if (value == 12) Num2Text = "doce";
                else if (value == 13) Num2Text = "trece";
                else if (value == 14) Num2Text = "catorce";
                else if (value == 15) Num2Text = "quince";
                else if (value < 20) Num2Text = "dieci" + (value - 10).ConvertirNumerosEnLetras();
                else if (value == 20) Num2Text = "veinte";
                else if (value < 30) Num2Text = "veinti" + (value - 20).ConvertirNumerosEnLetras();
                else if (value == 30) Num2Text = "treinta";
                else if (value == 40) Num2Text = "cuarenta";
                else if (value == 50) Num2Text = "cincuenta";
                else if (value == 60) Num2Text = "sesenta";
                else if (value == 70) Num2Text = "setenta";
                else if (value == 80) Num2Text = "ochenta";
                else if (value == 90) Num2Text = "noventa";
                else if (value < 100)
                {
                    int u = value % 10;
                    Num2Text = string.Format("{0} y {1}", ((value / 10) * 10).ConvertirNumerosEnLetras(), (u == 1 ? "un" : (value % 10).ConvertirNumerosEnLetras()));
                }
                else if (value == 100) Num2Text = "cien";
                else if (value < 200) Num2Text = "ciento " + (value - 100).ConvertirNumerosEnLetras();
                else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
                    Num2Text = ((value / 100)).ConvertirNumerosEnLetras() + "cientos";
                else if (value == 500) Num2Text = "quinientos";
                else if (value == 700) Num2Text = "setecientos";
                else if (value == 900) Num2Text = "novecientos";
                else if (value < 1000) Num2Text = string.Format("{0} {1}", ((value / 100) * 100).ConvertirNumerosEnLetras(), (value % 100).ConvertirNumerosEnLetras());
                else if (value == 1000) Num2Text = "mil";
                else if (value < 2000) Num2Text = "mil " + (value % 1000).ConvertirNumerosEnLetras();
                else if (value < 1000000)
                {
                    Num2Text = ((value / 1000)).ConvertirNumerosEnLetras() + " mil";
                    if ((value % 1000) > 0) Num2Text += " " + (value % 1000).ConvertirNumerosEnLetras();
                }
                else if (value == 1000000) Num2Text = "un millón";
                else if (value < 2000000) Num2Text = "un millón " + (value % 1000000).ConvertirNumerosEnLetras();
                else if (value < int.MaxValue)
                {
                    Num2Text = ((value / 1000000)).ConvertirNumerosEnLetras() + " millones";
                    if ((value - (value / 1000000) * 1000000) > 0) Num2Text += " " + (value - (value / 1000000) * 1000000).ConvertirNumerosEnLetras();
                }
                return Num2Text;
            }
            catch (Exception)
            {

                throw;
            }           
        }

        public static string TextoSinTildes(this string texto) =>
                 new String(
                     texto.Normalize(NormalizationForm.FormD)
                     .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                     .ToArray()
                 )
                 .Normalize(NormalizationForm.FormC);

        public static string TextoEnMinuscula(this string texto) {
            try
            {
                if (string.IsNullOrEmpty(texto))
                    return string.Empty;

                return texto.ToLower();
            }
            catch (Exception)
            {

                throw;
            }
        
        }

        public static string GetUrlAplicacion()
        {
            try
            {
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string FormatoPesos(double valor)
        {
            return valor.ToString("$#,##;($#,##);$0");
        }

        public static string FormatoPorcentaje(double valor)
        {
            return valor.ToString("N2");
        }

        public static string FormatoPorcentajeEnNumeroEntero(double valor)
        {
            var resultado = Convert.ToInt16(valor * 100);
            return $"{resultado}%";
        }

        public static string ObtenerFechaLarga(DateTime fecha)
        {
            try
            {
                var dia = fecha.Day.ToString();
                var mes = Common.ObtenerNombreDeMes(fecha);
                var anio = fecha.Year.ToString();
                var hora = Common.ObtenerHoraDeFecha(fecha);

                var horaCompleta = $"{dia} de {mes} del {anio} {hora}";
                return horaCompleta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string ObtenerFechaLargaSinHora(DateTime fecha)
        {
            try
            {
                var dia = fecha.Day.ToString();
                var mes = Common.ObtenerNombreDeMes(fecha);
                var anio = fecha.Year.ToString();

                var horaCompleta = $"{dia} de {mes} del {anio}";
                return horaCompleta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public static string ObtenerHoraDeFecha(DateTime fecha)
        //{
        //    try
        //    {
        //        var hora = fecha.Hour;
        //        var minutos = fecha.Hour;

        //        var asdfa = fecha.ToShortTimeString();
        //        var asfdasdf = fecha.ToShortDateString();
        //        var asdfasdf = fecha.Kind.ToString();

        //        var strHora = hora.ToString();
        //        var strMinutos = minutos.ToString();

        //        return $"{strHora}:{strMinutos}";
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public static string ObtenerNombreDeMes(DateTime fecha)
        {
            try
            {
                if (fecha.Month == 1)
                    return "enero";

                if (fecha.Month == 2)
                    return "febrero";

                if (fecha.Month == 3)
                    return "marzo";

                if (fecha.Month == 4)
                    return "abril";

                if (fecha.Month == 5)
                    return "mayo";

                if (fecha.Month == 6)
                    return "junio";

                if (fecha.Month == 7)
                    return "julio";

                if (fecha.Month == 8)
                    return "agosto";

                if (fecha.Month == 9)
                    return "septiembre";

                if (fecha.Month == 10)
                    return "octubre";

                if (fecha.Month == 11)
                    return "noviembre";

                return "diciembre";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string ObtenerHoraDeFecha(DateTime fecha)
        {
            try
            {
                var hora = fecha.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                return hora;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int CalcularDigitoVerificacion(string nit)
        {
            try
            {
                int[] vpri = new int[16];
                int cantidadDigitos = nit.Length;

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

                int x = 0;
                int y = 0;

                for (var i = 0; i < cantidadDigitos; i++)
                {
                    y = Convert.ToInt32((nit.Substring(i, 1)));

                    x += (y * vpri[cantidadDigitos - i]);
                }

                y = x % 11;

                return (y > 1) ? 11 - y : y;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }


}
