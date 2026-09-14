using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface ICentrosDeTrabajoCoreBusiness : ICRUDGenerico<CentrosDeTrabajo>, IDisposable
    {
        Task<SelectList> SelectListAsync(string valueSelected = null);
        Task<string> SaveAllAsync(CentrosDeTrabajo model);
        Task<List<CentrosDeTrabajo>> GetAllByCurrenTerceroAsync();
    }
}
