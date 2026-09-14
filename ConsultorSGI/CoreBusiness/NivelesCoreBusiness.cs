using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class NivelesCoreBusiness : CRUDGenerico<Niveles>, INivelesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public NivelesCoreBusiness() : base(new gestioni_consultorNetEntities())
        {

        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(Niveles entity)
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

        public new async Task<string> CreateAsync(Niveles entity)
        {
            try
            {
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Niveles entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Niveles> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Niveles, bool>> match)
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

        public new async Task<Niveles> FindAsync(Expression<Func<Niveles, bool>> match)
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

        public new async Task<List<Niveles>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Niveles entity)
        {
            try
            {
                var Niveles = await this.FindAsync(x => x.StrNivelID == entity.StrNivelID);

                Niveles.StrDescripcion = entity.StrDescripcion;
                Niveles.IntOrden = entity.IntOrden;

                await base.UpdateAsync(Niveles);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region DropDownList
        public async Task<SelectList> DropDownListNivelesAsync(string valueSelected = null)
        {
            try
            {
                var entidad = (from c in await this.GetAllAsync()
                               orderby c.IntOrden
                               select new { CodigoID = c.StrNivelID, Descripcion = $"{c.StrDescripcion}" });

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> DropDownListMultipleNivelesAsync(List<string> valueSelected = null)
        {
            try
            {
                var entidad = (from c in await this.GetAllAsync()
                               orderby c.IntOrden
                               select new { CodigoID = c.StrNivelID, Descripcion = $"{c.StrDescripcion}" });

                if (valueSelected == null)
                    return new MultiSelectList(entidad, "CodigoID", "Descripcion");

                return new MultiSelectList(entidad, "CodigoID", "Descripcion", valueSelected);

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

        ~NivelesCoreBusiness()
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
