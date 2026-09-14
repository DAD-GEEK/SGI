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
    public class DocumentosDiagnosticoPasos_NivelesCoreBusiness : CRUDGenerico<DocumentosDiagnosticoPasos_Niveles>, IDocumentosDiagnosticoPasos_NivelesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public DocumentosDiagnosticoPasos_NivelesCoreBusiness() : base(new gestioni_consultorNetEntities())
        {

        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(DocumentosDiagnosticoPasos_Niveles entity)
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

        private new async Task<string> CreateAsync(DocumentosDiagnosticoPasos_Niveles entity)
        {
            try
            {
                entity.StrRegistroID = Guid.NewGuid().ToString();

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(DocumentosDiagnosticoPasos_Niveles entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<DocumentosDiagnosticoPasos_Niveles> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<DocumentosDiagnosticoPasos_Niveles, bool>> match)
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

        public new async Task<DocumentosDiagnosticoPasos_Niveles> FindAsync(Expression<Func<DocumentosDiagnosticoPasos_Niveles, bool>> match)
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

        public new List<DocumentosDiagnosticoPasos_Niveles> GetAll()
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

        public new async Task<List<DocumentosDiagnosticoPasos_Niveles>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(DocumentosDiagnosticoPasos_Niveles entity)
        {
            try
            {
                var DocumentosDiagnosticoPasos_Niveles = await this.FindAsync(x => x.StrRegistroID == entity.StrRegistroID);

                DocumentosDiagnosticoPasos_Niveles.StrPasoID = entity.StrPasoID;
                DocumentosDiagnosticoPasos_Niveles.StrNivelID = entity.StrNivelID;

                await base.UpdateAsync(DocumentosDiagnosticoPasos_Niveles);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarAsync(DocumentosDiagnosticoPasos_Niveles model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.StrRegistroID))
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ActualizarNivelesEnDocumentoDiagnosticoPasosAsync(string pasoID, List<DocumentosDiagnosticoPasos_Niveles> listaNiveles)
        {
            try
            {
                var listaNivelesPorPaso = await this.FindWhereAsync(x => x.StrPasoID == pasoID);

                var listaNivelesParaCrear = listaNiveles.Where(x => !listaNivelesPorPaso.Any(y => y.StrNivelID == x.StrNivelID)).ToList();
                    
                var listaNivelesParaEliminar = listaNivelesPorPaso.Where(x => !listaNiveles.Any(y => y.StrNivelID == x.StrNivelID)).ToList();

                foreach (var item in listaNivelesParaCrear)
                {
                    item.StrPasoID = pasoID;
                    await this.GuardarAsync(item);
                }

                foreach (var item in listaNivelesParaEliminar)
                    await this.DeleteAsync(item);

                return string.Empty;

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

        ~DocumentosDiagnosticoPasos_NivelesCoreBusiness()
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
