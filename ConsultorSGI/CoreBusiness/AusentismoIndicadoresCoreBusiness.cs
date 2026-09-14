using Across;
using Across.CacheStorage;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Servicios;
using Microsoft.Win32;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class AusentismoIndicadoresCoreBusiness : CRUDGenerico<AusentismoIndicadores>, IAusentismoIndicadoresCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        private IAusentismoTiemposCoreBusiness _iAusentismoTiemposCoreBusiness;
        private IAusentismoIndicadores_InformacionPorEmpresaCoreBusiness _iAusentismoIndicadores_InformacionPorEmpresaCoreBusiness;
        private IAusentismoVariablesCoreBusiness _iAusentismoVariablesCoreBusiness;
        private ICommonCoreBusiness _iCommonCoreBusiness;
        private IAusentismoIndicadoresDataAccess _iAusentismoIndicadoresDataAccess;
        private ICacheStorage _iCacheStorage;


        public AusentismoIndicadoresCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iAusentismoTiemposCoreBusiness = new AusentismoTiemposCoreBusiness();
            this._iAusentismoVariablesCoreBusiness = new AusentismoVariablesCoreBusiness();
            this._iAusentismoIndicadores_InformacionPorEmpresaCoreBusiness = new AusentismoIndicadores_InformacionPorEmpresaCoreBusiness();
            this._iCommonCoreBusiness = new CommonCoreBusiness();
            this._iAusentismoIndicadoresDataAccess = new AusentismoIndicadoresDataAccess();
            this._iCacheStorage = new CacheStorage();

        }

        #region CRUD Generico
        public async Task<List<AusentismoIndicadoresDTO>> ObtenerListaDeTodosLosIndicadoresAsync(int anio = 0)
        {
            try
            {
                var terceroID = _iCommonCoreBusiness.GetTerceroIDFromCurrentUser();
                List<AusentismoIndicadoresDTO> listaIndicadoresPorEmpresaDTO = null;

                string listaIndicadoresPorEmpresaJSON = _iCacheStorage.Get<string>($"{CacheNames.ObtenerIndicadoresPorEmpresa}_{terceroID}");

                if (string.IsNullOrEmpty(listaIndicadoresPorEmpresaJSON))
                {
                    listaIndicadoresPorEmpresaDTO = _iAusentismoIndicadoresDataAccess.ObtenerListaIndicadoresGenerales(anio);
                    var listaIndicadoresPorEmpresa = await _iAusentismoIndicadores_InformacionPorEmpresaCoreBusiness.GetAllAsync();

                    foreach (var item in listaIndicadoresPorEmpresaDTO)
                    {
                        item.InformacionPorEmpresaDTO = new AusentismoIndicadores_InformacionPorEmpresaDTO();
                        var indicadorPorEmpresa = listaIndicadoresPorEmpresa.FirstOrDefault(x => x.StrAusentismoIndicadoresID == item.StrId);

                        if (indicadorPorEmpresa != null)
                            item.InformacionPorEmpresaDTO = this.ConvertirInformacionPorEmpresaEnDTO(indicadorPorEmpresa);
                    }

                    listaIndicadoresPorEmpresaJSON = JsonConvert.SerializeObject(listaIndicadoresPorEmpresaDTO);
                    _iCacheStorage.Insert($"{CacheNames.ObtenerIndicadoresPorEmpresa}_{terceroID}", listaIndicadoresPorEmpresaJSON, new TimeSpan(8, 0, 0));
                }
                else
                    listaIndicadoresPorEmpresaDTO = JsonConvert.DeserializeObject<List<AusentismoIndicadoresDTO>>(listaIndicadoresPorEmpresaJSON);

                listaIndicadoresPorEmpresaDTO.ForEach(item => item.IntAnio = anio);

                return listaIndicadoresPorEmpresaDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AusentismoIndicadoresDTO>> ObtenerListaDeIndicadoresDTOPorTipoAsync(enumTiposGlobalesDeIncapacidad tipoDeAusentismo, int anio = 0)
        {
            try
            {
                var listaIndicadoresDTO = await this.ObtenerListaDeTodosLosIndicadoresAsync(anio);
                listaIndicadoresDTO = listaIndicadoresDTO.Where(x => x.StrTipoEventoAusentismo == tipoDeAusentismo.ToString()).ToList();
                return listaIndicadoresDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(AusentismoIndicadores entity)
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

        public new async Task<string> CreateAsync(AusentismoIndicadores entity)
        {
            try
            {
                entity.StrId = Guid.NewGuid().ToString();
                await base.CreateAsync(entity);
                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AusentismoIndicadores entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AusentismoIndicadores> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AusentismoIndicadores, bool>> match)
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

        public new async Task<AusentismoIndicadores> FindAsync(Expression<Func<AusentismoIndicadores, bool>> match)
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

        public new async Task<List<AusentismoIndicadores>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(AusentismoIndicadores entity)
        {
            try
            {
                var indicadoresInformacion = await this.FindAsync(x => x.StrId == entity.StrId);

                indicadoresInformacion.StrNombreIndicador = entity.StrNombreIndicador;
                indicadoresInformacion.StrFormula = entity.StrFormula;
                indicadoresInformacion.StrFuente = entity.StrFuente;
                indicadoresInformacion.StrObjetivo = entity.StrObjetivo;

                await base.UpdateAsync(indicadoresInformacion);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarInformacionDeIndicadorPorEmpresaAsyncAsync(AusentismoIndicadores_InformacionPorEmpresa modelo)
        {
            try
            {
                var respuesta = await _iAusentismoIndicadores_InformacionPorEmpresaCoreBusiness.GuardarAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                {
                    var terceroID = _iCommonCoreBusiness.GetTerceroIDFromCurrentUser();
                    _iCacheStorage.Clear($"{CacheNames.ObtenerIndicadoresPorEmpresa}_{terceroID}");
                }

                return respuesta;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarIndicadorAsync(AusentismoIndicadores modelo)
        {
            try
            {
                var respuesta = await this.UpdateAsync(modelo);

                if (string.IsNullOrEmpty(respuesta))
                {
                    _iCacheStorage.Clear($"{CacheNames.ObtenerTodosLosIndicadoresGenerales}");
                    _iAusentismoIndicadoresDataAccess.EliminarAusentismoIndicadoresEnCachePorEmpresa();
                }

                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Ausentismo por Enfermedad General o Accidente Común
        public async Task<IndicadorAusentismoGlobalDTO> ObtenerDatosDeIndicadores_MedicionGeneralAusentismoAsync(int anio, string tipoDeAusentismo)
        {
            try
            {
                var listaTiemposAusentismoAnioActual = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaAusentismoVariables = await _iAusentismoVariablesCoreBusiness.FindWhereAsync(x => x.IntAno == anio);

                IndicadorAusentismoGlobalDTO ausentismoGlobalPorAnio = new IndicadorAusentismoGlobalDTO();
                ausentismoGlobalPorAnio.MedicionGeneralAusentismo = this.ObtenerMedicionGeneralDeAusentismoAsync(listaTiemposAusentismoAnioActual, anio, tipoDeAusentismo);
                ausentismoGlobalPorAnio.MedicionesGeneralAusentismo_DatosChart = this.ObtenerInformacionParaGraficar_MedicionGeneralAusentismo(ausentismoGlobalPorAnio.MedicionGeneralAusentismo, tipoDeAusentismo);
                ausentismoGlobalPorAnio.CanvasID = tipoDeAusentismo == enumTiposGlobalesDeIncapacidad.EG_AC.ToString() ? "canvas_mga" : "canvas_apat";

                return ausentismoGlobalPorAnio;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AusentismoIndicadoresDTO> ObtenerDatosDeIndicadoresPorEmpresaAsync(int anioActual, string ausentismoCodigoIndicador)
        {
            try
            {
                var listaIndicadores = await this.ObtenerListaDeTodosLosIndicadoresAsync(anioActual);

                var maestroIndicador =  listaIndicadores.Find(x => x.StrCodigo == ausentismoCodigoIndicador);

                if (maestroIndicador.InformacionPorEmpresaDTO != null)
                {
                    maestroIndicador.InformacionPorEmpresaDTO = this.ConvertirInformacionPorEmpresaEnDTO(maestroIndicador.InformacionPorEmpresaDTO);

                    maestroIndicador.Grafico = maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.indiceDeFrecuenciaPorCausaMedica.ToString() ? await this.ObtenerInformacionParaGraficar_IndiceDeFrecuenciaPorCausaMedica(maestroIndicador, anioActual) :

                            maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.indiceDeSeveridadPorCausaMedica.ToString() ? await this.ObtenerInformacionParaGraficar_IndiceDeSeveridadPorCausaMedica(maestroIndicador, anioActual) :

                            maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.tasaDeAusentismoPorCausaMedica.ToString() ? await this.ObtenerInformacionParaGraficar_TasaDeAusentismoPorCausaMedica(maestroIndicador, anioActual) :

                            maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.incidenciaYPrevalenciaDeAusentismoPorCausaMedica.ToString() ? await this.ObtenerInformacionParaGraficar_IncidenciaYPrevalenciaDeAusentismoPorCausaMedica(maestroIndicador, anioActual) :

                            maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.reporteEInvestigacionAT_EL.ToString() ? await this.ObtenerInformacionParaGraficar_ReporteEInvestigacionAT_EL(maestroIndicador, anioActual) :

                             maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.indiceDeFrecuenciaAT.ToString() ? await this.ObtenerInformacionParaGraficar_IndiceDeFrecuenciaAT(maestroIndicador, anioActual) : 
                             
                             maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.indiceDeFrecuenciaEL.ToString() ? await this.ObtenerInformacionParaGraficar_IndiceDeFrecuenciaEL(maestroIndicador, anioActual) : 
                             
                             maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.mortalidad.ToString() ? await this.ObtenerInformacionParaGraficar_Mortalidad(maestroIndicador, anioActual) :

                             maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.indiceDeSeveridadAT_EL.ToString() ? await this.ObtenerInformacionParaGraficar_IndiceDeSeveridadAT_EL(maestroIndicador, anioActual) :
                             
                             maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.tasaDeAccidentalidad.ToString() ? await this.ObtenerInformacionParaGraficar_TasaDeAccidentalidad(maestroIndicador, anioActual) :

                             maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.incidenciaYPrevalenciaDeAccidentes.ToString() ? await this.ObtenerInformacionParaGraficar_IncidenciaYPrevalenciaDeAccidentesDEnfermedadLaboral(maestroIndicador, anioActual) :
                             
                             maestroIndicador.StrCodigo == enumCodigosAusentismoIndicadores.lesionesIncapacitantes.ToString() ? await this.ObtenerInformacionParaGraficar_LesionesIncapacitantes(maestroIndicador, anioActual) :

                         new GraficosChartDTO();
                }

                return maestroIndicador;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private AusentismoIndicadores_InformacionPorEmpresaDTO ConvertirInformacionPorEmpresaEnDTO(AusentismoIndicadores_InformacionPorEmpresa informacionExtraPorEmpresa)
        {
            try
            {
                var informacionExtraPorEmpresaDTO = new AusentismoIndicadores_InformacionPorEmpresaDTO();

                if (informacionExtraPorEmpresa != null)
                {
                    informacionExtraPorEmpresaDTO.StrId = informacionExtraPorEmpresa.StrId;
                    informacionExtraPorEmpresaDTO.StrAusentismoIndicadoresID = informacionExtraPorEmpresa.StrAusentismoIndicadoresID;
                    informacionExtraPorEmpresaDTO.IntTerceroID = informacionExtraPorEmpresa.IntTerceroID;
                    informacionExtraPorEmpresaDTO.StrProceso = informacionExtraPorEmpresa.StrProceso;
                    informacionExtraPorEmpresaDTO.StrCargos = informacionExtraPorEmpresa.StrCargos;
                    informacionExtraPorEmpresaDTO.StrResponsable = informacionExtraPorEmpresa.StrResponsable;
                    informacionExtraPorEmpresaDTO.StrTipoGrafico = informacionExtraPorEmpresa.StrTipoGrafico;
                    informacionExtraPorEmpresaDTO.StrNombreTipoGrafico = informacionExtraPorEmpresa.StrTipoGrafico == enumTiposDeGrafico.bar.ToString() ? "Barras" : "Lineal";
                    informacionExtraPorEmpresaDTO.StrFrecuencia = informacionExtraPorEmpresa.StrFrecuencia;
                    informacionExtraPorEmpresaDTO.IntMeta_1 = informacionExtraPorEmpresa.IntMeta_1;
                    informacionExtraPorEmpresaDTO.IntMeta_2 = informacionExtraPorEmpresa.IntMeta_2;
                    informacionExtraPorEmpresaDTO.IntMeta_3 = informacionExtraPorEmpresa.IntMeta_3;
                    informacionExtraPorEmpresaDTO.IntMeta_4 = informacionExtraPorEmpresa.IntMeta_4;
                    informacionExtraPorEmpresaDTO.IntMeta_5 = informacionExtraPorEmpresa.IntMeta_5;
                    informacionExtraPorEmpresaDTO.IntMeta_6 = informacionExtraPorEmpresa.IntMeta_6;
                    informacionExtraPorEmpresaDTO.IntMeta_7 = informacionExtraPorEmpresa.IntMeta_7;
                    informacionExtraPorEmpresaDTO.IntMeta_8 = informacionExtraPorEmpresa.IntMeta_8;
                    informacionExtraPorEmpresaDTO.IntMeta_9 = informacionExtraPorEmpresa.IntMeta_9;
                    informacionExtraPorEmpresaDTO.IntMeta_10 = informacionExtraPorEmpresa.IntMeta_10;
                    informacionExtraPorEmpresaDTO.IntMeta_11 = informacionExtraPorEmpresa.IntMeta_11;
                    informacionExtraPorEmpresaDTO.IntMeta_12 = informacionExtraPorEmpresa.IntMeta_12;
                }

                return informacionExtraPorEmpresaDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<IndicadorAusentismoGeneralDTO> ObtenerMedicionGeneralDeAusentismoAsync(List<AusentismoTiempos> listaTiemposDeAusentismo, int anio, string tipoDeAusentismo)
        {
            try
            {
                var listaTiemposDeAusentismoPorTipo = new List<AusentismoTiempos>();

                if (tipoDeAusentismo == enumTiposGlobalesDeIncapacidad.EG_AC.ToString())
                    listaTiemposDeAusentismoPorTipo = listaTiemposDeAusentismo.Where(x => x.IntAño == anio && (x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Enfermedad_General || x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Accidente_Comun)).ToList();

                if (tipoDeAusentismo == enumTiposGlobalesDeIncapacidad.AT_EL.ToString())
                    listaTiemposDeAusentismoPorTipo = listaTiemposDeAusentismo.Where(x => x.IntAño == anio && (x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Accidente_Trabajo || x.IntTipoEventoAusentismoID == (int)enumTiposDeIncapacidad.Enfermedad_Laboral)).ToList();

                List<IndicadorAusentismoGeneralDTO> listaMedicionesGeneralesAusentismo = new List<IndicadorAusentismoGeneralDTO>();

                IndicadorAusentismoGeneralDTO medicionGeneralAusentismo = new IndicadorAusentismoGeneralDTO();

                medicionGeneralAusentismo.Anio = anio;
                medicionGeneralAusentismo.NumeroTotalIncapacidades = _iAusentismoTiemposCoreBusiness.ObtenerCantidadTotalDeIncapacidadesPorAnio(listaTiemposDeAusentismo);
                medicionGeneralAusentismo.NumeroTotalDias = _iAusentismoTiemposCoreBusiness.ObtenerTotalDiasDeIncapacidadPorAnio(listaTiemposDeAusentismo);
                medicionGeneralAusentismo.NumeroIncapacidadesPorEnfermedadGeneral_AccidenteComun = _iAusentismoTiemposCoreBusiness.ObtenerCantidadTotalIncapacidadesPorAnio(listaTiemposDeAusentismoPorTipo);
                medicionGeneralAusentismo.NumeroDiasPorEnfermedadGeneral_AccidenteComun = _iAusentismoTiemposCoreBusiness.ObtenerTotalDiasDeIncapacidadPorAnio(listaTiemposDeAusentismoPorTipo);
                medicionGeneralAusentismo.backgroundColor = tipoDeAusentismo == enumTiposGlobalesDeIncapacidad.EG_AC.ToString() ? ParametrosGenerales.Color_Primary : ParametrosGenerales.Color_Success;

                listaMedicionesGeneralesAusentismo.Add(medicionGeneralAusentismo);

                return listaMedicionesGeneralesAusentismo;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Información para graficar
        private GraficosChartDTO ObtenerInformacionParaGraficar_MedicionGeneralAusentismo(List<IndicadorAusentismoGeneralDTO> listaRegistro, string tipoDeAusentismo)
        {
            try
            {
                GraficosChartDTO chartMedicionGeneralAusentismo = new GraficosChartDTO();

                List<DatasetsChartDTO> listaDataSet = new List<DatasetsChartDTO>();

                listaRegistro.ForEach(registro =>
                {
                    DatasetsChartDTO dataSet = new DatasetsChartDTO();

                    dataSet.backgroundColor = new List<string>() { registro.backgroundColor };
                    dataSet.backgroundColor = new List<string> { registro.backgroundColor, registro.backgroundColor, registro.backgroundColor, registro.backgroundColor };
                    dataSet.borderColor = ParametrosGenerales.Color_Black;
                    dataSet.data = new List<int>()
                    {
                        registro.NumeroTotalIncapacidades,
                        registro.NumeroTotalDias,
                        registro.NumeroIncapacidadesPorEnfermedadGeneral_AccidenteComun,
                        registro.NumeroDiasPorEnfermedadGeneral_AccidenteComun
                    };

                    listaDataSet.Add(dataSet);
                });

                chartMedicionGeneralAusentismo.StrTitulo = "Medición general de ausentismo";
                chartMedicionGeneralAusentismo.labels = new List<string> { "N° Total de Incapacidades", "N° Total de Dias Incapacidad", tipoDeAusentismo == enumTiposGlobalesDeIncapacidad.EG_AC.ToString() ? "N° Incapacidades EG AC" : "N° Incapacidades AT EL", tipoDeAusentismo == enumTiposGlobalesDeIncapacidad.EG_AC.ToString() ? "N° Dias EG AC" : "N° Dias AT EL" };
                chartMedicionGeneralAusentismo.datasets = listaDataSet;

                return chartMedicionGeneralAusentismo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_IndiceDeFrecuenciaPorCausaMedica(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var tipoGeneralDeAusentismo = enumTiposGlobalesDeIncapacidad.EG_AC;
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalIncapacidadesPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_NumeroDeIncapacidadesPorPeriodo(listaTiempos, tipoGeneralDeAusentismo);
                var totalHorasHombreTrabajadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeFrecuenciaPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalIncapacidadesPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableHorasHombre = totalHorasHombreTrabajadas.FirstOrDefault(x => x.Numero == item.Numero);

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableHorasHombre.Valor != 0 ? item.Valor / variableHorasHombre.Valor : 0;

                    indiceDeFrecuenciaPorCausaMedicaPorMes.Add(indicePorMes);

                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeFrecuenciaPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_IndiceDeSeveridadPorCausaMedica(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var tipoGeneralDeAusentismo = enumTiposGlobalesDeIncapacidad.EG_AC;
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalDiasIncapacidadPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_NumeroDeDiasDeIncapacidadPorPeriodo(listaTiempos, tipoGeneralDeAusentismo);
                var totalHorasHombreProgramadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreProgramadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalDiasIncapacidadPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableHorasHombre = totalHorasHombreProgramadas.FirstOrDefault(x => x.Numero == item.Numero);

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableHorasHombre.Valor != 0 ? item.Valor / variableHorasHombre.Valor * 240 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);

                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_TasaDeAusentismoPorCausaMedica(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var tipoGeneralDeAusentismo = enumTiposGlobalesDeIncapacidad.EG_AC;
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var numeroIncapacidadesPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_NumeroDeDiasDeIncapacidadPorPeriodo(listaTiempos, tipoGeneralDeAusentismo);
                var numeroTrabajadoresPorPeriodo = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                numeroIncapacidadesPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = numeroTrabajadoresPorPeriodo.FirstOrDefault(x => x.Numero == item.Numero);

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 100 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_IncidenciaYPrevalenciaDeAusentismoPorCausaMedica(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var tipoGeneralDeAusentismo = enumTiposGlobalesDeIncapacidad.EG_AC;
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var numeroIncapacidadesPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_NumeroDeDiasDeIncapacidadPorPeriodo(listaTiempos, tipoGeneralDeAusentismo);
                var numeroTrabajadoresPorPeriodo = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                numeroIncapacidadesPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = numeroTrabajadoresPorPeriodo.FirstOrDefault(x => x.Numero == item.Numero);

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 100 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_ReporteEInvestigacionAT_EL(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var tipoGeneralDeAusentismo = enumTiposGlobalesDeIncapacidad.AT_EL;
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var numeroIncapacidadesPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_NumeroDeDiasDeIncapacidadPorPeriodo(listaTiempos, tipoGeneralDeAusentismo);
                var numeroTrabajadoresPorPeriodo = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                numeroIncapacidadesPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = numeroTrabajadoresPorPeriodo.FirstOrDefault(x => x.Numero == item.Numero);

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 100 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_IndiceDeFrecuenciaAT(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalDeAccidentesDeTrabajoPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_Cantidad_AT_PorPeriodo(listaTiempos);
                var totalHorasHombreTrabajadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalDeAccidentesDeTrabajoPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = totalHorasHombreTrabajadas.FirstOrDefault(x => x.Numero == item.Numero);

                    if (item.Numero == 2)
                    {

                    }

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 1 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_IndiceDeFrecuenciaEL(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalDeAccidentesDeTrabajoPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_Cantidad_AT_PorPeriodo(listaTiempos);
                var totalHorasHombreTrabajadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalDeAccidentesDeTrabajoPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = totalHorasHombreTrabajadas.FirstOrDefault(x => x.Numero == item.Numero);

                    if (item.Numero == 2)
                    {

                    }

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 1 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_Mortalidad(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalDeAccidentesDeTrabajoPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_Cantidad_AT_PorPeriodo(listaTiempos);
                var totalHorasHombreTrabajadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalDeAccidentesDeTrabajoPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = totalHorasHombreTrabajadas.FirstOrDefault(x => x.Numero == item.Numero);

                    if (item.Numero == 2)
                    {

                    }

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 1 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_IndiceDeSeveridadAT_EL(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalDeAccidentesDeTrabajoPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_Cantidad_AT_PorPeriodo(listaTiempos);
                var totalHorasHombreTrabajadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalDeAccidentesDeTrabajoPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = totalHorasHombreTrabajadas.FirstOrDefault(x => x.Numero == item.Numero);

                    if (item.Numero == 2)
                    {

                    }

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 1 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_TasaDeAccidentalidad(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalDeAccidentesDeTrabajoPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_Cantidad_AT_PorPeriodo(listaTiempos);
                var totalHorasHombreTrabajadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalDeAccidentesDeTrabajoPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = totalHorasHombreTrabajadas.FirstOrDefault(x => x.Numero == item.Numero);

                    if (item.Numero == 2)
                    {

                    }

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 1 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_IncidenciaYPrevalenciaDeAccidentesDEnfermedadLaboral(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalDeAccidentesDeTrabajoPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_Cantidad_AT_PorPeriodo(listaTiempos);
                var totalHorasHombreTrabajadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalDeAccidentesDeTrabajoPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = totalHorasHombreTrabajadas.FirstOrDefault(x => x.Numero == item.Numero);

                    if (item.Numero == 2)
                    {

                    }

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 1 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task<GraficosChartDTO> ObtenerInformacionParaGraficar_LesionesIncapacitantes(AusentismoIndicadoresDTO ausentismoIndicadorDTO, int anio)
        {
            try
            {
                var listaTiempos = await _iAusentismoTiemposCoreBusiness.ObtenerTiemposDeAusentismoPorAnio(anio);
                var listaVariables = await _iAusentismoVariablesCoreBusiness.ObtenerVariablesPorAnioAsync(anio);

                var totalDeAccidentesDeTrabajoPorPeriodo = _iAusentismoTiemposCoreBusiness.Obtener_Cantidad_AT_PorPeriodo(listaTiempos);
                var totalHorasHombreTrabajadas = _iAusentismoVariablesCoreBusiness.Obtener_NumeroHorasHombreTrabajadasPorPeriodo(listaVariables);

                List<MesesDelAnioDTO> indiceDeSeveridadPorCausaMedicaPorMes = new List<MesesDelAnioDTO>();
                totalDeAccidentesDeTrabajoPorPeriodo.OrderBy(x => x.Numero).ToList().ForEach(item =>
                {
                    var variableTrabajadoresPorPeriodo = totalHorasHombreTrabajadas.FirstOrDefault(x => x.Numero == item.Numero);

                    if (item.Numero == 2)
                    {

                    }

                    MesesDelAnioDTO indicePorMes = new MesesDelAnioDTO();

                    indicePorMes.Numero = item.Numero;
                    indicePorMes.Nombre = item.Nombre;
                    indicePorMes.Valor = variableTrabajadoresPorPeriodo.Valor != 0 ? item.Valor / variableTrabajadoresPorPeriodo.Valor * 1 : 0;

                    indiceDeSeveridadPorCausaMedicaPorMes.Add(indicePorMes);
                });

                var informacionGrafico = this.GenerarGraficoDeIndicador(ausentismoIndicadorDTO, indiceDeSeveridadPorCausaMedicaPorMes);

                return informacionGrafico;
            }
            catch (Exception)
            {

                throw;
            }
        }
        




        private GraficosChartDTO GenerarGraficoDeIndicador(AusentismoIndicadoresDTO ausentismoIndicadorDTO, List<MesesDelAnioDTO> listaValoresGraficoPrincipal)
        {
            try
            {
                GraficosChartDTO chartAusentismo = new GraficosChartDTO();
                chartAusentismo.StrTitulo = ausentismoIndicadorDTO.StrNombreIndicador;
                chartAusentismo.datasets = new List<DatasetsChartDTO>();
                chartAusentismo.labels = new List<string>();

                // Creación del dataset principal
                DatasetsChartDTO dataSetPrincipal = new DatasetsChartDTO();
                dataSetPrincipal.data = new List<int>();
                dataSetPrincipal.type = enumTiposDeGrafico.bar.ToString();
                dataSetPrincipal.label = "Dias";
                dataSetPrincipal.backgroundColor = new List<string>();
                dataSetPrincipal.borderColor = ParametrosGenerales.Color_Black;
                dataSetPrincipal.order = ausentismoIndicadorDTO.InformacionPorEmpresaDTO.StrTipoGrafico == enumTiposDeGrafico.line.ToString() ? 1 : 0;

                foreach (var item in listaValoresGraficoPrincipal)
                {
                    dataSetPrincipal.backgroundColor.Add(ParametrosGenerales.Color_Primary);
                    chartAusentismo.labels.Add(item.Nombre);
                    dataSetPrincipal.data.Add((int)item.Valor);
                }

                //Creación del dataset lineal con la meta comparativa
                if (ausentismoIndicadorDTO.InformacionPorEmpresaDTO != null)
                {
                    DatasetsChartDTO dataSetLineal = new DatasetsChartDTO();
                    dataSetLineal.data = new List<int>();
                    dataSetLineal.type = ausentismoIndicadorDTO.InformacionPorEmpresaDTO.StrTipoGrafico;

                    if (ausentismoIndicadorDTO.InformacionPorEmpresaDTO.StrTipoGrafico == "bar")
                        dataSetLineal.backgroundColor = new List<string>();

                    dataSetLineal.borderColor = ParametrosGenerales.Color_Warning;
                    dataSetLineal.label = "Meta";
                    dataSetLineal.order = ausentismoIndicadorDTO.InformacionPorEmpresaDTO.StrTipoGrafico == enumTiposDeGrafico.line.ToString() ? 0 : 1;

                    foreach (var item in listaValoresGraficoPrincipal)
                    {
                        if (ausentismoIndicadorDTO.InformacionPorEmpresaDTO.StrTipoGrafico == "bar")
                            dataSetLineal.backgroundColor.Add(ParametrosGenerales.Color_Warning);

                        var valorDelMes = 0;
                        valorDelMes = item.Numero == (int)EnumMesesDelAño.Enero ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_1 :
                            item.Numero == (int)EnumMesesDelAño.Febrero ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_2 :
                            item.Numero == (int)EnumMesesDelAño.Marzo ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_3 :
                            item.Numero == (int)EnumMesesDelAño.Abril ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_4 :
                            item.Numero == (int)EnumMesesDelAño.Mayo ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_5 :
                            item.Numero == (int)EnumMesesDelAño.Junio ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_6 :
                            item.Numero == (int)EnumMesesDelAño.Julio ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_7 :
                            item.Numero == (int)EnumMesesDelAño.Agosto ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_8 :
                            item.Numero == (int)EnumMesesDelAño.Septiembre ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_9 :
                            item.Numero == (int)EnumMesesDelAño.Octubre ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_10 :
                            item.Numero == (int)EnumMesesDelAño.Noviembre ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_11 :
                            item.Numero == (int)EnumMesesDelAño.Diciembre ? ausentismoIndicadorDTO.InformacionPorEmpresaDTO.IntMeta_12 : 0;

                        dataSetLineal.data.Add(valorDelMes);

                        dataSetLineal.fill = false;
                        dataSetLineal.tension = Convert.ToDecimal(0.1);
                    }

                    chartAusentismo.datasets.Add(dataSetPrincipal);
                    chartAusentismo.datasets.Add(dataSetLineal);
                }

                return chartAusentismo;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion



    }
}
