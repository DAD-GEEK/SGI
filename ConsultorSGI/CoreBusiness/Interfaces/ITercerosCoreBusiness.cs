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
    public interface ITercerosCoreBusiness : ICRUDGenerico<Terceros>, IDisposable
    {
        Task<string> SaveAllAsync(Terceros model);
        Task<MultiSelectList> SelectListAspNetRolesAsync(string valueSelected = null, List<string> groupSelected = null, bool bitActivo = true);
        Task<SelectList> SelectListCiudadesAsync(string valueSelected = null);
        Task<MultiSelectList> SelectListAspNetRolesPorTerceroAsync(int terceroID, bool bitActivo = true);
        Task<SelectList> SelectListAsync(string valueSelected = null);
        Task<SelectList> SelectListNormasAsync(string valueSelected = null);
        Task<List<AspNetTerceroRoles>> ObtenerPermisosPorTerceroAsync(int terceroID);
        Task<List<Terceros_Normas>> ObtenerNormasPorTerceroAsync(int terceroID);
        Task<string> AgregarNormaPorTerceroAsync(Terceros_Normas modelo);
        Task<string> EliminarNormaPorTerceroAsync(int registroID);
        Task<string> GuardarImagenAsyn(HttpFileCollectionBase imagenTercero, FormCollection collection);
        Task<string> EliminarImagenAsync(int terceroID);
        Task<Terceros> ObtenerInformacionDeTerceroEnSesionAsync();
        Task<MultiSelectList> MultiDropDownListNormasAsync(List<Normas> listaNormas = null);
        Task<SistemasDeGestionDTO> ObtenerTerceros_SistemasDeGestionPorIDAsync(int registroID = 0);
        Task<string> GuardarNivelDeTerceroAsync(int terceroID, string nivelID);
        Task<TercerosDTO> ObtenerTerceroDTOAsync(int terceroID);
        Task<string> GuardarInformacionTerceroDocumentoDiagnosticoAsync(Terceros tercero);
        Task<string> AgregarRolPorTerceroAsync(AspNetTerceroRoles modelo);
        Task<string> EliminarTerceroAsync(int terceroID);
    }
}
