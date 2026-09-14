using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class UsuariosConectadosDTO
    {
        public string ConnectionId { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string TerceroNombre { get; set; }
        public string FechaConectado { get; set; }
    }
}
