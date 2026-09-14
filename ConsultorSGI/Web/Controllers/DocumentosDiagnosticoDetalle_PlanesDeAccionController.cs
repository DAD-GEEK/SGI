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
    public class DocumentosDiagnosticoDetalle_PlanesDeAccionController : Controller
    {
        #region Inyección de dependencias
        private IDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness _iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public DocumentosDiagnosticoDetalle_PlanesDeAccionController()
        {
            _iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness = new DocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas DocumentosDiagnosticoDetalle_PlanesDeAccion
 

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Guardar(DocumentosDiagnosticoDetalle_PlanesDeAccion modelo)
        {
            try
            {
                var respuesta = await _iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness.GuardarAsync(modelo);

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