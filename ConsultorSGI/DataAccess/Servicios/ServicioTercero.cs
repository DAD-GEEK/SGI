using Models;
using Models.DTO;
using Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace DataAccess.Servicios
{
    public class ServicioTercero : IServicioTercero
    {
        public int ObtenerTerceroDeUsuarioEnSesion()
        {
            try
            {
                var datosUsuarioEnSesion = ServicioUsuario.ObtenerDatosDeUsuarioEnSesion;
                int IntTerceroID = datosUsuarioEnSesion.TerceroID;

                return IntTerceroID;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
