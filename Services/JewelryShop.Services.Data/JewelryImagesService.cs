namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;

    public class JewelryImagesService : IJewelryImagesService
    {
        private readonly IRepository<JewelImages> jewelryImagesRepository;

        public JewelryImagesService(IRepository<JewelImages> jewelryImagesRepository)
        {
            this.jewelryImagesRepository = jewelryImagesRepository;
        }

        public async Task<bool> AddAsync(int jewelId, List<string> listUrls)
        {
            foreach (string url in listUrls)
            {
                var jewelImages = new JewelImages
                {
                    JewelId = jewelId,
                    ImageUrl = url,
                };

                await this.jewelryImagesRepository.AddAsync(jewelImages);
                await this.jewelryImagesRepository.SaveChangesAsync();
            }

            return true;
        }
    }
}
