using Across.ArchivosDeRecurso;
using CoreBusiness;
using Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private Usuarios _usuario;
        private ModulosCoreBusiness _modulosCoreBusiness;
        private UsuariosCoreBusiness _usuariosCoreBusiness;
        private LogsExCoreBusiness _logsExCoreBusiness;

        public async override void OnAuthorization(AuthorizationContext filterContext)
        {
            _modulosCoreBusiness = new ModulosCoreBusiness();
            _usuariosCoreBusiness = new UsuariosCoreBusiness();
            _logsExCoreBusiness = new LogsExCoreBusiness();

            string nombreModulo = string.Empty;
            bool permiso = false;

            try
            {
                _usuario = (Usuarios)HttpContext.Current.Session["Usuario"];
                //var inSession = HttpContext.Current.User.Identity.IsAuthenticated;

                if (_usuario != null)
                {
                //    if (_usuario == null)
                //    {
                //        var userName = HttpContext.Current.User.Identity.Name;
                //        var _usuario = _usuariosCoreBusiness.GetAll().Where(x => x.StrCodigo == userName).FirstOrDefault();
                //    }

                    string controlador = filterContext.RouteData.Values["controller"].ToString();
                    string accion = filterContext.RouteData.Values["action"].ToString();

                    var usuarioActual = _usuariosCoreBusiness.GetAll().Where(x => x.IntUsuarioID == _usuario.IntUsuarioID).FirstOrDefault();
                    var modulosUsuario = usuarioActual.Roles.Roles_Modulos;

                    foreach (var item in modulosUsuario)
                    {
                        var modulo = _modulosCoreBusiness.GetAll().Where(x => x.IntModuloID == item.IntModuloID).FirstOrDefault();
                        if (modulo != null)
                        {
                            if (modulo.StrControlador == controlador && modulo.StrAccion == accion)
                            {
                                permiso = true;
                                break;
                            }
                        }
                    }

                    if (!permiso)
                    {
                        filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?&modulo=" + controlador + "&msjException=" + RecursoError.msnSinAutorizacion);
                    }

                }
               
            }

            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)HttpContext.Current.Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
        }
    }
}