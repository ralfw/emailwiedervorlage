using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ewv.server.domain;
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
            var filename = PATH + "\\" + (Directory.GetFiles(PATH).Length + 1) + ".txt";
            using (var sw = new StreamWriter(filename))
            {
                sw.WriteLine("1.0");
                sw.WriteLine(einplanung.Id);
                sw.WriteLine(einplanung.Termin);
                sw.WriteLine(einplanung.AngelegtAm);
                sw.WriteLine(einplanung.Von);
                sw.WriteLine(einplanung.Betreff);
                sw.Write(einplanung.Text);
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
                        Id = sr.ReadLine(),
                        Termin = DateTime.Parse(sr.ReadLine()),
                        AngelegtAm = DateTime.Parse(sr.ReadLine()),
                        Von = sr.ReadLine(),
                        Betreff = sr.ReadLine(),
                        Text = sr.ReadToEnd()
                    };

                return einplanung;
            }
        }


        public void Dispose()
        {}
    }
}