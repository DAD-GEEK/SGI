using System.Collections.Generic;
using System.Linq;

namespace Models.DTO
{
    public class FasesDTO : Fases
    {
        public List<DocumentosDiagnosticoPasosDTO> PasosDocumento { get; set; }
        public decimal DecNivelDeCumplimiento
        {
            get
            {
                decimal resultado = decimal.Zero;

                try
                {
                    if (PasosDocumento != null)
                    {
                        if (PasosDocumento.Where(x => x.StrFaseID == StrFaseID).Count() != 0)
                        {
                            if (PasosDocumento.Where(x => x.StrFaseID == StrFaseID).Sum(x => x.IntCantidadSICumple) != 0)
                            {
                                decimal cantidadSIAplica = PasosDocumento.Where(x => x.StrFaseID == StrFaseID).Sum(x => x.IntCantidadSIAplica);
                                decimal cantidadSICumple = PasosDocumento.Where(x => x.StrFaseID == StrFaseID).Sum(x => x.IntCantidadSICumple);
                                resultado = (decimal)cantidadSICumple / (decimal)cantidadSIAplica;
                            }

                        }
                    }
                }
                catch (System.Exception)
                { }


                return resultado;
            }
        }
        public string StrNivelDeCumplimiento => ((decimal)DecNivelDeCumplimiento * (decimal)100).ToString("N0");
        public GraficosChartDTO GraficaDeRequisitos { get; set; }

    }
}
