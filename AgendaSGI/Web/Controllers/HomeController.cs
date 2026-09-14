using CoreBusiness;
using Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        LogsExCoreBusiness _logsExCoreBusiness;
        public async Task<ActionResult> Index()
        {
            try
            {
                return View();
            }
            catch (System.Exception ex)
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