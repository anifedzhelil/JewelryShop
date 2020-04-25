namespace JewelryShop.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Data.Repositories;
    using JewelryShop.Services.Data;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels.Ratings;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class RatingsServiceTests
    {
        [Fact]
        public async Task AvarageRatingsIsCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Rating>(new ApplicationDbContext(options.Options));
            var service = new RatingsService(repository);
            await service.RateAsync(5, "1", RatingType.Excellent, "excellent");
            await service.RateAsync(5, "1", RatingType.Poor, "poor");
            await service.RateAsync(5, "98", RatingType.Average, "poor");

            AutoMapperConfig.RegisterMappings(typeof(RatingsViewModel).Assembly);

            var rates = service.GetAvarageRating(5);
            Assert.Equal(2.0, rates);
        }

        [Fact]
        public async Task RateCountIsCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Rating>(new ApplicationDbContext(options.Options));
            var service = new RatingsService(repository);
            await service.RateAsync(5, "1", RatingType.Excellent, "excellent");
            await service.RateAsync(5, "1", RatingType.Poor, "poor");
            await service.RateAsync(5, "98", RatingType.Average, "poor");
            repository.SaveChangesAsync().GetAwaiter().GetResult();

            AutoMapperConfig.RegisterMappings(typeof(RatingsViewModelTest).Assembly);

            var rates = service.GetAllRatings<RatingsViewModelTest>(5);
            Assert.Equal(2, rates.Count());
        }
    }

    public class RatingsViewModelTest : IMapFrom<Rating>
    {
        public RatingType Type { get; set; }

        public string Review { get; set; }
    }
}
