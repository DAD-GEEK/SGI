using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IElementosComunes_NormasCoreBusiness : ICRUDGenerico<ElementosComunes_Normas>, IDisposable
    {
        Task<string> SaveAllAsync(ElementosComunes_Normas model);
    }
}
