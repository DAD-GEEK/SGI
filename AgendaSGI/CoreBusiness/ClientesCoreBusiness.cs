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
    public class ClientesCoreBusiness : CRUDGenerico<Clientes>, IClientes
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public ClientesCoreBusiness() : base(new datosNetEntities()) { }

        public new async Task<string> CreateAsync(Clientes entity)
        {
            try
            {
                var listaClientes = await GetAllAsync();

                if (listaClientes.Any(x => x.StrIdentificacion.Equals(entity.StrIdentificacion)))
                {
                    return RecursoClientes.msnValidacionCrear;
                }

                var email = string.Empty;
                if (!string.IsNullOrEmpty(entity.StrEmail))
                {
                    email = entity.StrEmail.Trim();
                }

                entity.StrIdentificacion = entity.StrIdentificacion;
                entity.IntDV = entity.IntDV;
                entity.StrNombre = entity.StrNombre.Trim();
                entity.StrDireccion = entity.StrDireccion;
                entity.StrEmail = email;
                entity.IntCiudadID = entity.IntCiudadID;
                entity.StrTelefono = entity.StrTelefono;
                entity.StrPaginaWeb = entity.StrPaginaWeb;
                entity.DatFechaIngreso = entity.DatFechaIngreso;
                entity.OpcEstado = entity.OpcEstado;
               
                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Clientes entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Clientes> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Clientes, bool>> match)
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

        public new async Task<Clientes> FindAsync(Expression<Func<Clientes, bool>> match)
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

        public new async Task<List<Clientes>> GetAllAsync()
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

        public new async Task<string> UpdateAsync(Clientes entity)
        {
            try
            {
                var listaClientes = await GetAllAsync();

                if (listaClientes.Any(x => x.StrIdentificacion.Equals(entity.StrIdentificacion) && x.IntClienteID != entity.IntClienteID))
                {
                    return RecursoClientes.msnValidacionCrear;
                }

                Clientes Clientes = await this.FindAsync(x => x.IntClienteID == entity.IntClienteID);

                var email = string.Empty;
                if (!string.IsNullOrEmpty(entity.StrEmail))
                {
                    email = entity.StrEmail.Trim();
                }

                Clientes.StrIdentificacion = entity.StrIdentificacion;
                Clientes.StrNombre = entity.StrNombre.Trim();
                Clientes.IntDV = entity.IntDV;
                Clientes.StrDireccion = entity.StrDireccion;
                Clientes.StrEmail = email;
                Clientes.IntCiudadID = entity.IntCiudadID;
                Clientes.StrTelefono = entity.StrTelefono;
                Clientes.StrPaginaWeb = entity.StrPaginaWeb;
                Clientes.DatFechaIngreso = entity.DatFechaIngreso;
                Clientes.OpcEstado = entity.OpcEstado;

                await base.UpdateAsync(Clientes);

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

        ~ClientesCoreBusiness()
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
