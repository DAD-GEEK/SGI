using Across;
using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface ISistemasDeGestionCoreBusiness : ICRUDGenerico<SistemasDeGestion>, IDisposable
    {
        Task<SelectList> DropDownListSistemasDeGestionAsync(string valueSelected = null);
        Task<string> EliminarAsync(SistemasDeGestion entity);
        Task<string> GuardarRegistroAsync(SistemasDeGestion model, string tipoDeAccion);
        Task<List<DocumentosDiagnosticoPasosDTO>> ObtenerDocumentoDiagnosticoPasosPorSistemaDeGestion(string sistemaDeGestionID);
        Task<List<SistemasDeGestionDTO>> ObtenerSistemasDeGestionPorTerceroEnSesion();
    }
}
