using System.Collections.Generic;

namespace Models.DTO
{
    public class NumeralesDTO : Numerales
    {
        public int IntCodigoPrincipal { get; set; }
        public int IntCodigoNumerico { get; set; }
        public int IntNivel { get; set; }
        public string StrCodigoNorma { get; set; }
        public string StrDescripcionNorma { get; set; }
        public bool BitSeleccionado { get; set; }
        
    }
}
