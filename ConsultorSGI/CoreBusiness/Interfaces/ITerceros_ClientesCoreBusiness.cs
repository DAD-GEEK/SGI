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
    public interface ITerceros_ClientesCoreBusiness : ICRUDGenerico<Terceros_Clientes>, IDisposable
    {
        int TotalRegistrosDataTable { get; }

        Task<SelectList> DropDownListCiudadesAsync(string valueSelected = null);
        Task<string> EliminarImagenAsync(Terceros_Clientes modelo, bool esEliminacion = false);
        Task<string> EliminarTerceroClienteAsync(int terceroClienteID);
        Task<List<TercerosClientesDTO>> GetPaginacionTercerosClientes(DatatableParamsDTO datatableParamsDTO, bool paginarInformacion = true);
        Task<string> GuardarImagenAsyn(HttpFileCollectionBase imagenTercero, FormCollection collection);
        Task<string> GuardarRegistroAsync(Terceros_Clientes model);
        Task<string> GuardarTerceroCliente_ClienteSegunTerceroAsync(Terceros tercero);
    }
}
