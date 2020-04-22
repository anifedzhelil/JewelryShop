namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Data;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.ViewModels;
    using JewelryShop.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class HomeController : BaseController
    {
        private const int ItemsPerPage = 3;

        private readonly IJewelryService jewelryService;

        public HomeController(IJewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }

        [HttpGet]
        public IActionResult Index(CategoryType? category, SortType? sort, string search, int page = 1)
        {
            var count = this.jewelryService.GetCount(category);

            IQueryable<IndexJewelryViewModel> query = this.jewelryService.GetAllActivedByCategories(category, search, ItemsPerPage, (page - 1) * ItemsPerPage).To<IndexJewelryViewModel>();

            switch (sort)
            {
                case SortType.BestSelling:
                    query = query.OrderByDescending(j => j.SoldCount);
                    break;

                case SortType.Rating:
                    query = query.OrderByDescending(j => j.Ratings);
                    break;
                case SortType.HighPrice:
                    query = query.OrderByDescending(j => j.Price);
                    break;
                case SortType.LowPrice:
                    query = query.OrderBy(j => j.Price);
                    break;
                default:
                    query = query.OrderByDescending(j => j.CreatedOn);
                    break;
            }

            IndexViewModel viewModel = new IndexViewModel();
            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            if (page > viewModel.PagesCount)
            {
                page = viewModel.PagesCount;
            }

            query = query.Skip((page - 1) * ItemsPerPage);

            viewModel.Jewelry = query.Take(ItemsPerPage);

            if (sort.HasValue)
            {
                viewModel.Sort = (SortType)sort;
            }

            if (category.HasValue)
            {
                viewModel.Category = (CategoryType)category;
            }

            viewModel.CurrentPage = page;
            viewModel.Search = search;
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
