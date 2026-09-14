using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface INumeralesCoreBusiness : ICRUDGenerico<Numerales>, IDisposable
    {
        Task<MultiSelectList> MultiListNumeralesPorTerceroAsync(int terceroID, Procesos procesos = null);
        Task<MultiSelectList> MultiListNumeralesPorTercero_ProcesoAsync(int terceroID, Procesos procesos = null);
        Task<Normas> ObtenerNormaPorIdAsync(int normaID);
        Task<List<Numerales>> ObtenerNumeralesPorItemDeListaDeVerificacionAsync(ListasDeVerificacion listasDeVerificacion);
        Task<List<NormasDTO>> ObtenerNumeralesPorNormasDTOAsync(List<Numerales> listaNumerales);
        Task<List<Numerales>> ObtenerNumeralesPorNormasAsync(List<Normas> listaNormas);
        Task<List<Numerales>> ObtenerNumeralesPorProceso_NormasAsync(int auditoriaID, int procesoID);
        Task<string> SaveAllAsync(Numerales model, int normaID);
        Task<SelectList> SelectListAsync(string valueSelected = null);
        Task<SelectList> SelectListNormasAsync(string valorSeleccionado = null);
        Task<List<Numerales>> ObtenerNumeralesPorNormasAsync(int normaID);
        Task<SelectList> DropDownNumeralesPorNormaAsync(int normaID, string valorSeleccionado = null);
        Task<List<Numerales>> ObtenerNumeralesPorNormasDeAuditoriaAsync(Auditorias auditoriaModelo);
        Task<List<NumeralesDTO>> ObtenerTodosLosNumeralesDTOAsync();
        Task<List<Numerales>> ObtenerTodosLosNumeralesAsync();
    }
}
