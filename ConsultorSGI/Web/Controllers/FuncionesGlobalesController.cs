using CoreBusiness;
using CoreBusiness.Interfaces;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class FuncionesGlobalesController : Controller
    {
        IFuncionesGlobalesCoreBusiness _iFuncionesGlobalesCoreBusiness => new FuncionesGlobalesCoreBusiness();
        ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness => new LogsExceptionCoreBusiness();

        [HttpPost]
        public async Task<ActionResult> ObtenerNumeralesParaSeleccionarAsync(NumeralesGenericosDTO datosGenericos)
        {
            try
            {
                var modelo = await _iFuncionesGlobalesCoreBusiness.ObtenerNumeralesParaSeleccionarAsync(datosGenericos);
                return PartialView("_SeleccionarNumerales", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_SeleccionarNumerales", new List<NormasDTO>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> VistaExportarPDFOptions(string vistaExportacion)
        {
            try
            {
                var modelo = _iFuncionesGlobalesCoreBusiness.VistaExportarPDFOptions(vistaExportacion);
                return PartialView("_ExportarPDFOpciones", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ExportarPDFOpciones", new ExportarPDFOptionsDTO());
            }
        }


    }
}