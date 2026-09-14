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
    public class TipoEventosAusentismoCoreBusiness : CRUDGenerico<TipoEventosAusentismo>, ITipoEventosAusentismoCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public TipoEventosAusentismoCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(TipoEventosAusentismo entity)
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

        public new async Task<string> CreateAsync(TipoEventosAusentismo entity)
        {
            try
            {
                entity.StrCodigo = entity.StrCodigo;
                entity.StrDescripcion = entity.StrDescripcion;
                entity.BitActivo = entity.BitActivo;
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(TipoEventosAusentismo entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<TipoEventosAusentismo> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<TipoEventosAusentismo, bool>> match)
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

        public new async Task<TipoEventosAusentismo> FindAsync(Expression<Func<TipoEventosAusentismo, bool>> match)
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

        public new async Task<List<TipoEventosAusentismo>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(TipoEventosAusentismo entity)
        {
            try
            {
                var TipoEventosAusentismo = await this.FindAsync(x => x.IntTipoEventoAusentismoID == entity.IntTipoEventoAusentismoID);

                TipoEventosAusentismo.StrCodigo = entity.StrCodigo;
                TipoEventosAusentismo.StrDescripcion = entity.StrDescripcion;
                TipoEventosAusentismo.BitActivo = entity.BitActivo;
                await base.UpdateAsync(TipoEventosAusentismo);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public async Task<SelectList> SelectListAsync(string valueSelected = null)
        {
            try
            {
                var entidad = (from tabla in await this.GetAllAsync()
                               orderby tabla.IntTipoEventoAusentismoID
                               select new { CodigoID = tabla.IntTipoEventoAusentismoID, Descripcion = $"{tabla.StrCodigo} - {tabla.StrDescripcion}"});

                if (string.IsNullOrEmpty(valueSelected)) return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
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

        ~TipoEventosAusentismoCoreBusiness()
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
