using NUnit.Framework;
using Soneta.Business;
using Soneta.CRM;
using Soneta.Handel;
using Soneta.Magazyny;
using Soneta.Test;
using Soneta.Towary;
using Soneta.Types;
using System;
using System.Reflection;

namespace Soneta.Napoje.Tests
{
    // Wykorzystanie istniejącej bazy danych, atrybut nie jest wymagany, bez niego zostanie wykorzystana baza nunit:default
    [TestDatabase("GeekOut")]
    class WorkerTests : TestBase
    {
        public override void ClassSetup()
        {
            // Konieczne jest wczytanie assembly dodatku
            Assembly.Load("Soneta.Napoje");
            base.ClassSetup();
        }

        // Przygotowanie bazy przed każdym testem
        // Opłata będzie naliczana dla dokumentów rozchodu, dlatego wcześniej musimy przyjąć na magazyn
        // odpowiednie towary dokumentami przychodu
        public override void TestSetup()
        {
            base.TestSetup();
            NowyDokument("ZK", "PIWO", new Quantity(10.0, "l"), new DoubleCy(5.0, "PLN"));
        }

        [Test]
        public void WorkersTest()
        {
            var dokumentGuid = NowyDokument("FV", "PIWO", new Quantity(1.0, "l"), new DoubleCy(10.0, "PLN"));
            var dokument = Session.Get<DokumentHandlowy>(dokumentGuid);

            var naliczOplateWorker = new NaliczOplateWorker()
            {
                Session = Session,
                Params = new NaliczOplateWorker.NaliczOplateParams(Context)
                {
                    Okres = FromTo.All
                }
            };
            naliczOplateWorker.NaliczOplate();

            var oplataNapojuWorker = new OplataNapojuWorker()
            {
                Pozycja = dokument.PozycjaWgIdent(1)
            };
            var niskoprocentowe = Session.GetNapoje().KategorieNapoj.NaglowekWgKodu["ALKO_N"];
            Assert.AreEqual(niskoprocentowe, oplataNapojuWorker.Kategoria);
            // 5% * 10 PLN => 0.5 PLN
            Assert.AreEqual(new Currency(0.5), oplataNapojuWorker.Oplata);
        }

        private Guid NowyDokument(string definicja, string kodTowaru, Quantity ilosc, DoubleCy cena)
        {
            Guid dokumentGuid = Guid.Empty;
            // Wywołujemy metodę pomocniczą do otwarcia transakcji
            InTransaction(() =>
            {
                var dokument = new DokumentHandlowy();
                dokument.Definicja = Session.GetHandel().DefDokHandlowych.WgSymbolu[definicja];
                dokument.Magazyn = Session.GetMagazyny().Magazyny.Firma;
                dokument.Kontrahent = Session.GetCRM().Kontrahenci.WgKodu["ABC"];
                Session.AddRow(dokument);

                // Każda pozycja dokumentu handlowego powinna być wywoływana w oddzielnej transakcji
                InUITransaction(() =>
                {
                    var pozycja = new PozycjaDokHandlowego(dokument);
                    Session.AddRow(pozycja);
                    pozycja.Towar = Session.GetTowary().Towary.WgKodu[kodTowaru];
                    pozycja.Ilosc = ilosc;
                    pozycja.Cena = cena;
                    pozycja.Rabat = Percent.Zero;
                });

                dokument.Stan = StanDokumentuHandlowego.Zatwierdzony;
                dokumentGuid = dokument.Guid;
            });
            // Zapis i zwolnienie sesji, kolejne odwołanie do obiektu Session utworzy sobie autoamtycznie nową sesję
            SaveDispose();
            return dokumentGuid;
        }
    }
}

