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
    public class CiudadesCoreBusiness : CRUDGenerico<Ciudades>, ICiudades
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public CiudadesCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Ciudades entity)
        {
            try
            {

                entity.StrCodigo = entity.StrCodigo.Trim();
                entity.StrDescripcion = entity.StrDescripcion.Trim();
                entity.IntDepartamentoID = entity.IntDepartamentoID;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Ciudades entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Ciudades> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Ciudades, bool>> match)
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

        public new async Task<Ciudades> FindAsync(Expression<Func<Ciudades, bool>> match)
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

        public new async Task<List<Ciudades>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Ciudades entity)
        {
            try
            { 

                Ciudades Ciudades = await this.FindAsync(x => x.IntCiudadID == entity.IntCiudadID);

                Ciudades.StrCodigo = entity.StrCodigo.Trim();
                Ciudades.StrDescripcion = entity.StrDescripcion.Trim();
                Ciudades.IntDepartamentoID = entity.IntDepartamentoID;
               
                await base.UpdateAsync(Ciudades);

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

        ~CiudadesCoreBusiness()
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
