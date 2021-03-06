﻿namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
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

            await this.AddOrderDetailAsync(order.Id, jewelId, quantity);
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

            await this.AddOrderDetailAsync(order.Id, jewelId, quantity);
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
                    this.orderRepository.Update(guestUserOrder);
                    await this.orderRepository.SaveChangesAsync();
                }
                else
                {
                    IEnumerable<OrderDetails> guestOrderDetails = this.orderDetailsRepository.All()
                        .Where(x => x.OrderId == guestUserOrder.Id);

                    foreach (var item in guestOrderDetails)
                    {
                        var currentOrderDetail = this.orderDetailsRepository.All()
                            .Where(x => x.OrderId == userOrder.Id && x.JewelId == item.JewelId)
                            .FirstOrDefault();

                        if (currentOrderDetail != null)
                        {
                            currentOrderDetail.Quantity = this.GetExistOrderDetailsQuantity(item.JewelId, currentOrderDetail.Quantity, item.Quantity);
                            this.orderDetailsRepository.Update(currentOrderDetail);
                            this.orderDetailsRepository.Delete(item);
                        }
                        else
                        {
                            item.OrderId = userOrder.Id;
                            this.orderDetailsRepository.Update(item);
                        }
                    }

                    await this.orderDetailsRepository.SaveChangesAsync();
                }
            }
        }

        public T GetActiveGuestOrder<T>(string guestId)
        {
            return this.orderRepository.All()
               .Where(x => x.GuestId == guestId && x.Status == OrderStatusType.Created)
               .To<T>()
               .FirstOrDefault();
        }

        public int GetActiveGuestOrderCount(string guestId)
        {
            var order = this.orderRepository.All()
              .Where(x => x.GuestId == guestId && x.Status == OrderStatusType.Created)
              .FirstOrDefault();
            if (order == null)
            {
                return 0;
            }

            return this.orderDetailsRepository.All()
                .Where(x => x.OrderId == order.Id)
                .Sum(x => x.Quantity);
        }

        public int GetActiveOrderCount(string userId)
        {
            var order = this.orderRepository.All()
              .Where(x => x.UserID == userId && x.Status == OrderStatusType.Created)
              .FirstOrDefault();

            if (order == null)
            {
                return 0;
            }

            return this.orderDetailsRepository.All()
                .Where(x => x.OrderId == order.Id)
                .Sum(x => x.Quantity);
        }

        public T GetActiveOrder<T>(string userId)
        {
            return this.orderRepository.All()
                 .Where(x => x.UserID == userId && x.Status == OrderStatusType.Created)
                 .To<T>()
                 .FirstOrDefault();
        }

        public async Task<StockType> CheckJewelQuantityAsync(int orderId)
        {
            var orderDetails = this.orderDetailsRepository.All()
                .Where(x => x.OrderId == orderId)
                .ToArray();

            StockType result = StockType.Available;

            foreach (var item in orderDetails)
            {
                var jewel = this.jewelRepository.All()
                    .Where(x => x.Id == item.JewelId)
                    .FirstOrDefault();
                if (jewel != null)
                {
                     if (jewel.Count == 0)
                    {
                        this.orderDetailsRepository.Delete(item);
                        await this.orderDetailsRepository.SaveChangesAsync();
                        result = StockType.LowAvailability;
                    }
                    else if (item.Quantity > jewel.Count)
                    {
                        item.Quantity = jewel.Count;
                        this.orderDetailsRepository.Update(item);
                        await this.orderDetailsRepository.SaveChangesAsync();
                        result = StockType.LowAvailability;
                    }
                }
            }

            orderDetails = this.orderDetailsRepository.All()
              .Where(x => x.OrderId == orderId)
              .ToArray();
            if (orderDetails.Length == 0)
            {
                var order = this.orderRepository.All()
                    .Where(x => x.Id == orderId)
                    .FirstOrDefault();
                this.orderRepository.Delete(order);
                await this.orderRepository.SaveChangesAsync();
                result = StockType.OutOfStock;
            }

            return result;
        }

        public async Task DeleteOrderDetail(int orderDetailsId)
        {
            var orderDetail = this.orderDetailsRepository.All()
                 .Where(x => x.Id == orderDetailsId)
                 .FirstOrDefault();

            if (orderDetail != null)
            {
                this.orderDetailsRepository.Delete(orderDetail);

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
                        this.orderRepository.Delete(order);
                        await this.orderRepository.SaveChangesAsync();
                    }
                }

                await this.orderDetailsRepository.SaveChangesAsync();
            }
        }

        public int GetActiveOrderUserId(string userId)
        {
            var order = this.orderRepository.All()
                .Where(x => x.UserID == userId  && x.Status == OrderStatusType.Created)
                .FirstOrDefault();

            if (order != null)
            {
                return order.Id;
            }
            else
            {
                return -1;
            }
        }

        public int GetActiveOrderGuestId(string userId)
        {
            var order = this.orderRepository.All()
                .Where(x => x.GuestId == userId  && x.Status==OrderStatusType.Created)
                .FirstOrDefault();

            if (order != null)
            {
                return order.Id;
            }
            else
            {
                return -1;
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

        public async Task<bool> CompleteOrderAsync(int orderId, DeliveryType deliveryType, int shippingAddressId, decimal shippingPrice)
        {
            var order = this.orderRepository.All()
                .Where(x => x.Id == orderId)
                .FirstOrDefault();

            if (order == null)
            {
                return false;
            }

            order.Delivery = deliveryType;
            order.Status = OrderStatusType.Completed;
            order.CompleteDate = DateTime.UtcNow;
            order.ShippingAddressId = shippingAddressId;
            order.ShippingPrice = shippingPrice;

            this.orderRepository.Update(order);
            var value = await this.orderRepository.SaveChangesAsync();

            var orderDetails = this.orderDetailsRepository.All()
                .Where(x => x.OrderId == orderId)
                .ToList();

            foreach (var item in orderDetails)
            {
                var jewel = this.jewelRepository.All()
                      .Where(x => x.Id == item.JewelId)
                      .FirstOrDefault();

                jewel.Count = jewel.Count - item.Quantity;
                this.jewelRepository.Update(jewel);
                await this.jewelRepository.SaveChangesAsync();

                if (jewel.SaleDate > DateTime.UtcNow)
                {
                    item.Price = (decimal)jewel.SalePrice;
                }
                else
                {
                    item.Price = jewel.Price;
                }

                this.orderDetailsRepository.Update(item);
                await this.orderDetailsRepository.SaveChangesAsync();
            }

            return true;
        }

        public ICollection<T> GetUserAllCompletedOrders<T>(string userId)
        {
            return this.orderRepository.All()
                .Where(x => x.UserID == userId && (x.Status == OrderStatusType.Completed || x.Status == OrderStatusType.Shipped))
                .To<T>()
                .ToArray();
        }

        public ICollection<T> GetAllCompletedOrders<T>(int? take = null, int skip = 0)
        {
            IQueryable<Order> query = this.orderRepository.All()
                .Where(x => x.Status == OrderStatusType.Completed || x.Status == OrderStatusType.Shipped)
                .OrderBy(x => x.Status)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take((int)take);
            }

            return query.To<T>()
             .ToArray();
        }

        public T GetOrderById<T>(int orderId)
        {
           return this.orderRepository.All()
                .Where(x => x.Id == orderId)
                .To<T>()
                .FirstOrDefault();
        }

        public int GetAllOrdersCount()
        {
            return this.orderRepository.All()
                .Where(x => x.Status == OrderStatusType.Completed || x.Status == OrderStatusType.Shipped)
                .Count();
        }

        public async Task ChangeStatusAsync(int id)
        {
            var order = this.orderRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            order.Status = OrderStatusType.Shipped;
            order.ShippedDate = DateTime.UtcNow;
            this.orderRepository.Update(order);
            await this.orderRepository.SaveChangesAsync();
        }

        public bool ChechShippingAddressIsUsed(int id)
        {
            var orders = this.orderRepository.All()
                .Where(x => x.ShippingAddressId == id)
                .ToList();

            return orders.Count != 0;
        }

        private int GetNewOrderDetailsQuantity(int jewelId, int quantity)
        {
            var jewel = this.jewelRepository.All()
                   .Where(x => x.Id == jewelId)
                   .FirstOrDefault();

            if (jewel == null)
            {
                return 0;
            }
            else
            {
                if (jewel.Count >= quantity)
                {
                    return quantity;
                }
                else
                {
                    return jewel.Count;
                }
            }
        }

        private int GetExistOrderDetailsQuantity(int jewelId, int oldQuantity, int newQuantity)
        {
            var jewel = this.jewelRepository.All()
                   .Where(x => x.Id == jewelId)
                   .FirstOrDefault();

            if (jewel == null)
            {
                return 0;
            }
            else
            {
                if (jewel.Count >= (newQuantity + oldQuantity))
                {
                    return newQuantity + oldQuantity;
                }
                else
                {
                    return jewel.Count;
                }
            }
        }

        private async Task AddOrderDetailAsync(int orderId, int jewelId, int quantity)
        {
            var orderDetails = this.orderDetailsRepository.All()
                .Where(x => x.OrderId == orderId && x.JewelId == jewelId)
                .FirstOrDefault();

            if (orderDetails == null)
            {
                orderDetails = new OrderDetails()
                {
                    OrderId = orderId,
                    JewelId = jewelId,
                    Quantity = this.GetNewOrderDetailsQuantity(jewelId, quantity),
                };
                await this.orderDetailsRepository.AddAsync(orderDetails);
            }
            else
            {
                orderDetails.Quantity = this.GetExistOrderDetailsQuantity(jewelId, orderDetails.Quantity, quantity);
                this.orderDetailsRepository.Update(orderDetails);
            }

            await this.orderDetailsRepository.SaveChangesAsync();
        }
    }
}
