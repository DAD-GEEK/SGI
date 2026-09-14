using Across.ArchivosDeRecurso;
using Across.Interfaces;
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
    public class UsuariosCoreBusiness : CRUDGenerico<Usuarios>, IUsuarios
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        SeguridadCoreBusiness _seguridadCoreBusiness;

        public UsuariosCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Usuarios entity)
        {
            try
            {
                _seguridadCoreBusiness = new SeguridadCoreBusiness();

                var listaUsuarios = await GetAllAsync();

                if (listaUsuarios.Any(x => x.StrCodigo.ToLower().Equals(entity.StrCodigo.ToLower())))
                {
                    return RecursoUsuarios.msnValidacionCrear;
                }

                entity.StrCodigo = entity.StrCodigo.Trim();
                entity.StrNombre = entity.StrNombre.Trim();
                entity.StrClave = _seguridadCoreBusiness.cifrarCadena(entity.StrClave.Trim());
                if (!string.IsNullOrEmpty(entity.StrEmail))
                {
                    entity.StrEmail = entity.StrEmail.Trim();
                }
                entity.StrColor = entity.StrColor;
                entity.OpcEstado = entity.OpcEstado;
                entity.IntRolID = entity.IntRolID;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Usuarios entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Usuarios> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Usuarios, bool>> match)
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

        public new async Task<Usuarios> FindAsync(Expression<Func<Usuarios, bool>> match)
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

        public new async Task<List<Usuarios>> GetAllAsync()
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

        public async Task<string> UpdateAsync(Usuarios entity, bool opcClave)
        {
            try
            {
                _seguridadCoreBusiness = new SeguridadCoreBusiness();

                var listaUsuarios = await GetAllAsync();

                if (listaUsuarios.Any(x => x.StrCodigo.ToLower().Equals(entity.StrCodigo.ToLower()) && x.IntUsuarioID != entity.IntUsuarioID))
                {
                    return RecursoUsuarios.msnValidacionCrear;
                }

                Usuarios Usuarios = await this.FindAsync(x => x.IntUsuarioID == entity.IntUsuarioID);

                Usuarios.StrCodigo = entity.StrCodigo.Trim();
                Usuarios.StrNombre = entity.StrNombre.Trim();
                if (opcClave)
                {
                    Usuarios.StrClave = _seguridadCoreBusiness.cifrarCadena(entity.StrClave.Trim());
                }
                if (!string.IsNullOrEmpty(entity.StrEmail))
                {
                    Usuarios.StrEmail = entity.StrEmail.Trim();
                }
                Usuarios.StrColor = entity.StrColor;
                Usuarios.IntCiudadID = entity.IntCiudadID;
                Usuarios.StrDireccion = entity.StrDireccion;
                Usuarios.StrTelefonoFijo = entity.StrTelefonoFijo;
                Usuarios.StrCelular = entity.StrCelular;
                Usuarios.OpcEstado = entity.OpcEstado;

                if (entity.IntRolID != null)
                {
                    Usuarios.IntRolID = entity.IntRolID;
                }

                await base.UpdateAsync(Usuarios);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ValidarCambioClave(Usuarios usuario, string claveAnt) 
        {
            try
            {
                _seguridadCoreBusiness = new SeguridadCoreBusiness();

                var nuevaClave = _seguridadCoreBusiness.cifrarCadena(claveAnt);
                var usuarioBD = await this.FindAsync(x => x.IntUsuarioID == usuario.IntUsuarioID);

                if (nuevaClave != usuarioBD.StrClave)
                {
                    return RecursoUsuarios.msnValicacionCambioClave;
                }

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

        ~UsuariosCoreBusiness()
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
