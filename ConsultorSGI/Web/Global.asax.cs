using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            string mensaje = exception.Message;

            int error = httpException != null 
                ? httpException.GetHttpCode() 
                : 0;

            Server.ClearError();

            if (error == 404)
            {
                Response.Redirect("~/Home/ModuloEnMantenimiento");
            }
            else
            {
                if (mensaje != null)
                    Response.Redirect($"~/Home/Error?mensaje={mensaje}");

                Response.Redirect($"~/Home/Error?mensaje=");

            }
        }

        protected void Application_EndRequest()
        {
            try
            {
                if (Response.Cookies.Count > 0)
                {
                    for (int i = 0; i < Response.Cookies.Count; i++)
                    {
                        var cookie = Response.Cookies[i];
                        if (cookie != null)
                        {
                            cookie.Secure = true;
                            cookie.SameSite = SameSiteMode.None;
                        }
                    }
                }

                var setCookieHeaders = Response.Headers.GetValues("Set-Cookie");
                if (setCookieHeaders != null && setCookieHeaders.Length > 0)
                {
                    Response.Headers.Remove("Set-Cookie");
                    foreach (var header in setCookieHeaders)
                    {
                        var newHeader = header;
                        if (newHeader.IndexOf("SameSite", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            newHeader += "; SameSite=None";
                        }
                        if (newHeader.IndexOf("Secure", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            newHeader += "; Secure";
                        }
                        Response.Headers.Add("Set-Cookie", newHeader);
                    }
                }
            }
            catch
            {
                // Ignorar excepciones al cerrar la respuesta
            }
        }

    }
}
