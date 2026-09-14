using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class IndicadorAusentismoGlobalDTO
    {
        public List<IndicadorAusentismoGeneralDTO> MedicionGeneralAusentismo { get; set; }
        public GraficosChartDTO MedicionesGeneralAusentismo_DatosChart { get; set; }
        public string CanvasID { get; set; }

    }
}
