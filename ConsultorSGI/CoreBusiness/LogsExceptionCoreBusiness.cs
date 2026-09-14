using Across.ArchivosDeRecurso;
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
    public class LogsExceptionCoreBusiness : CRUDGenerico<LogsException>, ILogsExceptionCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        IEmpleadosCoreBusiness _iEmpleadosCoreBusiness;

        public LogsExceptionCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            _iEmpleadosCoreBusiness = new EmpleadosCoreBusiness();
        }

        public new async Task<string> CreateAsync(LogsException entity)
        {
            try
            {
                entity.StrUsuarioEmail = entity.StrUsuarioEmail;
                entity.StrMensaje = entity.StrMensaje;
                entity.StrRecurso = entity.StrRecurso;
                entity.DatFecha = entity.DatFecha;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(LogsException entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<LogsException> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<LogsException, bool>> match)
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

        public new async Task<LogsException> FindAsync(Expression<Func<LogsException, bool>> match)
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

        public new async Task<List<LogsException>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(LogsException entity)
        {
            try
            {

                LogsException LogsException = await this.FindAsync(x => x.IntLogID == entity.IntLogID);

                LogsException.StrUsuarioEmail = entity.StrUsuarioEmail;
                LogsException.StrMensaje = entity.StrMensaje;
                LogsException.StrRecurso = entity.StrRecurso;
                LogsException.DatFecha = entity.DatFecha;

                await base.UpdateAsync(LogsException);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GenerarLogException(Exception ex)
        {
            try
            {

                string MensajeError = "Error message: " + ex.Message;

                if (ex.InnerException != null) MensajeError = MensajeError + " Inner exception: " + ex.Message;

                MensajeError = MensajeError + " Stack trace: " + ex.StackTrace;
                string fuente = ex.Source == null ? string.Empty : ex.Source;

                LogsException modelo = new LogsException();
                modelo.StrUsuarioEmail = System.Web.HttpContext.Current.User.Identity.Name;
                modelo.StrMensaje = MensajeError;
                modelo.StrRecurso = fuente;
                modelo.DatFecha = DateTime.Now;

                await this.CreateAsync(modelo);

                return RecursoLogsException.msnGenerarLogException + "\nLogID: " + modelo.IntLogID + "\n Mensaje: " + ex.Message;
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

        ~LogsExceptionCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iEmpleadosCoreBusiness != null)
                {
                    _iEmpleadosCoreBusiness.Dispose();
                    _iEmpleadosCoreBusiness = null;
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
