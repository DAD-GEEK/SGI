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
using Models.DTO;

namespace CoreBusiness
{
    public class ContactosCoreBusiness : CRUDGenerico<Contactos>, IContactos
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ContactosCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Contactos entity)
        {
            try
            {
                entity.IntClienteID = entity.IntClienteID;
                entity.StrNombre = entity.StrNombre.Trim();
                entity.StrCargo = entity.StrCargo.Trim();
                entity.StrTelefonoFijo = entity.StrTelefonoFijo;
                entity.StrCelular = entity.StrCelular;
                entity.StrEmail = entity.StrEmail;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Contactos entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Contactos> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Contactos, bool>> match)
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

        public new async Task<Contactos> FindAsync(Expression<Func<Contactos, bool>> match)
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

        public new async Task<List<Contactos>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Contactos entity)
        {
            try
            {              
                Contactos Contactos = await this.FindAsync(x => x.IntContactoID == entity.IntContactoID);

                Contactos.IntContactoID = entity.IntContactoID;
                Contactos.IntClienteID = entity.IntClienteID;
                Contactos.StrNombre = entity.StrNombre.Trim();
                Contactos.StrCargo = entity.StrCargo.Trim();
                Contactos.StrTelefonoFijo = entity.StrTelefonoFijo;
                Contactos.StrCelular = entity.StrCelular;
                Contactos.StrEmail = entity.StrEmail;

                await base.UpdateAsync(Contactos);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task<Contactos> ConvertContactosFromDTO(ContactosDTO contactoDTO)
        //{
        //    try
        //    {
        //        Contactos contacto = new Contactos();

        //        contacto.IntContactoID = contactoDTO.IntContactoID;
        //        contacto.StrNombre = contactoDTO.StrNombre;
        //        contacto.StrCargo = contactoDTO.StrCargo;
        //        contacto.StrTelefonoFjo = contactoDTO.StrTelefonoFjo;
        //        contacto.StrCelular = contactoDTO.StrCelular;
        //        contacto.StrEmail = contactoDTO.StrEmail;
        //        contacto.IntClienteID = contactoDTO.IntClienteID;

        //        return contacto;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        #region Dispose

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ContactosCoreBusiness()
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
