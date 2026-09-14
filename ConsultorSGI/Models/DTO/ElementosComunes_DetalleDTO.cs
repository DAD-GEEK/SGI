using System.Collections.Generic;

namespace Models.DTO
{
    public class ElementosComunes_DetalleDTO : ElementosComunes_Detalle
    {
        public string NumeralDescripcion { get; set; }
        public List<NumeralesDTO> listaNumeralesDTO { get; set; }


    }
}
