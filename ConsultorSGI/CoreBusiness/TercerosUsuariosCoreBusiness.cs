using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using CoreBusiness.Servicios;
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
    public class TercerosUsuariosCoreBusiness : CRUDGenerico<TercerosUsuarios>, ITercerosUsuariosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private IAspNetUsersCoreBusiness _iAspNetUsersCoreBusiness;
        private ISeguridadCoreBusiness _iSeguridadCoreBusiness;

        public TercerosUsuariosCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        #region CRUD Generico
        private new async Task<string> CreateAsync(TercerosUsuarios entity)
        {
            try
            {
                var existeUsuario = this.GetAll().Any(x => x.StrUsuarioEmail.ToLower().Trim() == entity.StrUsuarioEmail.ToLower().Trim());

                if (existeUsuario) return RecursoUsuarios.msnUsuarioExiste;

                entity.IntTerceroID = entity.IntTerceroID;
                entity.StrUsuarioNombre = entity.StrUsuarioNombre;
                entity.StrUsuarioEmail = entity.StrUsuarioEmail;
                entity.OpcEmailEnviado = entity.OpcEmailEnviado;
                entity.OpcEstado = entity.OpcEstado;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(TercerosUsuarios entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<TercerosUsuarios> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<TercerosUsuarios, bool>> match)
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

        public new async Task<TercerosUsuarios> FindAsync(Expression<Func<TercerosUsuarios, bool>> match)
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

        public new async Task<List<TercerosUsuarios>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(TercerosUsuarios entity)
        {
            try
            {
                _iAspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();

                var existeUsuario = this.GetAll().Any(x => x.StrUsuarioEmail.ToLower().Trim() == entity.StrUsuarioEmail.ToLower().Trim() && x.IntRegistroID != entity.IntRegistroID);
                if (existeUsuario) return RecursoUsuarios.msnUsuarioExiste;

                TercerosUsuarios TercerosUsuarios = await this.FindAsync(x => x.IntRegistroID == entity.IntRegistroID);

                if (TercerosUsuarios.OpcEstado != entity.OpcEstado)
                {
                    var aspNetUser = await _iAspNetUsersCoreBusiness.FindAsync(x => x.Email == entity.StrUsuarioEmail);
                    if (aspNetUser != null)
                    {
                        if (entity.OpcEstado != true) await this.BloquearAspNetUser(entity.StrUsuarioEmail);
                        if (entity.OpcEstado != false) await this.DesBloquearAspNetUser(entity.StrUsuarioEmail);
                    }
                }

                TercerosUsuarios.IntTerceroID = entity.IntTerceroID;
                TercerosUsuarios.StrUsuarioNombre = entity.StrUsuarioNombre;
                TercerosUsuarios.StrUsuarioEmail = entity.StrUsuarioEmail;
                TercerosUsuarios.OpcEmailEnviado = entity.OpcEmailEnviado;
                TercerosUsuarios.OpcEstado = entity.OpcEstado;
                await base.UpdateAsync(TercerosUsuarios);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(TercerosUsuarios model)
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
        #endregion

        #region CoreBusiness
        public async Task BloquearAspNetUser(string email)
        {
            try
            {
                _iSeguridadCoreBusiness = new SeguridadCoreBusiness();
                _iAspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();

                var usuario = await _iAspNetUsersCoreBusiness.FindAsync(x => x.Email == email);
                await _iSeguridadCoreBusiness.BloquearAspNetUser(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DesBloquearAspNetUser(string email)
        {
            try
            {
                _iSeguridadCoreBusiness = new SeguridadCoreBusiness();
                _iAspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();

                var usuario = await _iAspNetUsersCoreBusiness.FindAsync(x => x.Email == email);
                await _iSeguridadCoreBusiness.DesBloquearAspNetUser(usuario);
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

        ~TercerosUsuariosCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iAspNetUsersCoreBusiness != null)
                {
                    _iAspNetUsersCoreBusiness.Dispose();
                    _iAspNetUsersCoreBusiness = null;
                }

                if (_iSeguridadCoreBusiness != null)
                {
                    _iSeguridadCoreBusiness.Dispose();
                    _iSeguridadCoreBusiness = null;
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
