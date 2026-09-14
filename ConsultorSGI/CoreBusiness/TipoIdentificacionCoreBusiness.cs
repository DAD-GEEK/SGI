using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace CoreBusiness
{
    public class TipoIdentificacionCoreBusiness : CRUDGenerico<TipoIdentificacion>, ITipoIdentificacionCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public TipoIdentificacionCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(TipoIdentificacion entity)
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

        public new async Task<string> CreateAsync(TipoIdentificacion entity)
        {
            try
            {
                entity.StrCodigo = entity.StrCodigo;
                entity.StrDescripcion = entity.StrDescripcion;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(TipoIdentificacion entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<TipoIdentificacion> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<TipoIdentificacion, bool>> match)
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

        public new async Task<TipoIdentificacion> FindAsync(Expression<Func<TipoIdentificacion, bool>> match)
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

        public new async Task<List<TipoIdentificacion>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(TipoIdentificacion entity)
        {
            try
            {
                var TipoIdentificacion = await this.FindAsync(x => x.StrCodigo == entity.StrCodigo);

                TipoIdentificacion.StrCodigo = entity.StrCodigo;
                TipoIdentificacion.StrDescripcion = entity.StrDescripcion;

                await base.UpdateAsync(TipoIdentificacion);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Select List
        public async Task<SelectList> SelectListAsync(string valueSelected = null)
        {
            try
            {
                var entidad = (from c in await this.GetAllAsync()
                                orderby c.StrCodigo
                                select new { CodigoID = c.IntTipoIdentificacionID, Descripcion = $"{c.StrCodigo} - {c.StrDescripcion}" });

                if (string.IsNullOrEmpty(valueSelected)) return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
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

        ~TipoIdentificacionCoreBusiness()
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
