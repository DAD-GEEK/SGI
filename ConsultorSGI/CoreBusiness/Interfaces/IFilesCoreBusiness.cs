using Models.DTO;
using System.Threading.Tasks;
using System.Web;

namespace CoreBusiness.Interfaces
{
    public interface IFilesCoreBusiness
    {
        string GuardarArchivo(HttpFileCollectionBase files, ParamFilesDTO parametros);
        long ObtenerTamanoDeArchivo(string rutaArchivo);
        string EliminarArchivo(string rutaArchivo);
        string ConvertirRutaEnUrl(string rutaArchivo);
        string ObtenerRutaDeImagenEmpleado(string nombreImagen);

    }
}
