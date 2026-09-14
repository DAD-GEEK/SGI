using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class MenuDinamicoDTO
    {
        public int ModuloID { get; set; }
        public string NombreModulo { get; set; }
        public string Icono { get; set; }
        public byte? Orden { get; set; }
        public List<SubMenuDinamicoDTO> SubMenuDinamico { get; set; }


    }
}
