using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IDocumentosDiagnosticoDetalleCoreBusiness : ICRUDGenerico<DocumentosDiagnosticoDetalle>, IDisposable
    {
        Task<string> CreateRangeAsync(List<DocumentosDiagnosticoDetalle> entity);
        Task<string> GuardarAsync(DocumentosDiagnosticoDetalle model);
    }
}
