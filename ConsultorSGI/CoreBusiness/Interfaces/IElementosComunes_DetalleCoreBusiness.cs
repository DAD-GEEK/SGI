using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IElementosComunes_DetalleCoreBusiness : ICRUDGenerico<ElementosComunes_Detalle>, IDisposable
    {
        Task<List<NumeralesDTO>> ObtenerNumeralesPorNormasAsync(int requisitosSoporteID);
        Task<ElementosComunes_DetalleDTO> ObtenerElementosComunes_DetalleDTO(int elementoComunDetalleID);
        Task<ElementosComunesDTO> ObtenerElementoComunDTOPorIdAsync(int elementoComunID);
        Task<string> SaveAllAsync(ElementosComunes_Detalle model);
        Task<List<NormasDTO>> ObtenerNumeralesPorNormasDTODeElementosComunesAsync(List<Numerales> listaNumerales, List<Auditorias_Normas> listaNormasPorAuditoria, List<ElementosComunes> elementosComunesDeAuditoria);
    }
}
