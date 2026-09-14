using Across.ArchivosDeRecurso;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreBusiness
{

    public class AccountCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private TercerosUsuariosCoreBusiness _tercerosUsuariosCoreBusiness;
        private AspNetUsersCoreBusiness _aspNetUsersCoreBusiness;

        public async Task<string> ValidarInicioSesion(string emailUsuario)
        {
            try
            {                
                _tercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();
                _aspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();

                var modeloUsuario = await _aspNetUsersCoreBusiness.FindAsync(x => x.Email == emailUsuario);

                if (modeloUsuario != null)
                {
                    if (modeloUsuario.LockoutEnabled == true && modeloUsuario.LockoutEndDateUtc > DateTime.Now) return RecursoUsuarios.msnUsuarioBloqueado;

                    var usuarioAsignado = await this.UsuarioAsignadoATercero(emailUsuario);

                    if (usuarioAsignado) return string.Empty;

                    return RecursoUsuarios.msnUsuarioSinAsignar;

                }

                return RecursoUsuarios.msnUsuarioInvalido;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UsuarioAsignadoATercero(string email)
        {
            try
            {
                _tercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();

                var usuario = await _tercerosUsuariosCoreBusiness.FindAsync(x => x.StrUsuarioEmail == email && x.OpcEstado == true);

                if (usuario != null) return true;             

                return false;             

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ExisteUsuario(string email) 
        {
            try
            {
                _aspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();

                var existeUsuario = await _aspNetUsersCoreBusiness.FindAsync(x => x.Email == email);

                if (existeUsuario != null) return RecursoUsuarios.msnUsuarioExiste;

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AccountCoreBusiness()
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
