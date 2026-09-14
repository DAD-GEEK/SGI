using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class Auditorias_ProcesosCoreBusiness : CRUDGenerico<Auditorias_Procesos>, IAuditorias_ProcesosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private INormasCoreBusiness _iNormasCoreBusiness;

        public Auditorias_ProcesosCoreBusiness() : base(new gestioni_consultorNetEntities()) 
        {
            this._iNormasCoreBusiness = new NormasCoreBusiness();
        }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(Auditorias_Procesos entity)
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

        public new async Task<string> CreateAsync(Auditorias_Procesos entity)
        {
            try
            {
                entity.IntAuditoriaID = entity.IntAuditoriaID;
                entity.IntProcesoID = entity.IntProcesoID;              

                await base.CreateAsync(entity);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Auditorias_Procesos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Auditorias_Procesos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Auditorias_Procesos, bool>> match)
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

        public new async Task<Auditorias_Procesos> FindAsync(Expression<Func<Auditorias_Procesos, bool>> match)
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

        public new async Task<List<Auditorias_Procesos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Auditorias_Procesos entity)
        {
            try
            {
                Auditorias_Procesos Auditorias_Procesos = await this.FindAsync(x => x.IntID == entity.IntID);

                Auditorias_Procesos.IntAuditoriaID = entity.IntAuditoriaID;
                Auditorias_Procesos.IntProcesoID = entity.IntProcesoID;
               
                await base.UpdateAsync(Auditorias_Procesos);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(Auditorias_Procesos model)
        {
            try
            {
                if (model.IntAuditoriaID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Select List
        public async Task<SelectList> SelectListAsync(int auditoriaID, string valueSelected = null)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntAuditoriaID == auditoriaID);

                var entidad = listaRegistros.Select(x => new SelectListItem()
                {
                    Value = x.IntProcesoID.ToString(),
                    Text = $"{x.Procesos.StrCodigo} - {x.Procesos.StrDescripcion}"
                }).ToList();

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "Value", "Text");

                return new SelectList(entidad, "Value", "Text", valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<MultiSelectList> MultiSelectListNormasAsync(Auditorias auditoria)
        {
            try
            {
                return await _iNormasCoreBusiness.MultiSelectListAsync(auditoria);
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

        ~Auditorias_ProcesosCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iNormasCoreBusiness != null)
                {
                    _iNormasCoreBusiness.Dispose();
                    _iNormasCoreBusiness = null;
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
