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

        Task<bool> CompleteOrderAsync(int orderId, DeliveryType deliveryType, int? shippingAddressId, string officeAddres, decimal shippingPrice);
    }
}
