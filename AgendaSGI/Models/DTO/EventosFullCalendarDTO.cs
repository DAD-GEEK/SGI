using System;

namespace Models.DTO
{
    public class EventosFullCalendarDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public bool allDay { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string textColor { get; set; }
        public int clienteID { get; set; }
        public string clienteNombre { get; set; }
        public string tipoEvento { get; set; }
        public int tipoEventoID { get; set; }
        public bool soporte { get; set; }
        public string descripcion { get; set; }
        public int asesorID { get; set; }
        public string asesorNombre { get; set; }
        public bool enviado { get; set; }
        public bool cancelada { get; set; }
    }
}
