using Across.ArchivosDeRecurso;
using Across.CacheStorage;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class Terceros_SistemasDeGestion_NormasCoreBusiness : CRUDGenerico<Terceros_SistemasDeGestion_Normas>, ITerceros_SistemasDeGestion_NormasCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private readonly ICacheStorage _iCacheStorage;

        public Terceros_SistemasDeGestion_NormasCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iCacheStorage = new CacheStorage();
        }

        public new async Task<string> SaveEntityAsync(Terceros_SistemasDeGestion_Normas entity)
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

        public new async Task<string> CreateAsync(Terceros_SistemasDeGestion_Normas entity)
        {
            try
            {
                entity.IntTerceros_SistemasDeGestionID = entity.IntTerceros_SistemasDeGestionID;
                entity.IntNormaID = entity.IntNormaID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Terceros_SistemasDeGestion_Normas entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Terceros_SistemasDeGestion_Normas> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Terceros_SistemasDeGestion_Normas, bool>> match)
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

        public new async Task<Terceros_SistemasDeGestion_Normas> FindAsync(Expression<Func<Terceros_SistemasDeGestion_Normas, bool>> match)
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

        public new List<Terceros_SistemasDeGestion_Normas> GetAll()
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

        public new async Task<List<Terceros_SistemasDeGestion_Normas>> GetAllAsync()
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
        public new async Task<string> UpdateAsync(Terceros_SistemasDeGestion_Normas entity)
        {
            try
            {
                var Terceros_SistemasDeGestion_Normas = await this.FindAsync(x => x.IntRegistroID == entity.IntRegistroID);

                Terceros_SistemasDeGestion_Normas.IntTerceros_SistemasDeGestionID = entity.IntTerceros_SistemasDeGestionID;
                Terceros_SistemasDeGestion_Normas.IntNormaID = entity.IntNormaID;

                await base.UpdateAsync(Terceros_SistemasDeGestion_Normas);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarRegistroAsync(Terceros_SistemasDeGestion_Normas model)
        {
            try
            {
                if (model.IntRegistroID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
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

        ~Terceros_SistemasDeGestion_NormasCoreBusiness()
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
