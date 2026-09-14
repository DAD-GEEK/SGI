using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class ProcesosCoreBusiness : CRUDGenerico<Procesos>, IProcesosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness;
        private INumeralesCoreBusiness _iNumeralesCoreBusiness;
        private INormasCoreBusiness _iNormasCoreBusiness;
        private IPlantillasListasDeVerificacionDetalleCoreBusiness _iPlantillasListasDeVerificacionDetalleCoreBusiness;
        private IPlantillasListasDeVerificacion_NumeralesCoreBusiness _iPlantillasListasDeVerificacion_NumeralesCoreBusiness;

        public ProcesosCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iCommonCoreBusiness = new CommonCoreBusiness();
            this._iNumeralesCoreBusiness = new NumeralesCoreBusiness();
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iPlantillasListasDeVerificacionDetalleCoreBusiness = new PlantillasListasDeVerificacionDetalleCoreBusiness();
            this._iPlantillasListasDeVerificacion_NumeralesCoreBusiness = new PlantillasListasDeVerificacion_NumeralesCoreBusiness();
        }

        public async Task<List<Procesos>> ObtenerProcesosGlobalesAsync()
        {
            try
            {
                var terceroGenericoID = await _iCommonCoreBusiness.GenerarTerceroClienteGenericoAsync();

                var listaProcesos = await this.FindWhereAsync(x => x.IntTerceroClienteID == terceroGenericoID && x.BitActivo == true);

                return listaProcesos;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public new async Task<string> SaveEntityAsync(Procesos entity)
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

        public new async Task<string> CreateAsync(Procesos entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntTerceroClienteID == entity.IntTerceroClienteID && x.StrCodigo.ToLower() == entity.StrCodigo.ToLower());

                if (listaRegistros.Count() != 0)
                    return string.Format(RecursoProcesos.msnRegistroYaExiste, entity.StrCodigo);

                entity.StrCodigo = entity.StrCodigo;
                entity.StrDescripcion = entity.StrDescripcion;
                entity.IntTerceroClienteID = entity.IntTerceroClienteID;
                entity.IntMacroProceso = entity.IntMacroProceso;
                entity.BitActivo = entity.BitActivo;
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Procesos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Procesos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Procesos, bool>> match)
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

        public new async Task<Procesos> FindAsync(Expression<Func<Procesos, bool>> match)
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

        public new List<Procesos> GetAll()
        {
            try
            {
                return base.GetAll().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<Procesos>> GetAllAsync()
        {
            try
            {
                var listaEntidad = await base.GetAllAsync();
                return listaEntidad.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(Procesos entity)
        {
            try
            {
                var listaRegistros = await this.GetAllAsync();
                var existeRegistro = listaRegistros.Any(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntProcesoID != entity.IntProcesoID && x.IntTerceroClienteID == entity.IntTerceroClienteID);
                if (existeRegistro)
                    return string.Format(RecursoProcesos.msnRegistroYaExiste, entity.StrCodigo);

                var Procesos = await this.FindAsync(x => x.IntProcesoID == entity.IntProcesoID);

                Procesos.StrCodigo = entity.StrCodigo;
                Procesos.StrDescripcion = entity.StrDescripcion;
                Procesos.IntTerceroClienteID = entity.IntTerceroClienteID;
                Procesos.IntMacroProceso = entity.IntMacroProceso;
                Procesos.BitActivo = entity.BitActivo;
                await base.UpdateAsync(Procesos);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Procesos model)
        {
            try
            {
                if (model.IntTerceroClienteID == 0)
                    model.IntTerceroClienteID = await _iCommonCoreBusiness.GenerarTerceroClienteGenericoAsync();

                if (model.IntProcesoID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> AgregarProcesoGlobalAlTerceroAsync(Procesos modelo)
        {
            try
            {
                var procesoGlobal = await this.FindAsync(x => x.IntProcesoID == modelo.IntProcesoID);

                if (procesoGlobal is null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                Procesos nuevoProceso = new Procesos()
                {
                    StrCodigo = procesoGlobal.StrCodigo,
                    StrDescripcion = procesoGlobal.StrDescripcion,
                    IntTerceroClienteID = modelo.IntTerceroClienteID,
                    IntProcesoOrigen = modelo.IntProcesoID,
                    BitActivo = true,

                    PlantillasListasDeVerificacionDetalle = procesoGlobal.PlantillasListasDeVerificacionDetalle.Select(x => new PlantillasListasDeVerificacionDetalle()
                    {
                        IntProcesoID = x.IntProcesoID,
                        StrTitulo = x.StrTitulo,
                        IntOrden = x.IntOrden,
                        BitEstado = x.BitEstado,
                        PlantillasListasDeVerificacion_Numerales = x.PlantillasListasDeVerificacion_Numerales.Select(numeral => new PlantillasListasDeVerificacion_Numerales()
                        {
                            IntNumeralID = numeral.IntNumeralID,
                        }).ToList(),

                    }).ToList(),

                };

                return await this.CreateAsync(nuevoProceso);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Procesos_NumeralesDTO>> ObtenerNumeralesPorProcesoDTOAsync(int procesoID)
        {
            try
            {
                List<int> listaPlantillasIDs = new List<int>();
                var listaPlantillas = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindWhereAsync(x => x.IntProcesoID == procesoID);
                listaPlantillas.ForEach(registro => listaPlantillasIDs.Add(registro.IntPlantillaDetalleID));

                var listaNumeralesPorProceso = await _iPlantillasListasDeVerificacion_NumeralesCoreBusiness.FindWhereAsync(x => listaPlantillasIDs.Any(y => y == x.IntPlantillaDetalleID));

                var listaNumeralesDTO = listaNumeralesPorProceso.Select(x => new Procesos_NumeralesDTO()
                {
                    IntID = x.IntID,
                    IntNumeralID = x.IntNumeralID,
                    IntProcesoID = x.PlantillasListasDeVerificacionDetalle.Procesos.IntProcesoID,
                    CodigoNorma = x.Numerales.Normas.StrCodigo,
                    CodigoNumeral = x.Numerales.StrCodigo,
                    DescripcionNumeral = x.Numerales.StrDescripcion
                }).ToList();

                return listaNumeralesDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> SelectListAsync(int terceroClienteID, string valueSelected = null)
        {
            try
            {
                var entidad = (from c in await this.FindWhereAsync(x => x.IntTerceroClienteID == terceroClienteID)
                               where c.IntTerceroClienteID == terceroClienteID
                               orderby c.StrCodigo
                               select new { CodigoID = c.IntProcesoID, Descripcion = $"{c.StrCodigo} - {c.StrDescripcion}" });

                if (string.IsNullOrEmpty(valueSelected)) return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> MultiSelectListPorTerceroAsync( Terceros_Clientes terceroClientes, Auditorias auditoria = null)
        {
            try
            {
                var listaProcesos = await this.FindWhereAsync(x => x.IntTerceroClienteID == terceroClientes.IntTerceroClienteID);

                var entidad = listaProcesos.Select(x => new SelectListItem()
                {
                    Value = x.IntProcesoID.ToString(),
                    Text = $"{x.StrCodigo} - {x.StrDescripcion}"
                }).ToList();

                var listaProcesosText = new List<string>();

                if (auditoria != null)
                {
                    foreach (var item in auditoria.Auditorias_Procesos.ToList())
                        listaProcesosText.Add(item.IntProcesoID.ToString());
                }
                else
                {
                    foreach (var item in terceroClientes.Procesos.ToList())
                        listaProcesosText.Add(item.IntProcesoID.ToString());
                }

                return new MultiSelectList(entidad, "Value", "Text", listaProcesosText);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MultiSelectList> MultiSelectListNumeralesPorTerceroAsync(int terceroID, Procesos proceso = null)
        {
            try
            {
                return await _iNumeralesCoreBusiness.MultiListNumeralesPorTerceroAsync(terceroID, proceso);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SelectList SelectListMacroProcesosAsync(string valueSelected = null)
        {
            try
            {
                var listaMaroProcesos = Common.GetEnumToSelectList<enumMacroProcesos>();

                var entidad = (from c in listaMaroProcesos
                               orderby c.Value
                               select new { CodigoID = c.Value, Descripcion = $"{c.Text}" });

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> ObtenerNumeralesPorNormaAsync(int normaID)
        {
            try
            {
                return await _iNumeralesCoreBusiness.DropDownNumeralesPorNormaAsync(normaID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> ObtenerTodasLasNormasAsync(int normaID)
        {
            try
            {
                var norma = normaID.ToString();
                return await _iNormasCoreBusiness.SelectListAsync(norma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> ObtenerTerceroClienteGenericoIDAsync()
        {
            try
            {
                return await _iCommonCoreBusiness.GenerarTerceroClienteGenericoAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NormasDTO>> ObtenerNumeralesParaSeleccionarAsync(NumeralesGenericosDTO datosGenericos)
        {
            try
            {
                List<int> listaPlantillasIDs = new List<int>();
                var listaPlantillas = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindWhereAsync(x => x.IntProcesoID == datosGenericos.ProcesoID);
                listaPlantillas.ForEach(registro => listaPlantillasIDs.Add(registro.IntPlantillaDetalleID));

                var listaNumeralesPorProceso = await _iPlantillasListasDeVerificacion_NumeralesCoreBusiness.FindWhereAsync(x => listaPlantillasIDs.Any(y => y == x.IntPlantillaDetalleID));

                var listaNormasDTO = await _iNormasCoreBusiness.ObtenerNormasDTOAsync();

                listaNormasDTO.ForEach(normas =>
                {
                    normas.NumeralesDTO.ForEach(numeral =>
                    {

                        bool existeNumeral = listaNumeralesPorProceso.Exists(x => x.IntNumeralID == numeral.IntNumeralID);

                        if (existeNumeral)
                            numeral.BitSeleccionado = true;

                    });
                });

                return listaNormasDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> CargarDatosDeProcesoAProcesoAsync(int procesoGenericoID, int procesoActualID)
        {
            try
            {
                var procesoGenerico = await this.FindAsync(x => x.IntProcesoID == procesoGenericoID);
                var procesoActual = await this.FindAsync(x => x.IntProcesoID == procesoActualID);

                if (procesoGenerico is null || procesoActual is null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                var listaDeVerificacionProcesoGenerico = procesoGenerico.PlantillasListasDeVerificacionDetalle.ToList();
                var listaDeVerificacionProcesoActual = procesoActual.PlantillasListasDeVerificacionDetalle.ToList();

                if (listaDeVerificacionProcesoGenerico.Count() == 0)
                    return String.Format(RecursoProcesos.msnProcesoSinListaDeVerificacion, procesoGenerico.StrDescripcion);

                var listaDeVerificacionParaCrear = new List<PlantillasListasDeVerificacionDetalle>();

                if (listaDeVerificacionProcesoActual.Count() == 0)
                    listaDeVerificacionParaCrear = listaDeVerificacionProcesoGenerico.ToList();
                else
                    listaDeVerificacionParaCrear = listaDeVerificacionProcesoGenerico.Where(x => !listaDeVerificacionProcesoActual.Any(y => x.StrTitulo.ToLower() == y.StrTitulo.ToLower())).ToList();

                if (listaDeVerificacionParaCrear.Count() != 0)
                {
                    await _iPlantillasListasDeVerificacionDetalleCoreBusiness.CreateRangeAsync(listaDeVerificacionParaCrear.Select(x => new PlantillasListasDeVerificacionDetalle()
                    {
                        IntProcesoID = procesoActualID,
                        StrTitulo = x.StrTitulo,
                        IntOrden = x.IntOrden,
                        BitEstado = x.BitEstado,
                        PlantillasListasDeVerificacion_Numerales = x.PlantillasListasDeVerificacion_Numerales.Select(numeral => new PlantillasListasDeVerificacion_Numerales()
                        {
                            IntNumeralID = numeral.IntNumeralID
                        }).ToList(),
                    }).ToList());
                }

                if (listaDeVerificacionProcesoActual.Count() == 0)
                    return string.Empty;

                var listaDeVerificacionGenericosParaEditar = listaDeVerificacionProcesoGenerico.Where(x => !listaDeVerificacionParaCrear.Any(y => y.IntPlantillaDetalleID != x.IntPlantillaDetalleID)).ToList();

                var listaDeVerificacionParaEditar = listaDeVerificacionProcesoActual.Where(x => listaDeVerificacionGenericosParaEditar.Any(y => x.StrTitulo.ToLower() == y.StrTitulo.ToLower())).ToList();

                foreach (var item in listaDeVerificacionParaEditar)
                {
                    var numeralesProcesoActual = item.PlantillasListasDeVerificacion_Numerales.ToList();
                    var numeralesProcesoGenerico = procesoGenerico.PlantillasListasDeVerificacionDetalle.FirstOrDefault(x => x.StrTitulo == item.StrTitulo).PlantillasListasDeVerificacion_Numerales.ToList();

                    var numeralesParaCrear = numeralesProcesoGenerico.Where(x => !numeralesProcesoActual.Any(y => y.IntNumeralID != x.IntNumeralID)).ToList();

                    if (numeralesParaCrear.Count() != 0)
                    {
                        var numerales = numeralesParaCrear.Select(x => new PlantillasListasDeVerificacion_Numerales()
                        {
                            IntPlantillaDetalleID = item.IntPlantillaDetalleID,
                            IntNumeralID = x.IntNumeralID
                        }).ToList();

                        await _iPlantillasListasDeVerificacion_NumeralesCoreBusiness.CreateRangeAsync(numerales);
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarProcesoAsync(Procesos modelo)
        {
            try
            {

                if (modelo.Auditorias_Procesos.Count() != 0)
                    return string.Format(RecursoProcesos.msnRelacionConAuditoria, $"{modelo.StrCodigo} - {modelo.StrDescripcion}");

                if (modelo.AuditoriasDetalle.Count() != 0)
                    return string.Format(RecursoProcesos.msnRelacionConAuditoriaDetalle, $"{modelo.StrCodigo} - {modelo.StrDescripcion}");

                if (modelo.AuditoriasDetalle.Count() != 0)
                    return string.Format(RecursoProcesos.msnRelacionConAuditoriaDetalle, $"{modelo.StrCodigo} - {modelo.StrDescripcion}");

                await this.DeleteAsync(modelo);

                return String.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Procesos>> ObtenerProcesosPorTercerClienteIDAsync(string filtro, int terceroClienteID)
        {
            try
            {
                List<Procesos> listaProcesosPorCliente = null;

                if (string.IsNullOrEmpty(filtro))
                listaProcesosPorCliente = await this.FindWhereAsync(x => x.IntTerceroClienteID == terceroClienteID);
                else
                    listaProcesosPorCliente = await this.FindWhereAsync(x => x.IntTerceroClienteID == terceroClienteID && (x.StrCodigo.Contains(filtro) || x.StrDescripcion.Contains(filtro)));

                return listaProcesosPorCliente;
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

        ~ProcesosCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iNumeralesCoreBusiness != null)
                {
                    this._iNumeralesCoreBusiness.Dispose();
                    this._iNumeralesCoreBusiness = null;
                }

                if (_iNormasCoreBusiness != null)
                {
                    this._iNormasCoreBusiness.Dispose();
                    this._iNormasCoreBusiness = null;
                }

                if (_iPlantillasListasDeVerificacionDetalleCoreBusiness != null)
                {
                    this._iPlantillasListasDeVerificacionDetalleCoreBusiness.Dispose();
                    this._iPlantillasListasDeVerificacionDetalleCoreBusiness = null;
                }

                if (_iPlantillasListasDeVerificacion_NumeralesCoreBusiness != null)
                {
                    this._iPlantillasListasDeVerificacion_NumeralesCoreBusiness.Dispose();
                    this._iPlantillasListasDeVerificacion_NumeralesCoreBusiness = null;
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
