using System.Collections.Generic;

namespace Models.DTO
{
    public class DocumentosDiagnosticoDetalle_PlanesDeAccionDTO : DocumentosDiagnosticoDetalle_PlanesDeAccion
    {
        public string StrFaseDescripcion { get; set; }
        public string StrPasoID { get; set; }
        public string StrNumeroPaso { get; set; }
        public string StrCriterioDescripcion { get; set; }
        public string StrSoporteDescripcion { get; set; }
        public byte IntOrden { get; set; }
        public bool BitCalificado => DatFechaCalificacion != null ? true : false;
        public bool BitBloquear { get; set; }
    }
}
