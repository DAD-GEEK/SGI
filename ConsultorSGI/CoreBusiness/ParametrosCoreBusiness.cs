using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class ParametrosCoreBusiness : CRUDGenerico<Parametros>, IParametrosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ParametrosCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> CreateAsync(Parametros entity)
        {
            try
            {
                entity.StrSMPTEmail = entity.StrSMPTEmail;
                entity.StrSMTPPassword = entity.StrSMTPPassword;
                entity.StrSMTPHost = entity.StrSMTPHost;
                entity.StrSMTPPort = entity.StrSMTPPort;
                entity.OpcSMTPSSL = entity.OpcSMTPSSL;
                entity.TIntDiasModulosNuevos = entity.TIntDiasModulosNuevos;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Parametros entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Parametros> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Parametros, bool>> match)
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

        public new async Task<Parametros> FindAsync(Expression<Func<Parametros, bool>> match)
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

        public new async Task<List<Parametros>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Parametros entity)
        {
            try
            {
                var Parametros = await this.FindAsync(x => x.IntID == entity.IntID);

                Parametros.StrSMPTEmail = entity.StrSMPTEmail;
                Parametros.StrSMTPPassword = entity.StrSMTPPassword;
                Parametros.StrSMTPHost = entity.StrSMTPHost;
                Parametros.StrSMTPPort = entity.StrSMTPPort;
                Parametros.OpcSMTPSSL = entity.OpcSMTPSSL;
                Parametros.TIntDiasModulosNuevos = entity.TIntDiasModulosNuevos;

                await base.UpdateAsync(Parametros);

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

        ~ParametrosCoreBusiness()
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
