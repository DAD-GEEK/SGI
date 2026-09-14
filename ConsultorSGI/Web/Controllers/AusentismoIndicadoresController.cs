using Across;
using CoreBusiness;
using CoreBusiness.Interfaces;
using DocumentFormat.OpenXml.InkML;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Filters;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class AusentismoIndicadoresController : Controller
    {
        ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness => new LogsExceptionCoreBusiness();
        AusentismoCoreBusiness _ausentismoCoreBusiness;
        IAusentismoIndicadoresCoreBusiness _iAusentismoIndicadoresCoreBusiness;

        public AusentismoIndicadoresController()
        {
            this._ausentismoCoreBusiness = new AusentismoCoreBusiness();
            this._iAusentismoIndicadoresCoreBusiness = new AusentismoIndicadoresCoreBusiness();
        }


        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var listaIndicadores = await _iAusentismoIndicadoresCoreBusiness.ObtenerListaDeTodosLosIndicadoresAsync();
                return PartialView("_GetAll", listaIndicadores);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAll", new List<AusentismoIndicadoresDTO>());
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditarIndicador(string indicadorID)
        {
            try
            {
                var indicador = await _iAusentismoIndicadoresCoreBusiness.FindAsync(x => x.StrId == indicadorID);
                return PartialView("_EditarIndicador", indicador);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarIndicador", new AusentismoIndicadoresDTO());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarIndicador(AusentismoIndicadores modelo)
        {
            try
            {
                var respuesta = await _iAusentismoIndicadoresCoreBusiness.GuardarIndicadorAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), JsonRequestBehavior.AllowGet });

                return Json(new { error = respuesta, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarIndicador", new AusentismoIndicadoresDTO());
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerDatosDeIndicadores_MedicionGeneralAusentismo(int anio, string tipoDeAusentismo)
        {
            try
            {
                var indicadores = await _iAusentismoIndicadoresCoreBusiness.ObtenerDatosDeIndicadores_MedicionGeneralAusentismoAsync(anio, tipoDeAusentismo);
                return Json(new { msn = ResponseType.success.ToString(), indicadores = indicadores, anio = anio, JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, JsonRequestBehavior.AllowGet });
            }
        }
        #region Ausentismo 
        [HttpGet]
        public async Task<ActionResult> ObtenerVistaIndicadores(int anio, string tipoDeAusentismo)
        {
            try
            {
                enumTiposGlobalesDeIncapacidad enumTipoAusentismo = tipoDeAusentismo == Across.Enumeraciones.enumTiposGlobalesDeIncapacidad.EG_AC.ToString() ? Across.Enumeraciones.enumTiposGlobalesDeIncapacidad.EG_AC : Across.Enumeraciones.enumTiposGlobalesDeIncapacidad.AT_EL;

                var listaIndicadores = await _iAusentismoIndicadoresCoreBusiness.ObtenerListaDeIndicadoresDTOPorTipoAsync(enumTipoAusentismo, anio);
                ViewBag.anioActual = anio;
                ViewBag.tipoDeAusentismo = tipoDeAusentismo;

                return PartialView("_ObtenerIndicadores", listaIndicadores);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_ObtenerIndicadores", new List<AusentismoIndicadoresDTO>());
            }
        }

        [HttpGet]
        public async Task<ActionResult> CargarGraficosPorIndicador(string ausentismoCodigoIndicador, string canvasID)
        {
            try
            {
                ViewBag.ausentismoCodigoIndicador = ausentismoCodigoIndicador;
                ViewBag.canvasID = canvasID;
                return PartialView("_GraficosDeFrecuencia");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GraficosDeFrecuencia");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerDatosDeIndicadoresParaGraficar(string ausentismoCodigoIndicador, int anio)
        {
            try
            {
                var informacionIndicador = await _iAusentismoIndicadoresCoreBusiness.ObtenerDatosDeIndicadoresPorEmpresaAsync(anio, ausentismoCodigoIndicador);

                if (string.IsNullOrEmpty(informacionIndicador.InformacionPorEmpresaDTO.StrId))
                    return Json(new { error = "Indicador sin información", JsonRequestBehavior.AllowGet });

                return Json(new { msn = ResponseType.success.ToString(), informacionIndicador = informacionIndicador, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, JsonRequestBehavior.AllowGet });
            }
        }

        #endregion

        #region Indicadores por empresa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarInformacionDeIndicadorPorEmpresa(AusentismoIndicadores_InformacionPorEmpresa modelo)
        {
            try
            {
                var respuesta = await _iAusentismoIndicadoresCoreBusiness.GuardarInformacionDeIndicadorPorEmpresaAsyncAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), id = modelo.StrId }, JsonRequestBehavior.AllowGet);

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