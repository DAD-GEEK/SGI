using Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace CoreBusiness
{
    public class EmailCoreBusiness : IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        ConfiguracionCoreBusiness _configuracionCoreBusiness;

        public Task EnviarEmailAsync(string Receptor, string Asunto, string Body, byte[] Adjuntos = null, bool IsBodyHtml = true, string Copia = null, Actas Acta = null)
        {
            try
            {
                _configuracionCoreBusiness = new ConfiguracionCoreBusiness();

                var configuracion = _configuracionCoreBusiness.GetAll().FirstOrDefault();

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(configuracion.StrSMPTEmail.Trim());
                    mailMessage.Subject = Asunto.Trim();
                    mailMessage.Body = Body;
                    mailMessage.IsBodyHtml = IsBodyHtml;

                    if (Copia != null) mailMessage.CC.Add(new MailAddress(Copia));
                    
                    foreach (var item in Receptor.Trim().Split(';'))
                    {
                        mailMessage.To.Add(new MailAddress(item));
                    }

                    if (Adjuntos != null)
                    {
                        string nombrePDF = "Acta N° " + Acta.IntConsecutivo + "_" + Acta.Agenda.Clientes.StrIdentificacion + ".pdf";
                        MemoryStream ActaPDF = new MemoryStream(Adjuntos);
                        mailMessage.Attachments.Add(new Attachment(ActaPDF, nombrePDF, "application/pdf"));
                    }

                    SmtpClient smtp = new SmtpClient
                    {
                        Host = configuracion.StrSMTPHost,
                        EnableSsl = Convert.ToBoolean(configuracion.OpcSMTPSSL)
                    };
                    NetworkCredential NetworkCred = new NetworkCredential
                    {
                        UserName = configuracion.StrSMPTEmail.Trim(),
                        Password = configuracion.StrSMTPPassword
                    };
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = (int)configuracion.StrSMTPPort;
                    smtp.Send(mailMessage);
                    return Task.FromResult(0);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string EmailBody(string Plantilla, Dictionary<string, string> Parametros, string RutaPlantilla = null)
        {
            try
            {
                string body = string.Empty;
                string pathEmailFolder = ConfigurationManager.AppSettings["pathTemplateEmailFolder"];               

                if (!string.IsNullOrEmpty((Plantilla)))
                {
                    if (RutaPlantilla != null)
                    {
                        var pathFullFinal = Path.Combine(pathEmailFolder, Plantilla);
                        if (File.Exists(pathFullFinal))
                        {
                            body = File.ReadAllText(pathFullFinal);
                            if (Parametros != null)
                            {
                                foreach (var tag in Parametros)
                                {
                                    body = body.Replace(tag.Key, tag.Value);
                                }
                            }
                        }
                    }
                    else
                    {
                        var pathTemplates = HttpContext.Current.Server.MapPath(pathEmailFolder);
                        var pathFulls = Path.Combine(pathTemplates, Plantilla);
                        if (File.Exists(pathFulls))
                        {
                            body = File.ReadAllText(pathFulls);
                            if (Parametros != null)
                            {
                                foreach (var tag in Parametros)
                                {
                                    body = body.Replace(tag.Key, tag.Value);
                                }
                            }
                        }
                    }
                }

                return body;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EmailCoreBusiness()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_configuracionCoreBusiness != null)
                {
                    _configuracionCoreBusiness.Dispose();
                }

            }

            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }

        #endregion Dispose
    }
}
