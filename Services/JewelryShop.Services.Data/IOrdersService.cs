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

    }
}
