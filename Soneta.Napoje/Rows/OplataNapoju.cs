// Zdefiniowanie klasy pośredniej (przelotki) jest wymagane dla wszystkich tabel opisanych w business.xml 
// i generowanych do business.cs przez Soneta.Generator.
// W odróżnieniu od business.cs - ten plik możemy bezpiecznie edytować i rozbudowywać

using Soneta.Business;
using Soneta.Handel;
using Soneta.Types;

namespace Soneta.Napoje
{
    // W tym pliku definiujemy klasę pośrednią dla wiersza tabeli
    public class OplataNapoju : NapojeModule.OplataNapojuRow
    {
        internal OplataNapoju()
        {
        }

        public OplataNapoju(PozycjaDokHandlowego pozycja, KategoriaNapoju kategoria)
        {
            basePozycja = pozycja;
            baseKategoria = kategoria;
        }

        public void NaliczOplate()
        {
            Currency oplata = Currency.Zero;
            var napoj = Module.Napoje.WgTowar[Pozycja.Towar];
            // Naliczamy opłatę jeżeli napój nie ma podanego producenta lub jeżeli producent jest opodatkowany
            if (napoj != null && !napoj.Kategoria.Zablokowany &&
                (napoj.Producent == null || napoj.Producent.Typ == TypProducentaNapoju.Opodatkowany))
            {
                oplata = new Currency(Pozycja.Wartość * napoj.Kategoria.Oplata);
            }  
            using (var tran = Session.Logout(true))
            {
                baseOplata = oplata;
                tran.Commit();
            }
        }
    }
}
