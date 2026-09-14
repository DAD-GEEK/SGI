using System.Collections.Generic;

namespace Models.DTO
{
    public class SistemasDeGestionDTO : SistemasDeGestion
    {
        public int IntSistemaDeGestion_TerceroID { get; set; }
        public string StrNivelID { get; set; }
        public List<NormasDTO> Normas { get; set; }
    }
}
