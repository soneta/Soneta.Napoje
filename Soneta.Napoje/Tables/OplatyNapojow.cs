// Zdefiniowanie klasy pośredniej (przelotki) jest wymagane dla wszystkich tabel opisanych w business.xml 
// i generowanych do business.cs przez Soneta.Generator.
// W odróżnieniu od business.cs - ten plik możemy bezpiecznie edytować i rozbudowywać

using Soneta.Handel;
using Soneta.Magazyny;

namespace Soneta.Napoje
{
    // W tym pliku definiujemy klasę pośrednią dla samej tabeli
    public class OplatyNapojow : NapojeModule.OplataNapojuTable
    {
        // Metoda pomocnicza tworząca lub usuwająca obiekt OplataNapoju rozszerzający pozycję dokumentu handlowego
        public void NaliczOplate(PozycjaDokHandlowego pozycja)
        {
            // Opłata naliczana tylko dla pozycji dokumentów rozchodu
            if (pozycja.Dokument.KierunekMagazynu == KierunekPartii.Rozchód)
            {
                var oplataNapoju = WgPozycja[pozycja];
                var napoj = Module.Napoje.WgTowar[pozycja.Towar];

                if (oplataNapoju == null && napoj != null)
                {
                    oplataNapoju = DodajOplateNapoju(pozycja, napoj.Kategoria);
                }
                else if (oplataNapoju != null && napoj == null)
                {
                    UsunOplateNapoju(oplataNapoju);
                    oplataNapoju = null;
                }

                if (oplataNapoju != null)
                    oplataNapoju.NaliczOplate();
            }
        }

        private OplataNapoju DodajOplateNapoju(PozycjaDokHandlowego pozycja, KategoriaNapoju kategoria)
        {
            using (var tran = pozycja.Session.Logout(true))
            {
                var oplataNapoju = new OplataNapoju(pozycja, kategoria);
                AddRow(oplataNapoju);
                tran.Commit();
                return oplataNapoju;
            }
        }

        private void UsunOplateNapoju(OplataNapoju oplataNapoju)
        {
            using (var tran = oplataNapoju.Session.Logout(true))
            {
                oplataNapoju.Delete();
                tran.Commit();
            }
        }
    }
}
