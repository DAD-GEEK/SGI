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
    public class Terceros_SistemasDeGestionCoreBusiness : CRUDGenerico<Terceros_SistemasDeGestion>, ITerceros_SistemasDeGestionCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private readonly ICacheStorage _iCacheStorage;
        private readonly INormasCoreBusiness _iNormasCoreBusiness;
        private readonly ISistemasDeGestionCoreBusiness _iSistemasDeGestionCoreBusiness;
        private readonly ITerceros_SistemasDeGestion_NormasCoreBusiness _iTerceros_SistemasDeGestion_NormasCoreBusiness;
        private readonly INivelesCoreBusiness _iNivelesCoreBusiness;
        private ITercerosCoreBusiness _iTercerosCoreBusiness;

        public Terceros_SistemasDeGestionCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iCacheStorage = new CacheStorage();
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iSistemasDeGestionCoreBusiness = new SistemasDeGestionCoreBusiness();
            this._iTerceros_SistemasDeGestion_NormasCoreBusiness = new Terceros_SistemasDeGestion_NormasCoreBusiness();
            this._iNivelesCoreBusiness = new NivelesCoreBusiness();
        }

        public async Task<List<SistemasDeGestionDTO>> ObtenerSistemasDeGestionPorTerceroAsync(int terceroID)
        {
            try
            {
                List<SistemasDeGestionDTO> sistemasDeGestionDTO;
                string sistemasDeGestionJson = _iCacheStorage.Get<string>($"{CacheNames.ObtenerSistemasDeGestionPorTercero}_{terceroID.ToString()}");

                if (sistemasDeGestionJson is null)
                {
                    var sistemasDeGestion = await this.FindWhereAsync(x => x.IntTerceroID == terceroID);
                    sistemasDeGestionDTO = await this.ConvertirTerceros_SistemasDeGestionEnSistemasDeGestionDTOAsync(sistemasDeGestion);

                    sistemasDeGestionJson = JsonConvert.SerializeObject(sistemasDeGestionDTO);

                    _iCacheStorage.Insert($"{CacheNames.ObtenerSistemasDeGestionPorTercero}_{terceroID.ToString()}", sistemasDeGestionJson, new TimeSpan(12, 0, 0));
                }
                else
                    sistemasDeGestionDTO = JsonConvert.DeserializeObject<List<SistemasDeGestionDTO>>(sistemasDeGestionJson);

                return sistemasDeGestionDTO.OrderBy(x => x.TIntOrden).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<SistemasDeGestionDTO>> ConvertirTerceros_SistemasDeGestionEnSistemasDeGestionDTOAsync(List<Terceros_SistemasDeGestion> listaSistemasDeGestionTercero)
        {
            try
            {
                List<SistemasDeGestionDTO> listaSistemasDeGestionDTO = new List<SistemasDeGestionDTO>();

                foreach (var item in listaSistemasDeGestionTercero)
                {
                    SistemasDeGestionDTO sistemasDeGestionDTO = await this.ObtenerTerceros_SistemasDeGestionPorIDAsync(item.IntRegistroID);
                    listaSistemasDeGestionDTO.Add(sistemasDeGestionDTO);
                }

                return listaSistemasDeGestionDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SistemasDeGestionDTO> ObtenerTerceros_SistemasDeGestionPorIDAsync(int registroID = 0)
        {
            try
            {
                SistemasDeGestionDTO sistemasDeGestionDTO = new SistemasDeGestionDTO();

                var listaNormas = await _iNormasCoreBusiness.ObtenerNormasDTOAsync();

                if (registroID != 0)
                {
                    var sistemaDeGestionEnTercero = await this.FindAsync(x => x.IntRegistroID == registroID);

                    sistemasDeGestionDTO.IntSistemaDeGestion_TerceroID = registroID;
                    sistemasDeGestionDTO.StrCodigoID = sistemaDeGestionEnTercero.SistemasDeGestion.StrCodigoID;
                    sistemasDeGestionDTO.StrDescripcion = sistemaDeGestionEnTercero.SistemasDeGestion.StrDescripcion;
                    sistemasDeGestionDTO.TIntOrden = sistemaDeGestionEnTercero.SistemasDeGestion.TIntOrden;
                    sistemasDeGestionDTO.StrNivelID = sistemaDeGestionEnTercero.Terceros.StrNivelID;
                    sistemasDeGestionDTO.StrIcono = sistemaDeGestionEnTercero.SistemasDeGestion.StrIcono;

                    listaNormas.ForEach(item =>
                    {
                        if (sistemaDeGestionEnTercero.Terceros_SistemasDeGestion_Normas.Any(x => x.IntNormaID == item.IntNormaID))
                        {
                            item.IntTerceros_SistemaDeGestionID = sistemaDeGestionEnTercero.Terceros_SistemasDeGestion_Normas.FirstOrDefault(x => x.IntNormaID == item.IntNormaID).IntRegistroID;
                            item.BitAplica = true;
                        }
                    });
                }

                sistemasDeGestionDTO.Normas = listaNormas;

                return sistemasDeGestionDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(Terceros_SistemasDeGestion entity)
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

        public new async Task<string> CreateAsync(Terceros_SistemasDeGestion entity)
        {
            try
            {
                var sistemaDeGestion = await this.FindWhereAsync(x => x.IntTerceroID == entity.IntTerceroID && x.StrSistemaDeGestionID == entity.StrSistemaDeGestionID);

                if (sistemaDeGestion.Count() != 0)
                    return RecursoTerceros_SistemasDeGestion.msnRegistroDuplicado;

                entity.IntTerceroID = entity.IntTerceroID;
                entity.StrSistemaDeGestionID = entity.StrSistemaDeGestionID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Terceros_SistemasDeGestion entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Terceros_SistemasDeGestion> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Terceros_SistemasDeGestion, bool>> match)
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

        public new async Task<Terceros_SistemasDeGestion> FindAsync(Expression<Func<Terceros_SistemasDeGestion, bool>> match)
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

        public new List<Terceros_SistemasDeGestion> GetAll()
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

        public new async Task<List<Terceros_SistemasDeGestion>> GetAllAsync()
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
        public new async Task<string> UpdateAsync(Terceros_SistemasDeGestion entity)
        {
            try
            {
                var sistemaDeGestion = await this.FindWhereAsync(x => x.IntRegistroID != entity.IntRegistroID && x.IntTerceroID == entity.IntTerceroID && x.StrSistemaDeGestionID == entity.StrSistemaDeGestionID);

                if (sistemaDeGestion.Count() != 0)
                    return RecursoTerceros_SistemasDeGestion.msnRegistroDuplicado;

                var Terceros_SistemasDeGestion = await this.FindAsync(x => x.IntRegistroID == entity.IntRegistroID);

                Terceros_SistemasDeGestion.IntTerceroID = entity.IntTerceroID;
                Terceros_SistemasDeGestion.StrSistemaDeGestionID = entity.StrSistemaDeGestionID;

                await base.UpdateAsync(Terceros_SistemasDeGestion);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarRegistroAsync(Terceros_SistemasDeGestion model, string nivelID)
        {
            try
            {
                _iTercerosCoreBusiness = new TercerosCoreBusiness();
                _iCacheStorage.ClearAllByCacheKey($"{CacheNames.ObtenerSistemasDeGestionPorTercero}_{model.IntTerceroID.ToString()}");

                string respuesta = string.Empty;

                if (model.IntRegistroID != 0)
                {
                    respuesta = await this.UpdateAsync(model);

                    if (string.IsNullOrEmpty(respuesta))
                        await _iTercerosCoreBusiness.GuardarNivelDeTerceroAsync(model.IntTerceroID, nivelID);

                    return respuesta;
                }

                respuesta = await this.CreateAsync(model);

                if (string.IsNullOrEmpty(respuesta))
                    await _iTercerosCoreBusiness.GuardarNivelDeTerceroAsync(model.IntTerceroID, nivelID);

                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> AplicarNormaAsync(Terceros_SistemasDeGestion_Normas modelo)
        {
            try
            {
                _iCacheStorage.ClearAllByCacheKey($"{CacheNames.ObtenerSistemasDeGestionPorTercero}_{modelo.Terceros_SistemasDeGestion.IntTerceroID.ToString()}");

                if (modelo.IntRegistroID == 0)
                {
                    Terceros_SistemasDeGestion_Normas terceros_SistemasDeGestion_Normas = new Terceros_SistemasDeGestion_Normas();
                    terceros_SistemasDeGestion_Normas.IntTerceros_SistemasDeGestionID = modelo.IntTerceros_SistemasDeGestionID;
                    terceros_SistemasDeGestion_Normas.IntNormaID = modelo.IntNormaID;

                    return await _iTerceros_SistemasDeGestion_NormasCoreBusiness.GuardarRegistroAsync(terceros_SistemasDeGestion_Normas);
                }

                var modeloBD = await _iTerceros_SistemasDeGestion_NormasCoreBusiness.FindAsync(x => x.IntRegistroID == modelo.IntRegistroID);
                await _iTerceros_SistemasDeGestion_NormasCoreBusiness.DeleteAsync(modeloBD);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> DropDownListSistemasDeGestionAsync(string valueSelected = null)
        {
            try
            {
                return await _iSistemasDeGestionCoreBusiness.DropDownListSistemasDeGestionAsync(valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> DropDownListNivelesAsync(string valueSelected = null)
        {
            try
            {
                return await _iNivelesCoreBusiness.DropDownListNivelesAsync(valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarRegistroAsync(int registroID)
        {
            try
            {
                var modelo = await this.FindAsync(x => x.IntRegistroID == registroID);
                _iCacheStorage.ClearAllByCacheKey($"{CacheNames.ObtenerSistemasDeGestionPorTercero}_{modelo.IntTerceroID.ToString()}");

                await this.DeleteAsync(modelo);

                return string.Empty;

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

        ~Terceros_SistemasDeGestionCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._iNormasCoreBusiness != null)
                    this._iNormasCoreBusiness.Dispose();

                if (this._iSistemasDeGestionCoreBusiness != null)
                    this._iSistemasDeGestionCoreBusiness.Dispose();

                if (this._iNivelesCoreBusiness != null)
                    this._iNivelesCoreBusiness.Dispose();

                if (this._iTercerosCoreBusiness != null)
                {
                    this._iTercerosCoreBusiness.Dispose();
                    this._iTercerosCoreBusiness = null;
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
