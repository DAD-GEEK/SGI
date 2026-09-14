using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IAusentismoVariablesCoreBusiness : ICRUDGenerico<AusentismoVariables>, IDisposable
    {
        Task<string> SaveAllAsync(AusentismoVariables model);
        Task<int> GetTotalEmpleadosActivosAsync();
        List<MesesDelAnioDTO> Obtener_NumeroHorasHombreTrabajadasPorPeriodo(List<AusentismoVariables> listaRegistros);
        List<MesesDelAnioDTO> Obtener_NumeroHorasHombreProgramadasPorPeriodo(List<AusentismoVariables> listaRegistros);
        Task<List<AusentismoVariables>> ObtenerVariablesPorAnioAsync(int anio);
    }
}
