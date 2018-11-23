using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Web;
using IMessage = SmlGround.DLL.Interfaces.IMessage;

namespace SmlGround.SMTP
{
    public class Sender
    {
        string display;
        string receiver;
        public Sender(string display, string receiver)
        {
            this.display = display;
            this.receiver = receiver;
        }
        public void SendMessage(IMessage message)//string subject, string body
        {
            var i = ConfigurationManager.AppSettings;
            MailAddress from = new MailAddress(ConfigurationManager.AppSettings["CredentialsEmail"], display);
            MailAddress to = new MailAddress(receiver);
            MailMessage m = new MailMessage(from, to);
            (m.Subject,m.Body) = message.FormMessage();
            m.IsBodyHtml = true;
            SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 25)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["CredentialsEmail"], ConfigurationManager.AppSettings["CredentialsPassword"])

            };
            smtp.Send(m);
        }
    }
}