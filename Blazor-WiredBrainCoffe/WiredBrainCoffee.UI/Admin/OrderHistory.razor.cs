using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using WiredBrainCoffee.Models;
using WiredBrainCoffee.UI.Services;

namespace WiredBrainCoffee.UI.Admin
{
    public partial class OrderHistory
    {
        [Inject]
        public IOrderService OrderService { get; set; }

        IQueryable<Order> items = new List<Order>().AsQueryable();

        PaginationState pagination = new PaginationState() { ItemsPerPage = 4 };

        private string nameFilter = string.Empty;

        private IQueryable<Order> FilteredItems => items.Where(p => p.LastName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));

        protected override async Task OnInitializedAsync()
        {
            items = (await OrderService.GetOrders()).AsQueryable();
        }
    }
}
