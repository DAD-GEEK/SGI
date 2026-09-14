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
    public class NivelesDeRiesgoCoreBusiness : CRUDGenerico<NivelesDeRiesgo>, INivelesDeRiesgoCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public NivelesDeRiesgoCoreBusiness() : base(new gestioni_consultorNetEntities())
        {

        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(NivelesDeRiesgo entity)
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

        public new async Task<string> CreateAsync(NivelesDeRiesgo entity)
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

        public new async Task DeleteAsync(NivelesDeRiesgo entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<NivelesDeRiesgo> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<NivelesDeRiesgo, bool>> match)
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

        public new async Task<NivelesDeRiesgo> FindAsync(Expression<Func<NivelesDeRiesgo, bool>> match)
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

        public new async Task<List<NivelesDeRiesgo>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(NivelesDeRiesgo entity)
        {
            try
            {
                var NivelesDeRiesgo = await this.FindAsync(x => x.StrNivelDeRiesgoID == entity.StrNivelDeRiesgoID);

                NivelesDeRiesgo.StrCodigo = entity.StrCodigo;
                NivelesDeRiesgo.StrDescripcion = entity.StrDescripcion;

                await base.UpdateAsync(NivelesDeRiesgo);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region DropDownList
        public async Task<SelectList> DropDownListNivelesDeRiesgoAsync(string valueSelected = null)
        {
            try
            {
                var entidad = (from c in await this.GetAllAsync()
                               orderby c.StrCodigo
                               select new { CodigoID = c.StrNivelDeRiesgoID, Descripcion = $"{c.StrDescripcion}" });

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

        ~NivelesDeRiesgoCoreBusiness()
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
