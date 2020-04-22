namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels.Administration.Jewelry;

    public class JewelryService : IJewelryService
    {
        private readonly IDeletableEntityRepository<Jewel> jewelryRepository;
        private readonly IDeletableEntityRepository<OrderDetails> orderDetailsRepository;

        public JewelryService(
            IDeletableEntityRepository<Jewel> jewelryRepository,
            IDeletableEntityRepository<OrderDetails> orderDetailsRepository)
        {
            this.jewelryRepository = jewelryRepository;
            this.orderDetailsRepository = orderDetailsRepository;
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

        public async Task Update(EditJewelViewModel jewelModel)
        {
            var jewel = this.jewelryRepository.All()
                  .Where(x => x.Id == jewelModel.Id)
                  .FirstOrDefault();

            if (jewel != null)
            {
                jewel.Name = jewelModel.Name;
                jewel.Price = jewelModel.Price;
                jewel.Description = jewelModel.Description;
                jewel.Category = jewelModel.Category;
                jewel.SalePrice = jewelModel.SalePrice;
                jewel.SaleDate = jewelModel.SaleDate;
                jewel.Count = jewelModel.Count;
                jewel.IsArchived = jewelModel.IsArchived;

                this.jewelryRepository.Update(jewel);
                await this.jewelryRepository.SaveChangesAsync();
            }
        }

        public IQueryable<Jewel> GetAll()
        {
            return this.jewelryRepository.All()
                .OrderBy(c => c.Name);
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

        public IEnumerable<T> GetAllActivedByCategories<T>(CategoryType? category, string search, int? take = null, int skip = 0)
        {
            IQueryable<Jewel> query = this.jewelryRepository.All();

            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search) || x.Description.Contains(search));
            }

            if (category > 0)
            {
                query = query
                .OrderBy(c => c.CreatedOn)
                .Where(x => x.IsArchived == false && x.Category == category && x.Count > 0).Skip(skip);
            }
            else
            {
                query = query
                .OrderBy(c => c.CreatedOn)
                .Where(x => x.IsArchived == false && x.Count > 0).Skip(skip);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToArray();
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
                this.jewelryRepository.Delete(jewel);
                await this.jewelryRepository.SaveChangesAsync();
            }
        }

        public int GetCount(CategoryType? category)
        {
            if (category > 0)
            {
                return this.jewelryRepository.All().Count(x => x.IsArchived == false && x.Category == category && x.Count > 0);
            }

            return this.jewelryRepository.All().Count(x => x.IsArchived == false && x.Count > 0);
        }

        public int GetAdminJewelryCount(FilterType filter)
        {
            switch (filter)
            {
                case FilterType.Archived:
                    return this.jewelryRepository.All().Where(x => x.IsArchived == true).Count();
                case FilterType.OutOfStock:
                    return this.jewelryRepository.All().Where(x => x.Count == 0).Count();
                case FilterType.Stock:
                    return this.jewelryRepository.All().Where(x => x.Count > 0).Count();
            }

            return this.jewelryRepository.All().Count();
        }
    }
}
