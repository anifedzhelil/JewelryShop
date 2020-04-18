namespace JewelryShop.Web.Components
{
    using System.Threading.Tasks;

    using JewelryShop.Common;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCartComponent : ViewComponent
    {
        private readonly IOrdersService ordersService;
        private readonly UserManager<ApplicationUser> userManager;

        public ShoppingCartComponent(IOrdersService ordersService, UserManager<ApplicationUser> userManager)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(this.Request.HttpContext.User);

            if (user != null)
            {
                var count = this.ordersService.GetActiveOrderCount(user.Id);
                return this.View(count);
            }
            else
            {
                var guestUserId = this.Request.Cookies[GlobalConstants.GuestId];

                if (guestUserId == null)
                {
                    return this.View(0);
                }

                var count = this.ordersService.GetActiveGuestOrderCount(guestUserId);
                return this.View(count);
            }
        }
    }
}
