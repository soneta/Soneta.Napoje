// Zdefiniowanie klasy pośredniej (przelotki) jest wymagane dla wszystkich tabel opisanych w business.xml 
// i generowanych do business.cs przez Soneta.Generator.
// W odróżnieniu od business.cs - ten plik możemy bezpiecznie edytować i rozbudowywać

using Soneta.Business;
using Soneta.Towary;

namespace Soneta.Napoje
{
    // W tym pliku definiujemy klasę pośrednią dla wiersza tabeli
    public class Napoj : NapojeModule.NapojRow
    {
        internal Napoj(RowCreator creator) : base(creator)
        {
        }

        public Napoj(Towar towar) : base(towar)
        {
        }

        public View GetListKategoria()
        {
            var view = Module.KategorieNapoj.CreateView();
            view.Condition = new FieldCondition.Equal("Zablokowany", false);
            return view;
        }
    }
}
