using Across.Interfaces;
using DataAccess;
using Across.ArchivosDeRecurso;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class ModulosCoreBusiness : CRUDGenerico<Modulos>, IModulos
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ModulosCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Modulos entity)
        {
            try
            {
                var listaModulos = await GetAllAsync();

                if (listaModulos.Any(x => x.StrDescripcion.ToLower().Equals(entity.StrDescripcion.ToLower())))
                {
                    return RecursoModulos.msnValidacionCrear;
                }

                entity.StrDescripcion = entity.StrDescripcion.Trim();
                entity.StrControlador = entity.StrControlador;
                entity.StrAccion = entity.StrAccion;
                entity.OpcEstado = entity.OpcEstado;
               
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
                var listaModulos = await GetAllAsync();

                if (listaModulos.Any(x => x.StrDescripcion.ToLower().Equals(entity.StrDescripcion.ToLower()) && x.IntModuloID != entity.IntModuloID))
                {
                    return RecursoModulos.msnValidacionCrear;
                }

                Modulos Modulos = await this.FindAsync(x => x.IntModuloID == entity.IntModuloID);

                Modulos.StrDescripcion = entity.StrDescripcion.Trim();
                Modulos.StrControlador = entity.StrControlador;
                Modulos.StrAccion = entity.StrAccion;
                Modulos.OpcEstado = entity.OpcEstado;
               
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
