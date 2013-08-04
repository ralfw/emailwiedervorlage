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
            var countdown = Countdown_bestimmen(email.An);
            var termin = DateTime.Now.Add(countdown);
            return new Einplanung
            {
                Id = Guid.NewGuid().ToString(),
                Termin = termin,
                AngelegtAm = DateTime.Now,
                Email = email
            };
        }

        internal TimeSpan Countdown_bestimmen(string emailadresse)
        {
            var m = System.Text.RegularExpressions.Regex.Match(emailadresse, @"in(\d+)(\w+)@");
            if (m.Success)
            {
                var dauer = int.Parse(m.Groups[1].Value);
                var zeitraum = m.Groups[2].Value;

                switch (zeitraum.ToLower())
                {
                    case "min":
                    case "minute":
                    case "minuten":
                    case "minutes":
                        return new TimeSpan(0, 0, dauer, 0);

                    case "h":
                    case "std":
                    case "stunde":
                    case "stunden":
                    case "hour":
                    case "hours":
                        return new TimeSpan(0, dauer, 0, 0);

                    case "t":
                    case "d":
                    case "tag":
                    case "tage":
                    case "tagen":
                    case "day":
                    case "days":
                        return new TimeSpan(dauer, 0, 0, 0);

                    case "m":
                    case "mon":
                    case "monat":
                    case "monate":
                    case "monaten":
                    case "month":
                    case "months":
                        return new TimeSpan(30 * dauer, 0, 0, 0);

                    case "y":
                    case "j":
                    case "jahr":
                    case "jahre":
                    case "jahren":
                    case "year":
                    case "years":
                        return new TimeSpan(365 * dauer, 0, 0, 0);

                    default:
                        throw new InvalidOperationException("Ungültiger Zeitraum in Einplanungsadresse: " + emailadresse);
                }
            }
           
            throw new InvalidOperationException("Adresse entspricht nicht der Einplanungssyntax: " + emailadresse);
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