using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
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
    public class ConfiguracionPorTerceroCoreBusiness : CRUDGenerico<ConfiguracionPorTercero>, IConfiguracionPorTerceroCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness;
        private ITercerosCoreBusiness _iTercerosCoreBusiness;

        public ConfiguracionPorTerceroCoreBusiness() : base(new gestioni_consultorNetEntities(new ServicioTercero()))
        {
            this._iCommonCoreBusiness = new CommonCoreBusiness();
            this._iTercerosCoreBusiness = new TercerosCoreBusiness();
        }

        public new async Task<string> SaveEntityAsync(ConfiguracionPorTercero entity)
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

        public new async Task<string> CreateAsync(ConfiguracionPorTercero entity)
        {
            try
            {
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();
                var configuracionTercero = await this.FindAsync(x => x.IntTerceroID == terceroID);

                if (configuracionTercero != null)
                    return RecursoConfiguracionPorTercero.msnConfiguracionExistente;

                entity.IntTerceroID = entity.IntTerceroID;
                entity.SIntAusentismoBajo = entity.SIntAusentismoBajo;
                entity.SIntAusentismoMedio = entity.SIntAusentismoMedio;
                entity.SIntAusentismoAlto = entity.SIntAusentismoAlto;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(ConfiguracionPorTercero entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<ConfiguracionPorTercero> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<ConfiguracionPorTercero, bool>> match)
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

        public new async Task<ConfiguracionPorTercero> FindAsync(Expression<Func<ConfiguracionPorTercero, bool>> match)
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

        public new List<ConfiguracionPorTercero> GetAll()
        {
            try
            {
                var terceroID = _iCommonCoreBusiness.GetTerceroIDFromCurrentUser();
                return base.GetAll().Where(x => x.IntTerceroID == terceroID).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<ConfiguracionPorTercero>> GetAllAsync()
        {
            try
            {
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();
                var listaEntidad = await base.GetAllAsync();
                return listaEntidad.Where(x => x.IntTerceroID == terceroID).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(ConfiguracionPorTercero entity)
        {
            try
            {
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();
                var configuracionTercero = await this.FindAsync(x => x.IntTerceroID == terceroID && x.IntConfiguracionID != entity.IntConfiguracionID);

                if (configuracionTercero != null)
                    return RecursoConfiguracionPorTercero.msnConfiguracionExistente;

                var ConfiguracionPorTercero = await this.FindAsync(x => x.IntConfiguracionID == entity.IntConfiguracionID && x.IntTerceroID == terceroID);

                ConfiguracionPorTercero.IntTerceroID = entity.IntTerceroID;
                ConfiguracionPorTercero.SIntAusentismoBajo = entity.SIntAusentismoBajo;
                ConfiguracionPorTercero.SIntAusentismoMedio = entity.SIntAusentismoMedio;
                ConfiguracionPorTercero.SIntAusentismoAlto = entity.SIntAusentismoAlto;

                await base.UpdateAsync(ConfiguracionPorTercero);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(ConfiguracionPorTercero model)
        {
            try
            {
                if (model.IntConfiguracionID != 0) return await this.UpdateAsync(model);
                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Terceros> ObtenerInformacionDeTerceroEnSesionAsync()
        {
            try
            {
                return await _iTercerosCoreBusiness.ObtenerInformacionDeTerceroEnSesionAsync();
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

        ~ConfiguracionPorTerceroCoreBusiness()
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
                    _iCommonCoreBusiness = null;
                }

                if (_iTercerosCoreBusiness != null)
                {
                    _iTercerosCoreBusiness.Dispose();
                    _iTercerosCoreBusiness = null;
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
