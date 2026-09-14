using Across.ArchivosDeRecurso;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class InformesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        AgendaCoreBusiness _agendaCoreBusiness;
        ContratosCoreBusiness _contratosCoreBusiness;
        ClientesCoreBusiness _clientesCoreBusiness;

        public async Task<List<InformesDTO>> GetHorasContratoVSHorasMensuales(int ano, int mes, string cliente)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                List<Agenda> eventosByCliente = new List<Agenda>();
                List<InformesDTO> informeByCliente = new List<InformesDTO>();

                var eventos = await _agendaCoreBusiness.GetAllAsync();

                if (!string.IsNullOrEmpty(cliente))
                {
                    eventosByCliente = eventos.Where(x => (x.Clientes.StrNombre.ToLower().Contains(cliente.ToLower()) == true || x.Clientes.StrIdentificacion.ToLower().Contains(cliente.ToLower()) == true) && x.DatFechaInicial.Value.Year == ano && x.DatFechaInicial.Value.Month == mes && x.OpcCancelada == false && x.TipoEventos.OpcSoporte == false).ToList();
                }
                else
                {
                    eventosByCliente = eventos.Where(x => x.DatFechaInicial.Value.Year == ano && x.DatFechaInicial.Value.Month == mes && x.OpcCancelada == false && x.TipoEventos.OpcSoporte == false).ToList();
                }

                if (eventosByCliente.Count() != 0)
                {
                    var clientes = eventosByCliente.GroupBy(x => x.IntClienteID);

                    foreach (var item in clientes)
                    {
                        var clienteID = item.Key;
                        var horasCliente = decimal.Round(eventosByCliente.Where(x => x.IntClienteID == clienteID).Sum(x => Convert.ToDecimal(x.StrHoras)),2);
                        
                        //Validar si hay contrato
                        _contratosCoreBusiness = new ContratosCoreBusiness();
                        var contratoCliente = await _contratosCoreBusiness.FindAsync(x => x.IntClienteID == clienteID && x.OpcEstado == true);

                        if (contratoCliente != null)
                        {
                            _clientesCoreBusiness = new ClientesCoreBusiness();
                            InformesDTO informesDTO = new InformesDTO();

                            var clienteInfo = await _clientesCoreBusiness.FindAsync(x => x.IntClienteID == clienteID);

                            informesDTO.Identificacion = clienteInfo.StrIdentificacion;
                            informesDTO.Nombre = clienteInfo.StrNombre;
                            informesDTO.FechaIngreso = clienteInfo.DatFechaIngreso;
                            informesDTO.HorasMes = horasCliente;
                            informesDTO.HorasContrato = (int)contratoCliente.IntHoras;
                            informesDTO.diferencia = informesDTO.HorasContrato - informesDTO.HorasMes;

                            informeByCliente.Add(informesDTO);
                        }                       
                    }
                }

                return informeByCliente;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetHorasContratoVSHorasMensuales(int ano, int mes, int clienteID)
        {
            try
            {
                _agendaCoreBusiness = new AgendaCoreBusiness();
                InformesDTO informesDTO = new InformesDTO();

                var eventos = await _agendaCoreBusiness.GetAllAsync();
                eventos = eventos.Where(x => x.DatFechaInicial.Value.Year == ano && x.DatFechaInicial.Value.Month == mes && x.OpcCancelada == false && x.TipoEventos.OpcSoporte == false && x.IntClienteID == clienteID).ToList();              

                if (eventos.Count() != 0)
                {
                    var horasEjecutadas = eventos.Sum(x => Convert.ToDouble(x.StrHoras));
                  
                    _contratosCoreBusiness = new ContratosCoreBusiness();
                    _clientesCoreBusiness = new ClientesCoreBusiness();

                    var contratoCliente = await _contratosCoreBusiness.FindAsync(x => x.IntClienteID == clienteID && x.OpcEstado == true);

                    if (contratoCliente != null)
                    {
                        if (horasEjecutadas >= contratoCliente.IntHoras)
                        {
                            return RecursoClientes.msnValidacionHorasContrato;
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~InformesCoreBusiness()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
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
