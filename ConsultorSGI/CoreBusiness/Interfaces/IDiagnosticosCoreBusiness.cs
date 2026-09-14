using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IDiagnosticosCoreBusiness : ICRUDGenerico<Diagnosticos>, IDisposable
    {
        Task<string> SaveAllAsync(Diagnosticos model);
        Task<SelectList> SelectListAsync(string valueSelected = null);
    }
}
