using Across.ArchivosDeRecurso;
using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using Models.DTO;
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
    public class SistemasDeGestionController : Controller
    {
        #region Variables
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private ISistemasDeGestionCoreBusiness _iSistemasDeGestionCoreBusiness;

        public SistemasDeGestionController()
        {
            _iSistemasDeGestionCoreBusiness = new SistemasDeGestionCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }
        #endregion

        #region Vistas Configuración Sistemas de gestión
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        public async Task<ActionResult> DocumentoDiagnosticoPasos(string sistemaDeGestion)
        {
            try
            {
                ViewBag.sistemaDeGestionID = sistemaDeGestion;
                ViewBag.existeSistemaDeGestion = true;

                var sistemaDeGestionModelo = await _iSistemasDeGestionCoreBusiness.FindAsync(x => x.StrCodigoID == sistemaDeGestion);

                if (sistemaDeGestionModelo == null)
                {
                    ViewBag.Error = String.Format(RecursoSistemasDeGestion.msnSistemaDeGestionNoEncontrado, sistemaDeGestion);
                    ViewBag.existeSistemaDeGestion = false;
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return View();
            }

        }

        public async Task<ActionResult> GetAll()
        {
            try
            {
                var listaSistemasDeGestion = await _iSistemasDeGestionCoreBusiness.GetAllAsync();
                return PartialView("_GetAll", listaSistemasDeGestion);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAll", new List<SistemasDeGestion>());
            }
        }
        

        [HttpPost]
        public async Task<ActionResult> Crear()
        {
            try
            {
                return PartialView("_Crear");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Crear");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetByEditar(string registroID)
        {
            try
            {
                var modeloTurno = await _iSistemasDeGestionCoreBusiness.FindAsync(x => x.StrCodigoID == registroID);
                return PartialView("_Editar", modeloTurno);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarTurno", new SistemasDeGestion());
            }
        }

        #endregion

        #region Implementación sistemas de gestión

        [UserAuthenticationFilter]
        public ActionResult Disenio(string sistemaDeGestion)
        {
            ViewBag.sistemaDeGestion = sistemaDeGestion;
            return View();
        }

        public async Task<ActionResult> GetAllPorTercero()
        {
            try
            {
                var listaSistemasDeGestionPorTercero = await _iSistemasDeGestionCoreBusiness.ObtenerSistemasDeGestionPorTerceroEnSesion();

                if (listaSistemasDeGestionPorTercero.Count() == 0)
                    ViewBag.Error = "No hay sistemas de gestión disponibles para la empresa actual. Comuniquese con un asesor de Gestión Integral.";

                return PartialView("_GetAllPorTercero", listaSistemasDeGestionPorTercero);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllPorTercero", new List<SistemasDeGestionDTO>());
            }
        }


        public async Task<ActionResult> GetInformacionPorSistema(string sistemaDeGestion)
        {
            try
            {
                if (string.IsNullOrEmpty(sistemaDeGestion))
                    ViewBag.Error = "No se ha seleccionado un sistema de gestión";


                ViewBag.sistemaDeGestion = sistemaDeGestion;

                return PartialView("_GetInformacionPorSistema");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetInformacionPorSistema", new List<DocumentosDiagnosticoDTO>());
            }
        }
        #endregion

        #region Acceso a base de datos        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Guardar(SistemasDeGestion modelo, string tipoDeAccion)
        {
            try
            {
                var respuesta = await _iSistemasDeGestionCoreBusiness.GuardarRegistroAsync(modelo, tipoDeAccion);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), registroID = modelo.StrCodigoID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string registroID)
        {
            try
            {
                var modelo = await _iSistemasDeGestionCoreBusiness.FindAsync(x => x.StrCodigoID == registroID);
                string respuesta = await _iSistemasDeGestionCoreBusiness.EliminarAsync(modelo);

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