using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class AusentismoCoreBusiness : CRUDGenerico<Ausentismo>, IAusentismoCoreBusiness
    {
        #region Variables
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private string rutaArchivoDeExcel = string.Empty;
        private ICommonCoreBusiness _iCommonCoreBusiness => new CommonCoreBusiness();
        private IEmpleadosCoreBusiness _iEmpleadosCoreBusiness;
        private ITipoEventosAusentismoCoreBusiness _iTipoEventosAusentismoCoreBusiness;
        private IDiagnosticosCoreBusiness _iDiagnosticosCoreBusiness;
        private IParametrosCoreBusiness _iParametrosCoreBusiness;
        private IAusentismoTiemposCoreBusiness _iAusentismoTiemposCoreBusiness;
        private IAreasCoreBusiness _iAreasCoreBusiness;
        private IProcesosCoreBusiness _iProcesosCoreBusiness;
        private IAusentismoProrrogasCoreBusiness _iAusentismoProrrogasCoreBusiness;

        public AusentismoCoreBusiness() : base(new gestioni_consultorNetEntities(new ServicioTercero()))
        {
            _iEmpleadosCoreBusiness = new EmpleadosCoreBusiness();
            _iTipoEventosAusentismoCoreBusiness = new TipoEventosAusentismoCoreBusiness();
            _iDiagnosticosCoreBusiness = new DiagnosticosCoreBusiness();
            _iParametrosCoreBusiness = new ParametrosCoreBusiness();
            _iAusentismoTiemposCoreBusiness = new AusentismoTiemposCoreBusiness();
            _iAreasCoreBusiness = new AreasCoreBusiness();
            _iProcesosCoreBusiness = new ProcesosCoreBusiness();
            _iAusentismoProrrogasCoreBusiness = new AusentismoProrrogasCoreBusiness();
        }

        #endregion

        #region CRUD Generico
        public async Task<List<AusentismoDTO>> ObtenerListaAusentismoDTOAsync(DateTime? fechaInicial = null, DateTime? fechaFinal = null)
        {
            try
            {
                var incial = fechaInicial == null ? DateTime.Now.AddYears(-100) : fechaInicial;
                var final = fechaFinal == null ? DateTime.Now.AddYears(100) : fechaInicial;

                var listaAusentismo = await this.FindWhereAsync(x => (x.DatFechaInicial >= incial && x.DatFechaFinal <= final));

                var listaAusentismoDTO = listaAusentismo.Select(x => new AusentismoDTO()
                {
                    TiemposAusentismo = x.AusentismoTiempos.ToList(),
                    IntAusentismoID = x.IntAusentismoID,
                    DatFechaInicial = x.DatFechaInicial,
                    DatFechaFinal = x.DatFechaFinal,
                    StrObservaciones = x.StrObservaciones,
                    IntEmpleadoID = x.IntEmpleadoID,
                    StrIdentificacionEmpleado = x.Empleados.StrIdentificacion,
                    StrNombreEmpleado = x.Empleados.StrNombreCompleto,
                    StrImagenEmpleado = Archivos.GetUrlImagenEmpleadoOrUrlDefault(x.Empleados.StrRutaImagen),
                    IntTipoEventoID = x.IntTipoEventoAusentismoID,
                    StrDescripcionEvento = x.TipoEventosAusentismo.StrDescripcion,
                    IntCantidadProrrogas = x.AusentismoProrrogas.ToList().Count(),
                    IntDiagnosticoID = x.IntDiagnosticoID,
                    StrCodigoDiagnostico = x.Diagnosticos.StrCodigo,
                    StrDescripcionDiagnostico = x.Diagnosticos.StrDescripcion,
                    StrCargo = x.Empleados.Cargos.StrDescripcion

                }).ToList();

                return listaAusentismoDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(Ausentismo entity)
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

        public new async Task<string> CreateAsync(Ausentismo entity)
        {
            try
            {
                entity.IntAusentismoID = entity.IntAusentismoID;
                entity.DatFechaInicial = entity.DatFechaInicial;
                entity.DatFechaFinal = entity.DatFechaFinal;
                entity.StrObservaciones = entity.StrObservaciones;
                entity.IntEmpleadoID = entity.IntEmpleadoID;
                entity.IntTipoEventoAusentismoID = entity.IntTipoEventoAusentismoID;
                entity.IntDiagnosticoID = entity.IntDiagnosticoID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Ausentismo entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Ausentismo> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Ausentismo, bool>> match)
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

        public new async Task<Ausentismo> FindAsync(Expression<Func<Ausentismo, bool>> match)
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

        public new List<Ausentismo> GetAll()
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

        public new async Task<List<Ausentismo>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Ausentismo entity)
        {
            try
            {
                var Ausentismo = await this.FindAsync(x => x.IntAusentismoID == entity.IntAusentismoID);

                Ausentismo.IntAusentismoID = entity.IntAusentismoID;
                Ausentismo.DatFechaInicial = entity.DatFechaInicial;
                Ausentismo.DatFechaFinal = entity.DatFechaFinal;
                Ausentismo.StrObservaciones = entity.StrObservaciones;
                Ausentismo.IntEmpleadoID = entity.IntEmpleadoID;
                Ausentismo.IntTipoEventoAusentismoID = entity.IntTipoEventoAusentismoID;
                Ausentismo.IntDiagnosticoID = entity.IntDiagnosticoID;

                await base.UpdateAsync(Ausentismo);

                await _iAusentismoTiemposCoreBusiness.GuardarTiempoDeIncapacidadPorMesAsync(Ausentismo);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Ausentismo model)
        {
            using (var unitOfWork = new gestioni_consultorNetEntities())
            {
                using (var transaction = unitOfWork.Database.BeginTransaction())
                {
                    try
                    {
                        var respuesta = string.Empty;


                        if (!string.IsNullOrEmpty(respuesta))
                            return respuesta;

                        if (model.IntAusentismoID != 0)
                        {
                            var respuestaValidarIncapacidad = await this.ValidarSiExisteIncapacidadPorAusentismoAsync(model.IntAusentismoID, model.DatFechaInicial, model.DatFechaFinal);

                            if (!string.IsNullOrEmpty(respuestaValidarIncapacidad))
                                return respuestaValidarIncapacidad;

                            respuesta = await this.UpdateAsync(model);
                            if (!string.IsNullOrEmpty(respuesta))
                                return respuesta;

                            var listaAusentismoTiempos = await _iAusentismoTiemposCoreBusiness.FindWhereAsync(x => x.IntAusentismoID == model.IntAusentismoID);
                            await _iAusentismoTiemposCoreBusiness.DeleteRangeAsync(listaAusentismoTiempos);
                            await _iAusentismoTiemposCoreBusiness.GuardarTiempoDeIncapacidadPorMesAsync(model);

                        }
                        else
                        {
                            var respuestaValidarIncapacidad = await this.ValidarSiExisteIncapacidadPorEmpleadoAsync(model, model.DatFechaInicial, model.DatFechaFinal);

                            if (!string.IsNullOrEmpty(respuestaValidarIncapacidad))
                                return respuestaValidarIncapacidad;

                            respuesta = await this.CreateAsync(model);
                            if (!string.IsNullOrEmpty(respuesta))
                                return respuesta;

                            await _iAusentismoTiemposCoreBusiness.GuardarTiempoDeIncapacidadPorMesAsync(model);
                        }

                        transaction.Commit();

                        return respuesta;

                    }
                    catch (DbEntityValidationException ex)
                    {
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        var fullErrorMessage = string.Join("; ", errorMessages);

                        var exceptionMessage = string.Concat(ex.Message, " Los errores de validación son: ", fullErrorMessage);
                        transaction.Rollback();
                        throw new Exception(exceptionMessage);
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        throw ex;
                    }

                }
            }

        }

        public async Task<List<AusentismoProrrogas>> ObtenerProrrogasAsync(int ausentismoID)
        {
            try
            {
                return await _iAusentismoProrrogasCoreBusiness.FindWhereAsync(x => x.IntAusentismoID == ausentismoID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> AgregarProrrogaAsync(AusentismoProrrogas modelo)
        {
            try
            {
                var ausentismoModelo = await this.FindAsync(x => x.IntAusentismoID == modelo.IntAusentismoID);

                var respuesta = string.Empty;
                bool esProrroga = true;
                respuesta = await this.ValidarSiExisteIncapacidadPorEmpleadoAsync(ausentismoModelo, modelo.DatFechaInicial, modelo.DatFechaFinal, esProrroga);

                if (!string.IsNullOrEmpty(respuesta))
                    return respuesta;

                var respuestaProrroga = await _iAusentismoProrrogasCoreBusiness.SaveAllAsync(modelo);

                if (!string.IsNullOrEmpty(respuestaProrroga))
                    return respuestaProrroga;

                return String.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> DeleteProrrogaAsync(int prorrogaID)
        {
            try
            {
                var modeloProrroga = await _iAusentismoProrrogasCoreBusiness.FindAsync(x => x.IntProrrogaID == prorrogaID);
                int ausentismoID = modeloProrroga.IntAusentismoID;
                await _iAusentismoProrrogasCoreBusiness.DeleteAsync(modeloProrroga);

                var listaAusentismoTiempos = await _iAusentismoTiemposCoreBusiness.FindWhereAsync(x => x.IntAusentismoID == ausentismoID);
                await _iAusentismoTiemposCoreBusiness.DeleteRangeAsync(listaAusentismoTiempos);

                Ausentismo ausentismoModelo = await this.FindAsync(x => x.IntAusentismoID == ausentismoID);

                await _iAusentismoTiemposCoreBusiness.GuardarTiempoDeIncapacidadPorMesAsync(ausentismoModelo);

                return ausentismoID.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Agrupación mesual
        public async Task<List<AusentismoAgrupadoMensualmentePorAnioDTO>> GetAusentismoAgrupadoMensualmentePorAñoAsync(short anio)
        {
            try
            {
                var listaMesesAño = Common.GetEnumToSelectList<EnumMesesDelAño>();
                var listaAusentismoTiempos = await _iAusentismoTiemposCoreBusiness.GetAllAsync();
                var listaTipoEventos = await _iTipoEventosAusentismoCoreBusiness.GetAllAsync();

                List<AusentismoAgrupadoMensualmentePorAnioDTO> listaAusentismoAgrupadoPorAño = new List<AusentismoAgrupadoMensualmentePorAnioDTO>();

                foreach (var item in listaMesesAño)
                {
                    var Periodo = Convert.ToByte(item.Value);
                    var ausentismoPorPeriodo = new AusentismoAgrupadoMensualmentePorAnioDTO()
                    {
                        Año = anio,
                        Mes = Periodo,
                        MesNombre = item.Text,
                        ListaTipoEventos = listaTipoEventos,
                        ListaAusentismoTiempos = listaAusentismoTiempos.Where(x => x.IntAño == anio && x.TIntPeriodo == Periodo).ToList()
                    };

                    listaAusentismoAgrupadoPorAño.Add(ausentismoPorPeriodo);
                }

                return listaAusentismoAgrupadoPorAño;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AusentismoAgrupadoPorEmpleadoDTO> GetAusentismoAgrupadoMensualmentePorEmpleadoAsync(short anio, byte periodo)
        {
            try
            {
                var listaAusentismoTiempos = await _iAusentismoTiemposCoreBusiness.GetAllAsync();
                var listaEmpleados = await _iEmpleadosCoreBusiness.GetAllAsync();
                var listaTipoEventos = await _iTipoEventosAusentismoCoreBusiness.GetAllAsync();
                var listaMesesAño = Common.GetEnumToSelectList<EnumMesesDelAño>();
                var periodoActual = periodo.ToString();

                var ausentismoPorEmpleado = new AusentismoAgrupadoPorEmpleadoDTO()
                {
                    Año = anio,
                    Mes = periodo,
                    NombreMes = listaMesesAño.Where(x => x.Value == periodoActual).FirstOrDefault().Text,
                    TituloModal = listaMesesAño.Where(x => x.Value == periodoActual).FirstOrDefault().Text,
                    ListaAusentismoTiempos = listaAusentismoTiempos.Where(x => x.IntAño == anio && x.TIntPeriodo == periodo).ToList(),
                    ListaEmpleados = listaEmpleados.Where(x => listaAusentismoTiempos.Any(y => y.IntEmpleadoID == x.IntEmpleadoID)).ToList(),
                    ListaTipoEventos = listaTipoEventos
                };

                return ausentismoPorEmpleado;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Agrupación por area
        public async Task<AusentismoAgrupadoPorAreaDTO> GetAusentismoAgrupadoPorAreaAsync(short anio)
        {
            try
            {
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                var listaAreas = await _iAreasCoreBusiness.GetAllAsync();
                var listaAusentismoTiempos = await _iAusentismoTiemposCoreBusiness.GetAllAsync();
                var listaTipoEventos = await _iTipoEventosAusentismoCoreBusiness.GetAllAsync();

                var ausentismoPorArea = new AusentismoAgrupadoPorAreaDTO()
                {
                    Año = anio,
                    ListaAreas = listaAreas.Where(x => x.IntTerceroID == terceroID).ToList(),
                    ListaAusentismoTiempos = listaAusentismoTiempos.Where(x => x.IntAño == anio).ToList(),
                    ListaTipoEventos = listaTipoEventos
                };

                return ausentismoPorArea;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AusentismoAgrupadoPorEmpleadoDTO> GetAusentismoAgrupadoPorArea_EmpleadoAsync(short anio, int areaID)
        {
            try
            {
                var listaAusentismoTiempos = await _iAusentismoTiemposCoreBusiness.GetAllAsync();
                var listaEmpleados = await _iEmpleadosCoreBusiness.GetAllAsync();
                var listaTipoEventos = await _iTipoEventosAusentismoCoreBusiness.GetAllAsync();
                var listaAreas = await _iAreasCoreBusiness.GetAllAsync();

                var ausentismoPorEmpleado = new AusentismoAgrupadoPorEmpleadoDTO()
                {
                    Año = anio,
                    AreaID = areaID,
                    AreaDescripcion = listaAreas.Where(x => x.IntAreaID == areaID).FirstOrDefault().StrDescripcion,
                    TituloModal = listaAreas.Where(x => x.IntAreaID == areaID).FirstOrDefault().StrDescripcion,
                    ListaAusentismoTiempos = listaAusentismoTiempos.Where(x => x.IntAño == anio && x.IntAreaID == areaID).ToList(),
                    ListaEmpleados = listaEmpleados.Where(x => listaAusentismoTiempos.Any(y => y.IntEmpleadoID == x.IntEmpleadoID)).ToList(),
                    ListaTipoEventos = listaTipoEventos
                };

                return ausentismoPorEmpleado;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Agrupación por proceso
        public async Task<AusentismoAgrupadoPorProcesoDTO> GetAusentismoAgrupadoPorProcesoAsync(short anio)
        {
            try
            {
                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                var listaProcesos = await _iProcesosCoreBusiness.GetAllAsync();
                var listaAusentismoTiempos = await _iAusentismoTiemposCoreBusiness.GetAllAsync();
                var listaTipoEventos = await _iTipoEventosAusentismoCoreBusiness.GetAllAsync();

                var ausentismoPorProceso = new AusentismoAgrupadoPorProcesoDTO()
                {
                    Año = anio,
                    ListaProcesos = listaProcesos.Where(x => x.IntTerceroClienteID == terceroID).ToList(),
                    ListaAusentismoTiempos = listaAusentismoTiempos.Where(x => x.IntAño == anio).ToList(),
                    ListaTipoEventos = listaTipoEventos
                };

                return ausentismoPorProceso;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task<AusentismoAgrupadoPorEmpleadoDTO> GetAusentismoAgrupadoPorProceso_EmpleadoAsync(short anio, int procesoID)
        //{
        //    try
        //    {
        //        var listaAusentismoTiempos = await _iAusentismoTiemposCoreBusiness.GetAllAsync();
        //        var listaEmpleados = await _iEmpleadosCoreBusiness.GetAllAsync();
        //        var listaTipoEventos = await _iTipoEventosAusentismoCoreBusiness.GetAllAsync();
        //        var listaProcesos = await _iProcesosCoreBusiness.GetAllAsync();

        //        var ausentismoPorEmpleado = new AusentismoAgrupadoPorEmpleadoDTO()
        //        {
        //            Año = anio,
        //            ProcesoID = procesoID,
        //            ProcesoDescripcion = listaProcesos.Where(x => x.IntProcesoID == procesoID).FirstOrDefault().StrDescripcion,
        //            TituloModal = listaProcesos.Where(x => x.IntProcesoID == procesoID).FirstOrDefault().StrDescripcion,
        //            ListaAusentismoTiempos = listaAusentismoTiempos.Where(x => x.IntAño == anio && x.IntProcesoID == procesoID).ToList(),
        //            ListaEmpleados = listaEmpleados.Where(x => listaAusentismoTiempos.Any(y => y.IntEmpleadoID == x.IntEmpleadoID)).ToList(),
        //            ListaTipoEventos = listaTipoEventos
        //        };

        //        return ausentismoPorEmpleado;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        #endregion

        #region SelectList
        public async Task<SelectList> SelectListEmpleadosAsync(string valueSelected = null)
        {
            try
            {
                return await _iEmpleadosCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListTipoEvento(string valueSelected = null)
        {
            try
            {
                return await _iTipoEventosAusentismoCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SelectList> SelectListDiagnosticos(string valueSelected = null)
        {
            try
            {
                return await _iDiagnosticosCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Validaciones
        public async Task<Empleados> ObtenerEmpleadoConImageUrlAsync(int empleadoID)
        {
            try
            {
                var pathImagen = Archivos.rutaImagenUserDefault;

                var modeloEmpleado = await _iEmpleadosCoreBusiness.FindAsync(x => x.IntEmpleadoID == empleadoID);

                if (modeloEmpleado is null)
                    return new Empleados() { StrRutaImagen = Archivos.ConvertirRutaEnUrl(pathImagen) };

                if (!string.IsNullOrEmpty(modeloEmpleado.StrRutaImagen))
                {
                    string rutaArchivoFull = Archivos.ObtenerRutaDeImagenEmpleado(modeloEmpleado.StrRutaImagen);

                    if (File.Exists(rutaArchivoFull))
                        pathImagen = $"{Archivos.rutaImagenEmpleado}/{modeloEmpleado.StrRutaImagen}";

                }

                modeloEmpleado.StrRutaImagen = Archivos.ConvertirRutaEnUrl(pathImagen);

                return modeloEmpleado;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Diagnosticos> BuscarDiagnosticoAsync(string valor)
        {
            try
            {
                var listaDiagnosticos = await _iDiagnosticosCoreBusiness.GetAllAsync();
                var existeDiagnostico = listaDiagnosticos.Any(x => x.StrCodigo.ToLower().Trim() == valor.ToLower().Trim());

                if (existeDiagnostico)
                    return await _iDiagnosticosCoreBusiness.FindAsync(x => x.StrCodigo == valor);

                return new Diagnosticos();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<string> ValidarSiExisteIncapacidadPorEmpleadoAsync(Ausentismo modeloAusentismo, DateTime fechaInicial, DateTime fechaFinal, bool esProrroga = false)
        {
            try
            {
                var listaIncapacidadesPorEmpleado = await this.FindWhereAsync(x => x.IntEmpleadoID == modeloAusentismo.IntEmpleadoID);
                var listaprorrogasPorEmpleado = _iAusentismoProrrogasCoreBusiness.GetAll().Where(x => listaIncapacidadesPorEmpleado.Any(y => y.IntAusentismoID == x.IntAusentismoID)).ToList();

                var respuesta = this.ValidarIncapacidadEnAusentismos(modeloAusentismo, listaIncapacidadesPorEmpleado, listaprorrogasPorEmpleado, fechaInicial, fechaFinal, esProrroga);

                if (!string.IsNullOrEmpty(respuesta))
                    return respuesta;

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<string> ValidarSiExisteIncapacidadPorAusentismoAsync(int ausentismoID, DateTime fechaInicial, DateTime fechaFinal, bool esProrroga = false)
        {
            try
            {
                var modeloAusentismo = await this.FindAsync(x => x.IntAusentismoID == ausentismoID);

                var listaIncapacidadesPorEmpleado = await this.FindWhereAsync(x => x.IntEmpleadoID == modeloAusentismo.IntEmpleadoID);
                var listaprorrogasPorEmpleado = _iAusentismoProrrogasCoreBusiness.GetAll().Where(x => listaIncapacidadesPorEmpleado.Any(y => y.IntAusentismoID == x.IntAusentismoID)).ToList();

                var respuesta = this.ValidarIncapacidadEnAusentismos(modeloAusentismo, listaIncapacidadesPorEmpleado, listaprorrogasPorEmpleado, fechaInicial, fechaFinal, esProrroga);

                if (!string.IsNullOrEmpty(respuesta))
                    return respuesta;

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string ValidarIncapacidadEnAusentismos(Ausentismo modelo, List<Ausentismo> listaAusentismos, List<AusentismoProrrogas> listaProrrogas, DateTime fechaInicial, DateTime fechaFinal, bool esProrroga = false)
        {
            try
            {
                bool existeIncapacidad = false;
                if (modelo.IntAusentismoID != 0 && !esProrroga)
                    listaAusentismos = listaAusentismos.Where(x => x.IntAusentismoID != modelo.IntAusentismoID).ToList();

                existeIncapacidad = listaAusentismos.Any(x => (x.DatFechaInicial >= fechaInicial && x.DatFechaInicial <= fechaFinal) || (x.DatFechaFinal >= fechaInicial && x.DatFechaFinal <= fechaFinal) || (x.DatFechaInicial <= fechaInicial && x.DatFechaFinal >= fechaFinal));

                if (existeIncapacidad)
                    return RecursoAusentismo.msnEmpleadoConAusentismo;

                string respuestaProrrogas = this.ValidarIncapacidadEnProrrogas(modelo, listaProrrogas, fechaInicial, fechaFinal, esProrroga);

                if (!string.IsNullOrEmpty(respuestaProrrogas))
                    return respuestaProrrogas;

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private string ValidarIncapacidadEnProrrogas(Ausentismo modeloAusentismo, List<AusentismoProrrogas> listaProrrogas, DateTime fechaInicial, DateTime fechaFinal, bool esProrroga = false)
        {
            try
            {
                bool existeIncapacidad = listaProrrogas.Any(x => (x.DatFechaInicial >= fechaInicial && x.DatFechaInicial <= fechaFinal) || (x.DatFechaFinal >= fechaInicial && x.DatFechaFinal <= fechaFinal) || (x.DatFechaInicial <= fechaInicial && x.DatFechaFinal >= fechaFinal));

                if (existeIncapacidad)
                    return RecursoAusentismo.msnEmpleadoConProrroga;

                if (esProrroga)
                {
                    if (fechaInicial <= modeloAusentismo.DatFechaInicial)
                        return RecursoAusentismo.msnProrrogaMenosAIncapacidad;
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Costos ausentismo
        public async Task<AusentismoCostosDTO> ObtenerCostosDeAusentismoPorIDAsync(int ausentismoID)
        {
            try
            {
                var ausentismoModelo = await this.FindAsync(x => x.IntAusentismoID == ausentismoID);

                if (ausentismoModelo is null)
                    throw new Exception(RecursoCommon.msnRegistroNoEncontrado);

                AusentismoCostosDTO ausentismoCostosDTO = new AusentismoCostosDTO();
                ausentismoCostosDTO.Ausentismo = ausentismoModelo;

                return ausentismoCostosDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Archivo de Excel
        public async Task<List<ExcelErrorDTO>> CargarAusentismoDesdeExcelAsync(HttpFileCollectionBase archivoDeExcel, ParamFilesDTO paramFilesDTO)
        {
            try
            {
                var listaAusentismoExcelConNovedades = new List<AusentismoDTO>();

                var listaDatosArchivoExcel = await this.LeerArchivoDeExcelAsync(archivoDeExcel, paramFilesDTO);
                var listaDatosRevisados = await this.ObtenerNovedadesDeArchivoDeExcelAsync(listaDatosArchivoExcel);
                var listaDatosConNovedades = listaDatosRevisados.Where(x => x.ErrorArchivoExcel != string.Empty).ToList();
                var listaDatosSinNovedades = listaDatosRevisados.Where(x => x.ErrorArchivoExcel == string.Empty).ToList();

                var listaDatosConNovedadesAlGuardar = await this.GuardarAusentismoDesdeExcelAsync(listaDatosSinNovedades);

                listaAusentismoExcelConNovedades.AddRange(listaDatosConNovedades);
                listaAusentismoExcelConNovedades.AddRange(listaDatosConNovedadesAlGuardar);

                var listaNovedadesDTO = listaAusentismoExcelConNovedades.Select(x => new ExcelErrorDTO()
                {

                    NumeroRegistro = x.NumeroRegistro,
                    MensajeError = x.ErrorArchivoExcel

                }).ToList();

                return listaNovedadesDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<AusentismoDTO>> LeerArchivoDeExcelAsync(HttpFileCollectionBase archivoDeExcel, ParamFilesDTO paramFilesDTO)
        {
            try
            {
                var modeloTercero = await _iCommonCoreBusiness.GetTerceroModelFromCurrentUser();
                paramFilesDTO.nombreArchivo = $"Ausentismo_{modeloTercero.StrIdentificacion.Trim()}";

                var nombreArchivo = Archivos.GuardarArchivo(archivoDeExcel, paramFilesDTO);
                rutaArchivoDeExcel = Archivos.ObtenerRutaDeArchivoTemporal(nombreArchivo);

                var excelFile = new LinqToExcel.ExcelQueryFactory(rutaArchivoDeExcel);

                var ausentismoExcel =
                    from row in excelFile.Worksheet("Ausentismo")

                    let item = new
                    {
                        Empleado = row["Empleado"].Cast<string>(),
                        FechaInicial = row["FechaInicial"].Cast<DateTime>(),
                        FechaFinal = row["FechaFinal"].Cast<DateTime>(),
                        TipoEvento = row["TipoEvento"].Cast<string>(),
                        Diagnostico = row["Diagnostico"].Cast<string>(),
                        Observacion = row["Observacion"].Cast<string>(),
                    }

                    select new AusentismoDTO
                    {
                        StrIdentificacionEmpleado = item.Empleado,
                        DatFechaInicial = item.FechaInicial,
                        DatFechaFinal = item.FechaFinal,
                        StrTipoEvento = item.TipoEvento,
                        StrCodigoDiagnostico = item.Diagnostico,
                        StrObservaciones = item.Observacion,
                        TiemposAusentismo = new List<AusentismoTiempos>()
                    };

                var listaAusentismo = ausentismoExcel.ToList();

                if (listaAusentismo.Count() == 0)
                    throw new Exception(RecursoCommon.msnArchivoSinDatos);

                int contadorRegistros = 1;
                listaAusentismo.ForEach(registro =>
                {
                    registro.NumeroRegistro = contadorRegistros + 1;
                    contadorRegistros++;
                });

                Archivos.EliminarArchivo(rutaArchivoDeExcel);

                return listaAusentismo;

            }
            catch (Exception)
            {
                Archivos.EliminarArchivo(rutaArchivoDeExcel);
                throw;
            }
        }

        private async Task<List<AusentismoDTO>> ObtenerNovedadesDeArchivoDeExcelAsync(List<AusentismoDTO> ausentismoExcel)
        {
            try
            {
                List<AusentismoDTO> registrosConNovedades = new List<AusentismoDTO>();

                List<string> codigosDiagnosticos = new List<string>();
                ausentismoExcel.ForEach(registro => codigosDiagnosticos.Add(registro.StrCodigoDiagnostico));

                var listaEmpleados = await _iEmpleadosCoreBusiness.GetAllAsync();
                var listaTipoEventos = await _iTipoEventosAusentismoCoreBusiness.GetAllAsync();
                var listaDiagnosticos = await _iDiagnosticosCoreBusiness.FindWhereAsync(x => codigosDiagnosticos.Any(y => y == x.StrCodigo));
                var terceroUsuarioID = _iCommonCoreBusiness.GetTerceroIDFromCurrentUser();

                ausentismoExcel.Where(x => !listaEmpleados.Any(y => y.StrIdentificacion == x.StrIdentificacionEmpleado)).ToList()
                    .ForEach(registro =>
                    {
                        registro.ErrorArchivoExcel = String.Format("El tercero {0} no existe en la base de datos", registro.StrIdentificacionEmpleado);
                    });

                ausentismoExcel.Where(x => !listaTipoEventos.Any(y => y.StrCodigo == x.StrTipoEvento)).ToList()
                    .ForEach(registro =>
                    {
                        registro.ErrorArchivoExcel = String.Format("El tipo de evento {0} no existe en la base de datos", registro.StrTipoEvento);
                    });

                ausentismoExcel.Where(x => !listaDiagnosticos.Any(y => y.StrCodigo == x.StrCodigoDiagnostico)).ToList()
                   .ForEach(registro =>
                   {
                       registro.ErrorArchivoExcel = String.Format("El código de diagnóstico {0} no existe en la base de datos", registro.StrCodigoDiagnostico);
                   });

                ausentismoExcel.Where(x => x.ErrorArchivoExcel == string.Empty).ToList()
                    .ForEach(registro =>
                    {
                        registro.IntEmpleadoID = listaEmpleados.Find(x => x.StrIdentificacion == registro.StrIdentificacionEmpleado).IntEmpleadoID;
                        registro.IntTipoEventoID = listaTipoEventos.Find(x => x.StrCodigo == registro.StrTipoEvento).IntTipoEventoAusentismoID;
                        registro.IntDiagnosticoID = listaDiagnosticos.Find(x => x.StrCodigo == registro.StrCodigoDiagnostico).IntDiagnosticoID;
                        registro.IntTerceroID = terceroUsuarioID;

                    });

                return ausentismoExcel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<AusentismoDTO>> GuardarAusentismoDesdeExcelAsync(List<AusentismoDTO> ausentismoExcel)
        {
            try
            {
                var listaAusentismoParaCrear = ausentismoExcel.Select(x => new Ausentismo()
                {
                    DatFechaInicial = x.DatFechaInicial,
                    DatFechaFinal = x.DatFechaFinal,
                    StrObservaciones = x.StrObservaciones,
                    IntTerceroID = x.IntTerceroID,
                    IntEmpleadoID = x.IntEmpleadoID,
                    IntTipoEventoAusentismoID = x.IntTipoEventoID,
                    IntDiagnosticoID = x.IntDiagnosticoID

                }).ToList();

                if (listaAusentismoParaCrear.Count() != 0)
                {
                    foreach (var item in listaAusentismoParaCrear)
                    {
                        var respuesta = await this.SaveAllAsync(item);

                        if (!string.IsNullOrEmpty(respuesta))
                            ausentismoExcel.Find(x => x.IntEmpleadoID == item.IntEmpleadoID && x.DatFechaInicial == item.DatFechaInicial && x.DatFechaFinal == item.DatFechaFinal && x.IntTipoEventoID == item.IntTipoEventoAusentismoID).ErrorArchivoExcel = respuesta;
                    }
                }

                return ausentismoExcel.Where(x => x.ErrorArchivoExcel != string.Empty).ToList();

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

        ~AusentismoCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (_iCommonCoreBusiness != null)
                //    _iCommonCoreBusiness.Dispose();

                if (_iEmpleadosCoreBusiness != null)
                {
                    _iEmpleadosCoreBusiness.Dispose();
                    _iEmpleadosCoreBusiness = null;
                }

                if (_iTipoEventosAusentismoCoreBusiness != null)
                {
                    _iTipoEventosAusentismoCoreBusiness.Dispose();
                    _iTipoEventosAusentismoCoreBusiness = null;
                }

                if (_iDiagnosticosCoreBusiness != null)
                {
                    _iDiagnosticosCoreBusiness.Dispose();
                    _iDiagnosticosCoreBusiness = null;
                }

                if (_iParametrosCoreBusiness != null)
                {
                    _iParametrosCoreBusiness.Dispose();
                    _iParametrosCoreBusiness = null;
                }

                if (_iAusentismoTiemposCoreBusiness != null)
                {
                    _iAusentismoTiemposCoreBusiness.Dispose();
                    _iAusentismoTiemposCoreBusiness = null;
                }

                if (_iAreasCoreBusiness != null)
                {
                    _iAreasCoreBusiness.Dispose();
                    _iAreasCoreBusiness = null;
                }

                if (_iProcesosCoreBusiness != null)
                {
                    _iProcesosCoreBusiness.Dispose();
                    _iProcesosCoreBusiness = null;
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
