using JewelryShop.Data.Models.Enums;

namespace JewelryShop.Web.ViewModels.Ratings
{
    public class CreateRatingViewModel
    {
        public int JewelId { get; set; }

        public RatingType Rating { get; set; }

        public string Review { get; set; }
    }
}
