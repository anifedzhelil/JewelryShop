namespace JewelryShop.Web.Controllers
{
    using System;
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
        private const int ItemsPerPage = 2;

        private readonly IJewelryService jewelryService;

        public HomeController(IJewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }

        [HttpGet]
        public IActionResult Index(CategoryType? category, int page = 1)
        {
            var count = this.jewelryService.GetCount(category);

            IndexViewModel viewModel = new IndexViewModel()
            {
                Jewelry = this.jewelryService.GetAllActivedByCategories<IndexJewelryViewModel>(category, ItemsPerPage, (page - 1) * ItemsPerPage),
            };

            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            if (category.HasValue)
            { 
                viewModel.Category = (CategoryType)category;
            }
            viewModel.CurrentPage = page;

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
