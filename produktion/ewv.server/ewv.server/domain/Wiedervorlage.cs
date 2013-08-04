using System;
using ewv.server.adapter;
using ewv.server.kontrakt;

namespace ewv.server.domain
{
    internal class Wiedervorlage
    {
        public Einplanung Termin_berechnen(Email email)
        {
            return null;
        }


        public bool Ist_fällig(Einplanung einplanung)
        {
            return true;
        }


        public Email Wiedervorlageemail_generieren(Einplanung einplanung)
        {
            throw new NotImplementedException();
        }


        [Obsolete]
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