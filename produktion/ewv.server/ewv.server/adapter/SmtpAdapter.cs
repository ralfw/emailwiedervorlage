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
            _smtp.Send(msg);
            //Console.WriteLine("wiedervorlage versandt: {0}->{1}\n{2}\n{3}\n\n", email.Von, email.An, email.Betreff, email.Text);
        }


        public void Dispose()
        {
            _smtp.Dispose();
        }
    }
}