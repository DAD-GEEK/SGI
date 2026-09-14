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
    public class FasesCoreBusiness : CRUDGenerico<Fases>, IFasesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public FasesCoreBusiness() : base(new gestioni_consultorNetEntities())
        {

        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(Fases entity)
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

        public new async Task<string> CreateAsync(Fases entity)
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

        public new async Task DeleteAsync(Fases entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Fases> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Fases, bool>> match)
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

        public new async Task<Fases> FindAsync(Expression<Func<Fases, bool>> match)
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

        public new async Task<List<Fases>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Fases entity)
        {
            try
            {
                var Fases = await this.FindAsync(x => x.StrFaseID == entity.StrFaseID);

                Fases.StrDescripcionPrimaria = entity.StrDescripcionPrimaria;
                Fases.StrDescripcionSecundaria = entity.StrDescripcionSecundaria;

                await base.UpdateAsync(Fases);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region DropDownList
        public async Task<SelectList> DropDownListFasesAsync(string valueSelected = null)
        {
            try
            {
                var entidad = (from c in await this.GetAllAsync()
                               orderby c.StrDescripcionPrimaria
                               select new { CodigoID = c.StrFaseID, Descripcion = $"{c.StrDescripcionPrimaria}" });

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "CodigoID", "Descripcion");

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

        ~FasesCoreBusiness()
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
