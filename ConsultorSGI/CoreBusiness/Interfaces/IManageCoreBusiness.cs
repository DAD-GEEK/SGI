using Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface IManageCoreBusiness
    {
        Task<string> EliminarImagenUsuarioAsync(string usuarioID, bool isAvatar);
        Task<string> GuardarImagenAsyn(HttpFileCollectionBase imagenUsuario, FormCollection collection);
        string ObtenerRutaDeImagenUsuarioAvatar(string nombreImagen);
        string ObtenerRutaDeImagenUsuarioFirma(string nombreImagen);
        long ObtenerTamanoDeArchivo(string nombreArchivo);
        Task<string> UpdateDatosDePerfilDeUsuarioAsync(AspNetUsers model);

    }
}
