namespace JewelryShop.Web.ViewModels.ShippingAddresses
{
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;
    using System.Collections.Generic;

    public class OrderViewModel : IMapFrom<Order>
    {
        public IEnumerable<OrderDetailsViewModel> OrdersDetails { get; set; }
    }
}
