using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IDocumentosDiagnosticoCoreBusiness : ICRUDGenerico<DocumentosDiagnostico>, IDisposable
    {
        Task<string> AgregarElementosPendientesAsync(DocumentosDiagnosticoDTO documentosDiagnosticoDTO);
        Task<string> FinalizarDocumentoAsync(DocumentosDiagnostico modelo);
        Task<string> GuardarAsync(DocumentosDiagnostico model);
        Task<DocumentosDiagnosticoDTO> ObtenerDocumentoDiagnosticoDTO(string documentoID);
        Task<TercerosDTO> ObtenerInformacionDeEmpresaAsync(int terceroID);
        Task<GraficosChartDTO> ObtenerResultadoConsolidadoGlobalAsync(string documentoID);
    }
}
