using System.Configuration;

namespace ewv.server.adapter
{
    internal class KonfigurationAdapter
    {
        public string this[string schlüssel]
        {
            get { return ConfigurationManager.AppSettings[schlüssel]; }
        }
    }
}