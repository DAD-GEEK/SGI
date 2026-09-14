using Models;

namespace DataAccess.Servicios
{
    public static class ServicioUsuario
    {

        public static CurrentUser ObtenerDatosDeUsuarioEnSesion
        {
            get
            {
                return Models.ServicioUsuarioModel.ServicioUsuario.ObtenerDatosDeUsuarioEnSesion;
            }
        }

        public static void RefrescarDatosDeUsuarioEnSesion()
        {
            try
            {
                Models.ServicioUsuarioModel.ServicioUsuario.RefrescarDatosDeUsuarioEnSesion();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static void ExtenderMenuDinamico()
        {
            try
            {
                Models.ServicioUsuarioModel.ServicioUsuario.ExtenderMenuDinamico();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static void ColapsarMenuDinamico()
        {
            try
            {
                Models.ServicioUsuarioModel.ServicioUsuario.ColapsarMenuDinamico();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
