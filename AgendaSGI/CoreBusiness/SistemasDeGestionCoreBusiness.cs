using Across.Interfaces;
using DataAccess;
using Across.ArchivosDeRecurso;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class SistemasDeGestionCoreBusiness : CRUDGenerico<SistemasDeGestion>, ISistemasDeGestion
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public SistemasDeGestionCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(SistemasDeGestion entity)
        {
            try
            {
                var listaSistemasDeGestion = await GetAllAsync();

                if (listaSistemasDeGestion.Any(x => x.StrCodigo.ToLower().Equals(entity.StrCodigo.ToLower())))
                {
                    return RecursoSistemasDeGestion.msnValidacionCrear;
                }

                entity.StrCodigo = entity.StrCodigo.Trim();
                entity.StrDescripcion = entity.StrDescripcion.Trim();
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(SistemasDeGestion entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<SistemasDeGestion> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<SistemasDeGestion, bool>> match)
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

        public new async Task<SistemasDeGestion> FindAsync(Expression<Func<SistemasDeGestion, bool>> match)
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

        public new async Task<List<SistemasDeGestion>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(SistemasDeGestion entity)
        {
            try
            {
                var listaSistemasDeGestion = await GetAllAsync();

                if (listaSistemasDeGestion.Any(x => x.StrCodigo.ToLower().Equals(entity.StrCodigo.ToLower()) && x.IntSistemaID != entity.IntSistemaID))
                {
                    return RecursoSistemasDeGestion.msnValidacionCrear;
                }

                SistemasDeGestion SistemasDeGestion = await this.FindAsync(x => x.IntSistemaID == entity.IntSistemaID);

                SistemasDeGestion.StrCodigo = entity.StrCodigo.Trim();
                SistemasDeGestion.StrDescripcion = entity.StrDescripcion.Trim();

               
                await base.UpdateAsync(SistemasDeGestion);

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

        ~SistemasDeGestionCoreBusiness()
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
