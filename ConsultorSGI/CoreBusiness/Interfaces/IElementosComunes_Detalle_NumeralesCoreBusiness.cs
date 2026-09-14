using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IElementosComunes_Detalle_NumeralesCoreBusiness : ICRUDGenerico<ElementosComunes_Detalle_Numerales>, IDisposable
    {
        Task<string> SaveAllAsync(ElementosComunes_Detalle_Numerales model);
    }
}
