using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class PlantillasListasDeVerificacionDetalleCoreBusiness : CRUDGenerico<PlantillasListasDeVerificacionDetalle>, IPlantillasListasDeVerificacionDetalleCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private INumeralesCoreBusiness _iNumeralesCoreBusiness;
        private IPlantillasListasDeVerificacion_NumeralesCoreBusiness _iPlantillasListasDeVerificacion_NumeralesCoreBusiness;
        private INormasCoreBusiness _iNormasCoreBusiness;
        private IProcesosCoreBusiness _iProcesosCoreBusiness;

        public PlantillasListasDeVerificacionDetalleCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iNumeralesCoreBusiness = new NumeralesCoreBusiness();
            this._iPlantillasListasDeVerificacion_NumeralesCoreBusiness = new PlantillasListasDeVerificacion_NumeralesCoreBusiness();
            this._iNormasCoreBusiness = new NormasCoreBusiness();
        }

        public async Task<List<PlantillasListasDeVerificacionDetalleDTO>> GetAllPlantillasListasDeVerificacionDetalleAsync(int procesoID)
        {
            try
            {
                var listaPlantillasDetalle = await this.FindWhereAsync(x => x.IntProcesoID == procesoID);
                var listaNumerales = await _iNumeralesCoreBusiness.ObtenerTodosLosNumeralesAsync();
                listaNumerales = listaNumerales.Where(x => x.BitActivo == true).ToList();

                List<PlantillasListasDeVerificacionDetalleDTO> listasDeVerificacionDetalleDTOs = new List<PlantillasListasDeVerificacionDetalleDTO>();

                foreach (var item in listaPlantillasDetalle)
                {
                    var listaNumeralesPorDetalle = listaNumerales.Where(x => item.PlantillasListasDeVerificacion_Numerales.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();

                    PlantillasListasDeVerificacionDetalleDTO plantillasListasDeVerificacionDetalleDTO = new PlantillasListasDeVerificacionDetalleDTO();

                    plantillasListasDeVerificacionDetalleDTO.NormasDTO = await _iNumeralesCoreBusiness.ObtenerNumeralesPorNormasDTOAsync(listaNumeralesPorDetalle);
                    plantillasListasDeVerificacionDetalleDTO.IntPlantillaDetalleID = item.IntPlantillaDetalleID;
                    plantillasListasDeVerificacionDetalleDTO.IntProcesoID = item.IntProcesoID;
                    plantillasListasDeVerificacionDetalleDTO.StrTitulo = item.StrTitulo;
                    plantillasListasDeVerificacionDetalleDTO.IntOrden = item.IntOrden;
                    plantillasListasDeVerificacionDetalleDTO.BitEstado = item.BitEstado;

                    listasDeVerificacionDetalleDTOs.Add(plantillasListasDeVerificacionDetalleDTO);
                }

                return listasDeVerificacionDetalleDTOs;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PlantillasListasDeVerificacion_NumeralesDTO>> ObtenerNumeralesPorPlantillaDetalleDTOAsync(int detalleID)
        {
            try
            {
                return await _iPlantillasListasDeVerificacion_NumeralesCoreBusiness.ObtenerNumeralesPorPlantillaDetalleDTOAsync(detalleID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> ObtenerTodasLasNormasAsync(int normaID)
        {
            try
            {
                var norma = normaID.ToString();
                return await _iNormasCoreBusiness.SelectListAsync(norma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> ObtenerNumeralesPorNormaAsync(int normaID)
        {
            try
            {
                return await _iNumeralesCoreBusiness.DropDownNumeralesPorNormaAsync(normaID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarPlantillaDetalleNumeralAsync(PlantillasListasDeVerificacion_Numerales modelo)
        {
            try
            {
                return await _iPlantillasListasDeVerificacion_NumeralesCoreBusiness.SaveAllAsync(modelo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> SaveEntityAsync(PlantillasListasDeVerificacionDetalle entity)
        {
            try
            {
                await base.SaveEntityAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateAsync(PlantillasListasDeVerificacionDetalle entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntProcesoID == entity.IntProcesoID);

                var listaRegistrosDuplicados = listaRegistros.Where(x => x.StrTitulo.Trim().ToLower() == entity.StrTitulo.Trim().ToLower());

                if (listaRegistrosDuplicados.Count() > 0)
                    return String.Format(RecursoPlantillasListasDeVerificacion.msnRegistroYaExiste, entity.StrTitulo);

                entity.IntProcesoID = entity.IntProcesoID;
                entity.StrTitulo = entity.StrTitulo;
                entity.IntOrden = listaRegistros.Max(x => x.IntOrden) + 1;

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateRangeAsync(List<PlantillasListasDeVerificacionDetalle> listEntities)
        {
            try
            {
                if (listEntities.Count() == 0)
                    return string.Empty;

                await base.CreateRangeAsync(listEntities);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(PlantillasListasDeVerificacionDetalle entity)
        {
            try
            {
                await base.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteRangeAsync(IEnumerable<PlantillasListasDeVerificacionDetalle> entity)
        {
            try
            {
                await base.DeleteRangeAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<bool> ExistAsync(Expression<Func<PlantillasListasDeVerificacionDetalle, bool>> match)
        {
            try
            {
                return await base.ExistAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<PlantillasListasDeVerificacionDetalle> FindAsync(Expression<Func<PlantillasListasDeVerificacionDetalle, bool>> match)
        {
            try
            {
                return await base.FindAsync(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<PlantillasListasDeVerificacionDetalle>> GetAllAsync()
        {
            try
            {
                return await base.GetAllAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(PlantillasListasDeVerificacionDetalle entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.StrTitulo.Trim().ToLower() == entity.StrTitulo.Trim().ToLower() && x.IntPlantillaDetalleID != entity.IntPlantillaDetalleID && x.IntProcesoID == entity.IntProcesoID);

                if (listaRegistros.Count() > 0)
                    return String.Format(RecursoPlantillasListasDeVerificacion.msnRegistroYaExiste, entity.StrTitulo);

                PlantillasListasDeVerificacionDetalle PlantillasListasDeVerificacionDetalle = await this.FindAsync(x => x.IntPlantillaDetalleID == entity.IntPlantillaDetalleID);

                PlantillasListasDeVerificacionDetalle.StrTitulo = entity.StrTitulo;
                PlantillasListasDeVerificacionDetalle.BitEstado = entity.BitEstado;

                await base.UpdateAsync(PlantillasListasDeVerificacionDetalle);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(PlantillasListasDeVerificacionDetalle model, int auditoriaID = 0)
        {
            try
            {
                string respuesta = string.Empty;

                if (model.IntPlantillaDetalleID != 0)
                {
                    respuesta = await this.UpdateAsync(model);

                    if (!string.IsNullOrEmpty(respuesta))
                        return respuesta;

                    if (auditoriaID != 0)
                    {
                        IListasDeVerificacionCoreBusiness _iListasDeVerificacionCoreBusiness = new ListasDeVerificacionCoreBusiness();
                        IAuditoriasCoreBusiness _iAuditoriaCoreBusiness = new AuditoriasCoreBusiness();

                        var auditoria = await _iAuditoriaCoreBusiness.FindAsync(x => x.IntAuditoriaID == auditoriaID);

                        if (auditoria != null)
                        {
                            List<int> listaAuditoriaDetalleID = new List<int>();

                            auditoria.AuditoriasDetalle.ToList().ForEach(item => listaAuditoriaDetalleID.Add(item.IntAuditoriaDetalleID));

                            var listaDeVerificacion = await _iListasDeVerificacionCoreBusiness.FindWhereAsync(x => x.IntPlantillaDetalleID == model.IntPlantillaDetalleID && listaAuditoriaDetalleID.Any(y => y == x.IntAuditoriaDetalleID));

                            foreach (var item in listaDeVerificacion)
                            {
                                item.StrTitulo = model.StrTitulo;
                                await _iListasDeVerificacionCoreBusiness.SaveAllAsync(item);
                            }
                        }
                    }
                }
                else
                    respuesta = await this.CreateAsync(model);

                return respuesta;
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> SelectListNumeralesAsync()
        {
            try
            {
                return await _iNumeralesCoreBusiness.SelectListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> MultiSelectListNumeralesAsync(PlantillasListasDeVerificacionDetalle plantillaDetalle)
        {
            try
            {
                var listaNumerales = await _iNumeralesCoreBusiness.GetAllAsync();

                var entidad = listaNumerales.Select(x => new SelectListItem()
                {
                    Value = x.IntNumeralID.ToString(),
                    Text = $"{x.StrCodigo} - {x.StrDescripcion}"
                }).ToList();

                var listaRegistrosText = new List<string>();

                if (plantillaDetalle != null)
                {
                    foreach (var item in plantillaDetalle.PlantillasListasDeVerificacion_Numerales.ToList())
                        listaRegistrosText.Add(item.IntNumeralID.ToString());
                }

                return new MultiSelectList(entidad, "Value", "Text", listaRegistrosText);
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
                this._iProcesosCoreBusiness = new ProcesosCoreBusiness();

                var listaNumerales = await _iNumeralesCoreBusiness.GetAllAsync();
               
                var listaNumeralesPorPlantilla = await _iPlantillasListasDeVerificacion_NumeralesCoreBusiness.FindWhereAsync(x => x.IntPlantillaDetalleID == datosGenericos.RegistroID);

                var listaNormasDTO = await _iNormasCoreBusiness.ObtenerNormasDTOAsync(listaNumerales);
              
                listaNormasDTO.ForEach(normas =>
                {
                    normas.NumeralesDTO.ForEach(numeral => {

                        bool existeNumeral = listaNumeralesPorPlantilla.Exists(x => x.IntNumeralID == numeral.IntNumeralID);

                        if (existeNumeral)
                            numeral.BitSeleccionado = true;
                    });
                });

                return listaNormasDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> CambiarOrdenamientoPlantillasAsync(List<PlantillasListasDeVerificacionDetalle> listaPlantillasOrdenadas)
        {
            try
            {
                foreach (var item in listaPlantillasOrdenadas)
                {
                    var plantillaModelo = await this.FindAsync(x => x.IntPlantillaDetalleID == item.IntPlantillaDetalleID);

                    if (plantillaModelo is null)
                        break;

                    plantillaModelo.IntOrden = item.IntOrden;
                    await base.UpdateAsync(plantillaModelo);
                }           

                return string.Empty;
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

        ~PlantillasListasDeVerificacionDetalleCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iNumeralesCoreBusiness != null)
                {
                    _iNumeralesCoreBusiness.Dispose();
                    _iNumeralesCoreBusiness = null;
                }

                if (_iPlantillasListasDeVerificacion_NumeralesCoreBusiness != null)
                {
                    _iPlantillasListasDeVerificacion_NumeralesCoreBusiness.Dispose();
                    _iPlantillasListasDeVerificacion_NumeralesCoreBusiness = null;
                }

                if (_iNormasCoreBusiness != null)
                {
                    _iNormasCoreBusiness.Dispose();
                    _iNormasCoreBusiness = null;
                }

                if (_iProcesosCoreBusiness != null)
                {
                    _iProcesosCoreBusiness.Dispose();
                    _iProcesosCoreBusiness = null;
                }

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
