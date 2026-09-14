using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IDocumentosDiagnosticoPasos_CriteriosCoreBusiness : ICRUDGenerico<DocumentosDiagnosticoPasos_Criterios>, IDisposable
    {
        Task<string> AsociarListaDeCriteriosConEvidenciaAsync(List<string> listaDeCriteriosIDs, string evidenciaID, string pasoID);
        Task<string> CambiarOrdenamientoSortableAsync(List<DocumentosDiagnosticoPasos_Criterios> listaCriterios);
        Task<string> EliminarCriterioAsync(string criterioID);
        Task<string> GuardarAsync(DocumentosDiagnosticoPasos_Criterios model);
        Task<List<DocumentosDiagnosticoPasos_CriteriosDTO>> ObteneCriteriosGenericosPorSistemaDeGestionDTOAsync(string sistemaDeGestion);
        Task<List<DocumentosDiagnosticoPasos_CriteriosDTO>> ObtenerListaDeCriteriosPorDocumentoEvidenciaPorPasoAsync(string pasoID, string evidenciaID);
    }
}
