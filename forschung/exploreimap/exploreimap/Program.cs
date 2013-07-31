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
    class Program
    {
        static void Main(string[] args)
        {
            var mailserver = ConfigurationManager.AppSettings["mailserver"];
            var mailserver_user = ConfigurationManager.AppSettings["mailserver_user"];
            var mailserver_password = ConfigurationManager.AppSettings["mailserver_password"];

            Console.WriteLine("server: {0}, user: {1}", mailserver, mailserver_user);

            var rep = new MailRepository(mailserver, 143, false, mailserver_user, mailserver_password);
            Console.WriteLine("connected");
            foreach (var email in rep.GetAllMails("Inbox"))
            {
                Console.WriteLine("{0}->{1}:{2}\n{3}</p>", email.To[0].Email, email.From, email.Subject, email.BodyText.Text);
            }
        }
    }
}
