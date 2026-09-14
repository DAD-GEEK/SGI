using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;
using System.Linq;
using static Across.Enumeraciones;
using Across.ArchivosDeRecurso;

namespace Web.Controllers
{
    [Authorize]
    public class DiagnosticosController : Controller
    {
        #region Variables
        private IDiagnosticosCoreBusiness _iDiagnosticosCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public DiagnosticosController()
        {
            _iDiagnosticosCoreBusiness = new DiagnosticosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Diagnosticos
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllDiagnosticosAsync()
        {
            try
            {
                var listaDiagnosticos = await _iDiagnosticosCoreBusiness.GetAllAsync();
                return PartialView("_GetAllDiagnosticos", listaDiagnosticos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllDiagnosticos", new List<Diagnosticos>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearDiagnostico()
        {
            try
            {
                return PartialView("_CrearDiagnostico");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearDiagnostico");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetDiagnosticoByEditarAsync(int diagnosticoID)
        {
            try
            {
                var modeloDiagnostico = await _iDiagnosticosCoreBusiness.FindAsync(x => x.IntDiagnosticoID == diagnosticoID);
                return PartialView("_EditarDiagnostico", modeloDiagnostico);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarDiagnostico", new Diagnosticos());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarDiagnosticoAsync(Diagnosticos modelo)
        {
            try
            {
                var respuesta = await _iDiagnosticosCoreBusiness.SaveAllAsync(modelo);
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
        public async Task<ActionResult> DeleteDiagnosticoAsync(int diagnosticoID)
        {
            try
            {
                var modelo = await _iDiagnosticosCoreBusiness.FindAsync(x => x.IntDiagnosticoID == diagnosticoID);

                if (modelo.Ausentismo.Count() != 0) return Json(new { error = string.Format(RecursoDiagnostico.msnRelacionConAusentismo, modelo.StrCodigo) }, JsonRequestBehavior.AllowGet);

                await _iDiagnosticosCoreBusiness.DeleteAsync(modelo);
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