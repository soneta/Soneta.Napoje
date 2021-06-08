using Soneta.Business;
using Soneta.Handel;
using Soneta.Magazyny;
using Soneta.Tools;
using Soneta.Types;
using System;
using System.Xml.Serialization;

// Zarejestrowanie workera dla danego typu
// Workery możemy zarejestrować dla wiersza (np. DokumentHandlowy) oraz dla całej tabeli (np. DokHandlowe)
[assembly: Worker(typeof(Soneta.Napoje.NaliczOplateWorker), typeof(DokHandlowe))]

namespace Soneta.Napoje
{
    public class NaliczOplateWorker
    {
        // Parametry workera, okno z wyborem parametrów zostanie wyświetlone przed wywołaniem akcji
        [Context]
        public NaliczOplateParams Params { get; set; }

        // Zarejstrowanie akcji na interfejsie użytkownika
        // Przycisk będzie widoczny w menu Czynności -> Napoje -> Opłata dla napojów
        // Parametr Mode służy do dodatkowej konfiguracji, np. Progress wyświetli nam okienko postępu podczas trwania akcji
        // natomiast SingleSession oznacza, że dana akcja będzie wywołana w ramach istniejącej sesji
        // ConfirmFinished wyświetli nam informację po zakończeniu akcji
        [Action("Napoje/Nalicz opłatę dla napojów",
            Mode = ActionMode.SingleSession | ActionMode.Progress | ActionMode.ConfirmFinished)]
        public void NaliczOplate()
        {
            var condition = new FieldCondition.GreaterEqual("Dokument.Data", Params.Okres.From) &
                new FieldCondition.LessEqual("Dokument.Data", Params.Okres.To) &
                new FieldCondition.Equal("Dokument.KierunekMagazynu", KierunekPartii.Rozchód);
            if (Params.Definicja != null)
                condition &= new FieldCondition.Equal("Dokument.Definicja", Params.Definicja);

            var handelModule = Params.Session.GetHandel();
            var napojeModule = Params.Session.GetNapoje();

            using (var tran = Params.Session.Logout(true))
            {
                // Aby wyciągnąć SubTable z tabeli PozycjeDokHan podajemy nazwę dowolnego klucza
                foreach (var pozycja in handelModule.PozycjeDokHan.WgDaty[condition])
                {
                    napojeModule.OplatyNapojow.NaliczOplate(pozycja);
                }
                tran.Commit();
            }
        }

        // Aby okno z parametrami wyświetliło się przed wywołaniem akcji workera klasa musi dziedziczyć po ContextBase
        // SerializableContextBase pozwala nam dodatkowo zapamiętać bieżące parametry
        [Caption("Parametry do wyliczenia opłaty dla napojów")]
        public class NaliczOplateParams : SerializableContextBase
        {
            public NaliczOplateParams(Context context) : base(context) { }

            // Priority - ustala kolejność wyświetlania pól na formatce
            // XmlIgnore - oznacza że SerializableContextBase ma nie zapamiętywać bieżącego ustawienia
            private FromTo okres = new FromTo(Date.Today, Date.Today);
            [Priority(1)]
            [XmlIgnore]
            public FromTo Okres
            {
                get => okres;
                set
                {
                    if (value == FromTo.Empty)
                        okres = FromTo.All;
                    else
                        okres = value;
                    OnChanged();
                }
            }

            private DefDokHandlowego definicja;
            [Priority(2)]
            [Caption("Definicja dokumentu")]
            public DefDokHandlowego Definicja
            {
                get => definicja;
                set
                {
                    definicja = value;
                    OnChanged();
                }
            }

            // Pobranie własnej listy definicji, w naszym przypadku chcemy wyświetlać tylko definicji dokumentów rozchodu
            public View GetListDefinicja()
            {
                var view = HandelModule.GetInstance(Context).DefDokHandlowych.CreateView();
                view.Condition = new FieldCondition.Equal("KierunekMagazynu", KierunekPartii.Rozchód);
                return view;
            }
        }
    }
}
