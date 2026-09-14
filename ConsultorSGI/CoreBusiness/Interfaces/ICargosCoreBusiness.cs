using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface ICargosCoreBusiness : ICRUDGenerico<Cargos>, IDisposable
    {
        Task<SelectList> SelectListAsync(string valueSelected = null);
        Task<string> SaveAllAsync(Cargos model);
    }
}
