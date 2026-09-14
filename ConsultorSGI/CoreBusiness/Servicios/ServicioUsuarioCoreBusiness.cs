using Across;
using DataAccess.Servicios;
using Models;

namespace CoreBusiness.Servicios
{
    public static class ServicioUsuarioCoreBusiness
    {
        public static CurrentUser ObtenerDatosDeUsuarioEnSesion
        {
            get
            {
                var usuarioEnSesion = DataAccess.Servicios.ServicioUsuario.ObtenerDatosDeUsuarioEnSesion;
                usuarioEnSesion.UrlImagenDePerfil = Archivos.GetUrlImagenUsuarioAvatar(usuarioEnSesion.ImagenDePerfil);
                usuarioEnSesion.UrlImagenDeFirma = Archivos.GetUrlImagenUsuarioFirma(usuarioEnSesion.ImagenDeFirma);
                usuarioEnSesion.UrlImagenTerceroLogo = Archivos.GetUrlImagenTerceroOrUrlDefault(usuarioEnSesion.ImagenTerceroLogo);

                return usuarioEnSesion;
            }
        }

        public static void RefrescarDatosDeUsuarioEnSesion()
        {
            try
            {
                DataAccess.Servicios.ServicioUsuario.RefrescarDatosDeUsuarioEnSesion();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
