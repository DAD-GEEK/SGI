using Across.ArchivosDeRecurso;
using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Filters;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class PlantillasListasDeVerificacionDetalleController : Controller
    {
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private IPlantillasListasDeVerificacionDetalleCoreBusiness _iPlantillasListasDeVerificacionDetalleCoreBusiness;

        public PlantillasListasDeVerificacionDetalleController()
        {
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
            _iPlantillasListasDeVerificacionDetalleCoreBusiness = new PlantillasListasDeVerificacionDetalleCoreBusiness();
        }

        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllPlantillasListasDeVerificacionDetalleAsync(int procesoID, bool esVistaGlobal, bool esVistaListaDeVerificacion)
        {
            try
            {
                var listaPlantillasDetalle = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.GetAllPlantillasListasDeVerificacionDetalleAsync(procesoID);
                ViewBag.esVistaGlobal = esVistaGlobal;
                ViewBag.esVistaListaDeVerificacion = esVistaListaDeVerificacion;

                return PartialView("_GetAllPlantillaDetalle", listaPlantillasDetalle);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllPlantillaDetalle", new List<PlantillasListasDeVerificacionDetalle>());
            }
        }
              
        [HttpPost]
        public async Task<ActionResult> CrearPlantillaListaDeVerificacionDetalle(int procesoID, string procesoNombre, bool esVistaGlobal)
        {
            try
            {
                ViewBag.listaNumerales = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.SelectListNumeralesAsync();
                ViewBag.procesoID = procesoID;
                ViewBag.procesoNombre = procesoNombre;

                if (esVistaGlobal)
                    return PartialView("_CrearPlantillaDetalle");

                return PartialView("_CrearPlantillaDetalleModal");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearPlantillaDetalle");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerPlantillaListaDeVerificacionParaEditar(int plantillaDetalleID, bool esVistaGlobal)
        {
            try
            {
                var plantillaModelo = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindAsync(x => x.IntPlantillaDetalleID == plantillaDetalleID);
                ViewBag.listaNumerales = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.MultiSelectListNumeralesAsync(plantillaModelo);

                if (!esVistaGlobal)
                    return PartialView("_EditarPlantillaDetalleModal", plantillaModelo);

                return PartialView("_EditarPlantillaDetalle", plantillaModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarPlantillaDetalle", new PlantillasListasDeVerificacionDetalle());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarPlantillaListasDeVerificacionDetalleAsync(PlantillasListasDeVerificacionDetalle modelo, int auditoriaID)
        {
            try
            {
                var respuesta = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.SaveAllAsync(modelo, auditoriaID);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), procesoID = modelo.IntProcesoID, plantillaDetalleID = modelo.IntPlantillaDetalleID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<ActionResult> SeleccionarNumeralAsync(PlantillasListasDeVerificacion_Numerales modelo)
        {
            try
            {
                var respuesta = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.GuardarPlantillaDetalleNumeralAsync(modelo);

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
        public async Task<ActionResult> DeletePlantillaListaDeVerificacionDetalleAsync(int plantillaDetalleID)
        {
            try
            {
                var plantilla = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindAsync(x => x.IntPlantillaDetalleID == plantillaDetalleID);

                if (plantilla.ListasDeVerificacion.ToList().Count() > 0)
                {
                    if (plantilla.ListasDeVerificacion.Any(x => x.BitConformidad) || plantilla.ListasDeVerificacion.Any(x => x.BitNoConformidad) || plantilla.ListasDeVerificacion.Any(x => x.BitObservacion))
                        return Json(new { error = RecursoPlantillasListasDeVerificacion.msnPlantillaUtilizadaEnListaDeVerificacion }, JsonRequestBehavior.AllowGet);
                }

                await _iPlantillasListasDeVerificacionDetalleCoreBusiness.DeleteAsync(plantilla);
                return Json(new { msn = ResponseType.success.ToString(), procesoID = plantilla.IntProcesoID }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CambiarOrdenamientoPlantillasAsync(List<PlantillasListasDeVerificacionDetalle> listaPlantillasOrdenada)
        {
            try
            {
                var respuesta = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.CambiarOrdenamientoPlantillasAsync(listaPlantillasOrdenada);

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



    }
}