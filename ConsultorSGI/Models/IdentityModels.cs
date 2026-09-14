using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int TerceroID { get; set; }
        [StringLength(100)]
        public string NombreUsuario { get; set; }
        public DateTime? FechaIngreso { get; set; }
        [StringLength(100)]
        public string NombreImagen { get; set; }
        [StringLength(100)]
        public string NombreImagenFirma { get; set; }
        public DateTime? FechaNacimiento { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async static Task<ClaimsIdentity> CreateUserClaims(ClaimsIdentity identity, UserManager<ApplicationUser> manager, ApplicationUser currentUser)
        {

            // Your User Data
            var jUser = JsonConvert.SerializeObject(new CurrentUser
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                UsuarioNombre = currentUser.NombreUsuario,
                TerceroID = currentUser.TerceroID               

            }); 

            identity.AddClaim(new Claim(ClaimTypes.UserData, jUser));

            return await Task.FromResult(identity);
        }
    }

    public class ApplicationRole : IdentityRole
    {
        [StringLength(200)]
        public string Description { get; set; }
        public bool BitDefault { get; set; }
        public bool BitActivo { get; set; }      
    }


    public class CurrentUser
    {
        public string Id { get; set; }
        public string UsuarioNombre { get; set; }
        public string Email { get; set; }
        public string ImagenDePerfil { get; set; }
        public string UrlImagenDePerfil { get; set; }
        public string ImagenDeFirma { get; set; }
        public string UrlImagenDeFirma { get; set; }
        public int TerceroID { get; set; }
        public string TerceroIdentificacion { get; set; }
        public string TerceroNombre { get; set; }
        public string ImagenTerceroLogo { get; set; }
        public string UrlImagenTerceroLogo { get; set; }
        public bool MenuExtendido { get; set; }
        public List<ModulosDetalle> modulos { get; set; }


        public static CurrentUser Get
        {
            get
            {
                var user = HttpContext.Current.User;

                if (user == null) return null;
                else
                {
                    if (string.IsNullOrEmpty(user.Identity.GetUserId())) 
                        return null;
                }

                var jUser = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.UserData).Value;

                return JsonConvert.DeserializeObject<CurrentUser>(jUser);
            }
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        //: base("name=gestioni_consultorNetEntities")
        : base("AspNetIdentity", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
