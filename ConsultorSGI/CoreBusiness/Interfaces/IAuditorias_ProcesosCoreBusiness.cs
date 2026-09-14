using DataAccess.Interfaces;
using Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IAuditorias_ProcesosCoreBusiness : ICRUDGenerico<Auditorias_Procesos>, IDisposable
    {
        Task<SelectList> SelectListAsync(int auditoriaID, string valueSelected = null);
    }
}
