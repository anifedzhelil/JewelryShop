namespace JewelryShop.Web.Controllers
{
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.UserOrders;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

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
    }
}
