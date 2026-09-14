using Across;
using Across.ArchivosDeRecurso;
using CoreBusiness.Interfaces;
using DataAccess.Servicios;
using Models;
using Models.DTO;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoreBusiness
{
    public class ManageCoreBusiness : IManageCoreBusiness, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        private IAspNetUsersCoreBusiness _iAspNetUsersCoreBusiness;
        private ITercerosUsuariosCoreBusiness _iTercerosUsuariosCoreBusiness;

        public ManageCoreBusiness()
        {
            this._iAspNetUsersCoreBusiness = new AspNetUsersCoreBusiness();
            this._iTercerosUsuariosCoreBusiness = new TercerosUsuariosCoreBusiness();
        }

        public async Task<string> UpdateDatosDePerfilDeUsuarioAsync(AspNetUsers model)
        {
            try
            {
                var usuarioModel = await _iAspNetUsersCoreBusiness.FindAsync(x => x.UserName == model.UserName);

                if (usuarioModel == null)
                    return String.Format(RecursoManage.msnUsuarioNoExiste, model.UserName);


                if (usuarioModel.NombreUsuario != model.NombreUsuario || usuarioModel.PhoneNumber != model.PhoneNumber)
                {
                    usuarioModel.NombreUsuario = model.NombreUsuario.ToUpper();
                    usuarioModel.PhoneNumber = model.PhoneNumber;
                    var respuestaAspNetUsers = await _iAspNetUsersCoreBusiness.UpdateAsync(usuarioModel);
                }

                var terceroUsuario = await _iTercerosUsuariosCoreBusiness.FindAsync(x => x.StrUsuarioEmail == model.UserName);
                if (terceroUsuario == null)
                    return String.Format(RecursoManage.msnTerceroUsuarioNoExiste, model.UserName);

                if (terceroUsuario.StrUsuarioNombre != model.NombreUsuario)
                {
                    terceroUsuario.StrUsuarioNombre = model.NombreUsuario;
                    var respuestaTerceroUsuario = await _iTercerosUsuariosCoreBusiness.SaveAllAsync(terceroUsuario);
                }

                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GuardarImagenAsyn(HttpFileCollectionBase imagenUsuario, FormCollection collection)
        {
            try
            {
                if (imagenUsuario.Count == 0)
                    return RecursoManage.msnPerfilSinImagen;

                var userName = collection["userName"].ToString();
                var isAvatar = Convert.ToBoolean(collection["isUserImageAvatar"]);
                var isUserImageAvatar = Convert.ToBoolean(collection["isUserImageAvatar"]);
                var usuarioModel = await _iAspNetUsersCoreBusiness.FindAsync(x => x.UserName == userName);

                if (usuarioModel is null)
                    return String.Format(RecursoUsuarios.msnUsuarioNoEncontrado, userName);

                var rutaImagen = string.Empty;
                var rutaImagenAnterior = string.Empty;

                if (isAvatar)
                {
                    rutaImagen = HttpContext.Current.Server.MapPath($"~/{Archivos.rutaImagenUsuarioAvatar}");
                    rutaImagenAnterior = Archivos.ObtenerRutaDeImagenUsuarioAvatar(usuarioModel.NombreImagen);
                }
                else
                {
                    rutaImagen = HttpContext.Current.Server.MapPath($"~/{Archivos.rutaImagenUsuarioFirma}");
                    rutaImagenAnterior = Archivos.ObtenerRutaDeImagenUsuarioFirma(usuarioModel.NombreImagenFirma);
                }

                Archivos.EliminarArchivo(rutaImagenAnterior);

                ParamFilesDTO parametros = new ParamFilesDTO();
                parametros.ruta = rutaImagen;
                parametros.nombreArchivo = $"{usuarioModel.Email}";

                var nombreImagen = Archivos.GuardarArchivo(imagenUsuario, parametros);

                if (!string.IsNullOrEmpty(rutaImagen))
                {
                    if (isAvatar)
                        usuarioModel.NombreImagen = nombreImagen;
                    else
                        usuarioModel.NombreImagenFirma = nombreImagen;

                    await _iAspNetUsersCoreBusiness.UpdateAsync(usuarioModel);
                    return rutaImagen;
                }

                return string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> EliminarImagenUsuarioAsync(string userName, bool isAvatar)
        {
            try
            {
                AspNetUsers usuario = await _iAspNetUsersCoreBusiness.FindAsync(x => x.UserName == userName);

                if (usuario is null)
                    return String.Format(RecursoUsuarios.msnUsuarioNoEncontrado, userName);

                string rutaImagen = string.Empty;

                if (isAvatar)
                    rutaImagen = Archivos.ObtenerRutaDeImagenUsuarioAvatar(usuario.NombreImagen);
                else
                    rutaImagen = Archivos.ObtenerRutaDeImagenUsuarioAvatar(usuario.NombreImagenFirma);

                var respuestaEliminar = Archivos.EliminarArchivo(rutaImagen);

                if (string.IsNullOrEmpty(respuestaEliminar))
                {
                    if (isAvatar)
                        usuario.NombreImagen = string.Empty;
                    else
                        usuario.NombreImagenFirma = string.Empty;

                    await _iAspNetUsersCoreBusiness.UpdateAsync(usuario);
                    return string.Empty;
                }

                return respuestaEliminar;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public long ObtenerTamanoDeArchivo(string nombreArchivo)
        {
            try
            {
                return Archivos.ObtenerTamanoDeArchivo(nombreArchivo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ObtenerRutaDeImagenUsuarioAvatar(string nombreImagen)
        {
            try
            {
                return Archivos.ObtenerRutaDeImagenUsuarioAvatar(nombreImagen);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ObtenerRutaDeImagenUsuarioFirma(string nombreImagen)
        {
            try
            {
                return Archivos.ObtenerRutaDeImagenUsuarioFirma(nombreImagen);
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

        ~ManageCoreBusiness()
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
