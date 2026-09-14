using System.Collections.Generic;

namespace Models.DTO
{
    public class NumeralesGenericosDTO
    {
        public int TerceroID { get; set; }
        public int RegistroID { get; set; }
        public int AuditoriaID { get; set; }
        public int ProcesoID { get; set; }
        public string Controlador { get; set; }
        public List<NormasDTO> ListaNormasDTO { get; set; }
    }
}
