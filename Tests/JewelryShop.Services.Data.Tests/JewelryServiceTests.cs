namespace JewelryShop.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels.Administration.Jewelry;
    using Moq;
    using Xunit;

    [Collection("Mappings collection")]
    public class JewelryServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Jewel>> mockRepository;
        private readonly Mock<IDeletableEntityRepository<OrderDetails>> mockOrderDetailsRepository;
        private IJewelryService jewelryService;

        public JewelryServiceTests()
        {
            this.mockRepository = new Mock<IDeletableEntityRepository<Jewel>>();
            this.mockOrderDetailsRepository = new Mock<IDeletableEntityRepository<OrderDetails>>();
        }

        [Fact]
        public void GetCountShouldReturnCorrectNumberTests()
        {
            this.mockRepository.Setup(r => r.All()).Returns(new List<Jewel>
                                                        {
                                                            new Jewel(),
                                                            new Jewel(),
                                                        }.AsQueryable());
            this.jewelryService = new JewelryService(this.mockRepository.Object, null);
            Assert.Equal(2, this.jewelryService.GetAdminJewelryCount(FilterType.All));
            this.mockRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public void GetArchivedJewelryCountShouldReturnCorrectNumberTests()
        {
            this.mockRepository.Setup(r => r.All()).Returns(new List<Jewel>
                                                        {
                                                            new Jewel() { Id = 21, IsArchived = true },
                                                            new Jewel() { Id = 22, IsArchived = false },
                                                            new Jewel { Id = 23, IsArchived = false },
                                                        }.AsQueryable());

            this.jewelryService = new JewelryService(this.mockRepository.Object, null);
            Assert.Equal(1, this.jewelryService.GetAdminJewelryCount(FilterType.Archived));
            this.mockRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public void DeleteByjewelIdIsSuccessTests()
        {
            this.mockRepository.Setup(r => r.All()).Returns(new List<Jewel>
                                                        {
                                                            new Jewel() { Id = 1, Name = "бижу01", Description = "описание01", Category = CategoryType.Bracelet, Count = 4,  Price = 50 },
                                                            new Jewel() { Id = 2, Name = "бижу02", Description = "описание02", Category = CategoryType.Bracelet, Count = 4,  Price = 50 },
                                                        }.AsQueryable());

            this.jewelryService = new JewelryService(this.mockRepository.Object, this.mockOrderDetailsRepository.Object);
            Assert.True(this.jewelryService.DeleteByIdAsync(1).GetAwaiter().GetResult());
        }

        [Fact]
        public void DeleteByjewelIdReturnFalseTests()
        {
            this.mockRepository.Setup(r => r.All()).Returns(new List<Jewel>
                                                        {
                                                            new Jewel() { Id = 1, Name = "бижу01", Description = "описание01", Category = CategoryType.Bracelet, Count = 4,  Price = 50 },
                                                            new Jewel() { Id = 2, Name = "бижу02", Description = "описание02", Category = CategoryType.Bracelet, Count = 4,  Price = 50 },
                                                        }.AsQueryable());

            this.mockOrderDetailsRepository.Setup(r => r.All()).Returns(new List<OrderDetails>
                                                        {
                                                            new OrderDetails() { Id = 1656, JewelId = 2, Price = 20, Quantity = 1 },
                                                        }.AsQueryable());

            this.jewelryService = new JewelryService(this.mockRepository.Object, this.mockOrderDetailsRepository.Object);
            Assert.False(this.jewelryService.DeleteByIdAsync(2).GetAwaiter().GetResult());
        }

        [Fact]
        public void GetAllActivedByCategories()
        {
            this.mockRepository.Setup(r => r.All()).Returns(new List<Jewel>
                                                         {
                                                             new Jewel() { Id = 1, Name = "бижу01", Description = "описание01", Category = CategoryType.Bracelet, Count = 4,  Price = 50 },
                                                             new Jewel() { Id = 2, Name = "бижу02", Description = "описание02", Category = CategoryType.Bracelet, Count = 4,  Price = 50 },
                                                         }.AsQueryable());

            this.jewelryService = new JewelryService(this.mockRepository.Object, this.mockOrderDetailsRepository.Object);
            var result = this.jewelryService.GetAllActivedByCategories((int)CategoryType.Bracelet);
            this.mockRepository.Verify(x => x.All(), Times.Once);
        }
    }
}
