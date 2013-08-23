using System;
using System.Net;
using System.Net.Mail;
using ewv.server.kontrakt;

namespace ewv.server.adapter
{
    internal class SmtpAdapter : IDisposable
    {
        private readonly KonfigurationAdapter _config;
        private SmtpClient _smtp;

        public SmtpAdapter(KonfigurationAdapter config)
        {
            _config = config;
        }


        public void Verbinden()
        {
            _smtp = new SmtpClient(_config["mailserver_send"]);
            _smtp.Credentials = new NetworkCredential(_config["mailserver_user"], _config["mailserver_password"]);
        }


        public void Senden(Email email)
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
            if (_smtp == null) return;

            _smtp.Dispose();
        }
    }
}