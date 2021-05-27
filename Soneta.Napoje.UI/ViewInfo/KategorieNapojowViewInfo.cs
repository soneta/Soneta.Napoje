using Soneta.Business;

namespace Soneta.Napoje.UI
{
    public class KategorieNapojowViewInfo : ViewInfo
    {
        public KategorieNapojowViewInfo()
        {
            // View wiążemy z odpowiednią definicją viewform.xml poprzez property ResourceName
            ResourceName = "KategorieNapojow";

            // Inicjowanie contextu
            InitContext += KategorieNapojowViewInfo_InitContext;

            // Tworzenie view zawierającego konkretne dane
            CreateView += KategorieNapojowViewInfo_CreateView;
        }

        private void KategorieNapojowViewInfo_InitContext(object sender, ContextEventArgs args)
        {
            args.Context.Remove(typeof(KategorieNapojowParams));
            args.Context.TryAdd(() => new KategorieNapojowParams(args.Context));
        }

        private void KategorieNapojowViewInfo_CreateView(object sender, CreateViewEventArgs args)
        {
            KategorieNapojowParams parameters;
            if (!args.Context.Get(out parameters))
                parameters = new KategorieNapojowParams(args.Context);

            args.View = args.Session.GetNapoje().KategorieNapoj.CreateView();
            if (parameters.Blokada == Blokada.Niezablokowane)
                args.View.Condition = new FieldCondition.Equal("Zablokowany", false);
        }

        public enum Blokada
        {
            Niezablokowane,
            Wszystkie
        }

        // Klasa parametrów używanych w filtrze. Musi dziedziczyć z klasy ContextBase
        public class KategorieNapojowParams : ContextBase
        {
            public KategorieNapojowParams(Context context) : base(context) { }

            private Blokada blokada;
            public Blokada Blokada
            {
                get => blokada;
                set
                {
                    blokada = value;
                    Context.Set(this);
                }
            }
        }
    }
}
