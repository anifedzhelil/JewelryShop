namespace JewelryShop.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using JewelryShop.Data.Common.Repositories;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels;
    using JewelryShop.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IDeletableEntityRepository<Jewel> jewelryRepository;

        public HomeController(IDeletableEntityRepository<Jewel> jewelryRepository)
        {
            this.jewelryRepository = jewelryRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IQueryable<Jewel> query = this.jewelryRepository.All();

            IndexViewModel viewModel = new IndexViewModel()
            {
                Jewelry = query.To<IndexJewelryViewModel>().ToArray(),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
