namespace JewelryShop.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.Controllers;
    using JewelryShop.Web.ViewModels.Administration.Jewelry;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class JewelryController : BaseController
    {
        private readonly Cloudinary cloudinary;
        private readonly IJewelryService jewelryService;

        public JewelryController(Cloudinary cloudinary, IJewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
            this.cloudinary = cloudinary;
        }

        public IActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel()
            {
                Jewelry = this.jewelryService.GetAll<IndexJewelryViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
                return this.View();
        }
    }
}
