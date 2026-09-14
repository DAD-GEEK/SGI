using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ICRUDGenerico<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        List<T> GetAll();
        Task SaveEntityAsync(T entity);
        //Task CreateAsync(T entity);
        //Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entity);
        Task<bool> ExistAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<List<T>> FindWhereAsync(Expression<Func<T, bool>> match);
        List<T> FindWhere(Expression<Func<T, bool>> match);
    }
}
