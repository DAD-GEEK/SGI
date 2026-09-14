using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IFasesCoreBusiness : ICRUDGenerico<Fases>, IDisposable
    {
        Task<SelectList> DropDownListFasesAsync(string valueSelected = null);
    }
}
