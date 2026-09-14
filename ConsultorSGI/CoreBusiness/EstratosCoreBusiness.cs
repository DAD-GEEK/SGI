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
    public class EstratosCoreBusiness : CRUDGenerico<Estratos>, IEstratosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public EstratosCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(Estratos entity)
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

        public new async Task<string> CreateAsync(Estratos entity)
        {
            try
            {
                entity.StrDescripcion = entity.StrDescripcion;
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Estratos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Estratos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Estratos, bool>> match)
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

        public new async Task<Estratos> FindAsync(Expression<Func<Estratos, bool>> match)
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

        public new async Task<List<Estratos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Estratos entity)
        {
            try
            {
                var Estratos = await this.FindAsync(x => x.TIntEstratoID == entity.TIntEstratoID);

                Estratos.StrDescripcion = entity.StrDescripcion;
                await base.UpdateAsync(Estratos);

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
                               orderby c.StrDescripcion
                               select new { CodigoID = c.TIntEstratoID, Descripcion = $"{c.StrDescripcion}" });

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

        ~EstratosCoreBusiness()
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
