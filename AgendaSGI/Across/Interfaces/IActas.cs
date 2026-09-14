using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Across.Interfaces
{
    public interface IActas : ICRUDGenerico<Actas>
    {
        Task<List<ActividadesActa>> ObtenerUltimosCompromisosAsync(int actaID);
    }
}
