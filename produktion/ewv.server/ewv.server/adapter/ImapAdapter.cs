using System;
using System.Collections.Generic;
using ewv.server.kontrakt;

namespace ewv.server.adapter
{
    internal class ImapAdapter : IDisposable
    {
        public ImapAdapter(KonfigurationAdapter config)
        {}

        public IEnumerable<Email> Einplanungen_abholen()
        {
            return new[]
                {
                    new Email
                        {
                            An = "in10tagen@emailwiedervorlage.de",
                            Von = "ralfw08@gmail.com",
                            Betreff = "Ein Betreff",
                            Text = "Ein Email-Text"
                        },
                    new Email
                        {
                            An = "in5minuten@emailwiedervorlage.de",
                            Von = "info@ralfw.de",
                            Betreff = "Re: Ein Betreff",
                            Text = "Ein Antwort-Text"
                        }
                };
        }

        public void Dispose()
        {}
    }
}