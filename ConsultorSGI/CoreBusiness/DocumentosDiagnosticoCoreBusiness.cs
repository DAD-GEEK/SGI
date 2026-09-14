using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
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
    public class DocumentosDiagnosticoCoreBusiness : CRUDGenerico<DocumentosDiagnostico>, IDocumentosDiagnosticoCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private IDocumentosDiagnosticoPasosCoreBusiness _iDocumentosDiagnosticoPasosCoreBusiness;
        private IDocumentosDiagnosticoDetalleCoreBusiness _iDocumentosDiagnosticoDetalleCoreBusiness;
        private IDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness _iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness;
        private IDocumentosDiagnosticoPasos_CriteriosCoreBusiness _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness;
        private ITercerosCoreBusiness _iTercerosCoreBusiness;

        public DocumentosDiagnosticoCoreBusiness() : base(new gestioni_consultorNetEntities(new ServicioTercero()))
        {
            this._iDocumentosDiagnosticoPasosCoreBusiness = new DocumentosDiagnosticoPasosCoreBusiness();
            this._iDocumentosDiagnosticoPasos_CriteriosCoreBusiness = new DocumentosDiagnosticoPasos_CriteriosCoreBusiness();
            this._iDocumentosDiagnosticoDetalleCoreBusiness = new DocumentosDiagnosticoDetalleCoreBusiness();
            this._iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness = new DocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness();
            this._iTercerosCoreBusiness = new TercerosCoreBusiness();
        }

        #region CRUD Generico
        public async Task<List<DocumentosDiagnosticoDTO>> ObtenerListaDeDocumentosDiagnosticoDTOPorSistemaAsync(string sistemaDeGestion)
        {
            try
            {
                return new List<DocumentosDiagnosticoDTO>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DocumentosDiagnosticoDTO> ObtenerDocumentoDiagnosticoDTO(string documentoID)
        {
            try
            {
                var documentoDiagnostico = await this.FindAsync(x => x.StrDocumentoID == documentoID);

                var documentoDiagnosticoDTO = new DocumentosDiagnosticoDTO();

                documentoDiagnosticoDTO.InformacionEmpresa = await _iTercerosCoreBusiness.ObtenerTerceroDTOAsync(documentoDiagnostico.IntTerceroID);
                documentoDiagnosticoDTO.StrDocumentoID = documentoID;
                documentoDiagnosticoDTO.DatFechaElaboracion = documentoDiagnostico.DatFechaElaboracion;
                documentoDiagnosticoDTO.StrUsuarioID = documentoDiagnostico.StrUsuarioID;
                documentoDiagnosticoDTO.StrUsuarioNombre = documentoDiagnostico.AspNetUsers.NombreUsuario;
                documentoDiagnosticoDTO.BitFinalizado = documentoDiagnostico.BitFinalizado;
                documentoDiagnosticoDTO.StrSistemaDeGestionID = documentoDiagnostico.StrSistemaDeGestionID;

                var listaFasesEnDocumento = documentoDiagnostico.DocumentosDiagnosticoDetalle.Select(x => new Fases()
                {
                    StrFaseID = x.DocumentosDiagnosticoPasos.StrFaseID,
                    StrDescripcionPrimaria = x.DocumentosDiagnosticoPasos.Fases.StrDescripcionPrimaria,
                    StrDescripcionSecundaria = x.DocumentosDiagnosticoPasos.Fases.StrDescripcionSecundaria,

                }).ToList();

                documentoDiagnosticoDTO.FasesDocumento = listaFasesEnDocumento.GroupBy(fase => fase.StrFaseID).Select(fase => new FasesDTO()
                {
                    StrFaseID = fase.Key,
                    StrDescripcionPrimaria = listaFasesEnDocumento.FirstOrDefault(x => x.StrFaseID == fase.Key).StrDescripcionPrimaria,
                    StrDescripcionSecundaria = listaFasesEnDocumento.FirstOrDefault(x => x.StrFaseID == fase.Key).StrDescripcionSecundaria,
                    PasosDocumento = documentoDiagnosticoDTO.PasosDocumento

                }).ToList();

                documentoDiagnosticoDTO.PasosDocumento = documentoDiagnostico.DocumentosDiagnosticoDetalle.Select(x => new DocumentosDiagnosticoPasosDTO()
                {
                    StrPasoID = x.StrPasoID,
                    StrFaseID = x.DocumentosDiagnosticoPasos.StrFaseID,
                    StrFaseDescripcionPrimaria = x.DocumentosDiagnosticoPasos.Fases.StrDescripcionPrimaria,
                    StrFaseDescripcionSecundaria = x.DocumentosDiagnosticoPasos.Fases.StrDescripcionSecundaria,
                    StrSistemaDeGestionID = x.DocumentosDiagnosticoPasos.StrSistemaDeGestionID,
                    IntNumero = x.DocumentosDiagnosticoPasos.IntNumero,
                    StrRequisito = x.DocumentosDiagnosticoPasos.StrRequisito,
                    BitActivo = x.DocumentosDiagnosticoPasos.BitActivo,
                    BitAplica = x.DocumentosDiagnosticoPasos.DocumentosDiagnosticoPasos_Niveles.Any(p => p.StrNivelID == x.DocumentosDiagnostico.Terceros.StrNivelID),

                    DocumentosDiagnosticoPasos_CriteriosDTO = x.DocumentosDiagnosticoDetalle_PlanesDeAccion.Select(c => new DocumentosDiagnosticoPasos_CriteriosDTO()
                    {
                        StrCriterioID = c.StrCriterioID,
                    }).ToList(),

                    DocumentosDiagnosticoDetalle_PlanesDeAccionDTO = x.DocumentosDiagnosticoDetalle_PlanesDeAccion.Select(c => new DocumentosDiagnosticoDetalle_PlanesDeAccionDTO()
                    {
                        StrFaseDescripcion = x.DocumentosDiagnosticoPasos.Fases.StrDescripcionPrimaria,
                        StrPasoID = x.StrPasoID,
                        StrNumeroPaso = x.DocumentosDiagnosticoPasos.IntNumero.ToString(),
                        StrCalificacionID = c.StrCalificacionID,
                        StrCriterioID = c.StrCriterioID,
                        StrCriterioDescripcion = c.DocumentosDiagnosticoPasos_Criterios.StrDescripcion,
                        IntOrden = (byte)c.DocumentosDiagnosticoPasos_Criterios.IntOrden,
                        StrSoporteDescripcion = c.DocumentosDiagnosticoPasos_Criterios.StrEvidenciaID != null ? c.DocumentosDiagnosticoPasos_Criterios.DocumentosDiagnosticoPasos_DocumentoEvidencia.StrDescripcion : string.Empty,
                        BitAplica = c.BitAplica,
                        BitSeEvidencia = c.BitSeEvidencia,
                        StrComoSeEvidencia = c.StrComoSeEvidencia,
                        StrPlanDeAccion = c.StrPlanDeAccion,
                        StrResponsable = c.StrResponsable,
                        DatFecha = c.DatFecha,
                        BitBloquear = !x.DocumentosDiagnosticoPasos.DocumentosDiagnosticoPasos_Niveles.Any(p => p.StrNivelID == c.DocumentosDiagnosticoDetalle.DocumentosDiagnostico.Terceros.StrNivelID),
                        DatFechaCalificacion = c.DatFechaCalificacion

                    }).ToList(),

                }).ToList();

                documentoDiagnosticoDTO.FasesDocumento.OrderBy(x => x.StrDescripcionPrimaria).ToList().ForEach(item =>
                {
                    item.PasosDocumento = documentoDiagnosticoDTO.PasosDocumento.Where(x => x.StrFaseID == item.StrFaseID).ToList();
                    item.GraficaDeRequisitos = this.ConsolidadoDeRequisitosPorFases(documentoDiagnosticoDTO, item.StrFaseID);
                });

                documentoDiagnosticoDTO.GraficaFases = this.ConsolidadoGlobalPorFases(documentoDiagnosticoDTO);
                documentoDiagnosticoDTO.PasosGenericosPendientesDeAsignar = await this.ObtenerPasosGenericosPendientesDeAsignar(documentoDiagnosticoDTO);
                documentoDiagnosticoDTO.CriteriosGenericosPendientesDeAsignar = await this.ObtenerCriteriosGenericosPendientesDeAsignar(documentoDiagnosticoDTO);

                return documentoDiagnosticoDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<DocumentosDiagnosticoPasosDTO>> ObtenerPasosGenericosPendientesDeAsignar(DocumentosDiagnosticoDTO documentoDiagnosticoDTO)
        {
            try
            {
                var listaPasosGenericosDTO = await _iDocumentosDiagnosticoPasosCoreBusiness.ListaDocumentosDiagnosticoPasosPorSistemaDeGestionDTOAsync(documentoDiagnosticoDTO.StrSistemaDeGestionID);

                var listaPasosPendientesPorAsignar = listaPasosGenericosDTO.Where(x => !documentoDiagnosticoDTO.PasosDocumento.Any(y => y.StrPasoID == x.StrPasoID)).ToList();

                return listaPasosPendientesPorAsignar;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<DocumentosDiagnosticoPasos_CriteriosDTO>> ObtenerCriteriosGenericosPendientesDeAsignar(DocumentosDiagnosticoDTO documentoDiagnosticoDTO)
        {
            try
            {
                var listaCriteriosGenericosDTO = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.ObteneCriteriosGenericosPorSistemaDeGestionDTOAsync(documentoDiagnosticoDTO.StrSistemaDeGestionID);

                var listaCriteriosEnDocumentoDTO = new List<DocumentosDiagnosticoPasos_CriteriosDTO>();
                documentoDiagnosticoDTO.PasosDocumento.ForEach(x =>
                {
                    listaCriteriosEnDocumentoDTO.AddRange(x.DocumentosDiagnosticoPasos_CriteriosDTO);
                });

                var listaCriteriosPendientesPorAsignar = listaCriteriosGenericosDTO.Where(x => !listaCriteriosEnDocumentoDTO.Any(y => y.StrCriterioID == x.StrCriterioID)).ToList();

                return listaCriteriosPendientesPorAsignar;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public new async Task<string> SaveEntityAsync(DocumentosDiagnostico entity)
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

        private new async Task<string> CreateAsync(DocumentosDiagnostico entity)
        {
            try
            {
                var usuarioEnSesion = ServicioUsuario.ObtenerDatosDeUsuarioEnSesion;

                entity.StrDocumentoID = Guid.NewGuid().ToString();
                entity.DatFechaElaboracion = entity.DatFechaElaboracion;
                entity.StrUsuarioID = usuarioEnSesion.Id;
                entity.BitFinalizado = false;

                var obtenerPasosDeDocumento = await _iDocumentosDiagnosticoPasosCoreBusiness.FindWhereAsync(x => x.StrSistemaDeGestionID == entity.StrSistemaDeGestionID);

                entity.DocumentosDiagnosticoDetalle = obtenerPasosDeDocumento.Select(x => new DocumentosDiagnosticoDetalle()
                {
                    StrDocumentoDetalleID = Guid.NewGuid().ToString(),
                    StrPasoID = x.StrPasoID,

                    DocumentosDiagnosticoDetalle_PlanesDeAccion = x.DocumentosDiagnosticoPasos_Criterios.Select(c => new DocumentosDiagnosticoDetalle_PlanesDeAccion()
                    {
                        StrCalificacionID = Guid.NewGuid().ToString(),
                        StrCriterioID = c.StrCriterioID,
                        BitAplica = true,
                        BitSeEvidencia = false,
                    }).ToList()

                }).ToList();

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(DocumentosDiagnostico entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<DocumentosDiagnostico> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<DocumentosDiagnostico, bool>> match)
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

        public new async Task<DocumentosDiagnostico> FindAsync(Expression<Func<DocumentosDiagnostico, bool>> match)
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

        public new List<DocumentosDiagnostico> GetAll()
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

        public new async Task<List<DocumentosDiagnostico>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(DocumentosDiagnostico entity)
        {
            try
            {
                var DocumentosDiagnostico = await this.FindAsync(x => x.StrDocumentoID == entity.StrDocumentoID);

                DocumentosDiagnostico.DatFechaElaboracion = entity.DatFechaElaboracion;
                DocumentosDiagnostico.BitFinalizado = entity.BitFinalizado;

                await base.UpdateAsync(DocumentosDiagnostico);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> GuardarAsync(DocumentosDiagnostico model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.StrDocumentoID))
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region DiligenciarDocumento
        public async Task<TercerosDTO> ObtenerInformacionDeEmpresaAsync(int terceroID)
        {
            try
            {
                return await _iTercerosCoreBusiness.ObtenerTerceroDTOAsync(terceroID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GraficosChartDTO> ObtenerResultadoConsolidadoGlobalAsync(string documentoID)
        {
            try
            {
                var documentoDiagnosticoDTO = await this.ObtenerDocumentoDiagnosticoDTO(documentoID);

                GraficosChartDTO consolidadoDocumento = new GraficosChartDTO();

                consolidadoDocumento = this.ConsolidadoGlobalPorFases(documentoDiagnosticoDTO);

                return consolidadoDocumento;


            }
            catch (Exception)
            {

                throw;
            }
        }

        private GraficosChartDTO ConsolidadoGlobalPorFases(DocumentosDiagnosticoDTO documentosDiagnosticoDTO)
        {
            try
            {
                GraficosChartDTO chartGlobalFases = new GraficosChartDTO();
                chartGlobalFases.datasets = new List<DatasetsChartDTO>();
                chartGlobalFases.labels = new List<string>();

                var dataset = new DatasetsChartDTO();
                dataset.backgroundColor = new List<string>();
                dataset.borderColor = Across.ParametrosGenerales.Color_Black;
                dataset.data = new List<int>();

                documentosDiagnosticoDTO.FasesDocumento.OrderBy(x => x.StrDescripcionPrimaria).ToList().ForEach(item =>
                {
                    chartGlobalFases.labels.Add(item.StrDescripcionSecundaria);

                    var totalResultadoPorFase = item.DecNivelDeCumplimiento;
                    var valorEnPorcentaje = Convert.ToInt32(totalResultadoPorFase * 100);

                    if (valorEnPorcentaje >= 0 && valorEnPorcentaje < 50)
                        dataset.backgroundColor.Add(ParametrosGenerales.Color_DangerRGBTransparent);
                    else if (valorEnPorcentaje >= 50 && valorEnPorcentaje < 80)
                        dataset.backgroundColor.Add(ParametrosGenerales.Color_WarningRGBTransparent);
                    else
                        dataset.backgroundColor.Add(ParametrosGenerales.Color_SuccessRGBTransparent);

                    dataset.data.Add(valorEnPorcentaje);

                });

                chartGlobalFases.datasets.Add(dataset);

                return chartGlobalFases;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private GraficosChartDTO ConsolidadoDeRequisitosPorFases(DocumentosDiagnosticoDTO documentosDiagnosticoDTO, string faseID)
        {
            try
            {
                GraficosChartDTO chartGlobalFases = new GraficosChartDTO();
                chartGlobalFases.datasets = new List<DatasetsChartDTO>();
                chartGlobalFases.labels = new List<string>();

                var dataset = new DatasetsChartDTO();
                dataset.backgroundColor = new List<string>();
                dataset.borderColor = Across.ParametrosGenerales.Color_Black;
                dataset.data = new List<int>();

                var listaPasos = documentosDiagnosticoDTO.PasosDocumento.Where(x => x.StrFaseID == faseID).OrderBy(x => x.IntNumero).ToList();

                listaPasos.ForEach(paso =>
                {
                    chartGlobalFases.labels.Add(paso.StrRequisito);

                    var totalResultadoPorFase = paso.DecResultado;
                    var valorEnPorcentaje = Convert.ToInt32(totalResultadoPorFase * 100);

                    if (valorEnPorcentaje >= 0 && valorEnPorcentaje < 50)
                        dataset.backgroundColor.Add(ParametrosGenerales.Color_DangerRGBTransparent);
                    else if (valorEnPorcentaje >= 50 && valorEnPorcentaje < 80)
                        dataset.backgroundColor.Add(ParametrosGenerales.Color_WarningRGBTransparent);
                    else
                        dataset.backgroundColor.Add(ParametrosGenerales.Color_SuccessRGBTransparent);

                    dataset.data.Add(valorEnPorcentaje);
                });

                chartGlobalFases.datasets.Add(dataset);


                return chartGlobalFases;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> AgregarElementosPendientesAsync(DocumentosDiagnosticoDTO documentosDiagnosticoDTO)
        {
            try
            {
                var documentoDiagnostico = await this.FindAsync(x => x.StrDocumentoID == documentosDiagnosticoDTO.StrDocumentoID);

                if (documentosDiagnosticoDTO.PasosGenericosPendientesDeAsignar != null)
                {
                    var listaPasosPendientesParaCrear = documentosDiagnosticoDTO.PasosGenericosPendientesDeAsignar.Select(x => new DocumentosDiagnosticoDetalle()
                    {
                        StrDocumentoDetalleID = Guid.NewGuid().ToString(),
                        StrDocumentoID = documentosDiagnosticoDTO.StrDocumentoID,
                        StrPasoID = x.StrPasoID

                    }).ToList();

                    await _iDocumentosDiagnosticoDetalleCoreBusiness.CreateRangeAsync(listaPasosPendientesParaCrear);
                }

                if (documentosDiagnosticoDTO.CriteriosGenericosPendientesDeAsignar != null)
                {
                    var listaCriterios = await _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.GetAllAsync();

                    List<DocumentosDiagnosticoDetalle_PlanesDeAccion> listaCriteriosParaCrear = new List<DocumentosDiagnosticoDetalle_PlanesDeAccion>();

                    foreach (var criterio in documentosDiagnosticoDTO.CriteriosGenericosPendientesDeAsignar)
                    {
                        var pasoDeCriterio = listaCriterios.FirstOrDefault(x => x.StrCriterioID == criterio.StrCriterioID);
                        var detalleDePaso = documentoDiagnostico.DocumentosDiagnosticoDetalle.FirstOrDefault(x => x.StrPasoID == pasoDeCriterio.StrPasoID);

                        var documentosDiagnosticoDetalle_PlanesDeAccion = new DocumentosDiagnosticoDetalle_PlanesDeAccion();

                        documentosDiagnosticoDetalle_PlanesDeAccion.StrCalificacionID = Guid.NewGuid().ToString();
                        documentosDiagnosticoDetalle_PlanesDeAccion.StrDocumentoDetalleID = detalleDePaso.StrDocumentoDetalleID;
                        documentosDiagnosticoDetalle_PlanesDeAccion.StrCriterioID = criterio.StrCriterioID;
                        documentosDiagnosticoDetalle_PlanesDeAccion.BitAplica = true;

                        listaCriteriosParaCrear.Add(documentosDiagnosticoDetalle_PlanesDeAccion);
                    }

                    await _iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness.CreateRangeAsync(listaCriteriosParaCrear);

                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string>FinalizarDocumentoAsync(DocumentosDiagnostico modelo)
        {
            try
            {
                var documentoDiagnostico = await this.FindAsync(x => x.StrDocumentoID == modelo.StrDocumentoID);
                documentoDiagnostico.BitFinalizado = true;

                return await this.GuardarAsync(documentoDiagnostico);

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

        ~DocumentosDiagnosticoCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._iDocumentosDiagnosticoPasosCoreBusiness != null)
                {
                    _iDocumentosDiagnosticoPasosCoreBusiness.Dispose();
                    _iDocumentosDiagnosticoPasosCoreBusiness = null;
                }
                
                if (this._iDocumentosDiagnosticoDetalleCoreBusiness != null)
                {
                    _iDocumentosDiagnosticoDetalleCoreBusiness.Dispose();
                    _iDocumentosDiagnosticoDetalleCoreBusiness = null;
                }
                
                if (this._iDocumentosDiagnosticoPasos_CriteriosCoreBusiness != null)
                {
                    _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness.Dispose();
                    _iDocumentosDiagnosticoPasos_CriteriosCoreBusiness = null;
                }
                
                if (this._iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness != null)
                {
                    _iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness.Dispose();
                    _iDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness = null;
                }

                if (this._iTercerosCoreBusiness != null)
                {
                    _iTercerosCoreBusiness.Dispose();
                    _iTercerosCoreBusiness = null;
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
