using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO: DocumentosDiagnosticoPasos_DocumentoEvidencia
    {
        public List<DocumentosDiagnosticoPasos_CriteriosDTO> DocumentosDiagnosticoPasos_CriteriosDTO { get; set; }
        public int IntNumeroDeVecesEnCriterio => DocumentosDiagnosticoPasos_CriteriosDTO.Count();
        public bool BitSeleccionado { get; set; }
        public byte IntOrden { get; set; }

    }
}
