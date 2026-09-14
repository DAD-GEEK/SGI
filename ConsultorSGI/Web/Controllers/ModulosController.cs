using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class ModulosController : Controller
    {
        ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        ModulosCoreBusiness _modulosCoreBusiness;

        public ModulosController()
        {
            _modulosCoreBusiness = new ModulosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }

        #region Vistas
        [HttpPost]
        public async Task<ActionResult> GetAllModulosAsync()
        {
            try
            {
                var listaModulos = await _modulosCoreBusiness.GetAllAsync();
                return PartialView("_GetAllModulos", listaModulos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllModulos", new List<Modulos>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearModulo()
        {
            try
            {
                return PartialView("_CrearModulo");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearModulo");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetModuloByEditarAsync(int registroID)
        {
            try
            {
                var usuarioModelo = await _modulosCoreBusiness.FindAsync(x => x.IntModuloID == registroID);
                return PartialView("_EditarModulo", usuarioModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarModulo", new Modulos());
            }
        }
        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearModuloAsync(Modulos modelo)
        {
            try
            {
                var respuesta = await _modulosCoreBusiness.CreateAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), moduloID = modelo.IntModuloID }, JsonRequestBehavior.AllowGet);

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
        public async Task<ActionResult> UpdateModuloAsync(Modulos modelo)
        {
            try
            {
                var respuesta = await _modulosCoreBusiness.UpdateAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), moduloID = modelo.IntModuloID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteModuloAsync(int registroID)
        {
            try
            {
                var modelo = await _modulosCoreBusiness.FindAsync(x => x.IntModuloID == registroID);
                await _modulosCoreBusiness.DeleteAsync(modelo);
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