// Zdefiniowanie klasy pośredniej (przelotki) jest wymagane dla wszystkich tabel opisanych w business.xml 
// i generowanych do business.cs przez Soneta.Generator.
// W odróżnieniu od business.cs - ten plik możemy bezpiecznie edytować i rozbudowywać

namespace Soneta.Napoje
{
    // W tym pliku definiujemy klasę pośrednią dla wiersza tabeli
    public class ProducentNapoju : NapojeModule.ProducentNapojuRow
    {
        // Przeciążenie metody ToString spowoduje przyjazne wyświetlanie zawartości kontrolki
        public override string ToString()
        {
            return Naglowek.Nazwa;
        }
    }
}
