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
    public class AuditoriasDetalleCoreBusiness : CRUDGenerico<AuditoriasDetalle>, IAuditoriasDetalleCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private IAuditorias_ProcesosCoreBusiness _iAuditorias_ProcesosCoreBusiness;
        private IAuditoriasDetalleACCoreBusiness _iAuditoriasDetalleACCoreBusiness;
        private INormasCoreBusiness _iNormasCoreBusiness;
        private IAuditorias_NormasCoreBusiness _iAuditorias_NormasCoreBusiness;
        private INumeralesCoreBusiness _iNumeralesCoreBusiness;
        private IAuditoriasCoreBusiness _iAuditoriasCoreBusiness;
        private IProcesosCoreBusiness _iProcesosCoreBusiness;
        private IListasDeVerificacionCoreBusiness _iListasDeVerificacionCoreBusiness;
        private IPlantillasListasDeVerificacionDetalleCoreBusiness _iPlantillasListasDeVerificacionDetalleCoreBusiness;

        public AuditoriasDetalleCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iAuditorias_ProcesosCoreBusiness = new Auditorias_ProcesosCoreBusiness();
            this._iAuditoriasDetalleACCoreBusiness = new AuditoriasDetalleACCoreBusiness();
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iAuditorias_NormasCoreBusiness = new Auditorias_NormasCoreBusiness();
            this._iNumeralesCoreBusiness = new NumeralesCoreBusiness();
            this._iAuditoriasCoreBusiness = new AuditoriasCoreBusiness();
            this._iProcesosCoreBusiness = new ProcesosCoreBusiness();
            this._iPlantillasListasDeVerificacionDetalleCoreBusiness = new PlantillasListasDeVerificacionDetalleCoreBusiness();
        }

        #region CRUD Generico
        public async Task<List<AuditoriasDetalleDTO>> ObtenerDetalleAuditoriaAsync(int auditoriaID)
        {
            try
            {
                var auditoria = await _iAuditoriasCoreBusiness.FindAsync(x => x.IntAuditoriaID == auditoriaID);
                List<AuditoriasDetalleDTO> detalleAuditoria = new List<AuditoriasDetalleDTO>();

                if (auditoria is null)
                    return detalleAuditoria;

                var listaDetalleAperturaCierre = await _iAuditoriasDetalleACCoreBusiness.FindWhereAsync(x => x.IntAuditoriaID == auditoriaID);
                var listaDetaleAuditoria = auditoria.AuditoriasDetalle;

                detalleAuditoria.AddRange(listaDetalleAperturaCierre.Select(x => new AuditoriasDetalleDTO()
                {
                    IntAuditoriaDetalleID = x.IntAuditoriaDetalleACID,
                    IntAuditoriaID = auditoriaID,
                    EsApertura_Cierre = true,
                    StrDetalle = x.StrDetalle,
                    DatFechaInicial = x.DatFechaInicial,
                    DatFechaFinal = x.DatFechaFinal,
                }).ToList());

                detalleAuditoria.AddRange(await this.ObtenerAuditoriaDetalleDTOAsync(auditoria));

                return detalleAuditoria.OrderBy(x => x.DatFechaInicial).ToList().OrderBy(x => x.DatFechaInicial).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AuditoriasDetalleDTO>> ObtenerAuditoriaDetalleDTOAsync(Auditorias auditoria)
        {
            try
            {               
                var listaNumerales = await _iNumeralesCoreBusiness.ObtenerNumeralesPorNormasDeAuditoriaAsync(auditoria);

                List<AuditoriasDetalleDTO> listaAuditoriaDetalle = new List<AuditoriasDetalleDTO>();

                foreach (var item in auditoria.AuditoriasDetalle)
                {
                    var listaNumeralesPorProceso = await _iProcesosCoreBusiness.ObtenerNumeralesPorProcesoDTOAsync(item.IntProcesoID);
                    var listaNumeralesListasDeVerificacionPorProceso = listaNumerales.Where(x => listaNumeralesPorProceso.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();

                    AuditoriasDetalleDTO auditoriasDetalleDTO = new AuditoriasDetalleDTO();

                    auditoriasDetalleDTO.NormasDTO = await _iNumeralesCoreBusiness.ObtenerNumeralesPorNormasDTOAsync(listaNumeralesListasDeVerificacionPorProceso);
                    auditoriasDetalleDTO.IntAuditoriaDetalleID = item.IntAuditoriaDetalleID;
                    auditoriasDetalleDTO.IntAuditoriaID = item.IntAuditoriaID;
                    auditoriasDetalleDTO.IntProcesoID = item.IntProcesoID;
                    auditoriasDetalleDTO.StrProcesoCodigo = item.Procesos.StrCodigo;
                    auditoriasDetalleDTO.StrProcesoDescripcion = item.Procesos.StrDescripcion;
                    auditoriasDetalleDTO.StrModalidad = item.StrModalidad;
                    auditoriasDetalleDTO.StrAuditado = item.StrAuditado;
                    auditoriasDetalleDTO.StrAuditor = item.StrAuditor;
                    auditoriasDetalleDTO.DatFechaInicial = item.DatFechaInicial;
                    auditoriasDetalleDTO.DatFechaFinal = item.DatFechaFinal;
                    auditoriasDetalleDTO.StrDetalle = item.StrDetalle;
                    auditoriasDetalleDTO.EsApertura_Cierre = false;

                    var totalListasDeVerificacionParaAuditar = item.ListasDeVerificacion.Count();
                    var cantidadListasDeVerificacionSinAuditar = item.ListasDeVerificacion.Where(x => x.BitConformidad == false && x.BitNoConformidad == false && x.BitObservacion == false).Count();

                    var cantidadListasDeVerificacionAuditadas = item.ListasDeVerificacion.Where(x => x.BitConformidad == true || x.BitNoConformidad == true || x.BitObservacion == true).Count();

                    auditoriasDetalleDTO.StrEstadoAuditoriaProceso = totalListasDeVerificacionParaAuditar == cantidadListasDeVerificacionSinAuditar 
                        ? enumEstadosAuditoriaProcesos.Pendiente.ToString()
                        : totalListasDeVerificacionParaAuditar == cantidadListasDeVerificacionAuditadas 
                            ? enumEstadosAuditoriaProcesos.Auditado.ToString()
                            : enumEstadosAuditoriaProcesos.En_Proceso.ToString();

                    listaAuditoriaDetalle.Add(auditoriasDetalleDTO);
                }

                return listaAuditoriaDetalle;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(AuditoriasDetalle entity)
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

        public new async Task<string> CreateAsync(AuditoriasDetalle entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntAuditoriaDetalleID == entity.IntAuditoriaDetalleID && x.DatFechaInicial >= entity.DatFechaInicial && x.DatFechaFinal <= entity.DatFechaFinal && x.IntProcesoID == entity.IntProcesoID);

                if (listaRegistros.Count() != 0)
                    return String.Format(RecursoAuditorias.msnAuditoriaDetalleYaExisteParaElProceso, "seleccionado");

                if (entity.DatFechaInicial >= entity.DatFechaFinal)
                    return RecursoAuditorias.msnHoraFinalMenorAhoraInicial;

                entity.IntAuditoriaID = entity.IntAuditoriaID;
                entity.IntProcesoID = entity.IntProcesoID;
                entity.StrModalidad = entity.StrModalidad;
                entity.StrAuditado = entity.StrAuditado;
                entity.StrAuditor = entity.StrAuditor;
                entity.DatFechaInicial = Common.ObtenerFechaExacta(entity.DatFechaInicial);
                entity.DatFechaFinal = Common.ObtenerFechaExacta(entity.DatFechaFinal);
                entity.StrDetalle = entity.StrDetalle;

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AuditoriasDetalle entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AuditoriasDetalle> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AuditoriasDetalle, bool>> match)
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

        public new async Task<AuditoriasDetalle> FindAsync(Expression<Func<AuditoriasDetalle, bool>> match)
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

        public new async Task<List<AuditoriasDetalle>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(AuditoriasDetalle entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntAuditoriaID == entity.IntAuditoriaID && x.DatFechaInicial >= entity.DatFechaInicial && x.DatFechaFinal <= entity.DatFechaFinal && x.IntAuditoriaDetalleID != entity.IntAuditoriaDetalleID && x.IntProcesoID == entity.IntProcesoID);

                if (entity.DatFechaInicial >= entity.DatFechaFinal)
                    return RecursoAuditorias.msnHoraFinalMenorAhoraInicial;

                AuditoriasDetalle AuditoriasDetalle = await this.FindAsync(x => x.IntAuditoriaDetalleID == entity.IntAuditoriaDetalleID);

                if (listaRegistros.Count() != 0)
                    return String.Format(RecursoAuditorias.msnAuditoriaDetalleYaExisteParaElProceso, AuditoriasDetalle.Procesos.StrDescripcion);

                AuditoriasDetalle.IntAuditoriaID = entity.IntAuditoriaID;
                AuditoriasDetalle.IntProcesoID = entity.IntProcesoID;
                AuditoriasDetalle.StrModalidad = entity.StrModalidad;
                AuditoriasDetalle.StrAuditado = entity.StrAuditado;
                AuditoriasDetalle.StrAuditor = entity.StrAuditor;
                AuditoriasDetalle.DatFechaInicial = Common.ObtenerFechaExacta(entity.DatFechaInicial);
                AuditoriasDetalle.DatFechaFinal = Common.ObtenerFechaExacta(entity.DatFechaFinal);
                AuditoriasDetalle.StrDetalle = entity.StrDetalle;
                AuditoriasDetalle.DatFechaProgramacion = entity.DatFechaProgramacion;

                await base.UpdateAsync(AuditoriasDetalle);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(AuditoriasDetalle model)
        {
            try
            {
                if (model.DatFechaInicial > model.DatFechaFinal)
                    return RecursoCommon.msnHoraInicialMayorHoraFinal;

                this._iListasDeVerificacionCoreBusiness = new ListasDeVerificacionCoreBusiness();

                string respuesta = string.Empty;

                if (model.IntAuditoriaDetalleID != 0)
                    respuesta = await this.UpdateAsync(model);
                else
                    respuesta = await this.CreateAsync(model);

                if (!string.IsNullOrEmpty(respuesta))
                    return respuesta;

                var modeloAuditoriaDetalle = await this.FindAsync(x => x.IntAuditoriaDetalleID == model.IntAuditoriaDetalleID);

                if (modeloAuditoriaDetalle is null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                var listasPlantillasListasDeVerificacion = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.FindWhereAsync(x => x.IntProcesoID == model.IntProcesoID);
                var listasDeVerificacionEnDB = modeloAuditoriaDetalle.ListasDeVerificacion.ToList();

                listasPlantillasListasDeVerificacion = listasPlantillasListasDeVerificacion.Where(x => !listasDeVerificacionEnDB.Any(y => y.IntPlantillaDetalleID == x.IntPlantillaDetalleID)).ToList();

                if (listasPlantillasListasDeVerificacion.Count() != 0)
                {
                    var listaDeVerificacionParaCrear = listasPlantillasListasDeVerificacion.Select(x => new ListasDeVerificacion()
                    {
                        IntAuditoriaDetalleID = modeloAuditoriaDetalle.IntAuditoriaDetalleID,
                        IntPlantillaDetalleID = x.IntPlantillaDetalleID,
                        StrTitulo = x.StrTitulo,
                        BitEstado = x.BitEstado

                    }).ToList();

                    await _iListasDeVerificacionCoreBusiness.CreateRangeAsync(listaDeVerificacionParaCrear);
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region Select List
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

        public SelectList SelectListModalidades(string valueSelected = null)
        {
            try
            {
                var listaRegistros = Common.GetEnumToSelectList<enumModalidades>();

                var entidad = listaRegistros.Select(x => new SelectListItem()
                {
                    Value = x.Text[0].ToString().ToUpper(),
                    Text = $"{x.Text}"
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

        #endregion

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AuditoriasDetalleCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iAuditorias_ProcesosCoreBusiness != null)
                {
                    _iAuditorias_ProcesosCoreBusiness.Dispose();
                    _iAuditorias_ProcesosCoreBusiness = null;
                }

                if (_iAuditoriasDetalleACCoreBusiness != null)
                {
                    _iAuditoriasDetalleACCoreBusiness.Dispose();
                    _iAuditoriasDetalleACCoreBusiness = null;
                }

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

                if (_iNumeralesCoreBusiness != null)
                {
                    _iNumeralesCoreBusiness.Dispose();
                    _iNumeralesCoreBusiness = null;
                }

                if (_iAuditoriasCoreBusiness != null)
                {
                    _iAuditoriasCoreBusiness.Dispose();
                    _iAuditoriasCoreBusiness = null;
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
