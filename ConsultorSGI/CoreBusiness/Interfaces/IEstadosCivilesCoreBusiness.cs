using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IEstadosCivilesCoreBusiness : ICRUDGenerico<EstadosCiviles>, IDisposable
    {
        Task<SelectList> SelectListAsync(string valueSelected = null);
    }
}
