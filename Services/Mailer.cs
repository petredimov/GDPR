using DatabaseNamespace;
using Logger;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using DatabaseNamespace.Models;

namespace Services
{
    public class Mailer
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public List<string> Receivers { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnabledSSL { get; set; }
        public MailServerType Servertype { get; set; }

        public Mailer(MailServerType servertype, string serverhost, int serverport, string mailFrom, string username, string password, bool enableSsl)
        {
            Servertype = servertype;
            Host = serverhost;
            Port = serverport;
            From = mailFrom;
            Username = username;
            Password = password;
            EnabledSSL = enableSsl;
        }

        public Mailer(MailConfiguration mailConfig)
        {
            Servertype = mailConfig.MailType;
            Host = mailConfig.Host;
            Port = mailConfig.Port;
            From = mailConfig.From;
            Username = mailConfig.Username;
            Password = mailConfig.Password;
            EnabledSSL = mailConfig.EnableSsl;
        }

		/// <summary>
		/// Send mail depending on server type
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="recipients"></param>
		/// <param name="fileInfo"></param>
		/// <returns>boolean</returns>
        public bool SendMail(string subject, string body, List<string> recipients = null, List<string> fileInfo = null)
        {
            switch (Servertype)
            {
                case MailServerType.SMTP:
                    return SendSmtp(subject, body, recipients, fileInfo);
                case MailServerType.Exchange2007_SP1:
                case MailServerType.Exchange2010:
                case MailServerType.Exchange2010_SP1:
                case MailServerType.Exchange2010_SP2:
                case MailServerType.Exchange2013:
                case MailServerType.Exchange2013_SP1:
                    return SendExchange(subject, body, recipients, fileInfo);
            }
            return false;
        }

        /// <summary>
        /// Send a email message to specific recipients
        /// </summary>
        /// <param name="to">The specific recipient</param>
        /// <param name="subject">The mail subject string</param>
        /// <param name="body">The mail body (text) string</param>
        private bool SendSmtp(string subject, string body, List<string> recipients = null, List<string> fileInfo = null)
        {
            try
            {
                if (recipients != null)
                {
                    Receivers = recipients;
                }

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(From);

                foreach (string receiver in Receivers)
                {
                    mail.To.Add(receiver);
                }

                mail.Subject = subject;
                mail.Body = body;

                SmtpClient client = new SmtpClient(Host, Port);
                client.EnableSsl = EnabledSSL;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (!string.IsNullOrEmpty(Username))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(Username, Password);
                }
                if (fileInfo != null)
                {
                    foreach (var item in fileInfo)
                    {
                        if (item != "" && File.Exists(item))
                        {
                            ContentType contentType = new ContentType();
                            contentType.MediaType = MediaTypeNames.Application.Octet;
                            contentType.Name = new FileInfo(item).Name;
                            mail.Attachments.Add(new System.Net.Mail.Attachment(item, contentType));
                        }
                    }
                }
                client.Send(mail);
            }
            catch (AuthenticationException aex)
            {
                Log.Write("sendSmtp AuthenticationException: " + aex.Message, LogLevel.Error, string.Empty, aex);
                return false;
            }
            catch (SmtpException smtpex)
            {
                Log.Write("sendSmtp SmtpException: " + smtpex.Message, LogLevel.Error, string.Empty, smtpex);
                return false;
            }
            catch (Exception ex)
            {
                Log.Write("sendSmtp exception: " + ex.Message, LogLevel.Error, string.Empty, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Send a email message to all mail recipients
        /// </summary>
        /// <param name="subject">The mail subject string</param>
        /// <param name="body">The mail body (text) string</param>
        private bool SendExchange(string subject, string body, List<string> recipients = null, List<string> fileInfo = null)
        {
            try
            {
                if (recipients != null)
                {
                    Receivers = recipients;
                }

                ExchangeVersion serverType = (ExchangeVersion)Enum.Parse(typeof(ExchangeVersion), Servertype.ToString());

                ExchangeService service = new ExchangeService(serverType);
                service.Credentials = new WebCredentials(Username, Password);
                service.Url = new Uri(Host);

                EmailMessage message = new EmailMessage(service);
                message.Subject = subject;
                message.Body = body;
                foreach (string receiver in Receivers)
                {
                    message.ToRecipients.Add(receiver);
                }

                if (fileInfo != null)
                {
                    foreach (var item in fileInfo)
                    {
                        if (item != "" && File.Exists(item))
                        {
                            message.Attachments.AddFileAttachment(item);
                        }
                    }
                }
                message.SendAndSaveCopy();
            }
            catch (Exception ex)
            {
                Log.Write("sendExchange exception: " + ex.Message, LogLevel.Error, string.Empty, ex);
                return false;
                throw;
            }
            return true;
        }
    }
}
