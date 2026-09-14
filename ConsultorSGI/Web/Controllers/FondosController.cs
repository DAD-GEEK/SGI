using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class FondosController : Controller
    {
        #region Variables
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private IFondosCoreBusiness _iFondosCoreBusiness;

        public FondosController()
        {
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
            _iFondosCoreBusiness = new FondosCoreBusiness();
        }
        #endregion

        #region Vistas Fondos
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllFondosAsync()
        {
            try
            {
                var listaFondos = await _iFondosCoreBusiness.GetAllAsync();
                return PartialView("_GetAllFondos", listaFondos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllFondos", new List<Fondos>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearFondo()
        {
            try
            {
                ViewBag.listaTiposFondo = await _iFondosCoreBusiness.SelectListTiposFondosAsync();
                return PartialView("_CrearFondo");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearFondo");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetFondoByEditarAsync(int fondoID)
        {
            try
            {
                var modeloFondo = await _iFondosCoreBusiness.FindAsync(x => x.IntFondoID == fondoID);
                ViewBag.listaTiposFondo = await _iFondosCoreBusiness.SelectListTiposFondosAsync(modeloFondo.IntTipoFondoID.ToString());
                return PartialView("_EditarFondo", modeloFondo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarFondo", new Fondos());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarFondoAsync(Fondos modelo)
        {
            try
            {
                var respuesta = await _iFondosCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), fondoID = modelo.IntFondoID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFondoAsync(int fondoID)
        {
            try
            {
                var modelo = await _iFondosCoreBusiness.FindAsync(x => x.IntFondoID == fondoID);

                var mensajeValidacionRelaciones = _iFondosCoreBusiness.ValidarRelacionesConEntidades(modelo);
                if (!string.IsNullOrEmpty(mensajeValidacionRelaciones)) return Json(new { error = mensajeValidacionRelaciones }, JsonRequestBehavior.AllowGet);

                await _iFondosCoreBusiness.DeleteAsync(modelo);
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