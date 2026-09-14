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
    public class FondosCoreBusiness : CRUDGenerico<Fondos>, IFondosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        ITiposFondoCoreBusiness _iTiposFondoCoreBusiness;

        public FondosCoreBusiness() : base(new gestioni_consultorNetEntities()) 
        {
            _iTiposFondoCoreBusiness = new TiposFondoCoreBusiness();
        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(Fondos entity)
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

        public new async Task<string> CreateAsync(Fondos entity)
        {
            try
            {
                var listaFondos = await this.GetAllAsync();
                var existeEmpleado = listaFondos.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower());
                if (existeEmpleado) return RecursoFondos.msnRegistroYaExiste;

                entity.StrCodigo = entity.StrCodigo;
                entity.StrDescripcion = entity.StrDescripcion;
                entity.StrNit = entity.StrNit;
                entity.IntTipoFondoID = entity.IntTipoFondoID;
                entity.BitActivo = entity.BitActivo;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Fondos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Fondos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Fondos, bool>> match)
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

        public new async Task<Fondos> FindAsync(Expression<Func<Fondos, bool>> match)
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

        public new async Task<List<Fondos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Fondos entity)
        {
            try
            {
                var listaFondos = await this.GetAllAsync();
                var existeEmpleado = listaFondos.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntFondoID != entity.IntFondoID);
                if (existeEmpleado) return RecursoFondos.msnRegistroYaExiste;

                var Fondos = await this.FindAsync(x => x.IntFondoID == entity.IntFondoID);

                Fondos.StrCodigo = entity.StrCodigo;
                Fondos.StrDescripcion = entity.StrDescripcion;
                Fondos.StrNit = entity.StrNit;
                Fondos.IntTipoFondoID = entity.IntTipoFondoID;
                Fondos.BitActivo = entity.BitActivo;

                await base.UpdateAsync(Fondos);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Fondos model)
        {
            try
            {
                if (model.IntFondoID != 0) return await this.UpdateAsync(model);
                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Select List
        public async Task<SelectList> SelectListAsync(string tipoFondo, string valueSelected = null)
        {
            try
            {
                var listaFondos = await this.GetAllAsync();
                var listaTiposFondo = await _iTiposFondoCoreBusiness.GetAllAsync();

                var entidad = (from f in listaFondos
                                 join tf in listaTiposFondo on f.IntTipoFondoID equals tf.intTipoFondoID
                                 where tf.StrCodigo == tipoFondo && f.BitActivo == true
                                 orderby f.IntFondoID
                                 select new { CodigoID = f.IntFondoID, Descripcion = $"{f.StrCodigo} - {f.StrDescripcion}" });              

                if (string.IsNullOrEmpty(valueSelected)) 
                    return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> SelectListTiposFondosAsync(string valueSelected = null)
        {
            try
            {
                return await _iTiposFondoCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public string ValidarRelacionesConEntidades(Fondos modelo)
        {
            try
            {
                string respuesta = string.Empty;

                if (modelo.Empleados.Count() != 0) return string.Format(RecursoFondos.msnRelacionConEmpleados, modelo.StrCodigo, modelo.StrDescripcion, modelo.Empleados.FirstOrDefault().StrNombreCompleto, modelo.Empleados.FirstOrDefault().Terceros.StrNombre);
                if (modelo.Empleados1.Count() != 0) return string.Format(RecursoFondos.msnRelacionConEmpleados, modelo.StrCodigo, modelo.StrDescripcion, modelo.Empleados1.FirstOrDefault().StrNombreCompleto, modelo.Empleados.FirstOrDefault().Terceros.StrNombre);
                if (modelo.Empleados2.Count() != 0) return string.Format(RecursoFondos.msnRelacionConEmpleados, modelo.StrCodigo, modelo.StrDescripcion, modelo.Empleados2.FirstOrDefault().StrNombreCompleto, modelo.Empleados.FirstOrDefault().Terceros.StrNombre);

                return string.Empty;
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

        ~FondosCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iTiposFondoCoreBusiness != null)
                {
                    _iTiposFondoCoreBusiness.Dispose();
                    _iTiposFondoCoreBusiness = null;
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
