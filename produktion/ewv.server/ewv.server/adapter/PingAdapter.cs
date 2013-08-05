using System;

namespace ewv.server.adapter
{
    internal class PingAdapter
    {
        private readonly KonfigurationAdapter _config;

        public PingAdapter(KonfigurationAdapter config)
        {
            _config = config;
        }


        public void Internetverbindung_prüfen(Action bei_Verbindung, Action ohne_Verbindung)
        {
            try
            {
                System.Net.Dns.GetHostEntry(_config["mailserver_domain"]);
            }
            catch
            {
                ohne_Verbindung();
                return;
            }

           bei_Verbindung();
        }
    }
}