﻿namespace JewelryShop.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum CategoryType
    {
        [Display(Name = "Гривни")]
        Bracelet = 1,
        [Display(Name = "Колиета")]
        Necklace = 2,
        [Display(Name = "Обеци")]
        Earrings = 3,
        [Display(Name = "Брошки")]
        Brooch = 4,
        [Display(Name = "Комплекти")]
        Set = 5,
    }
}
