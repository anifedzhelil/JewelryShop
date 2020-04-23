namespace JewelryShop.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;

    public class IndexViewModel
    {
        public IEnumerable<IndexJewelryViewModel> Jewelry { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string Search { get; set; }

        public SortType Sort { get; set; }

        public int Category { get; set; }
    }
}
