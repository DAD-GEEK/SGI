using Across.ArchivosDeRecurso;
using Across.Interfaces;
using DataAccess;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace CoreBusiness
{
    public class AgendaCoreBusiness : CRUDGenerico<Agenda>, IAgenda
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        ConfiguracionCoreBusiness _configuracionCoreBusiness;
        TipoEventosCoreBusiness _tipoEventosCoreBusiness;
        UsuariosCoreBusiness _usuariosCoreBusiness;
        ClientesCoreBusiness _clientesCoreBusiness;

        string drop = "Drop";
        string resize = "Resize";

        public AgendaCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Agenda entity)
        {
            try
            {
                entity.StrDescripcion = entity.StrDescripcion;
                entity.DatFechaInicial = entity.DatFechaInicial;
                entity.DatFechaFinal = entity.DatFechaFinal;
                entity.OpcAllDay = entity.OpcAllDay;
                entity.OpcEnviado = entity.OpcEnviado;
                entity.OpcCancelada = entity.OpcCancelada;
                entity.StrHoras = entity.StrHoras;
                entity.IntTipoEventoID = entity.IntTipoEventoID;
                entity.IntClienteID = entity.IntClienteID;
                entity.IntUsuarioID = entity.IntUsuarioID;
                entity.OpcLunch = entity.OpcLunch;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Agenda entity)
        {
            try
            {
                await base.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteRangeAsync(IEnumerable<Agenda> entity)
        {
            try
            {
                await base.DeleteRangeAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<bool> ExistAsync(Expression<Func<Agenda, bool>> match)
        {
            try
            {
                return await base.ExistAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<Agenda> FindAsync(Expression<Func<Agenda, bool>> match)
        {
            try
            {
                return await base.FindAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<Agenda>> GetAllAsync()
        {
            try
            {
                return await base.GetAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new List<Agenda> GetAll()
        {
            try
            {
                return base.GetAll();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> UpdateAsync(Agenda entity, string tipo)
        {
            try
            {
                Agenda Agenda = await this.FindAsync(x => x.IntAgendaID == entity.IntAgendaID);


                if (tipo == drop || tipo == resize)
                {
                    Agenda.DatFechaInicial = entity.DatFechaInicial;
                    Agenda.DatFechaFinal = entity.DatFechaFinal;
                    Agenda.OpcAllDay = entity.OpcAllDay;
                    Agenda.StrHoras = entity.StrHoras;

                    await base.UpdateAsync(Agenda);

                    return string.Empty;
                }

                Agenda.IntAgendaID = entity.IntAgendaID;
                Agenda.StrDescripcion = entity.StrDescripcion;
                Agenda.DatFechaInicial = entity.DatFechaInicial;
                Agenda.DatFechaFinal = entity.DatFechaFinal;
                Agenda.OpcAllDay = entity.OpcAllDay;
                Agenda.OpcEnviado = entity.OpcEnviado;
                Agenda.OpcCancelada = entity.OpcCancelada;
                Agenda.StrHoras = entity.StrHoras;
                Agenda.IntTipoEventoID = entity.IntTipoEventoID;
                Agenda.IntClienteID = entity.IntClienteID;
                Agenda.OpcLunch = entity.OpcLunch;

                await base.UpdateAsync(Agenda);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ValidarEventoAsync(Agenda modelo)
        {
            try
            {
                var listaEventos = await this.GetAllAsync();

                List<Agenda> existe = listaEventos.FindAll(x => x.IntAgendaID != modelo.IntAgendaID && x.OpcCancelada == false && x.IntUsuarioID == modelo.IntUsuarioID);

                if (modelo.IntAgendaID != 0)
                {
                    existe = existe.FindAll(x => x.IntUsuarioID == modelo.IntUsuarioID && (x.DatFechaInicial > modelo.DatFechaInicial && x.DatFechaFinal < modelo.DatFechaFinal) || (x.DatFechaInicial < modelo.DatFechaInicial && x.DatFechaFinal > modelo.DatFechaInicial) || (x.DatFechaInicial < modelo.DatFechaFinal && x.DatFechaFinal > modelo.DatFechaFinal));
                }
                else
                {
                    existe = listaEventos.FindAll(x => x.IntUsuarioID == modelo.IntUsuarioID && ((x.DatFechaInicial > modelo.DatFechaInicial && x.DatFechaFinal < modelo.DatFechaFinal) || (x.DatFechaInicial < modelo.DatFechaInicial && x.DatFechaFinal > modelo.DatFechaInicial) || (x.DatFechaInicial < modelo.DatFechaFinal && x.DatFechaFinal > modelo.DatFechaFinal)));
                }


                if (existe.Count != 0)
                {
                    return RecursoAgenda.msnEventoYaExiste;
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ValidarHorasEjecutadas(Agenda modelo)
        {
            try
            {
                //Horas laboradas
                var horas = modelo.DatFechaFinal - modelo.DatFechaInicial;
                var dias = horas.Value.TotalDays;

                var horaAlmuerzo = 0;
                if (modelo.OpcLunch != false)
                {
                    horaAlmuerzo = 1;
                }

                if (dias < 1)
                {
                    modelo.StrHoras = Convert.ToString(horas.Value.TotalHours - horaAlmuerzo);
                }
                else
                {
                    _configuracionCoreBusiness = new ConfiguracionCoreBusiness();

                    var ArrayDiasString = Convert.ToDouble(dias).ToString().Split('.');
                    var diasString = ArrayDiasString[0];

                    var listaConfiguracion = await _configuracionCoreBusiness.GetAllAsync();
                    var configuracion = listaConfiguracion.ToList().FirstOrDefault();

                    var horaInicial = Convert.ToDateTime(configuracion.StrHoraInicial).Hour;
                    var horaFinal = Convert.ToDateTime(configuracion.StrHoraFinal).Hour;
                    int totalHorasDia = horaFinal - horaInicial;
                    int horasAlmuerzo = Convert.ToInt32(diasString);
                    int diasLaborados = horasAlmuerzo;
                    var horaAdicional = 1;
                    if (modelo.OpcLunch != true)
                    {
                        horasAlmuerzo = 0;
                        horaAdicional = 0;
                    }

                    if (modelo.DatFechaFinal.Value.Hour <= 12)
                    {
                        modelo.StrHoras = (diasLaborados * totalHorasDia + decimal.Round(modelo.DatFechaFinal.Value.Hour, 2) - horaInicial - horasAlmuerzo).ToString();//ültimo idasInt son las horas del almuerzo
                    }
                    else
                    {
                        modelo.StrHoras = (diasLaborados * totalHorasDia + decimal.Round(modelo.DatFechaFinal.Value.Hour, 2) - horaInicial - horasAlmuerzo - horaAdicional).ToString();//ültimo idasInt son las 
                    }

                }

                return modelo.StrHoras;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ValidarSiOtroAsesorProgramoEventoConCliente(string fechaInicial, string fechaFinal, Agenda agendaModelo)
        {
            try
            {
                if (!string.IsNullOrEmpty(fechaInicial) || !string.IsNullOrEmpty(fechaFinal))
                {
                    _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();
                    _usuariosCoreBusiness = new UsuariosCoreBusiness();
                    _clientesCoreBusiness = new ClientesCoreBusiness();

                    var tipoEvento = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == agendaModelo.IntTipoEventoID);

                    if ((bool)tipoEvento.OpcAlerta)
                    {
                        var inicio = Convert.ToDateTime(fechaInicial);
                        var final = Convert.ToDateTime(fechaFinal);

                        var eventosEnSemana = await this.FindWhereAsync(x => x.DatFechaInicial >= inicio && x.DatFechaInicial <= final && x.IntUsuarioID != agendaModelo.IntUsuarioID && x.IntClienteID == agendaModelo.IntClienteID && x.IntTipoEventoID == agendaModelo.IntTipoEventoID);
                        var cliente = await _clientesCoreBusiness.FindAsync(x => x.IntClienteID == agendaModelo.IntClienteID);

                        var mensaje = $"El cliente {cliente.StrNombre} ya tiene visita de {tipoEvento.StrDescripcion} programada esa semana con ";
                        var usuariosConEvento = string.Empty;

                        if (eventosEnSemana.Count() != 0)
                        {
                            var eventosAgrupadosPorUsuario = eventosEnSemana.GroupBy(x => x.IntUsuarioID);
                            var listaUsuarios = await _usuariosCoreBusiness.GetAllAsync();

                            var ultimoRegistroAgrupado = eventosAgrupadosPorUsuario.Last();

                            foreach (var item in eventosAgrupadosPorUsuario)
                            {
                                var usuario = listaUsuarios.Find(x => x.IntUsuarioID == item.Key);
                                if (item.Key != ultimoRegistroAgrupado.Key)
                                    usuariosConEvento = usuariosConEvento + $"{usuario.StrNombre},";
                                else
                                {
                                    usuariosConEvento = usuariosConEvento.TrimEnd(',');
                                    usuariosConEvento = usuariosConEvento + $" y {usuario.StrNombre}";
                                }
                            }

                            return mensaje + usuariosConEvento;
                        }
                    }
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TiemposAsesoriasDTO>> ObtenerTiemposDeAsesoriaAsync(Agenda modelo)
        {
            try
            {
                DateTime fechaInicialModulo = new DateTime(2022, 09, 27);
                DateTime fechafinal = modelo.DatFechaFinal.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                var listaEventos = new List<Agenda>();

                if (modelo.IntUsuarioID == 0 && modelo.IntClienteID == 0)
                    listaEventos = await this.FindWhereAsync(x => x.DatFechaInicial >= modelo.DatFechaInicial && x.DatFechaFinal <= fechafinal);
                else if(modelo.IntUsuarioID == 0)
                    listaEventos = await this.FindWhereAsync(x => x.IntClienteID == modelo.IntClienteID && x.DatFechaInicial >= modelo.DatFechaInicial && x.DatFechaFinal <= fechafinal);
                else if (modelo.IntClienteID == 0)
                    listaEventos = await this.FindWhereAsync(x => x.IntUsuarioID == modelo.IntUsuarioID && x.DatFechaInicial >= modelo.DatFechaInicial && x.DatFechaFinal <= fechafinal);

                var listaEvetosDTO = listaEventos.Where(x => x.DatFechaInicial >= fechaInicialModulo).Select(x => new TiemposAsesoriasDTO()
                {
                    NombreUsuario = x.Usuarios.StrNombre,
                    NombreCliente = x.Clientes.StrNombre,
                    FechaInicialDeVisita = (DateTime)x.DatFechaInicial,
                    FechaDeLlegada = x.DatFechaIngreso is null ? new DateTime(2000, 1, 1) : x.DatFechaIngreso.Value
                }).ToList();

                return listaEvetosDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AgendaCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_configuracionCoreBusiness != null)
                {
                    _configuracionCoreBusiness.Dispose();
                    _configuracionCoreBusiness = null;
                }

                if (_tipoEventosCoreBusiness != null)
                {
                    _tipoEventosCoreBusiness.Dispose();
                    _tipoEventosCoreBusiness = null;
                }

                if (_usuariosCoreBusiness != null)
                {
                    _usuariosCoreBusiness.Dispose();
                    _usuariosCoreBusiness = null;
                }

                if (_clientesCoreBusiness != null)
                {
                    _clientesCoreBusiness.Dispose();
                    _clientesCoreBusiness = null;
                }
            }

            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }


        #endregion Dispose
    }
}
