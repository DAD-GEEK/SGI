using System.Collections.Generic;

namespace Models.DTO
{
    public class ContratosDTO : Contratos
    {
        public List<ContratosUsuariosDTO> ContratosUsuariosDTO { get; set; }
        public List<ContratosSistemasDeGestionDTO> ContratosSistemasDTO { get; set; }
        public int IntNumeroContratoDB { get; set; }

    }
}
