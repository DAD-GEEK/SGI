using Models.DTO;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using CoreBusiness.Interfaces;
using CoreBusiness;
using System.Linq;
using Across.CacheStorage;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [RequireHttps]
    [Authorize]
    public class HomeController : Controller
    {
        private ISistemasDeGestionCoreBusiness _iSistemasDeGestionCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private ICacheStorage _iCacheStorage;
        public HomeController()
        {
            this._iSistemasDeGestionCoreBusiness = new SistemasDeGestionCoreBusiness();
            this._iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
            this._iCacheStorage = new CacheStorage();
        }

        public ActionResult Index() => View();

        public ActionResult About()
        {

            _iCacheStorage.Clear(CacheNames.ObtenerSistemasDeGestion);
            _iCacheStorage.Clear(CacheNames.ObtenerSistemasDeGestionPorTercero);
            _iCacheStorage.Clear(CacheNames.ObtenerTodasLasNormas);
            _iCacheStorage.Clear(CacheNames.ObtenerTodosLosNumerales);

            return View();
        }
        public ActionResult ModuloEnMantenimiento() => View();
        public ActionResult AccesoDenegado() => View();
        public ActionResult Error(string mensaje)
        {
            ViewBag.error = mensaje;
            return View();
        }

        public async Task<ActionResult> GetAllSistemasDeGestion()
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

    }
}