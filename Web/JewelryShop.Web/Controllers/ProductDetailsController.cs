namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.ProductDetails;
    using Microsoft.AspNetCore.Mvc;

    public class ProductDetailsController : BaseController
    {
        private readonly IJewelryService jewelryService;

        public ProductDetailsController(IJewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }

        public IActionResult Index(int id)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            IndexViewModel viewModel = this.jewelryService.GetById<IndexViewModel>(id);

            return this.View(viewModel);
        }
    }
}
