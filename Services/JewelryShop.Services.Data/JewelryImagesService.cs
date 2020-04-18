namespace JewelryShop.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public void DeleteImage(int jewelId, string imageUrl)
        {
            var image = this.jewelryImagesRepository.All()
                .Where(x => x.JewelId == jewelId && x.ImageUrl == imageUrl)
                .FirstOrDefault();

            if (image != null)
            {
                this.jewelryImagesRepository.Delete(image);
                this.jewelryImagesRepository.SaveChangesAsync();
            }
        }

        public IEnumerable<string> GetJewelImages(int jewelId)
        {
            return this.jewelryImagesRepository.All()
               .Where(x => x.JewelId == jewelId)
               .Select(x => x.ImageUrl)
               .ToList();
        }
    }
}
