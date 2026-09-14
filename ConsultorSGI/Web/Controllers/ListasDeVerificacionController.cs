using Across;
using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using Models.DTO;
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
    public class ListasDeVerificacionController : Controller
    {
        #region Variables
        private IListasDeVerificacionCoreBusiness _iListasDeVerificacionCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public ListasDeVerificacionController()
        {
            _iListasDeVerificacionCoreBusiness = new ListasDeVerificacionCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas ListasDeVerificacion
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> ObtenerAuditoriaDetalleAsync(int auditoriaID)
        {
            try
            {
                var modeloAuditoria = await _iListasDeVerificacionCoreBusiness.ObtenerAuditoriaAsync(auditoriaID);
                var modeloAuditoriaDetalle = await _iListasDeVerificacionCoreBusiness.ObtenerAuditoriaDetalleDTOAsync(modeloAuditoria);
                ViewBag.nombreTercero = modeloAuditoria.Terceros_Clientes.StrNombre;

                return PartialView("_ObtenerAuditoriaDetalle", modeloAuditoriaDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerAuditoriaDetalle", new List<AuditoriasDetalle>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerListaDeVerificacionEncabezadoAsync(int auditoriaDetalleID)
        {
            try
            {
                var modeloAuditoriaDetalle = await _iListasDeVerificacionCoreBusiness.ObtenerAuditoriaDetallePorIDAsync(auditoriaDetalleID);
                ViewBag.terceroNombre = modeloAuditoriaDetalle.Auditorias.Terceros_Clientes.StrNombre;

                return PartialView("_ObtenerListaDeVerificacionEncabezado", modeloAuditoriaDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerListaDeVerificacionEncabezado", new AuditoriasDetalle());
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerListaDeVerificacionDetalleAsync(int auditoriaDetalleID, bool isVistaInforme = false)
        {
            try
            {
                var modeloAuditoriaDetalle = await _iListasDeVerificacionCoreBusiness.FindWhereAsync(x => x.IntAuditoriaDetalleID == auditoriaDetalleID);
                ViewBag.isVistaInforme = isVistaInforme;

                return PartialView("_ObtenerListaDeVerificacionDetalle", modeloAuditoriaDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerListaDeVerificacionDetalle", new List<ListasDeVerificacion>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> AuditarListaDeVerificacion(int listaDeVerificacionID, bool isVistaInforme = false)
        {
            try
            {
                var listaDeVerificacion = await _iListasDeVerificacionCoreBusiness.FindAsync(x => x.IntListaVerificacionID == listaDeVerificacionID);
                var modeloAuditoriaDetalle = await _iListasDeVerificacionCoreBusiness.ObtenerItemListaDeVerificacionDTOAsync(listaDeVerificacion);
                ViewBag.isVistaInforme = isVistaInforme;

                return PartialView("_AuditarListaDeVerificacion", modeloAuditoriaDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_AuditarListaDeVerificacion", new List<ListasDeVerificacionDTO>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearItemListaDeVerificacion(int procesoID, int auditoriaID)
        {
            try
            {
                return PartialView("_CrearItemListaDeVerificacion");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearItemListaDeVerificacion");
            }
        }


        [HttpPost]
        public async Task<ActionResult> VistaObtenerNumeralesDeListaDeVerificacionAsync(int listaDeVerificacionID, int normaID)
        {
            try
            {
                var listaNumerales = await _iListasDeVerificacionCoreBusiness.ObtenerNumeralesPorListaDeVerificacionDTOAsync(listaDeVerificacionID);
                ViewBag.listaNormas = await _iListasDeVerificacionCoreBusiness.ObtenerNormasDeAuditoriaAsync(listaDeVerificacionID, normaID);

                return PartialView("_ObtenerNumeralesDeListaDeVerificacion", listaNumerales);

            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerNumeralesDeListaDeVerificacion", new List<ListasDeVerificacion_NumeralesDTO>());
            }
        }

        [HttpGet]
        public async Task<ActionResult> VistaCargarListaDeVerificacionDeProcesoActualAsync(int auditoriaDetalleID)
        {
            try
            {
                ViewBag.esVistaListaDeVerificacion = true;
                ViewBag.auditoriaDetalleID = auditoriaDetalleID;
                return PartialView("_CargarDatosProcesoActual");

            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CargarDatosProcesoActual", new List<ListasDeVerificacion>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> VistaDropDownNumeralesPorListaDeVerificacionAsync(int normaID)
        {
            try
            {
                return PartialView("_DropDownNumeralesPorListaDeVerificacion");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_DropDownNumeralesPorListaDeVerificacion");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerItemListaDeVerificacionParaEditar(int listaVerificacionID)
        {
            try
            {
                var modeloListaDeVerificacion = await _iListasDeVerificacionCoreBusiness.FindAsync(x => x.IntListaVerificacionID == listaVerificacionID);
                return PartialView("_EditarItemListaDeVerificacion", modeloListaDeVerificacion);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarItemListaDeVerificacion", new ListasDeVerificacion());
            }
        }

        public async Task<ActionResult> DescargarListaDeVerificacionPDFAsync(int auditoriaDetalleID)
        {
            try
            {
                var modelo = await _iListasDeVerificacionCoreBusiness.ObtenerInformeListasDeVerificacionPDFAsync(auditoriaDetalleID);
                var terceroNombre = modelo.Auditoria.Terceros_Clientes.StrNombre;
                var procesoNombre = modelo.AuditoriaDetalle.Procesos.StrDescripcion;

                var fechaAcual = Common.ObtenerFechaActualExacta();
                var fechaDeGeneracion = Common.ObtenerFechaLarga(fechaAcual);

                return new Rotativa.PartialViewAsPdf("_ExportarListasDeVerificacionPDF", modelo)
                {
                    CustomSwitches = $"--footer-center \" Listas de verificación - Auditoría N° {modelo.Auditoria.IntConsecutivo} - Fecha de generación: {fechaDeGeneracion} - Página: [page]/[toPage]\" --footer-line --footer-font-size \"8\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                    FileName = $"LISTA DE VERIFICACIÓN {procesoNombre}_{terceroNombre}.pdf",
                    PageSize = Rotativa.Options.Size.A4
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarListasDeVerificacionItemAsync(ListasDeVerificacion modelo, int procesoID)
        {
            try
            {
                var respuesta = await _iListasDeVerificacionCoreBusiness.SaveAllAsync(modelo, procesoID);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), listaDeVerificacionID = modelo.IntListaVerificacionID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CopiarPlantillaEnListaDeVerificacionDeProcesoActualAsync(int auditoriaDetalleID, int plantillaDetalleID)
        {
            try
            {
                var respuesta = await _iListasDeVerificacionCoreBusiness.CopiarPlantillaEnListaDeVerificacionDeProcesoActualAsync(auditoriaDetalleID, plantillaDetalleID);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarAuditoriaPorItemAsync(ListasDeVerificacion modelo, bool isVistaInforme = false)
        {
            try
            {
                var respuesta = await _iListasDeVerificacionCoreBusiness.UpdateListaDeVerificacionAuditarAsync(modelo, isVistaInforme);
                var modeloEnBD = await _iListasDeVerificacionCoreBusiness.FindAsync(x => x.IntListaVerificacionID == modelo.IntListaVerificacionID);

                int auditoriaID = modeloEnBD.AuditoriasDetalle.IntAuditoriaID;

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), auditoriaID = auditoriaID, listaVerificacionID = modelo.IntListaVerificacionID, isVistaInforme = isVistaInforme }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteItemListaDeVerificacionAsync(int listaDeVerificacionID)
        {
            try
            {
                var modelo = await _iListasDeVerificacionCoreBusiness.FindAsync(x => x.IntListaVerificacionID == listaDeVerificacionID);

                await _iListasDeVerificacionCoreBusiness.DeleteAsync(modelo);
                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> ObtenerNoConformidadesAuditoriaAsync(int auditoriaID, bool isVistaInforme = false)
        {
            try
            {
                var listaNoConformidades = await _iListasDeVerificacionCoreBusiness.ObtenerNoConformidadesAuditoriaAsync(auditoriaID);
                ViewBag.listaNormasCantidades = await _iListasDeVerificacionCoreBusiness.ContadorDeNoConformidadesAsync(listaNoConformidades);
                ViewBag.isVistaInforme = isVistaInforme;

                return PartialView("_ObtenerNoConformidades", listaNoConformidades);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerNoConformidades", new AuditoriaNoConformidadesDTO());
            }
        }


        #endregion
    }
}