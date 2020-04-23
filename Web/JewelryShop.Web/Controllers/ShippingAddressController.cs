namespace JewelryShop.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JewelryShop.Common;
    using JewelryShop.Data.Models;
    using JewelryShop.Services.Data;
    using JewelryShop.Services.Messaging;
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
        private readonly IEmailSender emailSender;

        public ShippingAddressController(
            UserManager<ApplicationUser> userManager,
            IShippingAddressService shippingAddressService,
            IOrdersService ordersService,
            IEmailSender emailSender)
        {
            this.shippingAddressService = shippingAddressService;
            this.userManager = userManager;
            this.ordersService = ordersService;
            this.emailSender = emailSender;
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

        public async Task<IActionResult> CompleteSuccess()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var name = user.FirstName != null && user.LastName != null ? user.FirstName + " " + user.LastName : user.Email;

            await this.emailSender.SendEmailAsync(GlobalConstants.SystemEmail, GlobalConstants.SystemNickname, user.Email, "Нова поръчка", "Здравейте " + name + "! Поръчката ви бе приета успешно! При въпроси, моля пишете ни. Благодарим ви, че пазарувате от нас!");
            return this.View();
        }
    }
}
