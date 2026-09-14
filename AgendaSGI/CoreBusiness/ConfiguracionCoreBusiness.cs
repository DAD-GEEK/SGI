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
    public class ConfiguracionCoreBusiness : CRUDGenerico<Configuracion>, IConfiguracion
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ConfiguracionCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Configuracion entity)
        {
            try
            {

                entity.StrHoraInicial = entity.StrHoraInicial;
                entity.StrHoraFinal = entity.StrHoraFinal;
                entity.StrVistaAgenda = entity.StrVistaAgenda;
                entity.StrSMPTEmail = entity.StrSMPTEmail;
                entity.StrSMTPPassword = entity.StrSMTPPassword;
                entity.StrSMTPHost = entity.StrSMTPHost;
                entity.StrSMTPPort = entity.StrSMTPPort;
                entity.OpcSMTPSSL = entity.OpcSMTPSSL;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Configuracion entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Configuracion> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Configuracion, bool>> match)
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

        public new async Task<Configuracion> FindAsync(Expression<Func<Configuracion, bool>> match)
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

        public new async Task<List<Configuracion>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Configuracion entity)
        {
            try
            {
                var listaConfiguracion = await this.GetAllAsync();
                Configuracion Configuracion = listaConfiguracion.FirstOrDefault(); 

                Configuracion.StrHoraInicial = entity.StrHoraInicial;
                Configuracion.StrHoraFinal = entity.StrHoraFinal;
                Configuracion.StrVistaAgenda = entity.StrVistaAgenda;
                Configuracion.StrSMPTEmail = entity.StrSMPTEmail;
                Configuracion.StrSMTPPassword = entity.StrSMTPPassword;
                Configuracion.StrSMTPHost = entity.StrSMTPHost;
                Configuracion.StrSMTPPort = entity.StrSMTPPort;
                Configuracion.OpcSMTPSSL = entity.OpcSMTPSSL;

                await base.UpdateAsync(Configuracion);

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

        ~ConfiguracionCoreBusiness()
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
