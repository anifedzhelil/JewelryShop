﻿namespace JewelryShop.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IndexViewModel
    {
        public IEnumerable<IndexJewelryViewModel> Jewelry { get; set; }
    }
}