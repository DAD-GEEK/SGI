using System.Collections.Generic;

namespace Models.DTO
{
    public class DatosUsuarioEnSesionDTO
    {
        public int TerceroID { get; set; }
        public string TerceroNombre { get; set; }
        public string TerceroImagen { get; set; }
        public string UsuarioID { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioEmail { get; set; }
        public string UsuarioImagen { get; set; }
        public List<MenuDinamicoDTO> MenuDinamico { get; set; }

    }
}
