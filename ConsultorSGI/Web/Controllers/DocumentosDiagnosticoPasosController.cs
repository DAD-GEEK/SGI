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
    public class DocumentosDiagnosticoPasosController : Controller
    {
        #region Inyección de dependencias
        private IDocumentosDiagnosticoPasosCoreBusiness _iDocumentosDiagnosticoPasosCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public DocumentosDiagnosticoPasosController()
        {
            _iDocumentosDiagnosticoPasosCoreBusiness = new DocumentosDiagnosticoPasosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas DocumentosDiagnosticoPasos
        public async Task<ActionResult> GetAllPorSistemaDeGestion(string sistemaDeGestion)
        {
            try
            {
                ViewBag.sistemaDeGestionID = sistemaDeGestion;
                var listaSistemasDeGestion = await _iDocumentosDiagnosticoPasosCoreBusiness.ListaDocumentosDiagnosticoPasosPorSistemaDeGestionDTOAsync(sistemaDeGestion);
                return PartialView("_GetAll", listaSistemasDeGestion);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAll", new List<DocumentosDiagnosticoPasosDTO>());
            }
        }

        public async Task<ActionResult> Crear(string sistemaDeGestionID)
        {
            try
            {
                ViewBag.sistemaDeGestionID = sistemaDeGestionID;
                ViewBag.listaNiveles = await _iDocumentosDiagnosticoPasosCoreBusiness.DropDownListMultipleNivelesAsync();
                ViewBag.listaFases = await _iDocumentosDiagnosticoPasosCoreBusiness.DropDownListFasesAsync();

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
                var modelo = await _iDocumentosDiagnosticoPasosCoreBusiness.FindAsync(x => x.StrPasoID == registroID);

                ViewBag.listaNiveles = await _iDocumentosDiagnosticoPasosCoreBusiness.DropDownListMultipleNivelesAsync(modelo);
                ViewBag.listaFases = await _iDocumentosDiagnosticoPasosCoreBusiness.DropDownListFasesAsync(modelo.StrFaseID);

                return PartialView("_Editar", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Editar", new DocumentosDiagnosticoPasos());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Guardar(DocumentosDiagnosticoPasos modelo)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoPasosCoreBusiness.GuardarAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), registroID = modelo.StrPasoID }, JsonRequestBehavior.AllowGet);

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
                var respuesta = await _iDocumentosDiagnosticoPasosCoreBusiness.EliminarDocumentoPasoAsync(registroID);

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