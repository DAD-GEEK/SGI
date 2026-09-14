using CoreBusiness.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;

namespace CoreBusiness
{
    public class EmailCoreBusiness : IEmailCoreBusiness
    {
        private ParametrosCoreBusiness parametrosCoreBusiness { get { return new ParametrosCoreBusiness(); } }
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);
        public Task EnviarEmailAsync(string Receptor, string Asunto, string Body, List<string> Adjuntos = null, bool IsBodyHtml = true, string Copia = null)
        {
            try
            {
                var parametros = parametrosCoreBusiness.GetAll().FirstOrDefault();

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(parametros.StrSMPTEmail);
                    mailMessage.Subject = Asunto.Trim();
                    mailMessage.Body = Body;
                    mailMessage.IsBodyHtml = IsBodyHtml;
                    if (Copia != null)
                    {
                        mailMessage.CC.Add(new MailAddress(Copia));
                    }
                    foreach (var item in Receptor.Trim().Split(';'))
                    {
                        mailMessage.To.Add(new MailAddress(item));
                    }

                    if (Adjuntos != null)
                    {
                        foreach (string archivo in Adjuntos)
                        {
                            if (System.IO.File.Exists(@archivo))
                                mailMessage.Attachments.Add(new Attachment(@archivo));
                        }
                    }

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                    SmtpClient smtp = new SmtpClient
                    {
                        Host = parametros.StrSMTPHost,
                        EnableSsl = (bool)parametros.OpcSMTPSSL
                    };
                    NetworkCredential NetworkCred = new NetworkCredential
                    {
                        UserName = parametros.StrSMPTEmail.Trim(),
                        Password = parametros.StrSMTPPassword
                    };
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = (int)parametros.StrSMTPPort;
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

                Parametros.Add("{year}", DateTime.Now.Year.ToString());
                Parametros.Add("{version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());

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
                if (parametrosCoreBusiness != null)
                {
                    parametrosCoreBusiness.Dispose();
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
