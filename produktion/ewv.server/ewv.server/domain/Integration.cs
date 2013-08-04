using System;
using System.Collections.Generic;
using System.Linq;
using ewv.server.adapter;
using ewv.server.kontrakt;

namespace ewv.server.domain
{
    internal class Integration
    {
        private readonly ImapAdapter _receivemail;
        private readonly SmtpAdapter _sendmail;
        private readonly Wiedervorlage _domain;
        private readonly WiedervorlagespeicherAdapter _wiedervorlagespeicher;

        public Integration(ImapAdapter receivemail, SmtpAdapter sendmail, WiedervorlagespeicherAdapter wiedervorlagespeicher, Wiedervorlage domain)
        {
            _receivemail = receivemail;
            _sendmail = sendmail;
            _wiedervorlagespeicher = wiedervorlagespeicher;
            _domain = domain;
        }


        public void Ausführen()
        {
            LogAdapter.Log("<<<");

            Einplanen();
            Wiedervorlegen();

            LogAdapter.Log(">>>");
        }


        private void Einplanen()
        {
            LogAdapter.Log("Einplanen");

            var emails = _receivemail.Einplanungen_abholen().ToList();
            emails.ForEach(Email_einplanen);
        }

        private void Email_einplanen(Email email)
        {
            LogAdapter.Log("  Email von {0} an {1}: {2}", email.Von, email.An, email.Betreff);

            var einplanung = _domain.Termin_berechnen(email);
            _wiedervorlagespeicher.Eintragen(einplanung);
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
            LogAdapter.Log("  Versenden an {0}: {1}", einplanung.Email.Von, einplanung.Email.Text);

            var email = _domain.Wiedervorlageemail_generieren(einplanung);
            _sendmail.Wiedervorlage_versenden(email);
            _wiedervorlagespeicher.Löschen(einplanung);
        }
    }
}