using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class ModulosPorRolCoreBusiness : CRUDGenerico<ModulosPorRol>, IModulosPorRolCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        public ModulosPorRolCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(ModulosPorRol entity)
        {
            try
            {
                await base.SaveEntityAsync(entity);

                return string.Empty;          
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateAsync(ModulosPorRol entity)
        {
            try
            {
                entity.IntRolID = entity.IntRolID;
                entity.IntModuloDetalleID = entity.IntModuloDetalleID;
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(ModulosPorRol entity)
        {
            try
            {
                await base.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteRangeAsync(IEnumerable<ModulosPorRol> entity)
        {
            try
            {
                await base.DeleteRangeAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<bool> ExistAsync(Expression<Func<ModulosPorRol, bool>> match)
        {
            try
            {
                return await base.ExistAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<ModulosPorRol> FindAsync(Expression<Func<ModulosPorRol, bool>> match)
        {
            try
            {
                return await base.FindAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<ModulosPorRol>> GetAllAsync()
        {
            try
            {
                return await base.GetAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(ModulosPorRol entity)
        {
            try
            {
                var ModulosPorRol = await this.FindAsync(x => x.IntModulosPorRolID == entity.IntModulosPorRolID);

                ModulosPorRol.IntRolID = entity.IntRolID;
                ModulosPorRol.IntModuloDetalleID = entity.IntModuloDetalleID;
                await base.UpdateAsync(ModulosPorRol);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(ModulosPorRol model)
        {
            try
            {
                if (model.IntModulosPorRolID != 0) return await this.UpdateAsync(model);
                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ModulosPorRolCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {


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
