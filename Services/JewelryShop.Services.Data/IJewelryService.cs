namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IJewelryService
    {
        IEnumerable<T> GetAll<T>(int? count = null);

        IEnumerable<T> GetAllActived<T>(int? count = null);

        IEnumerable<T> GetAllActivedByCategories<T>(int? category, int? count = null);

        T GetById<T>(int id);
    }
}
