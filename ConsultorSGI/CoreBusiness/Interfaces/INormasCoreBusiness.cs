using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface INormasCoreBusiness : ICRUDGenerico<Normas>, IDisposable
    {
        Task<List<NormasDTO>> ContadorDeNormasAsync(List<NormasDTO> listaNormasDTO);
        Task<MultiSelectList> MultiSelectListAsync(Auditorias valueSelected);
        Task<MultiSelectList> MultiSelectListTodasLasNormasAsync(List<Normas> valueSelected);
        Task<List<Normas>> ObtenerNormasAsync();
        Task<List<NormasDTO>> ObtenerNormasDTOAsync(List<Numerales> numeralesSeleccionados = null);
        Task<List<Normas>> ObtenerNormasPorAuditoriaAsync(int auditoriaID);
        Task<List<Normas>> ObtenerNormasPorAuditoriaYNumeralesAsync(int auditoriaID, List<Numerales> listaNumerales);
        Task<List<Normas>> ObtenerNormasPorTerceroAsync(int terceroID);
        Task<string> SaveAllAsync(Normas model);
        Task<SelectList> SelectListAsync(string valueSelected = null);
        SelectList SelectListCriteriosAsync(string valueSelected = null);
    }
}
