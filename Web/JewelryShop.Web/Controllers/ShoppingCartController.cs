namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Common;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.ShoppingCart;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;

    public class ShoppingCartController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly UserManager<ApplicationUser> userManager;

        public ShoppingCartController(
            IOrdersService ordersService,
            UserManager<ApplicationUser> userManager)
        {
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
            var user = await this.userManager.GetUserAsync(this.User);

            List<KeyValuePair<string, int>> listJewelryMessage = new List<KeyValuePair<string, int>>();
            if (user == null)
            {
                if (this.Request.Cookies[GlobalConstants.GuestId] != null)
                {
                    model = this.ordersService.GetActiveGuestOrder<IndexViewModel>(this.Request.Cookies[GlobalConstants.GuestId].ToString());
                }
            }
            else
            {
                model = this.ordersService.GetActiveOrder<IndexViewModel>(user.Id);
            }

            if (model != null && model.OrdersDetails != null)
            {
                var result = await this.ordersService.CheckJewelQuantityAsync(model.Id);
                if (result == StockType.LowAvailability)
                {
                    this.TempData["StockMessage"] = "Поради недостатъчна наличност броят на продуктите в кошницата е намален.";
                }
                else if (result == StockType.OutOfStock)
                {
                    this.TempData["StockMessage"] = "Продуктите, които сте добавили в кошницата са изчерпани.";
                    return this.View("Empty");
                }

                return this.View(model);
            }
            else
            {
                return this.View("Empty");
            }
        }

        public async Task<IActionResult> EditQuantity(int id, int quantity)
        {
            if (quantity == 0)
            {
                await this.ordersService.DeleteOrderDetail(id);
            }
            else
            {
                await this.ordersService.UpdateOrderDetailQuantityAsync(id, quantity);
            }

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            await this.ordersService.DeleteOrderDetail(id);

            return this.RedirectToAction("Index");
        }
    }
}
