namespace JewelryShop.Web.ViewModels.ShippingAddresses
{
    using System.Collections.Generic;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class OrderViewModel : IMapFrom<Order>
    {
        public IEnumerable<OrderDetailsViewModel> OrdersDetails { get; set; }
    }
}
