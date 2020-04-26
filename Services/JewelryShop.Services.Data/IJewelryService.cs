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

        public IQueryable<Jewel> GetAllActivedByCategories(int? category);

        T GetById<T>(int id);

        Task<int> AddAsync(CreateJewelViewModel createJewelModel);

        Task Update(EditJewelViewModel createJewelModel);

        Task<bool> DeleteByIdAsync(int id);

        int GetAdminJewelryCount(FilterType filter);
    }
}
