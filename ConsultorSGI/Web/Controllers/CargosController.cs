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
    public class CargosController : Controller
    {
        #region Inyección de dependencias
        private ICargosCoreBusiness _iCargosCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public CargosController()
        {
            _iCargosCoreBusiness = new CargosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Cargos
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllCargosAsync()
        {
            try
            {
                var listaCargos = await _iCargosCoreBusiness.GetAllAsync();
                return PartialView("_GetAllCargos", listaCargos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllCargos", new List<Cargos>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearCargo()
        {
            try
            {
                return PartialView("_CrearCargo");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearCargo");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetCargoByEditarAsync(int cargoID)
        {
            try
            {
                var modeloCargo = await _iCargosCoreBusiness.FindAsync(x => x.IntCargoID == cargoID);
                return PartialView("_EditarCargo", modeloCargo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarCargo", new Cargos());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarCargoAsync(Cargos modelo)
        {
            try
            {
                var respuesta = await _iCargosCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta)) 
                    return Json(new { msn = ResponseType.success.ToString(), cargoID = modelo.IntCargoID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCargoAsync(int cargoID)
        {
            try
            {
                var modelo = await _iCargosCoreBusiness.FindAsync(x => x.IntCargoID == cargoID);

                if (modelo.Empleados.Count() != 0) return Json(new { error = string.Format(RecursoCommon.msnRelacionConEmpleados, modelo.StrCodigo) }, JsonRequestBehavior.AllowGet);

                await _iCargosCoreBusiness.DeleteAsync(modelo);
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