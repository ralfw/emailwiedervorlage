using System;
using ewv.server.adapter;
using ewv.server.domain;
using ewv.server.kontrakt;

namespace ewv.server
{
    class Program
    {
        private static KonfigurationAdapter _config;
        private static SmtpAdapter _sendmail;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ewv.server...");

                _config = new KonfigurationAdapter();
                var ping = new PingAdapter(_config);
                using (_sendmail = new SmtpAdapter(_config))
                using (var receivemail = new ImapAdapter(_config))
                using (var wiedervorlagespeicher = new WiedervorlagespeicherAdapter(_config))
                {
                    var domain = new Wiedervorlage(_config);
                    var integration = new Integration(ping, receivemail, _sendmail, wiedervorlagespeicher, domain);
                    integration.Fehler_bei_Einplanung += Absender_über_Fehler_informieren;

                    integration.Ausführen();
                }
            }
            catch (Exception ex)
            {
                LogAdapter.Log(ex);
                Console.WriteLine("  Fehler: {0}", ex.Message);
            }
        }


        private static void Absender_über_Fehler_informieren(string fehlermeldung, Email email)
        {
            var text = string.Format("<b>Fehler bei der Einplanung zur Wiedervorlage: {0}<br/>Email vom {1} UTC. Empfänger: {2}, Wiedervorlage: {3}</b><br/><hr/>{4}",
                                      fehlermeldung,                      
                                      email.VersandzeitpunktUTC,
                                      email.An,
                                      email.AnWiedervorlage,
                                      email.Text.Replace("\n", "<br/>"));

            var fehlerEmail = new Email
            {
                MessageId = email.MessageId,
                Von = "no-reply-wiedervorlage@" + _config["mailserver_domain"],
                An = email.Von,
                Betreff = email.Betreff,
                Text = text
            };

            _sendmail.Senden(fehlerEmail);
        }
    }
}
