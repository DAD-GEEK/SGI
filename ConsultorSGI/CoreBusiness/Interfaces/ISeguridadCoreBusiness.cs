using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface ISeguridadCoreBusiness: IDisposable
    {
        Task<string> ValidarInicioSesion(string emailUsuario);
        Task<bool> UsuarioAsignadoATercero(string email);
        Task<string> ExisteUsuario(string email);
        Task<string> ValidacionesConfirmacionLoginExterno(string email);
        Task<TercerosUsuarios> ObtenerUsuarioTerceroAsync(string email);
        Task<List<TercerosUsuariosDTO>> ObtenerListaUsuariosByTercerosDTO();
        Task BloquearAspNetUser(AspNetUsers modelo);
        Task DesBloquearAspNetUser(AspNetUsers modelo);
        Task<TercerosUsuarios> GetTerceroUsuarioByEmail(string email);
        Task<List<TreeModulosDTO>> GetTreePermisosByRoles(string rolID);
        Task<string> GuardarPermisosPorRolAsync(List<string> listaPermisos, string rolID);
        Task<DatosUsuarioEnSesionDTO> GetMenuDinamicoAsync(string usuarioID, IList<string> rolesPorUsuario);
        Task<bool> ValidarSiEsGestionIntegralAsync(int terceroID);
    }
}
