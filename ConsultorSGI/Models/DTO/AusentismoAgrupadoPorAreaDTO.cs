using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class AusentismoAgrupadoPorAreaDTO
    {
        public int Año { get; set; }
        public List<Areas> ListaAreas { get; set; }
        public List<AusentismoTiempos> ListaAusentismoTiempos { set; get; }
        public List<TipoEventosAusentismo> ListaTipoEventos { get; set; }
        public List<AusentismoTiemposPorArea> ListaAusentismoAgrupadoPorArea => ListaAusentismoTiempos.GroupBy(x => x.IntAreaID).Select(x => new AusentismoTiemposPorArea()
        {
            AreaID = x.Key,
            AreaCodigo = ListaAreas.Where(areas => areas.IntAreaID == x.Key && areas.BitActivo == true).FirstOrDefault().StrCodigo,
            AreaDescripcion = ListaAreas.Where(areas => areas.IntAreaID == x.Key && areas.BitActivo == true).FirstOrDefault().StrDescripcion,
            TipoEventos = ListaAusentismoTiempos.Where(areas => areas.IntAreaID == x.Key).GroupBy(y => y.IntTipoEventoAusentismoID).Select(y => new TipoEventosAusentismoDTO()
            {
                IntTipoEventoAusentismoID = y.Key,
                StrCodigo = ListaTipoEventos.Where(evento => evento.IntTipoEventoAusentismoID == y.Key).FirstOrDefault().StrCodigo,
                DiasIncapacidad = ListaAusentismoTiempos.Where(tiempo => tiempo.IntTipoEventoAusentismoID == y.Key && tiempo.IntAreaID == x.Key).Sum(suma => suma.IntDiasIncapacidad)

            }).ToList(),
            TotalDiasIncapacidad = ListaAusentismoTiempos.Where(tiempo => tiempo.IntAreaID == x.Key).Sum(tiempo => tiempo.IntDiasIncapacidad)

        }).ToList();
    }

    public class AusentismoTiemposPorArea
    {
        public int AreaID { get; set; }
        public string AreaCodigo { get; set; }
        public string AreaDescripcion { get; set; }
        public List<TipoEventosAusentismoDTO> TipoEventos { get; set; }
        public int TotalDiasIncapacidad { get; set; }

    }
}
