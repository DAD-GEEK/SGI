using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IPlantillasListasDeVerificacion_NumeralesCoreBusiness : ICRUDGenerico<PlantillasListasDeVerificacion_Numerales>, IDisposable
    {
        Task<string> CreateRangeAsync(List<PlantillasListasDeVerificacion_Numerales> listEntities);
        Task<List<PlantillasListasDeVerificacion_NumeralesDTO>> ObtenerNumeralesPorPlantillaDetalleDTOAsync(int detalleID);
        Task<string> SaveAllAsync(PlantillasListasDeVerificacion_Numerales model);
    }
}
