using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class NormasDTO : Normas
    {
        public int IntTerceros_SistemaDeGestionID { get; set; }
        public int IntPlantillaDetalleID { get; set; }
        public string NormaConNumerales { get; set; }
        public List<NumeralesDTO> NumeralesDTO { get; set; }
        public int CantidadNumeralesPorNorma { get; set; }
        public int CantidadNumeralesSeleccionados => Numerales is null ? 0 : Numerales.ToList().Count();
        public int CantidadNoConformidades { get; set; }
        public bool BitAplica { get; set; }
        public bool BitNoConformidad { get; set; }
        public bool BitObservacion { get; set; }
    }
}
