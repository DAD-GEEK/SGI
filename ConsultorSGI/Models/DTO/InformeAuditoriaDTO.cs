using System.Collections.Generic;

namespace Models.DTO
{
    public class InformeAuditoriaDTO
    {
        public Auditorias Auditoria { get; set; }
        public AuditoriasDetalle AuditoriaDetalle { get; set; }
        public List<AuditoriaNoConformidadesDTO> NoConformidades { get; set; }
        public List<AuditoriasDetalleDTO> ListaAuditoriaDetalleDTO { get; set; }
        public List<ListasDeVerificacionDTO> ListaDeVerificacionDTO { get; set; }
        public AspNetUsers Usuario { get; set; }
        public List<NormasDTO> ListaNormasDTO { get; set; }
    }
}
