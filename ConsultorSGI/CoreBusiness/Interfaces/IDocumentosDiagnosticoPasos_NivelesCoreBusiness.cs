using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IDocumentosDiagnosticoPasos_NivelesCoreBusiness : ICRUDGenerico<DocumentosDiagnosticoPasos_Niveles>, IDisposable
    {
        Task<string> ActualizarNivelesEnDocumentoDiagnosticoPasosAsync(string pasoID, List<DocumentosDiagnosticoPasos_Niveles> listaNiveles);
        Task<string> GuardarAsync(DocumentosDiagnosticoPasos_Niveles model);
    }
}
