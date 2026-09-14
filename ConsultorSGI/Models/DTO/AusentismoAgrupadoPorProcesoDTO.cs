using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class AusentismoAgrupadoPorProcesoDTO
    {
        public int Año { get; set; }
        public List<Procesos> ListaProcesos { get; set; }
        public List<AusentismoTiempos> ListaAusentismoTiempos { set; get; }
        public List<TipoEventosAusentismo> ListaTipoEventos { get; set; }
    }

    public class AusentismoTiemposPorProceso
    {
        public int ProcesoID { get; set; }
        public string ProcesoCodigo { get; set; }
        public string ProcesoDescripcion { get; set; }
        public List<TipoEventosAusentismoDTO> TipoEventos { get; set; }
        public int TotalDiasIncapacidad { get; set; }

    }
}
