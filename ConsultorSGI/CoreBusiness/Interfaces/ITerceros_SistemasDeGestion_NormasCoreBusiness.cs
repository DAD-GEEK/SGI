using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface ITerceros_SistemasDeGestion_NormasCoreBusiness : ICRUDGenerico<Terceros_SistemasDeGestion_Normas>, IDisposable
    {
        Task<string> GuardarRegistroAsync(Terceros_SistemasDeGestion_Normas model);
    }
}
