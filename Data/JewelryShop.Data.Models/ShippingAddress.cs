namespace JewelryShop.Data.Models
{
    using JewelryShop.Data.Common.Models;

    public class ShippingAddress : BaseDeletableModel<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string? AdditionalAddress { get; set; }

        public string Phone { get; set; }

        public string PostCode { get; set; }

        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Order Order { get; set; }
    }
}
