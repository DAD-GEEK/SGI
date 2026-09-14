using System.Collections.Generic;

namespace Models.DTO
{
    public class ListasDeVerificacionDTO : ListasDeVerificacion
    {
        public List<NormasDTO> NormasDTO { get; set; }
        public bool IsVistaInforme { get; set; }
        public int? Orden { get; set; }

    }
}
