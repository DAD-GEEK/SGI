using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class CargarListaDeVerificacionDTO
    {
        public int IntPlantillaID { get; set; }
        public int IntAuditoriaDetalleID { get; set;}
        public bool BitSobreEscribirDatos { get; set; }
    }
}
