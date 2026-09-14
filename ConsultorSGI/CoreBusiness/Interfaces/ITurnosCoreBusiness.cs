using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface ITurnosCoreBusiness : ICRUDGenerico<Turnos>, IDisposable
    {
        Task<SelectList> SelectListAsync(string valueSelected = null);
        Task<string> SaveAllAsync(Turnos model);
    }
}
