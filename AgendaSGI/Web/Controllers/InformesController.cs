using CoreBusiness;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    public class InformesController : Controller
    {
        AgendaCoreBusiness _agendaCoreBusiness;
        UsuariosCoreBusiness _usuariosCoreBusiness;
        LogsExCoreBusiness _logsExCoreBusiness;
        InformesCoreBusiness _informesCoreBusiness;

        [AuthorizeUser]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetBarrasHorasByAnoByAsesor(int ano)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();

                int añoActual = ano;

                var listaEventos = await _agendaCoreBusiness.GetAllAsync();
                var eventos = listaEventos.ToList().Where(x => x.DatFechaInicial.Value.Year == añoActual && x.OpcCancelada != true);

                ViewBag.TotalHoras = decimal.Round(Convert.ToDecimal(eventos.Sum(x => Convert.ToDouble(x.StrHoras))));

                return PartialView("_GetBarrasHorasByAnoByAsesor");
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
                return PartialView("_GetBarrasHorasByAnoByAsesor");
            }

        }

        [HttpPost]
        public async Task<ActionResult> AsesorByHorasAsync(int ano)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                _usuariosCoreBusiness = new UsuariosCoreBusiness();
                List<InformesDTO> listaInformesChart = new List<InformesDTO>();
                decimal contadorTotalHoras = 0;

                var añoActual = ano;

                var eventosAño = await _agendaCoreBusiness.FindWhereAsync(x => x.DatFechaInicial.Value.Year == añoActual && x.DatFechaFinal.Value.Year == añoActual && x.OpcCancelada == false);

                var usuariosEventos = eventosAño.GroupBy(x => x.IntUsuarioID);

                foreach (var item in usuariosEventos)
                {
                    InformesDTO informesChart = new InformesDTO();
                    List<decimal> horasxMes = new List<decimal>();

                    var usuario = await _usuariosCoreBusiness.FindAsync(x => x.IntUsuarioID == item.Key);
                    informesChart.backgroundColor = usuario.StrColor;
                    informesChart.borderColor = "white";
                    informesChart.label = usuario.StrNombre;

                    for (int i = 1; i <= 12; i++)
                    {
                        var eventosxMes = eventosAño.Where(x => x.IntUsuarioID == item.Key && x.DatFechaInicial.Value.Month == i);
                        if (eventosxMes.Count() != 0)
                        {
                            horasxMes.Add(eventosxMes.Sum(x => decimal.Round(Convert.ToDecimal(x.StrHoras),2)));
                            contadorTotalHoras = decimal.Round(contadorTotalHoras + eventosxMes.Sum(x => Convert.ToDecimal(x.StrHoras)),2);
                        }
                        else
                        {
                            horasxMes.Add(0);
                        }
                    }

                    informesChart.data = horasxMes;
                    listaInformesChart.Add(informesChart);
                }

                var jsonChart = JsonConvert.SerializeObject(listaInformesChart);
                ViewBag.TotalHorasYear = contadorTotalHoras;

                return Json(new { msn = "success", data = jsonChart }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> GetHorasContratoVSHorasMensuales(int ano, int mes, string cliente)
        {
            try
            {
                _informesCoreBusiness = new InformesCoreBusiness();

                var informeByCliente = await _informesCoreBusiness.GetHorasContratoVSHorasMensuales(ano, mes, cliente);

                return PartialView("_GetHorasContratoVSHorasMensuales", informeByCliente);
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
                return PartialView("_GetHorasContratoVSHorasMensuales");
            }

        }
    }
}