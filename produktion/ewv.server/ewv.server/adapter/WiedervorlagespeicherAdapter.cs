using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ewv.server.kontrakt;

namespace ewv.server.adapter
{
    public class WiedervorlagespeicherAdapter : IDisposable
    {
        private readonly string _path = "Wiedervorlagen";

        public WiedervorlagespeicherAdapter(KonfigurationAdapter config)
        {
            _path = config["wiedervorlagespeicher_pfad"];
            if (string.IsNullOrEmpty(_path)) _path = "Wiedervorlagen";

            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
        }


        private const string VERSIONSNR = "1.1";

        public void Eintragen(Einplanung einplanung)
        {
            var filename = Dateiname_für_Einplanung(einplanung);
            if (File.Exists(filename)) return;

            var ci = CultureInfo.CreateSpecificCulture("de-DE");

            using (var sw = new StreamWriter(filename))
            {
                sw.WriteLine(VERSIONSNR);
                sw.WriteLine(einplanung.Id);
                sw.WriteLine(einplanung.Termin.ToString(ci));
                sw.WriteLine(einplanung.AngelegtAm.ToString(ci));

                sw.WriteLine(einplanung.Email.MessageId);
                sw.WriteLine(einplanung.Email.VersandzeitpunktUTC.ToString(ci));
                sw.WriteLine(einplanung.Email.An);
                sw.WriteLine(einplanung.Email.AnWiedervorlage);
                sw.WriteLine(einplanung.Email.Von);
                sw.WriteLine(einplanung.Email.Betreff);
                sw.Write(einplanung.Email.Text);
            }
        }



        public IEnumerable<Einplanung> Alle_Einträge_laden()
        {
            var filenames = Directory.GetFiles(_path, "*.txt");
            return filenames.Select(Eintrag_laden);
        }

        internal Einplanung Eintrag_laden(string filename)
        {
            var ci = CultureInfo.CreateSpecificCulture("de-DE");

            using (var sr = new StreamReader(filename))
            {
                var versionsnr = sr.ReadLine();
                if (versionsnr != VERSIONSNR) throw new InvalidOperationException(string.Format("Ungültige Versionsnr. der Einplanung: {0}, {1}", versionsnr, filename));

                var einplanung = new Einplanung
                    {
                        Id = sr.ReadLine(),
                        Termin = DateTime.Parse(sr.ReadLine(), ci),
                        AngelegtAm = DateTime.Parse(sr.ReadLine(), ci),
                    };

                var email = new Email
                    {
                        MessageId = sr.ReadLine(),
                        VersandzeitpunktUTC = DateTime.Parse(sr.ReadLine(), ci),
                        An = sr.ReadLine(),
                        AnWiedervorlage = sr.ReadLine(),
                        Von = sr.ReadLine(),
                        Betreff = sr.ReadLine(),
                        Text = sr.ReadToEnd()
                    };
                einplanung.Email = email;

                return einplanung;
            }
        }


        public void Löschen(Einplanung einplanung)
        {
            File.Delete(Dateiname_für_Einplanung(einplanung));
        }


        private string Dateiname_für_Einplanung(Einplanung einplanung)
        {
            return Path.Combine(_path, string.Format("{0}-{1}.txt", einplanung.Email.MessageId, einplanung.Email.AnWiedervorlage));
        }

        
        public void Dispose()
        {}
    }
}