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
    public class AspNetRolesController : Controller
    {
        #region Inyección de dependencias
        IAspNetRolesCoreBusiness _iAspNetRolesCoreBusiness;
        ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        public AspNetRolesController()
        {
            this._iAspNetRolesCoreBusiness = new AspNetRolesCoreBusiness();
            this._iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas
        [HttpPost]
        public async Task<ActionResult> GetAllRolesAsync()
        {
            try
            {
                var listaRoles = await _iAspNetRolesCoreBusiness.GetAllAsync();
                listaRoles = listaRoles.ToList();
                return PartialView("_GetAllRoles", listaRoles);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllRoles", new List<AspNetRoles>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearRol()
        {
            try
            {
                return PartialView("_CrearRol");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearRol");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetRolByEditarAsync(string registroID)
        {
            try
            {
                var rolModelo = await _iAspNetRolesCoreBusiness.FindAsync(x => x.Id == registroID);
                return PartialView("_EditarRol", rolModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarRol", new AspNetRoles());
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetPermisosModulos(string registroID)
        {
            try
            {
                var rolModelo = await _iAspNetRolesCoreBusiness.FindAsync(x => x.Id == registroID);
                return PartialView("_GetPermisosModulos", rolModelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetPermisosModulos");
            }
        }
        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarRolesAsync(AspNetRoles modelo)
        {
            try
            {
                var respuesta = await _iAspNetRolesCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), rolID = modelo.Id }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteRolAsync(string registroID)
        {
            try
            {
                var esRolPrincipal = _iAspNetRolesCoreBusiness.ValidarSiEsRolPrincipal(registroID);

                if (string.IsNullOrEmpty(esRolPrincipal))
                {
                    var modelo = await _iAspNetRolesCoreBusiness.FindAsync(x => x.Id == registroID);
                    await _iAspNetRolesCoreBusiness.DeleteAsync(modelo);
                    return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = esRolPrincipal }, JsonRequestBehavior.AllowGet);
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