namespace JewelryShop.Web.ViewModels.Administration.Orders
{
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class OrderDetailsItemViewModel : IMapFrom<OrderDetails>
    {
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string JewelName { get; set; }
    }
}
