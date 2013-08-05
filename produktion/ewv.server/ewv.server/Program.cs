﻿using System;
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
                var ping = new PingAdapter(config);
                using (var sendmail = new SmtpAdapter(config))
                using (var receivemail = new ImapAdapter(config))
                using (var wiedervorlagespeicher = new WiedervorlagespeicherAdapter(config))
                {
                    var domain = new Wiedervorlage(config);
                    var integration = new Integration(ping, receivemail, sendmail, wiedervorlagespeicher, domain);

                    integration.Ausführen();
                }
            }
            catch (Exception ex)
            {
                LogAdapter.Log(ex);
                Console.WriteLine("  Fehler: {0}", ex.Message);
            }
        }
    }
}
