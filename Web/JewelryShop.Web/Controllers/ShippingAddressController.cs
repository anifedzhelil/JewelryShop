namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.ShippingAddresses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [EnableCors]
    public class ShippingAddressController : Controller
    {
        private readonly IShippingAddressService shippingAddressService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrdersService ordersService;

        public ShippingAddressController(
            UserManager<ApplicationUser> userManager,
            IShippingAddressService shippingAddressService,
            IOrdersService ordersService)
        {
            this.shippingAddressService = shippingAddressService;
            this.userManager = userManager;
            this.ordersService = ordersService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var model = new IndexViewModel();
            model.ShippingAddressesCollection = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id);
            model.DeliveryCheked = "uncheked";
            model.ShippingAddress = new InputShippingAddressModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                UserID = user.Id,
            };

            var orderModel = this.ordersService.GetActiveOrder<OrderViewModel>(user.Id);
            model.OrdersDetails = orderModel.OrdersDetails;
            model.OrderId = orderModel.OrdersDetails.FirstOrDefault().OrderId;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShippingAddress(InputShippingAddressModel model, int? id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                var shippingAddressesModel = new IndexViewModel();
                shippingAddressesModel.ShippingAddressesCollection = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id);
                shippingAddressesModel.ShippingAddress = model;
                this.TempData["ShippingAddressIsValid"] = false;
                return this.View("Index", shippingAddressesModel);
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
                    await this.shippingAddressService.AddAddressAsync(model);
                }
            }

            return this.RedirectToAction("Index");
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

            var shippingAddressesModel = new IndexViewModel();
            shippingAddressesModel.ShippingAddressesCollection = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id);
            shippingAddressesModel.ShippingAddress = model;

            return this.View("Index", shippingAddressesModel);
        }

        public async Task<IActionResult> DeleteShippingAddressAsync(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.shippingAddressService.DeleteShippingAddress(id);
            var shippingAddressesModel = new IndexViewModel();
            shippingAddressesModel.ShippingAddressesCollection = this.shippingAddressService.GetAllUsersShippingAddress<ShippingAddressViewModel>(user.Id);
            shippingAddressesModel.ShippingAddress = new InputShippingAddressModel();
            return this.View("Index", shippingAddressesModel);
        }

        public IActionResult CompleteSuccess()
        {
            return this.View();
        }
    }
}
