using System.Collections.Generic;

namespace Models.DTO
{
    public class AuditoriasDetalleDTO : AuditoriasDetalle
    {
        public List<NormasDTO> NormasDTO { get; set; }
        public string StrProcesoCodigo { get; set; }
        public string StrProcesoDescripcion { get; set; }
        public bool EsApertura_Cierre { get; set; }
        public string StrEstadoAuditoriaProceso { get; set; }
    }
}
