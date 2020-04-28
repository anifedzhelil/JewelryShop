namespace JewelryShop.Web.ViewModels.ShippingAddresses
{
    using System.Collections.Generic;

    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Mapping;

    public class IndexViewModel : IMapFrom<ShippingAddress>
    {
        public int OrderId { get; set; }

        public ICollection<ShippingAddressViewModel> ShippingAddressesCollection { get; set; }

        public InputShippingAddressModel ShippingAddress { get; set; }

        public string DeliveryCheked { get; set; }

        public IEnumerable<OrderDetailsViewModel> OrdersDetails { get; set; }
    }
}
