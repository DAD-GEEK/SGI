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

namespace Web.Controllers
{
    [Authorize]
    public class ConfiguracionPorTerceroController : Controller
    {
        #region Inyección de dependencias
        private IConfiguracionPorTerceroCoreBusiness _iConfiguracionPorTerceroCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public ConfiguracionPorTerceroController()
        {
            _iConfiguracionPorTerceroCoreBusiness = new ConfiguracionPorTerceroCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas ConfiguracionPorTercero
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> VistaInformacionGeneralAsync()
        {
            try
            {
                var terceroModelo = await _iConfiguracionPorTerceroCoreBusiness.ObtenerInformacionDeTerceroEnSesionAsync();

                return PartialView("_MiEmpresa", terceroModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_MiEmpresa", new Terceros());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetConfiguracionPorTerceroAsync()
        {
            try
            {
                var listaConfiguracionPorTercero = await _iConfiguracionPorTerceroCoreBusiness.GetAllAsync();
                return PartialView("_GetConfiguracionPorTercero", listaConfiguracionPorTercero);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetConfiguracionPorTercero", new List<ConfiguracionPorTercero>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearConfiguracionPorTercero()
        {
            try
            {
                return PartialView("_CrearConfiguracionPorTercero");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearConfiguracionPorTercero");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetConfiguracionPorTerceroByEditarAsync(int configuracionPorTerceroID)
        {
            try
            {
                var modeloConfiguracionPorTercero = await _iConfiguracionPorTerceroCoreBusiness.FindAsync(x => x.IntConfiguracionID == configuracionPorTerceroID);
                return PartialView("_EditarConfiguracionPorTercero", modeloConfiguracionPorTercero);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarConfiguracionPorTercero", new ConfiguracionPorTercero());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarConfiguracionPorTerceroAsync(ConfiguracionPorTercero modelo)
        {
            try
            {
                var respuesta = await _iConfiguracionPorTerceroCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta)) return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfiguracionPorTerceroAsync(int configuracionPorTerceroID)
        {
            try
            {
                var modelo = await _iConfiguracionPorTerceroCoreBusiness.FindAsync(x => x.IntConfiguracionID == configuracionPorTerceroID);
               
                await _iConfiguracionPorTerceroCoreBusiness.DeleteAsync(modelo);
                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
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