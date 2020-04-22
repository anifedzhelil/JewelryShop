namespace JewelryShop.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Web.ViewModels.Administration.Jewelry;

    public interface IJewelryService
    {
        IQueryable<Jewel> GetAll();

        IEnumerable<T> GetAllActived<T>(int? count = null);

        public IQueryable<Jewel> GetAllActivedByCategories(CategoryType? category, string search, int? take = null, int skip = 0);

        T GetById<T>(int id);

        Task<int> AddAsync(CreateJewelViewModel createJewelModel);

        Task Update(EditJewelViewModel createJewelModel);

        Task DeleteByIdAsync(int id);

        int GetCount(CategoryType? category);

        int GetAdminJewelryCount(FilterType filter);
    }
}
