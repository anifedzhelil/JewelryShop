namespace JewelryShop.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.Administration.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator")]
    [Area("Administration")]
    public class OrdersController : AdministrationController
    {
        private const int ItemsPerPage = 5;

        private readonly IOrdersService orderService;

        public OrdersController(IOrdersService orderService)
        {
            this.orderService = orderService;
        }

        public IActionResult Index(int page = 1)
        {
            IndexViewModel model = new IndexViewModel()
            {
                Orders = this.orderService.GetAllCompletedOrders<IndexItemViewModel>(ItemsPerPage, (page - 1) * ItemsPerPage),
            };

            if (model.Orders == null)
            {
                return this.NotFound();
            }

            var count = this.orderService.GetAllOrdersCount();
            model.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);
            if (model.PagesCount == 0)
            {
                model.PagesCount = 1;
            }

            model.CurrentPage = page;

            return this.View(model);
        }

        public IActionResult Details(int id)
        {
            OrderDetailsViewModel model = this.orderService.GetOrderById<OrderDetailsViewModel>(id);
            if (model == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            await this.orderService.ChangeStatusAsync(id);
            return this.RedirectToAction("Details", new { id = id });
        }
    }
}
