namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class RatingsService : IRatingService
    {
        private readonly IRepository<Rating> ratingsRepository;

        public RatingsService(IRepository<Rating> ratingsRepository)
        {
            this.ratingsRepository = ratingsRepository;
        }

        public async Task RateAsync(int jewelId, string userId, int ratingType, string review)
        {
            var rating = this.ratingsRepository.All()
                .FirstOrDefault(x => x.JewelId == jewelId && x.UserId == userId);
            var type = (RatingType)Enum.ToObject(typeof(RatingType), ratingType);
            if (rating != null)
            {
                rating.Type = type;

                if (review != null)
                {
                    rating.Review = review;
                }
            }
            else
            {
                rating = new Rating()
                {
                    JewelId = jewelId,
                    UserId = userId,
                    Type = type,
                    Review = review,
                };

                await this.ratingsRepository.AddAsync(rating);
            }

            await this.ratingsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllRatings<T>(int jewelId, string userId)
        {
            IQueryable<Rating> query = this.ratingsRepository.All()
                .Where(x => x.JewelId == jewelId && x.UserId == userId);

            return query.To<T>().ToArray();
        }

        public double GetAvarageRating(int jewelId, string userId)
        {
            IQueryable<Rating> query = this.ratingsRepository.All()
                 .Where(x => x.JewelId == jewelId && x.UserId == userId);
            var average = query.Average(x => (long)x.Type);

            return average;
        }
    }
}
