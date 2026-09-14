using Models.DTO;
using System;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IAusentismoIndicadoresDataAccess : IDisposable
    {
        void EliminarAusentismoIndicadoresEnCachePorEmpresa();
        List<AusentismoIndicadoresDTO> ObtenerListaIndicadoresGenerales(int anio = 0);
    }
}
