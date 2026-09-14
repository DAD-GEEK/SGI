using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness;
using CoreBusiness.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Filters;
using static Across.Enumeraciones;

namespace Web.Controllers
{
    [Authorize]
    public class TercerosController : Controller
    {
        #region Inyección de dependencias
        private ITercerosCoreBusiness _iTercerosCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;

        public TercerosController()
        {
            _iTercerosCoreBusiness = new TercerosCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }

        #endregion

        #region Vistas Terceros
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllTerceros()
        {
            try
            {
                var listaTerceros = await _iTercerosCoreBusiness.FindWhereAsync(x => x.StrIdentificacion != "0");
                return PartialView("_GetAllTerceros", listaTerceros);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllTerceros", new List<Terceros>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearTercero()
        {
            try
            {
                ViewBag.listaCiudades = await _iTercerosCoreBusiness.SelectListCiudadesAsync();
                return PartialView("_CrearTercero");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearTercero");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerPermisosPorTerceroAsync(int terceroID)
        {
            try
            {
                var modelo = await _iTercerosCoreBusiness.ObtenerPermisosPorTerceroAsync(terceroID);
                return PartialView("_PermisosPorTercero", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_PermisosPorTercero", new List<AspNetTerceroRoles>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> ObtenerNormasPorTerceroAsync(int terceroID)
        {
            try
            {
                var modelo = await _iTercerosCoreBusiness.ObtenerNormasPorTerceroAsync(terceroID);
                return PartialView("_NormasPorTercero", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_NormasPorTercero", new List<Terceros_Normas>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> vistaAgregarRolesAsync()
        {
            try
            {
                ViewBag.listaTerceroRoles = await _iTercerosCoreBusiness.SelectListAspNetRolesAsync();
                return PartialView("_AgregarRoles");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_AgregarRoles");
            }
        } 
        
        [HttpPost]
        public async Task<ActionResult> vistaAgregarNormasAsync()
        {
            try
            {
                ViewBag.listaNormas = await _iTercerosCoreBusiness.SelectListNormasAsync();
                return PartialView("_AgregarNormas");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_AgregarNormas");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetTerceroByEditar(int terceroID)
        {
            try
            {
                var modelo = await _iTercerosCoreBusiness.FindAsync(x => x.IntTerceroID == terceroID);
                ViewBag.listaCiudades = await _iTercerosCoreBusiness.SelectListCiudadesAsync(modelo.IntCiudadID.ToString());

                long tamañoImagen = 0;
                if (!string.IsNullOrEmpty(modelo.StrRutaImagen))
                {
                    var rutaImagen = Archivos.ObtenerRutaDeImagenTercero(modelo.StrRutaImagen);
                    tamañoImagen = Archivos.ObtenerTamanoDeArchivo(rutaImagen);
                    modelo.StrRutaImagen = Archivos.GetUrlImagenTercero(modelo.StrRutaImagen);
                }

                ViewBag.tamañoImagen = tamañoImagen;

                return PartialView("_EditarTercero", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarTercero", new Terceros());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarTerceroAsync(Terceros modelo)
        {
            try
            {
                var respuesta = await _iTercerosCoreBusiness.SaveAllAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), terceroID = modelo.IntTerceroID }, JsonRequestBehavior.AllowGet);

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
        public async Task<ActionResult> GuardarInformacionTerceroDocumentoDiagnostico(Terceros modelo)
        {
            try
            {
                var respuesta = await _iTercerosCoreBusiness.GuardarInformacionTerceroDocumentoDiagnosticoAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), terceroID = modelo.IntTerceroID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GuardarImagenAsync(FormCollection collection)
        {
            try
            {
                HttpFileCollectionBase imagenUsuario = HttpContext.Request.Files;

                await _iTercerosCoreBusiness.GuardarImagenAsyn(imagenUsuario, collection);

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarNormaPorTerceroAsync(Terceros_Normas modelo)
        {
            try
            {
                var respuesta = await _iTercerosCoreBusiness.AgregarNormaPorTerceroAsync(modelo);

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
        public async Task<ActionResult> DeleteTerceroAsync(int terceroID)
        {
            try
            {
                var respuesta = await _iTercerosCoreBusiness.EliminarTerceroAsync(terceroID);

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
        public async Task<ActionResult> EliminarNormaPorTerceroAsync(int registroID)
        {
            try
            {
                var respuesta = await _iTercerosCoreBusiness.EliminarNormaPorTerceroAsync(registroID);

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
        public async Task<ActionResult> EliminarImagenAsync(string key)
        {
            try
            {
                int terceroID = Convert.ToInt32(key);
                var respuesta = await _iTercerosCoreBusiness.EliminarImagenAsync(terceroID);

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}