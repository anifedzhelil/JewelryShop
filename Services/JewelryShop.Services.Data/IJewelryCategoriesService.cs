namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IJewelryCategoriesService
    {
        Dictionary<int, string> GetJewelryCategories();
    }
}
