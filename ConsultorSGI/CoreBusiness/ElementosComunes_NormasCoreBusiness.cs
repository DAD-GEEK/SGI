using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class ElementosComunes_NormasCoreBusiness : CRUDGenerico<ElementosComunes_Normas>, IElementosComunes_NormasCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private INormasCoreBusiness _iNormasCoreBusiness;
        public ElementosComunes_NormasCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iNormasCoreBusiness = new NormasCoreBusiness();
        }

        public new async Task<string> SaveEntityAsync(ElementosComunes_Normas entity)
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

        public new async Task<string> CreateAsync(ElementosComunes_Normas entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntNormaID == entity.IntNormaID && x.IntElementoComunID == entity.IntElementoComunID);
                if (listaRegistros.Count() != 0)
                    return RecursosRequisitosYSoportes.msnNormaDuplicada;

                entity.IntElementoComunID = entity.IntElementoComunID;
                entity.IntNormaID = entity.IntNormaID;
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(ElementosComunes_Normas entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<ElementosComunes_Normas> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<ElementosComunes_Normas, bool>> match)
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

        public new async Task<ElementosComunes_Normas> FindAsync(Expression<Func<ElementosComunes_Normas, bool>> match)
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

        public new List<ElementosComunes_Normas> GetAll()
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

        public new async Task<List<ElementosComunes_Normas>> GetAllAsync()
        {
            try
            {
                var listaEntidad = await base.GetAllAsync();
                return listaEntidad.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(ElementosComunes_Normas entity)
        {
            try
            {
                var listaRegistros = await this.FindWhereAsync(x => x.IntNormaID == entity.IntNormaID && x.IntElementoComunID == entity.IntElementoComunID && x.IntRegistroID != entity.IntRegistroID);

                if (listaRegistros.Count() != 0)
                    return RecursosRequisitosYSoportes.msnNormaDuplicada;

                var ElementosComunes_Normas = await this.FindAsync(x => x.IntRegistroID == entity.IntRegistroID);

                ElementosComunes_Normas.IntElementoComunID = entity.IntElementoComunID;
                ElementosComunes_Normas.IntNormaID = entity.IntNormaID;
                await base.UpdateAsync(ElementosComunes_Normas);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(ElementosComunes_Normas model)
        {
            try
            {
                if (model.IntRegistroID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
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

        ~ElementosComunes_NormasCoreBusiness()
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
