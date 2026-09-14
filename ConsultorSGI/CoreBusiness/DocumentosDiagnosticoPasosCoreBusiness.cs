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

namespace CoreBusiness
{
    public class DocumentosDiagnosticoPasosCoreBusiness : CRUDGenerico<DocumentosDiagnosticoPasos>, IDocumentosDiagnosticoPasosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private ICommonCoreBusiness _iCommonCoreBusiness;
        private INivelesCoreBusiness _iNivelesCoreBusiness;
        private IFasesCoreBusiness _iFasesCoreBusiness;
        private IDocumentosDiagnosticoPasos_CriteriosCoreBusiness _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness;
        private IDocumentosDiagnosticoPasos_NivelesCoreBusiness _iDocumentosDiagnosticoPasos_NivelesCoreBusiness;


        public DocumentosDiagnosticoPasosCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iCommonCoreBusiness = new CommonCoreBusiness();
            this._iNivelesCoreBusiness = new NivelesCoreBusiness();
            this._iFasesCoreBusiness = new FasesCoreBusiness();
            this._iDocumentosDiagnosticoPasos_CriteriosCoreBusiness = new DocumentosDiagnosticoPasos_CriteriosCoreBusiness();
            this._iDocumentosDiagnosticoPasos_NivelesCoreBusiness = new DocumentosDiagnosticoPasos_NivelesCoreBusiness();
        }

        #region CRUD Generico
        public async Task<List<DocumentosDiagnosticoPasosDTO>> ListaDocumentosDiagnosticoPasosPorSistemaDeGestionDTOAsync(string sistemaDeGestion)
        {
            try
            {
                var listaPasosPorSistemaDeGestion = await this.FindWhereAsync(x => x.StrSistemaDeGestionID == sistemaDeGestion);

                var listaPasosPorSistemaDeGestionDTO = listaPasosPorSistemaDeGestion.Select(x => new DocumentosDiagnosticoPasosDTO()
                {
                    StrPasoID = x.StrPasoID,
                    StrFaseID = x.StrFaseID,
                    StrFaseDescripcionPrimaria = x.Fases.StrDescripcionPrimaria,
                    StrFaseDescripcionSecundaria = x.Fases.StrDescripcionSecundaria,
                    StrSistemaDeGestionID = x.StrSistemaDeGestionID,
                    IntNumero = x.IntNumero,
                    StrRequisito = x.StrRequisito,
                    BitActivo = x.BitActivo,

                    Niveles = x.DocumentosDiagnosticoPasos_Niveles.ToList().Select(nivel => new Niveles()
                    {
                        StrNivelID = nivel.StrNivelID,
                        StrDescripcion = nivel.Niveles.StrDescripcion,
                        IntOrden = nivel.Niveles.IntOrden,
                    }).ToList(),

                    DocumentosDiagnosticoPasos_CriteriosDTO = x.DocumentosDiagnosticoPasos_Criterios.ToList().Select(criterio => new DocumentosDiagnosticoPasos_CriteriosDTO()
                    {
                        StrCriterioID = criterio.StrCriterioID,
                        StrPasoID = criterio.StrPasoID,
                        StrDescripcion = criterio.StrDescripcion,
                        StrEvidenciaID = criterio.StrEvidenciaID,

                    }).ToList(),

                    DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO = x.DocumentosDiagnosticoPasos_DocumentoEvidencia.ToList().Select(evidencia => new DocumentosDiagnosticoPasos_DocumentoEvidenciaDTO()
                    {
                        StrEvidenciaID = evidencia.StrEvidenciaID,
                        StrPasoID = evidencia.StrPasoID,
                        StrDescripcion = evidencia.StrDescripcion,

                        DocumentosDiagnosticoPasos_CriteriosDTO = evidencia.DocumentosDiagnosticoPasos_Criterios.Where(criterio => criterio.StrEvidenciaID == evidencia.StrEvidenciaID).ToList().Select(criterio => new DocumentosDiagnosticoPasos_CriteriosDTO()
                        {
                            StrCriterioID = criterio.StrCriterioID,
                            StrDescripcion = criterio.StrDescripcion,
                            StrPasoID = criterio.StrPasoID,

                        }).ToList(),
                    }).ToList(),

                }).ToList();

                return listaPasosPorSistemaDeGestionDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(DocumentosDiagnosticoPasos entity)
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

        private new async Task<string> CreateAsync(DocumentosDiagnosticoPasos entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.StrSistemaDeGestionID == entity.StrSistemaDeGestionID);

                if (listaRegistros.Any(x => x.IntNumero == entity.IntNumero))
                    return String.Format(RecursosDocumentosDiagnosticosPasos.msnPasoDuplicado, entity.IntNumero, entity.StrSistemaDeGestionID);

                entity.StrPasoID = Guid.NewGuid().ToString();
                entity.StrRequisito = entity.StrRequisito.Trim();

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(DocumentosDiagnosticoPasos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<DocumentosDiagnosticoPasos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<DocumentosDiagnosticoPasos, bool>> match)
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

        public new async Task<DocumentosDiagnosticoPasos> FindAsync(Expression<Func<DocumentosDiagnosticoPasos, bool>> match)
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

        public new List<DocumentosDiagnosticoPasos> GetAll()
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

        public new async Task<List<DocumentosDiagnosticoPasos>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(DocumentosDiagnosticoPasos entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.StrPasoID != entity.StrPasoID && x.StrSistemaDeGestionID == entity.StrSistemaDeGestionID && x.IntNumero == entity.IntNumero);

                if (listaRegistros.Count() != 0)
                    return String.Format(RecursosDocumentosDiagnosticosPasos.msnPasoDuplicado, entity.IntNumero, entity.StrSistemaDeGestionID);

                var DocumentosDiagnosticoPasos = await this.FindAsync(x => x.StrPasoID == entity.StrPasoID);

                DocumentosDiagnosticoPasos.StrFaseID = entity.StrFaseID;
                DocumentosDiagnosticoPasos.StrSistemaDeGestionID = entity.StrSistemaDeGestionID;
                DocumentosDiagnosticoPasos.IntNumero = entity.IntNumero;
                DocumentosDiagnosticoPasos.StrRequisito = entity.StrRequisito;
                DocumentosDiagnosticoPasos.BitActivo = entity.BitActivo;

                await base.UpdateAsync(DocumentosDiagnosticoPasos);

                return await _iDocumentosDiagnosticoPasos_NivelesCoreBusiness.ActualizarNivelesEnDocumentoDiagnosticoPasosAsync(entity.StrPasoID, entity.DocumentosDiagnosticoPasos_Niveles.ToList());

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> GuardarAsync(DocumentosDiagnosticoPasos model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.StrPasoID))
                    return await this.UpdateAsync(model);

                model.DocumentosDiagnosticoPasos_Niveles.ToList().ForEach(item => item.StrRegistroID = Guid.NewGuid().ToString());
                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string>EliminarDocumentoPasoAsync(string pasoID)
        {
            try
            {
                var pasoModelo = await this.FindAsync(x => x.StrPasoID == pasoID);

                if (pasoModelo.DocumentosDiagnosticoDetalle.Count() != 0)
                    return RecursosDocumentosDiagnosticosPasos.msnPasoEnDocumentoDiagnostico;

                await this.DeleteAsync(pasoModelo);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region DropDownList
        public async Task<MultiSelectList> DropDownListMultipleNivelesAsync(DocumentosDiagnosticoPasos model = null)
        {
            try
            {
                List<string> listaRegistrosSeleccionados = new List<string>();

                if (model != null)
                    model.DocumentosDiagnosticoPasos_Niveles.ToList().ForEach(item => listaRegistrosSeleccionados.Add(item.StrNivelID));

                return await _iNivelesCoreBusiness.DropDownListMultipleNivelesAsync(listaRegistrosSeleccionados);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> DropDownListFasesAsync(string valueSelected = null)
        {
            try
            {
                return await _iFasesCoreBusiness.DropDownListFasesAsync(valueSelected);
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

        ~DocumentosDiagnosticoPasosCoreBusiness()
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

                if (_iNivelesCoreBusiness != null)
                {
                    _iNivelesCoreBusiness.Dispose();
                    _iNivelesCoreBusiness = null;
                }

                if (_iFasesCoreBusiness != null)
                {
                    _iFasesCoreBusiness.Dispose();
                    _iFasesCoreBusiness = null;
                }

                if (_iDocumentosDiagnosticoPasos_CriteriosCoreBusiness != null)
                {
                    _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.Dispose();
                    _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness = null;
                }

                if (_iDocumentosDiagnosticoPasos_NivelesCoreBusiness != null)
                {
                    _iDocumentosDiagnosticoPasos_NivelesCoreBusiness.Dispose();
                    _iDocumentosDiagnosticoPasos_NivelesCoreBusiness = null;
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
