namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Mapping;

    public class RatingsService : IRatingService
    {
        private readonly IRepository<Rating> ratingsRepository;

        public RatingsService(IRepository<Rating> ratingsRepository)
        {
            this.ratingsRepository = ratingsRepository;
        }

        public async Task RateAsync(int jewelId, string userId, RatingType ratingType, string review)
        {
            var rating = this.ratingsRepository.All()
                .FirstOrDefault(x => x.JewelId == jewelId && x.UserId == userId);

            if (rating != null)
            {
                rating.Type = ratingType;

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
                    Type = ratingType,
                    Review = review,
                };

                await this.ratingsRepository.AddAsync(rating);
            }

            await this.ratingsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllRatings<T>(int jewelId)
        {
            IQueryable<Rating> query = this.ratingsRepository.All()
                .Where(x => x.JewelId == jewelId);

            return query.To<T>().ToArray();
        }

        public double GetAvarageRating(int jewelId)
        {
            IQueryable<Rating> query = this.ratingsRepository.All()
                 .Where(x => x.JewelId == jewelId);
            var average = query.Average(x => (long)x.Type);

            return average;
        }
    }
}
