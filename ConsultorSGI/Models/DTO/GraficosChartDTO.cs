using System.Collections.Generic;

namespace Models.DTO
{
    public class GraficosChartDTO
    {
        public string StrTitulo { get; set; }
        public int IntOrdenamiento { get; set; }
        public List<string> labels { get; set; }
        public List<DatasetsChartDTO> datasets { get; set; }       
    }
}
