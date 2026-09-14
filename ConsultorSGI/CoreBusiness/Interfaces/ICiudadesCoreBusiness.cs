using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface ICiudadesCoreBusiness : ICRUDGenerico<Ciudades>, IDisposable
    {
        Task<SelectList> DropDownListCiudadesAsync(string valueSelected = null);
        Task<string> SaveEntityAsync(Ciudades entity);
    }
}
