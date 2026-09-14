using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IAspNetRolesCoreBusiness : ICRUDGenerico<AspNetRoles>, IDisposable
    {
        Task<dynamic> SelectListAsync(string valueSelected = null, List<string> groupSelected = null, bool bitDefault = true);
        Task<string> SaveAllAsync(AspNetRoles model);
        string ValidarSiEsRolPrincipal(string rolID);
    }
}
