namespace JewelryShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class JewelryCategories
    {
        public enum JewelryCategoriesEnum
        {
            [Display(Name = "Гривни")]
            Bracelet,
            [Display(Name = "Колиета")]
            Necklace,
            [Display(Name = "Обеци")]
            Earrings,
            [Display(Name = "Брошки")]
            Brooch,
            [Display(Name = "Комплекти")]
            Set,
        }
    }
}
