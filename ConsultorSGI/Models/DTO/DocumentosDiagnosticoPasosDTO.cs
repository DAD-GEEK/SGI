using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class DocumentosDiagnosticoPasosDTO : DocumentosDiagnosticoPasos
    {
        public string StrFaseDescripcionPrimaria { get; set; }
        public string StrFaseDescripcionSecundaria { get; set; }
        public int IntCantidadCriterios => DocumentosDiagnosticoPasos_CriteriosDTO != null ? DocumentosDiagnosticoPasos_CriteriosDTO.Where(x => x.StrPasoID == StrPasoID).Count() : 0;
        public byte IntCantidadSIAplica => BitAplica ? (byte)DocumentosDiagnosticoDetalle_PlanesDeAccionDTO.Where(x => x.BitAplica == true).Count() : (byte)0;
        public byte IntCantidadSICumple => BitAplica ? (byte)DocumentosDiagnosticoDetalle_PlanesDeAccionDTO.Where(x => x.BitAplica == true && x.BitSeEvidencia == true).Count() : (byte)0;
        public byte IntCantidadNOCumple => BitAplica ? (byte)DocumentosDiagnosticoDetalle_PlanesDeAccionDTO.Where(x => x.BitAplica == true && x.BitSeEvidencia == false).Count() : (byte)0;
        public double DecResultado => IntCantidadSIAplica != 0 ? Convert.ToDouble(IntCantidadSICumple) / Convert.ToDouble(IntCantidadSIAplica) : 0;
        public bool BitAplica { get; set; }

        public List<Niveles> Niveles { get; set; }
        public List<DocumentosDiagnosticoPasos_CriteriosDTO> DocumentosDiagnosticoPasos_CriteriosDTO { get; set; }
        public List<DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO> DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO { get; set; }
        public List<DocumentosDiagnosticoDetalle_PlanesDeAccionDTO> DocumentosDiagnosticoDetalle_PlanesDeAccionDTO { get; set; }


    }
}
