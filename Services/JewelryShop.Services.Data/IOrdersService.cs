namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IOrdersService
    {
        Task AddGuestProductAsync(string guestId, int jewelId, int quantity);

        Task AddProductAsync(string userId, int jewelId, int quantity);

        Task UpdateUserOrderAsync(string userId, string guestId);

        T GetActiveOrder<T>(string userId);

        T GetActiveGuestOrder<T>(string guestId);

        Task DeleteOrderDetail(int orderDetailId);

        Task UpdateOrderDetailQuantityAsync(int orderDetailId, int quantity);
    }
}
