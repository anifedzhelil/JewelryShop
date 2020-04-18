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

        public OrderCompleteController(IOrdersService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]

        public async Task<bool> CompleteAsync(OrderViewModel model)
        {
            DeliveryType delivery = model.DeliveryMethod == "Econt" ? DeliveryType.Econt : DeliveryType.Speedy;
            var result = await this.orderService.CompleteOrderAsync(model.OrderId, delivery, model.ShippingAddressId, model.SpeedyOfficeAddress, model.ShippingPrice);

            return true;
        }
    }
}
