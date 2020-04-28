namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels.ShippingAddresses;

    public class ShippingAddressService : IShippingAddressService
    {
        private readonly IDeletableEntityRepository<ShippingAddress> shippingAddressRepository;

        public ShippingAddressService(IDeletableEntityRepository<ShippingAddress> shippingAddressRepository)
        {
            this.shippingAddressRepository = shippingAddressRepository;
        }

        public async Task AddAddressAsync(InputShippingAddressModel model)
        {
            var shippingAddress = new ShippingAddress()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                City = model.City,
                Address = model.Address,
                AdditionalAddress = model.AdditionalAddress,
                PostCode = model.PostCode,
                UserID = model.UserID,
            };

            await this.shippingAddressRepository.AddAsync(shippingAddress);
            await this.shippingAddressRepository.SaveChangesAsync();
        }

        public async Task<int> AddOfficeAddressAsync(string firstName, string lastName, string phone, string officeAddres)
        {
            var shippingAddress = new ShippingAddress()
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                OfficeAddres = officeAddres,
            };

            await this.shippingAddressRepository.AddAsync(shippingAddress);
            await this.shippingAddressRepository.SaveChangesAsync();

            return shippingAddress.Id;
        }

        public async Task UpdateAsync(InputShippingAddressModel model)
        {
            var shippingAddress = this.shippingAddressRepository.All()
                   .Where(x => x.Id == model.Id)
                    .FirstOrDefault();

            if (shippingAddress != null)
            {
                shippingAddress.FirstName = model.FirstName;
                shippingAddress.LastName = model.LastName;
                shippingAddress.Phone = model.Phone;
                shippingAddress.City = model.City;
                shippingAddress.Address = model.Address;
                shippingAddress.AdditionalAddress = model.AdditionalAddress;
                shippingAddress.PostCode = model.PostCode;

                this.shippingAddressRepository.Update(shippingAddress);
                await this.shippingAddressRepository.SaveChangesAsync();
            }
        }

        public ICollection<T> GetUserAllShippingAddress<T>(string userId)
        {
            return this.shippingAddressRepository.All()
                 .Where(x => x.UserID == userId)
                 .To<T>()
                 .ToArray();
        }

        public T GetShippingAddressById<T>(int shippingAddressId)
        {
            return this.shippingAddressRepository.All()
                .Where(x => x.Id == shippingAddressId)
                 .To<T>()
                 .FirstOrDefault();
        }

        public async Task DeleteShippingAddress(int shippingAddressId)
        {
            var shippingAddress = this.shippingAddressRepository.All()
                  .Where(x => x.Id == shippingAddressId)
                   .FirstOrDefault();

            if (shippingAddress != null)
            {
                this.shippingAddressRepository.Delete(shippingAddress);
                await this.shippingAddressRepository.SaveChangesAsync();
            }
        }
    }
}
