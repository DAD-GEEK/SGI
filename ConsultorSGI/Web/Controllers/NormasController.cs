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
    public class NormasController : Controller
    {
        #region Variables
        private INormasCoreBusiness _iNormasCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public NormasController()
        {
            _iNormasCoreBusiness = new NormasCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Normas
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllNormasAsync()
        {
            try
            {
                var listaNormas = await _iNormasCoreBusiness.ObtenerNormasDTOAsync();
                return PartialView("_GetAllNormas", listaNormas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllNormas", new List<Normas>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearNorma()
        {
            try
            {
                ViewBag.listaCriterios = _iNormasCoreBusiness.SelectListCriteriosAsync();
                return PartialView("_CrearNorma");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearNorma");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetNormaByEditarAsync(int NormaID)
        {
            try
            {
                var modeloNorma = await _iNormasCoreBusiness.FindAsync(x => x.IntNormaID == NormaID);
                ViewBag.listaCriterios = _iNormasCoreBusiness.SelectListCriteriosAsync(modeloNorma.IntTipoCriterio.ToString());
                return PartialView("_EditarNorma", modeloNorma);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarNorma", new Normas());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarNormaAsync(Normas modelo)
        {
            try
            {
                var respuesta = await _iNormasCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), normaID = modelo.IntNormaID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteNormaAsync(int NormaID)
        {
            try
            {
                var modelo = await _iNormasCoreBusiness.FindAsync(x => x.IntNormaID == NormaID);

                await _iNormasCoreBusiness.DeleteAsync(modelo);
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