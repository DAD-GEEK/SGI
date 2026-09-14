using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IAuditoriasCoreBusiness : ICRUDGenerico<Auditorias>, IDisposable
    {
        int TotalRegistrosDataTable { get; }

        List<AuditoriasDTO> GetPaginacionAuditorias(DatatableParamsDTO datatableParamsDTO, bool paginarInformacion = true);
        Task<string> GuardarInformeDeAuditoriaAsync(Auditorias model);
        Task<MultiSelectList> MultiSelectListProcesosTercerosAsync(int terceroID, int auditoriaID = 0);
        Task<MultiSelectList> MultiSelectListTodasLasNormasAsync(int auditoriaID);
        Task<List<ElementosComunes>> ObtenerComplementoElementosComunesAsync(int terceroID, int auditoriaID);
        Task<List<ElementosComunes>> ObtenerElementosComunesEntreNormasAsync(List<Normas> listaNormasAuditoria);
        Task<InformeAuditoriaDTO> ObtenerInformePlanAuditoriaPDFAsync(int auditoriaID);
        Task<InformeAuditoriaDTO> ObtenerInformePlanDeAuditoriaPDFAsync(int auditoriaID);
        Task<List<ProcesosDTO>> ObtenerProcesosAuditoriaDTOAsync(int terceroClienteID, int auditoriaID);
        Task<AspNetUsers> ObtenerUsuarioQueFirmaAsync(string usuarioID);
        Task<string> SaveAllAsync(Auditorias model);
        Task<string> SeleccionarProcesoParaAuditarAsync(int auditoriaID, int procesoID);
        Task<MultiSelectList> SelectListAuditoriaProcesosAsync(int auditoriaID, string valorSeleccionado = null);
        Task<SelectList> SelectListNormasAsync(string valorSeleccionado = null);
        Task<SelectList> SelectListUsuariosAsync(string valorSeleccionado = null);
    }
}
