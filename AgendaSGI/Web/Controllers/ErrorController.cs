using CoreBusiness;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        LogsExCoreBusiness _logsExCoreBusiness;

        public async Task<ActionResult> UnauthorizedOperation(string modulo, string msjException)
        {
            try
            {
                ViewBag.Modulo = modulo;
                ViewBag.MsjExcepcion = msjException;

                return View();
            }
            catch (Exception ex)
            {

                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return View();

            }
        }


    }
}