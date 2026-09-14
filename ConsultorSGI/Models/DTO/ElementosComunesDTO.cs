using System.Collections.Generic;

namespace Models.DTO
{
    public class ElementosComunesDTO : ElementosComunes
    {
        public List<Normas> Normas { get; set; }
        public List<ElementosComunes_Detalle_Numerales> ElementosComunes_Numerales { get; set; }
        public List<ElementosComunes_DetalleDTO> ElementosComunes_DetalleDTO { get; set; }

    }
}
