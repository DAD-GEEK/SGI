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
using System.Reflection;

namespace CoreBusiness
{
    public class LogsExCoreBusiness : CRUDGenerico<LogsEx>, ILogsEx
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private EmailCoreBusiness _emailCoreBusiness;
        private UsuariosCoreBusiness _usuariosCoreBusiness;

        public LogsExCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(LogsEx entity)
        {
            try
            {
                entity.IntUsuarioID = entity.IntUsuarioID;
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

        public new async Task DeleteAsync(LogsEx entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<LogsEx> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<LogsEx, bool>> match)
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

        public new async Task<LogsEx> FindAsync(Expression<Func<LogsEx, bool>> match)
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

        public new async Task<List<LogsEx>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(LogsEx entity)
        {
            try
            {

                LogsEx LogsEx = await this.FindAsync(x => x.IntLogID == entity.IntLogID);

                LogsEx.IntUsuarioID = entity.IntUsuarioID;
                LogsEx.StrMensaje = entity.StrMensaje;
                LogsEx.StrRecurso = entity.StrRecurso;
                LogsEx.DatFecha = entity.DatFecha;

                await base.UpdateAsync(LogsEx);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GenerarLogException(Exception ex, int usuario)
        {
            try
            {
                _emailCoreBusiness = new EmailCoreBusiness();
                _usuariosCoreBusiness = new UsuariosCoreBusiness();

                string MensajeError = "Error message: " + ex.Message;

                if (ex.InnerException != null)
                {
                    MensajeError = MensajeError + " Inner exception: " + ex.InnerException.Message;
                }

                MensajeError = MensajeError + " Stack trace: " + ex.StackTrace;

                string fuente = ex.Source == null ? string.Empty : ex.Source;

                LogsEx modelo = new LogsEx();
                modelo.IntUsuarioID = usuario;
                modelo.StrMensaje = MensajeError;
                modelo.StrRecurso = fuente;
                modelo.DatFecha = DateTime.Now;

                await CreateAsync(modelo);

                string mensaje = RecursoLogsEx.msnGenerarLog + "\nLogID: " + modelo.IntLogID + "\n Mensaje: " + ex.Message;

                var usuarioModelo = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == usuario);

                var Parametros = new Dictionary<string, string>
                    {
                        {"{Cliente}", "JUAN DAVID GONZÁLEZ B." },
                        {"{Fecha}", DateTime.Now.ToString("yyyy-MM-dd hh:mm") },
                        {"{Año}", DateTime.Now.Year.ToString() },
                        {"{Version}", Assembly.GetExecutingAssembly().GetName().Version.ToString() },
                        {"{Mensaje}", MensajeError },
                        {"{Usuario}", usuarioModelo.StrNombre },

                    
                    };

                await _emailCoreBusiness.EnviarEmailAsync("juandmy@hotmail.com", "Notificación de inconsistencia Agenda SGI", _emailCoreBusiness.EmailBody("ErrorNotificacion.html", Parametros), null, true, null, null);

                return mensaje;
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

        ~LogsExCoreBusiness()
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
