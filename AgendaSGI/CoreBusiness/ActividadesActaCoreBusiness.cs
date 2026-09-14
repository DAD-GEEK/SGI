using Across.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace CoreBusiness
{
    public class ActividadesActaCoreBusiness : CRUDGenerico<ActividadesActa>, IActividadesActa
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ActividadesActaCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(ActividadesActa entity)
        {
            try
            {

                entity.StrDescripcion = Regex.Escape(entity.StrDescripcion);
                entity.StrResponsable = entity.StrResponsable;
                entity.DatFecha = entity.DatFecha;
                entity.OpcEjecuta = entity.OpcEjecuta;
                entity.IntActaID = entity.IntActaID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(ActividadesActa entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<ActividadesActa> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<ActividadesActa, bool>> match)
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

        public new async Task<ActividadesActa> FindAsync(Expression<Func<ActividadesActa, bool>> match)
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

        public new async Task<List<ActividadesActa>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(ActividadesActa entity)
        {
            try
            {
                ActividadesActa ActividadesActa = await this.FindAsync(x => x.IntActividadID == entity.IntActividadID);

                ActividadesActa.StrDescripcion = Regex.Escape(entity.StrDescripcion);
                ActividadesActa.StrResponsable = entity.StrResponsable;
                ActividadesActa.DatFecha = entity.DatFecha;
                ActividadesActa.OpcEjecuta = entity.OpcEjecuta;
                ActividadesActa.IntActaID = entity.IntActaID;

                await base.UpdateAsync(ActividadesActa);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ActividadesActa> UnescapeActividadesActa(List<ActividadesActa> listaActividades)
        {
            try
            {
                foreach (var item in listaActividades)
                {
                    try
                    {
                        item.StrDescripcion = Regex.Unescape(item.StrDescripcion);
                    }
                    catch (Exception)
                    {
                        item.StrDescripcion = item.StrDescripcion;
                    }
                }

                return listaActividades;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActividadesActa UnescapeActividadesActa(ActividadesActa actividad)
        {
            try
            {
                try
                {
                    actividad.StrDescripcion = Regex.Unescape(actividad.StrDescripcion);
                }
                catch (Exception)
                {
                    actividad.StrDescripcion = actividad.StrDescripcion;
                }

                return actividad;
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

        ~ActividadesActaCoreBusiness()
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
