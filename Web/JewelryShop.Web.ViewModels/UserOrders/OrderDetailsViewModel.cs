namespace JewelryShop.Web.ViewModels.UserOrders
{
    using System;
    using System.Collections.Generic;

    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Mapping;

    public class OrderDetailsViewModel : IMapFrom<Order>
    {
        public string UserID { get; set; }

        public DateTime CompleteDate { get; set; }

        public DateTime ShippingDate { get; set; }

        public OrderStatusType Status { get; set; }

        public DeliveryType Delivery { get; set; }

        public decimal ShippingPrice { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public ICollection<OrderDetailsItemViewModel> OrdersDetails { get; set; }
    }
}
