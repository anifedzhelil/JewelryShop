namespace JewelryShop.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> orderRepository;
        private readonly IDeletableEntityRepository<OrderDetails> orderDetailsRepository;

        public OrdersService(
            IDeletableEntityRepository<Order> orderRepository,
            IDeletableEntityRepository<OrderDetails> orderDetailsRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailsRepository = orderDetailsRepository;
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
    }
}
