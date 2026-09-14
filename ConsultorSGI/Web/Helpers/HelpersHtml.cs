using CoreBusiness;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Web.Helpers
{
    public static class HelpersHtml
    {
        public static string ActiveMenuPrincipal(this HtmlHelper html, string moduloMenu = null, string clase = null)
        {
            string actualController = string.Empty;

            try
            {
                actualController = (string)HttpContext.Current.Session["currentController"];
            }
            catch { }

            if (!string.IsNullOrEmpty(actualController))
            {
                ModulosCoreBusiness _modulosCoreBusiness = new ModulosCoreBusiness();
                ModulosDetalleCoreBusiness _modulosDetalleCoreBusiness = new ModulosDetalleCoreBusiness();
                var moduloDetalleActual = _modulosDetalleCoreBusiness.GetAll().Where(x => x.StrControlador.Trim().ToLower() == actualController.Trim().ToLower()).FirstOrDefault();

                if (moduloDetalleActual != null)
                {
                    var moduloPrincipalActual = _modulosCoreBusiness.GetAll().Where(x => x.IntModuloID == moduloDetalleActual.IntModuloID).FirstOrDefault();
                    if (moduloPrincipalActual.StrModulo == moduloMenu) return clase;
                }
            }

            return string.Empty;
        }

        public static string ActiveClassSubMenu(this HtmlHelper html, string controller = null)
        {
            string activeClass = "active";
            string actualController = string.Empty;

            try
            {
                actualController = (string)HttpContext.Current.Session["currentController"];
            }
            catch { }

            if (!string.IsNullOrEmpty(actualController))
            {
                if (actualController == controller)
                {
                    HttpContext.Current.Session["currentController"] = string.Empty;
                    return activeClass;
                }
            }

            return string.Empty;
        }

        public static string ActiveMenuPpalNewFeature(this HtmlHelper html, string moduloMenu = null, string clase = null)
        {
            ModulosCoreBusiness _modulosCoreBusiness = new ModulosCoreBusiness();
            ModulosDetalleCoreBusiness _modulosDetalleCoreBusiness = new ModulosDetalleCoreBusiness();
            ParametrosCoreBusiness _parametrosCoreBusiness = new ParametrosCoreBusiness();

            var moduloPrincipalActual = _modulosCoreBusiness.GetAll().Where(x => x.StrModulo == moduloMenu).FirstOrDefault();
            var listaModulosDetalle = _modulosDetalleCoreBusiness.GetAll().Where(x => x.IntModuloID == moduloPrincipalActual.IntModuloID);

            if (moduloPrincipalActual != null)
            {
                var parametrosApp = _parametrosCoreBusiness.GetAll().FirstOrDefault();
                if (listaModulosDetalle.Any(x => (DateTime.Now - x.DatFechaCreacion.Value).TotalDays < parametrosApp.TIntDiasModulosNuevos)) return clase;              
            }
         
            return string.Empty;
        }

        public static string ActiveSubMenuNewFeature(this HtmlHelper html, string moduloDetalle = null)
        {
            string newClass = "fas fa-exclamation-circle nav-icon text-primary";

            ParametrosCoreBusiness _parametrosCoreBusiness = new ParametrosCoreBusiness();
            ModulosDetalleCoreBusiness _modulosDetalleCoreBusiness = new ModulosDetalleCoreBusiness();
            var menuDetalle = _modulosDetalleCoreBusiness.GetAll().Where(x => x.StrModuloDetalle == moduloDetalle).FirstOrDefault();

            if (menuDetalle != null)
            {
                var parametrosApp = _parametrosCoreBusiness.GetAll().FirstOrDefault();

                var fechaModulo = menuDetalle.DatFechaCreacion;
                var diferencia = DateTime.Now - fechaModulo;
                if (diferencia.Value.TotalDays < parametrosApp.TIntDiasModulosNuevos) return newClass;
            }

            return "far fa-circle nav-icon";
        }
    }

}
