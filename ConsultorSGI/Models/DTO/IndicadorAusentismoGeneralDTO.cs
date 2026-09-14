namespace Models.DTO
{
    public class IndicadorAusentismoGeneralDTO
    {
        public int Anio { get; set; }
        public int NumeroTotalIncapacidades { get; set; }
        public int NumeroTotalDias { get; set; }
        public int NumeroIncapacidadesPorEnfermedadGeneral_AccidenteComun { get; set; }
        public int NumeroDiasPorEnfermedadGeneral_AccidenteComun { get; set; }
        public string backgroundColor { get; set; }
    }
}
