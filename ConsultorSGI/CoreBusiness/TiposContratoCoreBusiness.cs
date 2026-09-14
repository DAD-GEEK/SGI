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
    public class TiposContratoCoreBusiness : CRUDGenerico<TiposContrato>, ITiposContratoCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        public TiposContratoCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(TiposContrato entity)
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

        public new async Task<string> CreateAsync(TiposContrato entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrDescripcion.Trim().ToLower() == entity.StrDescripcion.Trim().ToLower());
                if (existeRegistro) return RecursoTiposContrato.msnRegistroYaExiste;

                entity.StrDescripcion = entity.StrDescripcion;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(TiposContrato entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<TiposContrato> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<TiposContrato, bool>> match)
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

        public new async Task<TiposContrato> FindAsync(Expression<Func<TiposContrato, bool>> match)
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

        public new List<TiposContrato> GetAll()
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

        public new async Task<List<TiposContrato>> GetAllAsync()
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
        public new async Task<string> UpdateAsync(TiposContrato entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrDescripcion.Trim().ToLower() == entity.StrDescripcion.Trim().ToLower() && x.IntTipoContratoID != entity.IntTipoContratoID);
                if (existeRegistro) return RecursoTiposContrato.msnRegistroYaExiste;

                var TiposContrato = await this.FindAsync(x => x.StrDescripcion == entity.StrDescripcion);

                TiposContrato.StrDescripcion = entity.StrDescripcion;
              
                await base.UpdateAsync(TiposContrato);

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
                               select new { CodigoID = c.IntTipoContratoID, Descripcion = $"{c.StrDescripcion}" });

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

        ~TiposContratoCoreBusiness()
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
