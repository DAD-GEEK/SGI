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
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class DocumentosDiagnosticoPasos_CriteriosCoreBusiness : CRUDGenerico<DocumentosDiagnosticoPasos_Criterios>, IDocumentosDiagnosticoPasos_CriteriosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness;

        public DocumentosDiagnosticoPasos_CriteriosCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iCommonCoreBusiness = new CommonCoreBusiness();
        }

        #region CRUD Generico
        public async Task<List<DocumentosDiagnosticoPasos_CriteriosDTO>> ObtenerListaDeCriteriosPorDocumentoEvidenciaPorPasoAsync(string pasoID, string evidenciaID)
        {
            try
            {
                var listaCriteriosConEvidencia = await this.FindWhereAsync(x => x.StrPasoID == pasoID && x.BitActivo == true);

                var listaCriteriosConEvidenciaDTO = listaCriteriosConEvidencia.Select(x => new DocumentosDiagnosticoPasos_CriteriosDTO()
                {
                    StrCriterioID = x.StrCriterioID,
                    StrDescripcion = x.StrDescripcion,
                    StrEvidenciaID = x.StrEvidenciaID,
                    BitSeleccionado = !string.IsNullOrEmpty(evidenciaID) ? (x.StrEvidenciaID == evidenciaID ? true : false) : false,
                    IntOrden = x.IntOrden

                }).ToList();

                return listaCriteriosConEvidenciaDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DocumentosDiagnosticoPasos_CriteriosDTO>> ObteneCriteriosGenericosPorSistemaDeGestionDTOAsync(string sistemaDeGestion)
        {
            try
            {
                var listaCriterios = await this.FindWhereAsync(x => x.DocumentosDiagnosticoPasos.StrSistemaDeGestionID == sistemaDeGestion);

                var listaCriteriosDTO = listaCriterios.Select(x => new DocumentosDiagnosticoPasos_CriteriosDTO()
                {
                    StrCriterioID = x.StrCriterioID,
                    StrDescripcion = x.StrDescripcion,
                    StrEvidenciaID = x.StrEvidenciaID,
                    StrDocumentoSoporte = x.StrEvidenciaID != null ? x.DocumentosDiagnosticoPasos_DocumentoEvidencia.StrDescripcion : string.Empty,
                }).ToList();

                return listaCriteriosDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(DocumentosDiagnosticoPasos_Criterios entity)
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

        private new async Task<string> CreateAsync(DocumentosDiagnosticoPasos_Criterios entity)
        {
            try
            {
                byte consecutivo = 1;

                var listaCriterios = await this.FindWhereAsync(x => x.StrPasoID == entity.StrPasoID);

                if (listaCriterios.Count() != 0)
                    consecutivo = Convert.ToByte(listaCriterios.Count() + 1);

                entity.StrCriterioID = Guid.NewGuid().ToString();
                entity.StrDescripcion = entity.StrDescripcion.Trim();
                entity.IntOrden = consecutivo;
                entity.BitActivo = true;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(DocumentosDiagnosticoPasos_Criterios entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<DocumentosDiagnosticoPasos_Criterios> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<DocumentosDiagnosticoPasos_Criterios, bool>> match)
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

        public new async Task<DocumentosDiagnosticoPasos_Criterios> FindAsync(Expression<Func<DocumentosDiagnosticoPasos_Criterios, bool>> match)
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

        public new List<DocumentosDiagnosticoPasos_Criterios> GetAll()
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

        public new async Task<List<DocumentosDiagnosticoPasos_Criterios>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(DocumentosDiagnosticoPasos_Criterios entity)
        {
            try
            {
                var DocumentosDiagnosticoPasos_Criterios = await this.FindAsync(x => x.StrCriterioID == entity.StrCriterioID);

                DocumentosDiagnosticoPasos_Criterios.StrDescripcion = entity.StrDescripcion;
                DocumentosDiagnosticoPasos_Criterios.StrPasoID = entity.StrPasoID;
                DocumentosDiagnosticoPasos_Criterios.StrEvidenciaID = entity.StrEvidenciaID;

                await base.UpdateAsync(DocumentosDiagnosticoPasos_Criterios);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarAsync(DocumentosDiagnosticoPasos_Criterios model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.StrCriterioID))
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> CambiarOrdenamientoSortableAsync(List<DocumentosDiagnosticoPasos_Criterios> listaCriterios)
        {
            try
            {
                List<string> listaCriteriosString = new List<string>();

                listaCriterios.ForEach(criterio => listaCriteriosString.Add(criterio.StrCriterioID));

                var listaCriteriosParaOrdenar = await this.FindWhereAsync(x => listaCriteriosString.Any(y => y == x.StrCriterioID));

                foreach (var criterio in listaCriteriosParaOrdenar)
                {
                    var criterioOrdenado = listaCriterios.FirstOrDefault(x => x.StrCriterioID == criterio.StrCriterioID);

                    if (criterio.IntOrden != criterioOrdenado.IntOrden)
                    {
                        criterio.IntOrden = criterioOrdenado.IntOrden;
                        await base.UpdateAsync(criterio);
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarCriterioAsync(string criterioID)
        {
            try
            {
                var criterio = await this.FindAsync(x => x.StrCriterioID == criterioID);

                if (criterio != null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                criterio.BitActivo = false;
                await base.UpdateAsync(criterio);

                return string.Empty;    
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Asociar con evidencias
        public async Task<string> AsociarListaDeCriteriosConEvidenciaAsync(List<string> listaDeCriteriosIDs, string evidenciaID, string pasoID)
        {
            try
            {
                var listaCriteriosPorEvidencia = await this.FindWhereAsync(x => x.StrPasoID == pasoID);

                var listaCriteriosParaAsociar = listaCriteriosPorEvidencia.Where(x => listaDeCriteriosIDs.Any(y => y == x.StrCriterioID)).ToList();
                var listaCriteriosParaDesAsociar = listaCriteriosPorEvidencia.Where(x => !listaDeCriteriosIDs.Any(y => y == x.StrCriterioID) && x.StrEvidenciaID == evidenciaID).ToList();

                foreach (var item in listaCriteriosParaAsociar)
                {
                    item.StrEvidenciaID = evidenciaID;
                    await base.UpdateAsync(item);
                }

                foreach (var item in listaCriteriosParaDesAsociar)
                {
                    item.StrEvidenciaID = null;
                    await base.UpdateAsync(item);
                }

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

        ~DocumentosDiagnosticoPasos_CriteriosCoreBusiness()
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
