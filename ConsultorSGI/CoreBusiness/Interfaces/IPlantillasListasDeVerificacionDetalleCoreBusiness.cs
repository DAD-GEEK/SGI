using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IPlantillasListasDeVerificacionDetalleCoreBusiness : ICRUDGenerico<PlantillasListasDeVerificacionDetalle>, IDisposable
    {
        Task<string> CambiarOrdenamientoPlantillasAsync(List<PlantillasListasDeVerificacionDetalle> listaPlantillasOrdenadas);
        Task<string> CreateRangeAsync(List<PlantillasListasDeVerificacionDetalle> listEntities);
        Task<List<PlantillasListasDeVerificacionDetalleDTO>> GetAllPlantillasListasDeVerificacionDetalleAsync(int procesoID);
        Task<string> GuardarPlantillaDetalleNumeralAsync(PlantillasListasDeVerificacion_Numerales modelo);
        Task<MultiSelectList> MultiSelectListNumeralesAsync(PlantillasListasDeVerificacionDetalle plantillaDetalle);
        Task<List<NormasDTO>> ObtenerNumeralesParaSeleccionarAsync(NumeralesGenericosDTO datosGenericos);
        Task<SelectList> ObtenerNumeralesPorNormaAsync(int normaID);
        Task<List<PlantillasListasDeVerificacion_NumeralesDTO>> ObtenerNumeralesPorPlantillaDetalleDTOAsync(int detalleID);
        Task<SelectList> ObtenerTodasLasNormasAsync(int normaID);
        Task<string> SaveAllAsync(PlantillasListasDeVerificacionDetalle model, int auditoriaID = 0);
        Task<SelectList> SelectListNumeralesAsync();
    }
}
