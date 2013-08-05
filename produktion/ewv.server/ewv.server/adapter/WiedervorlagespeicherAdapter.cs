using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ewv.server.kontrakt;

namespace ewv.server.adapter
{
    public class WiedervorlagespeicherAdapter : IDisposable
    {
        private const string PATH = "Wiedervorlagen";

        public WiedervorlagespeicherAdapter(KonfigurationAdapter config)
        {
            if (!Directory.Exists(PATH)) Directory.CreateDirectory(PATH);
        }


        public void Eintragen(Einplanung einplanung)
        {
            using (var sw = new StreamWriter(Dateiname_für_Einplanung(einplanung)))
            {
                sw.WriteLine("1.0");
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
            var filenames = Directory.GetFiles(PATH);
            return filenames.Select(Eintrag_laden);
        }

        private Einplanung Eintrag_laden(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                sr.ReadLine();

                var einplanung = new Einplanung
                    {
                        Id = Path.GetFileNameWithoutExtension(filename),
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


        private static string Dateiname_für_Einplanung(Einplanung einplanung)
        {
            return Path.Combine(PATH, einplanung.Id + ".txt");
        }

        
        public void Dispose()
        {}
    }
}