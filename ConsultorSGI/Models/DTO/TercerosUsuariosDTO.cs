namespace Models.DTO
{
    public class TercerosUsuariosDTO : TercerosUsuarios
    {
        public int IntRegistroID { get; set; }
        public string IntUsuarioID { get; set; }
        public bool StrEmailConfirmado { get; set; }
        public string StrTelefono { get; set; }
        public string StrFechaIngreso { get; set; }
        public string StrTerceroIdentificacion { get; set; }
        public string StrTerceroNombre { get; set; }
        public string StrNombreImagen { get; set; }
        public bool BitRegistrado { get; set; }
    }
}
