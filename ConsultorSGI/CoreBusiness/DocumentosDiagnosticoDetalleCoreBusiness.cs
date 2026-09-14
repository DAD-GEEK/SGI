using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class DocumentosDiagnosticoDetalleCoreBusiness : CRUDGenerico<DocumentosDiagnosticoDetalle>, IDocumentosDiagnosticoDetalleCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public DocumentosDiagnosticoDetalleCoreBusiness() : base(new gestioni_consultorNetEntities())
        {

        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(DocumentosDiagnosticoDetalle entity)
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

        private new async Task<string> CreateAsync(DocumentosDiagnosticoDetalle entity)
        {
            try
            {
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> CreateRangeAsync(List<DocumentosDiagnosticoDetalle> entity)
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

        public new async Task DeleteAsync(DocumentosDiagnosticoDetalle entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<DocumentosDiagnosticoDetalle> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<DocumentosDiagnosticoDetalle, bool>> match)
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

        public new async Task<DocumentosDiagnosticoDetalle> FindAsync(Expression<Func<DocumentosDiagnosticoDetalle, bool>> match)
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

        public new List<DocumentosDiagnosticoDetalle> GetAll()
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

        public new async Task<List<DocumentosDiagnosticoDetalle>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(DocumentosDiagnosticoDetalle entity)
        {
            try
            {
                var DocumentosDiagnosticoDetalle = await this.FindAsync(x => x.StrDocumentoID == entity.StrDocumentoID);

                DocumentosDiagnosticoDetalle.StrDocumentoID = entity.StrDocumentoID;
                DocumentosDiagnosticoDetalle.StrPasoID = entity.StrPasoID;

                await base.UpdateAsync(DocumentosDiagnosticoDetalle);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> GuardarAsync(DocumentosDiagnosticoDetalle model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.StrPasoID))
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
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

        ~DocumentosDiagnosticoDetalleCoreBusiness()
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
