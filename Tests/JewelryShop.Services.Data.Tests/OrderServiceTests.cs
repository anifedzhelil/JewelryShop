namespace JewelryShop.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using JewelryShop.Data;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Data.Repositories;
    using JewelryShop.Services.Mapping;

    using JewelryShop.Web;
    using JewelryShop.Web.ViewModels.Administration.Orders;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    [Collection("Mappings collection")]
    public class OrderServiceTests
    {
        [Fact]
        public async System.Threading.Tasks.Task JewelQuantityLowAvilableTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfDeletableEntityRepository<Jewel>(new ApplicationDbContext(options.Options));
            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                null,
                jewelRepository);

            repository.AddAsync(new Order { Id = 2, UserID = "testerUser01", Status = OrderStatusType.Created }).GetAwaiter().GetResult();
            repository.SaveChangesAsync().GetAwaiter().GetResult();

            orderDetailsRepository.AddAsync(new OrderDetails { Id = 8, JewelId = 5, OrderId = 2, Quantity = 4 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();
            jewelRepository.AddAsync(new Jewel { Id = 5, Name = "jewel", Count = 2 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            var result = await service.CheckJewelQuantityAsync(2);

            Assert.Equal((int)result, (int)StockType.LowAvailability);
        }

        [Fact]
        public void OrderDetailsCountIsEqualToExpected()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfDeletableEntityRepository<Jewel>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                null,
                jewelRepository);

            repository.AddAsync(new Order { Id = 2, UserID = "testerUser01", Status = OrderStatusType.Created }).GetAwaiter().GetResult();
            repository.SaveChangesAsync().GetAwaiter().GetResult();

            orderDetailsRepository.AddAsync(new OrderDetails { Id = 8, JewelId = 5, OrderId = 2, Quantity = 2 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();
            jewelRepository.AddAsync(new Jewel { Id = 5, Name = "jewel", Count = 0 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            service.CheckJewelQuantityAsync(2).GetAwaiter().GetResult();
            int orderCount = service.GetActiveOrderCount("testerUser01");
            Assert.Equal(0, orderCount);
        }

        [Fact]
        public void AddOrderDetailsIsEqualToExpectedAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfDeletableEntityRepository<Jewel>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                null,
                jewelRepository);

            repository.AddAsync(new Order { Id = 15, UserID = "testuser265", Status = OrderStatusType.Created }).GetAwaiter().GetResult();
            repository.SaveChangesAsync().GetAwaiter().GetResult();
            orderDetailsRepository.AddAsync(new OrderDetails { Id = 10, JewelId = 15, OrderId = 15, Quantity = 2 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();
            orderDetailsRepository.AddAsync(new OrderDetails { Id = 11, JewelId = 18, OrderId = 15, Quantity = 2 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();
            jewelRepository.AddAsync(new Jewel { Id = 15, Count = 5 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();
            jewelRepository.AddAsync(new Jewel { Id = 18, Count = 3 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            service.AddProductAsync("testuser265", 15, 3).GetAwaiter().GetResult();
            var orders = service.GetOrderById<JewelryShop.Web.ViewModels.Administration.Orders.OrderDetailsViewModel>(15);

            Assert.Equal(2, orders.OrdersDetails.Count);
        }

        [Fact]
        public void AddGuestOrderDetailsIsEqualToExpectedAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfDeletableEntityRepository<Jewel>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                null,
                jewelRepository);

            jewelRepository.AddAsync(new Jewel { Id = 5, Count = 2 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();
            jewelRepository.AddAsync(new Jewel { Id = 8, Count = 3 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            service.AddGuestProductAsync("guestuser265", 8, 3).GetAwaiter().GetResult();
            service.AddGuestProductAsync("guestuser265", 5, 2).GetAwaiter().GetResult();
            service.AddGuestProductAsync("guestuser265", 5, 3).GetAwaiter().GetResult();

            var orders = service.GetActiveGuestOrder<JewelryShop.Web.ViewModels.ShippingAddresses.OrderViewModel>("guestuser265");
            Assert.Equal(2, orders.OrdersDetails.Count);

            var resultCount = service.GetActiveGuestOrderCount("guestuser265");
            Assert.Equal(5, resultCount);
        }

        [Fact]
        public void AddUserOrderDetailsIsEqualToExpectedTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfDeletableEntityRepository<Jewel>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                null,
                jewelRepository);

            jewelRepository.AddAsync(new Jewel { Id = 5, Count = 2 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();
            jewelRepository.AddAsync(new Jewel { Id = 2, Count = 1 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            service.AddProductAsync("testUser545", 5, 3).GetAwaiter().GetResult();
            service.AddProductAsync("testUser545", 2, 2).GetAwaiter().GetResult();

            var orders = service.GetActiveOrder<JewelryShop.Web.ViewModels.ShippingAddresses.OrderViewModel>("testUser545");

            Assert.Equal(2, orders.OrdersDetails.Count);

            var resultCount = service.GetActiveOrderCount("testUser545");
            Assert.Equal(3, resultCount);
        }

        [Fact]
        public void UpdateOrderDetailsWhenUserLoginTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfDeletableEntityRepository<Jewel>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                userRepository,
                jewelRepository);

            userRepository.AddAsync(new ApplicationUser { Email = "testerUser@abv.bg", Id = "tester4545454" });
            userRepository.SaveChangesAsync().GetAwaiter().GetResult();

            jewelRepository.AddAsync(new Jewel { Id = 5, Count = 2 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            jewelRepository.AddAsync(new Jewel { Id = 8, Count = 3 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            service.AddGuestProductAsync("guestuser265", 8, 1).GetAwaiter().GetResult();
            service.AddGuestProductAsync("guestuser265", 5, 1).GetAwaiter().GetResult();
            service.AddProductAsync("tester4545454", 5, 2).GetAwaiter().GetResult();

            service.UpdateUserOrderAsync("testerUser@abv.bg", "guestuser265").GetAwaiter().GetResult();

            var resultUser = service.GetActiveOrderCount("tester4545454");
            Assert.Equal(3, resultUser);

            var resultCount = service.GetActiveGuestOrderCount("guestuser265");
            Assert.Equal(0, resultCount);
        }

        [Fact]
        public void UpdateOrderDetailsWithEmptyUserShoppingCartWhenUserLoginTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfDeletableEntityRepository<Jewel>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                userRepository,
                jewelRepository);

            userRepository.AddAsync(new ApplicationUser { Email = "testerUser@abv.bg", Id = "tester4545454" });
            userRepository.SaveChangesAsync().GetAwaiter().GetResult();

            jewelRepository.AddAsync(new Jewel { Id = 5, Count = 2 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            jewelRepository.AddAsync(new Jewel { Id = 8, Count = 3 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            service.AddGuestProductAsync("guestuser265", 8, 1).GetAwaiter().GetResult();
            service.AddGuestProductAsync("guestuser265", 5, 2).GetAwaiter().GetResult();

            service.UpdateUserOrderAsync("testerUser@abv.bg", "guestuser265").GetAwaiter().GetResult();

            var resultUserCount = service.GetActiveOrderCount("tester4545454");
            Assert.Equal(3, resultUserCount);
        }

        [Fact]
        public void CheckOrderIsCompletedSuccessTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));
            var jewelRepository = new EfDeletableEntityRepository<Jewel>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                userRepository,
                jewelRepository);

            userRepository.AddAsync(new ApplicationUser { Email = "testerUser@abv.bg", Id = "testuser587" });
            userRepository.SaveChangesAsync().GetAwaiter().GetResult();
            repository.AddAsync(new Order { Id = 15, UserID = "testuser587", Status = OrderStatusType.Created }).GetAwaiter().GetResult();
            repository.SaveChangesAsync().GetAwaiter().GetResult();

            orderDetailsRepository.AddAsync(new OrderDetails { Id = 10, JewelId = 15, OrderId = 15, Quantity = 2 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();
            orderDetailsRepository.AddAsync(new OrderDetails { Id = 11, JewelId = 18, OrderId = 15, Quantity = 2 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();

            jewelRepository.AddAsync(new Jewel { Id = 15, Count = 5, Price = 20 }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();
            jewelRepository.AddAsync(new Jewel { Id = 18, Count = 3, Price = 30, SalePrice = 25, SaleDate = DateTime.Now.AddDays(20) }).GetAwaiter().GetResult();
            jewelRepository.SaveChangesAsync().GetAwaiter().GetResult();

            service.CompleteOrderAsync(15, DeliveryType.Econt, 4, 6).GetAwaiter().GetResult();

            var resultById = service.GetOrderById<OrderDetailsViewModel>(15);
            Assert.Equal(2, resultById.OrdersDetails.Count);

            var orders = service.GetUserAllCompletedOrders<IndexItemViewModel>("testuser587");

            decimal totalSum = 0;
            foreach (var item in orders)
            {
                totalSum += item.TotalSum;
            }

            Assert.Equal(51, totalSum);
        }

        [Fact]
        public void DeleteOrderDetailsOnlyTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                null,
                null);

            repository.AddAsync(new Order { Id = 15, UserID = "testuser587", Status = OrderStatusType.Created }).GetAwaiter().GetResult();
            repository.SaveChangesAsync().GetAwaiter().GetResult();

            orderDetailsRepository.AddAsync(new OrderDetails { Id = 10, JewelId = 15, OrderId = 15, Quantity = 2 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();
            orderDetailsRepository.AddAsync(new OrderDetails { Id = 11, JewelId = 18, OrderId = 15, Quantity = 3 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();

            service.DeleteOrderDetail(10).GetAwaiter().GetResult();

            int orderCount = service.GetActiveOrderCount("testuser587");
            Assert.Equal(3, orderCount);
        }

        [Fact]
        public void DeleteOrderDetailsAndOrderTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Order>(new ApplicationDbContext(options.Options));
            var orderDetailsRepository = new EfDeletableEntityRepository<OrderDetails>(new ApplicationDbContext(options.Options));

            var service = new OrdersService(
                repository,
                orderDetailsRepository,
                null,
                null);

            repository.AddAsync(new Order { Id = 15, UserID = "testuser587", Status = OrderStatusType.Created }).GetAwaiter().GetResult();
            repository.SaveChangesAsync().GetAwaiter().GetResult();

            orderDetailsRepository.AddAsync(new OrderDetails { Id = 10, JewelId = 15, OrderId = 15, Quantity = 2 }).GetAwaiter().GetResult();
            orderDetailsRepository.SaveChangesAsync().GetAwaiter().GetResult();
            service.DeleteOrderDetail(10).GetAwaiter().GetResult();

            int orderCount = service.GetActiveOrderCount("testuser587");
            Assert.Equal(0, orderCount);
        }
    }
}
