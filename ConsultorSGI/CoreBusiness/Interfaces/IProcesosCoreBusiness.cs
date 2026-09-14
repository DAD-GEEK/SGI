using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IProcesosCoreBusiness : ICRUDGenerico<Procesos>, IDisposable
    {
        Task<string> SaveAllAsync(Procesos model);
        Task<MultiSelectList> MultiSelectListNumeralesPorTerceroAsync(int terceroID, Procesos proceso = null);
        Task<List<Procesos_NumeralesDTO>> ObtenerNumeralesPorProcesoDTOAsync(int procesoID);
        Task<SelectList> ObtenerTodasLasNormasAsync(int normaID);
        Task<string> EliminarProcesoAsync(Procesos modelo);
        Task<List<Procesos>> ObtenerProcesosGlobalesAsync();
        Task<string> AgregarProcesoGlobalAlTerceroAsync(Procesos modelo);
        Task<SelectList> ObtenerNumeralesPorNormaAsync(int normaID);
        Task<List<NormasDTO>> ObtenerNumeralesParaSeleccionarAsync(NumeralesGenericosDTO datosGenericos);
        Task<string> CargarDatosDeProcesoAProcesoAsync(int procesoGenericoID, int procesoActualID);
        SelectList SelectListMacroProcesosAsync(string valueSelected = null);
        Task<List<Procesos>> ObtenerProcesosPorTercerClienteIDAsync(string filtro, int terceroClienteID);
        Task<MultiSelectList> MultiSelectListPorTerceroAsync(Terceros_Clientes terceroClientes, Auditorias auditoria = null);
        Task<int> ObtenerTerceroClienteGenericoIDAsync();
    }
}
