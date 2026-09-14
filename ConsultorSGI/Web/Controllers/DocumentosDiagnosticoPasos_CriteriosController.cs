using Across.ArchivosDeRecurso;
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
    public class DocumentosDiagnosticoPasos_CriteriosController : Controller
    {
        #region Inyección de dependencias
        private IDocumentosDiagnosticoPasos_CriteriosCoreBusiness _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public DocumentosDiagnosticoPasos_CriteriosController()
        {
            _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness = new DocumentosDiagnosticoPasos_CriteriosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas DocumentosDiagnosticoPasos_Criterios
        public async Task<ActionResult> GetAllCriteriosPorEvidencia(string pasoID, string evidenciaID)
        {
            try
            {
                var listaDeCriteriosPorEvidenciaDTO = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.ObtenerListaDeCriteriosPorDocumentoEvidenciaPorPasoAsync(pasoID, evidenciaID);
                return PartialView("_GetAllPorEvidencia", listaDeCriteriosPorEvidenciaDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllPorEvidencia", new List<DocumentosDiagnosticoPasos_CriteriosDTO>());
            }
        }

        public async Task<ActionResult> Crear()
        {
            try
            {
                return PartialView("_Crear");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Crear");
            }
        }

        public async Task<ActionResult> GetByEditar(string registroID)
        {
            try
            {
                var modelo = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.FindAsync(x => x.StrCriterioID == registroID);
                return PartialView("_Editar", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Editar", new DocumentosDiagnosticoPasos_Criterios());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Guardar(DocumentosDiagnosticoPasos_Criterios modelo)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.GuardarAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), registroID = modelo.StrCriterioID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string registroID)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.EliminarCriterioAsync(registroID);

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
        public async Task<ActionResult> CambiarOrdenamientoSortable(List<DocumentosDiagnosticoPasos_Criterios> listaCriterios)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.CambiarOrdenamientoSortableAsync(listaCriterios);

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
        #endregion
    }
}