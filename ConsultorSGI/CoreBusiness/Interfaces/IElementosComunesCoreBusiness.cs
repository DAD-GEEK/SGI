using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IElementosComunesCoreBusiness : ICRUDGenerico<ElementosComunes>, IDisposable
    {
        Task<SelectList> DropDownListElementosComunesAsync(string valorSeleccionado = null);
        Task<MultiSelectList> DropDownListMultipleNormasAsync(ElementosComunes RequisitosYSoportes = null);
        Task<string> SaveAllAsync(ElementosComunes model);
    }
}
