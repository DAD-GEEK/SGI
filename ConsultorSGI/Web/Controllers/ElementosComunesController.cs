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
    public class ElementosComunesController : Controller
    {
        #region Variables
        private IElementosComunesCoreBusiness _iElementosComunesCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public ElementosComunesController()
        {
            _iElementosComunesCoreBusiness = new ElementosComunesCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas ElementosComunes
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllElementosComunesAsync()
        {
            try
            {
                var listaElementosComunes = await _iElementosComunesCoreBusiness.GetAllAsync();
                return PartialView("_GetAllElementosComunes", listaElementosComunes);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllElementosComunes", new List<ElementosComunes>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearElementoComun()
        {
            try
            {
                ViewBag.listaNormas = await _iElementosComunesCoreBusiness.DropDownListMultipleNormasAsync();
                return PartialView("_CrearElementoComun");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearElementoComun");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetElementoComunByEditarAsync(int elementoComunID)
        {
            try
            {
                var modeloElementoComun = await _iElementosComunesCoreBusiness.FindAsync(x => x.IntElementoComunID == elementoComunID);
                ViewBag.listaNormas = await _iElementosComunesCoreBusiness.DropDownListMultipleNormasAsync(modeloElementoComun);

                return PartialView("_EditarElementoComun", modeloElementoComun);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarElementoComun", new ElementosComunes());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarElementoComunAsync(ElementosComunes modelo)
        {
            try
            {
                var respuesta = await _iElementosComunesCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), elementoComunID = modelo.IntElementoComunID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteElementoComunAsync(int elementoComunID)
        {
            try
            {
                var modelo = await _iElementosComunesCoreBusiness.FindAsync(x => x.IntElementoComunID == elementoComunID);               

                await _iElementosComunesCoreBusiness.DeleteAsync(modelo);
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