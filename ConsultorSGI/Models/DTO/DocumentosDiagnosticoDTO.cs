using System.Collections.Generic;

namespace Models.DTO
{
    public class DocumentosDiagnosticoDTO: DocumentosDiagnostico
    {
        public List<FasesDTO> FasesDocumento { get; set; }
        public List<DocumentosDiagnosticoPasosDTO> PasosDocumento { get; set; }
        public TercerosDTO InformacionEmpresa { get; set; }
        public List<GraficosChartDTO> ConsolidadoDocumento { get; set; }
        public string StrUsuarioNombre { get; set; }
        public GraficosChartDTO GraficaFases { get; set; }
        public List<DocumentosDiagnosticoPasosDTO> PasosGenericosPendientesDeAsignar { get; set; }
        public List<DocumentosDiagnosticoPasos_CriteriosDTO> CriteriosGenericosPendientesDeAsignar { get; set; }

    }
}
