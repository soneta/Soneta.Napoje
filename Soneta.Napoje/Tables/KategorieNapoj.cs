// Zdefiniowanie klasy pośredniej (przelotki) jest wymagane dla wszystkich tabel opisanych w business.xml 
// i generowanych do business.cs przez Soneta.Generator.
// W odróżnieniu od business.cs - ten plik możemy bezpiecznie edytować i rozbudowywać

using Soneta.Business;

// rejestracja obiektu dla przycisku "Nowy"
[assembly: NewRow(typeof(Soneta.Napoje.KategoriaNapoju))]

namespace Soneta.Napoje
{
    // W tym pliku definiujemy klasę pośrednią dla samej tabeli
    public class KategorieNapoj : NapojeModule.KategoriaNapojuTable
    {
    }
}
