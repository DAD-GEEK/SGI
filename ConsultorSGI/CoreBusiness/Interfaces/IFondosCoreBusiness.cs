using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IFondosCoreBusiness : ICRUDGenerico<Fondos>, IDisposable
    {
        Task<string> SaveAllAsync(Fondos model);
        Task<SelectList> SelectListAsync(string tipoFondo, string valueSelected = null);
        Task<SelectList> SelectListTiposFondosAsync(string valueSelected = null);
        string ValidarRelacionesConEntidades(Fondos modelo);
    }
}
