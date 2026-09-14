using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IAuditoriasDetalleACCoreBusiness : ICRUDGenerico<AuditoriasDetalleAC>, IDisposable
    {
        Task<string> SaveAllAsync(AuditoriasDetalleAC model);
    }
}
