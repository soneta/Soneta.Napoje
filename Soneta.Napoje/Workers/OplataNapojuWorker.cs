using Soneta.Business;
using Soneta.Handel;
using Soneta.Types;

// Zarejestrowanie workera dla danego typu
// Workery możemy zarejestrować dla wiersza (np. PozycjaDokHandlowego) oraz dla całej tabeli (np. PozycjeDokHan)
[assembly: Worker(typeof(Soneta.Napoje.OplataNapojuWorker), typeof(PozycjaDokHandlowego))]

namespace Soneta.Napoje
{
    // Worker możemy zarejestrować bez żadnej akcji, za to zdefiniowane publiczne properties będą dostępne do wyciągnięcia
    // za pomocą organizatora listy
    public class OplataNapojuWorker
    {
        [Context]
        public PozycjaDokHandlowego Pozycja { get; set; }

        public KategoriaNapoju Kategoria => OplataNapoju?.Kategoria;

        public Currency Oplata => OplataNapoju != null ? OplataNapoju.Oplata : Currency.Zero;

        private OplataNapoju OplataNapoju => NapojeModule.GetInstance(Pozycja).OplatyNapojow.WgPozycja[Pozycja];
    }
}
