using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActiveUp.Net.Mail;

namespace exploreimap
{
    /*
     * IMAP Lib: http://mailsystem.codeplex.com/
     * 
     * Weitere Forschung:
     *  - Email im Posteingang löschen
     *  - Nur ungelesene Email laden
     *  - Email versenden
     *      - Email mit Originalinhalt + Zusatz versenden
     *  - Email zurücksenden mit/ohne Reply-to
     */
    class Program
    {
        private static void Main()
        {
            var mailserver = ConfigurationManager.AppSettings["mailserver"];
            var mailserver_user = ConfigurationManager.AppSettings["mailserver_user"];
            var mailserver_password = ConfigurationManager.AppSettings["mailserver_password"];

            Console.WriteLine("server: {0}, user: {1}", mailserver, mailserver_user);

            var rep = new MailRepository(mailserver, 143, false, mailserver_user, mailserver_password);
            Console.WriteLine("connected");
            foreach (var email in rep.GetAllMails("Inbox"))
            {
                var localDate = email.Date.ToLocalTime();
                Console.WriteLine("{0}: to:{1}, {2}/{3}", email.Subject, email.To[0], email.Date, localDate);
            }   
        }


        private static void MainLöschen()
        {
            // Wie können Inbox-Nachrichten gelöscht werden?
            var mailserver = ConfigurationManager.AppSettings["mailserver"];
            var mailserver_user = ConfigurationManager.AppSettings["mailserver_user"];
            var mailserver_password = ConfigurationManager.AppSettings["mailserver_password"];

            Console.WriteLine("server: {0}, user: {1}", mailserver, mailserver_user);

            var rep = new MailRepository(mailserver, 143, false, mailserver_user, mailserver_password);
            Console.WriteLine("connected");
            foreach (var email in rep.GetAllMails("Inbox"))
            {
                Console.WriteLine("vorher {0}: to:{1}", email.Subject, email.To[0]);
            }

            rep.DeleteMail(1);

            foreach (var email in rep.GetAllMails("Inbox"))
            {
                Console.WriteLine("nachher {0}: to:{1}", email.Subject, email.To[0]);
            }
        }

        private static void Main_Adressaten(string[] args)
        {
            // Was steht in To, Cc, Bcc drin?
            var mailserver = ConfigurationManager.AppSettings["mailserver"];
            var mailserver_user = ConfigurationManager.AppSettings["mailserver_user"];
            var mailserver_password = ConfigurationManager.AppSettings["mailserver_password"];

            Console.WriteLine("server: {0}, user: {1}", mailserver, mailserver_user);

            var rep = new MailRepository(mailserver, 143, false, mailserver_user, mailserver_password);
            Console.WriteLine("connected");
            foreach (var email in rep.GetAllMails("Inbox"))
            {
                Console.WriteLine("{0}: to:{1}, cc:{2}, bcc:{3}",
                                  email.Subject,
                                  email.To.Count,
                                  email.Cc.Count,
                                  email.Bcc.Count);
            }
        }


        static void Main_Receive(string[] args)
        {
            // Wie funktioniert das Abholen von Emails?
            var mailserver = ConfigurationManager.AppSettings["mailserver"];
            var mailserver_user = ConfigurationManager.AppSettings["mailserver_user"];
            var mailserver_password = ConfigurationManager.AppSettings["mailserver_password"];

            Console.WriteLine("server: {0}, user: {1}", mailserver, mailserver_user);

            var rep = new MailRepository(mailserver, 143, false, mailserver_user, mailserver_password);
            Console.WriteLine("connected");
            foreach (var email in rep.GetAllMails("Inbox"))
            {
                Console.WriteLine("{0}->{1}/{2}:{3}\n{4}</p>", 
                    email.To[0],
                    email.From, 
                    email.ReplyTo,
                    email.Subject, 
                    email.BodyText.Text);
            }
        }
    }
}
