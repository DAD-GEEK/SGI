using Across.Interfaces;
using DataAccess;
using Across.ArchivosDeRecurso;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreBusiness
{
    public class Contratos_SistemasDeGestionCoreBusiness : CRUDGenerico<Contratos_SistemasDeGestion>, IContratos_SistemasDeGestion
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public Contratos_SistemasDeGestionCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Contratos_SistemasDeGestion entity)
        {
            try
            {

                entity.IntContratoID = entity.IntContratoID;
                entity.IntSistemaID = entity.IntSistemaID;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Contratos_SistemasDeGestion entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Contratos_SistemasDeGestion> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Contratos_SistemasDeGestion, bool>> match)
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

        public new async Task<Contratos_SistemasDeGestion> FindAsync(Expression<Func<Contratos_SistemasDeGestion, bool>> match)
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

        public new async Task<List<Contratos_SistemasDeGestion>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Contratos_SistemasDeGestion entity)
        {
            try
            {
               
                Contratos_SistemasDeGestion Contratos_SistemasDeGestion = await this.FindAsync(x => x.IntRegistroID == entity.IntRegistroID);

                Contratos_SistemasDeGestion.IntContratoID = entity.IntContratoID;
                Contratos_SistemasDeGestion.IntSistemaID = entity.IntSistemaID;
               
                await base.UpdateAsync(Contratos_SistemasDeGestion);

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

        ~Contratos_SistemasDeGestionCoreBusiness()
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
