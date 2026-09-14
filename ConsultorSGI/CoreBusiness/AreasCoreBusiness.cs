using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class AreasCoreBusiness : CRUDGenerico<Areas>, IAreasCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness => new CommonCoreBusiness();

        public AreasCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(Areas entity)
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

        public new async Task<string> CreateAsync(Areas entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntTerceroID == entity.IntTerceroID);

                if (existeRegistro)
                    return string.Format(RecursoAreas.msnRegistroYaExiste, entity.StrCodigo);

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

        public new async Task DeleteAsync(Areas entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Areas> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Areas, bool>> match)
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

        public new async Task<Areas> FindAsync(Expression<Func<Areas, bool>> match)
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

        public new List<Areas> GetAll()
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

        public new async Task<List<Areas>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Areas entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntAreaID != entity.IntAreaID && x.IntTerceroID == entity.IntTerceroID);

                if (existeRegistro)
                    return string.Format(RecursoAreas.msnRegistroYaExiste, entity.StrCodigo);

                var Areas = await this.FindAsync(x => x.IntAreaID == entity.IntAreaID);

                Areas.StrCodigo = entity.StrCodigo;
                Areas.StrDescripcion = entity.StrDescripcion;
                Areas.IntTerceroID = entity.IntTerceroID;
                Areas.BitActivo = entity.BitActivo;
                await base.UpdateAsync(Areas);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Areas model)
        {
            try
            {
                if (model.IntAreaID != 0) return await this.UpdateAsync(model);
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
                               select new { CodigoID = c.IntAreaID, Descripcion = $"{c.StrCodigo} - {c.StrDescripcion}" });

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "CodigoID", "Descripcion");

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

        ~AreasCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _iCommonCoreBusiness.Dispose();
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
