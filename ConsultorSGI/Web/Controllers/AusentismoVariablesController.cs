using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class AusentismoVariablesController : Controller
    {
        private IAusentismoVariablesCoreBusiness _iAusentismoVariablesCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public AusentismoVariablesController() 
        {
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
            _iAusentismoVariablesCoreBusiness = new AusentismoVariablesCoreBusiness();
        }

        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllAusentismoVariablesAsync()
        {
            try
            {
                var listaAusentismo = await _iAusentismoVariablesCoreBusiness.GetAllAsync();
                return PartialView("_GetAllAutentismoVariables", listaAusentismo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllAutentismoVariables", new List<AusentismoVariables>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearAusentismoVariable()
        {
            try
            {
                ViewBag.cantidadEmpleadosActivos = await _iAusentismoVariablesCoreBusiness.GetTotalEmpleadosActivosAsync();
                return PartialView("_CrearAusentismoVariable");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearAusentismoVariable");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAusentismoVariableByEditarAsync(int ausentismoVariableID)
        {
            try
            {
                var modeloDiagnostico = await _iAusentismoVariablesCoreBusiness.FindAsync(x => x.IntAusentismoVariablesID == ausentismoVariableID);
                return PartialView("_EditarAusentismoVariable", modeloDiagnostico);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarAusentismoVariable", new Diagnosticos());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarAusentismoVariableAsync(AusentismoVariables modelo)
        {
            try
            {
                var respuesta = await _iAusentismoVariablesCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), ausentismoVariableID = modelo.IntAusentismoVariablesID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAusentismoVariableAsync(int ausentismoVariableID)
        {
            try
            {
                var modelo = await _iAusentismoVariablesCoreBusiness.FindAsync(x => x.IntAusentismoVariablesID == ausentismoVariableID);
                await _iAusentismoVariablesCoreBusiness.DeleteAsync(modelo);
                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}