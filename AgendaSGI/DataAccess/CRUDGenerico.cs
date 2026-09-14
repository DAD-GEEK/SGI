using Across.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CRUDGenerico<T> : ICRUDGenerico<T> where T : class
    {

        private readonly datosNetEntities _context;
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public CRUDGenerico(datosNetEntities context)
        {
            _context = context;
        }
        public async Task CreateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                await SaveAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                await SaveAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entity)
        {
            try
            {
                _context.Set<T>().RemoveRange(entity);
                await SaveAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                return await _context.Set<T>().AnyAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                return await _context.Set<T>().SingleOrDefaultAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<T>> FindWhereAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                return await _context.Set<T>().Where(match).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<T> FindWhere(Expression<Func<T, bool>> match)
        {
            try
            {
                return _context.Set<T>().Where(match).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<T> GetAll()
        {
            try
            {
                return _context.Set<T>().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await SaveAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CRUDGenerico()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (_context != null)
                {
                    _context.Dispose();
                }
            }

            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }


        #endregion Dispose

    }
}
