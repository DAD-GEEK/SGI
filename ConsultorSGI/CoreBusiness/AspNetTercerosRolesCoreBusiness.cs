using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class AspNetTerceroRolesCoreBusiness : CRUDGenerico<AspNetTerceroRoles>, IAspNetTerceroRolesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public AspNetTerceroRolesCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(AspNetTerceroRoles entity)
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

        public new async Task<string> CreateAsync(AspNetTerceroRoles entity)
        {
            try
            {
                var existeRegistro = await this.FindWhereAsync(x => x.IntTerceroID == entity.IntTerceroID && x.IntRolID == entity.IntRolID);

                if (existeRegistro.Count() != 0)
                    return RecursoTerceros.msnRolDuplicado;

                entity.IntRolID = entity.IntRolID;
                entity.IntTerceroID = entity.IntTerceroID;
              
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AspNetTerceroRoles entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AspNetTerceroRoles> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AspNetTerceroRoles, bool>> match)
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

        public new async Task<AspNetTerceroRoles> FindAsync(Expression<Func<AspNetTerceroRoles, bool>> match)
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

        public new List<AspNetTerceroRoles> GetAll()
        {
            try
            {
                return base.GetAll().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<AspNetTerceroRoles>> GetAllAsync()
        {
            try
            {
                var listaEntidad = await base.GetAllAsync();
                return listaEntidad.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(AspNetTerceroRoles entity)
        {
            try
            {
               
                AspNetTerceroRoles AspNetTerceroRoles = await this.FindAsync(x => x.IntRolID == entity.IntRolID);

                AspNetTerceroRoles.IntRolID = entity.IntRolID;
                AspNetTerceroRoles.IntTerceroID = entity.IntTerceroID;

                await base.UpdateAsync(AspNetTerceroRoles);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(AspNetTerceroRoles model)
        {
            try
            {
                if (model.IntAspNetTerceroRolesID != 0 ) 
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AspNetTerceroRolesCoreBusiness()
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
