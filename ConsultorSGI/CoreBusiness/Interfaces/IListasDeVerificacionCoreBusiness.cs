using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IListasDeVerificacionCoreBusiness : ICRUDGenerico<ListasDeVerificacion>, IDisposable
    {
        Task<string> CopiarPlantillaEnListaDeVerificacionDeProcesoActualAsync(int auditoriaDetalleID, int plantillaDetalleID);
        Task<List<NormasDTO>> ContadorDeNoConformidadesAsync(List<AuditoriaNoConformidadesDTO> listaNoConformidadesDTO);
        Task<string> CreateRangeAsync(List<ListasDeVerificacion> listEntities);
        Task<Auditorias> ObtenerAuditoriaAsync(int auditoriaID);
        Task<List<AuditoriasDetalleDTO>> ObtenerAuditoriaDetalleDTOAsync(Auditorias auditoria);
        Task<AuditoriasDetalle> ObtenerAuditoriaDetallePorIDAsync(int auditoriaDetalleID);
        Task<InformeAuditoriaDTO> ObtenerInformeListasDeVerificacionPDFAsync(int auditoriaDetalleID);
        Task<ListasDeVerificacionDTO> ObtenerItemListaDeVerificacionDTOAsync(ListasDeVerificacion listasDeVerificacion);
        Task<List<PlantillasListasDeVerificacionDetalle>> ObtenerListaDeVerificacionDeProcesoActualAsync(int procesoID);
        Task<List<AuditoriaNoConformidadesDTO>> ObtenerNoConformidadesAuditoriaAsync(int auditoriaID);
        Task<SelectList> ObtenerNormasDeAuditoriaAsync(int listaVerificacionID, int normaID);
        Task<List<NormasDTO>> ObtenerNumeralesParaSeleccionarAsync(NumeralesGenericosDTO datosGenericos);
        Task<List<ListasDeVerificacion_NumeralesDTO>> ObtenerNumeralesPorListaDeVerificacionDTOAsync(int listaDeVerificacionID);
        Task<string> UpdateListaDeVerificacionAuditarAsync(ListasDeVerificacion entity, bool isVistaInforme = false);
        Task<string> SaveAllAsync(ListasDeVerificacion model, int procesoID = 0);
    }
}
