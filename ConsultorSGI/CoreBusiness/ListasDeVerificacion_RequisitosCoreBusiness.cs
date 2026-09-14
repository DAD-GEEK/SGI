using CoreBusiness.Interfaces;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;

namespace CoreBusiness
{
    public class ListasDeVerificacion_RequisitosCoreBusiness : CRUDGenerico<ListasDeVerificacion_Requisitos>, IListasDeVerificacion_RequisitosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        public ListasDeVerificacion_RequisitosCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        public new async Task<string> SaveEntityAsync(ListasDeVerificacion_Requisitos entity)
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

        public new async Task<string> CreateAsync(ListasDeVerificacion_Requisitos entity)
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

        public new async Task DeleteAsync(ListasDeVerificacion_Requisitos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<ListasDeVerificacion_Requisitos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<ListasDeVerificacion_Requisitos, bool>> match)
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

        public new async Task<ListasDeVerificacion_Requisitos> FindAsync(Expression<Func<ListasDeVerificacion_Requisitos, bool>> match)
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

        public new async Task<List<ListasDeVerificacion_Requisitos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(ListasDeVerificacion_Requisitos entity)
        {
            try
            {
                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarRequisitosSeleccionadosEnListaDeVerificacionAsync(int listaDeVerificacionID, List<ListasDeVerificacion_Requisitos> listaRequisitosSeleccionados)
        {
            try
            {
                var listaRequisitosEnBD = await this.FindWhereAsync(x => x.IntListaDeVerificacionID == listaDeVerificacionID);

                var listaRequisitosNOSeleccionados = listaRequisitosEnBD.Where(x => !listaRequisitosSeleccionados.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();
                var listaRequisitosNuevos = listaRequisitosSeleccionados.Where(x => !listaRequisitosEnBD.Any(y => y.IntNumeralID == x.IntNumeralID)).ToList();

                if (listaRequisitosNOSeleccionados.Count() != 0)
                    await this.DeleteRangeAsync(listaRequisitosNOSeleccionados);

                if (listaRequisitosNuevos.Count() != 0)
                    await this.CreateRangeAsync(listaRequisitosNuevos);

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

        ~ListasDeVerificacion_RequisitosCoreBusiness()
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
