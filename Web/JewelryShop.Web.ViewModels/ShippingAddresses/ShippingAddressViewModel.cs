namespace JewelryShop.Web.ViewModels.ShippingAddresses
{
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class ShippingAddressViewModel : IMapFrom<ShippingAddress>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string AdditioanalAddress { get; set; }

        public string PostCode { get; set; }

        public InputShippingAddressModel ShippingAddress { get; set; }
    }
}
