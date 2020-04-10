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

            if (model != null && model.OrdersDetails != null)
            {
                return this.View(model);
            }
            else
            {
                return this.View("Empty");
            }
        }

        [Authorize]
        public async Task<IActionResult> ShippingAddress()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var model = new UserShippingAddressViewModel();
            model.ShippingAddressesCollection = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id);
            model.ShippingAddress = new InputShippingAddressModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                UserID = user.Id,
            };

            return this.View(model);
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

        [HttpPost]
        public async Task<IActionResult> CreateShippingAddress(InputShippingAddressModel model, int? id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                var shippingAddressesModel = new UserShippingAddressViewModel();
                shippingAddressesModel.ShippingAddressesCollection = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id);
                shippingAddressesModel.ShippingAddress = model;
                this.TempData["ShippingAddressIsValid"] = false;
                return this.View("ShippingAddress", shippingAddressesModel);
            }
            else
            {
                if (id > 0)
                {
                    await this.shippingAddressService.UpdateAsync(model);
                }
                else
                {
                    model.UserID = user.Id;
                    await this.shippingAddressService.AddAsync(model);
                }
            }

            return this.RedirectToAction("ShippingAddress");
        }

        public async Task<IActionResult> EditShippingAddressAsync(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var model = this.shippingAddressService.GetShippingAddressById<InputShippingAddressModel>(id);
            if (model == null)
            {
                return this.NotFound();
            }

            this.TempData["ShippingAddressIsValid"] = false;

            var shippingAddressesModel = new UserShippingAddressViewModel();
            shippingAddressesModel.ShippingAddressesCollection = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id);
            shippingAddressesModel.ShippingAddress = model;

            return this.View("ShippingAddress", shippingAddressesModel);
        }

        public async Task<IActionResult> DeleteShippingAddressAsync(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.shippingAddressService.DeleteShippingAddress(id);
            var shippingAddressesModel = new UserShippingAddressViewModel();
            shippingAddressesModel.ShippingAddressesCollection = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id);
            shippingAddressesModel.ShippingAddress = new InputShippingAddressModel();
            return this.View("ShippingAddress", shippingAddressesModel);
        }
    }
}
