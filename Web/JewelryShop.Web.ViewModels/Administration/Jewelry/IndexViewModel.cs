namespace JewelryShop.Web.ViewModels.Administration.Jewelry
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IndexViewModel
    {
        public IEnumerable<IndexJewelryViewModel> Jewelry { get; set; }
    }
}
