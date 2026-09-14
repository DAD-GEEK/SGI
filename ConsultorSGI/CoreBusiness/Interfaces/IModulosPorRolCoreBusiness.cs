using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IModulosPorRolCoreBusiness : ICRUDGenerico<ModulosPorRol>, IDisposable
    {
        Task<string> SaveAllAsync(ModulosPorRol model);
    }
}
