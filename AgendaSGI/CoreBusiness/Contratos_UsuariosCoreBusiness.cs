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
    public class Contratos_UsuariosCoreBusiness : CRUDGenerico<Contratos_Usuarios>, IContratos_Usuarios
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public Contratos_UsuariosCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Contratos_Usuarios entity)
        {
            try
            {

                entity.IntContratoID = entity.IntContratoID;
                entity.IntUsuarioID = entity.IntUsuarioID;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Contratos_Usuarios entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Contratos_Usuarios> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Contratos_Usuarios, bool>> match)
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

        public new async Task<Contratos_Usuarios> FindAsync(Expression<Func<Contratos_Usuarios, bool>> match)
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

        public new async Task<List<Contratos_Usuarios>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Contratos_Usuarios entity)
        {
            try
            {
               
                Contratos_Usuarios Contratos_Usuarios = await this.FindAsync(x => x.IntRegistroID == entity.IntRegistroID);

                Contratos_Usuarios.IntContratoID = entity.IntContratoID;
                Contratos_Usuarios.IntUsuarioID = entity.IntUsuarioID;
               
                await base.UpdateAsync(Contratos_Usuarios);

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

        ~Contratos_UsuariosCoreBusiness()
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
