using System.Collections.Generic;

namespace Models.DTO
{
    public class DatasetsChartDTO
    {
        public List<string> backgroundColor { get; set; }
        public string type { get; set; }
        public string borderColor { get; set; }
        public string label { get; set; }
        public bool fill { get; set; }
        public decimal tension { get; set; }
        public int order { get; set; }
        public List<int> data { get; set; }
    }
}
