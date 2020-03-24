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

        public IEnumerable<T> GetAllActived<T>(int? count = null)
        {
            IQueryable<Jewel> query = this.jewelryRepository.All()
             .OrderBy(c => c.CreatedOn)
             .Where(x => x.IsArchived == false);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToArray();
        }

        public IEnumerable<T> GetAllActivedByCategories<T>(int? category, int? count = null)
        {
            if (category.HasValue)
            {
                IQueryable<Jewel> query = this.jewelryRepository.All()
                .OrderBy(c => c.CreatedOn)
                .Where(x => x.IsArchived == false && x.Category == category);

                if (count.HasValue)
                {
                    query = query.Take(count.Value);
                }

                return query.To<T>().ToArray();
            }
            else
            {
                IQueryable<Jewel> query = this.jewelryRepository.All()
                   .OrderBy(c => c.CreatedOn)
                   .Where(x => x.IsArchived == false);

                if (count.HasValue)
                {
                    query = query.Take(count.Value);
                }

                return query.To<T>().ToArray();
            }
        }

        public T GetById<T>(int id)
        {
            return this.jewelryRepository.All()
                .Where(c => c.Id == id)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
