using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IAusentismoIndicadores_InformacionPorEmpresaCoreBusiness : ICRUDGenerico<AusentismoIndicadores_InformacionPorEmpresa>, IDisposable
    {
        Task<string> GuardarAsync(AusentismoIndicadores_InformacionPorEmpresa modelo);
    }
}
