namespace JewelryShop.Web.Controllers
{
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.UserOrders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class UserOrdersController : Controller
    {
        private readonly IOrdersService orderService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserOrdersController(
            IOrdersService orderService,
            UserManager<ApplicationUser> userManager)
        {
            this.orderService = orderService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            IndexViewModel model = new IndexViewModel()
            {
                Orders = this.orderService.GetUserAllCompletedOrders<IndexItemViewModel>(user.Id),
            };

            if (model.Orders.Count == 0)
            {
                return this.View("Empty");
            }

            return this.View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            OrderDetailsViewModel model = this.orderService.GetOrderById<OrderDetailsViewModel>(id);
            if (user.Id == model.UserID)
            {
                return this.View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}
