namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.Components;
    using JewelryShop.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private const string GuestId = "guest_id";
        private readonly IOrdersService ordersService;
        private readonly UserManager<ApplicationUser> userManager;

        public OrdersController(IOrdersService ordersService, UserManager<ApplicationUser> userManager)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        [HttpPost]

        public async Task<IActionResult> Create(int id, int quantity)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(30);
                option.IsEssential = true;

                if (this.Request.Cookies[GuestId] == null)
                {
                    var guest_id = Guid.NewGuid();
                    this.Response.Cookies.Append(GuestId, guest_id.ToString(), option);
                    await this.ordersService.AddGuestProductAsync(guest_id.ToString(), id, quantity);
                }
                else
                {
                    await this.ordersService.AddGuestProductAsync(this.Request.Cookies[GuestId], id, quantity);
                }
            }
            else
            {
                 await this.ordersService.AddProductAsync(user.Id, id, quantity);
            }

            return this.RedirectToAction("Index", "ProductDetails", new { id = id });
        }
    }
}
