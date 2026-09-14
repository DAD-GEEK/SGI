using System;
using System.Collections.Generic;

namespace Models.DTO
{
    public class InformesDTO
    {
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string label { get; set; }
        public List<decimal> data { get; set; }


        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int HorasContrato { get; set; }
        public decimal HorasMes { get; set; }
        public decimal diferencia { get; set; }
    }
}
