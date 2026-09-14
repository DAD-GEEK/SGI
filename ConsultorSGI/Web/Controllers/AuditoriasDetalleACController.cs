using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class AuditoriasDetalleACController : Controller
    {
        #region Variables
        private IAuditoriasDetalleACCoreBusiness _iAuditoriasDetalleACCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public AuditoriasDetalleACController()
        {
            _iAuditoriasDetalleACCoreBusiness = new AuditoriasDetalleACCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region AuditoriasDetalleAC
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> CrearDetalleAperturaCierre()
        {
            try
            {
                return PartialView("_CrearDetalleAperturaCierre");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearDetalleAperturaCierre");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetDetalleACByEditarAsync(int auditoriaDetalleACID)
        {
            try
            {
                var modeloAuditoriaDetalle = await _iAuditoriasDetalleACCoreBusiness.FindAsync(x => x.IntAuditoriaDetalleACID == auditoriaDetalleACID);
              

                return PartialView("_EditarDetalleAperturaCierre", modeloAuditoriaDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarDetalleAperturaCierre", new AuditoriasDetalleAC());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarDetalleAperturaCierreAsync(AuditoriasDetalleAC modelo)
        {
            try
            {
                var respuesta = await _iAuditoriasDetalleACCoreBusiness.SaveAllAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), auditoriaDetalleACID = modelo.IntAuditoriaDetalleACID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDetalleAperturaCierreAsync(int auditoriaDetalleACID)
        {
            try
            {
                var modelo = await _iAuditoriasDetalleACCoreBusiness.FindAsync(x => x.IntAuditoriaDetalleACID == auditoriaDetalleACID);

                await _iAuditoriasDetalleACCoreBusiness.DeleteAsync(modelo);
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