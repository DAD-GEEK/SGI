using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness : ICRUDGenerico<DocumentosDiagnosticoDetalle_PlanesDeAccion>, IDisposable
    {
        Task<string> CreateRangeAsync(List<DocumentosDiagnosticoDetalle_PlanesDeAccion> entity);
        Task<string> GuardarAsync(DocumentosDiagnosticoDetalle_PlanesDeAccion model);
    }
}
