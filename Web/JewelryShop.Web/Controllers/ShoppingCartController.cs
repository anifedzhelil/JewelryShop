namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCartController : BaseController
    {
        private const string GuestId = "guest_id";
        private readonly IOrdersService ordersService;
        private readonly UserManager<ApplicationUser> userManager;

        public ShoppingCartController(IOrdersService ordersService, UserManager<ApplicationUser> userManager)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
           /* {
                OrdersDetails = this.ordersService.GetActiveGuestOrder<OrderDetailsIndexViewModel>(this.Request.Cookies[GuestId].ToString()),
            };*/

        
        var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                if (this.Request.Cookies[GuestId] != null)
                {
                    model = this.ordersService.GetActiveGuestOrder<IndexViewModel>(this.Request.Cookies[GuestId].ToString());
                }
            }
            else
            {
                model = this.ordersService.GetActiveOrder<IndexViewModel>(user.Id);
            }

            if (model.OrdersDetails != null)
            {
                return this.View(model);
            }
            else
            {
                return this.View("Empty");
            }
        }
    }
}