using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class AuditoriasDetalleController : Controller
    {
        #region Variables
        private IAuditoriasDetalleCoreBusiness _iAuditoriasDetalleCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public AuditoriasDetalleController()
        {
            _iAuditoriasDetalleCoreBusiness = new AuditoriasDetalleCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas AuditoriasDetalle
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllDetallePlanAuditoriasAsync(int auditoriaID)
        {
            try
            {
                var listaAuditoriasDetalle = await _iAuditoriasDetalleCoreBusiness.ObtenerDetalleAuditoriaAsync(auditoriaID);
                return PartialView("_GetAllDetallePlanAuditoria", listaAuditoriasDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllDetallePlanAuditoria", new List<AuditoriasDetalleDTO>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearPlanAuditoriaDetalle(int auditoriaID)
        {
            try
            {
                ViewBag.listaProcesos = await _iAuditoriasDetalleCoreBusiness.SelectListAuditoriaProcesosAsync(auditoriaID);
                ViewBag.listaModalidades = _iAuditoriasDetalleCoreBusiness.SelectListModalidades();
                return PartialView("_CrearPlanAuditoriaDetalle");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearPlanAuditoriaDetalle");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAuditoriaDetalleByEditarAsync(int auditoriaDetalleID)
        {
            try
            {
                var modeloAuditoriaDetalle = await _iAuditoriasDetalleCoreBusiness.FindAsync(x => x.IntAuditoriaDetalleID == auditoriaDetalleID);
                ViewBag.listaProcesos = await _iAuditoriasDetalleCoreBusiness.SelectListAuditoriaProcesosAsync(modeloAuditoriaDetalle.Auditorias.IntAuditoriaID, modeloAuditoriaDetalle.IntProcesoID.ToString());
                ViewBag.listaModalidades = _iAuditoriasDetalleCoreBusiness.SelectListModalidades(modeloAuditoriaDetalle.StrModalidad);

                return PartialView("_EditarPlanAuditoriaDetalle", modeloAuditoriaDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarPlanAuditoriaDetalle", new AuditoriasDetalle());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarAuditoriaDetalleAsync(AuditoriasDetalle modelo)
        {
            try
            {
                var respuesta = await _iAuditoriasDetalleCoreBusiness.SaveAllAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), auditoriaDetalleID = modelo.IntAuditoriaDetalleID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAuditoriaDetalleAsync(int AuditoriaDetalleID)
        {
            try
            {
                var modelo = await _iAuditoriasDetalleCoreBusiness.FindAsync(x => x.IntAuditoriaDetalleID == AuditoriaDetalleID);

                await _iAuditoriasDetalleCoreBusiness.DeleteAsync(modelo);
                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

      

        #endregion
    }
}