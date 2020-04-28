namespace JewelryShop.Services.Data.Tests
{
    using System;

    using JewelryShop.Data;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Repositories;
    using JewelryShop.Web.ViewModels.ShippingAddresses;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    [Collection("Mappings collection")]
    public class ShippingAddressTests
    {
        private readonly EfDeletableEntityRepository<ShippingAddress> repository;

        public ShippingAddressTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.repository = new EfDeletableEntityRepository<ShippingAddress>(new ApplicationDbContext(options.Options));
        }

        [Fact]
        public void AddShippingAddressIsEqualToExpected()
        {
            var service = new ShippingAddressService(this.repository);
            service.AddAddressAsync(new InputShippingAddressModel() { Id = 2, UserID = "testUser502021", City = "София", Address = "ж.к Трошево" }).GetAwaiter().GetResult();
            var resultAddresses = service.GetUserAllShippingAddress<ShippingAddressViewModel>("testUser502021");

            Assert.Equal(1, resultAddresses.Count);
        }

        [Fact]
        public void AddOfficeAddressIsEqualToExpected()
        {
            var service = new ShippingAddressService(this.repository);
            var result = service.AddOfficeAddressAsync("Ани", "Петров", "0556656565656", "София ж.к Трошево").GetAwaiter().GetResult();
            Assert.Equal(1, result);
        }

        [Fact]
        public void DeleteShippingAddressIsSuccess()
        {
            this.repository.AddAsync(new ShippingAddress() { Id = 2, UserID = "testUser5021", City = "София", Address = "ж.к Трошево" }).GetAwaiter().GetResult();
            this.repository.SaveChangesAsync().GetAwaiter().GetResult();
            this.repository.AddAsync(new ShippingAddress() { Id = 3, UserID = "testUser5021", City = "Варна", Address = "ж.к Трошево" }).GetAwaiter().GetResult();
            this.repository.SaveChangesAsync().GetAwaiter().GetResult();

            var service = new ShippingAddressService(this.repository);
            service.DeleteShippingAddress(2).GetAwaiter().GetResult();
            var resultAddresses = service.GetUserAllShippingAddress<ShippingAddressViewModel>("testUser5021");

            Assert.Equal(1, resultAddresses.Count);
        }
    }
}
