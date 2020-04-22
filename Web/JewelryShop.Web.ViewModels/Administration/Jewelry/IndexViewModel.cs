namespace JewelryShop.Web.ViewModels.Administration.Jewelry
{
    using System.Collections.Generic;

    using JewelryShop.Data.Models.Enums;

    public class IndexViewModel
    {
        public IEnumerable<IndexJewelryViewModel> Jewelry { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public FilterType Filter { get; set; }

        public SortType Sort { get; set; }
    }
}
