using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IEmpleadosCoreBusiness : ICRUDGenerico<Empleados>, IDisposable
    {
        Task<string> GuardarEmpleadoAsync(Empleados modelo);
        Task<SelectList> SelectListAsync(string valueSelected = null);
        Task<SelectList> SelectListCiudadesAsync(string valueSelected = null);
        Task<SelectList> SelectListTipoIdentificacionAsync(string valueSelected = null);
        Task<SelectList> SelectListEscolaridadesAsync(string valueSelected = null);
        Task<SelectList> SelectListCentrosDeTrabajoAsync(string valueSelected = null);
        Task<SelectList> SelectListEstadosCivilesAsync(string valueSelected = null);
        Task<SelectList> SelectListCargosAsync(string valueSelected = null);
        Task<SelectList> SelectListTurnosAsync(string valueSelected = null);
        Task<SelectList> SelectListFondosAsync(string tipoFondo, string valueSelected = null);
        Task<SelectList> SelectListEstratosAsync(string valueSelected = null);
        Task<SelectList> SelectListTiposContratoAsync(string valueSelected = null);
        SelectList SelectListGrupoSanguineo(string valueSelected = null);
        string GuardarArchivoEmpleado(HttpFileCollectionBase files, ParamFilesDTO parametros);
        long ObtenerTamanoDeArchivo(string nombreArchivo);
        string EliminarImagenEmpleado(string pathArchivo);
        string ObtenerRutaDeImagenEmpleado(string nombreImagen);
        Task<SelectList> SelectListAreasAsync(string valueSelected = null);
        Task<List<ExcelErrorDTO>> CargarEmpleadosPorArchivoDeExcelAsync(HttpFileCollectionBase archivoDeExcel, ParamFilesDTO paramFilesDTO);
    }
}
