using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace Models
{
    public partial class gestioni_consultorNetEntities
    {
        IServicioTercero _iServicioTercero;
        public gestioni_consultorNetEntities(IServicioTercero servicioTercero)
        {
            _iServicioTercero = servicioTercero;
        }

        public override int SaveChanges()
        {
            if (_iServicioTercero != null)
            {
                var terceroID = _iServicioTercero.ObtenerTerceroDeUsuarioEnSesion();

                foreach (var entidad in this.ChangeTracker.Entries().Where(e => e.Entity is IEntidadPorTercero && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as IEntidadPorTercero))
                    entidad.IntTerceroID = terceroID;
            }
            
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            if (_iServicioTercero != null)
            {
                var terceroID = _iServicioTercero.ObtenerTerceroDeUsuarioEnSesion();

                foreach (var entidad in this.ChangeTracker.Entries().Where(e => e.Entity is IEntidadPorTercero && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as IEntidadPorTercero))
                    entidad.IntTerceroID = terceroID;
            }           

            return base.SaveChangesAsync();
        }


    }

    public static class Extensions
    {
        public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return;

            var usuario = identity.GetUserName();

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        }

        public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);

            // ?. prevents a exception if claim is null.
            return claim?.Value;
        }
    }

}
