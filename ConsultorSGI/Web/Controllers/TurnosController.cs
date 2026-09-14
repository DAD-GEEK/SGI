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
    public class TurnosController : Controller
    {
        #region Variables
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private ITurnosCoreBusiness _iTurnosCoreBusiness;

        public TurnosController()
        {
            _iTurnosCoreBusiness = new TurnosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Turnos
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllTurnosAsync()
        {
            try
            {
                var listaTurnos = await _iTurnosCoreBusiness.GetAllAsync();
                return PartialView("_GetAllTurnos", listaTurnos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllTurnos", new List<Turnos>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearTurno()
        {
            try
            {
                return PartialView("_CrearTurno");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearTurno");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetTurnoByEditarAsync(int turnoID)
        {
            try
            {
                var modeloTurno = await _iTurnosCoreBusiness.FindAsync(x => x.IntTurnoID == turnoID);
                return PartialView("_EditarTurno", modeloTurno);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarTurno", new Turnos());
            }
        }

        #endregion

        #region Acceso a base de datos        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarTurnoAsync(Turnos modelo)
        {
            try
            {
                var respuesta = await _iTurnosCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta)) return Json(new { msn = ResponseType.success.ToString(), registroID = modelo.IntTurnoID }, JsonRequestBehavior.AllowGet);
                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTurnoAsync(int turnoID)
        {
            try
            {
                var modelo = await _iTurnosCoreBusiness.FindAsync(x => x.IntTurnoID == turnoID);

                if (modelo.Empleados.Count() != 0) return Json(new { error = string.Format(RecursoTurnos.msnRelacionConEmpleados, modelo.StrCodigo, modelo.StrDescripcion, modelo.Empleados.FirstOrDefault().StrNombreCompleto, modelo.Empleados.FirstOrDefault().Terceros.StrNombre) }, JsonRequestBehavior.AllowGet);

                await _iTurnosCoreBusiness.DeleteAsync(modelo);
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