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
    public class Terceros_SistemasDeGestionController : Controller
    {
        #region Variables
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private ITerceros_SistemasDeGestionCoreBusiness _iTerceros_SistemasDeGestionCoreBusiness;

        public Terceros_SistemasDeGestionController()
        {
            _iTerceros_SistemasDeGestionCoreBusiness = new Terceros_SistemasDeGestionCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Terceros_SistemasDeGestion
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllByTerceroIDAsync(int terceroID)
        {
            try
            {
                var listaTerceros_SistemasDeGestion = await _iTerceros_SistemasDeGestionCoreBusiness.ObtenerSistemasDeGestionPorTerceroAsync(terceroID);
                return PartialView("_GetAll", listaTerceros_SistemasDeGestion);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAll", new List<Terceros_SistemasDeGestion>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> Crear()
        {
            try
            {
                ViewBag.listaSistemasDeGestion = await _iTerceros_SistemasDeGestionCoreBusiness.DropDownListSistemasDeGestionAsync();
                ViewBag.listaNiveles = await _iTerceros_SistemasDeGestionCoreBusiness.DropDownListNivelesAsync();
                var modeloSistemasDeGestion = await _iTerceros_SistemasDeGestionCoreBusiness.ObtenerTerceros_SistemasDeGestionPorIDAsync();

                return PartialView("_Crear", modeloSistemasDeGestion);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Crear");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetByEditarAsync(int registroID)
        {
            try
            {
                var modelo = await _iTerceros_SistemasDeGestionCoreBusiness.ObtenerTerceros_SistemasDeGestionPorIDAsync(registroID);

                ViewBag.listaSistemasDeGestion = await _iTerceros_SistemasDeGestionCoreBusiness.DropDownListSistemasDeGestionAsync(modelo.StrCodigoID);

                if (!string.IsNullOrEmpty(modelo.StrNivelID))
                    ViewBag.listaNiveles = await _iTerceros_SistemasDeGestionCoreBusiness.DropDownListNivelesAsync(modelo.StrNivelID);
                else
                    ViewBag.listaNiveles = await _iTerceros_SistemasDeGestionCoreBusiness.DropDownListNivelesAsync();

                return PartialView("_Editar", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Editar", new Terceros_SistemasDeGestion());
            }
        }

        #endregion

        #region Acceso a base de datos        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarAsync(Terceros_SistemasDeGestion modelo, string nivelID)
        {
            try
            {
                var respuesta = await _iTerceros_SistemasDeGestionCoreBusiness.GuardarRegistroAsync(modelo, nivelID);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), regitroID = modelo.IntRegistroID }, JsonRequestBehavior.AllowGet);

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
        public async Task<ActionResult> AplicarNormaAsync(Terceros_SistemasDeGestion_Normas modelo)
        {
            try
            {
                var respuesta = await _iTerceros_SistemasDeGestionCoreBusiness.AplicarNormaAsync(modelo);
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
        public async Task<ActionResult> DeleteAsync(int registroID)
        {
            try
            {
                var respuesta = await _iTerceros_SistemasDeGestionCoreBusiness.EliminarRegistroAsync(registroID);

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
        #endregion
    }
}