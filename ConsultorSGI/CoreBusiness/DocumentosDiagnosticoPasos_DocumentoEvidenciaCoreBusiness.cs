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

namespace CoreBusiness
{
    public class DocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness : CRUDGenerico<DocumentosDiagnosticoPasos_DocumentoEvidencia>, IDocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness;
        private IDocumentosDiagnosticoPasos_CriteriosCoreBusiness _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness;

        public DocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iCommonCoreBusiness = new CommonCoreBusiness();
            this._iDocumentosDiagnosticoPasos_CriteriosCoreBusiness = new DocumentosDiagnosticoPasos_CriteriosCoreBusiness();
        }

        #region CRUD Generico
        public async Task<List<DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO>> GetAllEvidenciasPorCriterioAsync(string pasoID, string criterioID)
        {
            try
            {
                var listaEvidenciaPorPaso = await this.FindWhereAsync(x => x.StrPasoID == pasoID && x.BitActivo == true);

                var criterioModelo = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.FindAsync(x => x.StrCriterioID == criterioID);

                var listaCriteriosConEvidenciaDTO = listaEvidenciaPorPaso.Select(x => new DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO()
                {
                    StrEvidenciaID = x.StrEvidenciaID,
                    StrDescripcion = x.StrDescripcion,
                    BitSeleccionado = criterioModelo != null ? (criterioModelo.StrEvidenciaID == x.StrEvidenciaID ? true : false) : false,
                }).ToList();

                return listaCriteriosConEvidenciaDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO>> GetAllEvidenciasConCriteriosPasoAsync(string pasoID)
        {
            try
            {
                var listaDeEvidenciasPorPaso = await this.FindWhereAsync(x => x.StrPasoID == pasoID && x.BitActivo == true);
                var listaCriteriosPorPaso = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.FindWhereAsync(x => x.StrPasoID == pasoID && x.BitActivo == true);

                var listaDeEvidenciasConCriterioDTO = listaDeEvidenciasPorPaso.Select(x => new DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO()
                {
                    StrEvidenciaID = x.StrEvidenciaID,
                    StrPasoID = x.StrPasoID,
                    StrDescripcion = x.StrDescripcion,
                    DocumentosDiagnosticoPasos_CriteriosDTO = x.DocumentosDiagnosticoPasos_Criterios.ToList().Select(criterio => new DocumentosDiagnosticoPasos_CriteriosDTO()
                    {
                        StrCriterioID = criterio.StrCriterioID,
                        StrDescripcion = criterio.StrDescripcion,
                        IntOrden = criterio.IntOrden

                    }).ToList(),

                    IntOrden = x.DocumentosDiagnosticoPasos_Criterios.Count() != 0 ? (byte)x.DocumentosDiagnosticoPasos_Criterios.Min(c => c.IntOrden) : (byte)0,

                }).ToList();

                var listaCriteriosDTOSinAsociar = listaCriteriosPorPaso.Where(x => string.IsNullOrEmpty(x.StrEvidenciaID)).Select(x => new DocumentosDiagnosticoPasos_CriteriosDTO()
                {
                    StrCriterioID = x.StrCriterioID,
                    StrDescripcion = x.StrDescripcion,
                    StrEvidenciaID = "sinEvidencia",
                    IntOrden = x.IntOrden,

                }).ToList();

                if (listaCriteriosPorPaso.Where(x => string.IsNullOrEmpty(x.StrEvidenciaID)).Count() != 0)
                {
                    var evidenciaDTO = new DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO();
                    evidenciaDTO.StrEvidenciaID = "sinEvidencia";
                    evidenciaDTO.DocumentosDiagnosticoPasos_CriteriosDTO = listaCriteriosPorPaso.Where(x => string.IsNullOrEmpty(x.StrEvidenciaID)).Select(x => new DocumentosDiagnosticoPasos_CriteriosDTO()
                    {
                        StrCriterioID = x.StrCriterioID,
                        StrDescripcion = x.StrDescripcion,
                        StrEvidenciaID = "sinEvidencia",
                        IntOrden = x.IntOrden,

                    }).ToList();

                    evidenciaDTO.IntOrden = evidenciaDTO.DocumentosDiagnosticoPasos_CriteriosDTO.Count() != 0 ? (byte)evidenciaDTO.DocumentosDiagnosticoPasos_CriteriosDTO.Min(c => c.IntOrden) : (byte)0;


                    listaDeEvidenciasConCriterioDTO.Add(evidenciaDTO);
                }

                return listaDeEvidenciasConCriterioDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(DocumentosDiagnosticoPasos_DocumentoEvidencia entity)
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

        private new async Task<string> CreateAsync(DocumentosDiagnosticoPasos_DocumentoEvidencia entity)
        {
            try
            {
                var listaCriteriosPorEvidencia = entity.DocumentosDiagnosticoPasos_Criterios.ToList();

                List<string> listaCriteriosIDs = new List<string>();
                listaCriteriosPorEvidencia.ForEach(item => listaCriteriosIDs.Add(item.StrCriterioID));

                entity.DocumentosDiagnosticoPasos_Criterios.Clear();

                entity.StrEvidenciaID = Guid.NewGuid().ToString();
                entity.StrPasoID = entity.StrPasoID;
                entity.StrDescripcion = entity.StrDescripcion.Trim();
                entity.BitActivo = true;

                await base.CreateAsync(entity);

                if (listaCriteriosIDs.Count() != 0)
                    await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.AsociarListaDeCriteriosConEvidenciaAsync(listaCriteriosIDs, entity.StrEvidenciaID, entity.StrPasoID);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(DocumentosDiagnosticoPasos_DocumentoEvidencia entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<DocumentosDiagnosticoPasos_DocumentoEvidencia> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<DocumentosDiagnosticoPasos_DocumentoEvidencia, bool>> match)
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

        public new async Task<DocumentosDiagnosticoPasos_DocumentoEvidencia> FindAsync(Expression<Func<DocumentosDiagnosticoPasos_DocumentoEvidencia, bool>> match)
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

        public new List<DocumentosDiagnosticoPasos_DocumentoEvidencia> GetAll()
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

        public new async Task<List<DocumentosDiagnosticoPasos_DocumentoEvidencia>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(DocumentosDiagnosticoPasos_DocumentoEvidencia entity)
        {
            try
            {
                var listaCriteriosPorEvidencia = entity.DocumentosDiagnosticoPasos_Criterios.ToList();

                List<string> listaCriteriosIDs = new List<string>();
                listaCriteriosPorEvidencia.ForEach(item => listaCriteriosIDs.Add(item.StrCriterioID));

                entity.DocumentosDiagnosticoPasos_Criterios.Clear();

                var DocumentosDiagnosticoPasos_DocumentoEvidencia = await this.FindAsync(x => x.StrEvidenciaID == entity.StrEvidenciaID);

                DocumentosDiagnosticoPasos_DocumentoEvidencia.StrEvidenciaID = entity.StrEvidenciaID;
                DocumentosDiagnosticoPasos_DocumentoEvidencia.StrPasoID = entity.StrPasoID;
                DocumentosDiagnosticoPasos_DocumentoEvidencia.StrDescripcion = entity.StrDescripcion;

                await base.UpdateAsync(DocumentosDiagnosticoPasos_DocumentoEvidencia);

                if (listaCriteriosIDs.Count() != 0)
                    await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.AsociarListaDeCriteriosConEvidenciaAsync(listaCriteriosIDs, entity.StrEvidenciaID, entity.StrPasoID);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarAsync(DocumentosDiagnosticoPasos_DocumentoEvidencia model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.StrEvidenciaID))
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarEvidenciaAsync(string evidenciaID)
        {
            try
            {
                var listaCriteriosPorEvidencia = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.FindWhereAsync(x => x.StrEvidenciaID == evidenciaID);

                foreach (var item in listaCriteriosPorEvidencia)
                {
                    item.StrEvidenciaID = null;
                    await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.GuardarAsync(item);
                }

                var evidenciaModelo = await this.FindAsync(x => x.StrEvidenciaID == evidenciaID);
                evidenciaModelo.BitActivo = false;

                await base.UpdateAsync(evidenciaModelo);

                return string.Empty;
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

        ~DocumentosDiagnosticoPasos_DocumentoEvidenciaCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iCommonCoreBusiness != null)
                {
                    _iCommonCoreBusiness.Dispose();
                    _iCommonCoreBusiness = null;
                }

                if (_iDocumentosDiagnosticoPasos_CriteriosCoreBusiness != null)
                {
                    _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.Dispose();
                    _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness = null;
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
