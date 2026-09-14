using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IDocumentosDiagnosticoPasosCoreBusiness : ICRUDGenerico<DocumentosDiagnosticoPasos>, IDisposable
    {
        Task<MultiSelectList> DropDownListFasesAsync(string valueSelected = null);
        Task<MultiSelectList> DropDownListMultipleNivelesAsync(DocumentosDiagnosticoPasos model = null);
        Task<string> EliminarDocumentoPasoAsync(string pasoID);
        Task<string> GuardarAsync(DocumentosDiagnosticoPasos model);
        Task<List<DocumentosDiagnosticoPasosDTO>> ListaDocumentosDiagnosticoPasosPorSistemaDeGestionDTOAsync(string sistemaDeGestion);
    }
}
