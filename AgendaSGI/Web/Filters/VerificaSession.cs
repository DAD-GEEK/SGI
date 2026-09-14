using CoreBusiness;
using Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Controllers;


namespace Web.Filters
{
    public class VerificaSession : ActionFilterAttribute
    {
        private Usuarios _usuario;
        private UsuariosCoreBusiness _usuariosCoreBusiness;
        private SeguridadCoreBusiness _seguridadCoreBusiness;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _seguridadCoreBusiness = new SeguridadCoreBusiness();

                //var isValid = HttpContext.Current.User.Identity.IsAuthenticated;
                _usuario = (Usuarios)HttpContext.Current.Session["Usuario"];
                var ti = FormsAuthentication.Timeout;

                var sessionTimeOut = _seguridadCoreBusiness.IsSessionTimedOut();

                if (_usuario == null)
                {
                    if (filterContext.Controller is AuthController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Auth/Login");
                    }
                    //_usuario = (Usuarios)HttpContext.Current.Session["Usuario"];

                    //if (_usuario == null)
                    //{
                    //    var userName = HttpContext.Current.User.Identity.Name;
                    //    var user = _usuariosCoreBusiness.GetAll().Where(x => x.StrCodigo == userName).FirstOrDefault();

                    //    HttpContext.Current.Session["Usuario"] = user;
                    //}
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}