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
    public class TipoEventosController : Controller
    {
        TipoEventosCoreBusiness _tipoEventosCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;

        [AuthorizeUser]

        public ActionResult Index()
        {
            return View();
        }

        #region Abrir vistas
        [HttpPost]
        public async Task<ActionResult> GetAllTipoEventos()
        {
            try
            {
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();
                var model = await _tipoEventosCoreBusiness.GetAllAsync();

                return PartialView("_GetAllTipoEventos", model);
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
                return PartialView("_GetAllTipoEventos");
            }
        }

        [HttpGet]
        public async Task<ActionResult> CrearTipoEvento()
        {
            try
            {
                return View("_CrearTipoEvento");
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
        public async Task<ActionResult> GetTipoEventoParaEditarAsync(int tipoEventoID)
        {
            try
            {
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();

                var modelo = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == tipoEventoID);

                return View("_EditarTipoEvento", modelo);
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
                return PartialView("_EditarTipoEvento", new TipoEventos());

            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearTipoEventoAsync(TipoEventos modelo)
        {
            try
            {
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();              

                string retur = await _tipoEventosCoreBusiness.CreateAsync(modelo);

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
        public async Task<ActionResult> UpdateTipoEventoAsync(TipoEventos modelo)
        {
            try
            {
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();

                string retur = await _tipoEventosCoreBusiness.UpdateAsync(modelo);

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
        public async Task<ActionResult> DeleteTipoEventoAsync(int tipoEventoID)
        {
            try
            {
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();

                var Model = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == tipoEventoID);

                await _tipoEventosCoreBusiness.DeleteAsync(Model);

                return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
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
        public async Task<bool> CheckExists(string StrCodigo)
        {
            try
            {
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();

                return await _tipoEventosCoreBusiness.ExistAsync(x => x.StrCodigo == StrCodigo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}