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
            var stockType = StockType.Available;

            if (user == null)
            {
                string guestId = this.Request.Cookies[GlobalConstants.GuestId];

                if (guestId != null)
                {
                    var guestOrderId = this.ordersService.GetActiveOrderGuestId(guestId);

                    if (guestOrderId > 0)
                    {
                        stockType = await this.ordersService.CheckJewelQuantityAsync(guestOrderId);

                        if (stockType != StockType.OutOfStock)
                        {
                            model = this.ordersService.GetActiveGuestOrder<IndexViewModel>(this.Request.Cookies[GlobalConstants.GuestId].ToString());
                        }
                    }
                }
            }
            else
            {
                var orderId = this.ordersService.GetActiveOrderUserId(user.Id);
                if (orderId > 0)
                {
                    stockType = await this.ordersService.CheckJewelQuantityAsync(orderId);

                    if (stockType != StockType.OutOfStock)
                    {
                        model = this.ordersService.GetActiveOrder<IndexViewModel>(user.Id);
                    }
                }
            }

            if (model != null && model.OrdersDetails != null)
            {
                if (stockType == StockType.LowAvailability)
                {
                    this.TempData["StockMessage"] = "Поради недостатъчна наличност броят на продуктите в кошницата е намален.";
                }

                return this.View(model);
            }
            else if (stockType == StockType.OutOfStock)
            {
                this.TempData["StockMessage"] = "Продуктите, които сте добавили в кошницата са изчерпани.";
                return this.View("Empty");
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
