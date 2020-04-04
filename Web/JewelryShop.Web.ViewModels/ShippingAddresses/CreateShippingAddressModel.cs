namespace JewelryShop.Web.ViewModels.ShippingAddresses
{
    using System.ComponentModel.DataAnnotations;

    public class CreateShippingAddressModel
    {
        [Required(ErrorMessage = "Моля въведете име")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Моля въведете фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Моля въведете град")]
        public string City { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Моля въведете телефонен номер")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Моля въведете адрес")]
        public string Address { get; set; }

        public string AdditionalAddress { get; set; }

        [Required(ErrorMessage = "Моля въведете пощенски код")]
        [RegularExpression("[0-9]{4}")]
        [StringLength(4, MinimumLength = 4)]
        public string PostCode { get; set; }
    }
}
