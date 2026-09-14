using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Web
{
    [HubName("trackingOnlineUsers")]

    public class TrackingOnlineUsers : Hub
    {
        static List<UsuariosConectadosDTO> signalRUsers = new List<UsuariosConectadosDTO>();

        public override Task OnConnected()
        {
            var datosDeUsuario = CoreBusiness.Servicios.ServicioUsuarioCoreBusiness.ObtenerDatosDeUsuarioEnSesion;

            if (!string.IsNullOrEmpty(datosDeUsuario.UsuarioNombre))
            {
                if (signalRUsers.Count(x => x.UserName == datosDeUsuario.UsuarioNombre) == 0)
                {
                    signalRUsers.Add(new UsuariosConectadosDTO { ConnectionId = Context.ConnectionId, UserName = datosDeUsuario.UsuarioNombre, Email = datosDeUsuario.Email, Image = datosDeUsuario.UrlImagenDePerfil, TerceroNombre = datosDeUsuario.TerceroNombre, FechaConectado = Across.Common.ObtenerFechaLarga(Across.Common.ObtenerFechaActualExacta()) });
                }
            }
            var context = GlobalHost.ConnectionManager.GetHubContext<TrackingOnlineUsers>();

            context.Clients.All.online(signalRUsers);


            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            if (HttpContext.Current != null)
            {
                var datosDeUsuario = CoreBusiness.Servicios.ServicioUsuarioCoreBusiness.ObtenerDatosDeUsuarioEnSesion;

                if (signalRUsers.FirstOrDefault(x => x.Email == datosDeUsuario.Email) is null)
                {
                    if (signalRUsers.Count(x => x.UserName == datosDeUsuario.UsuarioNombre) == 0)
                    {
                        signalRUsers.Add(new UsuariosConectadosDTO { ConnectionId = Context.ConnectionId, UserName = datosDeUsuario.UsuarioNombre, Email = datosDeUsuario.Email, Image = datosDeUsuario.UrlImagenDePerfil, FechaConectado = Across.Common.ObtenerFechaLarga(Across.Common.ObtenerFechaActualExacta()) });
                    }

                    var context = GlobalHost.ConnectionManager.GetHubContext<TrackingOnlineUsers>();

                    context.Clients.All.online(signalRUsers);
                    return base.OnReconnected();
                }
            }

            return Task.CompletedTask;
        }

        public static void EliminarUsuarioConectado(string Usuario)
        {
            var item = signalRUsers.FirstOrDefault(x => x.Email == Usuario);

            if (item != null)
                signalRUsers.Remove(item);

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TrackingOnlineUsers>();
            context.Clients.All.online(signalRUsers);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = signalRUsers.FirstOrDefault(x => x.UserName == Context.User.Identity.Name);
            if (item != null)
            {
                signalRUsers.Remove(item);

                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TrackingOnlineUsers>();
                context.Clients.All.online(signalRUsers);

            }

            return base.OnDisconnected(stopCalled);

        }
    }
}