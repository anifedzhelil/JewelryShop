namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models.Enums;

    public interface IOrdersService
    {
        Task AddGuestProductAsync(string guestId, int jewelId, int quantity);

        Task AddProductAsync(string userId, int jewelId, int quantity);

        Task UpdateUserOrderAsync(string userId, string guestId);

        T GetActiveOrder<T>(string userId);

        T GetActiveGuestOrder<T>(string guestId);

        Task DeleteOrderDetail(int orderDetailId);

        Task UpdateOrderDetailQuantityAsync(int orderDetailId, int quantity);

        int GetActiveOrderCount(string userId);

        int GetActiveGuestOrderCount(string guestId);

        Task<bool> CompleteOrderAsync(int orderId, DeliveryType deliveryType, int shippingAddressId, decimal shippingPrice);

        ICollection<T> GetUserAllCompletedOrders<T>(string userId);

        ICollection<T> GetAllCompletedOrders<T>(int? take = null, int skip = 0);

        T GetOrderById<T>(int orderId);

        int GetAllOrdersCount();

        Task ChangeStatusAsync(int id);
    }
}
