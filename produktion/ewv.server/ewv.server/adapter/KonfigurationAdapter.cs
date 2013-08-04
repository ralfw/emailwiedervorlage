using System.Configuration;

namespace ewv.server.adapter
{
    public class KonfigurationAdapter
    {
        public string this[string schlüssel]
        {
            get { return ConfigurationManager.AppSettings[schlüssel]; }
        }
    }
}