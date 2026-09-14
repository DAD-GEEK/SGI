using Across.ArchivosDeRecurso;
using Across.CacheStorage;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using Models.DTO;
using Newtonsoft.Json;
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
    public class NumeralesCoreBusiness : CRUDGenerico<Numerales>, INumeralesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private INormasCoreBusiness _iNormasCoreBusiness;
        private ITerceros_NormasCoreBusiness _iTerceros_NormasCoreBusiness;
        private IProcesosCoreBusiness _iProcesosCoreBusiness;
        private IListasDeVerificacionCoreBusiness _iListasDeVerificacionCoreBusiness;
        private ICacheStorage _iCacheStorage;

        public NumeralesCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iTerceros_NormasCoreBusiness = new Terceros_NormasCoreBusiness();
            this._iCacheStorage = new CacheStorage();
        }

        #region CRUD Generico
        public async Task<List<Numerales>> ObtenerTodosLosNumeralesAsync()
        {
            try
            {
                List<Numerales> entityList;
                string entityListJson = _iCacheStorage.Get<string>(CacheNames.ObtenerTodosLosNumerales);

                if (entityListJson is null)
                {
                    entityList = await this.GetAllAsync();

                    var nuevosNumerales = entityList.Select(x => new Numerales()
                    {
                        IntNumeralID = x.IntNumeralID,
                        StrDescripcion = x.StrDescripcion,
                        IntNormaID = x.IntNormaID,
                        BitActivo = x.BitActivo,
                        StrCodigo = x.StrCodigo,
                        StrInterpretacion = x.StrInterpretacion,
                        PlantillasListasDeVerificacion_Numerales = x.PlantillasListasDeVerificacion_Numerales.Select(p => new PlantillasListasDeVerificacion_Numerales()
                        {
                            IntID = p.IntID,
                            IntNumeralID = p.IntNumeralID,
                            IntPlantillaDetalleID = p.IntPlantillaDetalleID,
                            
                        }).ToList(),

                        Normas = new Normas()
                        {
                            IntNormaID = x.Normas.IntNormaID,
                            StrCodigo = x.Normas.StrCodigo,
                            StrDescripcion = x.Normas.StrDescripcion
                            
                        }
                    }).ToList();

                    entityListJson = JsonConvert.SerializeObject(nuevosNumerales);

                    _iCacheStorage.Insert(CacheNames.ObtenerTodosLosNumerales, entityListJson, new TimeSpan(12, 0, 0));
                }
                else
                    entityList = JsonConvert.DeserializeObject<List<Numerales>>(entityListJson);

                return entityList.ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NumeralesDTO>> ObtenerTodosLosNumeralesDTOAsync()
        {
            try
            {
                var listaNumerales = await this.ObtenerTodosLosNumeralesAsync();

                return listaNumerales.Select(numeral => new NumeralesDTO()
                {
                    IntNumeralID = numeral.IntNumeralID,
                    StrCodigo = numeral.StrCodigo,
                    StrDescripcion = numeral.StrDescripcion,
                    StrInterpretacion = numeral.StrInterpretacion,
                    IntNormaID = (int)numeral.IntNormaID,
                    BitActivo = numeral.BitActivo,

                }).OrderBy(numeral => numeral.StrCodigo).ToList().ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Numerales>> ObtenerNumeralesPorNormasAsync(List<Normas> listaNormas)
        {
            try
            {
                List<Numerales> listaNumerales = await this.ObtenerTodosLosNumeralesAsync();
                listaNumerales = listaNumerales.Where(x => x.BitActivo == true).ToList();

                return listaNumerales.Where(x => listaNormas.Any(y => y.IntNormaID == x.IntNormaID)).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Numerales>> ObtenerNumeralesPorNormasAsync(int normaID)
        {
            try
            {
                List<Numerales> listaNumerales = await this.ObtenerTodosLosNumeralesAsync();
                listaNumerales = listaNumerales.Where(x => x.BitActivo == true && x.IntNormaID == normaID).ToList();

                return listaNumerales;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(Numerales entity)
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

        public new async Task<string> CreateAsync(Numerales entity)
        {
            try
            {
                Numerales numeralModelo = await this.FindAsync(x => x.StrCodigo == entity.StrCodigo && x.IntNormaID == entity.IntNormaID);

                if (numeralModelo != null)
                {
                    var normaModelo = await _iNormasCoreBusiness.FindAsync(x => x.IntNormaID == entity.IntNormaID);
                    return String.Format(RecursoNumerales.msnNumeralDuplicado, entity.StrCodigo, $"{normaModelo.StrCodigo}");
                }

                entity.StrCodigo = entity.StrCodigo;
                entity.StrDescripcion = entity.StrDescripcion;
                entity.StrInterpretacion = entity.StrInterpretacion;
                entity.IntNormaID = entity.IntNormaID;
                entity.BitActivo = entity.BitActivo;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Numerales entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Numerales> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Numerales, bool>> match)
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

        public new async Task<Numerales> FindAsync(Expression<Func<Numerales, bool>> match)
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

        public new async Task<List<Numerales>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Numerales entity)
        {
            try
            {
                Numerales numeralModelo = await this.FindAsync(x => x.StrCodigo == entity.StrCodigo && x.IntNormaID == entity.IntNormaID && x.IntNumeralID != entity.IntNumeralID);

                if (numeralModelo != null)
                {
                    var normaModelo = await _iNormasCoreBusiness.FindAsync(x => x.IntNormaID == entity.IntNormaID);
                    return String.Format(RecursoNumerales.msnNumeralDuplicado, entity.StrCodigo, $"{normaModelo.StrCodigo}");
                }

                var Numerales = await this.FindAsync(x => x.IntNumeralID == entity.IntNumeralID);

                Numerales.StrCodigo = entity.StrCodigo;
                Numerales.StrDescripcion = entity.StrDescripcion;
                Numerales.StrInterpretacion = entity.StrInterpretacion;
                Numerales.IntNormaID = entity.IntNormaID;
                Numerales.BitActivo = entity.BitActivo;

                await base.UpdateAsync(Numerales);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Numerales model, int normaID)
        {
            try
            {
                _iCacheStorage.ClearAllByCacheKey(CacheNames.ObtenerTodosLosNumerales);

                if (model.IntNumeralID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Numerales>> ObtenerNumeralesPorItemDeListaDeVerificacionAsync(ListasDeVerificacion listasDeVerificacion)
        {
            try
            {
                this._iListasDeVerificacionCoreBusiness = new ListasDeVerificacionCoreBusiness();

                List<Numerales> listaNumerales = await this.ObtenerTodosLosNumeralesAsync();
                listaNumerales = listaNumerales.Where(x => x.BitActivo == true).ToList();
                
                var listaNumeralesPorListaDeVerificacion = await _iListasDeVerificacionCoreBusiness.ObtenerNumeralesPorListaDeVerificacionDTOAsync(listasDeVerificacion.IntListaVerificacionID);

                return listaNumerales.Where(x => listaNumeralesPorListaDeVerificacion.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Numerales>> ObtenerNumeralesPorProceso_NormasAsync(int auditoriaID, int procesoID)
        {
            try
            {
                this._iProcesosCoreBusiness = new ProcesosCoreBusiness();

                List<Numerales> listaNumerales = await this.ObtenerTodosLosNumeralesAsync();
                listaNumerales = listaNumerales.Where(x => x.BitActivo == true).ToList();
                
                var listaNormasPorAuditoria = await _iNormasCoreBusiness.ObtenerNormasPorAuditoriaAsync(auditoriaID);
                var listaNumeralesPorNorma = listaNumerales.Where(x => listaNormasPorAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                var listaNumeralesPorProceso = await _iProcesosCoreBusiness.ObtenerNumeralesPorProcesoDTOAsync(procesoID);

                return listaNumerales.Where(x => listaNumeralesPorProceso.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NormasDTO>> ObtenerNumeralesPorNormasDTOAsync(List<Numerales> listaNumerales)
        {
            try
            {
                var listaNormas = await _iNormasCoreBusiness.ObtenerNormasAsync();
                listaNormas = listaNormas.Where(x => x.BitActivo == true).ToList();

                List<Numerales> listaNumeralesEnBD = await this.ObtenerTodosLosNumeralesAsync();
                listaNumeralesEnBD = listaNumeralesEnBD.Where(x => x.BitActivo == true).ToList();

                List<NormasDTO> listaNormasDTO = new List<NormasDTO>();

                foreach (var item in listaNumerales.GroupBy(x => x.IntNormaID))
                {
                    NormasDTO normasDTO = new NormasDTO();

                    normasDTO.IntNormaID = (int)item.Key;
                    normasDTO.StrCodigo = listaNormas.Find(norma => norma.IntNormaID == item.Key).StrCodigo;
                    normasDTO.StrDescripcion = listaNormas.Find(norma => norma.IntNormaID == item.Key).StrDescripcion;
                    normasDTO.Numerales = listaNumerales.Where(x => x.IntNormaID == item.Key).ToList();
                    normasDTO.CantidadNumeralesPorNorma = listaNumeralesEnBD.Where(x => x.IntNormaID == item.Key).ToList().Count();

                    listaNormasDTO.Add(normasDTO);
                }

                return listaNormasDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Normas> ObtenerNormaPorIdAsync(int normaID)
        {
            try
            {
                return await _iNormasCoreBusiness.FindAsync(x => x.IntNormaID == normaID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Numerales>> ObtenerNumeralesPorNormasDeAuditoriaAsync(Auditorias auditoriaModelo)
        {
            try
            {
                var listaNormas = await _iNormasCoreBusiness.FindWhereAsync(x => x.BitActivo == true);
                var listaNormasPorAuditoria = listaNormas.Where(x => auditoriaModelo.Auditorias_Normas.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                return await this.ObtenerNumeralesPorNormasAsync(listaNormasPorAuditoria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Select List
        public async Task<SelectList> SelectListNormasAsync(string valorSeleccionado = null)
        {
            try
            {
                return await _iNormasCoreBusiness.SelectListAsync(valorSeleccionado);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<SelectList> SelectListAsync(string valueSelected = null)
        {
            try
            {
                var listaNumerales = await this.FindWhereAsync(x => x.BitActivo == true);

                var entidad = listaNumerales.Select(x => new SelectListItem()
                {
                    Value = x.IntNumeralID.ToString(),
                    Text = $"{x.StrCodigo} - {x.StrDescripcion}"
                }).ToList();

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "Value", "Text");

                return new SelectList(entidad, "Value", "Text", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> MultiListNumeralesPorTerceroAsync(int terceroID, Procesos procesos = null)
        {
            try
            {
                this._iProcesosCoreBusiness = new ProcesosCoreBusiness();

                var listaNumerales = await this.FindWhereAsync(x => x.BitActivo == true);
                var listaNormasPorTercero = await _iTerceros_NormasCoreBusiness.FindWhereAsync(x => x.IntTerceroID == terceroID);
                listaNumerales = listaNumerales.Where(x => listaNormasPorTercero.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                var entidad = listaNumerales.GroupBy(x => x.IntNumeralID).Select(x => new SelectListItem()
                {
                    Value = x.Key.ToString(),
                    Text = listaNumerales.Where(y => y.IntNumeralID == x.Key).FirstOrDefault().StrCodigo + " - " + listaNumerales.Where(y => y.IntNumeralID == x.Key).FirstOrDefault().StrDescripcion
                }).ToList();

                var listaNormasText = new List<string>();

                if (procesos != null)
                {
                    var listaNumeralesPorProceso = await _iProcesosCoreBusiness.ObtenerNumeralesPorProcesoDTOAsync(procesos.IntProcesoID);

                    foreach (var item in listaNumeralesPorProceso)
                        listaNormasText.Add(item.IntNumeralID.ToString());
                }

                return new MultiSelectList(entidad, "Value", "Text", listaNormasText);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> MultiListNumeralesPorTercero_ProcesoAsync(int terceroID, Procesos procesos = null)
        {
            try
            {
                this._iProcesosCoreBusiness = new ProcesosCoreBusiness();

                var listaNumeralesPorProceso = await _iProcesosCoreBusiness.ObtenerNumeralesPorProcesoDTOAsync(procesos.IntProcesoID);
                var listaNumerales = await this.GetAllAsync();

                var entidad = listaNumerales.GroupBy(x => x.IntNumeralID).Select(x => new SelectListItem()
                {
                    Value = x.Key.ToString(),
                    Text = listaNumerales.Where(y => y.IntNumeralID == x.Key).FirstOrDefault().StrCodigo + " - " + listaNumerales.Where(y => y.IntNumeralID == x.Key).FirstOrDefault().StrDescripcion
                }).ToList();

                var listaNormasText = new List<string>();

                if (procesos != null)
                {
                    foreach (var item in listaNumeralesPorProceso)
                        listaNormasText.Add(item.IntNumeralID.ToString());
                }

                return new MultiSelectList(entidad, "Value", "Text", listaNormasText);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> DropDownNumeralesPorNormaAsync(int normaID, string valorSeleccionado = null)
        {
            try
            {
                var listaNumerales = await this.FindWhereAsync(x => x.IntNormaID == normaID && x.BitActivo == true);

                var entidad = listaNumerales.Select(x => new SelectListItem()
                {
                    Value = x.IntNumeralID.ToString(),
                    Text = $"{x.StrCodigo} - {x.StrDescripcion}"
                }).ToList();

                if (string.IsNullOrEmpty(valorSeleccionado))
                    return new SelectList(entidad, "Value", "Text");

                return new SelectList(entidad, "Value", "Text", valorSeleccionado);
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

        ~NumeralesCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iNormasCoreBusiness != null)
                {
                    _iNormasCoreBusiness.Dispose();
                    _iNormasCoreBusiness = null;
                }

                if (_iTerceros_NormasCoreBusiness != null)
                {
                    _iTerceros_NormasCoreBusiness.Dispose();
                    _iTerceros_NormasCoreBusiness = null;
                }

                if (_iProcesosCoreBusiness != null)
                {
                    _iProcesosCoreBusiness.Dispose();
                    _iProcesosCoreBusiness = null;
                }

                if (_iListasDeVerificacionCoreBusiness != null)
                {
                    _iListasDeVerificacionCoreBusiness.Dispose();
                    _iListasDeVerificacionCoreBusiness = null;
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
