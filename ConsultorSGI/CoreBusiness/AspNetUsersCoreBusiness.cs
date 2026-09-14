using Across;
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
using System.Web.Mvc;

namespace CoreBusiness
{
    public class AspNetUsersCoreBusiness : CRUDGenerico<AspNetUsers>, IAspNetUsersCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        ITercerosCoreBusiness _iTercerosCoreBusiness;
        public AspNetUsersCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        #region CRUD Genérico

        public async Task<int> GetTerceroIDFromCurrentUserAsync()
        {
            try
            {
                var emailCurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
                var currentUser = await this.FindAsync(x => x.Email == emailCurrentUser);

                return currentUser.TerceroID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetTerceroIDFromCurrentUser()
        {
            try
            {
                var emailCurrentUser = System.Web.HttpContext.Current.User.Identity.Name;
                var currentUser = this.Find(x => x.Email == emailCurrentUser);

                if (currentUser == null)
                    return 0;

                return currentUser.TerceroID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<string> UpdateAsync(AspNetUsers entity)
        {
            try
            {
                AspNetUsers AspNetUsers = await this.FindAsync(x => x.UserName == entity.UserName);

                AspNetUsers.TerceroID = entity.TerceroID;
                AspNetUsers.NombreUsuario = entity.NombreUsuario;
                AspNetUsers.FechaIngreso = entity.FechaIngreso;
                AspNetUsers.Email = entity.Email;
                AspNetUsers.EmailConfirmed = entity.EmailConfirmed;
                AspNetUsers.PasswordHash = entity.PasswordHash;
                AspNetUsers.SecurityStamp = entity.SecurityStamp;
                AspNetUsers.PhoneNumber = entity.PhoneNumber;
                AspNetUsers.PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
                AspNetUsers.TwoFactorEnabled = entity.TwoFactorEnabled;
                AspNetUsers.LockoutEnabled = entity.LockoutEnabled;
                AspNetUsers.LockoutEndDateUtc = entity.LockoutEndDateUtc;
                AspNetUsers.AccessFailedCount = entity.AccessFailedCount;
                AspNetUsers.UserName = entity.UserName;
                AspNetUsers.NombreImagen = entity.NombreImagen;

                await base.UpdateAsync(AspNetUsers);

                ServicioUsuarioCoreBusiness.RefrescarDatosDeUsuarioEnSesion();

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AspNetUsers entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AspNetUsers, bool>> match)
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

        public new async Task<AspNetUsers> FindAsync(Expression<Func<AspNetUsers, bool>> match)
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

        public new AspNetUsers Find(Expression<Func<AspNetUsers, bool>> match)
        {
            try
            {
                return base.Find(match);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task<List<AspNetUsers>> GetAllAsync()
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
        #endregion

        #region Validaciones
        public async Task<bool> UserExists(string email)
        {
            try
            {
                var usuario = await this.FindAsync(x => x.Email == email);

                if (usuario != null) return true;

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> BloquearUsuario(TercerosUsuarios modelo)
        {
            var usuario = await FindAsync(x => x.Email == modelo.StrUsuarioEmail);

            if (modelo.OpcEstado == true && usuario.LockoutEndDateUtc < DateTime.Now)
            {
                //UserManager
            }

            return false;
        }

        #endregion

        #region DropDown List
        public async Task<SelectList> SelectListUsuariosAsync(string valueSelected = null)
        {
            try
            {
                _iTercerosCoreBusiness = new TercerosCoreBusiness();
                var terceroModelo = await _iTercerosCoreBusiness.FindAsync(x => x.StrIdentificacion == Empresa.Nit);

                var entidad = (from c in await this.FindWhereAsync(x => x.EmailConfirmed == true && x.TerceroID == terceroModelo.IntTerceroID)
                               orderby c.Email
                               select new { UsuarioID = c.Id, Descripcion = c.NombreUsuario });

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "UsuarioID", "Descripcion");

                return new SelectList(entidad, "UsuarioID", "Descripcion", valueSelected);

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

        ~AspNetUsersCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iTercerosCoreBusiness != null)
                {
                    _iTercerosCoreBusiness.Dispose();
                    _iTercerosCoreBusiness = null;
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
