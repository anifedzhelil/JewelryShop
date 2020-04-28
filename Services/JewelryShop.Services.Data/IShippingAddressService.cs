namespace JewelryShop.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using JewelryShop.Web.ViewModels.ShippingAddresses;

    public interface IShippingAddressService
    {
        Task AddAddressAsync(InputShippingAddressModel model);

        Task<int> AddOfficeAddressAsync(string firstName, string lastName, string phone, string officeAddres);

        Task UpdateAsync(InputShippingAddressModel model);

        ICollection<T> GetUserAllShippingAddress<T>(string userId);

        T GetShippingAddressById<T>(int shippingAddressId);

        Task DeleteShippingAddress(int shippingAddressId);
    }
}
