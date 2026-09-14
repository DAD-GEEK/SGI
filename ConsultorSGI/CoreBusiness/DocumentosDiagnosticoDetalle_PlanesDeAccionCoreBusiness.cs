using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
using Models;
using Models.DTO;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class DocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness : CRUDGenerico<DocumentosDiagnosticoDetalle_PlanesDeAccion>, IDocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public DocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness() : base(new gestioni_consultorNetEntities())
        {

        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(DocumentosDiagnosticoDetalle_PlanesDeAccion entity)
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

        private new async Task<string> CreateAsync(DocumentosDiagnosticoDetalle_PlanesDeAccion entity)
        {
            try
            {
                entity.BitAplica = true;
                entity.BitSeEvidencia = false;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateRangeAsync(List<DocumentosDiagnosticoDetalle_PlanesDeAccion> entity)
        {
            try
            {
                await base.CreateRangeAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(DocumentosDiagnosticoDetalle_PlanesDeAccion entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<DocumentosDiagnosticoDetalle_PlanesDeAccion> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<DocumentosDiagnosticoDetalle_PlanesDeAccion, bool>> match)
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

        public new async Task<DocumentosDiagnosticoDetalle_PlanesDeAccion> FindAsync(Expression<Func<DocumentosDiagnosticoDetalle_PlanesDeAccion, bool>> match)
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

        public new List<DocumentosDiagnosticoDetalle_PlanesDeAccion> GetAll()
        {
            try
            {
                return base.GetAll().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<DocumentosDiagnosticoDetalle_PlanesDeAccion>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(DocumentosDiagnosticoDetalle_PlanesDeAccion entity)
        {
            try
            {
                var DocumentosDiagnosticoDetalle_PlanesDeAccion = await this.FindAsync(x => x.StrCalificacionID == entity.StrCalificacionID);

                if (DocumentosDiagnosticoDetalle_PlanesDeAccion.DocumentosDiagnosticoDetalle.DocumentosDiagnostico.BitFinalizado)
                    return RecursoDocumentosDiagnostico.msnDocumentoFinalizado;

                DocumentosDiagnosticoDetalle_PlanesDeAccion.BitAplica = entity.BitAplica;
                DocumentosDiagnosticoDetalle_PlanesDeAccion.BitSeEvidencia = entity.BitAplica == true ? entity.BitSeEvidencia : false;
                DocumentosDiagnosticoDetalle_PlanesDeAccion.StrComoSeEvidencia = entity.StrComoSeEvidencia;
                DocumentosDiagnosticoDetalle_PlanesDeAccion.StrPlanDeAccion = entity.StrPlanDeAccion;
                DocumentosDiagnosticoDetalle_PlanesDeAccion.StrResponsable = entity.StrResponsable;
                DocumentosDiagnosticoDetalle_PlanesDeAccion.DatFecha = entity.DatFecha;
                DocumentosDiagnosticoDetalle_PlanesDeAccion.DatFechaCalificacion = Common.ObtenerFechaActualExacta();

                await base.UpdateAsync(DocumentosDiagnosticoDetalle_PlanesDeAccion);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> GuardarAsync(DocumentosDiagnosticoDetalle_PlanesDeAccion model)
        {
            try
            {
                return await this.UpdateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DocumentosDiagnosticoDetalle_PlanesDeAccionCoreBusiness()
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
