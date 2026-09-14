using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IAspNetUsersCoreBusiness: IDisposable
    {
        Task<bool> ExistAsync(Expression<Func<AspNetUsers, bool>> match);
        Task<int> GetTerceroIDFromCurrentUserAsync();
        int GetTerceroIDFromCurrentUser();
        Task<AspNetUsers> FindAsync(Expression<Func<AspNetUsers, bool>> match);
        Task<string> UpdateAsync(AspNetUsers entity);
        Task<List<AspNetUsers>> GetAllAsync();
        Task<SelectList> SelectListUsuariosAsync(string valueSelected = null);
    }
}
