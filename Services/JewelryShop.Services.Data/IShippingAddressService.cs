using System.Collections.Generic;
using System.Text;

namespace JewelryShop.Services.Data
{
    public interface IShippingAddressService
    {
        IEnumerable<T> GetAllUsersShippingAddress<T>(string userId);
    }
}
