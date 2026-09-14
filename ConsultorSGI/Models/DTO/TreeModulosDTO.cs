using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class TreeModulosDTO
    {
        public string id { get; set; }

        public string text { get; set; }

        public bool @checked { get; set; }

        public bool hasChildren { get; set; }
        public virtual List<TreeModulosDTO> children { get; set; }
    }
}
