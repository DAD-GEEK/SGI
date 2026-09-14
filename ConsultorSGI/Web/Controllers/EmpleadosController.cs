using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness;
using CoreBusiness.Interfaces;
using DataAccess.Servicios;
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
    public class EmpleadosController : Controller
    {
        private IEmpleadosCoreBusiness _iEmpleadosCoreBusiness;
        private ILogsExceptionCoreBusiness _logsExceptionCoreBusiness;

        private readonly string EPS = enumTiposFondos.EPS.ToString();
        private readonly string AFP = enumTiposFondos.AFP.ToString();
        private readonly string ARL = enumTiposFondos.ARL.ToString();
        public EmpleadosController()
        {
            _iEmpleadosCoreBusiness = new EmpleadosCoreBusiness();
            _logsExceptionCoreBusiness = new LogsExceptionCoreBusiness();
        }

        [UserAuthenticationFilter]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetAllEmpleadosAsync()
        {
            try
            {
                var listaEmpleados = await _iEmpleadosCoreBusiness.GetAllAsync();
                return PartialView("_GetAllEmpleados", listaEmpleados);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _logsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_GetAllEmpleados", new List<Empleados>());
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearEmpleado()
        {
            try
            {
                ViewBag.listaCiudades = await _iEmpleadosCoreBusiness.SelectListCiudadesAsync();
                ViewBag.listaTipoIdentificacion = await _iEmpleadosCoreBusiness.SelectListTipoIdentificacionAsync();
                ViewBag.listaEscolaridad = await _iEmpleadosCoreBusiness.SelectListEscolaridadesAsync();
                ViewBag.listaCentrosDeTrabajo = await _iEmpleadosCoreBusiness.SelectListCentrosDeTrabajoAsync();
                ViewBag.listaEstadoCivil = await _iEmpleadosCoreBusiness.SelectListEstadosCivilesAsync();
                ViewBag.listaCargos = await _iEmpleadosCoreBusiness.SelectListCargosAsync();
                ViewBag.listaTurnos = await _iEmpleadosCoreBusiness.SelectListTurnosAsync();
                ViewBag.listaEps = await _iEmpleadosCoreBusiness.SelectListFondosAsync(EPS);
                ViewBag.listaAfp = await _iEmpleadosCoreBusiness.SelectListFondosAsync(AFP);
                ViewBag.listaArl = await _iEmpleadosCoreBusiness.SelectListFondosAsync(ARL);
                ViewBag.listaEstratos = await _iEmpleadosCoreBusiness.SelectListEstratosAsync();
                ViewBag.listaTiposContrato = await _iEmpleadosCoreBusiness.SelectListTiposContratoAsync();
                ViewBag.listaAreas = await _iEmpleadosCoreBusiness.SelectListAreasAsync();

                return PartialView("_CrearEmpleado", new Empleados());
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _logsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CrearEmpleado", new Empleados());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuardarEmpleadoAsync(Empleados modelo)
        {
            try
            {
                string respuesta = await _iEmpleadosCoreBusiness.GuardarEmpleadoAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                    return Json(new { msn = ResponseType.success.ToString(), empleadoID = modelo.IntEmpleadoID }, JsonRequestBehavior.AllowGet);

                return Json(new { error = respuesta }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _logsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GuardarImagenAsync(FormCollection collection)
        {
            try
            {
                var empleadoID = Convert.ToInt32(collection["empleadoID"].ToString());
                var empleadoModelo = await _iEmpleadosCoreBusiness.FindAsync(x => x.IntEmpleadoID == empleadoID);

                if (empleadoModelo != null)
                {
                    var identificacionEmpleado = collection["empleadoIdentificacion"];
                    HttpFileCollectionBase imagen = HttpContext.Request.Files;
                    if (imagen.Count != 0)
                    {
                        var datosDeUsuario = ServicioUsuario.ObtenerDatosDeUsuarioEnSesion;

                        var rutaImagenAnterior = _iEmpleadosCoreBusiness.ObtenerRutaDeImagenEmpleado(empleadoModelo.StrRutaImagen);
                        _iEmpleadosCoreBusiness.EliminarImagenEmpleado(rutaImagenAnterior);

                        ParamFilesDTO parametros = new ParamFilesDTO();
                        parametros.ruta = Server.MapPath($"~/{Archivos.rutaImagenEmpleado}");
                        parametros.nombreArchivo = $"{identificacionEmpleado}_{empleadoModelo.StrNombreCompleto}";

                        var rutaImagen = _iEmpleadosCoreBusiness.GuardarArchivoEmpleado(imagen, parametros);

                        if (!string.IsNullOrEmpty(rutaImagen))
                        {
                            empleadoModelo.StrRutaImagen = rutaImagen;
                            await _iEmpleadosCoreBusiness.SaveEntityAsync(empleadoModelo);
                        }
                    }
                }

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _logsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EliminarImagenEmpleadoAsync(int key)
        {
            try
            {
                Empleados empleado = await _iEmpleadosCoreBusiness.FindAsync(x => x.IntEmpleadoID == key);

                var rutaImagen = $"{Archivos.rutaImagenEmpleado}/{empleado.StrRutaImagen}";
                var rutaImagenCompleta = Server.MapPath($"~/{rutaImagen}");
                var respuestaEliminar = _iEmpleadosCoreBusiness.EliminarImagenEmpleado(rutaImagenCompleta);

                if (string.IsNullOrEmpty(respuestaEliminar))
                {
                    empleado.StrRutaImagen = string.Empty;
                    await _iEmpleadosCoreBusiness.GuardarEmpleadoAsync(empleado);
                    return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = respuestaEliminar }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _logsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetEmpleadoByEditarAsync(int empleadoID)
        {
            try
            {
                var modelo = await _iEmpleadosCoreBusiness.FindAsync(x => x.IntEmpleadoID == empleadoID);

                ViewBag.listaCiudadNacimiento = await _iEmpleadosCoreBusiness.SelectListCiudadesAsync(modelo.IntCiudadNacimiento.ToString());
                ViewBag.listaCiudadResidencia = await _iEmpleadosCoreBusiness.SelectListCiudadesAsync(modelo.IntCiudadResidencia.ToString());
                ViewBag.listaCiudadLaboral = await _iEmpleadosCoreBusiness.SelectListCiudadesAsync(modelo.IntCiudadLabora.ToString());
                ViewBag.listaTipoIdentificacion = await _iEmpleadosCoreBusiness.SelectListTipoIdentificacionAsync(modelo.IntTipoIdentificacion.ToString());
                ViewBag.listaEscolaridad = await _iEmpleadosCoreBusiness.SelectListEscolaridadesAsync(modelo.IntEscolaridad.ToString());
                ViewBag.listaCentrosDeTrabajo = await _iEmpleadosCoreBusiness.SelectListCentrosDeTrabajoAsync(modelo.IntCentroDeTrabajo.ToString());
                ViewBag.listaEstadoCivil = await _iEmpleadosCoreBusiness.SelectListEstadosCivilesAsync(modelo.IntEstadoCivil.ToString());
                ViewBag.listaCargos = await _iEmpleadosCoreBusiness.SelectListCargosAsync(modelo.IntCargo.ToString());
                ViewBag.listaTurnos = await _iEmpleadosCoreBusiness.SelectListTurnosAsync(modelo.IntTurno.ToString());
                ViewBag.listaEps = await _iEmpleadosCoreBusiness.SelectListFondosAsync(EPS, modelo.IntEPS.ToString());
                ViewBag.listaAfp = await _iEmpleadosCoreBusiness.SelectListFondosAsync(AFP, modelo.IntAFP.ToString());
                ViewBag.listaArl = await _iEmpleadosCoreBusiness.SelectListFondosAsync(ARL, modelo.IntARL.ToString());
                ViewBag.listaEstratos = await _iEmpleadosCoreBusiness.SelectListEstratosAsync(modelo.TIntEstrato.ToString());
                ViewBag.listaTiposContrato = await _iEmpleadosCoreBusiness.SelectListTiposContratoAsync(modelo.IntTipoContrato.ToString());
                ViewBag.listaGrupoSanguineo = _iEmpleadosCoreBusiness.SelectListGrupoSanguineo(modelo.StrGrupoSanguineo);
                ViewBag.listaAreas = await _iEmpleadosCoreBusiness.SelectListAreasAsync(modelo.IntAreaID.ToString());
                ViewBag.nombreImagen = modelo.StrRutaImagen;
                ViewBag.tamañoImagen = _iEmpleadosCoreBusiness.ObtenerTamanoDeArchivo(_iEmpleadosCoreBusiness.ObtenerRutaDeImagenEmpleado(modelo.StrRutaImagen));
                modelo.StrRutaImagen = Archivos.GetUrlImagenEmpleado(modelo.StrRutaImagen);

                return PartialView("_EditarEmpleado", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _logsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_EditarEmpleado", new Terceros());
            }
        }

        [HttpPost]
        public async Task<ActionResult> AbrirVistaCargarArchivos()
        {
            try
            {
                var modeloVista = new InformacionEnVistaDTO();

                modeloVista.Controller = "Empleados";
                modeloVista.Titulo = "Cargar desde Excel";
                modeloVista.Url_Estructura = $"{System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/{Archivos.rutaDescargas}EstructuraEmpleados.xlsx";
                modeloVista.Url_Procesar = $"{modeloVista.Controller}/CargarEmpleadosPorArchivoDeExcelAsync";
                modeloVista.MetodoGetAll_Entidad = $"empleadosCRUD.getAllEmpleadosAsync()";

                return PartialView("_CargarArchivos", modeloVista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = await _logsExceptionCoreBusiness.GenerarLogException(ex);
                return PartialView("_CargarArchivos");
            }
        }


        [HttpPost]
        public async Task<ActionResult> CargarEmpleadosPorArchivoDeExcelAsync(FormCollection collection)
        {
            try
            {
                string respuesatError = string.Empty;

                HttpFileCollectionBase archivoDeExcel = HttpContext.Request.Files;

                if (archivoDeExcel.Count == 0)
                    return Json(new { error = RecursoCommon.msnArchivoNoValido }, JsonRequestBehavior.AllowGet);

                ParamFilesDTO paramFilesDTO = new ParamFilesDTO();
                paramFilesDTO.ruta = Server.MapPath($"~/{Archivos.rutaArchivosTemporales}");

                var respuesta = await _iEmpleadosCoreBusiness.CargarEmpleadosPorArchivoDeExcelAsync(archivoDeExcel, paramFilesDTO);

                if (respuesta.Count() != 0)
                {
                    var novedadesJson = JsonConvert.SerializeObject(respuesta.OrderBy(x => x.NumeroRegistro));
                    return Json(new { novedades = novedadesJson }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        public async Task<ActionResult> DeleteEmpleadoAsync(int empleadoID)
        {
            try
            {
                var modelo = await _iEmpleadosCoreBusiness.FindAsync(x => x.IntEmpleadoID == empleadoID);

                if (modelo.Ausentismo.Count() != 0)
                    return Json(new { error = string.Format(RecursoAusentismo.msnEmpleadoConAusentismo, modelo.StrNombreCompleto) }, JsonRequestBehavior.AllowGet);

                var respuesta = string.Empty;

                if (!string.IsNullOrEmpty(modelo.StrRutaImagen))
                {
                    var rutaImagen = $"{Archivos.rutaImagenEmpleado}/{modelo.StrRutaImagen}";
                    var rutaImagenCompleta = Server.MapPath($"~/{rutaImagen}");

                    respuesta = _iEmpleadosCoreBusiness.EliminarImagenEmpleado(rutaImagenCompleta);
                }
                if (string.IsNullOrEmpty(respuesta)) await _iEmpleadosCoreBusiness.DeleteAsync(modelo);

                return Json(new { msn = ResponseType.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var mensaje = await _logsExceptionCoreBusiness.GenerarLogException(ex);
                return Json(new { error = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}