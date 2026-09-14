using Across.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CoreBusiness
{
    public class TemasActaCoreBusiness : CRUDGenerico<TemasActa>, ITemasActa
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public TemasActaCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(TemasActa entity)
        {
            try
            {

                entity.StrDescripcion = entity.StrDescripcion;
                entity.StrDesarrollo = Regex.Escape(entity.StrDesarrollo);
                entity.StrResponsable = entity.StrResponsable;
                entity.IntActaID = entity.IntActaID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(TemasActa entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<TemasActa> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<TemasActa, bool>> match)
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

        public new async Task<TemasActa> FindAsync(Expression<Func<TemasActa, bool>> match)
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

        public new async Task<List<TemasActa>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(TemasActa entity)
        {
            try
            {
                TemasActa TemasActa = await this.FindAsync(x => x.IntTemaID == entity.IntTemaID);

                TemasActa.StrDescripcion = entity.StrDescripcion;
                TemasActa.StrDesarrollo = Regex.Escape(entity.StrDesarrollo);
                TemasActa.StrResponsable = entity.StrResponsable;
                TemasActa.IntActaID = entity.IntActaID;

                await base.UpdateAsync(TemasActa);

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

        ~TemasActaCoreBusiness()
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
