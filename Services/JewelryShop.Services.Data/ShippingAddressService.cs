namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class ShippingAddressService : IShippingAddressService
    {
        private readonly IDeletableEntityRepository<ShippingAddress> shippingAddressRepository;

        public ShippingAddressService(IDeletableEntityRepository<ShippingAddress> shippingAddressRepository)
        {
            this.shippingAddressRepository = shippingAddressRepository;
        }

        public IEnumerable<T> GetAllUsersShippingAddress<T>(string userId)
        {
            return this.shippingAddressRepository.All()
                 .Where(x => x.UserID == userId)
                 .To<T>()
                 .ToArray();
        }
    }
}
