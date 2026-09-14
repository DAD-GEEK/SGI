using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private AccountCoreBusiness _securityCoreBusiness;
        private TercerosUsuariosCoreBusiness _tercerosUsuariosCoreBusiness;
        private EmailCoreBusiness _emailCoreBusiness;
        private TercerosCoreBusiness _tercerosCoreBusiness;

        public AccountController()
        {
        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            try
            {
                if (TempData["errorAutenticacion"] != null) ViewBag.error = TempData["errorAutenticacion"];

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message + ex.InnerException.Message;
                return View("Error");
            }

        }

        public ActionResult Index() => View();

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                _securityCoreBusiness = new AccountCoreBusiness();

                if (!ModelState.IsValid) return View(model);

                var currentUser = await UserManager.FindByNameAsync(model.Email);
                if (currentUser != null)
                {
                    if (!await UserManager.IsEmailConfirmedAsync(currentUser.Id))
                    {
                        this.SendEmailConfirmationTokenAsync(currentUser);
                        ViewBag.ErrorAutenticacion = "Debe confirmar su cuenta en su correo electrónico para acceder a la aplicación.";
                        return View(model);
                    }
                }

                var validarDatosAcceso = await _securityCoreBusiness.ValidarInicioSesion(model.Email);

                //Usuario bloqueado - 
                if (!string.IsNullOrEmpty(validarDatosAcceso))
                {
                    ModelState.AddModelError("", validarDatosAcceso);
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
                switch (result)
                {
                    case SignInStatus.Success:

                        var identity = await UserManager.CreateIdentityAsync(currentUser, DefaultAuthenticationTypes.ApplicationCookie);

                        identity = await ApplicationUser.CreateUserClaims(
                            identity,
                            UserManager,
                            currentUser);

                        AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Datos de acceso incorrectos");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message + ex.InnerException.Message;
                return View("Error");
            }

        }

        [AllowAnonymous]
        private async void SendEmailConfirmationTokenAsync(ApplicationUser user)
        {
            _emailCoreBusiness = new EmailCoreBusiness();

            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var Parametros = new Dictionary<string, string>
            {
                {"{usuario}", user.NombreUsuario },
                {"{url}", callbackUrl }
            };

            await _emailCoreBusiness.EnviarEmailAsync(user.Email, "Confirmar correo electrónico - Consultor SGI", _emailCoreBusiness.EmailBody("ConfirmarCorreoElectronico.html", Parametros), null, true, null);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _securityCoreBusiness = new AccountCoreBusiness();
                _tercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();

                var usuarioAsignadoAtercero = await _securityCoreBusiness.UsuarioAsignadoATercero(model.Email);

                if (usuarioAsignadoAtercero)
                {
                    var existeUsuario = await _securityCoreBusiness.ExisteUsuario(model.Email);

                    if (string.IsNullOrEmpty(existeUsuario))
                    {
                        var terceroUsuario = await _tercerosUsuariosCoreBusiness.FindAsync(x => x.StrUsuarioEmail == model.Email);

                        var user = new ApplicationUser { UserName = model.Email.ToLower(), Email = model.Email.ToLower(), NombreUsuario = model.Name.ToUpper(), PhoneNumber = model.PhoneNumber, TerceroID = (int)terceroUsuario.IntTerceroID, FechaIngreso = DateTime.Now };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            _emailCoreBusiness = new EmailCoreBusiness();
                            //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                            // Send an email with this link
                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                            this.SendEmailConfirmationTokenAsync(user);

                            return View("ConfirmSendEmail");
                        }
                        AddErrors(result);

                    }
                    else ViewBag.ErrorRegistro = existeUsuario;
                }
                else ViewBag.ErrorRegistro = RecursoUsuarios.msnUsuarioSinAsignar;
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ConfirmSendEmail()
        {
            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            _emailCoreBusiness = new EmailCoreBusiness();
            _tercerosCoreBusiness = new TercerosCoreBusiness();

            if (ModelState.IsValid)
            {

                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var infoTercero = await _tercerosCoreBusiness.FindAsync(x => x.IntTerceroID == user.TerceroID);

                var Parametros = new Dictionary<string, string>
                {
                    {"{usuario}", user.NombreUsuario },
                    {"{url}", callbackUrl }
                };

                await _emailCoreBusiness.EnviarEmailAsync(model.Email, "Recuperar contraseña - Consultor SGI", _emailCoreBusiness.EmailBody("RecuperarContraseña.html", Parametros), null, true, null);

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            _tercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();

            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Manage");

            var terceroUsuario = await _tercerosUsuariosCoreBusiness.FindAsync(x => x.StrUsuarioEmail == model.Email && x.OpcEstado == true);

            //Validar si el usuario está asignado a un tercero
            if (terceroUsuario == null)
            {
                TempData["errorAutenticacion"] = RecursoUsuarios.msnUsuarioSinAsignar;
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    ViewBag.ErrorAutenticacion = RecursoUsuarios.msnErrorAutenticacionExterna;
                    //return View("ExternalLoginFailure");
                    return View();
                }

                var user = new ApplicationUser { UserName = model.Email.ToLower(), Email = model.Email.ToLower(), TerceroID = (int)terceroUsuario.IntTerceroID, NombreUsuario = terceroUsuario.StrUsuarioNombre.ToUpper(), FechaIngreso = DateTime.Now };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                        {
                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            await UserManager.ConfirmEmailAsync(user.Id, code);
                        }

                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("login", "Account");
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult VerificarSesionDeUsuario()
        {
            try
            {
                return Json(new { msn = ResponseType.success.ToString(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception)
            {
                var url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                return Json(new { error = "Su sesión ha caducado", url = url, JsonRequestBehavior.AllowGet });

            }
        }

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
    }
}