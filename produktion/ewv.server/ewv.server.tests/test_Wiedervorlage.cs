using System;
using NUnit.Framework;
using ewv.server.adapter;
using ewv.server.domain;

namespace ewv.server.tests
{
    [TestFixture]
    public class test_Wiedervorlage
    {
        [Test]
        public void Countdown_bestimmen()
        {
            var config = new KonfigurationAdapter();
            var sut = new Wiedervorlage(config);

            var c = sut.Countdown_bestimmen("in10minuten@wiedervorlage.cc");
            Assert.AreEqual(new TimeSpan(0,0,10,0), c);

            c = sut.Countdown_bestimmen("in99std@wiedervorlage.cc");
            Assert.AreEqual(new TimeSpan(0, 99, 0, 0), c);

            c = sut.Countdown_bestimmen("in5day@wiedervorlage.cc");
            Assert.AreEqual(new TimeSpan(5, 0, 0, 0), c);
        }

        [Test]
        public void Countdown_bestimmen_mit_falschem_Zeitraum()
        {
            var config = new KonfigurationAdapter();
            var sut = new Wiedervorlage(config);
            Assert.Throws<InvalidOperationException>(() => sut.Countdown_bestimmen("in99xxx@wiedervorlage.cc"));
        }

        [Test]
        public void Countdown_bestimmen_mit_falscher_Syntax()
        {
            var config = new KonfigurationAdapter();
            var sut = new Wiedervorlage(config);
            Assert.Throws<InvalidOperationException>(() => sut.Countdown_bestimmen("inXXyy@wiedervorlage.cc"));
        }
    }
}