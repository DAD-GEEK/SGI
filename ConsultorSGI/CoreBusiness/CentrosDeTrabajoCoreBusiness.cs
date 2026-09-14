using Across.ArchivosDeRecurso;
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
    public class CentrosDeTrabajoCoreBusiness : CRUDGenerico<CentrosDeTrabajo>, ICentrosDeTrabajoCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private int terceroID;
        private ICommonCoreBusiness _iCommonCoreBusiness => new CommonCoreBusiness();
        public CentrosDeTrabajoCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this.terceroID = _iCommonCoreBusiness.GetTerceroIDFromCurrentUser();
        }

        public new async Task<string> SaveEntityAsync(CentrosDeTrabajo entity)
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

        public new async Task<string> CreateAsync(CentrosDeTrabajo entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntTerceroID == entity.IntTerceroID);
                if (existeRegistro) return string.Format(RecursoCentrosDeTrabajo.msnRegistroYaExiste, entity.StrCodigo);

                entity.StrCodigo = entity.StrCodigo;
                entity.StrDescripcion = entity.StrDescripcion;
                entity.IntTerceroID = entity.IntTerceroID;
                entity.BitActivo = entity.BitActivo;
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(CentrosDeTrabajo entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<CentrosDeTrabajo> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<CentrosDeTrabajo, bool>> match)
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

        public new async Task<CentrosDeTrabajo> FindAsync(Expression<Func<CentrosDeTrabajo, bool>> match)
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

        public new List<CentrosDeTrabajo> GetAll()
        {
            try
            {
                return base.GetAll().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<CentrosDeTrabajo>> GetAllAsync()
        {
            try
            {
                var listaEntidad = await base.GetAllAsync();
                return listaEntidad.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CentrosDeTrabajo>> GetAllByCurrenTerceroAsync()
        {
            try
            {
                return await base.FindWhereAsync(x => x.IntTerceroID == terceroID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(CentrosDeTrabajo entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntCentroDeTrabajoID != entity.IntCentroDeTrabajoID && x.IntTerceroID == entity.IntTerceroID);
                if (existeRegistro) return string.Format(RecursoCentrosDeTrabajo.msnRegistroYaExiste, entity.StrCodigo);

                var CentrosDeTrabajo = await this.FindAsync(x => x.IntCentroDeTrabajoID == entity.IntCentroDeTrabajoID);

                CentrosDeTrabajo.StrCodigo = entity.StrCodigo;
                CentrosDeTrabajo.StrDescripcion = entity.StrDescripcion;
                CentrosDeTrabajo.IntTerceroID = entity.IntTerceroID;
                CentrosDeTrabajo.BitActivo = entity.BitActivo;
                await base.UpdateAsync(CentrosDeTrabajo);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(CentrosDeTrabajo model)
        {
            try
            {
                if (model.IntCentroDeTrabajoID != 0) return await this.UpdateAsync(model);
                return await this.CreateAsync(model);
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
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                var entidad = (from c in await this.GetAllAsync()
                               where c.IntTerceroID == terceroID
                               orderby c.StrCodigo
                               select new { CodigoID = c.IntCentroDeTrabajoID, Descripcion = $"{c.StrCodigo} - {c.StrDescripcion}" });

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

        ~CentrosDeTrabajoCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iCommonCoreBusiness != null)
                {
                    _iCommonCoreBusiness.Dispose();
                }


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
