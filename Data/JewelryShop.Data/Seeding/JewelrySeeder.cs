namespace JewelryShop.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;

    internal class JewelrySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Jewelry.Any())
            {
                return;
            }

            Jewel jewelFirst = new Jewel
            {
                Name = "Колие02",
                Description = "Златно колие",
                Price = 25,
                Count = 2,
                Category = Models.Enums.CategoryType.Necklace,
                IsArchived = false,
            };

            Jewel jewelSecond = new Jewel
            {
                Name = "Гривна 036",
                Description = "Гривна шарена",
                Price = 35,
                Count = 3,
                Category = Models.Enums.CategoryType.Bracelet,
                IsArchived = false,
            };

            await dbContext.AddAsync(jewelFirst);
            await dbContext.AddAsync(jewelSecond);
       }
    }
}
