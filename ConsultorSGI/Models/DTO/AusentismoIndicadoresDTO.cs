using System.Collections.Generic;

namespace Models.DTO
{
    public class AusentismoIndicadoresDTO : AusentismoIndicadores
    {
        public int IntAnio { get; set; }
        public string StrCanvasID { get; set; }
        public GraficosChartDTO Grafico { get; set; }
        public AusentismoIndicadores_InformacionPorEmpresaDTO InformacionPorEmpresaDTO { get; set; }

    }
}
