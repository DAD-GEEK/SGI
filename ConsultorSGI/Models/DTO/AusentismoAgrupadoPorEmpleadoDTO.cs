using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class AusentismoAgrupadoPorEmpleadoDTO
    {
        public short Año { get; set; }
        public byte Mes { get; set; }
        public string NombreMes { get; set; }
        public int AreaID { get; set; }
        public int ProcesoID { get; set; }
        public string ProcesoDescripcion { get; set; }
        public string TituloModal { get; set; }
        public string AreaDescripcion { get; set; }
        public List<AusentismoTiempos> ListaAusentismoTiempos { set; get; }
        public List<Empleados> ListaEmpleados { get; set; }
        public List<TipoEventosAusentismo> ListaTipoEventos { get; set; }
        public List<AusentismoTiemposPorEmpleado> ListaAusentismoAgrupadoPorEmpleado => ListaAusentismoTiempos.GroupBy(x => x.IntEmpleadoID).Select(x => new AusentismoTiemposPorEmpleado()
        {
            EmpleadoID = x.Key,
            UrlImagen = ListaEmpleados.Where(empleado => empleado.IntEmpleadoID == x.Key).FirstOrDefault().StrRutaImagen,
            EmpleadoIdentificacion = ListaEmpleados.Where(empleado => empleado.IntEmpleadoID == x.Key).FirstOrDefault().StrIdentificacion,
            EmpleadoNombre = ListaEmpleados.Where(empleado => empleado.IntEmpleadoID == x.Key).FirstOrDefault().StrNombreCompleto,        
            TipoEventos = ListaAusentismoTiempos.Where(empleado => empleado.IntEmpleadoID == x.Key).GroupBy(y => y.IntTipoEventoAusentismoID).Select(y => new TipoEventosAusentismoDTO()
            {
                IntTipoEventoAusentismoID = y.Key,
                StrCodigo = ListaTipoEventos.Where(evento => evento.IntTipoEventoAusentismoID == y.Key).FirstOrDefault().StrCodigo,
                DiasIncapacidad = ListaAusentismoTiempos.Where(tiempo => tiempo.IntTipoEventoAusentismoID == y.Key).Sum(suma => suma.IntDiasIncapacidad)

            }).ToList()

        }).ToList();
    }

    public class AusentismoTiemposPorEmpleado
    {
        public int EmpleadoID { get; set; }
        public string UrlImagen { get; set; }
        public string EmpleadoIdentificacion { get; set; }
        public string EmpleadoNombre { get; set; }
        public List<TipoEventosAusentismoDTO> TipoEventos { get; set; }

    }
}
