namespace JewelryShop.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;

    internal class JewelryImagesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Jewelry.Any())
            {
                return;
            }

            JewelImages imageFirst = new JewelImages()
            {
                ImageUrl = "https://res.cloudinary.com/drhqukmht/image/upload/v1588064198/seedimages/40542892_609510846113274_202105982033603282_n_bq15pr.jpg",
                JewelId = dbContext.Jewelry.Where(x => x.Name == "Колие02").FirstOrDefault().Id,
            };

            JewelImages imageSecond = new JewelImages()
            {
                ImageUrl = "https://res.cloudinary.com/drhqukmht/image/upload/v1588064448/seedimages/84319015_501075100819804_4884354215187970325_n_4_r0zgcg.jpg",
                JewelId = dbContext.Jewelry.Where(x => x.Name == "Гривна 036").FirstOrDefault().Id,
            };

            await dbContext.JewelryImages.AddAsync(imageFirst);
            await dbContext.JewelryImages.AddAsync(imageSecond);
        }
    }
}
