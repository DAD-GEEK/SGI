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
    public class Terceros_NormasCoreBusiness : CRUDGenerico<Terceros_Normas>, ITerceros_NormasCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        public Terceros_NormasCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(Terceros_Normas entity)
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

        public new async Task<string> CreateAsync(Terceros_Normas entity)
        {
            try
            {
                var existeRegistro = await this.FindWhereAsync(x => x.IntTerceroID == entity.IntTerceroID && x.IntNormaID == entity.IntNormaID);

                if (existeRegistro.Count() != 0)
                    return RecursoTerceros.msnNormaDuplicada;

                entity.IntTerceroID = entity.IntTerceroID;
                entity.IntNormaID = entity.IntNormaID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Terceros_Normas entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Terceros_Normas> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Terceros_Normas, bool>> match)
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

        public new async Task<Terceros_Normas> FindAsync(Expression<Func<Terceros_Normas, bool>> match)
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

        public new async Task<List<Terceros_Normas>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Terceros_Normas entity)
        {
            try
            {
                var Terceros_Normas = await this.FindAsync(x => x.IntID == entity.IntID);

                Terceros_Normas.IntTerceroID = entity.IntTerceroID;
                Terceros_Normas.IntNormaID = entity.IntNormaID;

                await base.UpdateAsync(Terceros_Normas);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Terceros_Normas model)
        {
            try
            {
                if (model.IntID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        public async Task<List<Terceros_Normas>> ObtenerNormasPorTerceroAsync(int terceroID)
        {
            try
            {
                return await this.FindWhereAsync(x => x.IntTerceroID == terceroID);
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

        ~Terceros_NormasCoreBusiness()
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
