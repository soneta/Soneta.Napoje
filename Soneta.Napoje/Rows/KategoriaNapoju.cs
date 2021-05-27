// Zdefiniowanie klasy pośredniej (przelotki) jest wymagane dla wszystkich tabel opisanych w business.xml 
// i generowanych do business.cs przez Soneta.Generator.
// W odróżnieniu od business.cs - ten plik możemy bezpiecznie edytować i rozbudowywać

using Soneta.Business;
using Soneta.Types;

namespace Soneta.Napoje
{
    // W tym pliku definiujemy klasę pośrednią dla wiersza tabeli
    public class KategoriaNapoju : NapojeModule.KategoriaNapojuRow
    { 
        // Przeciążenie metody ToString spowoduje przyjazne wyświetlanie zawartości kontrolki
        public override string ToString()
        {
            return Naglowek.Nazwa;
        }

        // Dodajemy dodatkową logikę przy ustawianiu flagi zablokowany na zerowanie opłaty
        public override bool Zablokowany
        {
            get => base.Zablokowany;
            set
            {
                base.Zablokowany = value;
                Oplata = Percent.Zero;
            }
        }

        public bool IsReadOnlyOplata()
        {
            return Zablokowany;
        }

        // Ponieważ w business.xml został dla pola zdefiniowany weryfikator dyrektywą
        //   <verifier>KategoriaNapoju.OplataVerifier</verifier>,
        // definiujemy go jako klasę zagnieżdzoną klasy KategoriaNapoju
        internal class OplataVerifier : RowVerifier<KategoriaNapoju>
        {
            internal OplataVerifier(KategoriaNapoju kategoria) : base(kategoria)
            {
            }

            public override string Description => "Wymagane jest, aby opłata była niewiększa niż 20%";

            protected override bool IsValid()
            {
                return Row.Oplata <= 0.2m;
            }
        }
    }
}
