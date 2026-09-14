using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IAusentismoProrrogasCoreBusiness : ICRUDGenerico<AusentismoProrrogas>, IDisposable
    {
        Task<string> SaveAllAsync(AusentismoProrrogas model);
    }
}
