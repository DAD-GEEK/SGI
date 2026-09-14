using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBusiness.Interfaces
{
    public interface IEmailCoreBusiness : IDisposable
    {
        string EmailBody(string Plantilla, Dictionary<string, string> Parametros, string RutaPlantilla = null);
        Task EnviarEmailAsync(string Receptor, string Asunto, string Body, List<string> Adjuntos = null, bool IsBodyHtml = true, string Copia = null);
    }
}
