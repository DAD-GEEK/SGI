using CoreBusiness;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Filters
{
    public class UserAuthenticationFilter : AuthorizeAttribute
    {
        SeguridadCoreBusiness _seguridadCoreBusiness;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool tienePermiso = false;
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Seguridad/Login");
                return;
            }
            string controlador = filterContext.RouteData.Values["controller"].ToString();
            string accion = filterContext.RouteData.Values["action"].ToString();

            //Identificar controlador actual para la clase active en el menú
            HttpContext.Current.Session["currentController"] = controlador;

            if ((controlador == "SistemasDeGestion" && accion == "Disenio") || controlador == "DocumentosDiagnostico")
                return;

            var userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = filterContext.HttpContext.User;
            string usuarioID = user.Identity.GetUserId();
            var rolesByUsuario = userManager.GetRoles(usuarioID);

            _seguridadCoreBusiness = new SeguridadCoreBusiness();
            var listaModulosPorUsuario = _seguridadCoreBusiness.ObtenerModulosPorUsuario(rolesByUsuario.ToArray());

            tienePermiso = listaModulosPorUsuario.Any(x => x.StrControlador == controlador && x.StrAccion == accion);

            if (!tienePermiso)
            {
                filterContext.Result = new RedirectResult("~/Home/AccesoDenegado");
                return;
            }

        }
    }
}