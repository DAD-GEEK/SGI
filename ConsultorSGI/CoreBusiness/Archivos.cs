using Across.ArchivosDeRecurso;
using CoreBusiness.Servicios;
using Models;
using Models.DTO;
using System;
using System.IO;
using System.Web;

namespace CoreBusiness
{
    public static class Archivos
    {
        static CurrentUser currentUser = ServicioUsuarioCoreBusiness.ObtenerDatosDeUsuarioEnSesion;

        public static string rutaImagenEmpleado = $"Files/images/empleados/{currentUser.TerceroIdentificacion}";
        public static string rutaImagenUsuarioAvatar = $"Files/images/usuarios/{currentUser.TerceroIdentificacion}/avatar";
        public static string rutaImagenUsuarioFirma = $"Files/images/usuarios/{currentUser.TerceroIdentificacion}/firmas";
        public const string rutaImagenTercero = "Files/images/terceros";
        public const string rutaImagenTercerosClientes = "Files/images/tercerosclientes";
        public const string rutaImagenUserDefault = "Files/images/userDefault.png";
        public const string rutaArchivosTemporales = "Files/archivosTemporales/";
        public const string rutaDescargas = "Files/descargas/";

        public static string GuardarArchivo(HttpFileCollectionBase files, ParamFilesDTO parametros)
        {
            try
            {
                string[] fieldNames = files.AllKeys;
                string path = string.Empty;
                string nombreArchivo = string.Empty;

                for (int i = 0; i < fieldNames.Length; ++i)
                {
                    string field = fieldNames[i];
                    HttpPostedFileBase file = files[i];
                    string fileName = files[i].FileName;
                    int len = files[i].ContentLength;
                    string type = files[i].ContentType;
                    Stream stream = files[i].InputStream;

                    if (len > 0)
                    {
                        nombreArchivo = parametros.nombreArchivo + Path.GetExtension(fileName);
                        path = Path.Combine(parametros.ruta, nombreArchivo);

                        if (!Directory.Exists(parametros.ruta))
                            Directory.CreateDirectory(parametros.ruta);

                        file.SaveAs(path);
                    }
                }

                return nombreArchivo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static long ObtenerTamanoDeArchivo(string rutaArchivo)
        {
            try
            {
                if (!string.IsNullOrEmpty(rutaArchivo))
                {
                    FileInfo fileInformacion = new FileInfo(rutaArchivo);
                    return fileInformacion.Length;
                }

                return 0;

            }
            catch (Exception)
            {

                return 0;
            }
        }

        public static string ObtenerRutaDeArchivoTemporal(string nombreArchivo)
        {
            try
            {
                return HttpContext.Current.Server.MapPath($"~/{rutaArchivosTemporales}/{nombreArchivo}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string ObtenerRutaDeImagenEmpleado(string nombreImagen)
        {
            try
            {
                return HttpContext.Current.Server.MapPath($"~/{rutaImagenEmpleado}/{nombreImagen}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string ObtenerRutaDeImagenUsuarioAvatar(string nombreImagen)
        {
            try
            {
                return HttpContext.Current.Server.MapPath($"~/{rutaImagenUsuarioAvatar}/{nombreImagen}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string ObtenerRutaDeImagenUsuarioFirma(string nombreImagen)
        {
            try
            {
                return HttpContext.Current.Server.MapPath($"~/{rutaImagenUsuarioAvatar}/{nombreImagen}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string ObtenerRutaDeImagenTerceroCliente(string terceroIdentificacion, string nombreImagen)
        {
            try
            {
                return HttpContext.Current.Server.MapPath($"~/{rutaImagenTercerosClientes}/{terceroIdentificacion}/{nombreImagen}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string ObtenerRutaDeImagenTercero(string nombreImagen)
        {
            try
            {
                return HttpContext.Current.Server.MapPath($"~/{rutaImagenTercero}/{nombreImagen}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string ConvertirRutaEnUrl(string rutaArchivo)
        {
            try
            {
                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + rutaArchivo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string EliminarArchivo(string rutaArchivo)
        {
            try
            {
                var validacionImagenPorDefecto = ValidarSiEsImagenDefault(rutaArchivo);
                if (validacionImagenPorDefecto)
                    return RecursoArchivos.msnImagenNoSePuedeEliminar;

                var existeFile = File.Exists(rutaArchivo);

                if (existeFile)
                    File.Delete(rutaArchivo);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return RecursoFiles.msnErrorEnEliminacion + " - " + ex.Message;
            }
        }
        public static string GetUrlImagenEmpleado(string nombreImagen)
        {
            try
            {
                if (!string.IsNullOrEmpty(nombreImagen))
                {
                    var pathImagen = HttpContext.Current.Server.MapPath($"~/{rutaImagenEmpleado}/{nombreImagen}");

                    if (File.Exists(pathImagen))
                        return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenEmpleado}/{nombreImagen}?version{Guid.NewGuid()}";
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetUrlImagenTercero(string nombreImagen)
        {
            try
            {
                if (!string.IsNullOrEmpty(nombreImagen))
                {
                    var pathImagen = HttpContext.Current.Server.MapPath($"~/{rutaImagenTercero}/{nombreImagen}");

                    if (File.Exists(pathImagen))
                        return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenTercero}/{nombreImagen}?version{Guid.NewGuid()}";
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetUrlImagenTerceroClienteOrUrlDefault(string identificacionTercero, string nombreImagen)
        {
            try
            {
                var pathImagen = string.Empty;

                if (!string.IsNullOrEmpty(nombreImagen))
                {
                    pathImagen = HttpContext.Current.Server.MapPath($"~/{rutaImagenTercerosClientes}/{identificacionTercero}/{nombreImagen}");
                    if (File.Exists(pathImagen))
                        return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenTercerosClientes}/{identificacionTercero}/{nombreImagen}?version{Guid.NewGuid()}";
                }

                pathImagen = Archivos.GetUrlUserImagenDefault();

                return pathImagen;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetUrlImagenEmpleadoOrUrlDefault(string nombreImagen)
        {
            try
            {
                var pathImagen = string.Empty;

                if (!string.IsNullOrEmpty(nombreImagen))
                {
                    pathImagen = HttpContext.Current.Server.MapPath($"~/{rutaImagenEmpleado}/{nombreImagen}");
                    if (File.Exists(pathImagen))
                        return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenEmpleado}/{nombreImagen}?version{Guid.NewGuid()}";
                }

                pathImagen = Archivos.GetUrlUserImagenDefault();

                return pathImagen;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetUrlImagenTerceroOrUrlDefault(string nombreImagen)
        {
            try
            {
                var pathImagen = string.Empty;

                if (!string.IsNullOrEmpty(nombreImagen))
                {
                    pathImagen = HttpContext.Current.Server.MapPath($"~/{rutaImagenTercero}/{nombreImagen}");
                    if (File.Exists(pathImagen))
                        return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenTercero}/{nombreImagen}?version{Guid.NewGuid()}";
                }

                pathImagen = Archivos.GetUrlUserImagenDefault();

                return pathImagen;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetUrlUserImagenDefault()
        {
            try
            {
                var pathImagen = HttpContext.Current.Server.MapPath($"~/{rutaImagenUserDefault}");
                if (File.Exists(pathImagen))
                    return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenUserDefault}";

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetUrlImagenUsuarioAvatar(string nombreImagen)
        {
            try
            {
                if (!string.IsNullOrEmpty(nombreImagen))
                {
                    var pathImagen = HttpContext.Current.Server.MapPath($"~/{rutaImagenUsuarioAvatar}/{nombreImagen}");
                    if (File.Exists(pathImagen))
                        return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenUsuarioAvatar}/{nombreImagen}?version{Guid.NewGuid()}";
                }

                return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenUserDefault}";

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetUrlImagenUsuarioFirma(string nombreImagen)
        {
            try
            {
                if (!string.IsNullOrEmpty(nombreImagen))
                {
                    var pathImagen = HttpContext.Current.Server.MapPath($"~/{rutaImagenUsuarioFirma}/{nombreImagen}");
                    if (File.Exists(pathImagen))
                        return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + $"/{rutaImagenUsuarioFirma}/{nombreImagen}?version{Guid.NewGuid()}";
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool ValidarSiEsImagenDefault(string ruta)
        {
            try
            {
                if (ruta.Contains(rutaImagenUserDefault))
                    return true;

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string CopiarImagenEnOtraRuta(string rutaImagenOrigen, string rutaImagenDestino)
        {
            var existeFile = File.Exists(rutaImagenDestino);

            if (existeFile)
                File.Delete(rutaImagenDestino);

            File.Copy(rutaImagenOrigen, rutaImagenDestino);

            return string.Empty;
        }
    }
}
