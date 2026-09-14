using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace CoreBusiness
{
    public class SeguridadCoreBusiness
    {
        public string cifrarCadena(string cadena)
        {
            try
            {
                byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.

                byte[] arreglo = UTF8Encoding.UTF8.GetBytes(cadena); //Arreglo donde guardaremos la cadena descifrada.

                // Ciframos utilizando el Algoritmo MD5.
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(cadena));
                md5.Clear();

                //Ciframos utilizando el Algoritmo 3DES.
                TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
                tripledes.Key = llave;
                tripledes.Mode = CipherMode.ECB;
                tripledes.Padding = PaddingMode.PKCS7;
                ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
                byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
                tripledes.Clear();

                return Convert.ToBase64String(resultado, 0, resultado.Length); // Convertimos la cadena y la regresamos.
            }
            catch (Exception)
            {

                throw;
            }           
        }

        public bool IsSessionTimedOut()
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx == null)
                throw new Exception("Este método sólo se puede usar en una aplicación Web");

            //Comprobamos que haya sesión en primer lugar 
            //(por ejemplo si por ejemplo EnableSessionState=false)
            if (ctx.Session == null)
                return false;   //Si no hay sesión, no puede caducar
                                //Se comprueba si se ha generado una nueva sesión en esta petición
            if (!ctx.Session.IsNewSession)
                return false;   //Si no es una nueva sesión es que no ha caducado

            HttpCookie objCookie = ctx.Request.Cookies["ASP.NET_SessionId"];
            //Esto en teoría es imposible que pase porque si hay una 
            //nueva sesión debería existir la cookie, pero lo compruebo porque
            //IsNewSession puede dar True sin ser cierto (más en el post)
            if (objCookie == null)
                return false;

            //Si hay un valor en la cookie es que hay un valor de sesión previo, pero como la sesión 
            //es nueva no debería estar, por lo que deducimos que la sesión anterior ha caducado
            if (!string.IsNullOrEmpty(objCookie.Value))
                return true;
            else
                return false;
        }

        public int ObtenerUsuario()
        {
            int user_id = 0;
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    user_id = Convert.ToInt32(ticket.UserData);
                }
            }
            return user_id;
        }
    }
}
