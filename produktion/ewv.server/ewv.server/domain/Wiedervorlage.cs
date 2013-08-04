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
                    Termin = termin,
                    AngelegtAm = DateTime.Now,
                    Email = email
                };
        }


        public bool Ist_fällig(Einplanung einplanung)
        {
            return einplanung.Termin <= DateTime.Now;
        }


        public Email Wiedervorlageemail_generieren(Einplanung einplanung)
        {
            var text = string.Format("<b>Wiedervorlage für Email vom {0} an {1}</b><br/><hr/>{2}",
                                      einplanung.AngelegtAm,
                                      einplanung.Email.An,
                                      einplanung.Email.Text.Replace("\n", "<br/>"));

            return new Email
            {
                MessageId = einplanung.Email.MessageId,
                Von = "no-reply-wiedervorlage@" + _config["mailserver_domain"],
                An = einplanung.Email.Von,
                Betreff = einplanung.Email.Betreff,
                Text = text
            };
        }
    }
}