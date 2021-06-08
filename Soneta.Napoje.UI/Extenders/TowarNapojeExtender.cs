using Soneta.Business;
using Soneta.Towary;
using Soneta.Types;

[assembly: Worker(typeof(Soneta.Napoje.UI.TowarNapojeExtender))]

namespace Soneta.Napoje.UI
{
    class TowarNapojeExtender
    {
        [Context]
        public Towar Towar { get; set; }

        public Napoj Napoj
        {
            get
            {
                return NapojeModule.GetInstance(Towar).Napoje.WgTowar[Towar];
            }
        }

        public bool CzyNapoj
        {
            get
            {
                return Napoj != null;
            }
            set
            {
                if (value)
                {
                    NapojeModule.GetInstance(Towar).Napoje.AddRow(new Napoj(Towar));
                }
                else
                {
                    var napoj = NapojeModule.GetInstance(Towar).Napoje.WgTowar[Towar];
                    if (napoj != null) napoj.Delete();
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                // Sprawdzamy czy nasz Towar posiada przelicznik z jednostki podstawowej na litry
                return PrzelicznikiUtils.TryGetWspolczynnik(out Fraction _, Towar, Towar.Module.Jednostki.WgKodu["l"]);
            }
        }
    }
}
