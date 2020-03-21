namespace JewelryShop.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class IndexJewelryViewModel : IMapFrom<Jewel>
    {
        public string Description { get; set; }

        public string Name { get; set; }
    }
}
