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
        private readonly IDeletableEntityRepository<Jewel> jewelRepository;

        public OrdersService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<OrderDetails> orderDetailsRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<Jewel> jewelRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailsRepository = orderDetailsRepository;
            this.userRepository = userRepository;
            this.jewelRepository = jewelRepository;
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

            var orderDetails = this.orderDetailsRepository.All()
                .Where(x => x.OrderId == order.Id && x.JewelId == jewelId)
                .FirstOrDefault();

            if (orderDetails == null)
            {
                orderDetails = new OrderDetails()
                {
                    OrderId = order.Id,
                    JewelId = jewelId,
                    Quantity = quantity,
                };
                await this.orderDetailsRepository.AddAsync(orderDetails);
            }
            else
            {
                var jewel = this.jewelRepository.All()
                    .Where(x => x.Id == jewelId)
                    .FirstOrDefault();

                if(jewel.Count >= (quantity + orderDetails.Quantity))
                {
                    orderDetails.Quantity += quantity;
                }
                else
                {
                    orderDetails.Quantity = jewel.Count;
                }
                this.orderDetailsRepository.Update(orderDetails);
            }

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

        public async Task UpdateUserOrderAsync(string userEmail, string guestId)
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

                await this.orderRepository.SaveChangesAsync();
                await this.orderDetailsRepository.SaveChangesAsync();
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

        public async Task DeleteOrderDetail(int orderDetailsId)
        {
            var orderDetail = this.orderDetailsRepository.All()
                 .Where(x => x.Id == orderDetailsId)
                 .FirstOrDefault();

            if (orderDetail != null)
            {
                orderDetail.IsDeleted = true;
                this.orderDetailsRepository.Update(orderDetail);

                var allOrderDetails = this.orderDetailsRepository.All()
                 .Where(x => x.OrderId == orderDetail.OrderId && x.Id != orderDetail.Id)
                 .FirstOrDefault();

                if (allOrderDetails == null)
                {
                    var order = this.orderRepository.All()
                        .Where(x => x.Id == orderDetail.OrderId)
                        .FirstOrDefault();

                    if (order != null)
                    {
                        order.IsDeleted = true;
                        this.orderRepository.Update(order);
                        await this.orderRepository.SaveChangesAsync();
                    }
                }
                await this.orderDetailsRepository.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderDetailQuantityAsync(int orderDetailId, int quantity)
        {
            var orderDetail = this.orderDetailsRepository.All()
                 .Where(x => x.Id == orderDetailId)
                 .FirstOrDefault();

            if (orderDetail != null)
            {
                orderDetail.Quantity = quantity;
                this.orderDetailsRepository.Update(orderDetail);
                await this.orderDetailsRepository.SaveChangesAsync();
            }
        }
    }
}
