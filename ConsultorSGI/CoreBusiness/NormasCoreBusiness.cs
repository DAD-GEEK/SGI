using Across;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class NormasCoreBusiness : CRUDGenerico<Normas>, INormasCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ITerceros_NormasCoreBusiness _iTerceros_NormasCoreBusiness;
        private IAuditorias_NormasCoreBusiness _iAuditorias_NormasCoreBusiness;
        private INumeralesCoreBusiness _iNumeralesCoreBusiness;
        private ICacheStorage _iCacheStorage;
        
        public NormasCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iTerceros_NormasCoreBusiness = new Terceros_NormasCoreBusiness();
            this._iAuditorias_NormasCoreBusiness = new Auditorias_NormasCoreBusiness();
            this._iCacheStorage = new CacheStorage();
        }

        #region CRUD Generico
        public async Task<List<Normas>> ObtenerNormasAsync()
        {
            try
            {
                NormasDataAccess NormasDataAccess = new NormasDataAccess();
                List<Normas> normas;
                string normasJson = _iCacheStorage.Get<string>(CacheNames.ObtenerTodasLasNormas);

                if (normasJson is null)
                {
                    normas = await this.GetAllAsync();
                    normas = this.ConvertirNormasEnNormasAsync(normas);

                    normasJson = JsonConvert.SerializeObject(normas);

                    _iCacheStorage.Insert(CacheNames.ObtenerTodasLasNormas, normasJson, new TimeSpan(12, 0, 0));
                }
                else
                    normas = JsonConvert.DeserializeObject<List<Normas>>(normasJson);

                return normas.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Normas> ConvertirNormasEnNormasAsync(List<Normas> listaNormas)
        {
            try
            {
                List<Normas> nuevaListaNormas = new List<Normas>();

                foreach (var item in listaNormas)
                {
                    Normas normas = new Normas();
                    normas.IntNormaID = item.IntNormaID;
                    normas.StrCodigo = item.StrCodigo;
                    normas.StrDescripcion = item.StrDescripcion;
                    normas.IntTipoCriterio = item.IntTipoCriterio;
                    normas.BitActivo = item.BitActivo;

                    nuevaListaNormas.Add(normas);
                }

                return nuevaListaNormas;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NormasDTO>> ObtenerNormasDTOAsync(List<Numerales> numeralesSeleccionados = null)
        {
            try
            {
                _iNumeralesCoreBusiness = new NumeralesCoreBusiness();

                var listaNormas = await this.ObtenerNormasAsync();
                listaNormas = listaNormas.Where(x => x.BitActivo == true).ToList();

                var listaNumerales = await _iNumeralesCoreBusiness.FindWhereAsync(x => x.BitActivo == true);

                if (numeralesSeleccionados != null)
                    listaNumerales = listaNumerales.Where(x => numeralesSeleccionados.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();

                return listaNormas.Select(no => new NormasDTO()
                {
                    IntNormaID = no.IntNormaID,
                    StrCodigo = no.StrCodigo,
                    StrDescripcion = no.StrDescripcion,
                    IntTipoCriterio = no.IntTipoCriterio,
                    BitActivo = no.BitActivo,
                    NumeralesDTO = listaNumerales.Where(x => x.IntNormaID == no.IntNormaID).Select(nu => new NumeralesDTO() {
                        IntCodigoPrincipal = Convert.ToInt16(nu.StrCodigo[0]),
                        IntNumeralID = nu.IntNumeralID,
                        StrCodigo = nu.StrCodigo,
                        IntCodigoNumerico = Convert.ToInt32((nu.StrCodigo.Split('.'))[0]),
                        StrDescripcion = nu.StrDescripcion, 
                        StrInterpretacion = nu.StrInterpretacion,
                        IntNivel = nu.StrCodigo.Split('.').Count(),
                        BitActivo = nu.BitActivo,

                    }).ToList()

                }).ToList();


            }
            catch (Exception)
            {

                throw;
            }
        }
        public new async Task<string> SaveEntityAsync(Normas entity)
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

        public new async Task<string> CreateAsync(Normas entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower());
                if (listaRegistros.Count() != 0)
                    return String.Format(RecursoCommon.msnRegistroYaExiste, entity.StrCodigo);

                entity.StrCodigo = entity.StrCodigo;
                entity.StrDescripcion = entity.StrDescripcion;
                entity.IntTipoCriterio = entity.IntTipoCriterio;
                entity.BitActivo = entity.BitActivo;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Normas entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Normas> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Normas, bool>> match)
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

        public new async Task<Normas> FindAsync(Expression<Func<Normas, bool>> match)
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

        public new async Task<List<Normas>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Normas entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.StrCodigo.Trim().ToLower() == entity.StrCodigo.Trim().ToLower() && x.IntNormaID != entity.IntNormaID);

                if (listaRegistros.Count() != 0)
                    return RecursoCommon.msnRegistroYaExiste;

                var Normas = await this.FindAsync(x => x.IntNormaID == entity.IntNormaID);

                Normas.StrCodigo = entity.StrCodigo;
                Normas.StrDescripcion = entity.StrDescripcion;
                Normas.IntTipoCriterio = entity.IntTipoCriterio;
                Normas.BitActivo = entity.BitActivo;

                await base.UpdateAsync(Normas);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Normas model)
        {
            try
            {
                _iCacheStorage.ClearAllByCacheKey(CacheNames.ObtenerTodasLasNormas);

                if (model.IntNormaID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Normas>> ObtenerNormasPorTerceroAsync(int terceroID)
        {
            try
            {
                var listaNormas = await this.ObtenerNormasAsync();
                var listaNormasPorTercero = await _iTerceros_NormasCoreBusiness.FindWhereAsync(x => x.IntTerceroID == terceroID);

                return listaNormas.Where(x => listaNormasPorTercero.Any(y => y.IntNormaID == x.IntNormaID)).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Normas>> ObtenerNormasPorAuditoriaAsync(int auditoriaID)
        {
            try
            {
                var listaNormas = await this.ObtenerNormasAsync();
                var listaNormasPorAuditoria = await _iAuditorias_NormasCoreBusiness.FindWhereAsync(x => x.IntAuditoriaID == auditoriaID);

                return listaNormas.Where(x => listaNormasPorAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Normas>> ObtenerNormasPorAuditoriaYNumeralesAsync(int auditoriaID, List<Numerales> listaNumerales)
        {
            try
            {
                var listaNormasAuditoria = await this.ObtenerNormasPorAuditoriaAsync(auditoriaID);              
                return listaNormasAuditoria.Where(x => listaNumerales.Any(y => y.IntNormaID == x.IntNormaID)).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NormasDTO>> ContadorDeNormasAsync(List<NormasDTO> listaNormasDTO)
        {
            try
            {
                var listaNormasEnBD = await this.GetAllAsync();

                List<NormasDTO> listaContadorDeNormasDTO = new List<NormasDTO>();

                foreach (var item in listaNormasEnBD)
                {
                    NormasDTO normasDTO = new NormasDTO();
                    normasDTO.IntNormaID = item.IntNormaID;
                    normasDTO.StrCodigo = item.StrCodigo;
                    normasDTO.StrDescripcion = item.StrDescripcion;

                    var cantidadPorNorma = listaNormasDTO.Where(y => y.IntNormaID == item.IntNormaID).Count();

                    if (cantidadPorNorma != 0)
                    {
                        normasDTO.CantidadNoConformidades = cantidadPorNorma;
                        listaContadorDeNormasDTO.Add(normasDTO);
                    }
                }

                return listaContadorDeNormasDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Select List
        public async Task<SelectList> SelectListAsync(string valueSelected = null)
        {
            try
            {
                var listaNormas = await ObtenerNormasAsync();
                listaNormas = listaNormas.Where(x => x.BitActivo == true).ToList();

                var entidad = listaNormas.Select(x => new SelectListItem()
                {
                    Value = x.IntNormaID.ToString(),
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

        public SelectList SelectListCriteriosAsync(string valueSelected = null)
        {
            try
            {
                var listaCriterios = Common.GetEnumToSelectList<enumTipoDeCriterio>();

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(listaCriterios, "Value", "Text");

                return new SelectList(listaCriterios, "Value", "Text", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> MultiSelectListTodasLasNormasAsync(List<Normas> valueSelected)
        {
            try
            {
                var listaNormas = await ObtenerNormasAsync();
                listaNormas = listaNormas.Where(x => x.BitActivo == true).ToList();

                var entidad = listaNormas.Select(x => new SelectListItem()
                {
                    Value = x.IntNormaID.ToString(),
                    Text = $"{x.StrCodigo} - {x.StrDescripcion}"
                }).ToList();

                var listaNormasText = new List<string>();

                foreach (var item in valueSelected.ToList())
                    listaNormasText.Add(item.IntNormaID.ToString());

                return new MultiSelectList(entidad, "Value", "Text", listaNormasText);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> MultiSelectListAsync(Auditorias valueSelected)
        {
            try
            {
                var listaNormas = await ObtenerNormasAsync();
                listaNormas = listaNormas.Where(x => x.BitActivo == true).ToList();

                var entidad = listaNormas.Select(x => new SelectListItem()
                {
                    Value = x.IntNormaID.ToString(),
                    Text = $"{x.StrCodigo} - {x.StrDescripcion}"
                }).ToList();

                var listaNormasText = new List<string>();

                foreach (var item in valueSelected.Auditorias_Normas.ToList())
                    listaNormasText.Add(item.IntNormaID.ToString());

                return new MultiSelectList(entidad, "Value", "Text", listaNormasText);
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

        ~NormasCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iTerceros_NormasCoreBusiness != null)
                {
                    _iTerceros_NormasCoreBusiness.Dispose();
                    _iTerceros_NormasCoreBusiness = null;
                }

                if (_iAuditorias_NormasCoreBusiness != null)
                {
                    _iAuditorias_NormasCoreBusiness.Dispose();
                    _iAuditorias_NormasCoreBusiness = null;
                }

                if (_iNumeralesCoreBusiness != null)
                {
                    _iNumeralesCoreBusiness.Dispose();
                    _iNumeralesCoreBusiness = null;
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
