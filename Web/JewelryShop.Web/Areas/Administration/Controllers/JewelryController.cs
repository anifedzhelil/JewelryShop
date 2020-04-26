namespace JewelryShop.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Data;
    using JewelryShop.Services.Mapping;
    using JewelryShop.Web.CloudinaryHelper;
    using JewelryShop.Web.Controllers;
    using JewelryShop.Web.ViewModels.Administration.Jewelry;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class JewelryController : AdministrationController
    {
        private const int ItemsPerPage = 6;

        private readonly Cloudinary cloudinary;
        private readonly IJewelryService jewelryService;
        private readonly IJewelryImagesService jewelryImagesService;

        public JewelryController(Cloudinary cloudinary, IJewelryService jewelryService, IJewelryImagesService jewelryImagesService)
        {
            this.jewelryService = jewelryService;
            this.cloudinary = cloudinary;
            this.jewelryImagesService = jewelryImagesService;
        }

        public IActionResult Index(FilterType? filter, SortType? sort, int page = 1)
        {
            var count = 0;
            if (filter.HasValue)
            {
                count = this.jewelryService.GetAdminJewelryCount((FilterType)filter);
            }
            else
            {
                count = this.jewelryService.GetAdminJewelryCount(FilterType.All);
            }

            IQueryable<IndexJewelryViewModel> query = this.jewelryService.GetAll().To<IndexJewelryViewModel>();

            switch (filter)
            {
                case FilterType.Archived:
                    query = query.Where(j => j.IsArchived == true);
                    break;

                case FilterType.OutOfStock:
                    query = query.Where(j => j.Count == 0);
                    break;
                case FilterType.Stock:
                    query = query.Where(j => j.Count > 0);
                    break;
            }

            switch (sort)
            {
                case SortType.BestSelling:
                    query = query.OrderByDescending(j => j.SoldCount);
                    break;

                case SortType.Rating:
                    query = query.OrderByDescending(j => j.Rating);
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

            if (page > viewModel.PagesCount)
            {
                page = viewModel.PagesCount;
            }

            query = query.Skip((page - 1) * ItemsPerPage);
            query = query.Take(ItemsPerPage);

            viewModel.Jewelry = query;

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            if (filter.HasValue)
            {
                viewModel.Filter = (FilterType)filter;
            }

            if (sort.HasValue)
            {
                viewModel.Sort = (SortType)sort;
            }

            viewModel.CurrentPage = page;
            return this.View(viewModel);
        }

        public IActionResult Create()
         {
                 return this.View();
         }

        public IActionResult Details(int id)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            DetailsViewModel viewModel = this.jewelryService.GetById<DetailsViewModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateJewelViewModel createJewel, ICollection<IFormFile> imagesFiles)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(createJewel);
            }
            else
            {
                int jewelId = await this.jewelryService.AddAsync(createJewel);
                if (jewelId > 0)
                {
                    var listUrls = await CloudinaryExtention.UploadAsync(this.cloudinary, imagesFiles);
                    await this.jewelryImagesService.AddAsync(jewelId, listUrls);
                }

                this.TempData["InfoMessage"] = "Продуктът успешно е добавен.";
                return this.RedirectToAction("Index");
            }
        }

        public IActionResult Edit(int id)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            EditJewelViewModel editViewModel = this.jewelryService.GetById<EditJewelViewModel>(id);

            return this.View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditJewelViewModel jewel, ICollection<IFormFile> imagesFiles)
        {
            if (!this.ModelState.IsValid)
            {
                jewel.Images = this.jewelryImagesService.GetJewelImages(jewel.Id);
                return this.View(jewel);
            }
            else
            {
                var listUrls = await CloudinaryExtention.UploadAsync(this.cloudinary, imagesFiles);
                await this.jewelryImagesService.AddAsync(jewel.Id, listUrls);
                await this.jewelryService.Update(jewel);
            }

            this.TempData["InfoMessage"] = "Продуктът успешно е редактиран.";
            return this.RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> DeleteAsync(int id)
        {
            var result = await this.jewelryService.DeleteByIdAsync(id);
            if (result)
            {
                this.TempData["InfoMessage"] = "Продуктът е изтрит.";
            }
            else
            {
                this.TempData["DeleteMessage"] = "Продуктът не може да бъде изтрит! Този продукт е поръчан.";
            }

            return this.RedirectToAction("Index");
        }
    }
}
