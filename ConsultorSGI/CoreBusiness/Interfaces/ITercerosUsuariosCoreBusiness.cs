using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface ITercerosUsuariosCoreBusiness : ICRUDGenerico<TercerosUsuarios>, IDisposable
    {
        Task<string> SaveAllAsync(TercerosUsuarios model);
    }
}
