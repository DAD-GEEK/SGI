using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class FuncionesGlobalesCoreBusiness : IFuncionesGlobalesCoreBusiness
    {
        public FuncionesGlobalesCoreBusiness() { }

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        IProcesosCoreBusiness _iProcesosCoreBusiness => new ProcesosCoreBusiness();
        IPlantillasListasDeVerificacionDetalleCoreBusiness _iPlantillasListasDeVerificacionDetalleCoreBusiness => new PlantillasListasDeVerificacionDetalleCoreBusiness();
        IListasDeVerificacionCoreBusiness _iListasDeVerificacionCoreBusiness => new ListasDeVerificacionCoreBusiness();

        public ExportarPDFOptionsDTO VistaExportarPDFOptions(string vistaExportacion)
        {
            try
            {
                ExportarPDFOptionsDTO opcionesDTO = new ExportarPDFOptionsDTO();
                var urlAplicacion = Common.GetUrlAplicacion();

                if (vistaExportacion.ToLower() == enumExportacionPDF.PlanDeAuditoria.ToString().ToLower())
                {
                    opcionesDTO.Titulo = $"Exportar Plan de Auditoría a PDF";
                    opcionesDTO.UrlDescarga = $"{urlAplicacion}/Auditorias/DescargarInformePlanAuditoriaPDFAsync";
                }

                return opcionesDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<NormasDTO>> ObtenerNumeralesParaSeleccionarAsync(NumeralesGenericosDTO datosGenericos)
        {
            try
            {
                List<NormasDTO> listaNormasDTO = new List<NormasDTO>();

                if (datosGenericos.Controlador == "Procesos")
                    listaNormasDTO = await _iProcesosCoreBusiness.ObtenerNumeralesParaSeleccionarAsync(datosGenericos);

                if (datosGenericos.Controlador == "PlantillasListasDeVerificacionDetalle")
                    listaNormasDTO = await _iPlantillasListasDeVerificacionDetalleCoreBusiness.ObtenerNumeralesParaSeleccionarAsync(datosGenericos);

                if (datosGenericos.Controlador == "ListasDeVerificacion")
                    listaNormasDTO = await _iListasDeVerificacionCoreBusiness.ObtenerNumeralesParaSeleccionarAsync(datosGenericos);

                return listaNormasDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FuncionesGlobalesCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iProcesosCoreBusiness != null)
                    _iProcesosCoreBusiness.Dispose();

                if (_iPlantillasListasDeVerificacionDetalleCoreBusiness != null)
                    _iPlantillasListasDeVerificacionDetalleCoreBusiness.Dispose();

                if (_iListasDeVerificacionCoreBusiness != null)
                    _iListasDeVerificacionCoreBusiness.Dispose();
            }

            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }


        #endregion Dispose
    }
}
