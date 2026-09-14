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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class TercerosCoreBusiness : CRUDGenerico<Terceros>, ITercerosCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        private IAspNetRolesCoreBusiness _iAspNetRolesCoreBusiness;
        private IAspNetTerceroRolesCoreBusiness _iAspNetTerceroRolesCoreBusines;
        private ICiudadesCoreBusiness _iCiudadesCoreBusiness;
        private ITercerosUsuariosCoreBusiness _iTercerosUsuariosCoreBusines;
        private INormasCoreBusiness _iNormasCoreBusiness;
        private ITerceros_NormasCoreBusiness _iTerceros_NormasCoreBusiness;
        private IAspNetTerceroRolesCoreBusiness _iAspNetTerceroRolesCoreBusiness;
        private IServicioTercero _iServicioTercero;
        private ISistemasDeGestionCoreBusiness _iSistemasDeGestionCoreBusiness;
        private ITerceros_SistemasDeGestionCoreBusiness _iTerceros_SistemasDeGestionCoreBusiness;
        private ITerceros_ClientesCoreBusiness _iTerceros_ClientesCoreBusiness;


        public TercerosCoreBusiness() : base(new gestioni_consultorNetEntities())
        {
            this._iAspNetTerceroRolesCoreBusines = new AspNetTerceroRolesCoreBusiness();
            this._iAspNetRolesCoreBusiness = new AspNetRolesCoreBusiness();
            this._iCiudadesCoreBusiness = new CiudadesCoreBusiness();
            this._iTercerosUsuariosCoreBusines = new TercerosUsuariosCoreBusiness();
            this._iNormasCoreBusiness = new NormasCoreBusiness();
            this._iTerceros_NormasCoreBusiness = new Terceros_NormasCoreBusiness();
            this._iAspNetTerceroRolesCoreBusiness = new AspNetTerceroRolesCoreBusiness();
            this._iServicioTercero = new ServicioTercero();
            this._iTerceros_SistemasDeGestionCoreBusiness = new Terceros_SistemasDeGestionCoreBusiness();
            this._iSistemasDeGestionCoreBusiness = new SistemasDeGestionCoreBusiness();
        }

        #region CRUD Generico
        public async Task<TercerosDTO> ObtenerTerceroDTOAsync(int terceroID)
        {
            try
            {
                var tercero = await this.FindAsync(x => x.IntTerceroID == terceroID);

                if (tercero is null)
                    throw new Exception(RecursoCommon.msnRegistroNoEncontrado);

                TercerosDTO tercerosDTO = new TercerosDTO();

                tercerosDTO.IntTerceroID = terceroID;
                tercerosDTO.StrIdentificacion = tercero.StrIdentificacion;
                tercerosDTO.IntDV = tercero.IntDV;
                tercerosDTO.StrNombre = tercero.StrNombre;
                tercerosDTO.StrCiudadCodigo = tercero.Ciudades.StrCodigo;
                tercerosDTO.StrCiudadNombre = tercero.Ciudades.StrDescripcion;
                tercerosDTO.StrDepartamento = tercero.Ciudades.Departamentos.StrDescripcion;
                tercerosDTO.StrActividadEconomica = tercero.StrActividadEconomica;
                tercerosDTO.StrNivelID = tercero.StrNivelID;
                tercerosDTO.StrNivelDescripcion = tercero.Niveles != null ? tercero.Niveles.StrDescripcion : RecursoTerceros.msnTerceroSinNivel;
                tercerosDTO.IntNumeroEmpleados = tercero.IntNumeroEmpleados;
                tercerosDTO.IntContratistasConductores = tercero.IntContratistasConductores;
                tercerosDTO.IntNumeroDeVehiculos = tercero.IntNumeroDeVehiculos;
                tercerosDTO.StrNivelDeRiesgoCodigo = tercero.NivelesDeRiesgo.StrCodigo;
                tercerosDTO.StrNivelDeRiesgoDescripcion = tercero.NivelesDeRiesgo.StrDescripcion;
                tercerosDTO.StrEmail = tercero.StrEmail;
                tercerosDTO.StrDireccion = tercero.StrDireccion;
                tercerosDTO.StrTelefono = tercero.StrTelefono;
                tercerosDTO.StrUrlImagen = Archivos.GetUrlImagenTerceroOrUrlDefault(tercero.StrRutaImagen);

                return tercerosDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }
        private new async Task<string> CreateAsync(Terceros entity)
        {
            try
            {
                this._iTerceros_ClientesCoreBusiness = new Terceros_ClientesCoreBusiness();

                var listaTerceros = await this.FindWhereAsync(x => x.StrIdentificacion.Equals(entity.StrIdentificacion));

                if (listaTerceros.Count() != 0)
                    return RecursoTerceros.msnTerceroExiste;

                entity.StrIdentificacion = entity.StrIdentificacion.Trim();
                entity.IntDV = Common.CalcularDigitoVerificacion(entity.StrIdentificacion);
                entity.StrNombre = entity.StrNombre.Trim();

                await base.CreateAsync(entity);

                await _iTerceros_ClientesCoreBusiness.GuardarTerceroCliente_ClienteSegunTerceroAsync(entity);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public new async Task DeleteAsync(Terceros entity)
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

        public new async Task DeleteRangeAsync(IEnumerable<Terceros> entity)
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

        public new async Task<bool> ExistAsync(Expression<Func<Terceros, bool>> match)
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

        public new async Task<Terceros> FindAsync(Expression<Func<Terceros, bool>> match)
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

        public new async Task<List<Terceros>> GetAllAsync()
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

        private new async Task<string> UpdateAsync(Terceros entity)
        {
            try
            {
                this._iTerceros_ClientesCoreBusiness = new Terceros_ClientesCoreBusiness();

                var listaTerceros = await this.FindWhereAsync(x => x.StrIdentificacion.Equals(entity.StrIdentificacion) && x.IntTerceroID != entity.IntTerceroID);

                if (listaTerceros.Count() != 0)
                    return RecursoTerceros.msnTerceroExiste;

                Terceros Terceros = await this.FindAsync(x => x.IntTerceroID == entity.IntTerceroID);

                Terceros.StrIdentificacion = entity.StrIdentificacion.Trim();
                Terceros.IntDV = Common.CalcularDigitoVerificacion(entity.StrIdentificacion);
                Terceros.StrNombre = entity.StrNombre;
                Terceros.IntDV = entity.IntDV;
                Terceros.StrDireccion = entity.StrDireccion;
                Terceros.StrEmail = entity.StrEmail;
                Terceros.IntCiudadID = entity.IntCiudadID;
                Terceros.StrTelefono = entity.StrTelefono;
                Terceros.StrPaginaWeb = entity.StrPaginaWeb;
                Terceros.StrSigla = entity.StrSigla;
                Terceros.StrHabilitacion = entity.StrHabilitacion;
                Terceros.StrActividadEconomica = entity.StrActividadEconomica;
                Terceros.StrSede = entity.StrSede;
                Terceros.StrRepresentanteLegal = entity.StrRepresentanteLegal;
                Terceros.OpcEstado = entity.OpcEstado;
                Terceros.IntNumeroEmpleados = entity.IntNumeroEmpleados;
                Terceros.IntContratistasConductores = entity.IntContratistasConductores;
                Terceros.IntNumeroDeVehiculos = entity.IntNumeroDeVehiculos;
                Terceros.StrNivelID = entity.StrNivelID;

                await base.UpdateAsync(Terceros);

                if (!(bool)Terceros.OpcEstado)
                    await this.BloquearUsuariosTerceroAsync(Terceros.TercerosUsuarios.ToList());

                await _iTerceros_ClientesCoreBusiness.GuardarTerceroCliente_ClienteSegunTerceroAsync(Terceros);

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task CopiarImagenDeTerceroEnTerceroClienteAsync(Terceros tercero)
        {
            try
            {
                if (!string.IsNullOrEmpty(tercero.StrRutaImagen))
                {
                    this._iTerceros_ClientesCoreBusiness = new Terceros_ClientesCoreBusiness();

                    var rutaImagenOrigen = Archivos.ObtenerRutaDeImagenTercero(tercero.StrRutaImagen);
                    var rutaTercerosClientes = HttpContext.Current.Server.MapPath($"~/{Archivos.rutaImagenTercerosClientes}");
                    var rutaTerceroClienteIdentificacion = HttpContext.Current.Server.MapPath($"~/{Archivos.rutaImagenTercerosClientes}/{tercero.StrIdentificacion}");
                    var rutaImagenDestino = HttpContext.Current.Server.MapPath($"~/{Archivos.rutaImagenTercerosClientes}/{tercero.StrIdentificacion}/{tercero.StrRutaImagen}");

                    if (!Directory.Exists(rutaTercerosClientes))
                        Directory.CreateDirectory(rutaTercerosClientes);

                    if (!Directory.Exists(rutaTerceroClienteIdentificacion))
                        Directory.CreateDirectory(rutaTerceroClienteIdentificacion);

                    Archivos.CopiarImagenEnOtraRuta(rutaImagenOrigen, rutaImagenDestino);

                    var terceroCliente = await _iTerceros_ClientesCoreBusiness.FindAsync(x => x.StrIdentificacion == tercero.StrIdentificacion);

                    if (terceroCliente !=  null)
                    {
                        terceroCliente.StrRutaImagen = tercero.StrRutaImagen;
                        await _iTerceros_ClientesCoreBusiness.GuardarRegistroAsync(terceroCliente);
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        public async Task<string> SaveAllAsync(Terceros model)
        {
            try
            {
                if (model.IntTerceroID != 0)
                    return await this.UpdateAsync(model);

                return await this.CreateAsync(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarInformacionTerceroDocumentoDiagnosticoAsync(Terceros tercero)
        {
            try
            {
                var terceroModeo = await this.FindAsync(x => x.IntTerceroID == tercero.IntTerceroID);
                terceroModeo.IntNumeroEmpleados = tercero.IntNumeroEmpleados;
                terceroModeo.IntNumeroDeVehiculos = tercero.IntNumeroDeVehiculos;
                terceroModeo.IntContratistasConductores = tercero.IntContratistasConductores;

                await base.UpdateAsync(terceroModeo);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarNivelDeTerceroAsync(int terceroID, string nivelID)
        {
            try
            {
                var tercero = await this.FindAsync(x => x.IntTerceroID == terceroID);

                if (!string.IsNullOrEmpty(nivelID))
                {
                    if (tercero.StrNivelID != nivelID)
                    {
                        tercero.StrNivelID = nivelID;
                        await base.UpdateAsync(tercero);
                    }
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> AgregarRolPorTerceroAsync(AspNetTerceroRoles modelo)
        {
            try
            {
                return await _iAspNetTerceroRolesCoreBusines.SaveAllAsync(modelo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> AgregarNormaPorTerceroAsync(Terceros_Normas modelo)
        {
            try
            {
                return await _iTerceros_NormasCoreBusiness.SaveAllAsync(modelo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AspNetTerceroRoles>> ObtenerPermisosPorTerceroAsync(int terceroID)
        {
            try
            {
                var listaPermisosPorTerceros = await _iAspNetTerceroRolesCoreBusines.FindWhereAsync(x => x.IntTerceroID == terceroID);
                return listaPermisosPorTerceros;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<Terceros_Normas>> ObtenerNormasPorTerceroAsync(int terceroID)
        {
            try
            {
                var listaNormasPorTerceros = await _iTerceros_NormasCoreBusiness.FindWhereAsync(x => x.IntTerceroID == terceroID);
                return listaNormasPorTerceros;
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
                var terceroID = Convert.ToInt32(collection["terceroID"].ToString());

                var terceroModel = await this.FindAsync(x => x.IntTerceroID == terceroID);

                if (terceroModel is null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                string rutaImagenAnterior = Archivos.ObtenerRutaDeImagenTercero(terceroModel.StrRutaImagen);
                Archivos.EliminarArchivo(rutaImagenAnterior);

                ParamFilesDTO parametros = new ParamFilesDTO();
                parametros.ruta = HttpContext.Current.Server.MapPath($"~/{Archivos.rutaImagenTercero}");
                parametros.nombreArchivo = $"{terceroModel.StrIdentificacion}";

                var rutaImagen = Archivos.GuardarArchivo(imagenTercero, parametros);

                if (!string.IsNullOrEmpty(rutaImagen))
                {
                    terceroModel.StrRutaImagen = rutaImagen;
                    await base.UpdateAsync(terceroModel);

                    await this.CopiarImagenDeTerceroEnTerceroClienteAsync(terceroModel);

                    return rutaImagen;
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarImagenAsync(int terceroID)
        {
            try
            {
                Terceros terceroModel = await this.FindAsync(x => x.IntTerceroID == terceroID);

                if (terceroModel is null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                string rutaImagen = $"{Archivos.rutaImagenTercero}/{terceroModel.StrRutaImagen}";

                var rutaImagenCompleta = HttpContext.Current.Server.MapPath($"~/{rutaImagen}");
                var respuestaEliminar = Archivos.EliminarArchivo(rutaImagenCompleta);

                if (string.IsNullOrEmpty(respuestaEliminar))
                {
                    terceroModel.StrRutaImagen = string.Empty;

                    await base.UpdateAsync(terceroModel);
                    return string.Empty;
                }

                return respuestaEliminar;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarTerceroAsync(int terceroID)
        {
            try
            {
                this._iTerceros_ClientesCoreBusiness = new Terceros_ClientesCoreBusiness();

                var modeloTercero = await this.FindAsync(x => x.IntTerceroID == terceroID);

                if (modeloTercero is null)
                    return RecursoTerceros.msnTerceroNoExiste;

                if (modeloTercero.Terceros_Clientes.Count != 0)
                    return RecursoAuditorias.msnTerceroConAuditoria;

                var identificacion = modeloTercero.StrIdentificacion;

                await this.DeleteAsync(modeloTercero);

                var modeloTerceroClientes = await _iTerceros_ClientesCoreBusiness.FindAsync(x => x.StrIdentificacion == identificacion);

                if (modeloTerceroClientes != null)
                {
                    var cantidadAuditoriaClientes = modeloTerceroClientes.Auditorias.Count();

                    if (cantidadAuditoriaClientes != 0)
                        return RecursoAuditorias.msnTerceroConAuditoria;

                    await _iTerceros_ClientesCoreBusiness.DeleteAsync(modeloTerceroClientes);
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Otras entidades

        #region Select List
        public async Task<SelectList> SelectListAsync(string valueSelected = null)
        {
            try
            {
                var entidad = (from c in await this.FindWhereAsync(x => x.OpcEstado == true && x.StrIdentificacion != "0")
                               orderby c.StrIdentificacion
                               select new { TerceroID = c.IntTerceroID, Descripcion = c.StrIdentificacion + " - " + c.StrNombre });

                if (string.IsNullOrEmpty(valueSelected))
                    return new SelectList(entidad, "TerceroID", "Descripcion");

                return new SelectList(entidad, "TerceroID", "Descripcion", valueSelected);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> MultiDropDownListNormasAsync(List<Normas> listaNormas = null)
        {
            try
            {
                if (listaNormas is null)
                    listaNormas = new List<Normas>();

                return await _iNormasCoreBusiness.MultiSelectListTodasLasNormasAsync(listaNormas);
            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<MultiSelectList> SelectListAspNetRolesAsync(string valueSelected = null, List<string> groupSelected = null, bool bitActivo = true)
        {
            try
            {
                return await _iAspNetRolesCoreBusiness.SelectListAsync(valueSelected, groupSelected, bitActivo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MultiSelectList> SelectListAspNetRolesPorTerceroAsync(int terceroID, bool bitActivo = true)
        {
            try
            {
                var listaRolesPorTerceros = await _iAspNetTerceroRolesCoreBusines.GetAllAsync();
                listaRolesPorTerceros = listaRolesPorTerceros.Where(x => x.IntTerceroID == terceroID).ToList();
                var listaRoles = await _iAspNetRolesCoreBusiness.GetAllAsync();
                listaRoles = listaRoles.Where(x => listaRolesPorTerceros.Any(z => z.IntRolID == x.Id)).ToList();

                var listaRolesSeleccionados = (from x in listaRoles
                                               select x.Id).ToList();

                return await _iAspNetRolesCoreBusiness.SelectListAsync(string.Empty, listaRolesSeleccionados, bitActivo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> SelectListNormasAsync(string valueSelected = null)
        {
            try
            {
                return await _iNormasCoreBusiness.SelectListAsync(valueSelected);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SelectList> SelectListCiudadesAsync(string valueSelected = null)
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

        public async Task<SistemasDeGestionDTO> ObtenerTerceros_SistemasDeGestionPorIDAsync(int registroID = 0)
        {
            try
            {
                return await _iTerceros_SistemasDeGestionCoreBusiness.ObtenerTerceros_SistemasDeGestionPorIDAsync(registroID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AsignarRolesAlTerceroAsync(Terceros modelo, string listarRoles)
        {
            try
            {

                var listaRolesPorTercero = await _iAspNetTerceroRolesCoreBusines.GetAllAsync();
                await _iAspNetTerceroRolesCoreBusines.DeleteRangeAsync(listaRolesPorTercero.Where(x => x.IntTerceroID == modelo.IntTerceroID));

                var listaRoles = JsonConvert.DeserializeObject<List<string>>(listarRoles);
                foreach (var item in listaRoles)
                {
                    AspNetTerceroRoles tercerosRoles = new AspNetTerceroRoles();
                    tercerosRoles.IntTerceroID = modelo.IntTerceroID;
                    tercerosRoles.IntRolID = item;

                    await _iAspNetTerceroRolesCoreBusines.SaveAllAsync(tercerosRoles);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task BloquearUsuariosTerceroAsync(List<TercerosUsuarios> listaUsuarios)
        {
            try
            {
                foreach (var item in listaUsuarios)
                {
                    if (item.OpcEstado == true)
                    {
                        item.OpcEstado = false;
                        await _iTercerosUsuariosCoreBusines.SaveAllAsync(item);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> EliminarNormaPorTerceroAsync(int registroID)
        {
            try
            {
                Terceros_Normas modelo = await _iTerceros_NormasCoreBusiness.FindAsync(x => x.IntID == registroID);

                if (modelo is null)
                    return RecursoCommon.msnRegistroNoEncontrado;

                await _iTerceros_NormasCoreBusiness.DeleteAsync(modelo);

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Terceros> ObtenerInformacionDeTerceroEnSesionAsync()
        {
            try
            {
                var terceroID = _iServicioTercero.ObtenerTerceroDeUsuarioEnSesion();

                var terceroModelo = await this.FindAsync(x => x.IntTerceroID == terceroID);

                return terceroModelo;
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

        ~TercerosCoreBusiness()
        {
            Dispose(false);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_iAspNetRolesCoreBusiness != null)
                {
                    _iAspNetRolesCoreBusiness.Dispose();
                    _iAspNetRolesCoreBusiness = null;
                }

                if (_iAspNetTerceroRolesCoreBusines != null)
                {
                    _iAspNetTerceroRolesCoreBusines.Dispose();
                    _iAspNetTerceroRolesCoreBusines = null;
                }

                if (_iCiudadesCoreBusiness != null)
                {
                    _iCiudadesCoreBusiness.Dispose();
                    _iCiudadesCoreBusiness = null;
                }

                if (_iTercerosUsuariosCoreBusines != null)
                {
                    _iTercerosUsuariosCoreBusines.Dispose();
                    _iTercerosUsuariosCoreBusines = null;
                }

                if (_iNormasCoreBusiness != null)
                {
                    _iNormasCoreBusiness.Dispose();
                    _iNormasCoreBusiness = null;
                }

                if (_iAspNetTerceroRolesCoreBusiness != null)
                {
                    _iAspNetTerceroRolesCoreBusiness.Dispose();
                    _iAspNetTerceroRolesCoreBusiness = null;
                }

                if (_iTerceros_SistemasDeGestionCoreBusiness != null)
                {
                    _iTerceros_SistemasDeGestionCoreBusiness.Dispose();
                    _iTerceros_SistemasDeGestionCoreBusiness = null;
                }

                if (_iSistemasDeGestionCoreBusiness != null)
                {
                    _iSistemasDeGestionCoreBusiness.Dispose();
                    _iSistemasDeGestionCoreBusiness = null;
                }

                if (_iTerceros_ClientesCoreBusiness != null)
                {
                    _iTerceros_ClientesCoreBusiness.Dispose();
                    _iTerceros_ClientesCoreBusiness = null;
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
