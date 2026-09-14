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
using static Across.Enumeraciones;

namespace CoreBusiness
{
    public class AspNetRolesCoreBusiness : CRUDGenerico<AspNetRoles>, IAspNetRolesCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        readonly string rolAdministrador = Convert.ToInt16((int)enumRolesGestionIntegral.Administrador).ToString();
        readonly string rolAsesor = Convert.ToInt16((int)enumRolesGestionIntegral.Asesor).ToString();

        public AspNetRolesCoreBusiness() : base(new gestioni_consultorNetEntities()) { }

        #region CRUD Generico
        public new async Task<string> SaveEntityAsync(AspNetRoles entity)
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

        private new async Task<string> CreateAsync(AspNetRoles entity)
        {
            try
            {
                var listaRoles = await this.GetAllAsync();
                if (listaRoles.Any(x => x.Name.Trim().ToLower() == entity.Name.Trim().ToLower())) return RecursoAspNetRoles.msnRolYaExiste;

                entity.Id = Guid.NewGuid().ToString();
                entity.Name = entity.Name;
                entity.BitDefault = entity.BitDefault;
                entity.BitActivo = entity.BitActivo;
                entity.Discriminator = "IdentityRole";

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(AspNetRoles entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<AspNetRoles> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<AspNetRoles, bool>> match)
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

        public new async Task<AspNetRoles> FindAsync(Expression<Func<AspNetRoles, bool>> match)
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

        public new async Task<List<AspNetRoles>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(AspNetRoles entity)
        {
            try
            {
                var listaRoles = await this.GetAllAsync();
                if (listaRoles.Any(x => x.Id != entity.Id && x.Name.Trim().ToLower() == entity.Name.Trim().ToLower())) return RecursoAspNetRoles.msnRolYaExiste;

                AspNetRoles AspNetRoles = await this.FindAsync(x => x.Id == entity.Id);

                AspNetRoles.Name = entity.Name;
                AspNetRoles.Description = entity.Description;
                AspNetRoles.BitDefault = entity.BitDefault;
                AspNetRoles.BitActivo = entity.BitActivo;

                await base.UpdateAsync(AspNetRoles);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> SelectListAsync(string valueSelected = null, List<string> groupSelected = null, bool bitDefault = true)
        {
            try
            {
                var entidad = (from e in await GetAllAsync()
                               where e.BitActivo == true && e.BitDefault == bitDefault
                               orderby e.Name
                               select new { CodigoID = e.Id, Descripcion = $"{e.Name} - {e.Description}" });

                if (string.IsNullOrEmpty(valueSelected) && groupSelected == null) return new MultiSelectList(entidad, "CodigoID", "Descripcion");
                if (!string.IsNullOrEmpty(valueSelected)) return new MultiSelectList(entidad, "CodigoID", "Descripcion", valueSelected);

                return new MultiSelectList(entidad, "CodigoID", "Descripcion", groupSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SaveAllAsync(AspNetRoles model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Id)) return await this.UpdateAsync(model);
                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Validaciones        
        public string ValidarSiEsRolPrincipal(string rolID)
        {
            try
            {
                if (rolID == rolAdministrador || rolID == rolAsesor) return RecursoAspNetRoles.msnControlEliminarRol;
                return string.Empty;
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

        ~AspNetRolesCoreBusiness()
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
