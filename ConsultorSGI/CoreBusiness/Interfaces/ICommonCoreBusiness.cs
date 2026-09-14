using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface ICommonCoreBusiness : IDisposable
    {
        Task<int> GetTerceroIDFromCurrentUserAsync();
        int GetTerceroIDFromCurrentUser();
        Task<Terceros> GetTerceroModelFromCurrentUser();
        Task<AspNetUsers> GetCurrentUser();
        string GetUrlPrincipal();
        int ObtenerTerceroID();
        void AsignarTerceroID(int terceroID);
        SelectList DropDownListEnumerable<T>(string valueSelected = null);
        List<SelectListItem> SeleccionarRegistroEnDropDownList(string id, string text);
        DatatableParamsDTO GetParametrosDataTable(HttpRequestBase Request);
        Task<int> GenerarTerceroClienteGenericoAsync();
    }
}
