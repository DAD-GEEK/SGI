using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess;
using DataAccess.Servicios;
using Models;
using Models.DTO;
using Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using NPOI.SS.Formula.Functions;
using static Across.Enumeraciones;
using Models.Migrations;
using System.IO;

namespace CoreBusiness
{
    public class Terceros_ClientesCoreBusiness : CRUDGenerico<Terceros_Clientes>, ITerceros_ClientesCoreBusiness
    {

        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        ICiudadesCoreBusiness _iCiudadesCoreBusiness;
        ICommonCoreBusiness _iCommonCoreBusiness;

        private int _recordsTotal;
        public int TotalRegistrosDataTable { get { return _recordsTotal; } }

        public Terceros_ClientesCoreBusiness() : base(new gestioni_consultorNetEntities(new ServicioTercero()))
        {
            this._iCiudadesCoreBusiness = new CiudadesCoreBusiness();
            this._iCommonCoreBusiness = new CommonCoreBusiness();
        }

        #region CRUD Generico
        public async Task<List<TercerosClientesDTO>> GetPaginacionTercerosClientes(DatatableParamsDTO datatableParamsDTO, bool paginarInformacion = true)
        {
            try
            {
                List<TercerosClientesDTO> tercerosDTOs = null;
                IQueryable<TercerosClientesDTO> sentencia = null;

                var terceroID = await _iCommonCoreBusiness.GetTerceroIDFromCurrentUserAsync();

                int pageSize = datatableParamsDTO.length != null ? Convert.ToInt32(datatableParamsDTO.length) : 0;
                int skip = datatableParamsDTO.start != null ? Convert.ToInt32(datatableParamsDTO.start) : 0;

                using (gestioni_consultorNetEntities db = new gestioni_consultorNetEntities())
                {
                    sentencia = (from cli in db.Terceros_Clientes
                                 join ter in db.Terceros on cli.IntTerceroID equals ter.IntTerceroID
                                 where ter.IntTerceroID == terceroID && cli.StrIdentificacion != ((int)enumGenericos.General).ToString()
                                 orderby cli.IntTerceroClienteID descending
                                 select new TercerosClientesDTO
                                 {
                                     IntTerceroClienteID = cli.IntTerceroClienteID,
                                     StrIdentificacion = cli.StrIdentificacion,
                                     StrTerceroIdentificacion = ter.StrIdentificacion,
                                     IntDV = cli.IntDV,
                                     StrNombre = cli.StrNombre,
                                     DatFechaIngreso = cli.DatFechaIngreso,
                                     IntCiudadID = cli.IntCiudadID,
                                     StrDireccion = cli.StrDireccion,
                                     StrEmail = cli.StrEmail,
                                     IntTerceroID = cli.IntTerceroID,
                                     BitEstado = cli.BitEstado,
                                     StrPaginaWeb = cli.StrPaginaWeb,
                                     StrRutaImagen = cli.StrRutaImagen,
                                     StrRepresentanteLegal = cli.StrRepresentanteLegal,
                                     StrTelefono = cli.StrTelefono,

                                 });

                    if (!string.IsNullOrEmpty(datatableParamsDTO.searchValue))
                        sentencia = sentencia.Where(d =>
                        d.StrIdentificacion.ToString().Contains(datatableParamsDTO.searchValue) ||
                        d.StrNombre.Contains(datatableParamsDTO.searchValue));

                    if (!string.IsNullOrEmpty(datatableParamsDTO.sortColumn) && !string.IsNullOrEmpty(datatableParamsDTO.sortColumnDir))
                        sentencia = sentencia.OrderBy(datatableParamsDTO.sortColumn + " " + datatableParamsDTO.sortColumnDir);

                    _recordsTotal = sentencia.Count();

                    if (paginarInformacion)
                        tercerosDTOs = sentencia.Skip(skip).Take(pageSize).ToList();
                    else
                        tercerosDTOs = sentencia.ToList();
                }

                tercerosDTOs.ForEach(item => { item.StrRutaImagen = Archivos.GetUrlImagenTerceroClienteOrUrlDefault(item.StrIdentificacion, item.StrRutaImagen); });

                return tercerosDTOs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private new async Task<string> CreateAsync(Terceros_Clientes entity)
        {
            try
            {
                var terceros_Clientes = await this.FindAsync(x => x.StrIdentificacion == entity.StrIdentificacion);

                if (terceros_Clientes != null)
                    return RecursoTerceros_Clientes.msnTerceroDuplicado;

                entity.StrIdentificacion = entity.StrIdentificacion.Trim();
                entity.IntDV = entity.IntDV;
                entity.StrNombre = entity.StrNombre.Trim();
                entity.StrDireccion = entity.StrDireccion;
                entity.StrEmail = entity.StrEmail;
                entity.IntCiudadID = entity.IntCiudadID;
                entity.StrTelefono = entity.StrTelefono;
                entity.StrPaginaWeb = entity.StrPaginaWeb;
                entity.DatFechaIngreso = entity.DatFechaIngreso;
                entity.StrRepresentanteLegal = entity.StrRepresentanteLegal;
                entity.BitEstado = entity.BitEstado;

                await base.CreateAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Terceros_Clientes entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Terceros_Clientes> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Terceros_Clientes, bool>> match)
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

        public new async Task<Terceros_Clientes> FindAsync(Expression<Func<Terceros_Clientes, bool>> match)
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

        public new async Task<List<Terceros_Clientes>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(Terceros_Clientes entity)
        {
            try
            {
                var terceros_Clientes = await this.FindAsync(x => x.IntTerceroClienteID != entity.IntTerceroClienteID && x.StrIdentificacion == entity.StrIdentificacion);

                if (terceros_Clientes != null)
                    return RecursoTerceros_Clientes.msnTerceroDuplicado;

                Terceros_Clientes Terceros_Clientes = await this.FindAsync(x => x.IntTerceroClienteID == entity.IntTerceroClienteID);

                Terceros_Clientes.StrIdentificacion = entity.StrIdentificacion.Trim();
                Terceros_Clientes.StrNombre = entity.StrNombre;
                Terceros_Clientes.IntDV = entity.IntDV;
                Terceros_Clientes.StrDireccion = entity.StrDireccion;
                Terceros_Clientes.StrEmail = entity.StrEmail;
                Terceros_Clientes.IntCiudadID = entity.IntCiudadID;
                Terceros_Clientes.StrTelefono = entity.StrTelefono;
                Terceros_Clientes.StrPaginaWeb = entity.StrPaginaWeb;
                Terceros_Clientes.StrRepresentanteLegal = entity.StrRepresentanteLegal;
                Terceros_Clientes.BitEstado = entity.BitEstado;

                await base.UpdateAsync(Terceros_Clientes);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarRegistroAsync(Terceros_Clientes model)
        {
            try
            {
                if (model.IntTerceroClienteID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarTerceroClienteAsync(int terceroClienteID)
        {
            try
            {
                var terceroClienteModelo = await this.FindAsync(x => x.IntTerceroClienteID == terceroClienteID);
                bool esEliminacion = true;

                await this.EliminarImagenAsync(terceroClienteModelo, esEliminacion);

                await this.DeleteAsync(terceroClienteModelo);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarImagenAsyn(HttpFileCollectionBase imagenTercero, FormCollection collection)
        {
            try
            {
                var terceroClienteID = Convert.ToInt32(collection["terceroClienteID"].ToString());

                var terceroClienteModelo = await this.FindAsync(x => x.IntTerceroClienteID == terceroClienteID);

                if (terceroClienteModelo is null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                string rutaImagenAnterior = Archivos.ObtenerRutaDeImagenTerceroCliente(terceroClienteModelo.Terceros.StrIdentificacion, terceroClienteModelo.StrRutaImagen);
                Archivos.EliminarArchivo(rutaImagenAnterior);

                ParamFilesDTO parametros = new ParamFilesDTO();
                parametros.ruta = HttpContext.Current.Server.MapPath($"~/{Archivos.rutaImagenTercerosClientes}/{terceroClienteModelo.Terceros.StrIdentificacion}/");
                parametros.nombreArchivo = $"{terceroClienteModelo.StrIdentificacion}";

                var rutaImagen = Archivos.GuardarArchivo(imagenTercero, parametros);

                if (!string.IsNullOrEmpty(rutaImagen))
                {
                    terceroClienteModelo.StrRutaImagen = rutaImagen;
                    await base.UpdateAsync(terceroClienteModelo);
                    return rutaImagen;
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarImagenAsync(Terceros_Clientes modelo, bool esEliminacion = false)
        {
            try
            {
                if (modelo is null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                string rutaImagen = $"{Archivos.rutaImagenTercerosClientes}/{modelo.Terceros.StrIdentificacion}/{modelo.StrRutaImagen}";

                var rutaImagenCompleta = HttpContext.Current.Server.MapPath($"~/{rutaImagen}");
                var respuestaEliminar = Archivos.EliminarArchivo(rutaImagenCompleta);

                if (esEliminacion)
                {
                    if (string.IsNullOrEmpty(respuestaEliminar))
                    {
                        modelo.StrRutaImagen = string.Empty;

                        await base.UpdateAsync(modelo);
                        return string.Empty;
                    }
                }

                return respuestaEliminar;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarTerceroCliente_ClienteSegunTerceroAsync(Terceros tercero)
        {
            try
            {
                Terceros_Clientes terceroClienteEnBD = await this.FindAsync(x => x.StrIdentificacion == tercero.StrIdentificacion);

                var terceroCliente = new Terceros_Clientes()
                {
                    IntTerceroID = tercero.IntTerceroID,
                    StrIdentificacion = tercero.StrIdentificacion,
                    IntDV = tercero.IntDV,
                    StrNombre = tercero.StrNombre,
                    StrDireccion = tercero.StrDireccion,
                    StrEmail = tercero.StrEmail,
                    DatFechaIngreso = tercero.DatFechaIngreso,
                    IntCiudadID = (int)tercero.IntCiudadID,
                    StrPaginaWeb = tercero.StrPaginaWeb,
                    StrTelefono = tercero.StrTelefono,
                    StrRepresentanteLegal = tercero.StrRepresentanteLegal,
                    StrRutaImagen = tercero.StrRutaImagen,
                    BitEstado = (bool)tercero.OpcEstado
                };

                if (terceroClienteEnBD != null)
                    terceroCliente.IntTerceroClienteID = terceroClienteEnBD.IntTerceroClienteID;

                await this.GuardarRegistroAsync(terceroCliente);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region DropDownList
        public async Task<SelectList> DropDownListCiudadesAsync(string valueSelected = null)
        {
            try
            {
                return await _iCiudadesCoreBusiness.DropDownListCiudadesAsync(valueSelected);
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

        ~Terceros_ClientesCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._iCiudadesCoreBusiness != null)
                {
                    this._iCiudadesCoreBusiness.Dispose();
                    this._iCiudadesCoreBusiness = null;
                }

                if (this._iCommonCoreBusiness != null)
                {
                    this._iCommonCoreBusiness.Dispose();
                    this._iCommonCoreBusiness = null;
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
