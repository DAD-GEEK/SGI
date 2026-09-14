using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IFuncionesGlobalesCoreBusiness : IDisposable
    {
        Task<List<NormasDTO>> ObtenerNumeralesParaSeleccionarAsync(NumeralesGenericosDTO datosGenericos);
        ExportarPDFOptionsDTO VistaExportarPDFOptions(string vistaExportacion);
    }
}
