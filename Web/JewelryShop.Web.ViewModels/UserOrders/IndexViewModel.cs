namespace JewelryShop.Web.ViewModels.UserOrders
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public ICollection<IndexItemViewModel> Orders { get; set; }
    }
}
