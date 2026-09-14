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
    public class ProcesosController : Controller
    {
        #region Variables
        private IProcesosCoreBusiness _iProcesosCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;


        public ProcesosController()
        {
            _iProcesosCoreBusiness = new ProcesosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Procesos
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllProcesosAsync(int terceroID, bool esVistaGlobal)
        {
            try
            {
                if (terceroID == 0)
                    terceroID = await _iProcesosCoreBusiness.ObtenerTerceroClienteGenericoIDAsync();

                var listaProcesos = await _iProcesosCoreBusiness.FindWhereAsync(x => x.IntTerceroClienteID == terceroID);

                if (esVistaGlobal)
                    return PartialView("_GetAllProcesosGlobal", listaProcesos);

                return PartialView("_GetAllProcesos", listaProcesos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);

                if (esVistaGlobal)
                    return PartialView("_GetAllProcesosGlobal", new List<Procesos>());

                return PartialView("_GetAllProcesos", new List<Procesos>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> VistaObtenerNumeralesPorProcesoAsync(int procesoID, int normaID)
        {
            try
            {
                var listaNumerales = await _iProcesosCoreBusiness.ObtenerNumeralesPorProcesoDTOAsync(procesoID);
                ViewBag.listaNormas = await _iProcesosCoreBusiness.ObtenerTodasLasNormasAsync(normaID);
                ViewBag.listaNumerales = await _iProcesosCoreBusiness.ObtenerNumeralesPorNormaAsync(normaID);

                return PartialView("_ObtenerNumeralesProceso", listaNumerales);

            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerNumeralesProceso", new List<Procesos_NumeralesDTO>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> VistaDropDownNumeralesPorNormaAsync(int normaID)
        {
            try
            {
                ViewBag.listaNumerales = await _iProcesosCoreBusiness.ObtenerNumeralesPorNormaAsync(normaID);
                return PartialView("_ObtenerNumeralesPorNorma");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerNumeralesPorNorma");
            }
        }

        [HttpPost]
        public async Task<ActionResult> VistaAgregarNumeralesModalAsync(int procesoID)
        {
            try
            {
                var modelo = await _iProcesosCoreBusiness.FindAsync(x => x.IntProcesoID == procesoID);
                return PartialView("_AgregarNumerales", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_AgregarNumerales", new Procesos());
            }
        }

        [HttpPost]
        public async Task<ActionResult> VistaCargarDatosGenericosAsync(int procesoID)
        {
            try
            {
                var listaProcesos = await _iProcesosCoreBusiness.ObtenerProcesosGlobalesAsync();

                var procesoActual = await _iProcesosCoreBusiness.FindAsync(x => x.IntProcesoID == procesoID);
                ViewBag.procesoID = procesoID;
                ViewBag.procesoDescripcion = procesoActual.StrDescripcion;

                return PartialView("_CargarDatosGenericos", listaProcesos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CargarDatosGenericos", new Procesos());
            }
        }

        [HttpPost]
        public async Task<ActionResult> VistaSeleccionarProcesosAsync()
        {
            try
            {
                var listaProcesos = await _iProcesosCoreBusiness.ObtenerProcesosGlobalesAsync();
                return PartialView("_SeleccionarProcesos", listaProcesos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_SeleccionarProcesos", new List<Procesos>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearProceso(bool esVistaGlobal)
        {
            try
            {
                if (esVistaGlobal)
                    return PartialView("_CrearProcesoGlobal");

                ViewBag.listaMacroProcesos = _iProcesosCoreBusiness.SelectListMacroProcesosAsync();

                return PartialView("_CrearProceso");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);

                if (esVistaGlobal)
                    return PartialView("_CrearProcesoGlobal");

                return PartialView("_CrearProceso");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetProcesoByEditarAsync(int procesoID, bool esVistaGlobal)
        {
            try
            {
                var modeloProceso = await _iProcesosCoreBusiness.FindAsync(x => x.IntProcesoID == procesoID);
                ViewBag.listaNumerales = await _iProcesosCoreBusiness.MultiSelectListNumeralesPorTerceroAsync(modeloProceso.IntTerceroClienteID, modeloProceso);

                if (esVistaGlobal)
                    return PartialView("_EditarProcesoGlobal", modeloProceso);

                ViewBag.listaMacroProcesos = _iProcesosCoreBusiness.SelectListMacroProcesosAsync(modeloProceso.IntMacroProceso.ToString());

                return PartialView("_EditarProceso", modeloProceso);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                if (esVistaGlobal)
                    return PartialView("_EditarProcesoGlobal");

                return PartialView("_EditarProceso", new Procesos());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarProcesoAsync(Procesos modelo)
        {
            try
            {
                var respuesta = await _iProcesosCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), procesoID = modelo.IntProcesoID, terceroID = modelo.IntTerceroClienteID }, JsonRequestBehavior.AllowGet);

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
        public async Task<ActionResult> SeleccionarProcesoGlobalAsync(Procesos modelo)
        {
            try
            {
                var respuesta = await _iProcesosCoreBusiness.AgregarProcesoGlobalAlTerceroAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), procesoID = modelo.IntProcesoID, terceroID = modelo.IntTerceroClienteID }, JsonRequestBehavior.AllowGet);

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
        public async Task<ActionResult> CargarDatosDeProcesoGlobalAsync(int procesoGlobalID, int procesoActualID)
        {
            try
            {
                var respuesta = await _iProcesosCoreBusiness.CargarDatosDeProcesoAProcesoAsync(procesoGlobalID, procesoActualID);

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
        public async Task<ActionResult> DeleteProcesoAsync(int procesoID)
        {
            try
            {
                var modelo = await _iProcesosCoreBusiness.FindAsync(x => x.IntProcesoID == procesoID);

                if (modelo is null)
                    return Json(new { error = RecursoCommon.msnRegistroNoEncontrado }, JsonRequestBehavior.AllowGet);

                var respuestaEliminar = await _iProcesosCoreBusiness.EliminarProcesoAsync(modelo);

                if (string.IsNullOrEmpty(respuestaEliminar))
                    return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuestaEliminar }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerProcesos_TercerosClientes(string filtro, int terceroClienteID)
        {
            try
            {
                var listaProcesos = _iProcesosCoreBusiness.ObtenerProcesosPorTercerClienteIDAsync(filtro, terceroClienteID);

                return Json(new
                {
                    listaRegistros = listaProcesos
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}