using Across;
using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IAusentismoTiemposCoreBusiness : ICRUDGenerico<AusentismoTiempos>, IDisposable
    {
        Task<bool> GuardarTiempoDeIncapacidadPorMesAsync(Ausentismo ausentismoModelo);
        Task<string> GuardarTiempoDeProrrogaPorMesAsync(AusentismoProrrogas prorrogaModelo);
        int ObtenerCantidadTotalDeIncapacidadesPorAnio(List<AusentismoTiempos> tiempoAusentismoPorAnio);
        int ObtenerCantidadTotalIncapacidadesPorAnio(List<AusentismoTiempos> tiempoAusentismoPorAnio);
        Task<List<AusentismoTiempos>> ObtenerTiemposDeAusentismoPorAnio(int anio);
        int ObtenerTotalDiasDeIncapacidadPorAnio(List<AusentismoTiempos> tiempoAusentismoPorAnio);
        List<MesesDelAnioDTO> Obtener_Cantidad_AT_PorPeriodo(List<AusentismoTiempos> listaRegistros);
        List<MesesDelAnioDTO> Obtener_NumeroDeDiasDeIncapacidadPorPeriodo(List<AusentismoTiempos> listaAusentismo, Enumeraciones.enumTiposGlobalesDeIncapacidad tipoGeneralDeAusentismo);
        List<MesesDelAnioDTO> Obtener_NumeroDeIncapacidadesPorPeriodo(List<AusentismoTiempos> listaAusentismo, Enumeraciones.enumTiposGlobalesDeIncapacidad tipoGeneralDeAusentismo);
        Task<string> SaveAllAsync(AusentismoTiempos model);
    }
}
