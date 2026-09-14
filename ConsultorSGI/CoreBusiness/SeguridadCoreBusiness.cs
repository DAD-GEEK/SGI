using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace CoreBusiness
{
    public class SeguridadCoreBusiness : ISeguridadCoreBusiness
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        ITercerosUsuariosCoreBusiness _iTercerosUsuariosCoreBusiness;
        IAspNetUsersCoreBusiness _iAspNetUsersCoreBusiness;
        IAspNetRolesCoreBusiness _iAspNetRolesCoreBusiness;
        IModulosCoreBusiness _iModulosCoreBusiness;
        IModulosDetalleCoreBusiness _iModulosDetalleCoreBusiness;
        IModulosPorRolCoreBusiness _iModulosPorRolCoreBusiness;
        ITercerosCoreBusiness _iTercerosCoreBusiness;

        public SeguridadCoreBusiness()
        {
            _iAspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();
            _iTercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();
            _iAspNetRolesCoreBusiness = new AspNetRolesCoreBusiness();
            _iModulosPorRolCoreBusiness = new ModulosPorRolCoreBusiness();
            _iTercerosCoreBusiness = new TercerosCoreBusiness();
            _iModulosCoreBusiness = new ModulosCoreBusiness();
            _iModulosDetalleCoreBusiness = new ModulosDetalleCoreBusiness();
        }

        public async Task<string> ValidarInicioSesion(string emailUsuario)
        {
            try
            {
                var modeloUsuario = await _iAspNetUsersCoreBusiness.FindAsync(x => x.Email == emailUsuario);
                if (modeloUsuario != null)
                {
                    if (modeloUsuario.LockoutEnabled == true && modeloUsuario.LockoutEndDateUtc > DateTime.Now) return RecursoUsuarios.msnUsuarioBloqueado;

                    var usuarioAsignado = await this.UsuarioAsignadoATercero(emailUsuario);
                    if (usuarioAsignado) return string.Empty;

                    return RecursoUsuarios.msnUsuarioSinAsignar;
                }

                return RecursoUsuarios.msnUsuarioInvalido;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ValidarSiEsGestionIntegralAsync(int terceroID)
        {
            try
            {
                var terceroModelo = await _iTercerosCoreBusiness.FindAsync(x => x.IntTerceroID == terceroID);

                if (terceroModelo is null)
                    return false;

                if (terceroModelo.StrIdentificacion.Trim() == Empresa.Nit.Trim())
                    return true;

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TercerosUsuarios> GetTerceroUsuarioByEmail(string email)
        {
            try
            {
                return await _iTercerosUsuariosCoreBusiness.FindAsync(x => x.StrUsuarioEmail == email);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UsuarioAsignadoATercero(string email)
        {
            try
            {
                var usuario = await _iTercerosUsuariosCoreBusiness.FindAsync(x => x.StrUsuarioEmail == email && x.OpcEstado == true);
                if (usuario != null) return true;

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ExisteUsuario(string email)
        {
            try
            {
                var existeUsuario = await _iAspNetUsersCoreBusiness.FindAsync(x => x.Email == email);
                if (existeUsuario != null) return RecursoUsuarios.msnUsuarioExiste;

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> ValidacionesConfirmacionLoginExterno(string email)
        {
            try
            {
                var terceroUsuario = await _iTercerosUsuariosCoreBusiness.FindAsync(x => x.StrUsuarioEmail == email && x.OpcEstado == true);
                if (terceroUsuario == null) return RecursoUsuarios.msnUsuarioSinAsignar;

                var mensajeExisteUsuario = await this.ExisteUsuario(email);
                if (!string.IsNullOrEmpty(mensajeExisteUsuario)) return mensajeExisteUsuario;

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TercerosUsuarios> ObtenerUsuarioTerceroAsync(string email)
        {
            try
            {
                return await _iTercerosUsuariosCoreBusiness.FindAsync(x => x.StrUsuarioEmail == email);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TercerosUsuariosDTO>> ObtenerListaUsuariosByTercerosDTO()
        {
            try
            {
                var listaUsuariosByTercerosDTO = new List<TercerosUsuariosDTO>();

                var listaUsuariosByTerceros = await _iTercerosUsuariosCoreBusiness.GetAllAsync();
                var listaUsuarios = await _iAspNetUsersCoreBusiness.GetAllAsync();

                foreach (var item in listaUsuariosByTerceros)
                {
                    var aspNetUser = listaUsuarios.Find(x => x.Email == item.StrUsuarioEmail);

                    var usuarioByTercero = new TercerosUsuariosDTO();

                    usuarioByTercero.IntRegistroID = item.IntRegistroID;
                    usuarioByTercero.StrUsuarioNombre = item.StrUsuarioNombre;
                    usuarioByTercero.StrUsuarioEmail = item.StrUsuarioEmail;
                    usuarioByTercero.StrTerceroIdentificacion = item.Terceros.StrIdentificacion;
                    usuarioByTercero.StrTerceroNombre = item.Terceros.StrNombre;
                    usuarioByTercero.StrNombreImagen = item.Terceros.StrRutaImagen;
                    usuarioByTercero.OpcEstado = item.OpcEstado;

                    if (aspNetUser != null)
                    {
                        usuarioByTercero.IntUsuarioID = aspNetUser.Id;                      
                        usuarioByTercero.StrTelefono = aspNetUser.PhoneNumber;
                        usuarioByTercero.StrEmailConfirmado = aspNetUser.EmailConfirmed;
                        usuarioByTercero.StrFechaIngreso = aspNetUser.FechaIngreso != null ? aspNetUser.FechaIngreso.Value.ToString("yyyy-MM-dd") : string.Empty;
                        usuarioByTercero.BitRegistrado = aspNetUser.Id != null ? true : false;
                    }

                    listaUsuariosByTercerosDTO.Add(usuarioByTercero);

                }

                return listaUsuariosByTercerosDTO;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task BloquearAspNetUser(AspNetUsers modelo)
        {
            try
            {
                modelo.LockoutEndDateUtc = DateTime.Now.AddYears(100);
                await _iAspNetUsersCoreBusiness.UpdateAsync(modelo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DesBloquearAspNetUser(AspNetUsers modelo)
        {
            try
            {
                modelo.LockoutEndDateUtc = DateTime.Now;
                await _iAspNetUsersCoreBusiness.UpdateAsync(modelo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Menú
        public async Task<List<TreeModulosDTO>> GetTreePermisosByRoles(string rolID)
        {
            try
            {
                var listaModulos = await _iModulosCoreBusiness.GetAllAsync();

                var records = listaModulos.OrderBy(x => x.TIntOrden).Select(x => new TreeModulosDTO
                {
                    id = x.IntModuloID.ToString(),
                    text = x.StrModulo,
                    @checked = false,
                    children = GetDetalleModulosTree(x.ModulosDetalle.ToList(), x.IntModuloID, rolID)
                }).ToList();

                return records;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<TreeModulosDTO> GetDetalleModulosTree(List<ModulosDetalle> modulosDetalle, int ModuloID, string rolID)
        {
            try
            {
                return modulosDetalle.Where(x => x.IntModuloID == ModuloID && x.BitActivo == true).OrderBy(x => x.TIntOrden)
                    .Select(x => new TreeModulosDTO
                    {
                        id = ModuloID.ToString() + "-" + x.IntModuloDetalleID.ToString(),
                        text = x.StrModuloDetalle,
                        @checked = ValidarModuloDetallePorRol(rolID, x.IntModuloDetalleID)
                    }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ValidarModuloDetallePorRol(string rolID, int moduloDetalleID)
        {
            try
            {
                _iModulosPorRolCoreBusiness = new ModulosPorRolCoreBusiness();
                return _iModulosPorRolCoreBusiness.GetAll().Any(x => x.IntRolID == rolID && x.IntModuloDetalleID == moduloDetalleID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarPermisosPorRolAsync(List<string> listaPermisos, string rolID)
        {
            try
            {
                await this.EliminarPermisosPorRolAsync(rolID);

                if (listaPermisos != null)
                {
                    foreach (var item in listaPermisos)
                    {
                        if (item.Contains("-"))
                        {
                            var arrayItem = item.Split('-');
                            var moduloPrincipalID = arrayItem[0];
                            var moduloDetalleID = arrayItem[1];

                            ModulosPorRol moduloRol = new ModulosPorRol();
                            moduloRol.IntModuloDetalleID = Convert.ToInt32(moduloDetalleID);
                            moduloRol.IntRolID = rolID;

                            await _iModulosPorRolCoreBusiness.SaveAllAsync(moduloRol);
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task EliminarPermisosPorRolAsync(string rolID)
        {
            try
            {
                var listaRolesPorModulo = _iModulosPorRolCoreBusiness.GetAll().Where(x => x.IntRolID == rolID).ToList();
                foreach (var item in listaRolesPorModulo)
                {
                    await _iModulosPorRolCoreBusiness.DeleteAsync(item);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Menú dinámico
        public async Task<DatosUsuarioEnSesionDTO> GetMenuDinamicoAsync(string usuarioID, IList<string> rolesPorUsuario)
        {
            try
            {
                var datosUsuarioEnSesion = new DatosUsuarioEnSesionDTO();
                datosUsuarioEnSesion.MenuDinamico = new List<MenuDinamicoDTO>();

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var modeloUsuario = await _iAspNetUsersCoreBusiness.FindAsync(x => x.Id == usuarioID);
                    var modeloTercero = await _iTercerosCoreBusiness.FindAsync(x => x.IntTerceroID == modeloUsuario.TerceroID);

                    if (modeloTercero != null && modeloUsuario != null)
                    {
                        datosUsuarioEnSesion.TerceroID = modeloTercero.IntTerceroID;
                        datosUsuarioEnSesion.TerceroNombre = modeloTercero.StrNombre;
                        datosUsuarioEnSesion.TerceroImagen = modeloTercero.StrRutaImagen;
                        datosUsuarioEnSesion.UsuarioID = usuarioID;
                        datosUsuarioEnSesion.UsuarioNombre = modeloUsuario.NombreUsuario;
                        datosUsuarioEnSesion.UsuarioEmail = modeloUsuario.Email;
                        datosUsuarioEnSesion.UsuarioImagen = modeloTercero.StrRutaImagen;
                        datosUsuarioEnSesion.MenuDinamico = await this.ObtenerMenuDinamicoAsync(rolesPorUsuario);
                    }

                }

                return datosUsuarioEnSesion;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<MenuDinamicoDTO>> ObtenerMenuDinamicoAsync(IList<string> rolesPorUsuario)
        {
            try
            {
                var listaMenu = new List<MenuDinamicoDTO>();

                var listaModulosDetallePorRol = this.ObtenerModulosPorUsuario(rolesPorUsuario.ToArray());
                var listaModulosPrincipal = _iModulosCoreBusiness.GetAll().Where(x => listaModulosDetallePorRol.Any(z => z.IntModuloID == x.IntModuloID)).ToList();

                foreach (var item in listaModulosPrincipal)
                {
                    var detallePorModulo = item.ModulosDetalle.Where(x => listaModulosDetallePorRol.Any(z => z.IntModuloID == x.IntModuloID && z.IntModuloDetalleID == x.IntModuloDetalleID)).ToList();

                    var menuDinamico = new MenuDinamicoDTO();
                    menuDinamico.ModuloID = item.IntModuloID;
                    menuDinamico.NombreModulo = item.StrModulo;
                    menuDinamico.Icono = item.StrIcono;
                    menuDinamico.Orden = item.TIntOrden;
                    menuDinamico.SubMenuDinamico = await this.ObtenerSubMenuDinamicoAsync(detallePorModulo);

                    listaMenu.Add(menuDinamico);
                }

                return listaMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SubMenuDinamicoDTO>> ObtenerSubMenuDinamicoAsync(List<ModulosDetalle> listaModulosDetalle)
        {
            try
            {
                var listaSubMenu = new List<SubMenuDinamicoDTO>();

                foreach (var item in listaModulosDetalle)
                {

                    var subMenuDinamico = new SubMenuDinamicoDTO();
                    subMenuDinamico.ModuloID = (int)item.IntModuloID;
                    subMenuDinamico.NombreModulo = item.StrModuloDetalle;
                    subMenuDinamico.Icono = item.StrIcono;
                    subMenuDinamico.Orden = item.TIntOrden;
                    subMenuDinamico.Controlador = item.StrControlador;
                    subMenuDinamico.Accion = item.StrAccion;

                    listaSubMenu.Add(subMenuDinamico);
                }

                return listaSubMenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ModulosDetalle> ObtenerModulosPorUsuario(string[] rolesPorUsuario)
        {
            try
            {
                List<ModulosDetalle> listaModulosDetallePorRol = new List<ModulosDetalle>();

                foreach (var rol in rolesPorUsuario)
                {
                    var modeloRol = _iAspNetRolesCoreBusiness.GetAll().Where(x => x.Name == rol).FirstOrDefault();
                    var listaDetalleModulosPorRol = _iModulosPorRolCoreBusiness.GetAll().Where(x => x.IntRolID == modeloRol.Id).ToList();
                    var listaModeloDetalleModulos = _iModulosDetalleCoreBusiness.GetAll().Where(x => listaDetalleModulosPorRol.Any(z => z.IntModuloDetalleID == x.IntModuloDetalleID)).ToList();
                    listaModulosDetallePorRol.AddRange(listaModeloDetalleModulos);
                }

                return listaModulosDetallePorRol.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #endregion

        #region Claims
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SeguridadCoreBusiness()
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

                if (_iTercerosUsuariosCoreBusiness != null)
                {
                    _iTercerosUsuariosCoreBusiness.Dispose();
                    _iTercerosUsuariosCoreBusiness = null;
                }

                if (_iAspNetRolesCoreBusiness != null)
                {
                    _iAspNetRolesCoreBusiness.Dispose();
                    _iAspNetRolesCoreBusiness = null;
                }

                if (_iModulosCoreBusiness != null)
                {
                    _iModulosCoreBusiness.Dispose();
                    _iModulosCoreBusiness = null;
                }

                if (_iModulosDetalleCoreBusiness != null)
                {
                    _iModulosDetalleCoreBusiness.Dispose();
                    _iModulosDetalleCoreBusiness = null;
                }

                if (_iModulosPorRolCoreBusiness != null)
                {
                    _iModulosPorRolCoreBusiness.Dispose();
                    _iModulosPorRolCoreBusiness = null;
                }

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
