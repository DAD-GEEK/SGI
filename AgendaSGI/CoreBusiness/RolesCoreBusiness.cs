using Across.ArchivosDeRecurso;
using Across.Interfaces;
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
    public class RolesCoreBusiness : CRUDGenerico<Roles>, IRoles
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public RolesCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Roles entity)
        {
            try
            {
                var listaRoles = await GetAllAsync();

                if (listaRoles.Any(x => x.StrDescripcion.ToLower().Trim().Equals(entity.StrDescripcion.ToLower().Trim())))
                {
                    return RecursoRoles.msnValidacionCrear;
                }

                entity.StrCodigo = entity.StrCodigo;
                entity.StrDescripcion = entity.StrDescripcion.Trim();

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Roles entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Roles> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Roles, bool>> match)
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

        public new async Task<Roles> FindAsync(Expression<Func<Roles, bool>> match)
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

        public new async Task<List<Roles>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Roles entity)
        {
            try
            {
                var listaRoles = await GetAllAsync();

                if (listaRoles.Any(x => x.StrDescripcion.ToLower().Trim() == entity.StrDescripcion.ToLower().Trim() && x.IntRolID != entity.IntRolID))
                {
                    return RecursoRoles.msnValidacionCrear;
                }

                Roles Roles = await this.FindAsync(x => x.IntRolID == entity.IntRolID);

                Roles.StrCodigo = entity.StrCodigo;
                Roles.StrDescripcion = entity.StrDescripcion.Trim();

                await base.UpdateAsync(Roles);

                return string.Empty;

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

        ~RolesCoreBusiness()
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
