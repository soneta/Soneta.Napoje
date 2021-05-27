using Soneta.Business;

namespace Soneta.Napoje.UI
{
    public class ProducenciNapojowViewInfo : ViewInfo
    {
        public ProducenciNapojowViewInfo()
        {
            // View wiążemy z odpowiednią definicją viewform.xml poprzez property ResourceName
            ResourceName = "ProducenciNapojow";

            // Inicjowanie contextu
            InitContext += ProducenciNapojowViewInfo_InitContext;

            // Tworzenie view zawierającego konkretne dane
            CreateView += ProducenciNapojowViewInfo_CreateView;
        }

        private void ProducenciNapojowViewInfo_InitContext(object sender, ContextEventArgs args)
        {
            args.Context.Remove(typeof(ProducenciNapojowParams));
            args.Context.TryAdd(() => new ProducenciNapojowParams(args.Context));
        }

        private void ProducenciNapojowViewInfo_CreateView(object sender, CreateViewEventArgs args)
        {
            ProducenciNapojowParams parameters;
            if (!args.Context.Get(out parameters))
                parameters = new ProducenciNapojowParams(args.Context);

            args.View = args.Session.GetNapoje().ProducenciNapoj.CreateView();
            if (parameters.Typ == TypProducenta.Opodatkowany)
                args.View.Condition = new FieldCondition.Equal("Typ", TypProducentaNapoju.Opodatkowany);
            else if (parameters.Typ == TypProducenta.Nieopodatkowany)
                args.View.Condition = new FieldCondition.Equal("Typ", TypProducentaNapoju.Nieopodatkowany);
        }

        public enum TypProducenta
        {
            Opodatkowany,
            Nieopodatkowany,
            Wszystkie
        }

        // Klasa parametrów używanych w filtrze. Musi dziedziczyć z klasy ContextBase
        public class ProducenciNapojowParams : ContextBase
        {
            public ProducenciNapojowParams(Context context) : base(context) { }

            private TypProducenta typ;
            public TypProducenta Typ
            {
                get => typ;
                set
                {
                    typ = value;
                    Context.Set(this);
                }
            }
        }
    }
}
