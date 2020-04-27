namespace JewelryShop.Services.Data.Tests
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
    using JewelryShop.Web;
    using JewelryShop.Web.ViewModels;
    using JewelryShop.Web.ViewModels.Ratings;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    [Collection("Mappings collection")]
    public class RatingsServiceTests
    {
        [Fact]
        public void AvarageRatingsIsCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Rating>(new ApplicationDbContext(options.Options));
            var service = new RatingsService(repository);
            service.RateAsync(5, "1", RatingType.Excellent, "excellent").GetAwaiter().GetResult();
            service.RateAsync(5, "1", RatingType.Poor, "poor").GetAwaiter().GetResult();
            service.RateAsync(5, "98", RatingType.Average, "poor").GetAwaiter().GetResult();

            var rates = service.GetAvarageRating(5);
            Assert.Equal(2.0, rates);
        }

        [Fact]
        public void RateCountIsCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Rating>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfRepository<Jewel>(new ApplicationDbContext(options.Options));
            var userRepository = new EfRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var service = new RatingsService(repository);

            userRepository.AddAsync(new ApplicationUser { Id = "testUser", UserName = "tester" }).GetAwaiter().GetResult();
            userRepository.AddAsync(new ApplicationUser { Id = "testUser54545", UserName = "tester" }).GetAwaiter().GetResult();
            userRepository.SaveChangesAsync().GetAwaiter().GetResult();

            jewelRepository.AddAsync(new Jewel { Id = 5, CreatedOn = DateTime.Now.AddDays(-10), Count = 2 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();
            service.RateAsync(5, "testUser", RatingType.Excellent, "excellent").GetAwaiter().GetResult();
            service.RateAsync(5, "testUser", RatingType.Poor, "poor").GetAwaiter().GetResult();
            service.RateAsync(5, "testUser54545", RatingType.Average, "poor").GetAwaiter().GetResult();

            var rates = service.GetAllRatings<RatingsViewModel>(5);
            Assert.Equal(2, rates.Count());
        }
    }

    public class RatingsViewModelTest : IMapFrom<Rating>
    {
        public RatingType Type { get; set; }

        public string Review { get; set; }
    }
}
