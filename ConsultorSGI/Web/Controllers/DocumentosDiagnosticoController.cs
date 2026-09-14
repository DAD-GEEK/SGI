using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness;
using CoreBusiness.Interfaces;
using DataAccess.Servicios;
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
    public class DocumentosDiagnosticoController : Controller
    {
        #region Inyección de dependencias
        private IDocumentosDiagnosticoCoreBusiness _iDocumentosDiagnosticoCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public DocumentosDiagnosticoController()
        {
            _iDocumentosDiagnosticoCoreBusiness = new DocumentosDiagnosticoCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas DocumentosDiagnostico
        [UserAuthenticationFilter]
        public ActionResult Index(string sistemaDeGestion)
        {
            try
            {
                ViewBag.sistemaDeGestion = sistemaDeGestion;

                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<ActionResult> GetAllPorSistemaDeGestion(string sistemaDeGestion)
        {
            try
            {
                if (string.IsNullOrEmpty(sistemaDeGestion))
                {
                    ViewBag.Error = "Sin sistema de gestión asignado";
                    return PartialView("_GetAll", new List<DocumentosDiagnostico>());
                }

                ViewBag.sistemaDeGestion = sistemaDeGestion;
                var listaDocumentosDiagnosticosPorSistema = await _iDocumentosDiagnosticoCoreBusiness.FindWhereAsync(x => x.StrSistemaDeGestionID == sistemaDeGestion);

                return PartialView("_GetAll", listaDocumentosDiagnosticosPorSistema);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAll", new List<DocumentosDiagnostico>());
            }
        }

        public async Task<ActionResult> AbrirVistaCrear()
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

        public async Task<ActionResult> AbrirVistaParaDiligenciar(string documentoID)
        {
            try
            {
                var documentoDiagnosticoDTO = await _iDocumentosDiagnosticoCoreBusiness.ObtenerDocumentoDiagnosticoDTO(documentoID);
                return PartialView("_DiligenciarDocumento", documentoDiagnosticoDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_DiligenciarDocumento", new DocumentosDiagnosticoDTO());
            }
        }

        public async Task<ActionResult> AbrirVistaInformacionEmpresa(int terceroID)
        {
            try
            {
                var documentoDiagnosticoDTO = await _iDocumentosDiagnosticoCoreBusiness.ObtenerInformacionDeEmpresaAsync(terceroID);
                return PartialView("_DiligenciarDocumento", documentoDiagnosticoDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_DiligenciarDocumento", new DocumentosDiagnosticoDTO());
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerConsolidadoDeResultadosGlobales(string documentoID)
        {
            try
            {
                var consolidadoDocumento = await _iDocumentosDiagnosticoCoreBusiness.ObtenerDocumentoDiagnosticoDTO(documentoID);

                return PartialView("_Consolidado", consolidadoDocumento);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Consolidado", new DocumentosDiagnosticoDTO());

            }
        }

        public async Task<ActionResult> DescargarPDFAsync(string documentoID)
        {
            try
            {
                var documentoDiagnosticoDTO = await _iDocumentosDiagnosticoCoreBusiness.ObtenerDocumentoDiagnosticoDTO(documentoID);

                var fechaAcual = Common.ObtenerFechaActualExacta();
                var fechaDeGeneracion = Common.ObtenerFechaLarga(fechaAcual);

                return new Rotativa.PartialViewAsPdf($"_DocumentoDiagnosticoPDF", documentoDiagnosticoDTO)
                {
                    CustomSwitches = $"--footer-center \" Documento diagnóstico - Fecha de generación: {fechaDeGeneracion} - Página: [page]/[toPage]\" --footer-line --footer-font-size \"8\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                    FileName = $"DocumentoDiagnostico_{documentoDiagnosticoDTO.InformacionEmpresa.StrCiudadNombre}.pdf",
                    PageSize = Rotativa.Options.Size.A4
                };
            }
            catch (Exception)
            {
                return new Rotativa.PartialViewAsPdf("_CotizacionPDF", new DocumentosDiagnosticoDTO())
                {
                    FileName = $"Documento diganóstico.pdf",
                    PageSize = Rotativa.Options.Size.A4
                };
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearDocumento(DocumentosDiagnostico modelo)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoCoreBusiness.GuardarAsync(modelo);

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
        public async Task<ActionResult> Delete(string registroID)
        {
            try
            {
                var modelo = await _iDocumentosDiagnosticoCoreBusiness.FindAsync(x => x.StrDocumentoID == registroID);

                if (modelo.BitFinalizado)
                    return Json(new { error = RecursoDocumentosDiagnostico.msnDocumentoFinalizado }, JsonRequestBehavior.AllowGet);

                await _iDocumentosDiagnosticoCoreBusiness.DeleteAsync(modelo);
                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AgregarElementosPendientes(DocumentosDiagnosticoDTO modelo)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoCoreBusiness.AgregarElementosPendientesAsync(modelo);

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
        public async Task<ActionResult> FinalizarDocumento(DocumentosDiagnostico documento)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoCoreBusiness.FinalizarDocumentoAsync(documento);

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