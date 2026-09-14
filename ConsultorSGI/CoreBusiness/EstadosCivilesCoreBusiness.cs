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
    public class EstadosCivilesCoreBusiness : CRUDGenerico<EstadosCiviles>, IEstadosCivilesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public EstadosCivilesCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(EstadosCiviles entity)
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

        public new async Task<string> CreateAsync(EstadosCiviles entity)
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

        public new async Task DeleteAsync(EstadosCiviles entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<EstadosCiviles> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<EstadosCiviles, bool>> match)
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

        public new async Task<EstadosCiviles> FindAsync(Expression<Func<EstadosCiviles, bool>> match)
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

        public new async Task<List<EstadosCiviles>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(EstadosCiviles entity)
        {
            try
            {
                var EstadosCiviles = await this.FindAsync(x => x.IntEstadoCivilID == entity.IntEstadoCivilID);

                EstadosCiviles.StrCodigo = entity.StrCodigo;
                EstadosCiviles.StrDescripcion = entity.StrDescripcion;
                await base.UpdateAsync(EstadosCiviles);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> SelectListAsync(string valueSelected = null)
        {
            try
            {
                var entidad = (from c in await this.GetAllAsync()
                               orderby c.StrCodigo
                               select new { CodigoID = c.IntEstadoCivilID, Descripcion = $"{c.StrCodigo} - {c.StrDescripcion}" });

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

        ~EstadosCivilesCoreBusiness()
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
