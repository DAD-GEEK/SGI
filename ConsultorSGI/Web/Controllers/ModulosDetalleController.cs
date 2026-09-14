using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class ModulosDetalleController : Controller
    {
        ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        ModulosDetalleCoreBusiness _modulosDetalleCoreBusiness;

        readonly string movimientoCreate = "C";
        readonly string movimientoUpdate = "U";

        public ModulosDetalleController()
        {
            _modulosDetalleCoreBusiness = new ModulosDetalleCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }

        #region Vistas
        [HttpPost]
        public async Task<ActionResult> GetAllModulosDetalleAsync(int moduloID)
        {
            try
            {
                var listaModulos = await _modulosDetalleCoreBusiness.GetAllAsync();
                listaModulos = listaModulos.Where(x => x.IntModuloID == moduloID).ToList();
                return PartialView("_GetAllModulosDetalle", listaModulos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllModulosDetalle", new List<ModulosDetalle>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearModuloDetalle()
        {
            try
            {
                return PartialView("_CrearModuloDetalle");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearModuloDetalle");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetModuloDetalleByEditarAsync(int registroID)
        {
            try
            {
                var usuarioModelo = await _modulosDetalleCoreBusiness.FindAsync(x => x.IntModuloDetalleID == registroID);
                return PartialView("_EditarModuloDetalle", usuarioModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarModuloDetalle", new ModulosDetalle());
            }
        }
        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearModuloDetalleAsync(ModulosDetalle modelo)
        {
            try
            {
                _modulosDetalleCoreBusiness.MovimientoCreate(movimientoCreate);

                var respuesta = await _modulosDetalleCoreBusiness.CreateAsync(modelo);
                if (string.IsNullOrEmpty(respuesta)) return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateModuloDetalleAsync(ModulosDetalle modelo)
        {
            try
            {
                _modulosDetalleCoreBusiness.MovimientoUpdate(movimientoUpdate);

                var respuesta = await _modulosDetalleCoreBusiness.UpdateAsync(modelo);
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
        public async Task<ActionResult> DeleteModuloDetalleAsync(int registroID)
        {
            try
            {
                var modelo = await _modulosDetalleCoreBusiness.FindAsync(x => x.IntModuloDetalleID == registroID);
                await _modulosDetalleCoreBusiness.DeleteAsync(modelo);
                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
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