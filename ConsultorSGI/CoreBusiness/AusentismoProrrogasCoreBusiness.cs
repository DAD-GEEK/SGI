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
    public class AusentismoProrrogasCoreBusiness : CRUDGenerico<AusentismoProrrogas>, IAusentismoProrrogasCoreBusiness
    {
        #region Variables
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness => new CommonCoreBusiness();
        private IAusentismoTiemposCoreBusiness _iAusentismoTiemposCoreBusiness;

        public AusentismoProrrogasCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iAusentismoTiemposCoreBusiness = new AusentismoTiemposCoreBusiness();
        }
        #endregion

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(AusentismoProrrogas entity)
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

        public new async Task<string> CreateAsync(AusentismoProrrogas entity)
        {
            try
            {
                entity.IntAusentismoID = entity.IntAusentismoID;
                entity.DatFechaInicial = entity.DatFechaInicial;
                entity.DatFechaFinal = entity.DatFechaFinal;

                await base.CreateAsync(entity);

                await _iAusentismoTiemposCoreBusiness.GuardarTiempoDeProrrogaPorMesAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AusentismoProrrogas entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AusentismoProrrogas> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AusentismoProrrogas, bool>> match)
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

        public new async Task<AusentismoProrrogas> FindAsync(Expression<Func<AusentismoProrrogas, bool>> match)
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

        public new List<AusentismoProrrogas> GetAll()
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

        public new async Task<List<AusentismoProrrogas>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(AusentismoProrrogas entity)
        {
            try
            {               
                var AusentismoProrrogas = await this.FindAsync(x => x.IntProrrogaID == entity.IntProrrogaID);

                AusentismoProrrogas.IntAusentismoID = entity.IntAusentismoID;
                AusentismoProrrogas.DatFechaInicial = entity.DatFechaInicial;
                AusentismoProrrogas.DatFechaFinal = entity.DatFechaFinal;

                await base.UpdateAsync(AusentismoProrrogas);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(AusentismoProrrogas model)
        {
            try
            {
                if (model.IntProrrogaID == 0)
                    return await this.CreateAsync(model);

                return await this.UpdateAsync(model);
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

        ~AusentismoProrrogasCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (_iCommonCoreBusiness != null)
                //    _iCommonCoreBusiness.Dispose();

                if (_iAusentismoTiemposCoreBusiness != null)
                {
                    _iAusentismoTiemposCoreBusiness.Dispose();
                    _iAusentismoTiemposCoreBusiness = null;
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
