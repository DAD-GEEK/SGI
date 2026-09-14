using Across.ArchivosDeRecurso;
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

namespace CoreBusiness
{
    public class Auditorias_NormasCoreBusiness : CRUDGenerico<Auditorias_Normas>, IAuditorias_NormasCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public Auditorias_NormasCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(Auditorias_Normas entity)
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

        public new async Task<string> CreateAsync(Auditorias_Normas entity)
        {
            try
            {
                entity.IntAuditoriaID = entity.IntAuditoriaID;
                entity.IntNormaID = entity.IntNormaID;              

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Auditorias_Normas entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Auditorias_Normas> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Auditorias_Normas, bool>> match)
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

        public new async Task<Auditorias_Normas> FindAsync(Expression<Func<Auditorias_Normas, bool>> match)
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

        public new async Task<List<Auditorias_Normas>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Auditorias_Normas entity)
        {
            try
            {
                Auditorias_Normas Auditorias_Normas = await this.FindAsync(x => x.IntID == entity.IntID);

                Auditorias_Normas.IntAuditoriaID = entity.IntAuditoriaID;
                Auditorias_Normas.IntNormaID = entity.IntNormaID;
               
                await base.UpdateAsync(Auditorias_Normas);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Auditorias_Normas model)
        {
            try
            {
                if (model.IntAuditoriaID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

      

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Auditorias_NormasCoreBusiness()
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
