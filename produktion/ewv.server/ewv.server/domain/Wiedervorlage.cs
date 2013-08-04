using System;
using ewv.server.adapter;
using ewv.server.kontrakt;

namespace ewv.server.domain
{
    internal class Wiedervorlage
    {
        private readonly KonfigurationAdapter _config;

        public Wiedervorlage(KonfigurationAdapter config)
        {
            _config = config;
        }


        public Einplanung Termin_berechnen(Email email)
        {
            var termin = DateTime.Now.Add(new TimeSpan(0, 0, 30));

            return new Einplanung
                {
                    Id = Guid.NewGuid().ToString(),
                    MessageId = email.MessageId,

                    Termin = termin,
                    AngelegtAm = DateTime.Now,

                    Von = email.Von,
                    Betreff = email.Betreff,
                    Text = email.Text
                };
        }


        public bool Ist_fällig(Einplanung einplanung)
        {
            return einplanung.Termin <= DateTime.Now;
        }


        public Email Wiedervorlageemail_generieren(Einplanung einplanung)
        {
            var text = string.Format("<b>Wiedervorlage für Email vom {0}</b><br/><hr/>{1}",
                                      einplanung.AngelegtAm,
                                      einplanung.Text.Replace("\n", "<br/>"));

            return new Email
            {
                MessageId = einplanung.MessageId,
                Von = "no-reply-wiedervorlage@" + _config["mailserver_domain"],
                An = einplanung.Von,
                Betreff = einplanung.Betreff,
                Text = text
            };
        }
    }
}