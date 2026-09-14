using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IEscolaridadesCoreBusiness : ICRUDGenerico<Escolaridades>, IDisposable
    {
        Task<SelectList> SelectListAsync(string valueSelected = null);
        Task<string> SaveAllAsync(Escolaridades model);
    }
}
