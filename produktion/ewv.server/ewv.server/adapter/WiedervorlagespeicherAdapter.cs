using System;
using System.Collections.Generic;
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


        public void Eintragen(Einplanung einplanung)
        {
            var filename = Dateiname_für_Einplanung(einplanung);
            if (File.Exists(filename)) return;

            using (var sw = new StreamWriter(filename))
            {
                sw.WriteLine("1.0");
                sw.WriteLine(einplanung.Id);
                sw.WriteLine(einplanung.Termin);
                sw.WriteLine(einplanung.AngelegtAm);

                sw.WriteLine(einplanung.Email.MessageId);
                sw.WriteLine(einplanung.Email.Von);
                sw.WriteLine(einplanung.Email.An);
                sw.WriteLine(einplanung.Email.Betreff);
                sw.Write(einplanung.Email.Text);
            }
        }



        public IEnumerable<Einplanung> Alle_Einträge_laden()
        {
            var filenames = Directory.GetFiles(_path);
            return filenames.Select(Eintrag_laden);
        }

        internal Einplanung Eintrag_laden(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                sr.ReadLine(); // Versionsnr

                var einplanung = new Einplanung
                    {
                        Id = sr.ReadLine(),
                        Termin = DateTime.Parse(sr.ReadLine()),
                        AngelegtAm = DateTime.Parse(sr.ReadLine()),
                    };

                var email = new Email
                    {
                        MessageId = sr.ReadLine(),
                        Von = sr.ReadLine(),
                        An = sr.ReadLine(),
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
            return Path.Combine(_path, einplanung.Email.MessageId + ".txt");
        }

        
        public void Dispose()
        {}
    }
}