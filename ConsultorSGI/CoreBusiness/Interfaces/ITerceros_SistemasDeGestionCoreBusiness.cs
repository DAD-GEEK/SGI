using DataAccess.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness.Interfaces
{
    public interface ITerceros_SistemasDeGestionCoreBusiness : ICRUDGenerico<Terceros_SistemasDeGestion>, IDisposable
    {
        Task<string> AplicarNormaAsync(Terceros_SistemasDeGestion_Normas modelo);
        Task<SelectList> DropDownListNivelesAsync(string valueSelected = null);
        Task<SelectList> DropDownListSistemasDeGestionAsync(string valueSelected = null);
        Task<string> EliminarRegistroAsync(int registroID);
        Task<string> GuardarRegistroAsync(Terceros_SistemasDeGestion model, string nivelID);
        Task<List<SistemasDeGestionDTO>> ObtenerSistemasDeGestionPorTerceroAsync(int terceroID);
        Task<SistemasDeGestionDTO> ObtenerTerceros_SistemasDeGestionPorIDAsync(int registroID = 0);
    }
}
