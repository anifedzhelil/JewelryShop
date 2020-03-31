namespace JewelryShop.Services.Data
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRatingService
    {
        Task RateAsync(int jewelId, string userId, int ratingType, string review);

        IEnumerable<T> GetAllRatings<T>(int jewelId);

        double GetAvarageRating(int jewelId);
    }
}
