namespace JewelryShop.Web.ViewModels.Ratings
{
    using System.Collections.Generic;

    public class RatingResponseModel
    {
        public double AvarageRating { get; set; }

        public IEnumerable<RatingsViewModel> JewelryRatings { get; set; }
    }
}
