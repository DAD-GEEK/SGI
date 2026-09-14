using Models;
using Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Across.Interfaces
{
    public interface IAgenda : ICRUDGenerico<Agenda>
    {
        Task<List<TiemposAsesoriasDTO>> ObtenerTiemposDeAsesoriaAsync(Agenda modelo);
    }
}
