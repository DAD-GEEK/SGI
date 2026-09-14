using Across;
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
    public class Terceros_ClientesController : Controller
    {
        private ITerceros_ClientesCoreBusiness _iTerceros_ClientesCoreBusiness;
        private ILogsExceptionCoreBusiness _iLogsExceptionCoreBusiness;
        private ICommonCoreBusiness _iCommonCoreBusiness;

        public Terceros_ClientesController()
        {
            _iTerceros_ClientesCoreBusiness = new Terceros_ClientesCoreBusiness();
            _iLogsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
            _iCommonCoreBusiness = new CommonCoreBusiness();
        }

        #region Vistas Terceros_Clientes
        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return PartialView("_GetAll");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAll");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Crear()
        {
            try
            {
                ViewBag.listaCiudades = await _iTerceros_ClientesCoreBusiness.DropDownListCiudadesAsync();
                return PartialView("_Crear");
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Crear");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetByEditar(int id)
        {
            try
            {
                var modelo = await _iTerceros_ClientesCoreBusiness.FindAsync(x => x.IntTerceroClienteID == id);
                ViewBag.listaCiudades = await _iTerceros_ClientesCoreBusiness.DropDownListCiudadesAsync(modelo.IntCiudadID.ToString());

                var identificacionTercero = modelo.Terceros.StrIdentificacion;

                long tamañoImagen = 0;
                if (!string.IsNullOrEmpty(modelo.StrRutaImagen))
                {
                    var rutaImagen = Archivos.ObtenerRutaDeImagenTerceroCliente(identificacionTercero, modelo.StrRutaImagen);
                    tamañoImagen = Archivos.ObtenerTamanoDeArchivo(rutaImagen);
                    modelo.StrRutaImagen = Archivos.GetUrlImagenTerceroClienteOrUrlDefault(identificacionTercero, modelo.StrRutaImagen);
                }

                ViewBag.tamañoImagen = tamañoImagen;

                return PartialView("_Editar", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_Editar", new Terceros_Clientes());
            }
        }

        #endregion

        #region Acceso a base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarAsync(Terceros_Clientes modelo)
        {
            try
            {
                var respuesta = await _iTerceros_ClientesCoreBusiness.GuardarRegistroAsync(modelo);
                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), terceroClienteID = modelo.IntTerceroClienteID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var respuesta = await _iTerceros_ClientesCoreBusiness.EliminarTerceroClienteAsync(id);

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
        public async Task<ActionResult> GetPaginacionTercerosClientes()
        {
            try
            {
                var datatableParamsDTO = _iCommonCoreBusiness.GetParametrosDataTable(Request);
                var listaTercerosClientes = await _iTerceros_ClientesCoreBusiness.GetPaginacionTercerosClientes(datatableParamsDTO);

                datatableParamsDTO.recordsTotal = _iTerceros_ClientesCoreBusiness.TotalRegistrosDataTable;

                return Json(new
                {
                    draw = datatableParamsDTO.draw,
                    recordsFiltered = datatableParamsDTO.recordsTotal,
                    recordsTotal = datatableParamsDTO.recordsTotal,
                    data = listaTercerosClientes
                });

            }
            catch (Exception ex)
            {
                ViewBag.Error = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new
                {
                    draw = 0,
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<TercerosDTO>()
                });
            }
        }


        [HttpGet]
        public async Task<ActionResult> ObtenerTercerosClientes(string filtroTercero)
        {
            try
            {
                var datatableParamsDTO = new DatatableParamsDTO() { searchValue = filtroTercero };

                bool paginarInformacion = false;
                var listaTerceros = await _iTerceros_ClientesCoreBusiness.GetPaginacionTercerosClientes(datatableParamsDTO, paginarInformacion);

                return Json(new
                {
                    listaRegistros = listaTerceros
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GuardarImagenAsync(FormCollection collection)
        {
            try
            {
                HttpFileCollectionBase imagenUsuario = HttpContext.Request.Files;

                await _iTerceros_ClientesCoreBusiness.GuardarImagenAsyn(imagenUsuario, collection);

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _iLogsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EliminarImagenAsync(string key)
        {
            try
            {
                int terceroID = Convert.ToInt32(key);
                var modelo = await _iTerceros_ClientesCoreBusiness.FindAsync(x => x.IntTerceroClienteID == terceroID);

                var respuesta = await _iTerceros_ClientesCoreBusiness.EliminarImagenAsync(modelo);

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