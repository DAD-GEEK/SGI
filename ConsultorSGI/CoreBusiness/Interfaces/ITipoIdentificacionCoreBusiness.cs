using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface ITipoIdentificacionCoreBusiness : ICRUDGenerico<TipoIdentificacion>, IDisposable
    {
        Task<SelectList> SelectListAsync(string valueSelected = null);
    }
}
