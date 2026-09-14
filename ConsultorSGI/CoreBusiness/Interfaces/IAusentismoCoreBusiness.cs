using DataAccess.Interfaces;
using Models;
using Models.DTO;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IAusentismoCoreBusiness : ICRUDGenerico<Ausentismo>, IDisposable
    {
        new Task<string> SaveEntityAsync(Ausentismo entity);
        Task<string> SaveAllAsync(Ausentismo model);
        Task<Empleados> ObtenerEmpleadoConImageUrlAsync(int empleadoID);
        Task<Diagnosticos> BuscarDiagnosticoAsync(string valor);
        Task<SelectList> SelectListEmpleadosAsync(string valueSelected = null);
        Task<SelectList> SelectListTipoEvento(string valueSelected = null);
        Task<SelectList> SelectListDiagnosticos(string valueSelected = null);
        Task<List<AusentismoAgrupadoMensualmentePorAnioDTO>> GetAusentismoAgrupadoMensualmentePorAñoAsync(short anio);
        Task<AusentismoAgrupadoPorEmpleadoDTO> GetAusentismoAgrupadoMensualmentePorEmpleadoAsync(short anio, byte periodo);
        Task<AusentismoAgrupadoPorAreaDTO> GetAusentismoAgrupadoPorAreaAsync(short anio);
        Task<AusentismoAgrupadoPorEmpleadoDTO> GetAusentismoAgrupadoPorArea_EmpleadoAsync(short anio, int areaID);
        Task<AusentismoAgrupadoPorProcesoDTO> GetAusentismoAgrupadoPorProcesoAsync(short anio);
        Task<string> AgregarProrrogaAsync(AusentismoProrrogas modelo);
        Task<List<AusentismoProrrogas>> ObtenerProrrogasAsync(int ausentismoID);
        Task<string> DeleteProrrogaAsync(int prorrogaID);
        Task<List<AusentismoDTO>> ObtenerListaAusentismoDTOAsync(DateTime? fechaInicial = null, DateTime? fechaFinal = null);
        Task<AusentismoCostosDTO> ObtenerCostosDeAusentismoPorIDAsync(int ausentismoID);
        Task<List<ExcelErrorDTO>> CargarAusentismoDesdeExcelAsync(HttpFileCollectionBase archivoDeExcel, ParamFilesDTO paramFilesDTO);
    }
}
