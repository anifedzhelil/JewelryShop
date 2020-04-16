namespace JewelryShop.Web.ViewModels.Ratings
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Mapping;

    public class RatingsViewModel : IMapFrom<Rating>
    {
        public RatingType Type { get; set; }

        public string Review { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserUserName { get; set; }
    }
}
