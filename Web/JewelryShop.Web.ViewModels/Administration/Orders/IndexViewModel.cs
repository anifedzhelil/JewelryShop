namespace JewelryShop.Web.ViewModels.Administration.Orders
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public ICollection<IndexItemViewModel> Orders { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
