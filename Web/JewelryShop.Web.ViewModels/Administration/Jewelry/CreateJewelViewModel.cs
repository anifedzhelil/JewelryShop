namespace JewelryShop.Web.ViewModels.Administration.Jewelry
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class CreateJewelViewModel
    {
        [Required(ErrorMessage = "Моля въведете име")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Моля въведете цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моля въведете описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Моля изберете тип")]
        public int? JewelType { get; set; }

        [DataType(DataType.Currency)]
        public decimal SalePrice { get; set; }

        public DateTime SaleDate { get; set; }

        [Required(ErrorMessage = "Моля въведете брой")]
        public int Count { get; set; }

        public int Category { get; set; }

        public bool IsArchived { get; set; }
    }
}
