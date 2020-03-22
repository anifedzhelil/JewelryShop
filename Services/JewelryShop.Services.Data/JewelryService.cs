namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;

    public class JewelryService : IJewelryService
    {
        private readonly IDeletableEntityRepository<Jewel> jewelryRepository;

        public JewelryService(IDeletableEntityRepository<Jewel> jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Jewel> query = this.jewelryRepository.All()
                .OrderBy(c => c.Name);
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToArray();
        }
    }
}
