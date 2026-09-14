using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface ILogsExceptionCoreBusiness : ICRUDGenerico<LogsException>
    {
        Task<string> GenerarLogException(Exception ex);
    }
}
