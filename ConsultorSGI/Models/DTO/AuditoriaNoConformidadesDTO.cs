using System;
using System.Collections.Generic;

namespace Models.DTO
{
    public class AuditoriaNoConformidadesDTO
    {
        public int Id { get; set; }
        public List<NormasDTO> NormasDTO { get; set; }
        public string Titulo { get; set; }
        public int ProcesoID { get; set; }
        public string ProcesoCodigo { get; set; }
        public string ProcesoDescripcion { get; set; }
        public string Hallazgo { get; set; }
        public string HallazgoInforme { get; set; }
        public bool Observacion { get; set; }
        public bool NoConformidad { get; set; }
        public int? Orden { get; set; }
        public DateTime? FechaQueSeAudita { get; set; }
    }
}
