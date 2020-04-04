namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.ShippingAddresses;
    using JewelryShop.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;

    public class ShoppingCartController : BaseController
    {
        private const string GuestId = "guest_id";
        private readonly IOrdersService ordersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IShippingAddressService shippingAddressService;

        public ShoppingCartController(
            IOrdersService ordersService,
            UserManager<ApplicationUser> userManager,
            IShippingAddressService shippingAddressService)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
            this.shippingAddressService = shippingAddressService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
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

        [Authorize]
        public async Task<IActionResult> LoadShippingAddress()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var model = new UserShippingAddressViewModel()
            {
                ShippingAddress = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id),
            };

            return this.View(model);
        }
    }
}
