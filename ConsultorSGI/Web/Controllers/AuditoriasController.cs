using Across;
using CoreBusiness;
using CoreBusiness.Interfaces;
using CoreBusiness.Servicios;
using DataAccess.Servicios;
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
    public class AuditoriasController : Controller
    {
        #region Variables
        private IAuditoriasCoreBusiness _iAuditoriasCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private ICommonCoreBusiness _iCommonCoreBusiness;

        public AuditoriasController()
        {
            _iAuditoriasCoreBusiness = new AuditoriasCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
            _iCommonCoreBusiness = new CommonCoreBusiness();
        }
        #endregion

        #region Vistas Auditorias
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllAuditoriasAsync()
        {
            try
            {
                var usuarioEnSesion = ServicioUsuario.ObtenerDatosDeUsuarioEnSesion;

                var listaAuditorias = await _iAuditoriasCoreBusiness.FindWhereAsync(x => x.Terceros_Clientes.Terceros.IntTerceroID == usuarioEnSesion.TerceroID);
                return PartialView("_GetAllAuditorias", listaAuditorias);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllAuditorias", new List<Auditorias>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearPlanAuditoria()
        {
            try
            {
                int auditoriaID = 0;
                ViewBag.listaNormas = await _iAuditoriasCoreBusiness.MultiSelectListTodasLasNormasAsync(auditoriaID);
                ViewBag.listaUsuarios = await _iAuditoriasCoreBusiness.SelectListUsuariosAsync();
                return PartialView("_CrearPlanAuditoria");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearPlanAuditoria");
            }
        }

        public async Task<ActionResult> GetAllProcesosAuditoriaAsync(int terceroClienteID, int auditoriaID)
        {
            try
            {
                var listaProcesosDTO = await _iAuditoriasCoreBusiness.ObtenerProcesosAuditoriaDTOAsync(terceroClienteID, auditoriaID);
                return PartialView("_GetAllProcesos", listaProcesosDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllProcesos", new List<ProcesosDTO>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAllDetallePlanAuditoriasAsync(int auditoriaID)
        {
            try
            {
                return PartialView("_GetAllDetallePlanAuditoria");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllDetallePlanAuditoria", new List<Auditorias>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAuditoriaByEditarAsync(int auditoriaID)
        {
            try
            {
                var modeloAuditoria = await _iAuditoriasCoreBusiness.FindAsync(x => x.IntAuditoriaID == auditoriaID);
                ViewBag.listaTerceros = _iCommonCoreBusiness.SeleccionarRegistroEnDropDownList(modeloAuditoria.IntTerceroClienteID.ToString(), modeloAuditoria.Terceros_Clientes.StrNombre);
                ViewBag.listaUsuarios = await _iAuditoriasCoreBusiness.SelectListUsuariosAsync(modeloAuditoria.StrUsuarioFirma.ToString());
                ViewBag.diasAuditoria = modeloAuditoria.DatFechaFinal.Subtract(modeloAuditoria.DatFechaInicial).TotalDays + 1;
                ViewBag.listaNormas = await _iAuditoriasCoreBusiness.MultiSelectListTodasLasNormasAsync(auditoriaID);


                return PartialView("_EditarPlanAuditoria", modeloAuditoria);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarPlanAuditoria", new Auditorias());
            }
        }


        [HttpPost]
        public async Task<ActionResult> CrearListaDeVerificacion(int auditoriaID)
        {
            try
            {
                ViewBag.listaProcesos = await _iAuditoriasCoreBusiness.SelectListAuditoriaProcesosAsync(auditoriaID);
                return PartialView("_ListaDeVerificacionProcesos");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ListaDeVerificacionProcesos");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerInformesDeAuditoria(int auditoriaID)
        {
            try
            {
                var modelo = await _iAuditoriasCoreBusiness.FindAsync(x => x.IntAuditoriaID == auditoriaID);

                var usuarioEnSesion = ServicioUsuarioCoreBusiness.ObtenerDatosDeUsuarioEnSesion;
                ViewBag.usuarioActualNombre = usuarioEnSesion.UsuarioNombre;
                ViewBag.usuarioModelo = await _iAuditoriasCoreBusiness.ObtenerUsuarioQueFirmaAsync(modelo.StrUsuarioFirma);
                return PartialView("_InformesAuditoria", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_InformesAuditoria");
            }
        }

        public async Task<ActionResult> DescargarInformeDeAuditoriaEnPDFAsync(int auditoriaID, bool isVistaInforme = false)
        {
            try
            {
                var modelo = await _iAuditoriasCoreBusiness.ObtenerInformePlanDeAuditoriaPDFAsync(auditoriaID);
                ViewBag.isVistaInforme = isVistaInforme;

                var fechaAcual = Common.ObtenerFechaActualExacta();
                var fechaDeGeneracion = Common.ObtenerFechaLarga(fechaAcual);

                return new Rotativa.PartialViewAsPdf("_ExportarInformesAuditoriaPDF", modelo)
                {
                    CustomSwitches = $"--footer-center \" Informe de auditoría N° {modelo.Auditoria.IntConsecutivo} - Fecha de generación: {fechaDeGeneracion} - Página: [page]/[toPage]\" --footer-line --footer-font-size \"8\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                    FileName = $"HALLAZGOS DE AUDITORÍA N° {modelo.Auditoria.IntConsecutivo}_{modelo.Auditoria.Terceros_Clientes.StrNombre}.pdf",
                    PageSize = Rotativa.Options.Size.A4
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ActionResult> DescargarInformePlanAuditoriaPDFAsync(int auditoriaID, string orientacionPDF)
        {
            try
            {
                var modelo = await _iAuditoriasCoreBusiness.ObtenerInformePlanAuditoriaPDFAsync(auditoriaID);

                var fechaAcual = Common.ObtenerFechaActualExacta();
                var fechaDeGeneracion = Common.ObtenerFechaLarga(fechaAcual);

                if (orientacionPDF.ToLower() == enumOpcionesPDF.Horizontal.ToString().ToLower())
                    return new Rotativa.PartialViewAsPdf("_ExportarInformesPlanAuditoriaPDF", modelo)
                    {
                        CustomSwitches = $"--footer-center \"Plan de auditoría N° {modelo.Auditoria.IntConsecutivo} - Fecha de generación: {fechaDeGeneracion} - Página: [page]/[toPage]\" --footer-line --footer-font-size \"8\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                        FileName = $"PLAN DE AUDITORÍA N° {modelo.Auditoria.IntConsecutivo}_{modelo.Auditoria.Terceros_Clientes.StrNombre}.pdf",
                        PageOrientation = Rotativa.Options.Orientation.Landscape
                    };
                else
                    return new Rotativa.PartialViewAsPdf("_ExportarInformesPlanAuditoriaPDF", modelo)
                    {
                        CustomSwitches = $"--footer-center \"Plan de auditoría N° {modelo.Auditoria.IntConsecutivo} - Fecha de generación: {fechaDeGeneracion} - Página: [page]/[toPage]\" --footer-line --footer-font-size \"8\" --footer-spacing 1 --footer-font-name \"Segoe UI\"",
                        FileName = $"PLAN DE AUDITORÍA N° {modelo.Auditoria.IntConsecutivo}_{modelo.Auditoria.Terceros_Clientes.StrNombre}.pdf",
                        PageSize = Rotativa.Options.Size.A4
                    };

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarAuditoriaAsync(Auditorias modelo)
        {
            try
            {
                var respuesta = await _iAuditoriasCoreBusiness.SaveAllAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), auditoriaID = modelo.IntAuditoriaID }, JsonRequestBehavior.AllowGet);

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
        public async Task<ActionResult> GuardarInformeDeAuditoriaAsync(Auditorias modelo)
        {
            try
            {
                var auditoriaModelo = await _iAuditoriasCoreBusiness.FindAsync(x => x.IntAuditoriaID == modelo.IntAuditoriaID);
                auditoriaModelo.StrFortalezas = modelo.StrFortalezas;
                auditoriaModelo.StrConclusiones = modelo.StrConclusiones;
                auditoriaModelo.BitEstado = modelo.BitEstado;

                string respuesta = await _iAuditoriasCoreBusiness.GuardarInformeDeAuditoriaAsync(auditoriaModelo);

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
        public async Task<ActionResult> DeleteAuditoriaAsync(int AuditoriaID)
        {
            try
            {
                var modelo = await _iAuditoriasCoreBusiness.FindAsync(x => x.IntAuditoriaID == AuditoriaID);

                await _iAuditoriasCoreBusiness.DeleteAsync(modelo);
                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SeleccionarProcesoParaAuditar(int auditoriaID, int procesoID)
        {
            try
            {
                var respuesta = await _iAuditoriasCoreBusiness.SeleccionarProcesoParaAuditarAsync(auditoriaID, procesoID);

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
        public async Task<ActionResult> GetPaginacionAuditorias()
        {
            try
            {
                var datatableParamsDTO = _iCommonCoreBusiness.GetParametrosDataTable(Request);
                var listaTerceros = _iAuditoriasCoreBusiness.GetPaginacionAuditorias(datatableParamsDTO);

                datatableParamsDTO.recordsTotal = _iAuditoriasCoreBusiness.TotalRegistrosDataTable;

                return Json(new
                {
                    draw = datatableParamsDTO.draw,
                    recordsFiltered = datatableParamsDTO.recordsTotal,
                    recordsTotal = datatableParamsDTO.recordsTotal,
                    data = listaTerceros
                });

            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new
                {
                    draw = 0,
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<TercerosDTO>()
                });
            }
        }

        #endregion
    }
}