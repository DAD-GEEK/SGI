using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class AusentismoAgrupadoMensualmentePorAnioDTO
    {
        public List<AusentismoTiempos> ListaAusentismoTiempos { set; get; }
        public short Año { get; set; }
        public short Mes { get; set; }
        public string MesNombre { get; set; }
        public List<TipoEventosAusentismo> ListaTipoEventos { get; set; }
        public List<AusentismoTiemposAgrupado> ListaAusentismoAgrupadoPorTipoEvento => ListaAusentismoTiempos.GroupBy(x => x.IntTipoEventoAusentismoID).Select(x => new AusentismoTiemposAgrupado()
        {
            TipoAusentismoID = x.Key,
            TipoAusentismoCodigo = ListaTipoEventos.Where(y => y.IntTipoEventoAusentismoID == x.Key).FirstOrDefault().StrCodigo,
            DiasAusentismo = x.Sum(tiempo => tiempo.IntDiasIncapacidad)

        }).ToList();
        public int TotalDiasIncapacidad => ListaAusentismoAgrupadoPorTipoEvento.Sum(x => x.DiasAusentismo);
    }

    public class AusentismoTiemposAgrupado
    {
        public int TipoAusentismoID { get; set; }
        public string TipoAusentismoCodigo { get; set; }
        public int DiasAusentismo { get; set; }
    }
}
