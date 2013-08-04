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


        public void Einplanen()
        {
            var emails = _receivemail.Einplanungen_abholen().ToList();
            Console.WriteLine("Einzuplanen: {0}", emails.Count);
            emails.ForEach(Email_einplanen);
        }

        private void Email_einplanen(Email email)
        {
            var einplanung = _domain.Termin_berechnen(email);
            _wiedervorlagespeicher.Eintragen(einplanung);
        }



        public void Wiedervorlegen()
        {
            var fälligeEinplanungen = Fällige_Einplanungen_selektieren().ToList();
            Console.WriteLine("Fällige Wiedervorlagen: {0}", fälligeEinplanungen.Count);
            fälligeEinplanungen.ForEach(Versenden);
        }

        private IEnumerable<Einplanung> Fällige_Einplanungen_selektieren()
        {
            var einplanungen = _wiedervorlagespeicher.Alle_Einträge_laden().ToList();
            return einplanungen.Where(_domain.Ist_fällig);
        }

        private void Versenden(Einplanung einplanung)
        {
            var email = _domain.Wiedervorlageemail_generieren(einplanung);
            _sendmail.Wiedervorlage_versenden(email);
            _wiedervorlagespeicher.Löschen(einplanung);
        }
    }
}