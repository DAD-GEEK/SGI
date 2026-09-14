using Across.ArchivosDeRecurso;
using Across.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CoreBusiness
{
    public class ActasCoreBusiness : CRUDGenerico<Actas>, IActas
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        AgendaCoreBusiness _agendaCoreBusiness;
        ConfiguracionCoreBusiness _configuracionCoreBusiness;
        EmailCoreBusiness _emailCoreBusiness;
        ClientesCoreBusiness _clientesCoreBusiness;
        ContactosCoreBusiness _contactosCoreBusiness;
        ActividadesActaCoreBusiness _actividadesActaCoreBusiness;
        TipoEventosCoreBusiness _tipoEventosCoreBusiness;

        public ActasCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Actas entity)
        {
            try
            {
                entity.IntAgendaID = entity.IntAgendaID;
                entity.IntConsecutivo = entity.IntConsecutivo;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Actas entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Actas> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Actas, bool>> match)
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

        public new async Task<Actas> FindAsync(Expression<Func<Actas, bool>> match)
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

        public new async Task<List<Actas>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Actas entity)
        {
            try
            {

                Actas Actas = await this.FindAsync(x => x.IntActaID == entity.IntActaID);

                Actas.IntAgendaID = entity.IntAgendaID;
                Actas.IntConsecutivo = entity.IntConsecutivo;

                await base.UpdateAsync(Actas);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Actas> UnescapeModelo(Actas modelo)
        {
            try
            {
                foreach (var item in modelo.ActividadesActa)
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

                foreach (var item in modelo.TemasActa)
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

                return modelo;
            }
            catch (Exception)
            {

                throw;
            }
        }

       

        public async Task<DateTime?> ValidarSiguienteVisita(int agendaID)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();

                var agenda = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == agendaID);
                DateTime? proximaVisita = null;

                var nextVisita = (await _agendaCoreBusiness.FindWhereAsync(x => x.Clientes.IntClienteID == agenda.IntClienteID && x.OpcCancelada == false && x.DatFechaInicial > agenda.DatFechaInicial)).OrderBy(x => x.DatFechaInicial);

                //nextVisita = nextVisita.OrderBy(x => x.DatFechaInicial);               

                if (nextVisita.Count() != 0)
                {
                    var fechaVisita = nextVisita.FirstOrDefault().DatFechaInicial;
                    if (fechaVisita.HasValue)
                        proximaVisita = (DateTime)fechaVisita;
                }

                return proximaVisita;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EnviarActaByEmailAsync(Actas ActaEmail, byte[] actaPDF)
        {
            try
            {
                _configuracionCoreBusiness = new ConfiguracionCoreBusiness();
                _emailCoreBusiness = new EmailCoreBusiness();
                _clientesCoreBusiness = new ClientesCoreBusiness();
                _contactosCoreBusiness = new ContactosCoreBusiness();
                var listaContatosMail = new List<Contactos>();

                //Si el acta existe
                if (ActaEmail != null)
                {
                    //Validar configuración básica.
                    var listaConfiguracionBasica = await _configuracionCoreBusiness.GetAllAsync();
                    var configuracionBasica = listaConfiguracionBasica.ToList().FirstOrDefault();
                    var clienteInfo = await _clientesCoreBusiness.FindAsync(x => x.IntClienteID == ActaEmail.Agenda.IntClienteID);
                    listaContatosMail = await _contactosCoreBusiness.FindWhereAsync(x => x.IntClienteID == clienteInfo.IntClienteID);

                    if (!string.IsNullOrEmpty(clienteInfo.StrEmail))
                    {
                        //Agregar el email ppal a los destinatarios
                        var emailPpal = new Contactos();
                        emailPpal.StrEmail = clienteInfo.StrEmail;
                        listaContatosMail.Add(emailPpal);
                    }

                    //Configuración en general
                    if (configuracionBasica == null) return RecursoConfiguracion.msnSinConfiguracion;
                    if (string.IsNullOrEmpty(configuracionBasica.StrSMTPHost)) return RecursoConfiguracion.msnSinHost;
                    if (string.IsNullOrEmpty(configuracionBasica.StrSMPTEmail)) return RecursoConfiguracion.msnSinEmailFrom;
                    if (string.IsNullOrEmpty(Convert.ToString(configuracionBasica.StrSMTPPort))) return RecursoConfiguracion.msnSinPuerto;
                    if (listaContatosMail.Count() == 0) return RecursoContactos.msnValidacionMailContacto;

                    string año = ActaEmail.Agenda.DatFechaFinal.Value.Year.ToString();
                    string fechaVisita = ActaEmail.Agenda.DatFechaFinal.Value.ToString("d MMMM", CultureInfo.CreateSpecificCulture("es-MX"));

                    var Parametros = new Dictionary<string, string>
                    {
                        {"{Cliente}", ActaEmail.Agenda.Clientes.StrNombre },
                        {"{NumeroActa}", Convert.ToString(ActaEmail.IntConsecutivo)},
                        {"{FechaVisita}", fechaVisita },
                        {"{Año}", año },
                        {"{Version}", Assembly.GetExecutingAssembly().GetName().Version.ToString() }
                    };

                    foreach (var item in listaContatosMail)
                    {
                        await _emailCoreBusiness.EnviarEmailAsync(item.StrEmail, "Acta de visita Gestión Integral SGI S.A.S", _emailCoreBusiness.EmailBody("ActaNotificacion.html", Parametros), actaPDF, true, null, ActaEmail);
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarActaAsync(Agenda agenda)
        {
            try
            {
                _tipoEventosCoreBusiness = new TipoEventosCoreBusiness();

                var response = string.Empty;

                var acta = await this.FindAsync(x => x.IntAgendaID == agenda.IntAgendaID);
                if (acta != null)
                    return RecursoActas.msnActaYaExiste;

                var tipoEvento = await _tipoEventosCoreBusiness.FindAsync(x => x.IntTipoEventoID == agenda.IntTipoEventoID);

                if ((bool)tipoEvento.OpcSoporte)
                    return RecursoActas.msnSoporte;

                var consecutivoActa = 1;
                var listaActasTercero = await this.FindWhereAsync(x => x.Agenda.IntClienteID == agenda.IntClienteID);
                if (listaActasTercero.Count() != 0)
                    consecutivoActa = (int)listaActasTercero.Max(x => x.IntConsecutivo) + 1;

                Actas objetoActa = new Actas();
                objetoActa.IntAgendaID = agenda.IntAgendaID;
                objetoActa.IntConsecutivo = consecutivoActa;

                return await this.CreateAsync(objetoActa);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Actas> GetActaAnterior(string id)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();

                int actaAnterior = 0;
                int actaID = Convert.ToInt32(id);
                var acta = await this.FindAsync(x => x.IntActaID == actaID);
                var diaAnterior = acta.Agenda.DatFechaInicial.Value.AddHours(-acta.Agenda.DatFechaInicial.Value.Hour - 1);
                var agenda = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == acta.IntAgendaID);
                var listaAgendaByTercero = await _agendaCoreBusiness.FindWhereAsync(x => x.IntClienteID == agenda.IntClienteID && x.DatFechaInicial <= diaAnterior);
                var listaActasByTercero = this.GetAll().Where(x => listaAgendaByTercero.Any(y => y.IntAgendaID == x.IntAgendaID)).ToList();

                if (listaActasByTercero.Count() != 0) actaAnterior = listaActasByTercero.OrderByDescending(x => x.IntActaID).FirstOrDefault().IntActaID;

                return await this.FindAsync(x => x.IntActaID == actaAnterior);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ActividadesActa>> ObtenerUltimosCompromisosAsync(int actaID)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                _actividadesActaCoreBusiness = new ActividadesActaCoreBusiness();

                var sinCompromisosAnteriores = new List<ActividadesActa>();
                List<int> listaAgendasIDString = new List<int>();
                List<int> listaActasIDString = new List<int>();

                var modeloActaActual = await this.FindAsync(x => x.IntActaID == actaID);

                if (modeloActaActual is null)
                    return sinCompromisosAnteriores;

                var modeloAgenda = await _agendaCoreBusiness.FindAsync(x => x.IntAgendaID == modeloActaActual.IntAgendaID);
                if (modeloAgenda is null)
                    return sinCompromisosAnteriores;

                var listaAgendasPorCliente = await _agendaCoreBusiness.FindWhereAsync(x => x.IntClienteID == modeloAgenda.IntClienteID && x.DatFechaInicial < modeloAgenda.DatFechaInicial);

                listaAgendasPorCliente.ForEach(registro => listaAgendasIDString.Add(registro.IntAgendaID));

                var listaActasPorCliente = await this.FindWhereAsync(x => listaAgendasIDString.Any(y => y == x.IntAgendaID));
                listaActasPorCliente.ForEach(registro => listaActasIDString.Add(registro.IntActaID));

                var listaCompromisosPendientes = await _actividadesActaCoreBusiness.FindWhereAsync(x => x.OpcEjecuta == false && listaActasIDString.Any(y => y == x.IntActaID));

                if (listaCompromisosPendientes.Count() != 0)
                {
                    listaCompromisosPendientes = _actividadesActaCoreBusiness.UnescapeActividadesActa(listaCompromisosPendientes);
                    return listaCompromisosPendientes;
                }

                return sinCompromisosAnteriores;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task ActualizarConsecutivoActa(Agenda modeloAgenda)
        {
            try
            {
                var actualizarConsecutivo = await this.GetConsecutivoActa(modeloAgenda);
                var modeloActa = await this.FindAsync(x => x.IntAgendaID == modeloAgenda.IntAgendaID);

                if (modeloActa.IntConsecutivo != actualizarConsecutivo)
                {
                    modeloActa.IntConsecutivo = actualizarConsecutivo;
                    await UpdateAsync(modeloActa);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetConsecutivoActa(Agenda modeloAgenda)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                var consecutivoActa = 0;

                var listaAgendasByTercero = (await _agendaCoreBusiness.FindWhereAsync(x => x.IntClienteID == modeloAgenda.IntClienteID && x.DatFechaInicial < modeloAgenda.DatFechaInicial && x.OpcCancelada == false)).ToList();
                var listaActasByTercero = GetAll().Where(x => listaAgendasByTercero.Any(y => y.IntAgendaID == x.IntAgendaID)).ToList();

                if (listaActasByTercero.Count() != 0)
                    consecutivoActa = listaActasByTercero.Count();

                return consecutivoActa + 1;
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

        ~ActasCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {


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
