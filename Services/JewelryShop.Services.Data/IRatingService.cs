namespace JewelryShop.Services.Data
{
    using JewelryShop.Data.Models.Enums;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRatingService
    {
        Task RateAsync(int jewelId, string userId, RatingType ratingType, string review);

        IEnumerable<T> GetAllRatings<T>(int jewelId);

        double GetAvarageRating(int jewelId);
    }
}
