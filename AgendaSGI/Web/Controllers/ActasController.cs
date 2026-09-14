using Across.ArchivosDeRecurso;
using CoreBusiness;
using Models;
using Newtonsoft.Json;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;


namespace Web.Controllers
{
    public class ActasController : Controller
    {
        ClientesCoreBusiness _clientesCoreBusiness;
        ActasCoreBusiness _actasCoreBusiness;
        AgendaCoreBusiness _agendaCoreBusiness;
        AsistentesActaCoreBusiness _asistentesActaCoreBusiness;
        TemasActaCoreBusiness _temasActaCoreBusiness;
        ActividadesActaCoreBusiness _actividadesActaCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;
        TipoEventosCoreBusiness _tipoEventosCoreBusiness;

        #region Vistas

        [AuthorizeUser]
        public async Task<ActionResult> Index()
        {
            _clientesCoreBusiness = new ClientesCoreBusiness();

            var clientes = (from c in await _clientesCoreBusiness.GetAllAsync()
                            where c.OpcEstado == true
                            orderby c.StrIdentificacion
                            select new { ClienteID = c.IntClienteID, Nombre = c.StrIdentificacion + " - " + c.StrNombre });

            ViewBag.listaClientes = new SelectList(clientes, "ClienteID", "Nombre");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetAllActasByCliente(int clienteID)
        {
            try
            {
                _actasCoreBusiness = new ActasCoreBusiness();
                var modeloActas = await _actasCoreBusiness.FindWhereAsync(x => x.Agenda.IntClienteID == clienteID);
                return PartialView("_GetAllActasByCliente", modeloActas);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_GetAllTemas");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GenerarActa(int AgendaID)
        {
            try
            {
                _actasCoreBusiness = new ActasCoreBusiness();
                _agendaCoreBusiness = new AgendaCoreBusiness();

                var modeloActas = await _actasCoreBusiness.FindAsync(x => x.IntAgendaID == AgendaID);
                var modeloAgenda = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == AgendaID);
                int clienteID = modeloAgenda.IntClienteID;

                if (modeloAgenda.TipoEventos.OpcSoporte == true)
                {
                    ViewBag.Error = RecursoActas.msnSoporte;
                    return PartialView("_ErrorEnModal");
                }

                DateTime? fecha = await _actasCoreBusiness.ValidarSiguienteVisita(AgendaID);
                if (fecha != null) ViewBag.NextVisita = fecha;
                else ViewBag.NextVisita = RecursoActas.msnVisitas;

                if (modeloActas == null)
                {
                    var consecutivoActa = await _actasCoreBusiness.GetConsecutivoActa(modeloAgenda);

                    await _actasCoreBusiness.CreateAsync(new Actas()
                    {
                        IntAgendaID = modeloAgenda.IntAgendaID,
                        IntConsecutivo = consecutivoActa
                    });

                    modeloActas = await _actasCoreBusiness.FindAsync(x => x.IntAgendaID == modeloAgenda.IntAgendaID);

                }

                var fechaActaAnterior = DateTime.Now;

                ViewBag.Consecutivo = modeloActas.IntConsecutivo;
                ViewBag.ActaID = modeloActas.IntActaID;

                return PartialView("_EditarActa", modeloAgenda);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> AgregarAsistentes()
        {
            try
            {
                return PartialView("_AgregarAsistentes");
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_AgregarAsistentes");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAllAsistentes(string actaID)
        {
            try
            {
                _asistentesActaCoreBusiness = new AsistentesActaCoreBusiness();

                int id = Convert.ToInt32(actaID);

                var modelo = await _asistentesActaCoreBusiness.FindWhereAsync(x => x.IntActaID == id);

                return PartialView("_GetAllAsistentes", modelo);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_GetAllAsistentes");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAllTemas(string actaID)
        {
            try
            {
                _temasActaCoreBusiness = new TemasActaCoreBusiness();

                int id = Convert.ToInt32(actaID);

                var modelo = await _temasActaCoreBusiness.GetAllAsync();
                modelo = modelo.Where(x => x.IntActaID == id).ToList();

                foreach (var item in modelo)
                {
                    try
                    {
                        item.StrDesarrollo = Regex.Unescape(item.StrDesarrollo);
                    }
                    catch (Exception)
                    {
                        item.StrDesarrollo = item.StrDesarrollo;
                    }
                }

                return PartialView("_GetAllTemas", modelo);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_GetAllTemas");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAllActividades(string actaID)
        {
            try
            {
                _actividadesActaCoreBusiness = new ActividadesActaCoreBusiness();

                int id = Convert.ToInt32(actaID);

                var modelo = await _actividadesActaCoreBusiness.GetAllAsync();
                modelo = modelo.Where(x => x.IntActaID == id).ToList();

                foreach (var item in modelo)
                {
                    try
                    {
                        item.StrDescripcion = Regex.Unescape(item.StrDescripcion);
                    }
                    catch (Exception)
                    {
                        item.StrDescripcion = item.StrDescripcion;

                    }
                }

                return PartialView("_GetAllActividades", modelo);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_GetAllActividades");
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAllCompromisosActaAnterior(int actaID)
        {
            try
            {
                _actasCoreBusiness = new ActasCoreBusiness();

                var listaCompromisosAnteriores = await _actasCoreBusiness.ObtenerUltimosCompromisosAsync(actaID);
                if (listaCompromisosAnteriores.Count() != 0)
                    return PartialView("_GetAllCompromisos", listaCompromisosAnteriores);

                return PartialView("_GetAllCompromisos", new List<ActividadesActa>());

            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_GetAllCompromisos");
            }
        }


        [HttpPost]
        public async Task<ActionResult> GetActividadesForEditarAsync(ActividadesActa modelo)
        {
            try
            {
                return PartialView("_EditarActividades", modelo);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                ViewBag.Error = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return PartialView("_AgregarAsistentes");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetVistaPreviaActa(int AgendaID)
        {
            try
            {
                _actasCoreBusiness = new ActasCoreBusiness();
                _agendaCoreBusiness = new AgendaCoreBusiness();

                var modelo = await _actasCoreBusiness.FindAsync(x => x.IntAgendaID == AgendaID);
                if (modelo != null)
                    modelo = await _actasCoreBusiness.UnescapeModelo(modelo);

                var modeloAgenda = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == AgendaID);

                if (modeloAgenda.TipoEventos.OpcSoporte == true)
                {
                    ViewBag.Error = RecursoActas.msnSoporte;
                    return PartialView("_ErrorEnModal");
                }
                ;

                DateTime? fecha = await _actasCoreBusiness.ValidarSiguienteVisita(AgendaID);
                if (fecha != null)
                    ViewBag.NextVisita = fecha;
                else
                    ViewBag.NextVisita = RecursoActas.msnVisitas;

                var listaCompromisosAnteriores = await _actasCoreBusiness.ObtenerUltimosCompromisosAsync(modelo.IntActaID);
                ViewBag.Compromisos = listaCompromisosAnteriores;

                if (modelo == null)
                    return Json(new { error = RecursoActas.msnSinActa }, JsonRequestBehavior.AllowGet);

                return PartialView("_GetVistaPreviaActa", modelo);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Acceso a datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearActaAsync(Actas modelo, string modeloAsistentes, string modeloTemas, string modeloActividades)
        {
            try
            {
                _actasCoreBusiness = new ActasCoreBusiness();

                string retur = await _actasCoreBusiness.CreateAsync(modelo);

                if (string.IsNullOrEmpty(retur))
                {
                    _asistentesActaCoreBusiness = new AsistentesActaCoreBusiness();
                    _temasActaCoreBusiness = new TemasActaCoreBusiness();
                    _actividadesActaCoreBusiness = new ActividadesActaCoreBusiness();

                    var listaAsistentes = JsonConvert.DeserializeObject<List<AsistentesActa>>(modeloAsistentes);
                    var listaTemas = JsonConvert.DeserializeObject<List<TemasActa>>(modeloTemas);
                    var listaActividades = JsonConvert.DeserializeObject<List<ActividadesActa>>(modeloActividades);

                    //Agregar asistentes
                    if (listaAsistentes.Count() != 0)
                    {
                        foreach (var item in listaAsistentes)
                        {
                            item.IntActaID = modelo.IntActaID;
                            await _asistentesActaCoreBusiness.CreateAsync(item);
                        }
                    }

                    //Agregar temas
                    if (listaTemas.Count() != 0)
                    {
                        foreach (var item in listaTemas)
                        {
                            item.IntActaID = modelo.IntActaID;
                            await _temasActaCoreBusiness.CreateAsync(item);
                        }
                    }

                    //Agregar actividades
                    if (listaActividades.Count() != 0)
                    {
                        foreach (var item in listaActividades)
                        {
                            if (item.OpcEjecuta == null)
                            {
                                item.OpcEjecuta = false;
                            }
                            item.IntActaID = modelo.IntActaID;
                            await _actividadesActaCoreBusiness.CreateAsync(item);
                        }
                    }

                    return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = retur }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateActaAsync(Actas modelo, string modeloAsistentes, string modeloTemas, string modeloActividades)
        {
            try
            {
                _asistentesActaCoreBusiness = new AsistentesActaCoreBusiness();
                _temasActaCoreBusiness = new TemasActaCoreBusiness();
                _actividadesActaCoreBusiness = new ActividadesActaCoreBusiness();

                var listaAsistentes = JsonConvert.DeserializeObject<List<AsistentesActa>>(modeloAsistentes);
                var listaTemas = JsonConvert.DeserializeObject<List<TemasActa>>(modeloTemas);
                var listaActividades = JsonConvert.DeserializeObject<List<ActividadesActa>>(modeloActividades);

                //Eliminar asistentes
                var currentAsistentes = await _asistentesActaCoreBusiness.GetAllAsync();
                currentAsistentes = currentAsistentes.Where(x => x.IntActaID == modelo.IntActaID).ToList();

                if (currentAsistentes.Count != 0)
                {
                    foreach (var item in currentAsistentes)
                    {
                        await _asistentesActaCoreBusiness.DeleteAsync(item);
                    }
                }

                //Agregar asistentes
                if (listaAsistentes.Count() != 0)
                {
                    foreach (var item in listaAsistentes)
                    {
                        if (item.OpcAsiste == null)
                        {
                            item.OpcAsiste = false;
                        }
                        item.IntActaID = modelo.IntActaID;
                        await _asistentesActaCoreBusiness.CreateAsync(item);
                    }
                }

                //Eliminar temas
                var currentTemas = await _temasActaCoreBusiness.GetAllAsync();
                currentTemas = currentTemas.Where(x => x.IntActaID == modelo.IntActaID).ToList();

                if (currentTemas.Count != 0)
                {
                    foreach (var item in currentTemas)
                    {
                        await _temasActaCoreBusiness.DeleteAsync(item);
                    }
                }

                //Agregar temas
                if (listaTemas.Count() != 0)
                {
                    foreach (var item in listaTemas)
                    {
                        item.IntActaID = modelo.IntActaID;
                        await _temasActaCoreBusiness.CreateAsync(item);
                    }
                }

                //Eliminar actividades
                var currentActividades = await _actividadesActaCoreBusiness.GetAllAsync();
                currentActividades = currentActividades.Where(x => x.IntActaID == modelo.IntActaID).ToList();

                if (currentActividades.Count != 0)
                {
                    foreach (var item in currentActividades)
                    {
                        await _actividadesActaCoreBusiness.DeleteAsync(item);
                    }
                }

                //Agregar actividades
                if (listaActividades.Count() != 0)
                {
                    foreach (var item in listaActividades)
                    {
                        item.IntActaID = modelo.IntActaID;
                        await _actividadesActaCoreBusiness.CreateAsync(item);
                    }
                }

                return Json(new { msn = "success", actaID = modelo.IntActaID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> DescargarActaPDF(int actaID, string listaCompromisosSeleccionados, string listaActividadesSeleccionadas)
        {
            try
            {
                _actasCoreBusiness = new ActasCoreBusiness();

                var model = await _actasCoreBusiness.FindAsync(x => x.IntActaID == actaID);

                if (model != null)
                    model = await _actasCoreBusiness.UnescapeModelo(model);

                var listaActividadesSeleccionadosSplit = listaActividadesSeleccionadas.Split(',');

                var listaActividadesSeleccionadosString = new List<string>();
                listaActividadesSeleccionadosSplit.ToList().ForEach(item => listaActividadesSeleccionadosString.Add(item));

                model.ActividadesActa = model.ActividadesActa.Where(x => listaActividadesSeleccionadosString.Any(y => y == x.IntActividadID.ToString())).ToList();

                DateTime? fecha = await _actasCoreBusiness.ValidarSiguienteVisita(model.IntAgendaID);

                if (fecha != null) ViewBag.NextVisita = fecha;
                else ViewBag.NextVisita = RecursoActas.msnVisitas;

                var listaCompromisosAnteriores = await _actasCoreBusiness.ObtenerUltimosCompromisosAsync(model.IntActaID);

                var listaCompromisosSeleccionadosSplit = listaCompromisosSeleccionados.Split(',');

                var listaCompromisosSeleccionadosString = new List<string>();
                listaCompromisosSeleccionadosSplit.ToList().ForEach(item => listaCompromisosSeleccionadosString.Add(item));
                listaCompromisosAnteriores = listaCompromisosAnteriores.Where(x => listaCompromisosSeleccionadosString.Any(y => y == x.IntActividadID.ToString())).ToList();

                ViewBag.Compromisos = listaCompromisosAnteriores;

                return new Rotativa.PartialViewAsPdf("_ActaPDF", model);
            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EnviarActaByEmailAsync(int AgendaID, List<string> listaCompromisosSeleccionados, List<string> listaActividadesSeleccionadas, int cantidadIntentos)
        {
            try
            {
                _actasCoreBusiness = new ActasCoreBusiness();
                _agendaCoreBusiness = new AgendaCoreBusiness();
                _temasActaCoreBusiness = new TemasActaCoreBusiness();

                var ActaEmail = await _actasCoreBusiness.FindAsync(x => x.IntAgendaID == AgendaID);
                var agenda = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == AgendaID);
                await _actasCoreBusiness.ActualizarConsecutivoActa(agenda);

                ActaEmail.ActividadesActa = ActaEmail.ActividadesActa.Where(x => listaActividadesSeleccionadas.Any(y => y == x.IntActividadID.ToString())).ToList();

                var temasDelActa = await _temasActaCoreBusiness.FindWhereAsync(x => x.IntActaID == ActaEmail.IntActaID);
                if (temasDelActa.Count() == 0)
                    return Json(new { error = RecursoActas.msnValidarExisteTemas, exc = false }, JsonRequestBehavior.AllowGet);

                if (ActaEmail != null) ActaEmail = await _actasCoreBusiness.UnescapeModelo(ActaEmail);
                DateTime? fecha = await _actasCoreBusiness.ValidarSiguienteVisita(AgendaID);
                if (fecha != null) ViewBag.NextVisita = fecha;
                else ViewBag.NextVisita = RecursoActas.msnVisitas;

                var listaCompromisosActaAnterior = new List<ActividadesActa>();

                if (listaCompromisosSeleccionados != null)
                {
                    listaCompromisosActaAnterior = await _actasCoreBusiness.ObtenerUltimosCompromisosAsync(ActaEmail.IntActaID);
                    listaCompromisosActaAnterior = listaCompromisosActaAnterior.Where(x => listaCompromisosSeleccionados.Any(y => y == x.IntActividadID.ToString())).ToList();
                }

                ViewBag.Compromisos = listaCompromisosActaAnterior;

                //Convertir PDF a array de bytes
                var pdfResult = new PartialViewAsPdf("_ActaPDF", ActaEmail);
                var arrayDeBytes = pdfResult.BuildFile(this.ControllerContext);

                string retur = await _actasCoreBusiness.EnviarActaByEmailAsync(ActaEmail, arrayDeBytes);

                if (string.IsNullOrEmpty(retur))
                {
                    if (agenda.OpcEnviado != true)
                    {
                        agenda.OpcEnviado = true;
                        await _agendaCoreBusiness.UpdateAsync(agenda);
                    }

                    return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = retur, exc = false }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }

                string mensaje = string.Empty;

                if (ex.Message.Contains("the server is too busy"))
                {
                    if (cantidadIntentos < 3)
                    {
                        cantidadIntentos++;
                        await this.EnviarActaByEmailAsync(AgendaID, listaCompromisosSeleccionados, listaActividadesSeleccionadas, cantidadIntentos);
                    }
                    else
                    {
                        mensaje = "El servidor de correos se encuentra ocupado en este momento, intentelo nuevamente mas tarde.";
                        return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
                    }
                }

                mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTemasByIDAsync(int temaID)
        {
            try
            {
                _temasActaCoreBusiness = new TemasActaCoreBusiness();

                var temaModelo = await _temasActaCoreBusiness.FindAsync(x => x.IntTemaID == temaID);
                int actaID = 0;

                if (temaModelo != null)
                {
                    actaID = (int)temaModelo.IntActaID;
                    await _temasActaCoreBusiness.DeleteAsync(temaModelo);
                }

                return Json(new { msn = "success", actaID = actaID }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _logsExCoreBusiness = new LogsExCoreBusiness();
                var usuarioID = 0;
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];
                if (usuarioSession != null)
                {
                    usuarioID = usuarioSession.IntUsuarioID;
                }
                string mensaje = await _logsExCoreBusiness.GenerarLogException(ex, usuarioID);
                return Json(new { error = mensaje, exc = true }, JsonRequestBehavior.AllowGet);
            }
        }




        #endregion
    }
}