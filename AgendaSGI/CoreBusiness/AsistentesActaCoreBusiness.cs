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
    public class AsistentesActaCoreBusiness : CRUDGenerico<AsistentesActa>, IAsistentesActa
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public AsistentesActaCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(AsistentesActa entity)
        {
            try
            {
                entity.StrNombre = entity.StrNombre.Trim();
                entity.OpcAsiste = entity.OpcAsiste;
                entity.IntActaID = entity.IntActaID;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AsistentesActa entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AsistentesActa> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AsistentesActa, bool>> match)
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

        public new async Task<AsistentesActa> FindAsync(Expression<Func<AsistentesActa, bool>> match)
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

        public new async Task<List<AsistentesActa>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(AsistentesActa entity)
        {
            try
            {

                AsistentesActa AsistentesActa = await this.FindAsync(x => x.IntActaID == entity.IntActaID);

                AsistentesActa.StrNombre = entity.StrNombre.Trim();
                AsistentesActa.OpcAsiste = entity.OpcAsiste;
                AsistentesActa.IntActaID = entity.IntActaID;

                await base.UpdateAsync(AsistentesActa);

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

        ~AsistentesActaCoreBusiness()
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
