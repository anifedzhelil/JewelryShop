namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IJewelryImagesService
    {
        Task<bool> AddAsync(int jewelId, List<string> urls);
    }
}
