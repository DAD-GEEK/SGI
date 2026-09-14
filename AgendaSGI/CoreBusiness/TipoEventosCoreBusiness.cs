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
    public class TipoEventosCoreBusiness : CRUDGenerico<TipoEventos>, ITipoEventos
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public TipoEventosCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(TipoEventos entity)
        {
            try
            {
                var listaTipoEventos = await GetAllAsync();

                if (listaTipoEventos.Any(x => x.StrCodigo.ToLower().Equals(entity.StrCodigo.ToLower())))
                {
                    return RecursoTipoEventos.msnValidacionCrear;
                }

                entity.StrCodigo = entity.StrCodigo.Trim();
                entity.StrDescripcion = entity.StrDescripcion.Trim();
                entity.OpcSoporte = entity.OpcSoporte;
                entity.OpcAlerta = entity.OpcAlerta;
                entity.OpcEstado = entity.OpcEstado;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(TipoEventos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<TipoEventos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<TipoEventos, bool>> match)
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

        public new async Task<TipoEventos> FindAsync(Expression<Func<TipoEventos, bool>> match)
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

        public new async Task<List<TipoEventos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(TipoEventos entity)
        {
            try
            {
                var listaTipoEventos = await GetAllAsync();

                if (listaTipoEventos.Any(x => x.StrCodigo.ToLower().Equals(entity.StrCodigo.ToLower()) && x.IntTipoEventoID != entity.IntTipoEventoID))
                {
                    return RecursoTipoEventos.msnValidacionCrear;
                }

                TipoEventos TipoEventos = await this.FindAsync(x => x.IntTipoEventoID == entity.IntTipoEventoID);

                TipoEventos.StrCodigo = entity.StrCodigo.Trim();
                TipoEventos.StrDescripcion = entity.StrDescripcion.Trim();
                TipoEventos.OpcSoporte = entity.OpcSoporte;
                TipoEventos.OpcAlerta = entity.OpcAlerta;
                TipoEventos.OpcEstado = entity.OpcEstado;
               
                await base.UpdateAsync(TipoEventos);

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

        ~TipoEventosCoreBusiness()
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
