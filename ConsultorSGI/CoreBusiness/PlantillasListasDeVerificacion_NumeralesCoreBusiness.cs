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

namespace CoreBusiness
{
    public class PlantillasListasDeVerificacion_NumeralesCoreBusiness : CRUDGenerico<PlantillasListasDeVerificacion_Numerales>, IPlantillasListasDeVerificacion_NumeralesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public PlantillasListasDeVerificacion_NumeralesCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(PlantillasListasDeVerificacion_Numerales entity)
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

        public new async Task<string> CreateAsync(PlantillasListasDeVerificacion_Numerales entity)
        {
            try
            {
                var procesoNumeral = await this.FindWhereAsync(x => x.IntPlantillaDetalleID == entity.IntPlantillaDetalleID && x.IntNumeralID == entity.IntNumeralID);

                if (procesoNumeral.Count() != 0)
                    return RecursoPlantillasListasDeVerificacion.msnNumeralDuplicado;

                entity.IntPlantillaDetalleID = entity.IntPlantillaDetalleID;
                entity.IntNumeralID = entity.IntNumeralID;

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateRangeAsync(List<PlantillasListasDeVerificacion_Numerales> listEntities)
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

        public new async Task DeleteAsync(PlantillasListasDeVerificacion_Numerales entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<PlantillasListasDeVerificacion_Numerales> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<PlantillasListasDeVerificacion_Numerales, bool>> match)
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

        public new async Task<PlantillasListasDeVerificacion_Numerales> FindAsync(Expression<Func<PlantillasListasDeVerificacion_Numerales, bool>> match)
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

        public new async Task<List<PlantillasListasDeVerificacion_Numerales>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(PlantillasListasDeVerificacion_Numerales entity)
        {
            try
            {
                PlantillasListasDeVerificacion_Numerales PlantillasListasDeVerificacion_Numerales = await this.FindAsync(x => x.IntPlantillaDetalleID == entity.IntPlantillaDetalleID);

                PlantillasListasDeVerificacion_Numerales.IntPlantillaDetalleID = entity.IntPlantillaDetalleID;
                PlantillasListasDeVerificacion_Numerales.IntNumeralID = entity.IntNumeralID;

                await base.UpdateAsync(PlantillasListasDeVerificacion_Numerales);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(PlantillasListasDeVerificacion_Numerales model)
        {
            try
            {
                var modelo = await this.FindAsync(x => x.IntPlantillaDetalleID == model.IntPlantillaDetalleID && x.IntNumeralID == model.IntNumeralID);

                if (modelo is null)
                    return await this.CreateAsync(model);

                await this.DeleteAsync(modelo);
                return string.Empty;
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
                var listaNumerales = await this.FindWhereAsync(x => x.IntPlantillaDetalleID == detalleID);

                var listaNumeralesDTO = listaNumerales.Select(x => new PlantillasListasDeVerificacion_NumeralesDTO()
                {
                    IntID = x.IntID,
                    IntNumeralID = x.IntNumeralID,
                    IntPlantillaDetalleID = x.IntPlantillaDetalleID,
                    CodigoNorma = x.Numerales.Normas.StrCodigo,
                    CodigoNumeral = x.Numerales.StrCodigo,
                    DescripcionNumeral = x.Numerales.StrDescripcion
                }).ToList();

                return listaNumeralesDTO;

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

        ~PlantillasListasDeVerificacion_NumeralesCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

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
