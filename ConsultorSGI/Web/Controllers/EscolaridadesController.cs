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
    public class EscolaridadesController : Controller
    {
        #region Variables
        private IEscolaridadesCoreBusiness _iEscolaridadesCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public EscolaridadesController()
        {
            _iEscolaridadesCoreBusiness = new EscolaridadesCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Escolaridades
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllEscolaridadesAsync()
        {
            try
            {
                var listaEscolaridades = await _iEscolaridadesCoreBusiness.GetAllAsync();
                return PartialView("_GetAllEscolaridades", listaEscolaridades);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllEscolaridades", new List<Escolaridades>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearEscolaridad()
        {
            try
            {
                return PartialView("_CrearEscolaridad");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearEscolaridad");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetEscolaridadByEditarAsync(int escolaridadID)
        {
            try
            {
                var modeloEscolaridad = await _iEscolaridadesCoreBusiness.FindAsync(x => x.IntEscolaridadID == escolaridadID);
                return PartialView("_EditarEscolaridad", modeloEscolaridad);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarEscolaridad", new Escolaridades());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarEscolaridadAsync(Escolaridades modelo)
        {
            try
            {
                var respuesta = await _iEscolaridadesCoreBusiness.SaveAllAsync(modelo);
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
        public async Task<ActionResult> DeleteEscolaridadAsync(int escolaridadID)
        {
            try
            {
                var modelo = await _iEscolaridadesCoreBusiness.FindAsync(x => x.IntEscolaridadID == escolaridadID);
                if (modelo.Empleados.Count() != 0) return Json(new { error = string.Format(RecursoCommon.msnRelacionConAusentismo, modelo.StrCodigo) }, JsonRequestBehavior.AllowGet);

                await _iEscolaridadesCoreBusiness.DeleteAsync(modelo);
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