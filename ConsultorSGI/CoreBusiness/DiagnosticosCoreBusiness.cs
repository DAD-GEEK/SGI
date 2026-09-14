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
    public class DiagnosticosCoreBusiness : CRUDGenerico<Diagnosticos>, IDiagnosticosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public DiagnosticosCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(Diagnosticos entity)
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

        public new async Task<string> CreateAsync(Diagnosticos entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower());
                if (existeRegistro) return string.Format(RecursoDiagnostico.msnRegistroYaExiste, entity.StrCodigo);

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

        public new async Task DeleteAsync(Diagnosticos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Diagnosticos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Diagnosticos, bool>> match)
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

        public new async Task<Diagnosticos> FindAsync(Expression<Func<Diagnosticos, bool>> match)
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

        public new async Task<List<Diagnosticos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Diagnosticos entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntDiagnosticoID != entity.IntDiagnosticoID);
                if (existeRegistro) return string.Format(RecursoDiagnostico.msnRegistroYaExiste, entity.StrCodigo);

                var Diagnosticos = await this.FindAsync(x => x.IntDiagnosticoID == entity.IntDiagnosticoID);

                Diagnosticos.StrCodigo = entity.StrCodigo;
                Diagnosticos.StrDescripcion = entity.StrDescripcion;
                await base.UpdateAsync(Diagnosticos);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Diagnosticos model)
        {
            try
            {
                if (model.IntDiagnosticoID != 0) return await this.UpdateAsync(model);
                return await this.CreateAsync(model);
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
                               orderby tabla.StrCodigo
                               select new { CodigoID = tabla.IntDiagnosticoID, Descripcion = $"{tabla.StrCodigo} - {tabla.StrDescripcion}" });

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

        ~DiagnosticosCoreBusiness()
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
