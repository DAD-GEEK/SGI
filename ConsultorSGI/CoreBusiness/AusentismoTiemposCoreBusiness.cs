using Across;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class AusentismoTiemposCoreBusiness : CRUDGenerico<AusentismoTiempos>, IAusentismoTiemposCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness;
        private IEmpleadosCoreBusiness _iEmpleadosCoreBusiness;
        private IAusentismoProrrogasCoreBusiness _iAusentismoProrrogasCoreBusiness;

        public AusentismoTiemposCoreBusiness() : base(new gestioni_consultorNetEntities(new ServicioTercero()))
        {
            _iCommonCoreBusiness = new CommonCoreBusiness();
            _iEmpleadosCoreBusiness = new EmpleadosCoreBusiness();
        }

        #region CRUD Generico
        public async Task<List<AusentismoTiempos>> ObtenerTiemposDeAusentismoPorAnio(int anio)
        {
            try
            {
                var listaAusentismoTiempos = await this.FindWhereAsync(x => x.IntAño == anio);

                return listaAusentismoTiempos;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(AusentismoTiempos entity)
        {
            try
            {
                await base.SaveEntityAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private new async Task<string> CreateAsync(AusentismoTiempos entity)
        {
            try
            {
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                entity.IntAño = entity.IntAño;
                entity.TIntPeriodo = entity.TIntPeriodo;
                entity.IntDiasIncapacidad = entity.IntDiasIncapacidad;
                entity.IntAusentismoID = entity.IntAusentismoID;
                entity.IntEmpleadoID = entity.IntEmpleadoID;
                entity.IntTerceroID = terceroID;
                entity.IntTipoEventoAusentismoID = entity.IntTipoEventoAusentismoID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AusentismoTiempos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AusentismoTiempos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AusentismoTiempos, bool>> match)
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

        public new async Task<AusentismoTiempos> FindAsync(Expression<Func<AusentismoTiempos, bool>> match)
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

        public new List<AusentismoTiempos> GetAll()
        {
            try
            {
                var terceroID = _iCommonCoreBusiness.GetTerceroIDFromCurrentUser();
                return base.GetAll().Where(x => x.IntTerceroID == terceroID).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<AusentismoTiempos>> GetAllAsync()
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


        private new async Task<string> UpdateAsync(AusentismoTiempos entity)
        {
            try
            {
                var AusentismoTiempos = await this.FindAsync(x => x.IntAusentismoTiempoID == entity.IntAusentismoTiempoID);

                AusentismoTiempos.IntAño = entity.IntAño;
                AusentismoTiempos.TIntPeriodo = entity.TIntPeriodo;
                AusentismoTiempos.IntDiasIncapacidad = entity.IntDiasIncapacidad;
                AusentismoTiempos.IntAusentismoID = entity.IntAusentismoID;
                AusentismoTiempos.IntEmpleadoID = entity.IntEmpleadoID;

                await base.UpdateAsync(AusentismoTiempos);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(AusentismoTiempos model)
        {
            try
            {
                if (model.IntAusentismoTiempoID != 0) return await this.UpdateAsync(model);
                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Acceso a datos
        public async Task<bool> GuardarTiempoDeIncapacidadPorMesAsync(Ausentismo ausentismoModelo)
        {
            try
            {
                this._iAusentismoProrrogasCoreBusiness = new AusentismoProrrogasCoreBusiness();

                var fechaInicialIncapacidad = ausentismoModelo.DatFechaInicial;
                var fechaFinalIncapacidad = ausentismoModelo.DatFechaFinal;

                var añoInicial = fechaInicialIncapacidad.Year;
                var mesInicial = fechaInicialIncapacidad.Month;
                var añoFinal = fechaFinalIncapacidad.Year;
                var mesFinal = fechaFinalIncapacidad.Month;

                if (añoInicial == añoFinal && mesInicial == mesFinal)
                    await this.IncapacidadEnElMismoMesAsync(ausentismoModelo);
                else
                    await this.IncapacidadEnMesesDiferentesAsync(ausentismoModelo);

                List<AusentismoProrrogas> listaProrrogas = await _iAusentismoProrrogasCoreBusiness.FindWhereAsync(x => x.IntAusentismoID == ausentismoModelo.IntAusentismoID);

                foreach (var item in listaProrrogas)
                    await this.GuardarTiempoDeProrrogaPorMesAsync(item);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<bool> IncapacidadEnElMismoMesAsync(Ausentismo ausentismoModelo)
        {
            try
            {
                var empleado = await _iEmpleadosCoreBusiness.FindAsync(x => x.IntEmpleadoID == ausentismoModelo.IntEmpleadoID);

                int anio = ausentismoModelo.DatFechaInicial.Year;
                int mes = ausentismoModelo.DatFechaInicial.Month;

                var ausentismoModeloEnBaseDeDatos = await this.FindAsync(x => x.IntAusentismoID == ausentismoModelo.IntAusentismoID && x.IntAño == anio && x.TIntPeriodo == mes);

                if (ausentismoModeloEnBaseDeDatos != null)
                {
                    ausentismoModeloEnBaseDeDatos.IntDiasIncapacidad = (ausentismoModelo.DatFechaFinal - ausentismoModelo.DatFechaInicial).Days + 1;
                    await this.UpdateAsync(ausentismoModeloEnBaseDeDatos);
                    return true;
                }

                var ausentismoTiempo = new AusentismoTiempos();

                ausentismoTiempo.IntAño = (short)ausentismoModelo.DatFechaInicial.Year;
                ausentismoTiempo.TIntPeriodo = (byte)ausentismoModelo.DatFechaInicial.Month;
                ausentismoTiempo.IntDiasIncapacidad = (ausentismoModelo.DatFechaFinal - ausentismoModelo.DatFechaInicial).Days + 1;
                ausentismoTiempo.IntAusentismoID = ausentismoModelo.IntAusentismoID;
                ausentismoTiempo.IntEmpleadoID = ausentismoModelo.IntEmpleadoID;
                ausentismoTiempo.IntDiagnosticoID = ausentismoModelo.IntDiagnosticoID;
                ausentismoTiempo.IntAreaID = (int)empleado.IntAreaID;
                ausentismoTiempo.IntTipoEventoAusentismoID = ausentismoModelo.IntTipoEventoAusentismoID;

                await this.CreateAsync(ausentismoTiempo);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<bool> IncapacidadEnMesesDiferentesAsync(Ausentismo ausentismoModelo)
        {
            try
            {
                var fechaInicialIncapacidad = ausentismoModelo.DatFechaInicial;
                var fechaFinalIncapacidad = ausentismoModelo.DatFechaFinal;
                var añoFinal = fechaFinalIncapacidad.Year;
                var mesFinal = fechaFinalIncapacidad.Month;
                bool primerMes = true;
                bool ultimoMes = false;

                while (ultimoMes != true)
                {
                    var añoFechaInicial = fechaInicialIncapacidad.Year;
                    var mesFechaInicial = fechaInicialIncapacidad.Month;

                    if (añoFechaInicial == añoFinal && mesFechaInicial == mesFinal)
                        ultimoMes = true;

                    var fechaInicioMes = new DateTime(añoFechaInicial, mesFechaInicial, 1);
                    var fechaFinalMes = ultimoMes == false ? fechaInicioMes.AddMonths(1).AddDays(-1) : fechaFinalIncapacidad;

                    if (primerMes)
                    {
                        ausentismoModelo.DatFechaInicial = fechaInicialIncapacidad;
                        ausentismoModelo.DatFechaFinal = fechaFinalMes;
                        primerMes = false;
                    }
                    else
                    {
                        ausentismoModelo.DatFechaInicial = fechaInicioMes;
                        ausentismoModelo.DatFechaFinal = fechaFinalMes;
                    }

                    await this.IncapacidadEnElMismoMesAsync(ausentismoModelo);

                    fechaInicialIncapacidad = fechaInicialIncapacidad.AddMonths(1);
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarTiempoDeProrrogaPorMesAsync(AusentismoProrrogas prorrogaModelo)
        {
            try
            {
                var fechaInicialIncapacidad = prorrogaModelo.DatFechaInicial;
                var fechaFinalIncapacidad = prorrogaModelo.DatFechaFinal;

                var añoInicial = fechaInicialIncapacidad.Year;
                var mesInicial = fechaInicialIncapacidad.Month;
                var añoFinal = fechaFinalIncapacidad.Year;
                var mesFinal = fechaFinalIncapacidad.Month;

                if (añoInicial == añoFinal && mesInicial == mesFinal)
                    await this.ProrrogaEnElMismoMesAsync(prorrogaModelo);
                else
                    await this.ProrrogaEnMesesDiferentesAsync(prorrogaModelo);

                return String.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<string> ProrrogaEnElMismoMesAsync(AusentismoProrrogas prorrogaModelo)
        {
            try
            {
                var anioProrroga = prorrogaModelo.DatFechaInicial.Year;
                var mesProrroga = prorrogaModelo.DatFechaInicial.Month;

                int diasProrroga = (prorrogaModelo.DatFechaFinal - prorrogaModelo.DatFechaInicial).Days + 1;

                var ausentismoTiempo = await this.FindAsync(x => x.IntAusentismoID == prorrogaModelo.IntAusentismoID && x.IntAño == anioProrroga && x.TIntPeriodo == mesProrroga);

                if (ausentismoTiempo != null)
                {
                    ausentismoTiempo.IntDiasProrroga = ausentismoTiempo.IntDiasProrroga + prorrogaModelo.DatFechaFinal.Subtract(prorrogaModelo.DatFechaInicial).Days + 1;

                    return await this.UpdateAsync(ausentismoTiempo);
                }

                var listaAusentismo = await this.FindWhereAsync(x => x.IntAusentismoID == prorrogaModelo.IntAusentismoID);
                var ausentismoTiemposModelo = listaAusentismo.FirstOrDefault();

                ausentismoTiempo = new AusentismoTiempos();

                ausentismoTiempo.IntAño = (short)anioProrroga;
                ausentismoTiempo.TIntPeriodo = (byte)mesProrroga;
                ausentismoTiempo.IntDiasIncapacidad = 0;
                ausentismoTiempo.IntDiasProrroga = diasProrroga;
                ausentismoTiempo.IntAusentismoID = prorrogaModelo.IntAusentismoID;
                ausentismoTiempo.IntEmpleadoID = ausentismoTiemposModelo.IntEmpleadoID;
                ausentismoTiempo.IntDiagnosticoID = ausentismoTiemposModelo.IntDiagnosticoID;
                ausentismoTiempo.IntAreaID = (int)ausentismoTiemposModelo.IntAreaID;
                ausentismoTiempo.IntTipoEventoAusentismoID = ausentismoTiemposModelo.IntTipoEventoAusentismoID;

                return await this.CreateAsync(ausentismoTiempo);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<bool> ProrrogaEnMesesDiferentesAsync(AusentismoProrrogas prorrogaModelo)
        {
            try
            {
                var fechaInicialIncapacidad = prorrogaModelo.DatFechaInicial;
                var fechaFinalIncapacidad = prorrogaModelo.DatFechaFinal;
                var añoFinal = fechaFinalIncapacidad.Year;
                var mesFinal = fechaFinalIncapacidad.Month;
                bool primerMes = true;
                bool ultimoMes = false;

                while (ultimoMes != true)
                {
                    var añoFechaInicial = fechaInicialIncapacidad.Year;
                    var mesFechaInicial = fechaInicialIncapacidad.Month;

                    if (añoFechaInicial == añoFinal && mesFechaInicial == mesFinal)
                        ultimoMes = true;

                    var fechaInicioMes = new DateTime(añoFechaInicial, mesFechaInicial, 1);
                    var fechaFinalMes = ultimoMes == false ? fechaInicioMes.AddMonths(1).AddDays(-1) : fechaFinalIncapacidad;

                    if (primerMes)
                    {
                        prorrogaModelo.DatFechaInicial = fechaInicialIncapacidad;
                        prorrogaModelo.DatFechaFinal = fechaFinalMes;
                        primerMes = false;
                    }
                    else
                    {
                        prorrogaModelo.DatFechaInicial = fechaInicioMes;
                        prorrogaModelo.DatFechaFinal = fechaFinalMes;
                    }

                    await this.ProrrogaEnElMismoMesAsync(prorrogaModelo);

                    fechaInicialIncapacidad = fechaInicialIncapacidad.AddMonths(1);
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Ausentismo tiempos
        public int ObtenerCantidadTotalDeIncapacidadesPorAnio(List<AusentismoTiempos> tiempoAusentismoPorAnio)
        {
            try
            {
                int totalIncapacidades = tiempoAusentismoPorAnio.GroupBy(x => x.IntAusentismoID).Count();

                return totalIncapacidades;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int ObtenerTotalDiasDeIncapacidadPorAnio(List<AusentismoTiempos> tiempoAusentismoPorAnio)
        {
            try
            {
                int totalDiasIncapacidades = tiempoAusentismoPorAnio.Sum(x => x.IntDiasIncapacidad);
                int totalDiasProrrogas = tiempoAusentismoPorAnio.Sum(x => x.IntDiasProrroga);

                int totalDias = totalDiasIncapacidades + totalDiasProrrogas;

                return totalDias;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int ObtenerCantidadTotalIncapacidadesPorAnio(List<AusentismoTiempos> tiempoAusentismoPorAnio)
        {
            try
            {
                int totalIncapacidades = tiempoAusentismoPorAnio.GroupBy(x => x.IntAusentismoID).Count();

                return totalIncapacidades;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MesesDelAnioDTO> Obtener_NumeroDeIncapacidadesPorPeriodo(List<AusentismoTiempos> listaAusentismo, enumTiposGlobalesDeIncapacidad tipoGeneralDeAusentismo)
        {
            try
            {
                var listaMeses = Common.GetMesesDelAnio();
                var registroAusentismo = listaAusentismo.FirstOrDefault();

                if (listaAusentismo.Count() != 0)
                {
                    List<AusentismoTiempos> listaAusentismoPorMes = new List<AusentismoTiempos>();

                    foreach (var item in listaMeses.OrderBy(x => x.Numero))
                    {
                        if (tipoGeneralDeAusentismo == enumTiposGlobalesDeIncapacidad.EG_AC)
                        {
                            listaAusentismoPorMes = listaAusentismo.Where(x => x.IntAño == registroAusentismo.IntAño && x.TIntPeriodo == item.Numero && (x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Enfermedad_General || x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Accidente_Comun)).ToList();
                        }

                        if (tipoGeneralDeAusentismo == enumTiposGlobalesDeIncapacidad.AT_EL)
                        {
                            listaAusentismoPorMes = listaAusentismo.Where(x => x.IntAño == registroAusentismo.IntAño && x.TIntPeriodo == item.Numero && (x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Accidente_Trabajo || x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Enfermedad_Laboral)).ToList();
                        }

                        item.Valor = listaAusentismoPorMes.GroupBy(x => x.IntAusentismoID).Count();
                    }
                }

                return listaMeses;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MesesDelAnioDTO> Obtener_NumeroDeDiasDeIncapacidadPorPeriodo(List<AusentismoTiempos> listaAusentismo, enumTiposGlobalesDeIncapacidad tipoGeneralDeAusentismo)
        {
            try
            {
                var listaMeses = Common.GetMesesDelAnio();
                var registroAusentismo = listaAusentismo.FirstOrDefault();

                if (listaAusentismo.Count() != 0)
                {
                    List<AusentismoTiempos> listaAusentismoPorMes = new List<AusentismoTiempos>();

                    foreach (var item in listaMeses.OrderBy(x => x.Numero))
                    {
                        if (tipoGeneralDeAusentismo == enumTiposGlobalesDeIncapacidad.EG_AC)
                        {
                            listaAusentismoPorMes = listaAusentismo.Where(x => x.IntAño == registroAusentismo.IntAño && x.TIntPeriodo == item.Numero && (x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Enfermedad_General || x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Accidente_Comun)).ToList();
                        }

                        if (tipoGeneralDeAusentismo == enumTiposGlobalesDeIncapacidad.AT_EL)
                        {
                            listaAusentismoPorMes = listaAusentismo.Where(x => x.IntAño == registroAusentismo.IntAño && x.TIntPeriodo == item.Numero && (x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Accidente_Trabajo || x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Enfermedad_Laboral)).ToList();
                        }

                        item.Valor = listaAusentismoPorMes.Sum(x => x.IntDiasIncapacidad + x.IntDiasProrroga);

                    }
                }

                return listaMeses;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MesesDelAnioDTO> Obtener_Cantidad_AT_PorPeriodo(List<AusentismoTiempos> listaRegistros)
        {
            try
            {
                listaRegistros = listaRegistros.Where(x => x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Accidente_Trabajo).ToList();
                var listaMeses = Common.GetMesesDelAnio();
                var registroAusentismo = listaRegistros.FirstOrDefault();

                if (listaRegistros.Count() != 0)
                {
                    foreach (var item in listaMeses.OrderBy(x => x.Numero))
                    {
                        var listaAusentismoPorMes = listaRegistros.Where(x => x.TIntPeriodo == item.Numero).ToList();
                        item.Valor = (decimal)listaAusentismoPorMes.Count();
                    }
                }

                return listaMeses;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AusentismoTiemposCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (_iCommonCoreBusiness != null)
                //{
                //    _iCommonCoreBusiness.Dispose();
                //    _iCommonCoreBusiness = null;
                //}

                if (_iEmpleadosCoreBusiness != null)
                {
                    _iEmpleadosCoreBusiness.Dispose();
                    _iEmpleadosCoreBusiness = null;
                }

                if (_iAusentismoProrrogasCoreBusiness != null)
                {
                    _iAusentismoProrrogasCoreBusiness.Dispose();
                    _iAusentismoProrrogasCoreBusiness = null;
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
