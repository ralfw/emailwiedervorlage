using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using ewv.server.adapter;
using ewv.server.kontrakt;


namespace ewv.server.tests
{
    [TestFixture]
    public class test_WiedervorlagespeicherAdapter
    {
        [Test]
        public void Speichern_und_lesen()
        {
            if (Directory.Exists("Wiedervorlagen")) Directory.Delete("Wiedervorlagen", true);

            var config = new KonfigurationAdapter();
            var sut = new WiedervorlagespeicherAdapter(config);

            sut.Eintragen(new Einplanung
                {
                    Id="id1",
                    Termin = new DateTime(2013, 08, 04, 14, 17, 00),
                    AngelegtAm = new DateTime(2013, 08, 04, 13, 17, 00),

                    Von = "me@gmail.com",
                    Betreff = "Ein Betreff",
                    Text = "Zeile1\nZeile2"
                });
            sut.Eintragen(new Einplanung
                {
                    Id = "id2",
                    Termin = new DateTime(2013, 08, 04, 15, 00, 00),
                    AngelegtAm = new DateTime(2013, 08, 04, 12, 00, 00),

                    Von = "you@gmail.com",
                    Betreff = "Noch ein Betreff",
                    Text = "ZeileA\nZeileB"
                });


            var einplanungen = sut.Alle_Einträge_laden().ToArray();
            Assert.AreEqual(2, einplanungen.Length);
            Assert.AreEqual("id1", einplanungen[0].Id);
            Assert.AreEqual(new DateTime(2013, 08, 04, 15, 00, 00), einplanungen[1].Termin);
            Assert.AreEqual("ZeileA\nZeileB", einplanungen[1].Text);
        }
    }
}
