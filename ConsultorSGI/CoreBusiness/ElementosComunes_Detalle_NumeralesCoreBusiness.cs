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
    public class ElementosComunes_Detalle_NumeralesCoreBusiness : CRUDGenerico<ElementosComunes_Detalle_Numerales>, IElementosComunes_Detalle_NumeralesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private INormasCoreBusiness _iNormasCoreBusiness;
        public ElementosComunes_Detalle_NumeralesCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iNormasCoreBusiness = new NormasCoreBusiness();
        }

        public new async Task<string> SaveEntityAsync(ElementosComunes_Detalle_Numerales entity)
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

        public new async Task<string> CreateAsync(ElementosComunes_Detalle_Numerales entity)
        {
            try
            {
                entity.IntDetalleID = entity.IntDetalleID;
                entity.IntNumeralID = entity.IntNumeralID;
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(ElementosComunes_Detalle_Numerales entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<ElementosComunes_Detalle_Numerales> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<ElementosComunes_Detalle_Numerales, bool>> match)
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

        public new async Task<ElementosComunes_Detalle_Numerales> FindAsync(Expression<Func<ElementosComunes_Detalle_Numerales, bool>> match)
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

        public new List<ElementosComunes_Detalle_Numerales> GetAll()
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

        public new async Task<List<ElementosComunes_Detalle_Numerales>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(ElementosComunes_Detalle_Numerales entity)
        {
            try
            {
                var ElementosComunes_Detalle_Numerales = await this.FindAsync(x => x.IntRegistroID == entity.IntRegistroID);

                ElementosComunes_Detalle_Numerales.IntDetalleID = entity.IntDetalleID;
                ElementosComunes_Detalle_Numerales.IntNumeralID = entity.IntNumeralID;
                await base.UpdateAsync(ElementosComunes_Detalle_Numerales);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(ElementosComunes_Detalle_Numerales model)
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

        ~ElementosComunes_Detalle_NumeralesCoreBusiness()
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
