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
    public class EscolaridadesCoreBusiness : CRUDGenerico<Escolaridades>, IEscolaridadesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        public EscolaridadesCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(Escolaridades entity)
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

        public new async Task<string> CreateAsync(Escolaridades entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower());
                if (existeRegistro) return string.Format(RecursoCommon.msnRegistroYaExiste, entity.StrCodigo);

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

        public new async Task DeleteAsync(Escolaridades entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Escolaridades> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Escolaridades, bool>> match)
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

        public new async Task<Escolaridades> FindAsync(Expression<Func<Escolaridades, bool>> match)
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

        public new List<Escolaridades> GetAll()
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

        public new async Task<List<Escolaridades>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Escolaridades entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntEscolaridadID != entity.IntEscolaridadID);
                if (existeRegistro) return string.Format(RecursoCommon.msnRegistroYaExiste, entity.StrCodigo);

                var Escolaridades = await this.FindAsync(x => x.IntEscolaridadID == entity.IntEscolaridadID);

                Escolaridades.StrCodigo = entity.StrCodigo;
                Escolaridades.StrDescripcion = entity.StrDescripcion;
                Escolaridades.BitActivo = entity.BitActivo;
                await base.UpdateAsync(Escolaridades);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Escolaridades model)
        {
            try
            {
                if (model.IntEscolaridadID != 0) return await this.UpdateAsync(model);
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
                var entidad = (from c in await this.GetAllAsync()
                               orderby c.StrCodigo
                               select new { CodigoID = c.IntEscolaridadID, Descripcion = $"{c.StrCodigo} - {c.StrDescripcion}" });

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

        ~EscolaridadesCoreBusiness()
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
