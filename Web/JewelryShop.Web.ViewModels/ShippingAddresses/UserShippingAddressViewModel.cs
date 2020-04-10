namespace JewelryShop.Web.ViewModels.ShippingAddresses
{
    using System.Collections.Generic;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class UserShippingAddressViewModel : IMapFrom<ShippingAddress>
    {
        public IEnumerable<ShippingAddressViewModel> ShippingAddressesCollection { get; set; }

        public InputShippingAddressModel ShippingAddress { get; set; }
    }
}
