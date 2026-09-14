using CoreBusiness;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    public class CuentaController : Controller
    {
        UsuariosCoreBusiness _usuariosCoreBusiness;
        CiudadesCoreBusiness _ciudadesCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;

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

        [HttpPost]
        public async Task<ActionResult> GetMiPerfil()
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _ciudadesCoreBusiness = new CiudadesCoreBusiness();

                Usuarios usuarioSession = (Usuarios)Session["Usuario"];

                var usuarioCodigo = usuarioSession.StrCodigo;
                var usuario = await _usuariosCoreBusiness.FindAsync(x => x.StrCodigo.ToLower().Trim() == usuarioCodigo.ToLower().Trim());

                var Ciudades = (from c in await _ciudadesCoreBusiness.GetAllAsync()
                                     orderby c.StrCodigo
                                     select new { Ciudad = c.IntCiudadID, Descripcion = c.StrCodigo + " - " + c.StrDescripcion});

                ViewBag.listaCiudades = new SelectList(Ciudades, "Ciudad", "Descripcion", usuario.IntCiudadID);

                return PartialView("_MiPerfil", usuario);
                
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
                return PartialView("_MiPerfil");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUsuarioAsync(Usuarios modelo, bool opcClave)
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();

                string retur = await _usuariosCoreBusiness.UpdateAsync(modelo, opcClave);

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
    }
}