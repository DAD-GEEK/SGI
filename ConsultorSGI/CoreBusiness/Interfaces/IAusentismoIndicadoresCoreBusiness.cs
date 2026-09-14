using Across;
using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IAusentismoIndicadoresCoreBusiness: ICRUDGenerico<AusentismoIndicadores>, IDisposable
    {
        Task<string> GuardarIndicadorAsync(AusentismoIndicadores modelo);
        Task<string> GuardarInformacionDeIndicadorPorEmpresaAsyncAsync(AusentismoIndicadores_InformacionPorEmpresa modelo);
        Task<AusentismoIndicadoresDTO> ObtenerDatosDeIndicadoresPorEmpresaAsync(int anioActual, string ausentismoCodigoIndicador);
        Task<IndicadorAusentismoGlobalDTO> ObtenerDatosDeIndicadores_MedicionGeneralAusentismoAsync(int anio, string tipoDeAusentismo);
        Task<List<AusentismoIndicadoresDTO>> ObtenerListaDeIndicadoresDTOPorTipoAsync(Enumeraciones.enumTiposGlobalesDeIncapacidad tipoDeAusentismo, int anio = 0);
        Task<List<AusentismoIndicadoresDTO>> ObtenerListaDeTodosLosIndicadoresAsync(int anio = 0);
    }
}
