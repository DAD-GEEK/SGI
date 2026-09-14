using CoreBusiness;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    public class ConfiguracionController : Controller
    {
        ConfiguracionCoreBusiness _configuracionCoreBusiness;
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
        public async Task<ActionResult> GetConfiguracion()
        {
            try
            {
                _configuracionCoreBusiness = new ConfiguracionCoreBusiness();
                var model = await _configuracionCoreBusiness.GetAllAsync();
                model = model.ToList();

                if (model.Count() != 0)
                {
                    var configuracion = model.FirstOrDefault();

                    List<SelectListItem> lista = new List<SelectListItem>();

                    lista.Add(new SelectListItem() { Value = "timeGridDay", Text = "Vista diaria"  });
                    lista.Add(new SelectListItem() { Value = "timeGridWeek", Text = "Vista semanal" });
                    lista.Add(new SelectListItem() { Value = "dayGridMonth", Text = "Vista mensual" });
                    lista.Add(new SelectListItem() { Value = "listWeek", Text = "Vista agenda" });             

                    ViewBag.ListaVistas = new SelectList(lista, "Value", "Text", configuracion.StrVistaAgenda);

                    return PartialView("_GetConfiguracionEditar", configuracion);
                }

                return PartialView("_GetConfiguracion");
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
                return PartialView("_GetConfiguracion");
            }
        }

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearConfiguracionAsync(Configuracion modelo)
        {
            try
            {
                _configuracionCoreBusiness = new ConfiguracionCoreBusiness();

                string retur = await _configuracionCoreBusiness.CreateAsync(modelo);

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
        public async Task<ActionResult> UpdateConfiguracionAsync(Configuracion modelo)
        {
            try
            {
                _configuracionCoreBusiness = new ConfiguracionCoreBusiness();

                string retur = await _configuracionCoreBusiness.UpdateAsync(modelo);

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

        #endregion
    }
}