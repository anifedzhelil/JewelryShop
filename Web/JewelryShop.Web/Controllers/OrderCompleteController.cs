namespace JewelryShop.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Data;
    using JewelryShop.Web.ViewModels.OrderComplete;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class OrderCompleteController : Controller
    {
        private readonly IOrdersService orderService;
        private readonly IShippingAddressService shippingAddressService;

        public OrderCompleteController(
            IOrdersService orderService,
            IShippingAddressService shippingAddressService)
        {
            this.orderService = orderService;
            this.shippingAddressService = shippingAddressService;
        }

        [HttpPost]

        public async Task<bool> CompleteAsync(OrderViewModel model)
        {
            DeliveryType delivery = model.DeliveryMethod == "Econt" ? DeliveryType.Econt : DeliveryType.Speedy;
            var addressId = -1;

            if (model.ShippingAddressId == null)
            {
                addressId = await this.shippingAddressService.AddOfficeAddressAsync(model.FirstName, model.LastName, model.Phone, model.SpeedyOfficeAddress);
            }
            else
            {
                addressId = (int)model.ShippingAddressId;
            }

            var result = await this.orderService.CompleteOrderAsync(model.OrderId, delivery, addressId, model.ShippingPrice);

            return result;
        }
    }
}
