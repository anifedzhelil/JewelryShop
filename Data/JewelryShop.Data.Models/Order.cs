namespace JewelryShop.Data.Models
{
    using System;
    using System.Collections.Generic;

    using JewelryShop.Data.Common.Models;
    using JewelryShop.Data.Models.Enums;

    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetails>();
        }

        public string? UserID { get; set; }

        public OrderStatusType Status { get; set; }

        public DateTime? CompleteDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public string? GuestId { get; set; }

        public DeliveryType? Delivery { get; set; }

        public decimal? ShippingPrice { get; set; }

        public int? ShippingAddressId { get; set; }

        public virtual ShippingAddress ShippingAddress { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
