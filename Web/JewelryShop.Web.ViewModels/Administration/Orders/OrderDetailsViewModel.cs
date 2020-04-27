namespace JewelryShop.Web.ViewModels.Administration.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using JewelryShop.Data.Models;
    using JewelryShop.Data.Models.Enums;
    using JewelryShop.Services.Mapping;

    public class OrderDetailsViewModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public string UserID { get; set; }

        public DateTime CompleteDate { get; set; }

        public string CompleteDateOnAsString =>
       this.CompleteDate.Hour == 0 && this.CompleteDate.Minute == 0
           ? this.CompleteDate.ToString("ddd, dd MMM yyyy", new CultureInfo("bg-BG"))
           : this.CompleteDate.ToString("ddd, dd MMM yyyy HH:mm", new CultureInfo("bg-BG"));

        public DateTime ShippingDate { get; set; }

        public OrderStatusType Status { get; set; }

        public DeliveryType Delivery { get; set; }

        public decimal ShippingPrice { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public ICollection<OrderDetailsItemViewModel> OrdersDetails { get; set; }
    }
}
