using Soneta.Business;
using Soneta.Handel;
using Soneta.Napoje;

[assembly: ProgramInitializer(typeof(SchemaInitializer))]

namespace Soneta.Napoje
{
    public class SchemaInitializer : IProgramInitializer
    {
        // Metoda Initialize wykona się raz przy uruchomieniu programu
        // Rejestrujemy w niej handlery do obsługi zdarzeń na pozycji dokumentu handlowego tj. zmiana wartości
        void IProgramInitializer.Initialize()
        {
            HandelModule.PozycjaDokHandlowegoSchema.AddWartoscCyAfterEdit(RejestrujPrzeliczPozycje);
        }

        private void RejestrujPrzeliczPozycje(HandelModule.PozycjaDokHandlowegoRow row)
        {
            // Handler uruchamiany jest dla różnych stanów obiektów, dlatego istotne jest sprawdzenie czy nadal IsLive
            if (row.IsLive && row is PozycjaDokHandlowego pozycja)
            {
                // Tutaj możemy np. zarejestorwać event wykonujący naliczanie opłaty po każdej zmianie wartości
                // pozycja.Session.Events.Add(_ => NapojeModule.GetInstance(pozycja).OplatyNapojow.NaliczOplate(pozycja));
            }
        }
    }
}
