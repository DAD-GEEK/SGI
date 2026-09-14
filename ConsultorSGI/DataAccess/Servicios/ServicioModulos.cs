using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Servicios
{
    public static class ServicioModulos
    {
        public static ModulosDTO ObtenerInformacionDeModuloActual(string controller, string action = "")
        {
            try
            {
                ModulosDTO modulosDTO = new ModulosDTO();

                using (var bd = new gestioni_consultorNetEntities())
                {
                    if (action == "")
                        action = "Index";

                    modulosDTO.ModuloDetalle = bd.ModulosDetalle.FirstOrDefault(x => x.StrControlador == controller && x.StrAccion == action);
                    modulosDTO.ModuloPrincial = bd.Modulos.FirstOrDefault(x => x.IntModuloID == modulosDTO.ModuloDetalle.IntModuloID);                 
                }

                return modulosDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
