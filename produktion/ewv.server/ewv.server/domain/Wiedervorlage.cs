using System;
using ewv.server.adapter;
using ewv.server.kontrakt;

namespace ewv.server.domain
{
    internal class Wiedervorlage
    {
        public Email Spiegelung_herstellen(Email email)
        {
            return new Email
                {
                    An = email.Von,
                    Von = "no-reply-wiedervorlage@emailwiedervorlage.de",
                    Betreff = email.Betreff,
                    Text = string.Format("Wiedervorlage von: {0}\n---\n{1}", email.An, email.Text)
                };
        }
    }
}