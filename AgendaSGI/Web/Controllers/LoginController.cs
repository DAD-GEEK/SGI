using CoreBusiness;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        UsuariosCoreBusiness _usuariosCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;


        public ActionResult Login()
        {
            try
            {

                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ActionResult> Login(string usuario, string contraseña)
        {

            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();

                var usuarioBD = await _usuariosCoreBusiness.FindAsync(x => (x.StrCodigo == usuario || x.StrEmail == usuario) && x.StrClave == contraseña);

                if (usuarioBD == null)
                    return View();

                Session["usuario"] = usuarioBD;

                return RedirectToAction("Index", "Home");
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