using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IConfiguracionPorTerceroCoreBusiness : ICRUDGenerico<ConfiguracionPorTercero>, IDisposable
    {
        Task<Terceros> ObtenerInformacionDeTerceroEnSesionAsync();
        Task<string> SaveAllAsync(ConfiguracionPorTercero model);
    }
}
