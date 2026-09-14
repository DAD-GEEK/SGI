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

namespace CoreBusiness
{
    public class ModulosCoreBusiness : CRUDGenerico<Modulos>, IModulosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ModulosCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(Modulos entity)
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

        public new async Task<string> CreateAsync(Modulos entity)
        {
            try
            {
                var listaModulos = await this.GetAllAsync();
                if (listaModulos.Any(x => x.StrModulo.Trim().ToLower() == entity.StrModulo.Trim().ToLower())) return RecursoModulos.msnModuloYaExiste;

                entity.StrModulo = entity.StrModulo.Trim();
                entity.StrDescripcion = entity.StrDescripcion;
                entity.StrIcono = entity.StrIcono;
                entity.TIntOrden = entity.TIntOrden;
                entity.BitActivo = entity.BitActivo;

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Modulos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Modulos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Modulos, bool>> match)
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

        public new async Task<Modulos> FindAsync(Expression<Func<Modulos, bool>> match)
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

        public new async Task<List<Modulos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Modulos entity)
        {
            try
            {
                var listaModulos = await this.GetAllAsync();
                if (listaModulos.Any(x => x.StrModulo.Trim().ToLower() == entity.StrModulo.Trim().ToLower() && x.IntModuloID != entity.IntModuloID)) return RecursoModulos.msnModuloYaExiste;

                Modulos Modulos = await this.FindAsync(x => x.IntModuloID == entity.IntModuloID);
                Modulos.IntModuloID = entity.IntModuloID;
                Modulos.StrModulo = entity.StrModulo;
                Modulos.StrDescripcion = entity.StrDescripcion;
                Modulos.StrIcono = entity.StrIcono;
                Modulos.TIntOrden = entity.TIntOrden;
                Modulos.BitActivo = entity.BitActivo;

                await base.UpdateAsync(Modulos);

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

        ~ModulosCoreBusiness()
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
