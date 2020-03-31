namespace JewelryShop.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels;
    using JewelryShop.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IJewelryService jewelryService;

        public HomeController(IJewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }

        [HttpGet]
        public IActionResult Index(int? category)
        {
            IndexViewModel viewModel = new IndexViewModel()
            {
                Jewelry = this.jewelryService.GetAllActivedByCategories<IndexJewelryViewModel>(category),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Details(int id)
        {
            return this.RedirectToAction("Index", "ProductDetails", new { id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }

}
