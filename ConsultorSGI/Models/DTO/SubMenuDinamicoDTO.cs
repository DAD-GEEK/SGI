using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class SubMenuDinamicoDTO: MenuDinamicoDTO
    {
        public string Controlador { get; set; }
        public string Accion { get; set; }
    }
}
