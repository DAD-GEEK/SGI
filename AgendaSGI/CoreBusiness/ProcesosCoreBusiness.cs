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
    public class ProcesosCoreBusiness : CRUDGenerico<Procesos>, IProcesos
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ProcesosCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Procesos entity)
        {
            try
            {
                var listaProcesos = await GetAllAsync();

                if (listaProcesos.Any(x => x.StrDescripcion.ToLower().Equals(entity.StrDescripcion.ToLower())))
                {
                    return RecursoProcesos.msnValidacionExiste;
                }

                entity.StrDescripcion = entity.StrDescripcion.Trim();
                entity.OpcEstado = entity.OpcEstado;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Procesos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Procesos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Procesos, bool>> match)
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

        public new async Task<Procesos> FindAsync(Expression<Func<Procesos, bool>> match)
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

        public new async Task<List<Procesos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Procesos entity)
        {
            try
            {
                var listaProcesos = await GetAllAsync();

                if (listaProcesos.Any(x => x.StrDescripcion.ToLower().Equals(entity.StrDescripcion.ToLower()) && x.IntProcesoID != entity.IntProcesoID))
                {
                    return RecursoProcesos.msnValidacionExiste;
                }

                Procesos Procesos = await this.FindAsync(x => x.IntProcesoID == entity.IntProcesoID);

                Procesos.StrDescripcion = entity.StrDescripcion.Trim();
                Procesos.OpcEstado = entity.OpcEstado;

                await base.UpdateAsync(Procesos);

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

        ~ProcesosCoreBusiness()
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
