using System;
using System.Net.NetworkInformation;
using ewv.server.adapter;

namespace ewv.server
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
                var p = new Ping();
                var result = p.Send(_config["mailserver_domain"]);
                if (result.Status == IPStatus.Success)
                    bei_Verbindung();
            }
            catch
            {
                ohne_Verbindung();
            }
        }
    }
}