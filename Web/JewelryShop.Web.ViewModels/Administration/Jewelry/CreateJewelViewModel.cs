namespace JewelryShop.Web.ViewModels.Administration.Jewelry
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using JewelryShop.Web.ViewModels.ValidationAttributes;

    public class CreateJewelViewModel
    {
        [Required(ErrorMessage = "Моля въведете име")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Моля въведете цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моля въведете описание")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [SalePriceLessThan("Price", "SaleDate")]
        public decimal? SalePrice { get; set; }

        [SaleDateLessThan("SalePrice")]
        [DataType(DataType.DateTime)]
        public DateTime? SaleDate { get; set; }

        [Required(ErrorMessage = "Моля въведете брой")]
        public int Count { get; set; }

        [Required(ErrorMessage = "Моля  изберете категория")]
        public int Category { get; set; }

        public bool IsArchived { get; set; }
    }
}
