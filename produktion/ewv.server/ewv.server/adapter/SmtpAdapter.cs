using System;
using System.Net;
using System.Net.Mail;
using ewv.server.kontrakt;

namespace ewv.server.adapter
{
    internal class SmtpAdapter : IDisposable
    {
        private readonly SmtpClient _smtp;

        public SmtpAdapter(KonfigurationAdapter config)
        {
            _smtp = new SmtpClient(config["mailserver_send"]);
            _smtp.Credentials = new NetworkCredential(config["mailserver_user"], config["mailserver_password"]);
        }


        public void Wiedervorlage_versenden(Email email)
        {
            var msg = new MailMessage(new MailAddress(email.Von, "Email-Wiedervorlage"), new MailAddress(email.An))
                {
                    Subject = email.Betreff,
                    Body = email.Text,
                    IsBodyHtml = true
                };
            msg.Headers.Add("In-Reply-To", email.MessageId);

            _smtp.Send(msg);
        }


        public void Dispose()
        {
            _smtp.Dispose();
        }
    }
}