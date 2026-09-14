using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IAuditoriasDetalleCoreBusiness : ICRUDGenerico<AuditoriasDetalle>, IDisposable
    {
        Task<List<AuditoriasDetalleDTO>> ObtenerAuditoriaDetalleDTOAsync(Auditorias auditoria);
        Task<List<AuditoriasDetalleDTO>> ObtenerDetalleAuditoriaAsync(int auditoriaID);
        Task<string> SaveAllAsync(AuditoriasDetalle model);
        Task<MultiSelectList> SelectListAuditoriaProcesosAsync(int auditoriaID, string valorSeleccionado = null);
        SelectList SelectListModalidades(string valueSelected = null);
    }
}
