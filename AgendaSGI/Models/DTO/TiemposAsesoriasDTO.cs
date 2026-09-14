using System;

namespace Models.DTO
{
    public class TiemposAsesoriasDTO
    {
        public string NombreUsuario { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaInicialDeVisita { get; set; }
        public DateTime FechaDeLlegada { get; set; }
        public double Diferencia => (FechaInicialDeVisita - FechaDeLlegada).TotalMinutes;
        public bool MarcoLlegada => FechaDeLlegada > new DateTime(2020,1,1) ? true : false;
    }
}
