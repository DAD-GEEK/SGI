using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
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
using System.Linq.Dynamic;

using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class AuditoriasCoreBusiness : CRUDGenerico<Auditorias>, IAuditoriasCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private INormasCoreBusiness _iNormasCoreBusiness;
        private IAuditorias_NormasCoreBusiness _iAuditorias_NormasCoreBusiness;
        private IAuditorias_ProcesosCoreBusiness _iAuditorias_ProcesosCoreBusiness;
        private IAuditoriasDetalleCoreBusiness _iAuditoriasDetalleCoreBusiness;
        private ITerceros_ClientesCoreBusiness _iTercerosCoreBusiness;
        private IProcesosCoreBusiness _iProcesosCoreBusiness;
        private ITerceros_NormasCoreBusiness _iTerceros_NormasCoreBusiness;
        private ICommonCoreBusiness _iCommonCoreBusiness;
        private IAspNetUsersCoreBusiness _iAspNetUsersCoreBusiness;
        private IListasDeVerificacionCoreBusiness _iListasDeVerificacionCoreBusiness;
        private IElementosComunes_NormasCoreBusiness _iElementosComunes_NormasCoreBusiness;
        private IElementosComunesCoreBusiness _iElementosComunesCoreBusiness;
        private IPlantillasListasDeVerificacionDetalleCoreBusiness _iPlantillasListasDeVerificacionDetalleCoreBusiness;

        private int _recordsTotal;
        public int TotalRegistrosDataTable
        {
            get { return _recordsTotal; }
        }

        public AuditoriasCoreBusiness() : base(new gestioni_consultorNetEntities(new ServicioTercero()))
        {
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iAuditorias_NormasCoreBusiness = new Auditorias_NormasCoreBusiness();
            this._iAuditorias_ProcesosCoreBusiness = new Auditorias_ProcesosCoreBusiness();
            this._iTercerosCoreBusiness = new Terceros_ClientesCoreBusiness();
            this._iProcesosCoreBusiness = new ProcesosCoreBusiness();
            this._iTerceros_NormasCoreBusiness = new Terceros_NormasCoreBusiness();
            this._iCommonCoreBusiness = new CommonCoreBusiness();
            this._iAspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();
            this._iElementosComunes_NormasCoreBusiness = new ElementosComunes_NormasCoreBusiness();
            this._iElementosComunesCoreBusiness = new ElementosComunesCoreBusiness();
            this._iPlantillasListasDeVerificacionDetalleCoreBusiness = new PlantillasListasDeVerificacionDetalleCoreBusiness();
        }

        #region CRUD Generico
        public List<AuditoriasDTO> GetPaginacionAuditorias(DatatableParamsDTO datatableParamsDTO, bool paginarInformacion = true)
        {
            try
            {
                List<AuditoriasDTO> listaAuditoriasDTO = null;
                IQueryable<AuditoriasDTO> sentencia = null;

                int pageSize = datatableParamsDTO.length != null ? Convert.ToInt32(datatableParamsDTO.length) : 0;
                int skip = datatableParamsDTO.start != null ? Convert.ToInt32(datatableParamsDTO.start) : 0;

                var usuarioEnSesion = Servicios.ServicioUsuarioCoreBusiness.ObtenerDatosDeUsuarioEnSesion;

                using (gestioni_consultorNetEntities db = new gestioni_consultorNetEntities())
                {
                    sentencia = (from au in db.Auditorias.
                                    Include("Terceros")
                                 join cl in db.Terceros_Clientes on au.IntTerceroClienteID equals cl.IntTerceroClienteID
                                    where au.Terceros_Clientes.Terceros.IntTerceroID == usuarioEnSesion.TerceroID
                                 orderby au.DatFechaInicial

                                 select new AuditoriasDTO
                                 {
                                     IntAuditoriaID = au.IntAuditoriaID,
                                     IntConsecutivo = au.IntConsecutivo,
                                     DatFechaInicial = au.DatFechaInicial,
                                     DatFechaFinal = au.DatFechaFinal,
                                     IntTerceroClienteID = au.IntTerceroClienteID,
                                     StrClienteIdentificacion = cl.StrIdentificacion,
                                     StrClienteNombre = cl.StrNombre,
                                     StrNombreImagen = cl.StrRutaImagen,
                                     BitEstado = cl.BitEstado,
                                 });

                    if (!string.IsNullOrEmpty(datatableParamsDTO.searchValue))
                        sentencia = sentencia.Where(d =>
                        d.StrClienteIdentificacion.ToString().Contains(datatableParamsDTO.searchValue) ||
                        d.StrClienteNombre.Contains(datatableParamsDTO.searchValue));

                    if (!string.IsNullOrEmpty(datatableParamsDTO.sortColumn) && !string.IsNullOrEmpty(datatableParamsDTO.sortColumnDir))
                        sentencia = sentencia.OrderBy(datatableParamsDTO.sortColumn + " " + datatableParamsDTO.sortColumnDir);

                    _recordsTotal = sentencia.Count();

                    if (paginarInformacion)
                        listaAuditoriasDTO = sentencia.Skip(skip).Take(pageSize).ToList();
                    else
                        listaAuditoriasDTO = sentencia.ToList();
                }

                listaAuditoriasDTO.ForEach(item => item.StrClienteLogo = Archivos.GetUrlImagenTerceroClienteOrUrlDefault(item.StrClienteIdentificacion, item.StrNombreImagen));

                return listaAuditoriasDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public new async Task<string> SaveEntityAsync(Auditorias entity)
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

        public new async Task<string> CreateAsync(Auditorias entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntTerceroClienteID == entity.IntTerceroClienteID && x.DatFechaInicial >= entity.DatFechaInicial && x.DatFechaFinal <= entity.DatFechaFinal);
                if (listaRegistros.Count() != 0)
                    return String.Format(RecursoAuditorias.msnAuditoriaYaExiste, entity.DatFechaInicial.ToString("yyyy-MM-dd"), entity.DatFechaFinal.ToString("yyyy-MM-dd"));

                AspNetUsers usuarioEnSesion = await _iCommonCoreBusiness.GetCurrentUser();

                DateTime? fechaDeVersion = null;

                if (!string.IsNullOrEmpty(entity.DatFechaVersion.ToString()))
                    fechaDeVersion = Common.ObtenerFechaExacta((DateTime)entity.DatFechaVersion);

                entity.IntConsecutivo = await this.ObtenerConsecutivoPorTerceroAsync(entity.IntTerceroClienteID);
                entity.DatFechaInicial = Common.ObtenerFechaExacta(entity.DatFechaInicial);
                entity.DatFechaFinal = Common.ObtenerFechaExacta(entity.DatFechaFinal);
                entity.IntTerceroClienteID = entity.IntTerceroClienteID;
                entity.StrVersion = entity.StrVersion;
                entity.DatFechaVersion = fechaDeVersion;
                entity.StrCodigo = entity.StrCodigo;
                entity.StrObjetivo = entity.StrObjetivo;
                entity.StrAlcance = entity.StrAlcance;
                entity.StrFortalezas = entity.StrFortalezas;
                entity.StrConclusiones = entity.StrConclusiones;
                entity.StrUsuarioID = usuarioEnSesion.Id;
                entity.StrUsuarioFirma = entity.StrUsuarioFirma;
                entity.StrIntegranteCopasst = entity.StrIntegranteCopasst;
                entity.StrRepresentanteLegal = entity.StrRepresentanteLegal;
                entity.BitFirma = true;
                entity.BitEstado = false;

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Auditorias entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Auditorias> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Auditorias, bool>> match)
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

        public new async Task<Auditorias> FindAsync(Expression<Func<Auditorias, bool>> match)
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

        public new async Task<List<Auditorias>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Auditorias entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntTerceroClienteID == entity.IntTerceroClienteID && x.DatFechaInicial >= entity.DatFechaInicial && x.DatFechaFinal <= entity.DatFechaFinal && x.IntAuditoriaID != entity.IntAuditoriaID);

                if (listaRegistros.Count() != 0)
                    return String.Format(RecursoAuditorias.msnAuditoriaYaExiste, entity.DatFechaInicial.ToString("yyyy-MM-dd"), entity.DatFechaFinal.ToString("yyyy-MM-dd"));

                Auditorias Auditorias = await this.FindAsync(x => x.IntAuditoriaID == entity.IntAuditoriaID);

                DateTime? fechaDeVersion = null;

                if (!string.IsNullOrEmpty(entity.DatFechaVersion.ToString()))
                    fechaDeVersion = Common.ObtenerFechaExacta((DateTime)entity.DatFechaVersion);

                Auditorias.DatFechaInicial = Common.ObtenerFechaExacta(entity.DatFechaInicial);
                Auditorias.DatFechaFinal = Common.ObtenerFechaExacta(entity.DatFechaFinal);
                Auditorias.IntTerceroClienteID = entity.IntTerceroClienteID;
                Auditorias.StrVersion = entity.StrVersion;
                Auditorias.DatFechaVersion = fechaDeVersion;
                Auditorias.StrCodigo = entity.StrCodigo;
                Auditorias.StrObjetivo = entity.StrObjetivo;
                Auditorias.StrAlcance = entity.StrAlcance;
                Auditorias.StrIntegranteCopasst = entity.StrIntegranteCopasst;
                Auditorias.StrRepresentanteLegal = entity.StrRepresentanteLegal;
                Auditorias.StrUsuarioFirma = entity.StrUsuarioFirma;
                Auditorias.BitEstado = entity.BitEstado;
                Auditorias.DatFechaElaboracionInforme = entity.DatFechaElaboracionInforme;

                await base.UpdateAsync(Auditorias);

                await this.CrearComplementosDeAuditoriaAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Auditorias model)
        {
            try
            {
                this._iListasDeVerificacionCoreBusiness = new ListasDeVerificacionCoreBusiness();

                var respuesta = string.Empty;

                if (model.IntAuditoriaID != 0)
                    respuesta = await this.UpdateAsync(model);
                else
                    return await this.CreateAsync(model);

                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarInformeDeAuditoriaAsync(Auditorias model)
        {
            try
            {
                await base.UpdateAsync(model);
                return String.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<string> CrearComplementosDeAuditoriaAsync(Auditorias auditoria)
        {
            try
            {
                var listaAuditoriaNormas = await _iAuditorias_NormasCoreBusiness.FindWhereAsync(x => x.IntAuditoriaID == auditoria.IntAuditoriaID);

                var listaNormasParaEliminar = listaAuditoriaNormas.Where(x => !auditoria.Auditorias_Normas.Any(y => y.IntNormaID == x.IntNormaID)).ToList();
                await _iAuditorias_NormasCoreBusiness.DeleteRangeAsync(listaNormasParaEliminar);

                foreach (var item in auditoria.Auditorias_Normas)
                {
                    if (!listaAuditoriaNormas.Any(x => x.IntNormaID == item.IntNormaID && x.IntAuditoriaID == item.IntAuditoriaID))
                        await _iAuditorias_NormasCoreBusiness.SaveEntityAsync(item);
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<int> ObtenerConsecutivoPorTerceroAsync(int terceroID)
        {
            try
            {
                int consecutivo = 0;

                List<Auditorias> listaAuditoriasPorTercero = await this.FindWhereAsync(x => x.IntTerceroClienteID == terceroID);

                if (listaAuditoriasPorTercero.Count() != 0)
                    consecutivo = listaAuditoriasPorTercero.Max(x => x.IntConsecutivo);

                return consecutivo + 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AspNetUsers> ObtenerUsuarioQueFirmaAsync(string usuarioID)
        {
            try
            {
                AspNetUsers usuarioModelo = await _iAspNetUsersCoreBusiness.FindAsync(x => x.Id == usuarioID);

                if (usuarioModelo != null)
                    return usuarioModelo;

                return new AspNetUsers();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<InformeAuditoriaDTO> ObtenerInformePlanDeAuditoriaPDFAsync(int auditoriaID)
        {
            try
            {
                this._iListasDeVerificacionCoreBusiness = new ListasDeVerificacionCoreBusiness();
                InformeAuditoriaDTO informeAuditoriaDTO = new InformeAuditoriaDTO();
                informeAuditoriaDTO.Auditoria = await this.FindAsync(x => x.IntAuditoriaID == auditoriaID);
                informeAuditoriaDTO.NoConformidades = await _iListasDeVerificacionCoreBusiness.ObtenerNoConformidadesAuditoriaAsync(auditoriaID);
                informeAuditoriaDTO.Usuario = await this.ObtenerUsuarioQueFirmaAsync(informeAuditoriaDTO.Auditoria.StrUsuarioFirma);
                informeAuditoriaDTO.ListaNormasDTO = await _iListasDeVerificacionCoreBusiness.ContadorDeNoConformidadesAsync(informeAuditoriaDTO.NoConformidades);


                return informeAuditoriaDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<InformeAuditoriaDTO> ObtenerInformePlanAuditoriaPDFAsync(int auditoriaID)
        {
            try
            {
                _iAuditoriasDetalleCoreBusiness = new AuditoriasDetalleCoreBusiness();

                var planDeAuditoriaDTO = await this.ObtenerInformePlanDeAuditoriaPDFAsync(auditoriaID);
                planDeAuditoriaDTO.ListaAuditoriaDetalleDTO = await _iAuditoriasDetalleCoreBusiness.ObtenerDetalleAuditoriaAsync(auditoriaID);

                return planDeAuditoriaDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ElementosComunes>> ObtenerComplementoElementosComunesAsync(int terceroID, int auditoriaID)
        {
            try
            {
                List<Normas> listaNormas = await _iNormasCoreBusiness.FindWhereAsync(x => x.BitActivo == true);
                List<ElementosComunes_Normas> listaElementosComunes = new List<ElementosComunes_Normas>();

                if (auditoriaID != 0)
                {
                    var listaNormasAuditoria = await _iAuditorias_NormasCoreBusiness.FindWhereAsync(x => x.IntAuditoriaID == auditoriaID);
                    listaNormas = listaNormas.Where(x => listaNormasAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();
                    return await this.ObtenerElementosComunesEntreNormasAsync(listaNormas);
                }

                var listaNormasTercero = await _iTerceros_NormasCoreBusiness.FindWhereAsync(x => x.IntTerceroID == terceroID);
                listaNormas = listaNormas.Where(x => listaNormasTercero.Any(y => y.IntNormaID == x.IntNormaID)).ToList();
                return await this.ObtenerElementosComunesEntreNormasAsync(listaNormas);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<ElementosComunes>> ObtenerElementosComunesEntreNormasAsync(List<Normas> listaNormasAuditoria)
        {
            try
            {
                var listaNormasElementosComunes = await _iElementosComunes_NormasCoreBusiness.GetAllAsync();
                listaNormasElementosComunes = listaNormasElementosComunes.Where(x => listaNormasAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                var listaElementosComunes = await _iElementosComunesCoreBusiness.FindWhereAsync(x => x.BitActivo == true);

                List<ElementosComunes> elementosComunesEnAuditoria = new List<ElementosComunes>();

                if (listaElementosComunes.Count() != 0)
                {
                    foreach (var elementoComun in listaNormasElementosComunes.GroupBy(x => x.IntElementoComunID))
                    {
                        bool existeNormaEnElementoComun = false;
                        int contadorNormasConincidentes = 0;

                        foreach (var normas in listaNormasAuditoria)
                        {
                            existeNormaEnElementoComun = listaNormasElementosComunes.Exists(x => x.IntNormaID == normas.IntNormaID && x.IntElementoComunID == elementoComun.Key);

                            if (existeNormaEnElementoComun)
                                contadorNormasConincidentes = contadorNormasConincidentes + 1;
                        }

                        if (contadorNormasConincidentes > 1)
                            elementosComunesEnAuditoria.Add(listaElementosComunes.Find(x => x.IntElementoComunID == elementoComun.Key));
                    }
                }

                return elementosComunesEnAuditoria;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Procesos
        public async Task<List<ProcesosDTO>> ObtenerProcesosAuditoriaDTOAsync(int terceroClienteID, int auditoriaID)
        {
            try
            {
                var listaProcesosPorCliente = await _iProcesosCoreBusiness.FindWhereAsync(x => x.IntTerceroClienteID == terceroClienteID);
                var listaProcesosAuditoria = await _iAuditorias_ProcesosCoreBusiness.FindWhereAsync(x => x.IntAuditoriaID == auditoriaID);

                var listaProcesosDTO = listaProcesosPorCliente.Select(x => new ProcesosDTO()
                {
                    IntProcesoID = x.IntProcesoID,
                    StrCodigo = x.StrCodigo,
                    StrDescripcion = x.StrDescripcion,
                    IntTerceroClienteID = x.IntTerceroClienteID,
                    IntMacroProceso = x.IntMacroProceso,
                    IntProcesoOrigen = x.IntProcesoOrigen,
                    BitActivo = x.BitActivo,
                    BitAuditar = listaProcesosAuditoria.FirstOrDefault(pro => pro.IntProcesoID == x.IntProcesoID) != null ? true : false

                }).ToList();

                return listaProcesosDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SeleccionarProcesoParaAuditarAsync(int auditoriaID, int procesoID)
        {
            try
            {
                var listaProcesosParaAuditar = await _iAuditorias_ProcesosCoreBusiness.FindWhereAsync(x => x.IntAuditoriaID == auditoriaID && x.IntProcesoID == procesoID);

                if (listaProcesosParaAuditar.Count() == 0)
                {
                    Auditorias_Procesos auditorias_Procesos = new Auditorias_Procesos()
                    {
                        IntAuditoriaID = auditoriaID,
                        IntProcesoID = procesoID
                    };

                    await _iAuditorias_ProcesosCoreBusiness.SaveEntityAsync(auditorias_Procesos);
                }
                else
                    await _iAuditorias_ProcesosCoreBusiness.DeleteRangeAsync(listaProcesosParaAuditar);

                return string.Empty;
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


        public async Task<SelectList> SelectListUsuariosAsync(string valorSeleccionado = null)
        {
            try
            {
                return await _iAspNetUsersCoreBusiness.SelectListUsuariosAsync(valorSeleccionado);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<MultiSelectList> MultiSelectListTodasLasNormasAsync(int auditoriaID)
        {
            try
            {
                List<Auditorias_Normas> listaNormasPorAuditoria = new List<Auditorias_Normas>();
                List<Normas> listaNormas = new List<Normas>();

                if (auditoriaID != 0)
                {
                    listaNormas = await _iNormasCoreBusiness.FindWhereAsync(x => x.BitActivo == true);
                    listaNormasPorAuditoria = await _iAuditorias_NormasCoreBusiness.FindWhereAsync(x => x.IntAuditoriaID == auditoriaID);
                }

                if (listaNormasPorAuditoria.Count() != 0)
                    listaNormas = listaNormas.Where(x => listaNormasPorAuditoria.Any(y => y.IntNormaID == x.IntNormaID)).ToList();

                return await _iNormasCoreBusiness.MultiSelectListTodasLasNormasAsync(listaNormas);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> SelectListAuditoriaProcesosAsync(int auditoriaID, string valorSeleccionado = null)
        {
            try
            {
                return await _iAuditorias_ProcesosCoreBusiness.SelectListAsync(auditoriaID, valorSeleccionado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> MultiSelectListProcesosTercerosAsync(int terceroID, int auditoriaID = 0)
        {
            try
            {
                var modeloTercero = await _iTercerosCoreBusiness.FindAsync(x => x.IntTerceroClienteID == terceroID);

                if (auditoriaID != 0)
                {
                    int auditoria = Convert.ToInt32(auditoriaID);
                    var modeloAuditoria = await this.FindAsync(x => x.IntAuditoriaID == auditoria);

                    return await _iProcesosCoreBusiness.MultiSelectListPorTerceroAsync(modeloTercero, modeloAuditoria);
                }

                return await _iProcesosCoreBusiness.MultiSelectListPorTerceroAsync(modeloTercero);
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

        ~AuditoriasCoreBusiness()
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

                if (_iAuditorias_NormasCoreBusiness != null)
                {
                    _iAuditorias_NormasCoreBusiness.Dispose();
                    _iAuditorias_NormasCoreBusiness = null;
                }

                if (_iAuditorias_ProcesosCoreBusiness != null)
                {
                    _iAuditorias_ProcesosCoreBusiness.Dispose();
                    _iAuditorias_ProcesosCoreBusiness = null;
                }

                if (_iAuditoriasDetalleCoreBusiness != null)
                {
                    _iAuditoriasDetalleCoreBusiness.Dispose();
                    _iAuditoriasDetalleCoreBusiness = null;
                }

                if (_iTercerosCoreBusiness != null)
                {
                    _iTercerosCoreBusiness.Dispose();
                    _iTercerosCoreBusiness = null;
                }

                if (_iProcesosCoreBusiness != null)
                {
                    _iProcesosCoreBusiness.Dispose();
                    _iProcesosCoreBusiness = null;
                }

                //if (_iCommonCoreBusiness != null)
                //{
                //    _iCommonCoreBusiness.Dispose();
                //    _iCommonCoreBusiness = null;
                //}

                if (_iAspNetUsersCoreBusiness != null)
                {
                    _iAspNetUsersCoreBusiness.Dispose();
                    _iAspNetUsersCoreBusiness = null;
                }

                if (_iElementosComunes_NormasCoreBusiness != null)
                {
                    _iElementosComunes_NormasCoreBusiness.Dispose();
                    _iElementosComunes_NormasCoreBusiness = null;
                }

                if (_iElementosComunesCoreBusiness != null)
                {
                    _iElementosComunesCoreBusiness.Dispose();
                    _iElementosComunesCoreBusiness = null;
                }

                if (_iPlantillasListasDeVerificacionDetalleCoreBusiness != null)
                {
                    _iPlantillasListasDeVerificacionDetalleCoreBusiness.Dispose();
                    _iPlantillasListasDeVerificacionDetalleCoreBusiness = null;
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
