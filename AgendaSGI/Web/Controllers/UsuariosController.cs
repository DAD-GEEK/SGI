using CoreBusiness;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    public class UsuariosController : Controller
    {
        UsuariosCoreBusiness _usuariosCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;
        RolesCoreBusiness _rolesCoreBusiness;

        [AuthorizeUser]
        public async Task<ActionResult> Index()
        {
            try
            {
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

        #region Abrir vistas
        [HttpPost]
        public async Task<ActionResult> GetAllUsuarios()
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                var model = await _usuariosCoreBusiness.GetAllAsync();

                return PartialView("_GetAllUsuarios", model);
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
                return PartialView("_GetAllUsuarios");
            }
        }


        [HttpGet]
        public async Task<ActionResult> CrearUsuario()
        {
            try
            {
                _rolesCoreBusiness = new RolesCoreBusiness();

                var roles = (from r in await _rolesCoreBusiness.GetAllAsync()
                             orderby r.IntRolID
                                select new { RolID = r.IntRolID, Descripcion = r.StrDescripcion });

                ViewBag.listaRoles = new SelectList(roles, "RolID", "Descripcion");

                return View("_CrearUsuario", new Usuarios());
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
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetUsuarioParaEditarAsync(int usuarioID)
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _rolesCoreBusiness = new RolesCoreBusiness();


                var model = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == usuarioID);

                var roles = (from r in await _rolesCoreBusiness.GetAllAsync()
                             orderby r.IntRolID
                             select new { RolID = r.IntRolID, Descripcion = r.StrDescripcion });

                ViewBag.listaRoles = new SelectList(roles, "RolID", "Descripcion", model.IntRolID);

                return View("_EditarUsuario", model);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_EditarUsuario", new Usuarios());
            }
        }


        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearUsuarioAsync(Usuarios modelo)
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();

                string retur = await _usuariosCoreBusiness.CreateAsync(modelo);

                if (string.IsNullOrEmpty(retur))
                {
                    return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = retur }, JsonRequestBehavior.AllowGet);
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
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUsuarioAsync(Usuarios modelo, string tipo, bool opcClave, string claveAnt)
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                string retur = string.Empty;

                if (opcClave == true && tipo == "p")
                {
                    retur = await _usuariosCoreBusiness.ValidarCambioClave(modelo, claveAnt);                   
                }

                if (string.IsNullOrEmpty(retur))
                {

                    //Tivo de vista p-> Perfil
                    if (tipo != "p")
                    {
                        var usuario = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == modelo.IntUsuarioID);
                        modelo.IntCiudadID = usuario.IntCiudadID;
                        modelo.StrDireccion = usuario.StrDireccion;
                        modelo.StrTelefonoFijo = usuario.StrTelefonoFijo;
                        modelo.StrCelular = usuario.StrCelular;
                        modelo.StrColor = usuario.StrColor;
                    }

                    retur = await _usuariosCoreBusiness.UpdateAsync(modelo, opcClave);

                    if (string.IsNullOrEmpty(retur))
                    {
                        return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { error = retur }, JsonRequestBehavior.AllowGet);

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
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUsuarioAsync(int usuarioID)
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();

                var Model = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == usuarioID);

                await _usuariosCoreBusiness.DeleteAsync(Model);

                return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public async Task<bool> CheckExists(string StrCodigo)
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();

                return await _usuariosCoreBusiness.ExistAsync(x => x.StrCodigo == StrCodigo);
            }
            catch (Exception)
            {
                return false;
            }
        }


        #endregion



    }
}