using System;

namespace Models.DTO
{
    public class TercerosDTO : Terceros
    {
        public DateTime DatFechaDocumentoDiagnostico { get; set; }
        public string StrCiudadCodigo { get; set; }
        public string StrCiudadNombre { get; set; }
        public string StrDepartamento { get; set; }
        public string StrNivelDescripcion { get; set; }
        public string StrNivelDeRiesgoCodigo { get; set; }
        public string StrNivelDeRiesgoDescripcion { get; set; }
        public string StrUrlImagen { get; set; }
        public string StrMensajeError { get; set; }
    }
}
