using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class EmpleadosDTO
    {
        public int NumeroRegistro { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El campo {0} no debe ser mayor a {1}.")]
        public int TerceroID { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Identificacion { get; set; }

        public int TipoIdentificacionID { get; set; }
        [StringLength(2, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string TipoIdentificacion { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string PrimerApellido { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string SegundoApellido { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string PrimerNombre { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string SegundoNombre { get; set; }

        public string NombreCompleto => $"{PrimerApellido} {SegundoApellido} {PrimerNombre} {SegundoNombre}";

        [StringLength(1, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Genero { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Nacionalidad { get; set; }

        public int CentroDeTrabajoID { get; set; }

        [StringLength(5, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string CentroDeTrabajo { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaDeNacimiento { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaDeIngreso { get; set; }

        public int CiudadNacimientoID { get; set; }
        [StringLength(5, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string CiudadNacimiento { get; set; }

        public int CiudadLaboralID { get; set; }
        [StringLength(5, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string CiudadLaboral { get; set; }

        public int CiudadResidenciaID { get; set; }
        [StringLength(5, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string CiudadResidencia { get; set; }

        [StringLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string DireccionResidencia { get; set; }

        [StringLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Telefono { get; set; }

        [StringLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Celular { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Email { get; set; }

        public int CargoID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Cargo { get; set; }

        public int TurnoID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Turno { get; set; }

        public int AreaID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Area { get; set; }

        public int ProcesoID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Proceso { get; set; }

        public int ARLID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string ARL { get; set; }

        public int EPSID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string EPS { get; set; }

        public int AFPID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string AFP { get; set; }

        public int TipoContratoID { get; set; }
        [StringLength(20, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string TipoContrato { get; set; }

        [Range(0, 99999999, ErrorMessage = "El campo {0} no debe ser mayor a {1}.")]
        public int Salario { get; set; }

        public int EscolaridadID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Escolaridad { get; set; }

        public int EstadoCivilID { get; set; }
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string EstadoCivil { get; set; }

        [Range(0, 20, ErrorMessage = "El campo {0} no debe ser mayor a {1}.")]
        public byte NumeroHijos { get; set; }

        [Range(0, 20, ErrorMessage = "El campo {0} no debe ser mayor a {1}.")]
        public byte PersonasACargo { get; set; }

        [Range(0, 6, ErrorMessage = "El campo {0} no debe ser mayor a {1}.")]
        public byte Estrato { get; set; }

        [StringLength(3, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string GrupoSanquineo { get; set; }

        [StringLength(1000, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Condiciones { get; set; }

        [StringLength(1000, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Alergias { get; set; }

        [StringLength(1000, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Medicinas { get; set; }

        [StringLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Contacto { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string Parentesco { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres. ")]
        public string TelefonoContacto { get; set; }

        public bool Activo { get; set; }
        public string ErrorArchivoExcel = string.Empty;
    }
}
