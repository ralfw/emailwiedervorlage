using System;
using ewv.server.adapter;
using ewv.server.domain;

namespace ewv.server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ewv.server...");

                var config = new KonfigurationAdapter();

                using (var sendmail = new SmtpAdapter(config))
                using (var receivemail = new ImapAdapter(config))
                using (var wiedervorlagespeicher = new WiedervorlagespeicherAdapter(config))
                {
                    var domain = new Wiedervorlage(config);
                    var integration = new Integration(receivemail, sendmail, wiedervorlagespeicher, domain);

                    integration.Ausführen();
                }

                Console.WriteLine("ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: {0}", ex.Message);
                LogAdapter.Log(ex);
            }
        }
    }
}
