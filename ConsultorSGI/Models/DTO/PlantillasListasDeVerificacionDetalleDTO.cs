using System.Collections.Generic;

namespace Models.DTO
{
    public class PlantillasListasDeVerificacionDetalleDTO : PlantillasListasDeVerificacionDetalle
    {
        public List<NormasDTO> NormasDTO { get; set; }
    }
}
