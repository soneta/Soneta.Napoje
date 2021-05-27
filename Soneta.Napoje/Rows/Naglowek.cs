// Zdefiniowanie klasy pośredniej (przelotki) jest wymagane dla wszystkich tabel opisanych w business.xml 
// i generowanych do business.cs przez Soneta.Generator.
// W odróżnieniu od business.cs - ten plik możemy bezpiecznie edytować i rozbudowywać

namespace Soneta.Napoje
{
    // W tym pliku definiujemy klasę pośrednią dla subRow, czyli typu reużywanego między różnymi tabelami
    public class Naglowek : NapojeModule.NaglowekRow
    {
    }
}
