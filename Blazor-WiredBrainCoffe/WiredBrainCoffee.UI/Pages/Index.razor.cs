using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace WiredBrainCoffee.UI.Pages
{
    public partial class Index
    {
        [Inject ]
        public NavigationManager NavigationManager { get; set; }
        private void ApplyPromo()
        {
            NavigationManager.NavigateTo("/order",new NavigationOptions()
            {
                HistoryEntryState = "Discount"
            }  );
        }
    }
}
