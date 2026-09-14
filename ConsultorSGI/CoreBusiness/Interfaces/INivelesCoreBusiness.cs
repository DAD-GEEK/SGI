using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface INivelesCoreBusiness : ICRUDGenerico<Niveles>, IDisposable
    {
        Task<MultiSelectList> DropDownListMultipleNivelesAsync(List<string> valueSelected = null);
        Task<SelectList> DropDownListNivelesAsync(string valueSelected = null);
    }
}
