using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Hubs
{
    public class AuditoriaHub : Hub
    {

        public void NotificarCambiosEnFortalezas(int auditoriaID, string name, string text, bool finalice)
        {
            Clients.All.sendNotificationFortalezas(auditoriaID, name, text, finalice);
        }

        public void NotificarCambiosEnConclusiones(int auditoriaID, string name, string text, bool finalice)
        {
            Clients.All.sendNotificationConclusiones(auditoriaID, name, text, finalice);
        }

        public void CambiarEstadoAuditoria(int auditoriaID, bool estado)
        {
            Clients.All.changeEstateAuditoria(auditoriaID, estado);
        }
    }
}