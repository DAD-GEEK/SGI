using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public static class ServicioUsuarioModel
    {
        public static class ServicioUsuario
        {
            public static IOwinContext GetOwinContext()
            {
                var context = HttpContext.Current;
                if (context == null)
                    throw new InvalidOperationException("HttpContext.Current is null. This method can only be called from a thread that has an HttpContext.");

                var owinContext = context.GetOwinContext();
                return owinContext;
            }

            private static CurrentUser datosUsuarioEnSesion;
            private static bool menuExtendido;

            public static CurrentUser ObtenerDatosDeUsuarioEnSesion
            {
                get
                {
                    var authenticationContext = GetOwinContext().Authentication;
                    var userIdentity = authenticationContext.User;

                    if (userIdentity.Identity.IsAuthenticated)
                    {
                        var datosUsuarioEnSesionJSON = Extensions.GetClaimValue(userIdentity, userIdentity.Identity.Name);

                        if (datosUsuarioEnSesionJSON is null)
                        {
                            datosUsuarioEnSesion = ObtenerDatosDeUsuarioEnBaseDeDatos;
                            datosUsuarioEnSesionJSON = JsonConvert.SerializeObject(datosUsuarioEnSesion);
                            Extensions.AddUpdateClaim(HttpContext.Current.User, userIdentity.Identity.Name, datosUsuarioEnSesionJSON);
                            HttpCookie sessionCookie = HttpContext.Current.Response.Cookies["ASP.NET_SessionId"];
                            sessionCookie.Expires = DateTime.Now.AddHours(8);
                            HttpContext.Current.Response.SetCookie(sessionCookie);
                        }
                        else
                            datosUsuarioEnSesion = JsonConvert.DeserializeObject<CurrentUser>(datosUsuarioEnSesionJSON);
                    }

                    return datosUsuarioEnSesion;
                }
            }

            private static CurrentUser ObtenerDatosDeUsuarioEnBaseDeDatos
            {
                get
                {
                    string emailUsuarioActual = HttpContext.Current.User.Identity.Name;

                    datosUsuarioEnSesion = new CurrentUser();

                    using (var bd = new gestioni_consultorNetEntities())
                    {
                        var usuarioEnBaseDeDatos = bd.AspNetUsers
                            .Include("AspNetRoles")                            
                            .FirstOrDefault(x => x.Email == emailUsuarioActual);

                        if (usuarioEnBaseDeDatos != null)
                        {
                            datosUsuarioEnSesion.TerceroID = usuarioEnBaseDeDatos.TerceroID;
                            datosUsuarioEnSesion.Id = usuarioEnBaseDeDatos.Id;
                            datosUsuarioEnSesion.Email = emailUsuarioActual;
                            datosUsuarioEnSesion.UsuarioNombre = usuarioEnBaseDeDatos.NombreUsuario;
                            datosUsuarioEnSesion.ImagenDePerfil = usuarioEnBaseDeDatos.NombreImagen;
                            datosUsuarioEnSesion.ImagenDeFirma = usuarioEnBaseDeDatos.NombreImagenFirma;
                            datosUsuarioEnSesion.MenuExtendido = true;

                            datosUsuarioEnSesion.modulos = new List<ModulosDetalle>();

                            usuarioEnBaseDeDatos.AspNetRoles.ToList().ForEach(item =>
                            {
                                var listaModulosPorRol = item.ModulosPorRol.Select(x => new ModulosDetalle()
                                {
                                    IntModuloDetalleID = x.ModulosDetalle.IntModuloDetalleID,
                                    IntModuloID = x.ModulosDetalle.IntModuloID,
                                    StrModuloDetalle = x.ModulosDetalle.StrModuloDetalle,
                                    StrDescripcion = x.ModulosDetalle.StrDescripcion,
                                    StrControlador = x.ModulosDetalle.StrControlador,
                                    StrAccion = x.ModulosDetalle.StrAccion,
                                    StrIcono = x.ModulosDetalle.StrIcono,
                                    TIntOrden = x.ModulosDetalle.TIntOrden,
                                    DatFechaCreacion = x.ModulosDetalle.DatFechaCreacion,
                                    BitActivo = x.ModulosDetalle.BitActivo,

                                    Modulos = new Modulos()
                                    {
                                        IntModuloID = x.ModulosDetalle.Modulos.IntModuloID,
                                        StrModulo = x.ModulosDetalle.Modulos.StrModulo,
                                        StrDescripcion = x.ModulosDetalle.Modulos.StrDescripcion,
                                        StrIcono = x.ModulosDetalle.Modulos.StrIcono,
                                        TIntOrden = x.ModulosDetalle.Modulos.TIntOrden,
                                        BitActivo = x.ModulosDetalle.Modulos.BitActivo
                                    }

                                }).ToList();

                                var listaModulosNoAgregados = listaModulosPorRol.Where(x => !datosUsuarioEnSesion.modulos.Any(y => y.IntModuloDetalleID == x.IntModuloDetalleID)).ToList();

                                datosUsuarioEnSesion.modulos.AddRange(listaModulosNoAgregados);
                            });

                            var terceroModelo = bd.Terceros.FirstOrDefault(x => x.IntTerceroID == datosUsuarioEnSesion.TerceroID);

                            datosUsuarioEnSesion.TerceroIdentificacion = terceroModelo.StrIdentificacion;
                            datosUsuarioEnSesion.TerceroNombre = terceroModelo.StrNombre;

                        }
                    }

                    return datosUsuarioEnSesion;
                }
            }

            public static void RefrescarDatosDeUsuarioEnSesion()
            {
                try
                {
                    var authenticationContext = GetOwinContext().Authentication;
                    var userIdentity = authenticationContext.User;

                    var datosUsuarioEnSesionJSON = Extensions.GetClaimValue(userIdentity, userIdentity.Identity.Name);
                    var datosUsuarioEnSesionAntes = JsonConvert.DeserializeObject<CurrentUser>(datosUsuarioEnSesionJSON);

                    datosUsuarioEnSesion = ObtenerDatosDeUsuarioEnBaseDeDatos;
                    datosUsuarioEnSesion.MenuExtendido = datosUsuarioEnSesionAntes.MenuExtendido;

                    datosUsuarioEnSesionJSON = JsonConvert.SerializeObject(datosUsuarioEnSesion);
                    Extensions.AddUpdateClaim(HttpContext.Current.User, userIdentity.Identity.Name, datosUsuarioEnSesionJSON);
                }
                catch (System.Exception)
                {

                    throw;
                }
            }

            public static void ExtenderMenuDinamico()
            {
                try
                {
                    datosUsuarioEnSesion.MenuExtendido = true;
                    var datosUsuarioEnSesionJSON = JsonConvert.SerializeObject(datosUsuarioEnSesion);
                    Extensions.AddUpdateClaim(HttpContext.Current.User, datosUsuarioEnSesion.Email, datosUsuarioEnSesionJSON);

                    menuExtendido = true;
                }
                catch (System.Exception)
                {
                    menuExtendido = true;
                }
            }

            public static void ColapsarMenuDinamico()
            {
                try
                {
                    datosUsuarioEnSesion.MenuExtendido = false;
                    var datosUsuarioEnSesionJSON = JsonConvert.SerializeObject(datosUsuarioEnSesion);
                    Extensions.AddUpdateClaim(HttpContext.Current.User, datosUsuarioEnSesion.Email, datosUsuarioEnSesionJSON);

                    menuExtendido = false;
                }
                catch (System.Exception)
                {
                    menuExtendido = false;
                }
            }
        }
    }

}
