namespace JewelryShop.Web.ViewModels.Ratings
{
    using JewelryShop.Data.Models.Enums;

    public class CreateRatingViewModel
    {
        public int JewelId { get; set; }

        public RatingType Rating { get; set; }

        public string Review { get; set; }
    }
}
