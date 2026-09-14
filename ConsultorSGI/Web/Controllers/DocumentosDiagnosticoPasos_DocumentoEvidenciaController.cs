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
    public class DocumentosDiagnosticoPasos_DocumentoEvidenciaController : Controller
    {
        #region Inyección de dependencias
        private IDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness _iDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public DocumentosDiagnosticoPasos_DocumentoEvidenciaController()
        {
            _iDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness = new DocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas DocumentosDiagnosticoPasos_DocumentoEvidencia

        public async Task<ActionResult> GetAllEvidenciasConCriteriosPaso(string pasoID)
        {
            try
            {
                var listaSistemasDeGestion = await _iDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness.GetAllEvidenciasConCriteriosPasoAsync(pasoID);
                return PartialView("_GetAllEvidenciasConCriterios", listaSistemasDeGestion);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllEvidenciasConCriterios", new List<DocumentosDiagnosticoPasosDTO>());
            }
        }

        public async Task<ActionResult> GetAllEvidenciasPorCriterio(string pasoID, string criterioID)
        {
            try
            {
                var listaSistemasDeGestion = await _iDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness.GetAllEvidenciasPorCriterioAsync(pasoID, criterioID);
                return PartialView("_GetAllPorPaso", listaSistemasDeGestion);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllPorPaso", new List<DocumentosDiagnosticoPasosDTO>());
            }
        }

        public async Task<ActionResult> Crear(string pasoID)
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
                var modelo = await _iDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness.FindAsync(x => x.StrEvidenciaID == registroID);
                return PartialView("_Editar", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Editar", new DocumentosDiagnosticoPasos_DocumentoEvidencia());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Guardar(DocumentosDiagnosticoPasos_DocumentoEvidencia modelo)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness.GuardarAsync(modelo);
                if (string.IsNullOrEmpty(respuesta)) 
                    return Json(new { msn = ResponseType.success.ToString(), registroID = modelo.StrEvidenciaID }, JsonRequestBehavior.AllowGet);

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
                var respuesta = await _iDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness.EliminarEvidenciaAsync(registroID);

                if (string.IsNullOrEmpty(respuesta)) 
                    return Json(new {msn = ResponseType.success.ToString(), JsonRequestBehavior.AllowGet});

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