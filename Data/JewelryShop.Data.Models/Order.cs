namespace JewelryShop.Data.Models
{
    using System;
    using System.Collections.Generic;

    using JewelryShop.Data.Common.Models;

    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.OrdersDetails = new HashSet<OrderDetails>();
        }

        public string? UserID { get; set; }

        public OrderStatusType Status { get; set; }

        public DateTime? OrderDate { get; set; }

        public virtual ICollection<OrderDetails> OrdersDetails { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
