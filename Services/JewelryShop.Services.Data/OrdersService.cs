namespace JewelryShop.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;
    using Microsoft.AspNetCore.Identity;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> orderRepository;
        private readonly IDeletableEntityRepository<OrderDetails> orderDetailsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        
        public OrdersService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<OrderDetails> orderDetailsRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailsRepository = orderDetailsRepository;
            this.userRepository = userRepository;
        }

        public async Task AddGuestProductAsync(string guestId, int jewelId, int quantity)
        {
            var order = this.orderRepository.All()
                .FirstOrDefault(x => x.GuestId == guestId);

            if (order == null)
            {
                order = new Order()
                {
                    GuestId = guestId,
                    Status = OrderStatusType.Created,
                };
                await this.orderRepository.AddAsync(order);
                await this.orderRepository.SaveChangesAsync();
            }

            var orderDetails = new OrderDetails()
            {
                OrderId = order.Id,
                JewelId = jewelId,
                Quantity = quantity,
            };

            await this.orderDetailsRepository.AddAsync(orderDetails);
            await this.orderDetailsRepository.SaveChangesAsync();
        }

        public async Task AddProductAsync(string userId, int jewelId, int quantity)
        {
            var order = this.orderRepository.All()
                .FirstOrDefault(x => x.UserID == userId && x.Status == OrderStatusType.Created);

            if (order == null)
            {
                order = new Order()
                {
                    UserID = userId,
                    Status = OrderStatusType.Created,
                };
                await this.orderRepository.AddAsync(order);
                await this.orderRepository.SaveChangesAsync();
            }

            var orderDetails = new OrderDetails()
            {
                OrderId = order.Id,
                JewelId = jewelId,
                Quantity = quantity,
            };

            await this.orderDetailsRepository.AddAsync(orderDetails);
            await this.orderDetailsRepository.SaveChangesAsync();
        }

        public void UpdateUserOrder(string userEmail, string guestId)
        {
            var user = this.userRepository.All()
                .Where(c => c.Email == userEmail)
                .FirstOrDefault();

            var guestUserOrder = this.orderRepository.All()
                            .Where(x => x.GuestId == guestId)
                            .FirstOrDefault();

            if (guestUserOrder != null)
            {
                var userOrder = this.orderRepository.All()
                    .Where(x => x.UserID == user.Id && x.Status == OrderStatusType.Created)
                    .FirstOrDefault();
                if (userOrder == null)
                {
                    guestUserOrder.UserID = user.Id;
                }
                else
                {
                    IEnumerable<OrderDetails> orderDetails = this.orderDetailsRepository.All()
                        .Where(x => x.OrderId == guestUserOrder.Id);

                    foreach (var item in orderDetails)
                    {
                        item.OrderId = userOrder.Id;
                        this.orderDetailsRepository.Update(item);
                    }
                }

                this.orderRepository.SaveChangesAsync();
                this.orderDetailsRepository.SaveChangesAsync();
            }
        }

        public T GetActiveGuestOrder<T>(string guestId)
        {
            return this.orderRepository.All()
               .Where(x => x.GuestId == guestId && x.Status == OrderStatusType.Created)
               .To<T>()
               .FirstOrDefault();
        }

        public T GetActiveOrder<T>(string userId)
        {
            return this.orderRepository.All()
                 .Where(x => x.UserID == userId && x.Status == OrderStatusType.Created)
                 .To<T>()
                 .FirstOrDefault();
        }
    }
}
