namespace JewelryShop.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using JewelryShop.Web.ViewModels.ShippingAddresses;

    public interface IShippingAddressService
    {
        Task AddAsync(InputShippingAddressModel model);

        Task UpdateAsync(InputShippingAddressModel model);

        IEnumerable<T> GetAllUsersShippingAddress<T>(string userId);

        T GetShippingAddressById<T>(int shippingAddressId);

        Task DeleteShippingAddress(int shippingAddressId);
    }
}
