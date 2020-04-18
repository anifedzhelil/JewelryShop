namespace JewelryShop.Web.ViewModels.OrderComplete
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public string DeliveryMethod { get; set; }

        public int? ShippingAddressId { get; set; }

        public string SpeedyOfficeAddress { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
