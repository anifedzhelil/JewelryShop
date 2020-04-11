namespace JewelryShop.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<IndexJewelryViewModel> Jewelry { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int Category { get; set; }

    }
}
