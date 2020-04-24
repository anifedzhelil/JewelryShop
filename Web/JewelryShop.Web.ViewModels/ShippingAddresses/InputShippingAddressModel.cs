namespace JewelryShop.Web.ViewModels.ShippingAddresses
{
    using System.ComponentModel.DataAnnotations;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class InputShippingAddressModel : IMapFrom<ShippingAddress>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Моля въведете име")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Моля въведете фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Моля въведете град")]
        public string City { get; set; }

        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Въведете валиден телефон.")]
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

        public string UserID { get; set; }
    }
}
