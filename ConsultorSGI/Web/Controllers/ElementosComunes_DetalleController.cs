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
    public class ElementosComunes_DetalleController : Controller
    {
        #region Variables
        private IElementosComunes_DetalleCoreBusiness _iElementosComunes_DetalleCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public ElementosComunes_DetalleController()
        {
            _iElementosComunes_DetalleCoreBusiness = new ElementosComunes_DetalleCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas ElementosComunesDetalle
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> ObtenerElementoComunPorIDAsync(int elementoComunID)
        {
            try
            {
                var listaElementosComunesDetalle = await _iElementosComunes_DetalleCoreBusiness.ObtenerElementoComunDTOPorIdAsync(elementoComunID);
                return PartialView("_ObtenerElementosComunesDetalle", listaElementosComunesDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerElementosComunesDetalle", new List<ElementosComunes_Detalle>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearDetalleElementosComunesAsync(int elementoComunID)
        {
            try
            {
                var listaNumerales = await _iElementosComunes_DetalleCoreBusiness.ObtenerNumeralesPorNormasAsync(elementoComunID);
                ViewBag.elementoComunID = elementoComunID;

                return PartialView("_CrearDetalleElementoComun", listaNumerales);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearDetalleElementoComun");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetDetalleElementosComunesByEditAsync(int elementoComunDetalleID)
        {
            try
            {
                var listaNumerales = await _iElementosComunes_DetalleCoreBusiness.ObtenerElementosComunes_DetalleDTO(elementoComunDetalleID);

                return PartialView("_EditarDetalleElementoComun", listaNumerales);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarDetalleElementoComun");
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarElementoComunDetalleAsync(ElementosComunes_Detalle modelo)
        {
            try
            {
                var respuesta = await _iElementosComunes_DetalleCoreBusiness.SaveAllAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), elementoComunDetalleID = modelo.IntDetalleID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteElementoComunDetalleAsync(int registroID)
        {
            try
            {
                var modelo = await _iElementosComunes_DetalleCoreBusiness.FindAsync(x => x.IntDetalleID == registroID);

                int elementoComunID = modelo.IntElementoComunID;
                await _iElementosComunes_DetalleCoreBusiness.DeleteAsync(modelo);
                return Json(new { msn = ResponseType.success.ToString(), elementoComunID = elementoComunID }, JsonRequestBehavior.AllowGet);
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