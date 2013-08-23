using System;
using System.Collections.Generic;
using System.Linq;
using ewv.server.adapter;
using ewv.server.kontrakt;

namespace ewv.server.domain
{
    internal class Integration
    {
        private readonly PingAdapter _ping;
        private readonly ImapAdapter _receivemail;
        private readonly SmtpAdapter _sendmail;
        private readonly Wiedervorlage _domain;
        private readonly WiedervorlagespeicherAdapter _wiedervorlagespeicher;

        public Integration(PingAdapter ping, ImapAdapter receivemail, SmtpAdapter sendmail, WiedervorlagespeicherAdapter wiedervorlagespeicher, Wiedervorlage domain)
        {
            _ping = ping;
            _receivemail = receivemail;
            _sendmail = sendmail;
            _wiedervorlagespeicher = wiedervorlagespeicher;
            _domain = domain;
        }


        public void Ausführen()
        {
            _ping.Internetverbindung_prüfen(
            () => {
                LogAdapter.Log("<<<");

                Verbinden();
                Einplanen();
                Wiedervorlegen();

                LogAdapter.Log(">>>");
                Console.WriteLine("  Ok");
            },
            () => {
                LogAdapter.Log("<<<Keine Internetverbindung!>>>");
                Console.WriteLine("  Keine Internetverbindung!");
            });
        }


        private void Verbinden()
        {
            _receivemail.Verbinden();
            _sendmail.Verbinden();
        }


        private void Einplanen()
        {
            LogAdapter.Log("Einplanen");

            var emails = _receivemail.Einplanungen_abholen().ToList();
            emails.ForEach(Email_einplanen);
        }

        private void Email_einplanen(Email email)
        {
            LogAdapter.Log("  {0} -> {1}: {2}", email.Von, email.An, email.Betreff);
            if (email.An.StartsWith("throwexception")) throw new ApplicationException("Fehler erzwungen zu Testzwecken!");

            _domain.Termin_berechnen(email,
                                     einplanung => {
                                        LogAdapter.Log("    eingeplant für {0}", einplanung.Termin);
                                        _wiedervorlagespeicher.Eintragen(einplanung);
                                     },
                                     fehlermeldung => Fehler_bei_Einplanung(fehlermeldung, email));
        }



        private void Wiedervorlegen()
        {
            LogAdapter.Log("Wiedervorlegen");

            var fälligeEinplanungen = Fällige_Einplanungen_selektieren().ToList();
            fälligeEinplanungen.ForEach(Versenden);
        }

        private IEnumerable<Einplanung> Fällige_Einplanungen_selektieren()
        {
            var einplanungen = _wiedervorlagespeicher.Alle_Einträge_laden().ToArray();
            var fälligeEinplanungen = einplanungen.Where(_domain.Ist_fällig).ToArray();

            LogAdapter.Log("  Einplanungen: {0}, Fällige: {1}", einplanungen.Length, fälligeEinplanungen.Length);
            return fälligeEinplanungen;
        }

        private void Versenden(Einplanung einplanung)
        {
            LogAdapter.Log("  {0} vom {1}: {2}", einplanung.Email.Von, einplanung.AngelegtAm, einplanung.Email.Betreff);

            var email = _domain.Wiedervorlageemail_generieren(einplanung);
            _sendmail.Senden(email);
            _wiedervorlagespeicher.Löschen(einplanung);
        }


        public event Action<string, Email> Fehler_bei_Einplanung;
    }
}