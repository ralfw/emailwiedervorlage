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

                    Email = new Email
                        {
                            MessageId = "abc",
                            Von = "me@gmail.com",
                            An = "in5minuten@wiedervorlage.cc",
                            Betreff = "Ein Betreff",
                            Text = "Zeile1\nZeile2"
                        }
                });
            sut.Eintragen(new Einplanung
                {
                    Id = "id2",
                    Termin = new DateTime(2013, 08, 04, 15, 00, 00),
                    AngelegtAm = new DateTime(2013, 08, 04, 12, 00, 00),

                    Email = new Email
                    {
                        MessageId = "xyz",
                        Von = "you@gmail.com",
                        An = "in3tagen@wiedervorlage.cc",
                        Betreff = "Noch ein Betreff",
                        Text = "ZeileA\nZeileB"
                    }
                });


            var einplanungen = sut.Alle_Einträge_laden().ToArray();
            Assert.AreEqual(2, einplanungen.Length);
            Assert.AreEqual("id1", einplanungen[0].Id);
            Assert.AreEqual(new DateTime(2013, 08, 04, 15, 00, 00), einplanungen[1].Termin);
            Assert.AreEqual("ZeileA\nZeileB", einplanungen[1].Email.Text);
        }


        [Test]
        public void Lesen_aus_einer_Datei()
        {
            var config = new KonfigurationAdapter();
            var sut = new WiedervorlagespeicherAdapter(config);
            
            var e = sut.Eintrag_laden(@"testdata\CAGcd=gEdKE5YyBmmMo=GWafnka0cRh_eLmdhF2-jJYN6QtU1sw@mail.gmail.com.txt");

            Assert.AreEqual("Ein Betreff", e.Email.Betreff);
        }
    }
}
