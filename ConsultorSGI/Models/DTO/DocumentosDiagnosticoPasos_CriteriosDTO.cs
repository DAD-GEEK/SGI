using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class DocumentosDiagnosticoPasos_CriteriosDTO : DocumentosDiagnosticoPasos_Criterios
    {
        public bool BitSeleccionado { get; set; }
        public string StrDocumentoSoporte { get; set; }
    }
}
