using Across.ArchivosDeRecurso;
using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class CentrosDeTrabajoController : Controller
    {
        #region Variables
        private ICentrosDeTrabajoCoreBusiness _iCentrosDeTrabajoCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public CentrosDeTrabajoController()
        {
            _iCentrosDeTrabajoCoreBusiness = new CentrosDeTrabajoCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas CentrosDeTrabajo
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllCentrosDeTrabajoAsync(int terceroID)
        {
            try
            {
                var listaCentrosDeTrabajo = await _iCentrosDeTrabajoCoreBusiness.FindWhereAsync(x => x.IntTerceroID == terceroID);
                return PartialView("_GetAllCentrosDeTrabajo", listaCentrosDeTrabajo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllCentrosDeTrabajo", new List<CentrosDeTrabajo>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearCentroDeTrabajo()
        {
            try
            {
                return PartialView("_CrearCentroDeTrabajo");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearCentroDeTrabajo");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetCentroDeTrabajoByEditarAsync(int centroDeTrabajoID)
        {
            try
            {
                var modeloCentroDeTrabajo = await _iCentrosDeTrabajoCoreBusiness.FindAsync(x => x.IntCentroDeTrabajoID == centroDeTrabajoID);
                return PartialView("_EditarCentroDeTrabajo", modeloCentroDeTrabajo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarCentroDeTrabajo", new CentrosDeTrabajo());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarCentroDeTrabajoAsync(CentrosDeTrabajo modelo)
        {
            try
            {
                var respuesta = await _iCentrosDeTrabajoCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta)) return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCentroDeTrabajoAsync(int centroDeTrabajoID)
        {
            try
            {
                var modelo = await _iCentrosDeTrabajoCoreBusiness.FindAsync(x => x.IntCentroDeTrabajoID == centroDeTrabajoID);

                if (modelo.Empleados.Count() != 0) return Json(new { error = string.Format(RecursoCentrosDeTrabajo.msnRelacionConEmpleados, modelo.StrCodigo) }, JsonRequestBehavior.AllowGet);

                await _iCentrosDeTrabajoCoreBusiness.DeleteAsync(modelo);
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