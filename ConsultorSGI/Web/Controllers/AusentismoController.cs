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
using Models.DTO;
using Across;
using System.Web;
using System.Text;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class AusentismoController : Controller
    {
        ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        IAusentismoCoreBusiness _iAusentismoCoreBusiness;

        public AusentismoController()
        {
            _iAusentismoCoreBusiness = new AusentismoCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }

        #region Open vistas
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllAusentismoAsync(DateTime? fechaInicial = null, DateTime? fechaFinal = null)
        {
            try
            {

                var listaAusentismo = await _iAusentismoCoreBusiness.ObtenerListaAusentismoDTOAsync(fechaInicial, fechaFinal);
                return PartialView("_GetAllAutentismo", listaAusentismo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllAusentismo", new List<Ausentismo>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarAusentismo()
        {
            try
            {
                ViewBag.listaEmpleados = await _iAusentismoCoreBusiness.SelectListEmpleadosAsync();
                ViewBag.listaTipoEventos = await _iAusentismoCoreBusiness.SelectListTipoEvento();
                ViewBag.Empleado = await _iAusentismoCoreBusiness.ObtenerEmpleadoConImageUrlAsync(0);

                return PartialView("_RegistrarAusentismo");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_RegistrarAusentismo", new List<Ausentismo>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> AbrirVistaCargarArchivos()
        {
            try
            {
                var modeloVista = new InformacionEnVistaDTO();

                modeloVista.Controller = "Ausentismo";
                modeloVista.Titulo = "Cargar desde Excel";
                modeloVista.Url_Estructura = $"{System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/{Archivos.rutaDescargas}EstructuraAusentismo.xlsx";
                modeloVista.Url_Procesar = $"{modeloVista.Controller}/CargarAusentismoPorArchivoDeExcelAsync";
                modeloVista.MetodoGetAll_Entidad = "ausentismoCRUD.getAllAusentismoAsync()";

                return PartialView("_CargarArchivos", modeloVista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CargarArchivos");
            }
        }


        [HttpPost]
        public async Task<ActionResult> GetDatosEmpleadosAsync(int empleadoID)
        {
            try
            {
                var modeloEmpleado = await _iAusentismoCoreBusiness.ObtenerEmpleadoConImageUrlAsync(empleadoID);
                return PartialView("_GetDatosEmpleado", modeloEmpleado);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetDatosEmpleado", new Empleados());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetCostosAusentismoAsync(int ausentismoID)
        {
            try
            {
                var costosAusentismo = await _iAusentismoCoreBusiness.ObtenerCostosDeAusentismoPorIDAsync(ausentismoID);
                return PartialView("_GetCostosAusentismo", costosAusentismo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetCostosAusentismo", new Empleados());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetProrrogasAsync(int ausentismoID)
        {
            try
            {
                var modelo = await _iAusentismoCoreBusiness.ObtenerProrrogasAsync(ausentismoID);
                return PartialView("_GetProrrogas", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetProrrogas", new List<AusentismoProrrogas>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAusentismoByEditarAsync(int ausentismoID)
        {
            try
            {
                var modeloAusentismo = await _iAusentismoCoreBusiness.FindAsync(x => x.IntAusentismoID == ausentismoID);

                ViewBag.listaTipoEventos = await _iAusentismoCoreBusiness.SelectListTipoEvento(modeloAusentismo.IntTipoEventoAusentismoID.ToString());
                ViewBag.Empleado = await _iAusentismoCoreBusiness.ObtenerEmpleadoConImageUrlAsync((int)modeloAusentismo.IntEmpleadoID);

                return PartialView("_EditarAusentismo", modeloAusentismo);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarAusentismo", new Ausentismo());
            }
        }

        #region Agrupación por año

        [HttpPost]
        public async Task<ActionResult> GetAusentismoAgrupadoMensualmentePorAñoAsync(short anio)
        {
            try
            {
                var ausentismoModelo = await _iAusentismoCoreBusiness.GetAusentismoAgrupadoMensualmentePorAñoAsync(anio);
                ViewBag.anio = anio;

                return PartialView("_GetAusentismoPorAño", ausentismoModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAusentismoPorAño", new List<AusentismoAgrupadoMensualmentePorAnioDTO>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAusentismoAgrupadoMensualmentePorEmpleadoAsync(short año, byte periodo)
        {
            try
            {
                var modeloAusentismoAgrupadoPorEmpleado = await _iAusentismoCoreBusiness.GetAusentismoAgrupadoMensualmentePorEmpleadoAsync(año, periodo);
                return PartialView("_GetAusentismoPorEmpleado", modeloAusentismoAgrupadoPorEmpleado);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAusentismoPorEmpleado", new List<AusentismoAgrupadoPorEmpleadoDTO>());
            }
        }
        #endregion

        #region Arupación por proceso
        [HttpPost]
        public async Task<ActionResult> GetAusentismoAgrupadoPorProcesoAsync(short anio)
        {
            try
            {
                var ausentismoModelo = await _iAusentismoCoreBusiness.GetAusentismoAgrupadoPorProcesoAsync(anio);
                return PartialView("_GetAusentismoPorProceso", ausentismoModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAusentismoPorProceso", new AusentismoAgrupadoPorProcesoDTO());
            }
        }


        //[HttpPost]
        //public async Task<ActionResult> GetAusentismoAgrupadoPorProceso_EmpleadoAsync(short año, int procesoID)
        //{
        //    try
        //    {
        //        var modeloAusentismoAgrupadoPorEmpleado = await _iAusentismoCoreBusiness.GetAusentismoAgrupadoPorProceso_EmpleadoAsync(año, procesoID);
        //        return PartialView("_GetAusentismoPorEmpleado", modeloAusentismoAgrupadoPorEmpleado);
        //    }
        //    catch (Exception ex)
        //    {
        //        var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
        //        return PartialView("_GetAusentismoPorEmpleado", new List<AusentismoAgrupadoPorEmpleadoDTO>());
        //    }
        //}
        #endregion

        #region Agrupación por área
        [HttpPost]
        public async Task<ActionResult> GetAusentismoAgrupadoPorAreaAsync(short anio)
        {
            try
            {
                var ausentismoModelo = await _iAusentismoCoreBusiness.GetAusentismoAgrupadoPorAreaAsync(anio);
                return PartialView("_GetAusentismoPorArea", ausentismoModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAusentismoPorArea", new List<Ausentismo>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAusentismoAgrupadoPorArea_EmpleadoAsync(short año, int areaID)
        {
            try
            {
                var modeloAusentismoAgrupadoPorEmpleado = await _iAusentismoCoreBusiness.GetAusentismoAgrupadoPorArea_EmpleadoAsync(año, areaID);
                return PartialView("_GetAusentismoPorEmpleado", modeloAusentismoAgrupadoPorEmpleado);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAusentismoPorEmpleado", new List<AusentismoAgrupadoPorEmpleadoDTO>());
            }
        }
        #endregion

        #endregion

        #region Acceso a datos
        [HttpPost]
        public async Task<ActionResult> BuscarDiagnosticoAsync(string codigoDiagnostico)
        {
            try
            {
                var modelo = await _iAusentismoCoreBusiness.BuscarDiagnosticoAsync(codigoDiagnostico);

                if (modelo.IntDiagnosticoID != 0)
                    return Json(new { msn = ResponseType.success.ToString(), diagnosticoID = modelo.IntDiagnosticoID, diagnosticoCodigo = modelo.StrCodigo, diagnosticoDescripcion = modelo.StrDescripcion }, JsonRequestBehavior.AllowGet);

                return Json(new { error = string.Format(RecursoDiagnostico.msnRegistroNoExiste, codigoDiagnostico) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AgregarProrrogaAsync(AusentismoProrrogas modelo)
        {
            try
            {
                var respuesta = await _iAusentismoCoreBusiness.AgregarProrrogaAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), ausentismoID = modelo.IntAusentismoID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegistrarAusentismoAsync(Ausentismo modelo)
        {
            try
            {
                var respuesta = await _iAusentismoCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), ausentismoID = modelo.IntAusentismoID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<ActionResult> DeleteAusentismoAsync(int ausentismoID)
        {
            try
            {
                var modelo = await _iAusentismoCoreBusiness.FindAsync(x => x.IntAusentismoID == ausentismoID);

                if (modelo is null)
                    return Json(new { error = RecursoAusentismo.msnAusentismoNoEncontrado }, JsonRequestBehavior.AllowGet);

                await _iAusentismoCoreBusiness.DeleteAsync(modelo);

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProrrogaAsync(int prorrogaID)
        {
            try
            {
                var ausentismoID = await _iAusentismoCoreBusiness.DeleteProrrogaAsync(prorrogaID);
                return Json(new { msn = ResponseType.success.ToString(), ausentismoID = ausentismoID }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CargarAusentismoPorArchivoDeExcelAsync(FormCollection collection)
        {
            try
            {
                string respuesatError = string.Empty;

                HttpFileCollectionBase archivoDeExcel = HttpContext.Request.Files;

                if (archivoDeExcel.Count == 0)
                    return Json(new { error = RecursoCommon.msnArchivoNoValido }, JsonRequestBehavior.AllowGet);

                ParamFilesDTO paramFilesDTO = new ParamFilesDTO();
                paramFilesDTO.ruta = Server.MapPath($"~/{Archivos.rutaArchivosTemporales}");

                var respuesta = await _iAusentismoCoreBusiness.CargarAusentismoDesdeExcelAsync(archivoDeExcel, paramFilesDTO);

                if (respuesta.Count() != 0)
                {
                    var novedadesJson = JsonConvert.SerializeObject(respuesta.OrderBy(x => x.NumeroRegistro));                   
                    return Json(new { novedades = novedadesJson }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}