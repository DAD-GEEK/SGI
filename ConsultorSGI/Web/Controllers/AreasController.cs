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
    public class AreasController : Controller
    {
        #region Variables
        private IAreasCoreBusiness _iAreasCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public AreasController()
        {
            _iAreasCoreBusiness = new AreasCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Areas
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllAreasAsync(int terceroID)
        {
            try
            {
                var listaAreas = await _iAreasCoreBusiness.FindWhereAsync(x => x.IntTerceroID == terceroID);
                return PartialView("_GetAllAreas", listaAreas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllAreas", new List<Areas>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearArea()
        {
            try
            {
                return PartialView("_CrearArea");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearArea");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAreaByEditarAsync(int areaID)
        {
            try
            {
                var modeloArea = await _iAreasCoreBusiness.FindAsync(x => x.IntAreaID == areaID);
                return PartialView("_EditarArea", modeloArea);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarArea", new Areas());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarAreaAsync(Areas modelo)
        {
            try
            {
                var respuesta = await _iAreasCoreBusiness.SaveAllAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), areaID = modelo.IntAreaID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAreaAsync(int areaID)
        {
            try
            {
                var modelo = await _iAreasCoreBusiness.FindAsync(x => x.IntAreaID == areaID);

                if (modelo.Empleados.Count() != 0) 
                    return Json(new { error = string.Format(RecursoAreas.msnRelacionConEmpleados, modelo.StrCodigo) }, JsonRequestBehavior.AllowGet);

                await _iAreasCoreBusiness.DeleteAsync(modelo);
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