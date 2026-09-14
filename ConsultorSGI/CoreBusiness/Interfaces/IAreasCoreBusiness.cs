using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IAreasCoreBusiness : ICRUDGenerico<Areas>, IDisposable
    {
        Task<string> SaveAllAsync(Areas model);
        Task<SelectList> SelectListAsync(string valueSelected = null);
    }
}
