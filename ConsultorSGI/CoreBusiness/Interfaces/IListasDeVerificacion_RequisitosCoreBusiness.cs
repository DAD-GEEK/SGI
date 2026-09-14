using DataAccess.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IListasDeVerificacion_RequisitosCoreBusiness : ICRUDGenerico<ListasDeVerificacion_Requisitos>, IDisposable
    {
        Task<string> GuardarRequisitosSeleccionadosEnListaDeVerificacionAsync(int listaDeVerificacionID, List<ListasDeVerificacion_Requisitos> listaRequisitosSeleccionados);
    }
}
