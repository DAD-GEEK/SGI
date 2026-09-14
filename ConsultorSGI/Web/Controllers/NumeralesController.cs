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
    public class NumeralesController : Controller
    {
        #region Variables
        private INumeralesCoreBusiness _iNumeralesCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public NumeralesController()
        {
            _iNumeralesCoreBusiness = new NumeralesCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Numerales
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllNumeralesAsync()
        {
            try
            {
                var listaNumerales = await _iNumeralesCoreBusiness.ObtenerTodosLosNumeralesAsync();
                return PartialView("_GetAllNumerales", listaNumerales);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllNumerales", new List<Numerales>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearNumeral(int normaID)
        {
            try
            {
                ViewBag.modeloNormas = await _iNumeralesCoreBusiness.ObtenerNormaPorIdAsync(normaID);
                return PartialView("_CrearNumeral");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearNumeral");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetNumeralByEditarAsync(int NumeralID, int normaID)
        {
            try
            {
                ViewBag.modeloNormas = await _iNumeralesCoreBusiness.ObtenerNormaPorIdAsync(normaID);
                var modeloNumeral = await _iNumeralesCoreBusiness.FindAsync(x => x.IntNumeralID == NumeralID);

                return PartialView("_EditarNumeral", modeloNumeral);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarNumeral", new Numerales());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarNumeralAsync(Numerales modelo, int normaID)
        {
            try
            {
                var respuesta = await _iNumeralesCoreBusiness.SaveAllAsync(modelo, normaID);

                if (string.IsNullOrEmpty(respuesta))                
                    return Json(new { msn = ResponseType.success.ToString(), numeralID = modelo.IntNumeralID, normaID = normaID }, JsonRequestBehavior.AllowGet);                

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteNumeralAsync(int NumeralID)
        {
            try
            {
                var modelo = await _iNumeralesCoreBusiness.FindAsync(x => x.IntNumeralID == NumeralID);

                await _iNumeralesCoreBusiness.DeleteAsync(modelo);
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