using Across.ArchivosDeRecurso;
using CoreBusiness;
using Models;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        UsuariosCoreBusiness _usuariosCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;
        SeguridadCoreBusiness _seguridadCoreBusiness;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string error = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.Error = error;
                }
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SSO(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) return RedirectToAction("Login");

                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                var usuarioBD = await _usuariosCoreBusiness.FindAsync(x => (x.StrEmail.ToLower() == email.ToLower() || x.StrCodigo.ToLower() == email.ToLower()) && x.OpcEstado == true);
                if (usuarioBD == null)
                {
                    return RedirectToAction("Login", new { error = "Usuario no sincronizado en Agenda SGI. Ingrese con sus credenciales del aplicativo o contacte al Administrador." });
                }

                this.AgregarUsuarioASession(usuarioBD.StrCodigo);
                Session["Usuario"] = usuarioBD;
                Session["UsuarioNombre"] = usuarioBD.StrNombre;

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Login", new { error = "No se pudo iniciar sesión automáticamente. Ingrese sus credenciales o contacte al Administrador." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string usuarioLogin, string contraseñaLogin)
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _seguridadCoreBusiness = new SeguridadCoreBusiness();

                var usuarioBD = await _usuariosCoreBusiness.FindAsync(x => (x.StrCodigo == usuarioLogin || x.StrEmail == usuarioLogin));

                if (usuarioBD == null)
                    return Json(new { login = false, error = RecursoLogin.msnAutenticacion });

                if (usuarioBD != null)
                {
                    if (usuarioBD.OpcEstado != true)
                        return Json(new { login = false, error = RecursoLogin.msnUsuarioInactivo });

                    var claveUsuario = _seguridadCoreBusiness.cifrarCadena(contraseñaLogin);

                    if (usuarioBD.StrClave != claveUsuario)
                        return Json(new { login = false, error = "Contraseña incorrecta. Si modificó su clave recientemente en el CRM, contacte al Administrador para sincronizar o ingrese su clave anterior." });
                }

                this.AgregarUsuarioASession(usuarioLogin);

                Session["Usuario"] = usuarioBD;
                Session["UsuarioNombre"] = usuarioBD.StrNombre;

                return Json(new { login = true });
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

        public async void AgregarUsuarioASession(string id)
        {
            try
            {
                bool persist = true;
                var cookie = FormsAuthentication.GetAuthCookie(id, persist);

                cookie.Name = FormsAuthentication.FormsCookieName;
                cookie.Expires = DateTime.Now.AddDays(1);

                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, id);

                cookie.Value = FormsAuthentication.Encrypt(newTicket);
                HttpContext.Response.Cookies.Add(cookie);
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
            }
        }

        public async Task<ActionResult> CerrarSesion()
        {
            try
            {
                Session["Usuario"] = null;
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Auth");
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> VerificarSessionFrontEnd()
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();

                //Usuarios usuarioSession = (Usuarios)HttpContext.Session["Usuario"];
                Usuarios usuarioSession = null;

                if (usuarioSession != null)
                    return Json(new { msn = true });

                HttpCookie authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    string userId = ticket.UserData; // Aquí puedes obtener el ID del usuario

                    if (!string.IsNullOrEmpty(userId))
                    {
                        usuarioSession = await _usuariosCoreBusiness.FindAsync(x => (x.StrCodigo == userId || x.StrEmail == userId));

                        if (usuarioSession != null)
                        {
                            Session["usuario"] = usuarioSession;
                            return Json(new { msn = true });
                        }
                    }
                }

                return Json(new { msn = false }); ;
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
                await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { msn = false }); ;

            }

        }
    }
}