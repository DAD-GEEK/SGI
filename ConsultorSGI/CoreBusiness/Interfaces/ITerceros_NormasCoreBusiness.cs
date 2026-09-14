using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface ITerceros_NormasCoreBusiness : ICRUDGenerico<Terceros_Normas>, IDisposable
    {
        Task<List<Terceros_Normas>> ObtenerNormasPorTerceroAsync(int terceroID);
        Task<string> SaveAllAsync(Terceros_Normas model);
    }
}
