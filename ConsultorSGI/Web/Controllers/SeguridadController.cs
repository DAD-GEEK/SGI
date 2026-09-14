using Across.ArchivosDeRecurso;
using CoreBusiness;
using CoreBusiness.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Filters;
using Web.Models;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class SeguridadController : Controller
    {
        #region Variables
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private IAspNetUsersCoreBusiness _iAspNetUsersCoreBusiness;
        private ITercerosUsuariosCoreBusiness _iTercerosUsuariosCoreBusiness;
        private ISeguridadCoreBusiness _iSeguridadCoreBusiness;
        private IEmailCoreBusiness _iEmailCoreBusiness;
        private IAspNetRolesCoreBusiness _iAspNetRolesCoreBusiness;
        private ITercerosCoreBusiness _iTercerosCoreBusiness;
        private IAspNetTerceroRolesCoreBusiness _iAspNetTerceroRolesCoreBusiness;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public SeguridadController()
        {
            this._iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
            this._iAspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();
            this._iTercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();
            this._iSeguridadCoreBusiness = new SeguridadCoreBusiness();
            this._iEmailCoreBusiness = new EmailCoreBusiness();
            this._iAspNetRolesCoreBusiness = new AspNetRolesCoreBusiness();
            this._iTercerosCoreBusiness = new TercerosCoreBusiness();
            this._iAspNetTerceroRolesCoreBusiness = new AspNetTerceroRolesCoreBusiness();
        }

        public SeguridadController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        #endregion

        #region Login
        [AllowAnonymous]
        public ActionResult Login(string error = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.error = error;
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SSO(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return RedirectToAction("Login");
                }

                var currentUser = await UserManager.FindByNameAsync(email);
                if (currentUser == null)
                {
                    currentUser = await UserManager.FindByEmailAsync(email);
                }

                if (currentUser != null)
                {
                    if (!currentUser.EmailConfirmed)
                    {
                        currentUser.EmailConfirmed = true;
                        await UserManager.UpdateAsync(currentUser);
                    }

                    var identity = await UserManager.CreateIdentityAsync(currentUser, DefaultAuthenticationTypes.ApplicationCookie);
                    identity = await ApplicationUser.CreateUserClaims(
                        identity,
                        UserManager,
                        currentUser);

                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Login", new { error = "Usuario no sincronizado en Consultor SGI. Ingrese con sus credenciales del aplicativo o contacte al Administrador." });
            }
            catch (Exception ex)
            {
                await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return RedirectToAction("Login", new { error = "No se pudo iniciar sesión automáticamente. Ingrese sus credenciales o contacte al Administrador." });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                string mensaje = string.Empty;

                var currentUser = await UserManager.FindByNameAsync(model.Email);

                if (currentUser != null)
                {
                    if (User.Identity.IsAuthenticated)
                        return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);

                    if (!await UserManager.IsEmailConfirmedAsync(currentUser.Id))
                    {
                        this.SendEmailConfirmationTokenAsync(currentUser);
                        return Json(new { error = RecursoSeguridad.msnConfirmarCuentaDeCorreo }, JsonRequestBehavior.AllowGet);
                    }

                    var validarDatosDeAcceso = await _iSeguridadCoreBusiness.ValidarInicioSesion(model.Email);

                    if (!string.IsNullOrEmpty(validarDatosDeAcceso))
                        return Json(new { error = validarDatosDeAcceso }, JsonRequestBehavior.AllowGet);


                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:

                            var terceroUsuario = await _iSeguridadCoreBusiness.GetTerceroUsuarioByEmail(model.Email);
                            //await AsignarPermisosUsuarioSegunPermisosTerceroAsync(terceroUsuario);

                            var identity = await UserManager.CreateIdentityAsync(currentUser, DefaultAuthenticationTypes.ApplicationCookie);

                            identity = await ApplicationUser.CreateUserClaims(
                              identity,
                              UserManager,
                              currentUser);

                            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

                            return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                        case SignInStatus.LockedOut:
                            return Json(new { error = RecursoSeguridad.msnUsuarioBloqueado }, JsonRequestBehavior.AllowGet);
                        case SignInStatus.Failure:
                            return Json(new { error = "Contraseña incorrecta. Si modificó su clave recientemente en el CRM, contacte al Administrador para sincronizar o ingrese su clave anterior." }, JsonRequestBehavior.AllowGet);
                        default:
                            return Json(new { error = RecursoSeguridad.msnCredencialesNoValidas }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { error = RecursoSeguridad.msnCredencialesNoValidas }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var mensaje = _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);

            }
        }

        [AllowAnonymous]
        private async void SendEmailConfirmationTokenAsync(ApplicationUser user)
        {
            try
            {
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmarCorreoElectronico", "Seguridad", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var Parametros = new Dictionary<string, string>
                {
                    {"{usuario}", user.NombreUsuario },
                    {"{url}", callbackUrl }
                };

                await _iEmailCoreBusiness.EnviarEmailAsync(user.Email, "Confirmar correo electrónico - Consultor SGI", _iEmailCoreBusiness.EmailBody("ConfirmarCorreoElectronico.html", Parametros), null, true, null);
            }
            catch (Exception ex)
            {
                await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
            }

        }

        [AllowAnonymous]
        public ActionResult RecuperarContraseña() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecuperarContraseñaAsync(ForgotPasswordViewModel model)
        {
            try
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = Url.Action("CambiarContraseña", "Seguridad", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var Parametros = new Dictionary<string, string>
                    {
                        {"{usuario}", user.NombreUsuario },
                        {"{url}", callbackUrl }
                    };

                await _iEmailCoreBusiness.EnviarEmailAsync(model.Email, "Recuperar contraseña - Consultor SGI", _iEmailCoreBusiness.EmailBody("RecuperarContraseña.html", Parametros), null, true, null);

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var mensaje = _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult CambiarContraseña(string code)
        {
            try
            {
                var parametros = HttpContext.Request.Params;
                var userID = parametros[0];
                var usuario = UserManager.FindById(userID);

                return code == null ? View("Error") : View(new ResetPasswordViewModel()
                {
                    Email = usuario.Email,
                    Code = code
                });
            }
            catch (Exception ex)
            {
                ViewBag.error = _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return View("Error");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CambiarContraseñaAsync(ResetPasswordViewModel model)
        {
            try
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null) return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);

                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded) return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);

                return Json(new { error = RecursoSeguridad.msnErrorCambioContraseña }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var mensaje = _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CerrarSesion()
        {
            try
            {
                Session.Contents.RemoveAll();
                Session.Abandon();
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                Response.AppendHeader("Cache-Control", "no-store");

                var usuarioEnSesion = HttpContext.User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(usuarioEnSesion);

                var userIdentity = User.Identity as ClaimsIdentity;

                var userClaims = new ClaimsPrincipal(userIdentity);
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                var result = await UserManager.RemoveClaimAsync(user.Id, userIdentity.FindFirst(x => x.Type == user.Email));

                TrackingOnlineUsers.EliminarUsuarioConectado(User.Identity.Name);

                return RedirectToAction("Login", "Seguridad");

            }
            catch (Exception ex)
            {

                return RedirectToAction("Account", "Error");
            }

        }

        [AllowAnonymous]
        public ActionResult Registro() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegistroAsync(RegisterViewModel model)
        {
            try
            {
                var usuarioAsignadoAtercero = await _iSeguridadCoreBusiness.UsuarioAsignadoATercero(model.Email);
                if (usuarioAsignadoAtercero)
                {
                    var existeUsuario = UserManager.FindByEmail(model.Email);

                    if (existeUsuario == null)
                    {
                        var terceroUsuario = await _iSeguridadCoreBusiness.GetTerceroUsuarioByEmail(model.Email);

                        var user = new ApplicationUser
                        {
                            UserName = model.Email.ToLower(),
                            Email = model.Email.ToLower(),
                            NombreUsuario = model.Name.ToUpper(),
                            PhoneNumber = model.PhoneNumber,
                            TerceroID = (int)terceroUsuario.IntTerceroID,
                            FechaIngreso = DateTime.Now
                        };

                        var result = await UserManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action("ConfirmarCorreoElectronico", "Seguridad", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                            this.SendEmailConfirmationTokenAsync(user);

                            await this.AsignarPermisosUsuarioSegunPermisosTerceroAsync(terceroUsuario);

                            return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else return Json(new { error = RecursoSeguridad.msnUsuarioYaEstaRegistrado }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = RecursoUsuarios.msnUsuarioSinAsignar }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var mensaje = _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult UsuariosOnline() => View();

        private async Task<string> GenerarClaimsDeUsuarioAsync(ApplicationUser user)
        {
            try
            {
                string respuesta = string.Empty;
                bool addClaim = true;

                var claimsDatosUsuario = await UserManager.GetClaimsAsync(userId: user.Id);
                claimsDatosUsuario = claimsDatosUsuario.Where(x => x.Type == ClaimTypes.UserData).ToList();

                if (claimsDatosUsuario.Count() != 0)
                {
                    var datosDeUsuarioClaims = claimsDatosUsuario.FirstOrDefault().Value;

                    await UserManager.RemoveClaimAsync(user.Id, new Claim(ClaimTypes.UserData, datosDeUsuarioClaims));


                    var usuarioActual = JsonConvert.DeserializeObject<ApplicationUser>(datosDeUsuarioClaims);

                    if (usuarioActual.TerceroID != user.TerceroID)
                    {
                        await UserManager.RemoveClaimAsync(user.Id, new Claim(ClaimTypes.UserData, datosDeUsuarioClaims));
                        addClaim = true;
                    }
                    else
                        addClaim = false;
                }

                if (addClaim)
                {
                    var datosUsuarioJson = JsonConvert.SerializeObject(user);
                    await UserManager.AddClaimAsync(user.Id, new Claim(ClaimTypes.UserData, datosUsuarioJson));
                }

                return respuesta;

            }
            catch (Exception)
            {

                throw;
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmarCorreoElectronico(string userId, string code)
        {
            try
            {
                if (userId == null || code == null) return View("Error");

                var user = await UserManager.FindByIdAsync(userId);

                if (user is null)
                {
                    ViewBag.Error = "El token está asociado a un usuario que ya no existe en la base de datos.";
                    return View("ConfirmarCorreoElectronico");
                }

                var result = await UserManager.ConfirmEmailAsync(userId, code);

                if (!result.Succeeded)
                    ViewBag.Error = $"Ocurrió un problema con el token de confirmación del correo. Vuelva a realizar nuevamente el proceso de confirmación.";

                return View("ConfirmarCorreoElectronico");

            }
            catch (Exception ex)
            {
                ViewBag.Error = _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return View("ConfirmarCorreoElectronico");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginExterno(string provider, string returnUrl)
        {
            try
            {
                return new ChallengeResult(provider, Url.Action("LoginExternoCallback", "Seguridad", new { ReturnUrl = returnUrl }));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> LoginExternoCallback(string returnUrl)
        {
            try
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (loginInfo == null) return RedirectToAction("Login");

                var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        ViewBag.error = RecursoUsuarios.msnUsuarioBloqueado;
                        return View("Login");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                    case SignInStatus.Failure:
                        ViewBag.error = RecursoSeguridad.msnLogeoFallido;
                        return View("Login");
                    default:
                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        return View("ConfirmacionLoginExterno", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return View("Error");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmacionLoginExterno(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            try
            {
                if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

                var validaciones = await _iSeguridadCoreBusiness.ValidacionesConfirmacionLoginExterno(model.Email);
                if (string.IsNullOrEmpty(validaciones))
                {
                    var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (info == null) return Json(new { error = RecursoUsuarios.msnErrorAutenticacionExterna }, JsonRequestBehavior.AllowGet);

                    var terceroUsuario = await _iSeguridadCoreBusiness.ObtenerUsuarioTerceroAsync(model.Email);

                    var user = new ApplicationUser
                    {
                        UserName = model.Email.ToLower(),
                        Email = model.Email.ToLower(),
                        TerceroID = (int)terceroUsuario.IntTerceroID,
                        NombreUsuario = terceroUsuario.StrUsuarioNombre.ToUpper(),
                        FechaIngreso = DateTime.Now
                    };

                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await this.AsignarPermisosUsuarioSegunPermisosTerceroAsync(terceroUsuario);

                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                            {
                                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                                await UserManager.ConfirmEmailAsync(user.Id, code);
                            }

                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                ViewBag.ReturnUrl = returnUrl;
                return Json(new { error = validaciones }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Módulo Seguridad
        [Authorize(Roles = "Administrador")]
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetModuloSeguridad()
        {
            try
            {
                return PartialView("_ModuloSeguridad");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ModuloSeguridad", new Terceros());
            }
        }

        public async Task<ActionResult> GetAllUsuariosTerceros()
        {
            try
            {
                var modelo = await _iSeguridadCoreBusiness.ObtenerListaUsuariosByTercerosDTO();
                return PartialView("_GetAllUsuariosTerceros", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllUsuariosTerceros", new List<TercerosUsuariosDTO>());
            }
        }

        public async Task<ActionResult> GetAllRoles()
        {
            try
            {
                return PartialView("_GetAllRoles");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllRoles", new Terceros());
            }
        }

        public async Task<ActionResult> GetAllModulos()
        {
            try
            {
                return PartialView("_GetAllModulos");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllModulos", new Terceros());
            }
        }

        public async Task<ActionResult> GetAllModulosDetalle()
        {
            try
            {
                return PartialView("_GetAllModulosDetalle");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllModulosDetalle", new Terceros());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAllUsuariosAsync(int terceroID)
        {
            try
            {
                _iTercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();

                var listaUsuarios = await _iTercerosUsuariosCoreBusiness.GetAllAsync();
                listaUsuarios = listaUsuarios.Where(x => x.IntTerceroID == terceroID).ToList();

                return PartialView("_GetAllUsuarios", listaUsuarios);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllUsuarios", new AspNetUsers());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearUsuario()
        {
            try
            {
                var terceros = (from c in await _iTercerosCoreBusiness.FindWhereAsync(x => x.OpcEstado == true && x.StrIdentificacion != "0")
                                orderby c.StrIdentificacion
                                select new { Codigo = c.IntTerceroID, Descripcion = c.StrIdentificacion + " - " + c.StrNombre });
                ViewBag.listaTerceros = new SelectList(terceros, "Codigo", "Descripcion");

                return PartialView("_CrearUsuario");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearUsuario");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearUsuarioAsync(TercerosUsuarios modelo)
        {
            try
            {
                var respuesta = await _iTercerosUsuariosCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                {
                    var user = await UserManager.FindByEmailAsync(modelo.StrUsuarioEmail);

                    if (user != null)
                    {
                        if (user.TerceroID != modelo.IntTerceroID)
                        {
                            user.TerceroID = (int)modelo.IntTerceroID;
                            user.NombreUsuario = modelo.StrUsuarioNombre;
                            await UserManager.UpdateAsync(user);
                        }

                        await AsignarPermisosUsuarioSegunPermisosTerceroAsync(modelo);
                    }

                    return Json(new { msn = ResponseType.success.ToString(), registroID = modelo.IntRegistroID }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetUsuarioByEditarAsync(int registroID)
        {
            try
            {
                var usuarioModelo = await _iTercerosUsuariosCoreBusiness.FindAsync(x => x.IntRegistroID == registroID);

                var aspNetUser = await _iAspNetUsersCoreBusiness.FindAsync(x => x.Email == usuarioModelo.StrUsuarioEmail);
                bool usuarioRegistrado = aspNetUser != null ? true : false;

                if (usuarioRegistrado)
                {
                    var listaRolesPorUsuario = await UserManager.GetRolesAsync(aspNetUser.Id);
                    var roles = (from r in await _iAspNetRolesCoreBusiness.GetAllAsync()
                                 orderby r.Name
                                 select new { Codigo = r.Name, Descripcion = r.Name });
                    ViewBag.listaRoles = new MultiSelectList(roles, "Codigo", "Descripcion", listaRolesPorUsuario);
                }

                ViewBag.registrado = usuarioRegistrado;
                ViewBag.esGestionIntegral = await _iSeguridadCoreBusiness.ValidarSiEsGestionIntegralAsync((int)usuarioModelo.IntTerceroID);

                var terceros = (from c in await _iTercerosCoreBusiness.FindWhereAsync(x => x.OpcEstado == true && x.StrIdentificacion != "0")
                                orderby c.StrIdentificacion
                                select new { Codigo = c.IntTerceroID, Descripcion = c.StrIdentificacion + " - " + c.StrNombre });
                ViewBag.listaTerceros = new SelectList(terceros, "Codigo", "Descripcion", usuarioModelo.IntTerceroID);

                return PartialView("_EditarUsuario", usuarioModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarTercero", new Terceros());
            }
        }

        private async Task<string> AsignarPermisosUsuarioSegunPermisosTerceroAsync(TercerosUsuarios modelo)
        {
            try
            {
                var usuario = await UserManager.FindByNameAsync(modelo.StrUsuarioEmail);

                if (usuario != null)
                {
                    var listaRolesUsuario = await UserManager.GetRolesAsync(usuario.Id);

                    foreach (var item in listaRolesUsuario)
                        await UserManager.RemoveFromRoleAsync(usuario.Id, item);

                    if (usuario is null)
                        return String.Format(RecursoUsuarios.msnUsuarioNoEncontrado, modelo.StrUsuarioEmail);

                    var listaRolesPorTercero = await _iAspNetTerceroRolesCoreBusiness.FindWhereAsync(x => x.IntTerceroID == modelo.IntTerceroID);
                    var listaRoles = await _iAspNetRolesCoreBusiness.GetAllAsync();

                    bool esGestionIntegral = await _iSeguridadCoreBusiness.ValidarSiEsGestionIntegralAsync((int)modelo.IntTerceroID);

                    foreach (var item in listaRolesPorTercero)
                    {
                        var modeloRol = listaRoles.Find(x => x.Id == item.IntRolID);

                        if (modeloRol != null)
                            this.AsignarRolAUsuario(modeloRol.Name, usuario.Id, esGestionIntegral);
                    }
                }

                return String.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool AsignarRolAUsuario(string nombreRol, string usuarioID, bool esGestionIntegral)
        {
            try
            {
                if ((nombreRol == enumRolesGestionIntegral.Administrador.ToString() || nombreRol == enumRolesGestionIntegral.Asesor.ToString()) && !esGestionIntegral)
                    return false;

                var rolesPorUsuario = UserManager.GetRoles(usuarioID);

                if (!rolesPorUsuario.Any(x => x == nombreRol))
                    UserManager.AddToRole(usuarioID, nombreRol);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarRolPorTerceroAsync(AspNetTerceroRoles modelo)
        {
            try
            {
                var respuesta = await _iTercerosCoreBusiness.AgregarRolPorTerceroAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                {
                    var listaUsuariosPorTercero = await _iTercerosUsuariosCoreBusiness.FindWhereAsync(x => x.IntTerceroID == modelo.IntTerceroID);

                    if (listaUsuariosPorTercero.Count() != 0)
                    {
                        var informacionDeRol = await _iAspNetRolesCoreBusiness.FindAsync(x => x.Id == modelo.IntRolID);

                        if (informacionDeRol != null)
                        {
                            foreach (var item in listaUsuariosPorTercero)
                            {
                                var usuario = UserManager.FindByEmail(item.StrUsuarioEmail);
                                var listaRolesPorUsuario = UserManager.GetRoles(usuario.Id);

                                if (!listaRolesPorUsuario.Any(x => x == informacionDeRol.Name))
                                    UserManager.AddToRole(usuario.Id, informacionDeRol.Name);
                            }
                        }
                    }

                    return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EliminarRolPorTerceroAsync(int registroID)
        {
            try
            {
                string respuesta = string.Empty;

                var rolPorTercero = await _iAspNetTerceroRolesCoreBusiness.FindAsync(x => x.IntAspNetTerceroRolesID == registroID);

                if (rolPorTercero != null)
                {
                    var terceroID = rolPorTercero.IntTerceroID;
                    var rolID = rolPorTercero.IntRolID;

                    await _iAspNetTerceroRolesCoreBusiness.DeleteAsync(rolPorTercero);

                    var listaUsuariosPorTercero = await _iTercerosUsuariosCoreBusiness.FindWhereAsync(x => x.IntTerceroID == terceroID);

                    if (listaUsuariosPorTercero.Count() != 0)
                    {
                        var informacionDeRol = await _iAspNetRolesCoreBusiness.FindAsync(x => x.Id == rolID);

                        if (informacionDeRol != null)
                        {
                            foreach (var item in listaUsuariosPorTercero)
                            {
                                var usuario = UserManager.FindByEmail(item.StrUsuarioEmail);
                                var listaRolesPorUsuario = UserManager.GetRoles(usuario.Id);

                                if (listaRolesPorUsuario.Any(x => x == informacionDeRol.Name))
                                    UserManager.RemoveFromRole(usuario.Id, informacionDeRol.Name);
                            }
                        }
                    }

                    return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                }
                else
                    respuesta = RecursoAspNetRoles.msnRolNoExiste;

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUsuarioAsync(TercerosUsuarios modelo, string roles)
        {
            try
            {
                var respuesta = await _iTercerosUsuariosCoreBusiness.SaveAllAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                {
                    if (roles != null)
                    {
                        var user = await UserManager.FindByNameAsync(modelo.StrUsuarioEmail);
                        var listaRoles = JsonConvert.DeserializeObject<List<string>>(roles);
                        var listaRolesUsuario = await UserManager.GetRolesAsync(user.Id);
                        bool esGestionIntegral = await _iSeguridadCoreBusiness.ValidarSiEsGestionIntegralAsync((int)modelo.IntTerceroID);

                        List<string> listaRolesParaEliminar = listaRolesUsuario.Where(x => !listaRoles.Any(y => x == y)).ToList();
                        List<string> listaRolesParaCrear = listaRoles.Where(x => !listaRolesUsuario.Any(y => x == y)).ToList();

                        foreach (var item in listaRolesParaEliminar)
                            await UserManager.RemoveFromRoleAsync(user.Id, item);

                        //Asignar roles de diferenciales
                        var terceroEsDiferente = false;
                        var listaRolesEnBD = await _iAspNetRolesCoreBusiness.GetAllAsync();

                        foreach (var item in listaRolesParaCrear)
                        {
                            var aspNetRoles = listaRolesEnBD.Find(x => x.Name == item);

                            if (aspNetRoles != null)
                            {
                                this.AsignarRolAUsuario(item, user.Id, esGestionIntegral);

                                //Volver a agregar perfiles de admin o asesor en caso de que ya lo tuviese
                                if (aspNetRoles.BitDefault != true && modelo.IntTerceroID != user.TerceroID)
                                    terceroEsDiferente = true;
                            }
                        }

                        if (terceroEsDiferente)
                        {
                            user.TerceroID = (int)modelo.IntTerceroID;
                            await UserManager.UpdateAsync(user);
                        }

                        if ((!esGestionIntegral && listaRoles.Count() > 0) || esGestionIntegral && listaRoles.Count() == 0)
                            await this.AsignarPermisosUsuarioSegunPermisosTerceroAsync(modelo);

                    }

                    return Json(new { msn = ResponseType.success.ToString(), registroID = modelo.IntRegistroID }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUsuarioAsync(int registroID)
        {
            try
            {
                _iTercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();

                var modelo = await _iTercerosUsuariosCoreBusiness.FindAsync(x => x.IntRegistroID == registroID);

                if (modelo != null)
                {
                    await _iTercerosUsuariosCoreBusiness.DeleteAsync(modelo);

                    var user = await UserManager.FindByEmailAsync(modelo.StrUsuarioEmail);

                    if (user != null)
                    {
                        var rolesPorUsuario = await UserManager.GetRolesAsync(user.Id);

                        foreach (var item in rolesPorUsuario)
                            await UserManager.RemoveFromRolesAsync(user.Id, item);

                        user.EmailConfirmed = false;
                        await UserManager.UpdateAsync(user);
                    }

                    return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = RecursoSeguridad.msnUsuarioNoExiste }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> AsignarRolesPorDefectoPorTerceros(int terceroID)
        {
            try
            {
                var listaRolesPorTercero = await _iAspNetTerceroRolesCoreBusiness.GetAllAsync();
                listaRolesPorTercero = listaRolesPorTercero.Where(x => x.IntTerceroID == terceroID).ToList();

                var listaUsuariosPorTercero = await _iAspNetUsersCoreBusiness.GetAllAsync();
                listaUsuariosPorTercero = listaUsuariosPorTercero.Where(x => x.TerceroID == terceroID).ToList();

                foreach (var item in listaUsuariosPorTercero)
                {
                    var listaRolesUsuario = await UserManager.GetRolesAsync(item.Id);
                    var listaRolesDefault = await _iAspNetRolesCoreBusiness.GetAllAsync();
                    listaRolesDefault = listaRolesDefault.Where(x => x.BitDefault == true).ToList();

                    foreach (var rol in listaRolesUsuario)
                    {
                        if (listaRolesDefault.Any(x => x.Name == rol))
                            await UserManager.RemoveFromRoleAsync(item.Id, rol);
                    }

                    foreach (var roles in listaRolesPorTercero)
                        await UserManager.AddToRoleAsync(item.Id, roles.AspNetRoles.Name);
                }

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Permisos
        public async Task<JsonResult> GetTreePermisosByRoles(string rolID)
        {
            try
            {
                var data = await _iSeguridadCoreBusiness.GetTreePermisosByRoles(rolID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                var data = "[]";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarPermisosByRolAsync(List<string> listaPermisos, string rolID)
        {
            try
            {
                string respuesta = await _iSeguridadCoreBusiness.GuardarPermisosPorRolAsync(listaPermisos, rolID);
                if (string.IsNullOrEmpty(respuesta)) return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Menú dinámico
        [HttpPost]
        public async Task<ActionResult> GetMenuDinamicoAsync()
        {
            try
            {
                string usuarioID = User.Identity.GetUserId();
                IList<string> rolesPorUsuario = await UserManager.GetRolesAsync(usuarioID);

                var modelo = await _iSeguridadCoreBusiness.GetMenuDinamicoAsync(usuarioID, rolesPorUsuario);

                return PartialView("_Menu", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_MenuDinamico", new DatosUsuarioEnSesionDTO());
            }
        }

        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}