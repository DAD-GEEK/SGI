using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness : ICRUDGenerico<DocumentosDiagnosticoPasos_DocumentoEvidencia>, IDisposable
    {
        Task<string> EliminarEvidenciaAsync(string evidenciaID);
        Task<List<DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO>> GetAllEvidenciasConCriteriosPasoAsync(string pasoID);
        Task<List<DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO>> GetAllEvidenciasPorCriterioAsync(string pasoID, string criterioID);
        Task<string> GuardarAsync(DocumentosDiagnosticoPasos_DocumentoEvidencia model);
    }
}
