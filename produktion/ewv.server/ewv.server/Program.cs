using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ewv.server.adapter;
using ewv.server.domain;

namespace ewv.server
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new KonfigurationAdapter();
            using(var sendmail = new SmtpAdapter(config))
            using (var receivemail = new ImapAdapter(config))
            using(var wiedervorlagespeicher = new WiedervorlagespeicherAdapter(config))
            {
                var domain = new Wiedervorlage();
                var integration = new Integration(receivemail, sendmail, wiedervorlagespeicher, domain);

                integration.Einplanen();
                integration.Wiedervorlegen();
            }
        }
    }
}
