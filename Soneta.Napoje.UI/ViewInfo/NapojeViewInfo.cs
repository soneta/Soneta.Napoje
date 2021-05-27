using Soneta.Business;

namespace Soneta.Napoje.UI
{
    public class NapojeViewInfo : ViewInfo
    {
        public NapojeViewInfo()
        {
            // View wiążemy z odpowiednią definicją viewform.xml poprzez property ResourceName
            ResourceName = "Napoje";
        }
    }
}
