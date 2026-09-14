using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface INivelesDeRiesgoCoreBusiness : ICRUDGenerico<NivelesDeRiesgo>, IDisposable
    {
        Task<SelectList> DropDownListNivelesDeRiesgoAsync(string valueSelected = null);
    }
}
