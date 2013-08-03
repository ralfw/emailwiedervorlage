using System;
using ewv.server.kontrakt;

namespace ewv.server.adapter
{
    internal class SmtpAdapter : IDisposable
    {
        public SmtpAdapter(KonfigurationAdapter config)
        {
            
        }

        public void Wiedervorlage_versenden(Email email)
        {
            Console.WriteLine("wiedervorlage versenden: {0}->{1}\n{2}\n{3}\n\n",
                                    email.Von,
                                    email.An,
                                    email.Betreff,
                                    email.Text);
        }


        public void Dispose()
        {}
    }
}