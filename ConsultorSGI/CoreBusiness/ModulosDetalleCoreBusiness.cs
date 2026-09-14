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
using Across;

namespace CoreBusiness
{
    public class ModulosDetalleCoreBusiness : CRUDGenerico<ModulosDetalle>, IModulosDetalleCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        readonly string tipoMovimientoCreate = "C";
        readonly string tipoMovimientoUpdate = "U";

        private string _movimientoCreate;
        private string _movimientoUpdate;

        public void MovimientoCreate(string movimientoCreate)
        {
            _movimientoCreate = movimientoCreate;
        }

        public void MovimientoUpdate(string movimientoUpdate)
        {
            _movimientoUpdate = movimientoUpdate;
        }

        public string movimientoCreate { get; set; }
        public string movimientoUpdate { get; set; }


        public ModulosDetalleCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(ModulosDetalle entity)
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

        public new async Task<string> CreateAsync(ModulosDetalle entity)
        {
            try
            {
                var listaModulosDetalle = await this.GetAllAsync();
                if (listaModulosDetalle.Any(x => x.StrModuloDetalle.Trim().ToLower() == entity.StrModuloDetalle.Trim().ToLower())) return RecursoModulos.msnModuloYaExiste;

                entity.IntModuloID = entity.IntModuloID;
                entity.StrModuloDetalle = entity.StrModuloDetalle;
                entity.StrDescripcion = entity.StrDescripcion;
                entity.StrControlador = entity.StrControlador;
                entity.StrAccion = entity.StrAccion;
                entity.StrIcono = entity.StrIcono;
                entity.TIntOrden = entity.TIntOrden;
                entity.DatFechaCreacion = entity.DatFechaCreacion;
                entity.BitActivo = entity.BitActivo;

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(ModulosDetalle entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<ModulosDetalle> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<ModulosDetalle, bool>> match)
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

        public new async Task<ModulosDetalle> FindAsync(Expression<Func<ModulosDetalle, bool>> match)
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

        public new ModulosDetalle Find(Expression<Func<ModulosDetalle, bool>> match)
        {
            try
            {
                return base.Find(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<ModulosDetalle>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(ModulosDetalle entity)
        {
            try
            {
                var listaModulosDetalle = await this.GetAllAsync();
                if (listaModulosDetalle.Any(x => x.StrModuloDetalle.Trim().ToLower() == entity.StrModuloDetalle.Trim().ToLower() && x.IntModuloDetalleID != entity.IntModuloDetalleID)) return RecursoModulos.msnModuloYaExiste;

                ModulosDetalle ModulosDetalle = await this.FindAsync(x => x.IntModuloDetalleID == entity.IntModuloDetalleID);

                ModulosDetalle.IntModuloID = entity.IntModuloID;
                ModulosDetalle.StrModuloDetalle = entity.StrModuloDetalle;
                ModulosDetalle.StrDescripcion = entity.StrDescripcion;
                ModulosDetalle.StrControlador = entity.StrControlador;
                ModulosDetalle.StrAccion = entity.StrAccion;
                ModulosDetalle.StrIcono = entity.StrIcono;
                ModulosDetalle.TIntOrden = entity.TIntOrden;
                ModulosDetalle.DatFechaCreacion =  Common.ObtenerFechaExacta((DateTime)entity.DatFechaCreacion);
                ModulosDetalle.BitActivo = entity.BitActivo;

                await base.UpdateAsync(ModulosDetalle);

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

        ~ModulosDetalleCoreBusiness()
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
