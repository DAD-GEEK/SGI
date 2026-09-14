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
    public class SistemasDeGestionCoreBusiness : CRUDGenerico<SistemasDeGestion>, ISistemasDeGestionCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        private ITerceros_SistemasDeGestionCoreBusiness _iTerceros_SistemasDeGestionCoreBusiness;
        private ICacheStorage _iCacheStorage;

        public SistemasDeGestionCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iCacheStorage = new CacheStorage();
        }

        #region CRUD Genérico

        public async Task<List<SistemasDeGestionDTO>> ObtenerSistemasDeGestionDTOAsync()
        {
            try
            {
                List<SistemasDeGestionDTO> sistemasDeGestionDTO;
                string sistemasDeGestionJson = _iCacheStorage.Get<string>(CacheNames.ObtenerSistemasDeGestion);

                if (sistemasDeGestionJson is null)
                {
                    var sistemasDeGestion = await this.GetAllAsync();
                    sistemasDeGestionDTO = this.ConvertirSistemasDeGestionEnSistemasDeGestionDTOAsync(sistemasDeGestion);

                    sistemasDeGestionJson = JsonConvert.SerializeObject(sistemasDeGestionDTO);

                    _iCacheStorage.Insert(CacheNames.ObtenerSistemasDeGestion, sistemasDeGestionJson, new TimeSpan(12, 0, 0));
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

        public async Task<List<SistemasDeGestionDTO>> ObtenerSistemasDeGestionPorTerceroEnSesion()
        {
            try
            {
                this._iTerceros_SistemasDeGestionCoreBusiness = new Terceros_SistemasDeGestionCoreBusiness();

                var usuarioEnSesion = Servicios.ServicioUsuarioCoreBusiness.ObtenerDatosDeUsuarioEnSesion;
                var listaSistemasDeGestionPorTerceroDTO = await _iTerceros_SistemasDeGestionCoreBusiness.ObtenerSistemasDeGestionPorTerceroAsync(usuarioEnSesion.TerceroID);

                return listaSistemasDeGestionPorTerceroDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private List<SistemasDeGestionDTO> ConvertirSistemasDeGestionEnSistemasDeGestionDTOAsync(List<SistemasDeGestion> listaSistemasDeGestion)
        {
            try
            {
                List<SistemasDeGestionDTO> listaSistemasDeGestionDTO = new List<SistemasDeGestionDTO>();

                foreach (var item in listaSistemasDeGestion)
                {
                    SistemasDeGestionDTO sistemasDeGestionDTO = new SistemasDeGestionDTO();
                    sistemasDeGestionDTO.StrCodigoID = item.StrCodigoID;
                    sistemasDeGestionDTO.StrDescripcion = item.StrDescripcion;

                    listaSistemasDeGestionDTO.Add(sistemasDeGestionDTO);
                }

                return listaSistemasDeGestionDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public new async Task<string> SaveEntityAsync(SistemasDeGestion entity)
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

        public new async Task<string> CreateAsync(SistemasDeGestion entity)
        {
            try
            {
                entity.StrCodigoID = entity.StrCodigoID;
                entity.StrDescripcion = entity.StrDescripcion;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(SistemasDeGestion entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<SistemasDeGestion> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<SistemasDeGestion, bool>> match)
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

        public new async Task<SistemasDeGestion> FindAsync(Expression<Func<SistemasDeGestion, bool>> match)
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

        public new List<SistemasDeGestion> GetAll()
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

        public new async Task<List<SistemasDeGestion>> GetAllAsync()
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
        public new async Task<string> UpdateAsync(SistemasDeGestion entity)
        {
            try
            {
                var SistemasDeGestion = await this.FindAsync(x => x.StrCodigoID == entity.StrCodigoID);

                SistemasDeGestion.StrDescripcion = entity.StrDescripcion;
                SistemasDeGestion.StrIcono = entity.StrIcono;

                await base.UpdateAsync(SistemasDeGestion);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarAsync(SistemasDeGestion entity)
        {
            try
            {
                await this.DeleteAsync(entity);
                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarRegistroAsync(SistemasDeGestion model, string tipoDeAccion)
        {
            try
            {
                if (tipoDeAccion == enumTipoDeAccion.Editar.ToString())
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
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
                var entidad = (from c in await this.ObtenerSistemasDeGestionDTOAsync()
                               select new { CodigoID = c.StrCodigoID, Descripcion = $"{c.StrCodigoID} - {c.StrDescripcion}" });

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "CodigoID", "Descripcion");

                return new SelectList(entidad, "CodigoID", "Descripcion", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Documento diagnóstico
        public async Task<List<DocumentosDiagnosticoPasosDTO>> ObtenerDocumentoDiagnosticoPasosPorSistemaDeGestion(string sistemaDeGestionID)
        {
            try
            {
                return new List<DocumentosDiagnosticoPasosDTO>();
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

        ~SistemasDeGestionCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iCacheStorage != null)
                {
                    _iCacheStorage = null;
                }

                if (_iTerceros_SistemasDeGestionCoreBusiness != null)
                {
                    _iTerceros_SistemasDeGestionCoreBusiness.Dispose();
                    _iTerceros_SistemasDeGestionCoreBusiness = null;
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
