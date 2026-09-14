using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class AusentismoDTO
    {
        public List<AusentismoTiempos> TiemposAusentismo { get; set; }
        public int NumeroRegistro { get; set; }
        public int IntAusentismoID { get; set; }
        public DateTime DatFechaInicial { get; set; }
        public DateTime DatFechaFinal { get; set; }
        public int IntDiasIncapacidad => TiemposAusentismo.Count() != 0 ? TiemposAusentismo.Sum(x => x.IntDiasIncapacidad) : 0;
        public int IntDiasProrroga => TiemposAusentismo.Count() != 0 ? TiemposAusentismo.Sum(x => x.IntDiasProrroga) : 0;
        public int IntTotalDiasIncapacidad => TiemposAusentismo.Count() != 0 ? IntDiasIncapacidad + IntDiasProrroga : 0;
        public string StrObservaciones { get; set; }
        public int IntTerceroID { get; set; }
        public int IntEmpleadoID { get; set; }
        public string StrIdentificacionEmpleado { get; set; }
        public string StrNombreEmpleado { get; set; }
        public string StrImagenEmpleado { get; set; }
        public string StrTipoEvento { get; set; }
        public int IntTipoEventoID { get; set; }
        public int IntCantidadProrrogas { get; set; }
        public string StrDescripcionEvento { get; set; }
        public int IntDiagnosticoID { get; set; }
        public string StrCodigoDiagnostico { get; set; }
        public string StrDescripcionDiagnostico { get; set; }
        public string StrCargo { get; set; }
        public string ErrorArchivoExcel = string.Empty;

    }
}
