using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IAspNetTerceroRolesCoreBusiness : ICRUDGenerico<AspNetTerceroRoles>, IDisposable
    {
        Task<string> SaveAllAsync(AspNetTerceroRoles model);
    }
}
