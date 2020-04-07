namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels.Administration.Jewelry;

    public class JewelryService : IJewelryService
    {
        private readonly IDeletableEntityRepository<Jewel> jewelryRepository;

        public JewelryService(IDeletableEntityRepository<Jewel> jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }

        public async Task<int> AddAsync(CreateJewelViewModel createJewelModel)
        {
            var jewel = new Jewel
            {
                Name = createJewelModel.Name,
                Price = createJewelModel.Price,
                Description = createJewelModel.Description,
                Category = createJewelModel.Category,
                SalePrice = createJewelModel.SalePrice,
                SaleDate = createJewelModel.SaleDate,
                Count = createJewelModel.Count,
                IsArchived = createJewelModel.IsArchived,
            };

            await this.jewelryRepository.AddAsync(jewel);
            await this.jewelryRepository.SaveChangesAsync();
            return jewel.Id;
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

        public async Task DeleteByIdAsync(int id)
        {
            var jewel = this.jewelryRepository.All().FirstOrDefault(d => d.Id == id);
            if (jewel != null)
            {
                jewel.IsDeleted = true;
                this.jewelryRepository.Update(jewel);
                await this.jewelryRepository.SaveChangesAsync();
            }
        }
    }
}
