using DataAccess.Interfaces;
using DataAccess.Servicios;
using Models;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CRUDGenerico<T> : ICRUDGenerico<T> where T : class
    {
        private readonly DbContext _context;
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private IServicioTercero _iServicioTercero;

        public CRUDGenerico(DbContext context)
        {
            this._context = context;
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

        public async Task CreateRangeAsync(List<T> entity)
        {
            try
            {
                _context.Set<T>().AddRange(entity);
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
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                {
                    if (await this.ExistAsync(GetlambdaExpression()))
                    {
                        _context.Set<T>().Remove(entity);
                        await SaveAllAsync();
                    }
                }
                else
                {
                    _context.Set<T>().Remove(entity);
                    await SaveAllAsync();
                }
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
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                {
                    if (await this.ExistAsync(GetlambdaExpression()))
                    {
                        _context.Set<T>().RemoveRange(entity);
                        await SaveAllAsync();
                    }
                }
                else
                {
                    _context.Set<T>().RemoveRange(entity);
                    await SaveAllAsync();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                {
                    var expresionLambdaPorTercero = GetlambdaExpression();
                    var lambdaConcatenada = CombineLambdaExpression(expresionLambdaPorTercero, match);

                    return await _context.Set<T>().AnyAsync(lambdaConcatenada);
                }

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
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                {
                    var expresionLambdaPorTercero = GetlambdaExpression();
                    var lambdaConcatenada = CombineLambdaExpression(expresionLambdaPorTercero, match);

                    return await _context.Set<T>().SingleOrDefaultAsync(lambdaConcatenada);
                }

                return await _context.Set<T>().SingleOrDefaultAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            try
            {
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                {
                    var expresionLambdaPorTercero = GetlambdaExpression();
                    var lambdaConcatenada = CombineLambdaExpression(expresionLambdaPorTercero, match);

                    return _context.Set<T>().SingleOrDefault(lambdaConcatenada);
                }

                return _context.Set<T>().SingleOrDefault(match);
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
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                {
                    var expresionLambdaPorTercero = GetlambdaExpression();
                    var lambdaConcatenada = CombineLambdaExpression(expresionLambdaPorTercero, match);

                    return await _context.Set<T>().Where(lambdaConcatenada).ToListAsync();
                }

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
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                {
                    var expresionLambdaPorTercero = GetlambdaExpression();
                    var lambdaConcatenada = CombineLambdaExpression(expresionLambdaPorTercero, match);

                    return _context.Set<T>().Where(lambdaConcatenada).ToList();
                }

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
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                    return await _context.Set<T>().Where(GetlambdaExpression()).ToListAsync();

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
                if (typeof(IEntidadPorTercero).IsAssignableFrom(typeof(T)))
                    return _context.Set<T>().Where(GetlambdaExpression()).ToList();

                return _context.Set<T>().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SaveEntityAsync(T entity)
        {
            try
            {
                _context.Set<T>().AddOrUpdate(entity);
                await SaveAllAsync();
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


        private Expression<Func<T, bool>> GetlambdaExpression()
        {
            try
            {
                this._iServicioTercero = new ServicioTercero();
                int terceroID = _iServicioTercero.ObtenerTerceroDeUsuarioEnSesion();

                var parameterExpression = Expression.Parameter(typeof(T), "object");
                var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, "IntTerceroID");
                var equalityExpression = Expression.Equal(propertyOrFieldExpression, Expression.Constant(terceroID, typeof(int)));
                var lambdaExpression = Expression.Lambda<Func<T, bool>>(equalityExpression, parameterExpression);

                return lambdaExpression;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private Expression<Func<T, bool>> CombineLambdaExpression(Expression<Func<T, bool>> lambdaOne, Expression<Func<T, bool>> lambdaTwo)
        {
            try
            {
                var newlambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(new SwapVisitor(lambdaOne.Parameters[0], lambdaTwo.Parameters[0]).Visit(lambdaOne.Body), lambdaTwo.Body), lambdaTwo.Parameters);

                return newlambda;
            }
            catch (Exception)
            {

                throw;
            }
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
                    _context.Dispose();
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
