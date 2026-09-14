using System.Linq;

namespace Models.DTO
{
    public class AusentismoCostosDTO
    {
        public Ausentismo Ausentismo { get; set; }
        public bool EsAccidenteDetrabajo => (Ausentismo.TipoEventosAusentismo.StrCodigo == "AT" || Ausentismo.TipoEventosAusentismo.StrCodigo == "EL") ? true : false;
        public int? Salario => Ausentismo.Empleados.IntSalario;
        public int? SalarioBaseDia => Salario / 30;
        public int TotalDiasIncapacidad => Ausentismo.AusentismoTiempos.Sum(x => x.IntDiasIncapacidad) + Ausentismo.AusentismoTiempos.Sum(x => x.IntDiasProrroga);
        public int DiasIncapacidad => Ausentismo.AusentismoTiempos.Sum(x => x.IntDiasIncapacidad);
        public int DiasProrroga => Ausentismo.AusentismoTiempos.Sum(x => x.IntDiasProrroga);
        public int DiasAsegurados => (EsAccidenteDetrabajo == true ? TotalDiasIncapacidad - 1 : TotalDiasIncapacidad - 3) < 0 ? 0 : (EsAccidenteDetrabajo == true) ? TotalDiasIncapacidad - 1 : TotalDiasIncapacidad - 3;
        public double? CostosAsegurados => EsAccidenteDetrabajo == true ? DiasAsegurados * SalarioBaseDia : DiasAsegurados * SalarioBaseDia * 0.66;
        public int DiasAsumidos => TotalDiasIncapacidad - DiasAsegurados;
        public int? CostosAsumidos => SalarioBaseDia * DiasAsumidos;

    }
}
