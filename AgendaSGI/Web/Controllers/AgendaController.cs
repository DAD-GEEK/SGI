using CoreBusiness;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using Web.Filters;
using Across.ArchivosDeRecurso;
using Across;

namespace Web.Controllers
{
    public class AgendaController : Controller
    {
        AgendaCoreBusiness _agendaCoreBusiness;
        ClientesCoreBusiness _clientesCoreBusiness;
        TipoEventosCoreBusiness _tipoEventosCoreBusiness;
        ConfiguracionCoreBusiness _configuracionCoreBusiness;
        UsuariosCoreBusiness _usuariosCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;
        InformesCoreBusiness _informesCoreBusiness;
        ActasCoreBusiness _actasCoreBusiness;

        readonly string admin = "admin";

        #region Vistas
        [AuthorizeUser]

        public async Task<ActionResult> Index()
        {
            try
            {
                _configuracionCoreBusiness = new ConfiguracionCoreBusiness();
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];

                var configuración = await _configuracionCoreBusiness.GetAllAsync();
                var usuario = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == usuarioSession.IntUsuarioID);

                string horaInicial = "00:00:00";
                string horaFinal = "23:59:59";
                string duracionEvento = string.Empty;
                string vistaDefault = "timeGridWeek";

                ViewBag.UsuarioID = usuario.IntUsuarioID;
                ViewBag.ColorAgenda = usuario.StrColor;

                if (configuración.Count() != 0)
                {
                    horaInicial = configuración.FirstOrDefault().StrHoraInicial;
                    horaFinal = configuración.FirstOrDefault().StrHoraFinal;
                    duracionEvento = configuración.FirstOrDefault().IntDuracionDefault.ToString();

                    if (!string.IsNullOrEmpty(configuración.FirstOrDefault().StrVistaAgenda))
                    {
                        vistaDefault = configuración.FirstOrDefault().StrVistaAgenda;
                    }
                }

                ViewBag.HoraInicial = horaInicial;
                ViewBag.HoraFinal = horaFinal;
                ViewBag.DuracionEvento = duracionEvento;
                ViewBag.VistaDefault = vistaDefault;

                var Usuarios = (from u in await _usuariosCoreBusiness.FindWhereAsync(x => x.OpcEstado == true && x.StrCodigo.ToLower() != "admin")
                                where u.OpcEstado == true
                                orderby u.StrCodigo
                                select new { Usuario = u.IntUsuarioID, Descripcion = u.StrNombre });

                ViewBag.listaUsuarios = new SelectList(Usuarios, "Usuario", "Descripcion");

                var clientes = (from c in await _clientesCoreBusiness.FindWhereAsync(x => x.OpcEstado == true)
                                orderby c.StrIdentificacion
                                select new { ClienteID = c.IntClienteID, Nombre = c.StrIdentificacion + " - " + c.StrNombre });

                ViewBag.listaClientes = new SelectList(clientes, "ClienteID", "Nombre");

                var tipoEventos = (from t in await _tipoEventosCoreBusiness.GetAllAsync()
                                   where t.OpcEstado == true
                                   orderby t.StrCodigo
                                   select new { EventoID = t.IntTipoEventoID, Descripcion = t.StrCodigo + " - " + t.StrDescripcion });

                ViewBag.listaTipoEvento = new SelectList(tipoEventos, "EventoID", "Descripcion");


                return View();
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
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> AgregarCita()
        {
            try
            {
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();

                var clientes = (from c in await _clientesCoreBusiness.GetAllAsync()
                                where c.OpcEstado == true
                                orderby c.StrIdentificacion
                                select new { ClienteID = c.IntClienteID, Nombre = c.StrIdentificacion + " - " + c.StrNombre });

                ViewBag.listaClientes = new SelectList(clientes, "ClienteID", "Nombre");

                var tipoEventos = (from t in await _tipoEventosCoreBusiness.GetAllAsync()
                                   where t.OpcEstado == true
                                   orderby t.StrCodigo
                                   select new { EventoID = t.IntTipoEventoID, Descripcion = t.StrCodigo + " - " + t.StrDescripcion });

                ViewBag.listaTipoEvento = new SelectList(tipoEventos, "EventoID", "Descripcion");

                return View("_AgregarCita");
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
        public async Task<ActionResult> GetEvento(int EventoID)
        {
            try
            {
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();
                _agendaCoreBusiness = new AgendaCoreBusiness();

                var model = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == EventoID);

                DateTime? fechaIni = model.DatFechaInicial;
                DateTime? fechaFin = model.DatFechaFinal;
                string fechaIniFormat = string.Empty;
                string fechaFinFormat = string.Empty;

                if (fechaIni.HasValue)
                {
                    fechaIniFormat = fechaIni.Value.ToString("u");
                    fechaFinFormat = fechaFin.Value.ToString("u");
                }

                var clientes = (from c in await _clientesCoreBusiness.GetAllAsync()
                                orderby c.StrIdentificacion
                                where c.OpcEstado == true || c.IntClienteID == model.IntClienteID
                                select new { ClienteID = c.IntClienteID, Nombre = c.StrIdentificacion + " - " + c.StrNombre });

                ViewBag.listaClientes = new SelectList(clientes, "ClienteID", "Nombre", model.IntClienteID);

                var tipoEventos = (from t in await _tipoEventosCoreBusiness.GetAllAsync()
                                   orderby t.StrCodigo
                                   select new { EventoID = t.IntTipoEventoID, Descripcion = t.StrCodigo + " - " + t.StrDescripcion });

                ViewBag.listaTipoEvento = new SelectList(tipoEventos, "EventoID", "Descripcion", model.IntTipoEventoID);
                ViewBag.FechaInicial = fechaIniFormat;
                ViewBag.FechaFinal = fechaFinFormat;

                var isAdmin = false;
                var usuario = (Usuarios)Session["Usuario"];

                if (usuario.StrCodigo.ToLower() == admin.ToLower()) isAdmin = true;

                ViewBag.isAdmin = isAdmin;
                ViewBag.horaMarcada = !string.IsNullOrEmpty(model.DatFechaIngreso.ToString()) ? true : false;

                return View("_GetEvento", model);
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
        public async Task<ActionResult> GetCalendario()
        {
            try
            {
                return View("_GetCalendario");
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
                return PartialView("_GetCalendario");
            }
        }

        [HttpGet]
        [AuthorizeUser]
        public async Task<ActionResult> TiemposAsesorias()
        {
            try
            {
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _clientesCoreBusiness = new ClientesCoreBusiness();

                var Usuarios = (from u in await _usuariosCoreBusiness.FindWhereAsync(x => x.OpcEstado == true && x.StrCodigo.ToLower() != "admin")
                                orderby u.StrCodigo
                                select new { Usuario = u.IntUsuarioID, Descripcion = u.StrNombre });

                ViewBag.listaUsuarios = new SelectList(Usuarios, "Usuario", "Descripcion");

                var clientes = (from c in await _clientesCoreBusiness.FindWhereAsync(x => x.OpcEstado == true)
                                orderby c.StrIdentificacion
                                select new { ClienteID = c.IntClienteID, Nombre = c.StrIdentificacion + " - " + c.StrNombre });

                ViewBag.listaClientes = new SelectList(clientes, "ClienteID", "Nombre");


                return View();
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
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetTiemposAsesoriasAsync(Agenda modelo)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();

                ViewBag.usuarioID = modelo.IntUsuarioID;
                ViewBag.clienteID = modelo.IntClienteID;

                var tiemposAsesoria = await _agendaCoreBusiness.ObtenerTiemposDeAsesoriaAsync(modelo);
                return PartialView("_GetTiemposAsesoria", tiemposAsesoria);
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
                return PartialView("_GetTiemposAsesoria", new TiemposAsesoriasDTO());
            }
        }
        #endregion

        #region Acceso a Base de datos

        [HttpPost]
        public async Task<ActionResult> GetAllProgramacion(string fechaInicio, string fechaFinal, string asesores, string clientes, string tipoEvento)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                Usuarios usuarioSession = (Usuarios)Session["Usuario"];

                DateTime inicio = Convert.ToDateTime(fechaInicio);
                DateTime final = Convert.ToDateTime(fechaFinal);

                var listaAsesores = JsonConvert.DeserializeObject<List<Usuarios>>(asesores);
                var listaCLientes = JsonConvert.DeserializeObject<List<Clientes>>(clientes);
                var listaTipoEvento = JsonConvert.DeserializeObject<List<TipoEventos>>(tipoEvento);

                var eventos = (from a in await _agendaCoreBusiness.FindWhereAsync(x => x.DatFechaInicial >= inicio || x.DatFechaFinal > final)
                               orderby a.IntAgendaID
                               select new
                               {
                                   id = a.IntAgendaID,
                                   titulo = a.Clientes.StrNombre,
                                   descripcion = a.StrDescripcion,
                                   fechaInicial = Convert.ToDateTime(a.DatFechaInicial).ToString("yyyy-MM-dd HH:mm:"),
                                   fechaFinal = Convert.ToDateTime(a.DatFechaFinal).ToString("yyyy-MM-dd HH:mm:"),
                                   allDay = a.OpcAllDay,
                                   backgroundColor = a.OpcCancelada == false ? a.Usuarios.StrColor : "#ACACAC",
                                   clienteID = a.IntClienteID,
                                   clienteNombre = a.Clientes.StrNombre,
                                   tipoEventoID = a.IntTipoEventoID,
                                   tipoEvento = a.TipoEventos.StrDescripcion,
                                   soporte = a.TipoEventos.OpcSoporte,
                                   asesorID = a.Usuarios.IntUsuarioID,
                                   asesor = a.Usuarios.StrNombre,
                                   editable = a.Usuarios.IntUsuarioID != usuarioSession.IntUsuarioID ? false : a.OpcEnviado == true ? false : true,
                                   enviado = a.OpcEnviado,
                                   cancelada = a.OpcCancelada
                               }).ToList();

                if (listaAsesores.Count() != 0)
                    eventos = eventos.Where(x => listaAsesores.Any(z => z.IntUsuarioID == (int)x.asesorID)).ToList();


                if (listaCLientes.Count() != 0)
                    eventos = eventos.Where(x => listaCLientes.Any(z => z.IntClienteID == (int)x.clienteID)).ToList();


                if (listaTipoEvento.Count() != 0)
                    eventos = eventos.Where(x => listaTipoEvento.Any(z => z.IntTipoEventoID == (int)x.tipoEventoID)).ToList();


                var jsonData = Json(new
                {
                    data = eventos,
                    success = true,
                    fechaInicial = inicio.ToString("yyyy-MM-dd"),
                    fechaFinal = final.ToString("yyyy-MM-dd"),
                }, JsonRequestBehavior.AllowGet);

                return jsonData;
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
        public async Task<ActionResult> CrearEventoAsync(Agenda modelo, string fechaInicialCalendario, string fechaFinalCalendario)
        {
            try
            {
                _actasCoreBusiness = new ActasCoreBusiness();
                _agendaCoreBusiness = new AgendaCoreBusiness();
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();
                _configuracionCoreBusiness = new ConfiguracionCoreBusiness();
                _informesCoreBusiness = new InformesCoreBusiness();

                Usuarios usuarioSession = (Usuarios)Session["Usuario"];

                string retur = string.Empty;

                var clienteInfo = await _clientesCoreBusiness.FindAsync(x => x.IntClienteID == modelo.IntClienteID);
                modelo.IntUsuarioID = usuarioSession.IntUsuarioID;
                modelo.StrTitulo = clienteInfo.StrNombre;
                modelo.StrHoras = "0";

                //Obtener horas en campo
                var tipoEvento = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == modelo.IntTipoEventoID);
                if (tipoEvento.OpcSoporte != true)
                    modelo.StrHoras = await _agendaCoreBusiness.ValidarHorasEjecutadas(modelo);

                //Validar si la visita se está programando en horario de otra visita
                retur = await _agendaCoreBusiness.ValidarEventoAsync(modelo);

                if (string.IsNullOrEmpty(retur))
                {
                    retur = await _agendaCoreBusiness.CreateAsync(modelo);

                    if (string.IsNullOrEmpty(retur))
                    {
                        await _actasCoreBusiness.GuardarActaAsync(modelo);

                        var usuarioInfo = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == modelo.IntUsuarioID);
                        var tipoEventoInfo = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == modelo.IntTipoEventoID);

                        EventosFullCalendarDTO modeloEventos = new EventosFullCalendarDTO();

                        modeloEventos.id = modelo.IntAgendaID;
                        modeloEventos.title = modelo.StrTitulo;
                        modeloEventos.start = (DateTime)modelo.DatFechaInicial;
                        modeloEventos.end = (DateTime)modelo.DatFechaFinal;
                        modeloEventos.allDay = (bool)modelo.OpcAllDay;
                        modeloEventos.backgroundColor = usuarioInfo.StrColor;
                        modeloEventos.borderColor = "white";
                        modeloEventos.textColor = "white";
                        modeloEventos.clienteID = modelo.IntClienteID;
                        modeloEventos.clienteNombre = clienteInfo.StrNombre;
                        modeloEventos.tipoEventoID = modelo.IntTipoEventoID;
                        modeloEventos.tipoEvento = tipoEventoInfo.StrDescripcion;
                        modeloEventos.soporte = (bool)tipoEventoInfo.OpcSoporte;
                        modeloEventos.descripcion = tipoEventoInfo.StrDescripcion;
                        modeloEventos.asesorID = modelo.IntUsuarioID;
                        modeloEventos.asesorNombre = usuarioInfo.StrNombre;
                        modeloEventos.enviado = (bool)modelo.OpcEnviado;
                        modeloEventos.cancelada = (bool)modelo.OpcCancelada;
                        modeloEventos.descripcion = modelo.StrDescripcion;

                        //Validar si supero el número de horas contratadas
                        var fechaEvento = modelo.DatFechaInicial.Value;
                        var validarHoras = await _informesCoreBusiness.GetHorasContratoVSHorasMensuales(fechaEvento.Year, fechaEvento.Month, modelo.IntClienteID);

                        var jsonModeloEventos = JsonConvert.SerializeObject(modeloEventos);

                        var existeOtraVisitaEnSemanaParaOtroAsesor = await _agendaCoreBusiness.ValidarSiOtroAsesorProgramoEventoConCliente(fechaInicialCalendario, fechaFinalCalendario, modelo);

                        return Json(new { msn = "success", modeloEventos = jsonModeloEventos, superaHoras = validarHoras, validacionEventoAsesores = existeOtraVisitaEnSemanaParaOtroAsesor }, JsonRequestBehavior.AllowGet);
                    }
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
        public async Task<ActionResult> UpdateEventoAsync(Agenda modelo)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();
                _informesCoreBusiness = new InformesCoreBusiness();
                _actasCoreBusiness = new ActasCoreBusiness();

                Usuarios usuarioSession = (Usuarios)Session["Usuario"];

                string retur = string.Empty;

                var clienteInfo = await _clientesCoreBusiness.FindAsync(x => x.IntClienteID == modelo.IntClienteID);
                modelo.IntUsuarioID = usuarioSession.IntUsuarioID;
                modelo.StrTitulo = clienteInfo.StrNombre;
                modelo.StrHoras = "0";

                //Obtener horas en campo
                var tipoEvento = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == modelo.IntTipoEventoID);
                if (tipoEvento.OpcSoporte != true)
                    modelo.StrHoras = await _agendaCoreBusiness.ValidarHorasEjecutadas(modelo);

                retur = await _agendaCoreBusiness.ValidarEventoAsync(modelo);

                if (string.IsNullOrEmpty(retur))
                {
                    retur = await _agendaCoreBusiness.UpdateAsync(modelo, "*");

                    if (string.IsNullOrEmpty(retur))
                    {
                        await _actasCoreBusiness.GuardarActaAsync(modelo);

                        var usuarioInfo = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == modelo.IntUsuarioID);
                        var tipoEventoInfo = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == modelo.IntTipoEventoID);

                        EventosFullCalendarDTO modeloEventos = new EventosFullCalendarDTO();

                        modeloEventos.id = modelo.IntAgendaID;
                        modeloEventos.title = modelo.StrTitulo;
                        modeloEventos.start = (DateTime)modelo.DatFechaInicial;
                        modeloEventos.end = (DateTime)modelo.DatFechaFinal;
                        modeloEventos.allDay = (bool)modelo.OpcAllDay;
                        modeloEventos.backgroundColor = usuarioInfo.StrColor;
                        modeloEventos.borderColor = "white";
                        modeloEventos.textColor = "white";
                        modeloEventos.clienteNombre = clienteInfo.StrNombre;
                        modeloEventos.tipoEvento = tipoEventoInfo.StrDescripcion;
                        modeloEventos.descripcion = tipoEventoInfo.StrDescripcion;
                        modeloEventos.soporte = (bool)tipoEventoInfo.OpcSoporte;
                        modeloEventos.asesorID = modelo.IntUsuarioID;
                        modeloEventos.asesorNombre = usuarioInfo.StrNombre;
                        modeloEventos.enviado = (bool)modelo.OpcEnviado;
                        modeloEventos.descripcion = modelo.StrDescripcion;
                        modeloEventos.cancelada = (bool)modelo.OpcCancelada;

                        var jsonModeloEventos = JsonConvert.SerializeObject(modeloEventos);

                        //Validar si supero el número de horas contratadas
                        var fechaEvento = modelo.DatFechaInicial.Value;
                        var validarHoras = await _informesCoreBusiness.GetHorasContratoVSHorasMensuales(fechaEvento.Year, fechaEvento.Month, modelo.IntClienteID);

                        return Json(new { msn = "success", modeloEventos = jsonModeloEventos, superaHoras = validarHoras }, JsonRequestBehavior.AllowGet);
                    }
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
        public async Task<ActionResult> UpdateEventoDropResizeAsync(Agenda modelo, string tipo)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                _configuracionCoreBusiness = new ConfiguracionCoreBusiness();
                _informesCoreBusiness = new InformesCoreBusiness();

                //Si cambio de evento de todo el dia, quedará con una hora adicional a la hora inicial 
                if (modelo.OpcAllDay == false && modelo.DatFechaFinal == null)
                {
                    modelo.DatFechaFinal = Convert.ToDateTime(modelo.DatFechaInicial).AddHours(1);
                }

                modelo.StrHoras = await _agendaCoreBusiness.ValidarHorasEjecutadas(modelo);


                string retur = await _agendaCoreBusiness.UpdateAsync(modelo, tipo);

                if (string.IsNullOrEmpty(retur))
                {
                    //Validar si supero el número de horas contratadas
                    var fechaEvento = modelo.DatFechaInicial.Value;
                    var agenda = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == modelo.IntAgendaID);

                    var validarHoras = await _informesCoreBusiness.GetHorasContratoVSHorasMensuales(fechaEvento.Year, fechaEvento.Month, agenda.IntClienteID);
                    return Json(new { msn = "success", superaHoras = validarHoras }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> CancelarEventoAsync(int EventoID)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();

                Usuarios usuarioSession = (Usuarios)Session["Usuario"];

                var retur = string.Empty;

                var modelo = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == EventoID);
                modelo.OpcCancelada = true;

                modelo.StrHoras = "0";

                var clienteInfo = await _clientesCoreBusiness.FindAsync(x => x.IntClienteID == modelo.IntClienteID);
                modelo.IntUsuarioID = usuarioSession.IntUsuarioID;
                modelo.StrTitulo = clienteInfo.StrNombre;

                retur = await _agendaCoreBusiness.UpdateAsync(modelo, "*");

                if (string.IsNullOrEmpty(retur))
                {
                    var usuarioInfo = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == modelo.IntUsuarioID);
                    var tipoEventoInfo = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == modelo.IntTipoEventoID);

                    EventosFullCalendarDTO modeloEventos = new EventosFullCalendarDTO();

                    modeloEventos.id = modelo.IntAgendaID;
                    modeloEventos.title = modelo.StrTitulo;
                    modeloEventos.start = (DateTime)modelo.DatFechaInicial;
                    modeloEventos.end = (DateTime)modelo.DatFechaFinal;
                    modeloEventos.allDay = (bool)modelo.OpcAllDay;
                    modeloEventos.backgroundColor = "#ACACAC";
                    modeloEventos.borderColor = "white";
                    modeloEventos.textColor = "white";
                    modeloEventos.clienteNombre = clienteInfo.StrNombre;
                    modeloEventos.tipoEvento = tipoEventoInfo.StrDescripcion;
                    modeloEventos.descripcion = tipoEventoInfo.StrDescripcion;
                    modeloEventos.soporte = (bool)tipoEventoInfo.OpcSoporte;
                    modeloEventos.asesorID = modelo.IntUsuarioID;
                    modeloEventos.asesorNombre = usuarioInfo.StrNombre;
                    modeloEventos.enviado = (bool)modelo.OpcEnviado;
                    modeloEventos.descripcion = modelo.StrDescripcion;
                    modeloEventos.cancelada = (bool)modelo.OpcCancelada;

                    var jsonModeloEventos = JsonConvert.SerializeObject(modeloEventos);

                    return Json(new { msn = "success", modeloEventos = jsonModeloEventos }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> DeleteEventoAsync(int EventoID)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();

                var modelo = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == EventoID);

                await _agendaCoreBusiness.DeleteAsync(modelo);

                return Json(new { msn = "success" }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> MarcarHoraDeLlegadaAsync(int agendaID)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();

                var agendaModelo = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == agendaID);

                if (agendaModelo is null)
                    return Json(new { error = "No se encontró el recurso" }, JsonRequestBehavior.AllowGet);

                agendaModelo.DatFechaIngreso = common.ConvertirFechaUTCaZonaLocal(DateTime.UtcNow);

                await _agendaCoreBusiness.UpdateAsync(agendaModelo);

                return Json(new { msn = "success", agendaID = agendaID }, JsonRequestBehavior.AllowGet);

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